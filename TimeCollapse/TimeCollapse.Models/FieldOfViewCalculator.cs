using System;
using System.Drawing;
using System.Linq;

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

        public static Point[] GetFieldOfViewNonTrivial(this Explorer e, Game game)
        {
            var viewRect = GetViewRectangle(e);
            var location = new Point(e.TurnedRight ? viewRect.Left : viewRect.Right,
                viewRect.Top + viewRect.Size.Height / 2);
            var viewDirection = new Vector(e.TurnedRight ? 1 : -1, 0);
            var colliders = game.ActualMap.Blocks
                .Where(r => r.IntersectsWith(viewRect))
                .ToList();

            var sections = colliders
                .SelectMany(r => new[]
                {
                    (new Point(r.Left, r.Top), new Point(r.Left, r.Bottom)),
                    (new Point(r.Right, r.Top), new Point(r.Right, r.Bottom)),
                    (new Point(r.Left, r.Top), new Point(r.Right, r.Top)),
                    (new Point(r.Left, r.Bottom), new Point(r.Right, r.Top))
                })
                .ToList();

            var anglePoints = sections
                .SelectMany(s => new[] {s.Item1, s.Item2})
                .Distinct()
                .ToList();

            return anglePoints.ToArray();
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
    }
}