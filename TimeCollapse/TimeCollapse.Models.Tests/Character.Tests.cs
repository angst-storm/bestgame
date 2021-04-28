using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;

namespace TimeCollapse.Models.Tests
{
    public class CharacterTests
    {
/*
        private static Game GameForTest(Point startPosition)
        {
            var blockSize = new Size(3, 3);

            var collisionTestMap = new Map(new[]
            {
                new Rectangle(new Point(1, 1), blockSize),
                new Rectangle(new Point(1, 9), blockSize),
                new Rectangle(new Point(1, 17), blockSize),
                new Rectangle(new Point(9, 1), blockSize),
                new Rectangle(new Point(17, 1), blockSize),
                new Rectangle(new Point(17, 9), blockSize),
                new Rectangle(new Point(17, 17), blockSize),
                new Rectangle(new Point(9, 17), blockSize)
            }, startPosition);

            var maxSpeedTestMap = new Map(new[]
            {
                new Rectangle(new Point(1, 4), new Size(12, 12))
            }, new Point(1, 1));

            return new Game(new List<Map> {collisionTestMap, maxSpeedTestMap}, new Size(3, 3), false);
        }

        [TestCase(0, 3, 0, 3)]
        [TestCase(0, 7, 0, 5)]
        [TestCase(0, -7, 0, -5)]
        [TestCase(7, 0, 5, 0)]
        [TestCase(-7, 0, -5, 0)]
        [TestCase(7, 7, 5, 5)]
        [TestCase(-7, -7, -5, -5)]
        [TestCase(-7, 7, -5, 5)]
        [TestCase(7, -7, 5, -5)]
        public void StraightPathTranslateTest(int xOffSet, int yOffSet, int expectedXOffSet, int expectedYOffSet)
        {
            var game = GameForTest(new Point(9, 9));
            var startPosition = game.PresentExplorer.Location;
            game.PresentExplorer.Translate(new Vector(xOffSet, yOffSet));
            Assert.AreEqual(new Point(startPosition.X + expectedXOffSet, startPosition.Y + expectedYOffSet),
                game.PresentExplorer.Location);
        }

        [TestCase(5, 2, -3, 0, -1, 0)]
        [TestCase(5, 2, 3, 0, 1, 0)]
        [TestCase(2, 5, 0, -3, 0, -1)]
        [TestCase(2, 5, 0, 3, 0, 1)]
        [TestCase(5, 8, -3, 0, -1, 0)]
        [TestCase(13, 8, 3, 0, 1, 0)]
        public void AngleApproachTranslateTest(int startX, int startY, int xOffSet, int yOffSet,
            int expectedXOffSet, int expectedYOffSet)
        {
            var game = GameForTest(new Point(startX, startY));
            var startPosition = game.PresentExplorer.Location;
            game.PresentExplorer.Translate(new Vector(xOffSet, yOffSet));
            Assert.AreEqual(new Point(startPosition.X + expectedXOffSet, startPosition.Y + expectedYOffSet),
                game.PresentExplorer.Location);
        }

        [Test]
        public void ThroughTheBlocksTest()
        {
            var game = GameForTest(new Point(9, 9));
            game.SwitchMap(1);
            var startPosition = game.PresentExplorer.Location;
            game.PresentExplorer.Translate(new Vector(0, 10));
            Assert.AreEqual(game.PresentExplorer.Location, startPosition);
        }

        [Test]
        public void OnFloorTest()
        {
            var game = GameForTest(new Point(9, 14));
            game.PresentExplorer.Translate(new Vector(0,0));
            Assert.AreEqual(true, game.PresentExplorer.OnFloor);
            game.PresentExplorer.Translate(new Vector(0, -1));
            Assert.AreEqual(false, game.PresentExplorer.OnFloor);
        }
*/
    }
}