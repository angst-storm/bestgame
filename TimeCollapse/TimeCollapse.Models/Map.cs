using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TimeCollapse.Models
{
    public class Map
    {
        public readonly List<Stage> Stages;
        private IEnumerator<Stage> stagesSwitcher;

        public Map(string name, IEnumerable<Rectangle> blocks, IEnumerable<Rectangle> timeAnomalies,
            IEnumerable<Stage> stages)
        {
            Name = name;
            Blocks = blocks.ToHashSet();
            TimeAnomalies = timeAnomalies.ToHashSet();
            Stages = stages.ToList();
            stagesSwitcher = Stages.GetEnumerator();
            if (stagesSwitcher.MoveNext())
                ActualStage = stagesSwitcher.Current;
        }

        public string Name { get; }

        public Stage ActualStage { get; private set; }

        public HashSet<Rectangle> Blocks { get; }

        public HashSet<Rectangle> TimeAnomalies { get; }

        public bool TrySwitchStage()
        {
            if (!stagesSwitcher.MoveNext())
            {
                ResetMap();
                return false;
            }

            ActualStage = stagesSwitcher.Current;
            return true;
        }

        public void ResetMap()
        {
            stagesSwitcher = Stages.GetEnumerator();
            stagesSwitcher.MoveNext();
            ActualStage = stagesSwitcher.Current;
        }

        #region StaticMembers

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

        public static Map[] Plot { get; } = {TestMap, SpiralMap};

        static Map()
        {
            foreach (var map in Plot)
                AllMaps.Add(map);
            var maps = File.ReadLines(@"UserMaps.txt");
            foreach (var map in maps)
                AllMaps.Add(MapConstructorSerializer.DeserializeMap(map));
        }

        public static void SaveMap(Map map)
        {
            if (map.Blocks.Count == 0) map.Blocks.Add(Rectangle.Empty);
            if (map.TimeAnomalies.Count == 0) map.TimeAnomalies.Add(Rectangle.Empty);
            if (map.Stages.Count == 0) throw new InvalidOperationException();

            AllMaps.Add(map);

            var sw = File.AppendText(@"UserMaps.txt");
            sw.WriteLine(MapConstructorSerializer.SerializeMap(map));
            sw.Close();
        }

        #endregion
    }
}