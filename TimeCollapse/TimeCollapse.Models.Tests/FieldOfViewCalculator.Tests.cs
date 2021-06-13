using System;
using System.Drawing;
using NUnit.Framework;

namespace TimeCollapse.Models.Tests
{
    [TestFixture]
    public class FieldOfViewCalculatorTests
    {
        [TestCase(1, 1, true)]
        [TestCase(7, 4, true)]
        [TestCase(5, 3, true)]
        [TestCase(9, 5, false)]
        [TestCase(3, 3, false)]
        [TestCase(5, 1, false)]
        [TestCase(6, 1, false)]
        public void PointOnLineSegmentTest(double x, double y, bool on)
        {
            var line = (new Vector(1, 1), new Vector(7, 4));
            Assert.AreEqual(on, FieldOfViewCalculator.PointOnLineSegment(new Vector(x, y), line));
        }

        [TestCase(5, 9, 1, 5, true, 3, 7)]
        [TestCase(5, 3, 5, 7, true, 5, 5)]
        [TestCase(6, 1, 10, 3, true, 8, 2)]
        [TestCase(1, 4, 4, 1, false, 9, 1)]
        [TestCase(5, 11, 3, 9, false, 9, 1)]
        [TestCase(7, 5, 7, 9, false, 9, 1)]
        [TestCase(10, 8, 8, 3, false, 9, 1)]
        [TestCase(9, 0, 12, 3, false, 9, 1)]
        public void RayCrossTest(double x1, double y1, double x2, double y2, bool cross, double rx, double ry)
        {
            var section = (new Vector(x1, y1), new Vector(x2, y2));
            var rayLocation = new Vector(1, 9);
            var rayVector = new Vector(8, -8);
            var crossResult = FieldOfViewCalculator.RayCross(rayLocation, rayVector, section, out var result);
            Assert.AreEqual(cross, crossResult);
            Assert.AreEqual(new Vector(rx, ry), result);
        }

        [TestCase(2, 2, false)]
        [TestCase(2, 6, false)]
        [TestCase(3, 4, true)]
        [TestCase(5, 0, false)]
        [TestCase(5, 8, false)]
        [TestCase(6, 4, true)]
        [TestCase(7, 3, true)]
        [TestCase(7, 5, true)]
        [TestCase(9, 1, true)]
        [TestCase(9, 4, true)]
        [TestCase(9, 7, true)]
        [TestCase(10, 2, true)]
        [TestCase(8, 8, false)]
        [TestCase(10, 9, false)]
        public void SectorContainsTest(int x, int y, bool contains)
        {
            Assert.IsTrue(contains == FieldOfViewCalculator.SectorContains(
                new Vector(x, y), new Rectangle(1, 1, 7, 6), true));
        }

        [TestCase(5, 3, true, true)]
        [TestCase(7, 2, true, true)]
        [TestCase(8, 9, true, true)]
        [TestCase(9, 5, true, false)]
        [TestCase(10, 3, true, false)]
        [TestCase(10, 6, true, false)]
        [TestCase(5, 3, false, true)]
        [TestCase(4, 8, false, true)]
        [TestCase(3, 1, false, true)]
        [TestCase(1, 10, false, true)]
        [TestCase(2, 5, false, false)]
        [TestCase(1, 5, false, false)]
        [TestCase(1, 7, false, false)]
        [TestCase(2, 2, false, true)]
        public void DontCrossTest(int x, int y, bool right, bool dontCross)
        {
            Assert.IsTrue(dontCross == FieldOfViewCalculator.DontCross(new Vector(x, y),
                new Rectangle(right ? 5 : 0, 1, 6, 8), right,
                new[] {new Rectangle(2, 3, 1, 4), new Rectangle(7, 3, 1, 4)}));
        }
    }
}