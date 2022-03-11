using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Collections;
using Microsoft.Win32.SafeHandles;

namespace Opulos.Core.IO
{
    /// <summary>
    /// https://sourceforge.net/projects/fastfileinfo/
    /// A faster way to get file information than System.IO.FileInfo.
    /// </summary>
    /// <remarks>
    /// This enumerator is substantially faster than using <see cref="Directory.GetFiles(string)"/>
    /// and then creating a new FileInfo object for each path.  Use this version when you 
    /// will need to look at the attibutes of each file returned (for example, you need
    /// to check each file in a directory to see if it was modified after a specific date).
    /// </remarks>
    [Serializable]
    public class FastFileInfo
    {

        public readonly FileAttributes Attributes;

        public DateTime CreationTime
        {
            get { return this.CreationTimeUtc.ToLocalTime(); }
        }

        /// <summary>
        /// File creation time in UTC
        /// </summary>
        public readonly DateTime CreationTimeUtc;

        /// <summary>
        /// Gets the last access time in local time.
        /// </summary>
        public DateTime LastAccesTime
        {
            get { return this.LastAccessTimeUtc.ToLocalTime(); }
        }

        /// <summary>
        /// File last access time in UTC
        /// </summary>
        public readonly DateTime LastAccessTimeUtc;

        /// <summary>
        /// Gets the last access time in local time.
        /// </summary>
        public DateTime LastWriteTime
        {
            get { return this.LastWriteTimeUtc.ToLocalTime(); }
        }

        /// <summary>
        /// File last write time in UTC
        /// </summary>
        public readonly DateTime LastWriteTimeUtc;

        /// <summary>
        /// Size of the file in bytes
        /// </summary>
        public readonly long Length;

        /// <summary>
        /// Name of the file
        /// </summary>
        public readonly String Name;

        /// <summary>
        /// Shortened version of Name that has the tidle character
        /// </summary>
        public readonly String AlternateName;

        /// <summary>
        /// Full path to the file.
        /// </summary>
        public readonly String FullName;

        public String DirectoryName
        {
            get { return System.IO.Path.GetDirectoryName(FullName); }
        }

        public bool Exists
        {
            get { return File.Exists(FullName); }
        }

        public override String ToString()
        {
            return this.Name;
        }

        public FastFileInfo(String filename) : this(new FileInfo(filename))
        {
        }

