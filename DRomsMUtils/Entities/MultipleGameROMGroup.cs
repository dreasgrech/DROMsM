using System.Linq;

namespace Frontend
{
    public class MultipleGameROMGroup : ROMGroup
    {
        protected override ROMGroup _Clone()
        {
            var romGroup = new MultipleGameROMGroup();
            romGroup.AddRange(Entries);
            return romGroup;
        }

        protected override void _Add(ROMEntry entry)
        {
        }
    }
}