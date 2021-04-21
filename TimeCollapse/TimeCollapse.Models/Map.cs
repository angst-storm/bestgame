using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TimeCollapse.Models
{
    public class Map
    {
        public readonly HashSet<Rectangle> Blocks;
        public Point PlayerStartPosition { get; }

        public Map(IEnumerable<Rectangle> blocks, Point playerStartPosition)
        {
            Blocks = blocks.ToHashSet();
            PlayerStartPosition = playerStartPosition;
        }

        public static readonly Map TestMap = new Map(new[]
        {
            new Rectangle(100, 490, 600, 10),
            new Rectangle(100, 350, 10, 140),
            new Rectangle(690, 350, 10, 140),
            new Rectangle(300, 390, 200, 10)
        }, new Point(120, 470));
    }
}