        public FastFileInfo(FileInfo file)
        {
            this.Name = file.Name;
            this.FullName = file.FullName;
            if (file.Exists)
            {
                this.Length = file.Length;
                this.Attributes = file.Attributes;
                this.CreationTimeUtc = file.CreationTimeUtc;
                this.LastAccessTimeUtc = file.LastAccessTimeUtc;
                this.LastWriteTimeUtc = file.LastWriteTimeUtc;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="FastFileInfo"/> class.</summary>
        /// <param name="dir">The directory that the file is stored at</param>
        /// <param name="findData">WIN32_FIND_DATA structure that this object wraps.</param>
        internal FastFileInfo(String dir, WIN32_FIND_DATA findData)
        {
            this.Attributes = findData.dwFileAttributes;
            this.CreationTimeUtc = ConvertDateTimeUtc(findData.ftCreationTime_dwHighDateTime,
                findData.ftCreationTime_dwLowDateTime);
            this.LastAccessTimeUtc = ConvertDateTimeUtc(findData.ftLastAccessTime_dwHighDateTime,
                findData.ftLastAccessTime_dwLowDateTime);
            this.LastWriteTimeUtc = ConvertDateTimeUtc(findData.ftLastWriteTime_dwHighDateTime,
                findData.ftLastWriteTime_dwLowDateTime);
            this.Length = CombineHighLowInts(findData.nFileSizeHigh, findData.nFileSizeLow);
            this.Name = findData.cFileName;
            this.AlternateName = findData.cAlternateFileName;
            this.FullName = System.IO.Path.Combine(dir, findData.cFileName);
        }

        //---------------------------------
        // static methods:

        /// <summary>
        /// Gets <see cref="FastFileInfo"/> for all the files in a directory.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <returns>An object that implements <see cref="IEnumerable{FileData}"/> and 
        /// allows you to enumerate the files in the given directory.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is a null reference (Nothing in VB)
        /// </exception>
        public static IEnumerable<FastFileInfo> EnumerateFiles(String path)
        {
            return EnumerateFiles(path, "*");
        }

        /// <summary>
        /// Gets <see cref="FastFileInfo"/> for all the files in a directory that match a 
        /// specific filter.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <param name="searchPattern">The search string to match against files in the path.</param>
        /// <returns>An object that implements <see cref="IEnumerable{FileData}"/> and 
        /// allows you to enumerate the files in the given directory.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is a null reference (Nothing in VB)
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="filter"/> is a null reference (Nothing in VB)
        /// </exception>
        public static IEnumerable<FastFileInfo> EnumerateFiles(String path, String searchPattern)
        {
            return EnumerateFiles(path, searchPattern, SearchOption.TopDirectoryOnly, null);
        }

        /// <summary>
        /// Gets <see cref="FastFileInfo"/> for all the files in a directory that match a specific filter, optionally including all sub directories.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <param name="searchPattern">The search string to match against files in the path.</param>
        /// <param name="searchOption">
        /// One of the SearchOption values that specifies whether the search 
        /// operation should include all subdirectories or only the current directory.
        /// </param>
        /// <returns>An object that implements <see cref="IEnumerable{FileData}"/> and allows enumerating files in the specified directory.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is a null reference (Nothing in VB)
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="filter"/> is a null reference (Nothing in VB)
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="searchOption"/> is not one of the valid values of the
        /// <see cref="System.IO.SearchOption"/> enumeration.
        /// </exception>
        public static IEnumerable<FastFileInfo> EnumerateFiles(String path, String searchPattern,
            SearchOption searchOption, IFolderFilter folderFilter)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            if (searchPattern == null)
                throw new ArgumentNullException("searchPattern");

            if ((searchOption != SearchOption.TopDirectoryOnly) && (searchOption != SearchOption.AllDirectories))
                throw new ArgumentOutOfRangeException("searchOption");

            String fullPath = System.IO.Path.GetFullPath(path);

            return new FileEnumerable(fullPath, searchPattern, searchOption, folderFilter);
        }

        public static IList<FastFileInfo> GetFiles2(String path, String searchPattern = "*",
            bool searchSubfolders = false)
        {
            SearchOption searchOption =
                (searchSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            return GetFiles(path, searchPattern, searchOption);
        }

        /// <summary>
        /// Gets <see cref="FastFileInfo"/> for all the files in a directory that match a specific filter.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <param name="searchPattern">The search string to match against files in the path. Multiple can be specified separated by the pipe character.</param>
        /// <returns>An list of FastFileInfo objects that match the specified search pattern.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is a null reference (Nothing in VB)
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="filter"/> is a null reference (Nothing in VB)
        /// </exception>
        public static IList<FastFileInfo> GetFiles(String path, String searchPattern = "*",
            SearchOption searchOption = SearchOption.TopDirectoryOnly, IFolderFilter folderFilter = null)
        {
            List<FastFileInfo> list = new List<FastFileInfo>();
            String[] arr = searchPattern.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
            Hashtable
                ht = (arr.Length > 1
                    ? new Hashtable()
                    : null); // don't need to worry about case since it should be consistent
            foreach (String sp in arr)
            {
                String sp2 = sp.Trim();
                if (sp2.Length == 0)
                    continue;

                IEnumerable<FastFileInfo> e = EnumerateFiles(path, sp2, searchOption, folderFilter);
                if (ht == null)
                    list.AddRange(e);
                else
                {
                    using (var e2 = e.GetEnumerator())
                    {
                        if (ht.Count == 0)
                        {
                            while (e2.MoveNext())
                            {
                                FastFileInfo f = e2.Current;
                                list.Add(f);
                                ht[f.FullName] = f;
                            }
                        }
                        else
                        {
                            while (e2.MoveNext())
                            {
                                FastFileInfo f = e2.Current;
                                if (!ht.Contains(f.FullName))
                                {
                                    list.Add(f);
                                    ht[f.FullName] = f;
                                }
                            }
                        }
                    }
                }
            }

            return list;
        }

        private class FileEnumerable : IEnumerable<FastFileInfo>
        {
            private readonly String path;
            private readonly String filter;
            private readonly SearchOption searchOption;
            private readonly IFolderFilter folderFilter;

            public FileEnumerable(String path, String filter, SearchOption searchOption, IFolderFilter folderFilter)
            {
                this.path = path;
                this.filter = filter;
                this.searchOption = searchOption;
                this.folderFilter = folderFilter;
            }

            public IEnumerator<FastFileInfo> GetEnumerator()
            {
                return new FileEnumerator(path, filter, searchOption, folderFilter, true, false, false);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new FileEnumerator(path, filter, searchOption, folderFilter, true, false, false);
            }
        }

        // Wraps a FindFirstFile handle.
        private sealed class SafeFindHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            [DllImport("kernel32.dll")]
            private static extern bool FindClose(IntPtr handle);

            [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
            internal SafeFindHandle() : base(true)
            {
            }

            /// <summary>
            /// When overridden in a derived class, executes the code required to free the handle.
            /// </summary>
            /// <returns>
            /// true if the handle is released successfully; otherwise, in the 
            /// event of a catastrophic failure, false. In this case, it 
            /// generates a releaseHandleFailed MDA Managed Debugging Assistant.
            /// </returns>
            protected override bool ReleaseHandle()
            {
                return FindClose(base.handle);
            }
        }

        [System.Security.SuppressUnmanagedCodeSecurity]
        public class FileEnumerator : IEnumerator<FastFileInfo>
        {
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern SafeFindHandle FindFirstFile(String fileName, [In, Out] WIN32_FIND_DATA data);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern bool FindNextFile(SafeFindHandle hndFindFile,
                [In, Out, MarshalAs(UnmanagedType.LPStruct)]
                WIN32_FIND_DATA lpFindFileData);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern SafeFindHandle FindFirstFileEx(String fileName, int infoLevel,
                [In, Out] WIN32_FIND_DATA data, int searchScope, String notUsedNull, int additionalFlags);

            private String initialFolder;
            private SearchOption searchOption;
            private String searchFilter;

            private IFolderFilter folderFilter; // added 2020-09-26

            //---
            private String currentFolder;
            private SafeFindHandle hndFile;
            private WIN32_FIND_DATA findData;
            private int currentPathIndex;
            private IList<String> currentPaths;
            private IList<String> pendingFolders;
            private Queue<IList<String>> queue;
            private bool advanceNext;
            private bool usePendingFolders = false;
            private bool useGetDirectories = false;

            private bool hasCurrent = false;

            //---
            private bool useEx = false;
            private int infoLevel = 0;

            private int
                searchScope = 0; // always files (1 = limit to directories, 2 = limit to devices (not supported))

            private int additionalFlags = 0;

            public FileEnumerator(String initialFolder, String searchFilter, SearchOption searchOption,
                IFolderFilter folderFilter)
            {
                init(initialFolder, searchFilter, searchOption, folderFilter);
            }

            // basicInfoOnly is about 30% faster. E.g. the C:\Windows\ directory takes 4.3 sec for standard info, and 3.3 sec for basic info.
            // basicInfoOnly excludes getting the cAlternateName, which is the short name with the tidle character)
            //
            // Note: case sensitive only works if \HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\kernel\obcaseinsensitive is set to 0
            // which is probably not a good idea.
            public FileEnumerator(String initialFolder, String searchFilter, SearchOption searchOption,
                IFolderFilter folderFilter, bool basicInfoOnly, bool caseSensitive, bool largeBuffer)
            {
                init(initialFolder, searchFilter, searchOption, folderFilter);
                useEx = true;
                infoLevel = (basicInfoOnly
                        ? 1
                        : 0
                    ); // 0 is standard (includes the cAlternateName, which is the short name with the tidle character)
                additionalFlags |= (caseSensitive ? 1 : 0);
                additionalFlags |= (largeBuffer ? 2 : 0);
            }

            private void init(String initialFolder, String searchFilter, SearchOption searchOption,
                IFolderFilter folderFilter)
            {
                this.initialFolder = initialFolder;
                this.searchFilter = searchFilter;
                this.searchOption = searchOption;
                this.folderFilter = folderFilter;
                // usePendingFolders is 60% faster. E.g. the C:\Windows\ directory takes 7.7 seconds if using Directory.GetDirectories
                // but only takes 3.3 seconds if the folders are cached as they are encountered.
                this.usePendingFolders = (searchFilter == "*" || searchFilter == "*.*") &&
                                         searchOption == SearchOption.AllDirectories;
                // The problem is that when a filter like *.txt is used, none of the directories are returned by FindNextFile.
                this.useGetDirectories = !usePendingFolders && searchOption == SearchOption.AllDirectories;
                Reset();
            }

            public FastFileInfo Current
            {
                get { return new FastFileInfo(currentFolder, findData); }
            }

            public void Dispose()
            {
                if (hndFile != null)
                {
                    hndFile.Dispose();
                    hndFile = null;
                }
            }

            object System.Collections.IEnumerator.Current
            {
                get { return new FastFileInfo(currentFolder, findData); }
            }

            public bool MoveNext()
            {
                while (true)
                {
                    if (advanceNext)
                    {
                        hasCurrent = FindNextFile(hndFile, findData);
                    }

                    if (hasCurrent || !advanceNext)
                    {
                        // first skip over any directories, but store them if usePendingFolders is true
                        while (((FileAttributes) findData.dwFileAttributes & FileAttributes.Directory) ==
                               FileAttributes.Directory)
                        {
                            if (usePendingFolders)
                            {
                                String c = findData.cFileName;
                                if (!(c[0] == '.' && (c.Length == 1 || c[1] == '.' && c.Length == 2)))
                                {
                                    // skip folders '.' and '..'
                                    if (folderFilter == null ||
                                        folderFilter.SearchFolder(new FastFileInfo(currentFolder, findData)))
                                        pendingFolders.Add(Path.Combine(currentFolder, c));
                                }
                            }

                            hasCurrent = FindNextFile(hndFile, findData);
                            if (!hasCurrent)
                                break;
                        }
                    }

                    if (hasCurrent)
                    {
                        advanceNext = true;
                        return true;
                    }

                    if (useGetDirectories)
                    {
                        // even though the docs claim searchScope '1' only returns directories, it actually returns files and directories
                        var h = FindFirstFileEx(Path.Combine(currentFolder, "*"), 1, findData, 1, null, 0);
                        if (!h.IsInvalid)
                        {
                            while (true)
                            {
                                if (((FileAttributes) findData.dwFileAttributes & FileAttributes.Directory) ==
                                    FileAttributes.Directory)
                                {
                                    String c = findData.cFileName;
                                    if (!(c[0] == '.' && (c.Length == 1 || c[1] == '.' && c.Length == 2)))
                                    {
                                        // skip folders '.' and '..'
                                        if (folderFilter == null ||
                                            folderFilter.SearchFolder(new FastFileInfo(currentFolder, findData)))
                                            pendingFolders.Add(Path.Combine(currentFolder, c));
                                    }
                                }

                                if (!FindNextFile(h, findData))
                                    break;
                            }
                        }

                        h.Dispose();

                        // using this code is twice as slow. E.g. the C:\Windows\ folder took 7.4 sec versus 3.8 sec.
                        //try {
                        //	pendingFolders = Directory.GetDirectories(currentFolder);
                        //} catch {} // Access to the path '...\System Volume Information' is denied.
                    }

                    // at this point, the current folder is exhausted. If search subfolders then enqueue them.
                    if (pendingFolders.Count > 0)
                    {
                        queue.Enqueue(pendingFolders);
                        pendingFolders = new List<String>();
                    }

                    currentPathIndex++;
                    if (currentPathIndex == currentPaths.Count)
                    {
                        // at the end of the current paths
                        if (queue.Count == 0)
                        {
                            currentPathIndex--; // so that calling MoveNext() after very last has no impact
                            return false; // no more paths to process
                        }

                        currentPaths = queue.Dequeue();
                        currentPathIndex = 0;
                    }

                    String f = currentPaths[currentPathIndex];
                    InitFolder(f);
                }
            }

            private void InitFolder(String folder)
            {
                if (hndFile != null)
                    hndFile.Dispose();

                new FileIOPermission(FileIOPermissionAccess.PathDiscovery, folder).Demand();
                String searchPath = System.IO.Path.Combine(folder, searchFilter);
                if (useEx)
                    hndFile = FindFirstFileEx(searchPath, infoLevel, findData, searchScope, null, additionalFlags);
                else
                    hndFile = FindFirstFile(searchPath, findData);
                currentFolder = folder;
                advanceNext = false;
                hasCurrent =
                    !hndFile.IsInvalid; // e.g. unaccessible C:\System Volume Information or filter like *.txt in a directory with no text files
            }

            public void Reset()
            {
                currentPathIndex = 0;
                advanceNext = false;
                hasCurrent = false;
                currentPaths = new[] {initialFolder};
                findData = new WIN32_FIND_DATA();
                pendingFolders = new List<String>();
                queue = new Queue<IList<String>>();
                InitFolder(initialFolder);
            }
        }

        public static long CombineHighLowInts(uint high, uint low)
        {
            return (((long) high) << 0x20) | low;
        }

        public static DateTime ConvertDateTimeUtc(uint high, uint low)
        {
            long fileTime = CombineHighLowInts(high, low);
            return DateTime.FromFileTimeUtc(fileTime);
        }

        public static DateTime ConvertDateTime(uint high, uint low)
        {
            long fileTime = CombineHighLowInts(high, low);
            return DateTime.FromFileTime(fileTime);
        }
    }

    /// <summary>
    /// Contains information about the file that is found by the FindFirstFile or FindNextFile functions.
    /// </summary>
    [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto), BestFitMapping(false)]
    internal class WIN32_FIND_DATA
    {
        public FileAttributes dwFileAttributes;
        public uint ftCreationTime_dwLowDateTime;
        public uint ftCreationTime_dwHighDateTime;
        public uint ftLastAccessTime_dwLowDateTime;
        public uint ftLastAccessTime_dwHighDateTime;
        public uint ftLastWriteTime_dwLowDateTime;
        public uint ftLastWriteTime_dwHighDateTime;
        public uint nFileSizeHigh;
        public uint nFileSizeLow;
        public int dwReserved0;
        public int dwReserved1;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string cFileName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
        public string cAlternateFileName;

        public override String ToString()
        {
            return "File name=" + cFileName;
        }
    }

    public interface IFolderFilter
    {
        bool SearchFolder(FastFileInfo folder);
    }
}
