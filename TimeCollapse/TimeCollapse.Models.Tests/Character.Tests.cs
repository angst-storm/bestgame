using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;

namespace TimeCollapse.Models.Tests
{
    public class CharacterTests
    {
        public static readonly Map CollisionTestMap1 = new(new[]
        {
            new Rectangle(2, 1, 4, 1),
            new Rectangle(1, 2, 1, 4),
            new Rectangle(6, 2, 1, 4),
            new Rectangle(2, 6, 4, 1)
        }, new[] {(new Point(3, 4), new Point(7, 7))});

        public static Map CollisionTestMap2(int wallWidth) => new Map(new[]
        {
            new Rectangle(1, 6, 5, 16),
            new Rectangle(6, 1, wallWidth, 5)
        }, new[] {(new Point(4, 4), new Point())});

        [TestCase(0, 0, 3, 4)]
        [TestCase(-1, 0, 2, 4)]
        [TestCase(-2, 0, 2, 4)]
        [TestCase(1, 0, 4, 4)]
        [TestCase(2, 0, 4, 4)]
        [TestCase(0, 1, 3, 4)]
        [TestCase(0, -1, 3, 4)]
        [TestCase(0, -2, 3, 3)]
        [TestCase(0, -3, 3, 2)]
        [TestCase(-1, -3, 2, 2)]
        [TestCase(-2, -4, 2, 2)]
        [TestCase(1, -3, 4, 2)]
        [TestCase(2, -4, 4, 2)]
        public void RoomCollisionTest(int offsetX, int offsetY, int expectedX, int expectedY)
        {
            var game = new Game(new List<Map> {CollisionTestMap1}, new Size(2, 2));
            game.PresentExplorer.Translate(new Vector(offsetX, offsetY));
            Assert.AreEqual(new Point(expectedX, expectedY), game.PresentExplorer.Location);
        }

        [TestCase(1, 1, 4, 4)]
        [TestCase(3, 1, 7, 5)]
        [TestCase(1, 16, 4, 4)]
        [TestCase(16, 16, 4, 5)]
        [TestCase(20, 16, 4, 5)]
        public void ThroughTheBlocksTest(int speed, int wallWidth, int expectedX, int expectedY)
        {
            var game = new Game(new List<Map> {CollisionTestMap2(wallWidth)}, new Size(2, 2));
            game.PresentExplorer.Translate(new Vector(speed, 0));
            Assert.AreEqual(new Point(expectedX, expectedY), game.PresentExplorer.Location);
        }

        [Test]
        public void OnFloorTest()
        {
            var game = new Game(new List<Map> {CollisionTestMap2(0)}, new Size(2, 2));
            game.PresentExplorer.Translate(new Vector(0,0));
            Assert.AreEqual(true, game.PresentExplorer.OnFloor);
            game.PresentExplorer.Translate(new Vector(0, -2));
            Assert.AreEqual(false, game.PresentExplorer.OnFloor);
        }
    }
}