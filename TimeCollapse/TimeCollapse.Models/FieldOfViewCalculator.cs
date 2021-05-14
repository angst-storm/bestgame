using System;
using System.Drawing;

namespace TimeCollapse.Models
{
    public static class FieldOfViewCalculator
    {
        private const int ViewAngleHalf = 15;
        private const int ViewingRange = 320;

        public static Point[] GetFieldOfView(Game game, Explorer e)
        {
            return GetViewTriangle(e);
        }

        private static Rectangle GetViewRectangle(Explorer e)
        {
            var heightHalf = (int) (Math.Tan(ViewAngleHalf * Math.PI / 180) * ViewingRange);
            return new Rectangle(
                e.Location.X - (e.TurnedRight ? -e.Collider.Size.Width : ViewingRange),
                e.Location.Y - heightHalf + e.Collider.Size.Height / 4,
                ViewingRange,
                heightHalf * 2);
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

        private static Point[] GetCollidersIntersection(Game game, Explorer e)
        {
            throw new NotImplementedException();
        }

        private static bool TryFindIntersection((Point, Point) a, (Point, Point) b, out Point intersection)
        {
            throw new NotImplementedException();
        }
    }
}