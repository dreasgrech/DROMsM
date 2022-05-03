#nullable enable
using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace U8Xml.Internal
{
    // [NOTE]
    // It is similar to Dictionary<RawString, RawString>.
    //
    // - It's only support to add and get items.
    // - Empty key is forbidden.
    // - Fixed capacity
    //
    //    RawStringTable
    // +-------------------+
    // |  RawStringTable_* |
    // |       Table       |   +-----------------------------------+---------------------------+---------------------------+--
    // +---------|---------+   |          RawStringTable_          |           Entry           |           Entry           |  ...
    //           |             +------------+------------+---------+-------------+-------------+-------------+-------------+--
    //           `------------>|   Entry*   |    int     |   int   |  RawString  |  RawString  |  RawString  |  RawString  |  
    //                         |   Entries  |  Capacity  |  Count  |     Key     |     Key     |     Key     |     Key     |  ...
    //                         +-----|------+------------+---------+-------------+-------------+-------------+-------------+--
    //                               |                                   ↑
    //                               `-----------------------------------'

    [DebuggerDisplay("{DebugDisplay,nq}")]
    internal unsafe readonly struct RawStringTable : IDisposable, IReference
    {
        private readonly IntPtr _table; // RawStringTable_*

        public bool IsNull => _table == IntPtr.Zero;

        private RawStringTable_* Table => (RawStringTable_*)_table;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebugDisplay => Table != null ? $"{nameof(RawStringTable)} (Count={Table->Count})" : "null";

        private RawStringTable(RawStringTable_* table)
        {
            _table = (IntPtr)table;
        }

        /// <summary>Add a pair of key and value. It returns false if the key already registered.</summary>
        /// <param name="key">key to add. (Must not be empty)</param>
        /// <param name="value">value to add</param>
        /// <returns>true if success</returns>
        public bool TryAdd(in RawString key, in RawString value)
        {
            Debug.Assert(_table != IntPtr.Zero);
            Debug.Assert(Table->Capacity > 0);

            var capacity = Table->Capacity;
            var count = Table->Count;
            if(count >= capacity) { ThrowHelper.ThrowInvalidOperation("Cannot add any more."); }
            if(key.IsEmpty) { ThrowHelper.ThrowArg("Cannot add empty key."); }

            var hash = GetKeyHash(key.GetPtr(), key.Length, capacity);
            var entries = Table->Entries;
            ref var entry = ref entries[hash];
            if(entry.Key.IsEmpty) {
                entry.Key = key;
                entry.Value = value;
                Table->Count++;
                return true;
            }
            if(entry.Key == key) {
                return false;       // already contains the key
            }

            return TryAddWithRehash(key, value, Table, hash);

            static bool TryAddWithRehash(in RawString key, in RawString value, RawStringTable_* table, int hash)
            {
                var capacity = table->Capacity;
                var entries = table->Entries;
                for(int i = 0; i < capacity; i++) {
                    hash = Rehash(hash, capacity);
                    ref var entry = ref entries[hash];
                    if(entry.Key.IsEmpty) {
                        entry.Key = key;
                        entry.Value = value;
                        table->Count++;
                        return true;
                    }
                    if(entry.Key == key) {
                        return false;   // already contains the key
                    }
                }
                throw new InvalidOperationException("Cannot add any more.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(in RawString key, out RawString value)
        {
            return TryGetValue(key.GetPtr(), key.Length, out value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(ReadOnlySpan<byte> key, out RawString value)
        {
            fixed(byte* ptr = key) {
                return TryGetValue(ptr, key.Length, out value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryGetValue(byte* keyPtr, int keyLength, out RawString value)
        {
            Debug.Assert(keyLength >= 0);
            if(keyLength == 0) {
                value = RawString.Empty;
                return false;
            }
            var capacity = Table->Capacity;
            var entries = Table->Entries;
            var hash = GetKeyHash(keyPtr, keyLength, capacity);

            ref var entry = ref entries[hash];
            var key = SpanHelper.CreateReadOnlySpan<byte>(keyPtr, keyLength);
            if(entry.Key.SequenceEqual(key)) {
                value = entry.Value;
                return true;
            }
            if(entry.Key.IsEmpty) {
                value = RawString.Empty;
                return false;
            }

            return TryGetWithRehash(key, out value, Table, hash);

            static bool TryGetWithRehash(ReadOnlySpan<byte> key, out RawString value, RawStringTable_* table, int hash)
            {
                var capacity = table->Capacity;
                var entries = table->Entries;
                for(int i = 0; i < capacity; i++) {
                    hash = Rehash(hash, capacity);
                    ref var entry = ref entries[hash];
                    if(entry.Key.SequenceEqual(key)) {
                        value = entry.Value;
                        return true;
                    }
                    if(entry.Key.IsEmpty) {
                        value = RawString.Empty;
                        return false;
                    }
                }
                value = RawString.Empty;
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetKeyHash(byte* keyPtr, int keyLength, int tableCapacity)
        {
            return (RawString.GetHashCode(keyPtr, keyLength) & 0x7FFFFFFF) % tableCapacity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Rehash(int baseHash, int tableCapacity)
        {
            var rehash = baseHash + 1;
            return rehash >= tableCapacity ? 0 : rehash;
        }


        public static RawStringTable Create(int count)
        {
            if(count == 0) { return default; }
            const int Ratio = 3;
            var entryCapacity = count * Ratio;
            var allocSize = sizeof(RawStringTable_) + entryCapacity * sizeof(Entry);
            var table = (RawStringTable_*)Marshal.AllocHGlobal(allocSize);
            AllocationSafety.Add(allocSize);

            table->Entries = (Entry*)(table + 1);
            table->Capacity = entryCapacity;
            table->Count = 0;
            SpanHelper.CreateSpan<Entry>(table->Entries, entryCapacity).Clear();
            return new RawStringTable(table);
        }

        public void Dispose()
        {
#if DEBUG
            if(_table != IntPtr.Zero) {
                var size = sizeof(RawStringTable_) + Table->Capacity * sizeof(Entry);
                AllocationSafety.Remove(size);
            }
#endif

            Marshal.FreeHGlobal(_table);
            Unsafe.AsRef(_table) = IntPtr.Zero;
        }

        private struct RawStringTable_
        {
            public Entry* Entries;
            public int Capacity;
            public int Count;
        }

        [DebuggerDisplay("Key={Key}, Value={Value}")]
        private struct Entry
        {
            public RawString Key;
            public RawString Value;
        }
    }
}
