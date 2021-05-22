using NUnit.Framework;

namespace TimeCollapse.Models.Tests
{
    public class FieldOfViewCalculatorTests
    {
        [TestCase(1, 1, true)]
        [TestCase(7, 4, true)]
        [TestCase(5, 3, true)]
        [TestCase(9, 5, false)]
        [TestCase(3, 3, false)]
        [TestCase(5, 1, false)]
        [TestCase(6, 1, false)]
        public void PointOnLineTest(double x, double y, bool on)
        {
            var line = (new Vector(1, 1), new Vector(7, 4));
            Assert.AreEqual(on, FieldOfViewCalculator.PointOnLine(new Vector(x, y), line));
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
            var rayLocation = new Vector(1, 2);
            var rayVector = new Vector(8, 8);
            var crossResult = FieldOfViewCalculator.RayCross(rayLocation, rayVector, section, out var result);
            Assert.AreEqual(cross, crossResult);
            Assert.AreEqual(new Vector(rx, ry), result);
        }
    }
}