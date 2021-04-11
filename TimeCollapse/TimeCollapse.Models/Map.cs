using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TimeCollapse.Models
{
    public class Map
    {
        public readonly HashSet<RectangleF> Blocks;

        public Map(IEnumerable<Rectangle> blocks)
        {
            Blocks = blocks.Select(r => new RectangleF(r.Location, r.Size)).ToHashSet();
        }
    }
}