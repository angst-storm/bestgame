using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TimeCollapse.Models
{
    public class Map
    {
        public readonly HashSet<Rectangle> Blocks;

        public Map(IEnumerable<Rectangle> blocks)
        {
            Blocks = blocks.ToHashSet();
        }
    }
}