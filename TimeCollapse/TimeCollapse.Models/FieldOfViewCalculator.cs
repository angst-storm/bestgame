using System;
using System.Drawing;

namespace TimeCollapse.Models
{
    public static class FieldOfViewCalculator
    {
        private const int ViewAngleHalf = 15;
        private const int ViewingRange = 320;
        private static double ViewAngleHalfRad => ViewAngleHalf * Math.PI / 180;

        public static Point[] GetFieldOfView(this Explorer e)
        {
            return GetViewTriangle(e);
        }

        private static Point[] GetViewTriangle(Explorer e)
        {
            var viewRect = GetViewRectangle(e);
            return new[]
            {
                new Point(e.TurnedRight ? viewRect.Left : viewRect.Right, viewRect.Top + viewRect.Size.Height / 2),
                new Point(e.TurnedRight ? viewRect.Right : viewRect.Left, viewRect.Top),
                new Point(e.TurnedRight ? viewRect.Right : viewRect.Left, viewRect.Bottom)
            };
        }

        private static Rectangle GetViewRectangle(Explorer e)
        {
            var heightHalf = (int) (Math.Tan(ViewAngleHalfRad) * ViewingRange);
            return new Rectangle(
                e.Location.X - (e.TurnedRight ? -e.Collider.Size.Width : ViewingRange),
                e.Location.Y - heightHalf + e.Collider.Size.Height / 4,
                ViewingRange,
                heightHalf * 2);
        }

        public static Point[] GetFieldOfViewNonTrivial(this Explorer e, Game game, Vector start)
        {
            return Array.Empty<Point>();
        }

        public static bool RayCross(Vector rayStart, Vector rayVector, (Vector, Vector) line, out Vector result)
        {
            throw new NotImplementedException();
        }

        private static (double, double, double) FindTheLineClerics(Vector a, Vector b)
        {
            return (b.Y - a.Y, b.X - a.X, a.X * b.Y - b.X * a.Y);
        }

        public static bool PointOnLine(Vector point, (Vector, Vector) line)
        {
            throw new NotImplementedException();
        }
    }
}