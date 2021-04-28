using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TimeCollapse.Models
{
    public class Map
    {
        public static readonly Map TestMap = new(new[]
        {
            new Rectangle(32, 720, 960, 16),
            new Rectangle(32, 544, 16, 176),
            new Rectangle(32, 528, 768, 16),
            new Rectangle(976, 528, 16, 192),
            new Rectangle(800, 624, 176, 16),
            new Rectangle(256, 672, 16, 48),
            new Rectangle(416, 656, 16, 64),
            new Rectangle(576, 640, 16, 80),
            new Rectangle(32, 352, 16, 176),
            new Rectangle(976, 352, 16, 176),
            new Rectangle(32, 336, 960, 16),
            new Rectangle(48, 432, 80, 16)
        }, new[]
        {
            (new Point(64, 672), new Point(64, 382)),
            (new Point(480, 672), new Point(64, 382)),
            (new Point(944, 672), new Point(64, 382))
        });

        private readonly List<(Point, Point, int)> stages;
        private (Point, Point, int) actualStage;


        public Map(IEnumerable<Rectangle> blocks, IEnumerable<(Point, Point)> spawnsAndTargets)
        {
            Blocks = blocks.ToHashSet();
            stages = spawnsAndTargets.Select((st, i) => (st.Item1, st.Item2, i)).ToList();
            actualStage = stages.First();
        }

        public HashSet<Rectangle> Blocks { get; }
        public Point ActualSpawn => actualStage.Item1;
        public Rectangle ActualTarget => new(actualStage.Item2, new Size(32, 32));

        public bool TrySwitchStage()
        {
            if (actualStage.Item3 + 1 == stages.Count)
                return false;

            actualStage = stages[actualStage.Item3 + 1];
            return true;
        }

        public void ResetStages()
        {
            actualStage = stages.First();
        }
    }
}