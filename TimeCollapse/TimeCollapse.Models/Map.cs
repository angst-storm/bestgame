using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace TimeCollapse.Models
{
    public class Map
    {
        private static readonly Map TestMap = new("Тестовая карта", new[]
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
        }, Array.Empty<Rectangle>(), new[]
        {
            new Stage(new Point(64, 640), new Rectangle(64, 366, 48, 64)),
            new Stage(new Point(480, 640), new Rectangle(64, 366, 48, 64)),
            new Stage(new Point(944, 640), new Rectangle(64, 366, 48, 64))
        });

        private static readonly Map SpiralMap = new("Простая спираль", new[]
        {
            new Rectangle(416, 848, 1072, 16),
            new Rectangle(416, 752, 16, 96),
            new Rectangle(432, 752, 944, 16),
            new Rectangle(1440, 752, 32, 16),
            new Rectangle(1472, 656, 16, 192),
            new Rectangle(528, 656, 944, 16),
            new Rectangle(1280, 672, 16, 80),
            new Rectangle(416, 560, 16, 192),
            new Rectangle(432, 656, 32, 16),
            new Rectangle(432, 560, 192, 16),
            new Rectangle(608, 576, 16, 80)
        }, Array.Empty<Rectangle>(), new[]
        {
            new Stage(new Point(448, 784), new Rectangle(1312, 688, 48, 48)),
            new Stage(new Point(1232, 688), new Rectangle(544, 592, 48, 48))
        });

        public static readonly BindingList<Map> AllMaps = new();

        private readonly List<Stage> stages;
        private IEnumerator<Stage> stagesSwitcher;

        static Map()
        {
            foreach (var map in Plot)
                AllMaps.Add(map);
        }

        public Map(string name, IEnumerable<Rectangle> blocks, IEnumerable<Rectangle> timeAnomalies,
            IEnumerable<Stage> stages)
        {
            Name = name;
            Blocks = blocks.ToHashSet();
            TimeAnomalies = timeAnomalies.ToHashSet();
            this.stages = stages.ToList();
            stagesSwitcher = this.stages.GetEnumerator();
            if (stagesSwitcher.MoveNext())
                ActualStage = stagesSwitcher.Current;
        }

        public static Map[] Plot { get; } = {TestMap, SpiralMap};

        public string Name { get; }

        public Stage ActualStage { get; private set; }

        public HashSet<Rectangle> Blocks { get; }

        public HashSet<Rectangle> TimeAnomalies { get; }

        public bool TrySwitchStage()
        {
            if (!stagesSwitcher.MoveNext())
            {
                stagesSwitcher = stages.GetEnumerator();
                stagesSwitcher.MoveNext();
                ActualStage = stagesSwitcher.Current;
                return false;
            }

            ActualStage = stagesSwitcher.Current;
            return true;
        }
    }
}