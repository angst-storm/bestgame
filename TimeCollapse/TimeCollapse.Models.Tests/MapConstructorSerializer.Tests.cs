using System;
using System.Drawing;
using System.Linq;
using NUnit.Framework;

namespace TimeCollapse.Models.Tests
{
    [TestFixture]
    public class MapConstructorSerializerTests
    {
        [TestCase(0, 0, 0, 0)]
        [TestCase(1920, 1920, 1920, 1920)]
        public void SerializeRectangleTest(int x, int y, int width, int height)
        {
            var rect = new Rectangle(x, y, width, height);
            var code = MapConstructorSerializer.SerializeRectangle(rect);
            Assert.IsTrue(code is >= 0 and <= 214358880);
            var restoredRect = MapConstructorSerializer.DeserializeRectangle(code);
            Assert.AreEqual(rect, restoredRect);
        }

        [Test]
        public void SerializeRectangleRandomTest()
        {
            var rnd = new Random();
            for (var i = 0; i < 1000; i++)
            {
                var rect = RandomRectangle(rnd);
                SerializeRectangleTest(
                    rect.X,
                    rect.Y,
                    rect.Width,
                    rect.Height);
            }
        }

        [TestCase(0, 0, 0, 0, 0, 0)]
        [TestCase(1920, 1920, 1920, 1920, 1920, 1920)]
        public void SerializeStageTest(int sx, int sy, int tx, int ty, int tWidth, int tHeight)
        {
            var stage = new Stage(new Point(sx, sy), new Rectangle(tx, ty, tWidth, tHeight));
            var (code1, code2) = MapConstructorSerializer.SerializeStage(stage);
            Assert.IsTrue(code1 is >= 0 and <= 14640);
            Assert.IsTrue(code2 is >= 0 and <= 214358880);
            var restoredStage = MapConstructorSerializer.DeserializeStage(code1, code2);
            Assert.AreEqual(stage.Spawn, restoredStage.Spawn);
            Assert.AreEqual(stage.Target, restoredStage.Target);
        }

        [Test]
        public void SerializeStageRandomTest()
        {
            var rnd = new Random();
            for (var i = 0; i < 1000; i++)
            {
                var stage = RandomStage(rnd);
                SerializeStageTest(
                    stage.Spawn.X,
                    stage.Spawn.Y,
                    stage.Target.X,
                    stage.Target.Y,
                    stage.Target.Width,
                    stage.Target.Height);
            }
        }

        [Test]
        public void SerializeMapRandomTest()
        {
            var rnd = new Random();
            for (var i = 1; i <= 100; i++)
            {
                var name = "test" + i;
                var blocks = Enumerable
                    .Repeat(0, rnd.Next(10, 1000))
                    .Select(_ => RandomRectangle(rnd));
                var timeAnomalies = Enumerable
                    .Repeat(0, rnd.Next(10, 1000))
                    .Select(_ => RandomRectangle(rnd));
                var stages = Enumerable
                    .Repeat(0, rnd.Next(1, 20))
                    .Select(_ => RandomStage(rnd));
                var map = new Map(name, blocks, timeAnomalies, stages);
                var code = MapConstructorSerializer.SerializeMap(map);
                var restoredMap = MapConstructorSerializer.DeserializeMap(code);
                CollectionAssert.AreEqual(map.Blocks, restoredMap.Blocks,
                    $"Ошибка в тесте {name} (блоки)");
                CollectionAssert.AreEqual(map.TimeAnomalies, restoredMap.TimeAnomalies,
                    $"Ошибка в тесте {name} (аномалии)");
                CollectionAssert.AreEqual(map.Stages.Select(s => s.Spawn),
                    restoredMap.Stages.Select(s => s.Spawn),
                    $"Ошибка в тесте {name} (спавны)");
                CollectionAssert.AreEqual(map.Stages.Select(s => s.Target),
                    restoredMap.Stages.Select(s => s.Target),
                    $"Ошибка в тесте {name} (порталы)");
            }
        }

        private static Stage RandomStage(Random rnd)
        {
            return new(new Point(rnd.Next(0, 120) * 16, rnd.Next(0, 120) * 16), RandomRectangle(rnd));
        }

        private static Rectangle RandomRectangle(Random rnd)
        {
            return new(rnd.Next(0, 120) * 16, rnd.Next(0, 120) * 16, rnd.Next(0, 120) * 16,
                rnd.Next(0, 120) * 16);
        }
    }
}