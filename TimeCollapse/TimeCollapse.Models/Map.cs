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

        private static readonly Map Acquaintance = new("Знакомство", new[]
        {
            new Rectangle(496, 544, 928, 16),
            new Rectangle(496, 416, 16, 128),
            new Rectangle(512, 416, 896, 16),
            new Rectangle(1408, 416, 16, 128)
        }, new[]
        {
            new Rectangle(0, 0, 0, 0)
        }, new[]
        {
            new Stage(new Point(528, 480), new Rectangle(1344, 464, 48, 64))
        });

        private static readonly Map SafeChallenge = new("Безопасный вызов", new[]
        {
            new Rectangle(672, 624, 32, 16),
            new Rectangle(640, 640, 32, 16),
            new Rectangle(608, 656, 32, 16),
            new Rectangle(576, 672, 32, 16),
            new Rectangle(544, 688, 32, 16),
            new Rectangle(512, 704, 32, 16),
            new Rectangle(480, 720, 32, 16),
            new Rectangle(448, 736, 32, 16),
            new Rectangle(32, 752, 416, 16),
            new Rectangle(32, 576, 16, 176),
            new Rectangle(704, 608, 32, 16),
            new Rectangle(736, 592, 32, 16),
            new Rectangle(768, 576, 176, 16),
            new Rectangle(944, 592, 32, 16),
            new Rectangle(976, 608, 16, 32),
            new Rectangle(992, 640, 32, 16),
            new Rectangle(1024, 608, 16, 32),
            new Rectangle(1040, 592, 32, 16),
            new Rectangle(1072, 576, 80, 16),
            new Rectangle(1152, 592, 32, 16),
            new Rectangle(1184, 608, 16, 32),
            new Rectangle(1200, 640, 32, 16),
            new Rectangle(1232, 608, 16, 32),
            new Rectangle(1248, 592, 32, 16),
            new Rectangle(1280, 576, 80, 16),
            new Rectangle(1488, 576, 80, 16),
            new Rectangle(1568, 592, 32, 16),
            new Rectangle(1600, 608, 32, 16),
            new Rectangle(1632, 624, 32, 16),
            new Rectangle(1824, 752, 48, 16),
            new Rectangle(1872, 576, 16, 192),
            new Rectangle(1792, 736, 32, 16),
            new Rectangle(1760, 720, 32, 16)
        }, new[]
        {
            new Rectangle(1200, 608, 32, 32),
            new Rectangle(1360, 592, 32, 16),
            new Rectangle(1392, 608, 16, 32),
            new Rectangle(1408, 640, 32, 16),
            new Rectangle(1440, 608, 16, 32),
            new Rectangle(1456, 592, 32, 16),
            new Rectangle(1648, 656, 16, 16),
            new Rectangle(1680, 688, 16, 16),
            new Rectangle(1632, 688, 16, 16),
            new Rectangle(1648, 720, 16, 16),
            new Rectangle(1696, 736, 16, 16),
            new Rectangle(1664, 768, 16, 16),
            new Rectangle(1616, 768, 16, 16),
            new Rectangle(1616, 736, 16, 16),
            new Rectangle(1568, 736, 16, 16),
            new Rectangle(1584, 768, 16, 16),
            new Rectangle(1552, 784, 16, 16),
            new Rectangle(1584, 816, 16, 16),
            new Rectangle(1552, 816, 16, 16),
            new Rectangle(1568, 848, 16, 16),
            new Rectangle(1600, 864, 16, 16),
            new Rectangle(1600, 896, 16, 16),
            new Rectangle(1632, 896, 16, 16),
            new Rectangle(1616, 832, 16, 16),
            new Rectangle(1616, 928, 16, 16),
            new Rectangle(1648, 944, 16, 16),
            new Rectangle(1728, 704, 16, 16)
        }, new[]
        {
            new Stage(new Point(64, 688), new Rectangle(1808, 672, 48, 48))
        });

        private static readonly Map Clones1 = new("Тестирование машины времени", new[]
        {
            new Rectangle(560, 528, 16, 128),
            new Rectangle(560, 512, 800, 16),
            new Rectangle(1344, 528, 16, 128),
            new Rectangle(1072, 240, 16, 144),
            new Rectangle(1344, 400, 16, 112),
            new Rectangle(560, 384, 528, 16),
            new Rectangle(1168, 384, 32, 16),
            new Rectangle(1088, 240, 272, 16),
            new Rectangle(1344, 256, 16, 144),
            new Rectangle(1120, 336, 32, 16),
            new Rectangle(560, 400, 16, 112),
            new Rectangle(1216, 432, 32, 16),
            new Rectangle(576, 640, 768, 16),
            new Rectangle(1200, 400, 16, 32)
        }, new[]
        {
            new Rectangle(0, 0, 0, 0)
        }, new[]
        {
            new Stage(new Point(592, 576), new Rectangle(1280, 576, 48, 48)),
            new Stage(new Point(592, 448), new Rectangle(1280, 448, 48, 48)),
            new Stage(new Point(1296, 448), new Rectangle(592, 448, 48, 48))
        });

        private static readonly Map Clones2 = new("Тестирование машины времени. Усложение", new[]
        {
            new Rectangle(560, 816, 16, 80),
            new Rectangle(1344, 816, 16, 80),
            new Rectangle(560, 800, 320, 16),
            new Rectangle(560, 896, 800, 16),
            new Rectangle(560, 640, 16, 160),
            new Rectangle(1344, 464, 16, 160),
            new Rectangle(800, 624, 96, 16),
            new Rectangle(560, 464, 16, 160),
            new Rectangle(1200, 624, 160, 16),
            new Rectangle(1344, 640, 16, 160),
            new Rectangle(560, 624, 160, 16),
            new Rectangle(1008, 624, 112, 16),
            new Rectangle(1024, 800, 336, 16),
            new Rectangle(560, 448, 320, 16),
            new Rectangle(1024, 448, 336, 16),
            new Rectangle(816, 384, 16, 64),
            new Rectangle(1072, 384, 16, 64),
            new Rectangle(560, 288, 16, 160),
            new Rectangle(560, 272, 320, 16),
            new Rectangle(1344, 192, 16, 256),
            new Rectangle(560, 192, 16, 80),
            new Rectangle(560, 176, 800, 16)
        }, new[]
        {
            new Rectangle(880, 800, 144, 16)
        }, new[]
        {
            new Stage(new Point(592, 832), new Rectangle(1280, 832, 48, 48)),
            new Stage(new Point(592, 736), new Rectangle(1280, 736, 48, 48)),
            new Stage(new Point(592, 560), new Rectangle(1280, 560, 48, 48)),
            new Stage(new Point(1296, 384), new Rectangle(592, 384, 48, 48)),
            new Stage(new Point(592, 208), new Rectangle(928, 736, 48, 48))
        });

        private static readonly Map Evolution = new("Эволюция", new[]
        {
            new Rectangle(560, 528, 16, 112),
            new Rectangle(896, 880, 576, 16),
            new Rectangle(1344, 528, 16, 112),
            new Rectangle(1280, 768, 192, 16),
            new Rectangle(560, 384, 304, 16),
            new Rectangle(784, 848, 32, 16),
            new Rectangle(1152, 768, 64, 16),
            new Rectangle(560, 400, 16, 112),
            new Rectangle(560, 512, 800, 16),
            new Rectangle(1456, 400, 16, 368),
            new Rectangle(816, 864, 80, 16),
            new Rectangle(896, 768, 64, 16),
            new Rectangle(560, 640, 384, 16),
            new Rectangle(1024, 768, 64, 16),
            new Rectangle(752, 832, 32, 16),
            new Rectangle(720, 816, 32, 16),
            new Rectangle(688, 800, 32, 16),
            new Rectangle(656, 784, 32, 16),
            new Rectangle(624, 768, 32, 16),
            new Rectangle(592, 752, 32, 16),
            new Rectangle(560, 736, 32, 16),
            new Rectangle(1456, 784, 16, 96),
            new Rectangle(432, 736, 48, 16),
            new Rectangle(416, 256, 16, 576),
            new Rectangle(432, 816, 128, 16),
            new Rectangle(432, 656, 16, 16),
            new Rectangle(512, 608, 48, 16),
            new Rectangle(544, 528, 16, 16),
            new Rectangle(432, 480, 48, 16),
            new Rectangle(432, 400, 16, 16),
            new Rectangle(1008, 384, 464, 16),
            new Rectangle(800, 304, 32, 16),
            new Rectangle(832, 256, 32, 16),
            new Rectangle(1040, 304, 32, 16),
            new Rectangle(1008, 256, 32, 16),
            new Rectangle(912, 208, 48, 16),
            new Rectangle(1456, 32, 16, 352),
            new Rectangle(272, 16, 1200, 16),
            new Rectangle(768, 144, 48, 16),
            new Rectangle(704, 160, 48, 16),
            new Rectangle(640, 176, 48, 16),
            new Rectangle(560, 192, 48, 16),
            new Rectangle(464, 208, 48, 16),
            new Rectangle(352, 224, 48, 16),
            new Rectangle(272, 32, 16, 288),
            new Rectangle(288, 320, 16, 16),
            new Rectangle(304, 336, 16, 16),
            new Rectangle(320, 352, 16, 16),
            new Rectangle(336, 368, 16, 16),
            new Rectangle(352, 384, 16, 16),
            new Rectangle(288, 512, 96, 16),
            new Rectangle(320, 784, 32, 16),
            new Rectangle(176, 352, 16, 560),
            new Rectangle(352, 896, 384, 16),
            new Rectangle(1280, 624, 16, 16),
            new Rectangle(1216, 608, 64, 16),
            new Rectangle(1040, 640, 32, 16),
            new Rectangle(1200, 624, 16, 16),
            new Rectangle(1168, 640, 32, 16),
            new Rectangle(1152, 624, 16, 16),
            new Rectangle(1088, 608, 64, 16),
            new Rectangle(1072, 624, 16, 16),
            new Rectangle(1296, 640, 64, 16),
            new Rectangle(1024, 624, 16, 16),
            new Rectangle(960, 608, 64, 16),
            new Rectangle(944, 624, 16, 16)
        }, new[]
        {
            new Rectangle(0, 0, 0, 0),
            new Rectangle(960, 784, 16, 16),
            new Rectangle(976, 800, 32, 16),
            new Rectangle(1008, 784, 16, 16),
            new Rectangle(1088, 784, 16, 16),
            new Rectangle(1104, 800, 32, 16),
            new Rectangle(1136, 784, 16, 16),
            new Rectangle(1216, 784, 16, 16),
            new Rectangle(1232, 800, 32, 16),
            new Rectangle(1264, 784, 16, 16),
            new Rectangle(560, 752, 16, 64),
            new Rectangle(416, 224, 32, 16),
            new Rectangle(528, 208, 16, 16),
            new Rectangle(384, 528, 32, 16),
            new Rectangle(192, 896, 160, 16),
            new Rectangle(176, 320, 16, 16),
            new Rectangle(208, 288, 16, 16),
            new Rectangle(240, 256, 16, 16),
            new Rectangle(240, 320, 16, 16),
            new Rectangle(208, 336, 16, 16),
            new Rectangle(176, 272, 16, 16),
            new Rectangle(192, 224, 16, 16),
            new Rectangle(160, 240, 16, 16),
            new Rectangle(128, 256, 16, 16),
            new Rectangle(128, 304, 16, 16),
            new Rectangle(96, 288, 16, 16),
            new Rectangle(80, 256, 16, 16),
            new Rectangle(64, 304, 16, 16),
            new Rectangle(32, 288, 16, 16),
            new Rectangle(48, 256, 16, 16),
            new Rectangle(16, 320, 16, 16)
        }, new[]
        {
            new Stage(new Point(592, 576), new Rectangle(1280, 544, 48, 48)),
            new Stage(new Point(592, 448), new Rectangle(1392, 704, 48, 48)),
            new Stage(new Point(1264, 448), new Rectangle(496, 752, 48, 48)),
            new Stage(new Point(1408, 816), new Rectangle(1088, 320, 48, 48)),
            new Stage(new Point(1408, 320), new Rectangle(656, 832, 48, 48))
        });

        private static readonly Map BigMap = new("Большой уровень", new[]
        {
            new Rectangle(16, 928, 720, 16),
            new Rectangle(16, 288, 16, 640),
            new Rectangle(32, 624, 80, 16),
            new Rectangle(176, 784, 96, 16),
            new Rectangle(256, 800, 16, 64),
            new Rectangle(176, 720, 16, 64),
            new Rectangle(96, 704, 96, 16),
            new Rectangle(96, 640, 16, 64),
            new Rectangle(96, 720, 16, 144),
            new Rectangle(176, 800, 16, 64),
            new Rectangle(480, 880, 48, 16),
            new Rectangle(480, 896, 16, 32),
            new Rectangle(528, 832, 48, 16),
            new Rectangle(528, 848, 16, 48),
            new Rectangle(576, 800, 16, 48),
            new Rectangle(576, 784, 160, 16),
            new Rectangle(736, 912, 32, 16),
            new Rectangle(768, 896, 32, 16),
            new Rectangle(800, 880, 32, 16),
            new Rectangle(832, 864, 32, 16),
            new Rectangle(864, 848, 432, 16),
            new Rectangle(960, 816, 16, 32),
            new Rectangle(1040, 800, 16, 48),
            new Rectangle(1120, 784, 16, 64),
            new Rectangle(1200, 768, 16, 80),
            new Rectangle(1280, 752, 32, 16),
            new Rectangle(1456, 608, 16, 80),
            new Rectangle(1360, 688, 112, 16),
            new Rectangle(1280, 768, 16, 80),
            new Rectangle(1264, 928, 448, 16),
            new Rectangle(1600, 848, 16, 16),
            new Rectangle(1520, 784, 16, 16),
            new Rectangle(1600, 720, 16, 16),
            new Rectangle(1520, 656, 16, 16),
            new Rectangle(1600, 592, 16, 16),
            new Rectangle(1504, 656, 16, 16),
            new Rectangle(1504, 784, 16, 16),
            new Rectangle(1616, 848, 16, 16),
            new Rectangle(1616, 720, 16, 16),
            new Rectangle(1616, 592, 16, 16),
            new Rectangle(1424, 592, 48, 16),
            new Rectangle(1712, 528, 176, 16),
            new Rectangle(1664, 528, 48, 16),
            new Rectangle(1696, 544, 16, 384),
            new Rectangle(1760, 432, 16, 96),
            new Rectangle(1872, 288, 16, 240),
            new Rectangle(1840, 448, 32, 16),
            new Rectangle(1136, 416, 640, 16),
            new Rectangle(944, 544, 96, 16),
            new Rectangle(864, 480, 32, 16),
            new Rectangle(784, 416, 32, 16),
            new Rectangle(416, 352, 320, 16),
            new Rectangle(592, 288, 16, 64),
            new Rectangle(512, 288, 16, 64),
            new Rectangle(832, 112, 16, 64),
            new Rectangle(832, 176, 96, 16),
            new Rectangle(912, 112, 16, 64),
            new Rectangle(688, 256, 16, 16),
            new Rectangle(736, 224, 16, 16),
            new Rectangle(768, 192, 16, 16),
            new Rectangle(800, 144, 16, 16),
        }, new[]
        {
            new Rectangle(128, 736, 32, 32),
            new Rectangle(208, 816, 32, 32),
            new Rectangle(48, 656, 32, 32),
            new Rectangle(1200, 880, 48, 48),
            new Rectangle(1600, 368, 32, 32),
            new Rectangle(1408, 368, 32, 32),
            new Rectangle(1216, 368, 32, 32),
            new Rectangle(704, 400, 48, 48),
            new Rectangle(784, 464, 48, 48),
            new Rectangle(384, 416, 16, 16),
            new Rectangle(336, 464, 16, 16),
            new Rectangle(288, 512, 16, 16),
        }, new[]
        {
            new Stage(new Point(48, 560), new Rectangle(1392, 624, 48, 48)),
            new Stage(new Point(48, 720), new Rectangle(576, 864, 48, 48)),
            new Stage(new Point(656, 864), new Rectangle(1696, 464, 48, 48)),
            new Stage(new Point(1792, 464), new Rectangle(560, 864, 48, 48)),
            new Stage(new Point(976, 480), new Rectangle(48, 560, 48, 48)),
            new Stage(new Point(544, 288), new Rectangle(848, 112, 64, 64)),
        });

        private static readonly Map CrossMap = new("Эндшпиль", new[]
        {
            new Rectangle(624, 928, 1264, 16),
            new Rectangle(16, 832, 16, 96),
            new Rectangle(1872, 832, 16, 96),
            new Rectangle(160, 912, 16, 16),
            new Rectangle(176, 896, 16, 16),
            new Rectangle(192, 880, 16, 16),
            new Rectangle(208, 864, 16, 16),
            new Rectangle(288, 864, 16, 16),
            new Rectangle(304, 880, 16, 16),
            new Rectangle(320, 896, 16, 16),
            new Rectangle(336, 912, 16, 16),
            new Rectangle(432, 912, 16, 16),
            new Rectangle(448, 896, 16, 16),
            new Rectangle(464, 880, 16, 16),
            new Rectangle(480, 864, 16, 16),
            new Rectangle(560, 864, 16, 16),
            new Rectangle(576, 880, 16, 16),
            new Rectangle(592, 896, 16, 16),
            new Rectangle(608, 912, 16, 16),
            new Rectangle(16, 928, 416, 16),
            new Rectangle(960, 832, 32, 96),
            new Rectangle(768, 880, 32, 16),
            new Rectangle(864, 848, 32, 16),
            new Rectangle(1024, 752, 32, 16),
            new Rectangle(896, 704, 32, 16),
            new Rectangle(768, 656, 32, 16),
            new Rectangle(16, 656, 624, 16),
            new Rectangle(1680, 864, 16, 16),
            new Rectangle(1696, 880, 16, 16),
            new Rectangle(1712, 896, 16, 16),
            new Rectangle(1728, 912, 16, 16),
            new Rectangle(1120, 848, 32, 16),
            new Rectangle(1072, 800, 32, 16),
            new Rectangle(1168, 896, 32, 16),
            new Rectangle(1136, 688, 32, 16),
            new Rectangle(1168, 672, 32, 16),
            new Rectangle(1200, 656, 688, 16),
            new Rectangle(1872, 544, 16, 112),
            new Rectangle(16, 544, 16, 112),
            new Rectangle(864, 80, 16, 224),
            new Rectangle(864, 432, 32, 16),
            new Rectangle(864, 448, 16, 80),
            new Rectangle(1072, 80, 16, 224),
            new Rectangle(1056, 432, 32, 16),
            new Rectangle(1072, 448, 16, 80),
            new Rectangle(336, 576, 64, 16),
            new Rectangle(384, 592, 16, 64),
            new Rectangle(416, 592, 48, 16),
            new Rectangle(480, 608, 48, 16),
            new Rectangle(544, 624, 48, 16),
            new Rectangle(512, 512, 32, 16),
            new Rectangle(576, 480, 32, 16),
            new Rectangle(640, 448, 32, 16),
            new Rectangle(720, 432, 32, 16),
            new Rectangle(800, 432, 32, 16),
            new Rectangle(1120, 432, 32, 16),
            new Rectangle(1200, 432, 32, 16),
            new Rectangle(1280, 448, 32, 16),
            new Rectangle(1344, 480, 32, 16),
            new Rectangle(1408, 512, 32, 16),
            new Rectangle(1472, 544, 32, 16),
            new Rectangle(16, 288, 848, 16),
            new Rectangle(1536, 576, 32, 16),
            new Rectangle(704, 240, 16, 16),
            new Rectangle(608, 192, 16, 16),
            new Rectangle(496, 160, 16, 16),
            new Rectangle(368, 160, 16, 16),
            new Rectangle(240, 192, 16, 16),
            new Rectangle(128, 208, 16, 16),
            new Rectangle(64, 160, 16, 16),
            new Rectangle(48, 112, 16, 16),
            new Rectangle(16, 16, 16, 272),
            new Rectangle(1088, 288, 800, 16),
            new Rectangle(1872, 16, 16, 272),
            new Rectangle(1216, 272, 16, 16),
            new Rectangle(1232, 256, 16, 16),
            new Rectangle(1248, 240, 16, 16),
            new Rectangle(1264, 224, 16, 16),
            new Rectangle(1280, 208, 16, 16),
            new Rectangle(1296, 192, 16, 16),
            new Rectangle(1328, 160, 16, 16),
            new Rectangle(1312, 176, 16, 16),
            new Rectangle(1088, 144, 96, 16),
            new Rectangle(1600, 176, 16, 16),
            new Rectangle(1664, 208, 16, 16),
            new Rectangle(1744, 240, 16, 16),
            new Rectangle(1552, 240, 16, 16),
            new Rectangle(1504, 208, 16, 16),
            new Rectangle(1440, 160, 16, 16),
            new Rectangle(1376, 224, 16, 16),
            new Rectangle(1456, 864, 16, 16),
            new Rectangle(1440, 880, 16, 16),
            new Rectangle(1424, 896, 16, 16),
            new Rectangle(1408, 912, 16, 16),
            new Rectangle(1120, 544, 96, 16),
            new Rectangle(1216, 560, 32, 16),
            new Rectangle(1248, 576, 32, 16),
        }, new[]
        {
            new Rectangle(432, 944, 192, 16),
            new Rectangle(1744, 912, -16, 16),
            new Rectangle(1712, 912, 16, 16),
            new Rectangle(1696, 896, 16, 16),
            new Rectangle(1680, 880, 16, 16),
            new Rectangle(1648, 880, 16, 16),
            new Rectangle(1632, 896, 16, 16),
            new Rectangle(1616, 912, 16, 16),
            new Rectangle(1520, 912, 16, 16),
            new Rectangle(1504, 896, 16, 16),
            new Rectangle(1488, 880, 16, 16),
            new Rectangle(1456, 880, 16, 16),
            new Rectangle(1440, 896, 16, 16),
            new Rectangle(1424, 912, 16, 16),
            new Rectangle(880, 512, 192, 16),
            new Rectangle(48, 256, 640, 16),
        }, new[]
        {
            new Stage(new Point(48, 864), new Rectangle(32, 592, 48, 48)),
            new Stage(new Point(1824, 864), new Rectangle(1808, 592, 48, 48)),
            new Stage(new Point(48, 592), new Rectangle(1376, 352, 48, 48)),
            new Stage(new Point(1824, 592), new Rectangle(528, 352, 48, 48)),
            new Stage(new Point(816, 224), new Rectangle(48, 48, 48, 48)),
            new Stage(new Point(1808, 208), new Rectangle(1120, 224, 48, 48)),
            new Stage(new Point(1104, 224), new Rectangle(944, 432, 64, 64)),
        });

        public static readonly BindingList<Map> AllMaps = new();

        public static Map[] Plot { get; } =
            {Acquaintance, SafeChallenge, Clones1, Clones2, Evolution, BigMap, CrossMap};

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

        public static void DeleteMap(Map map)
        {
            AllMaps.Remove(map);
            File.WriteAllLines(@"UserMaps.txt",
                File.ReadLines(@"UserMaps.txt").Where(l => MapConstructorSerializer.DeserializeMap(l).Name != map.Name)
                    .ToList());
        }

        #endregion
    }
}