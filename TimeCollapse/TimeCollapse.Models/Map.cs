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
            new Stage(new Point(64, 640), new Rectangle(64, 382, 32, 32)),
            new Stage(new Point(480, 640), new Rectangle(64, 382, 32, 32)),
            new Stage(new Point(944, 640), new Rectangle(64, 382, 32, 32))
        });

        private readonly List<Stage> stages;
        public Stage ActualStage { get; private set; }
        private IEnumerator<Stage> stagesSwitcher;


        private Map(IEnumerable<Rectangle> blocks, IEnumerable<Stage> stages)
        {
            Blocks = blocks.ToHashSet();
            this.stages = stages.ToList();
            stagesSwitcher = this.stages.GetEnumerator();
            if (stagesSwitcher.MoveNext())
                ActualStage = stagesSwitcher.Current;
        }

        public HashSet<Rectangle> Blocks { get; }

        public bool TrySwitchStage()
        {
            if (!stagesSwitcher.MoveNext())
                return false;

            ActualStage = stagesSwitcher.Current;
            return true;
        }

        public void ResetStages()
        {
            stagesSwitcher = stages.GetEnumerator();
            if (stagesSwitcher.MoveNext())
                ActualStage = stagesSwitcher.Current;
        }
    }
}