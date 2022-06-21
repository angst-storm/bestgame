using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TimeCollapse.Models
{
    public static class FieldOfViewCalculator
    {
        private const int ViewAngleHalf = 15;
        private const int ViewingRange = 320;
        private const int NumberOfRays = 100;
        private static double ViewAngleHalfRad => ViewAngleHalf * (Math.PI / 180);

        public static Point[] GetFieldOfViewRayTracing(this Explorer e, Game game)
        {
            var viewRect = GetViewRectangle(e);
            var linesInSight = game.Map.Blocks.Where(b => b.IntersectsWith(viewRect)).SelectMany(b => new[]
            {
                (new Vector(b.Left, b.Top), new Vector(b.Right, b.Top)),
                (new Vector(b.Left, b.Bottom), new Vector(b.Right, b.Bottom)),
                (new Vector(b.Left, b.Top), new Vector(b.Left, b.Bottom)),
                (new Vector(b.Right, b.Top), new Vector(b.Right, b.Bottom))
            }).ToList();

            var start = new Vector(e.TurnedRight ? viewRect.Left : viewRect.Right,
                viewRect.Top + viewRect.Size.Height / 2);
            var currentVector = new Vector(e.TurnedRight ? viewRect.Right : viewRect.Left, viewRect.Top) - start;
            var dAngle = (e.TurnedRight ? -1 : 1) * ViewAngleHalfRad * 2 / NumberOfRays;

            var result = new List<Point> { start.ToPoint() };
            for (var i = 0; i < NumberOfRays; i++)
            {
                var shortestCrossedRay = start + currentVector;
                foreach (var line in linesInSight)
                    if (RayCross(start, currentVector, line, out var cross))
                        if ((cross - start).Length < (shortestCrossedRay - start).Length)
                            shortestCrossedRay = cross;

                result.Add(shortestCrossedRay.ToPoint());
                currentVector = currentVector.Turn(dAngle);
            }

            return result.ToArray();
        }

        private static Rectangle GetViewRectangle(Explorer e)
        {
            var heightHalf = (int)(Math.Tan(ViewAngleHalfRad) * ViewingRange);
            return new Rectangle(
                e.Location.X - (e.TurnedRight ? -e.Collider.Size.Width : ViewingRange),
                e.Location.Y - heightHalf + e.Collider.Size.Height / 4,
                ViewingRange,
                heightHalf * 2);
        }

        public static bool RayCross(Vector rayStart, Vector rayVector, (Vector, Vector) line, out Vector result)
        {
            result = new Vector(rayStart.X + rayVector.X, rayStart.Y + rayVector.Y);

            var (a, b, c) = FindTheLineClerics(line.Item1, line.Item2);
            var (v, w) = (rayVector.X, rayVector.Y);
            if (a * v + b * w == 0) return false;

            var t = (-a * rayStart.X - b * rayStart.Y - c) / (a * v + b * w);
            if (t < 0) return false;

            var (x, y) = (rayStart.X + v * t, rayStart.Y + w * t);
            if (!PointOnLineSegment(new Vector(x, y), line) ||
                !PointOnLineSegment(new Vector(x, y), (rayStart, rayStart + rayVector))) return false;
            result = new Vector(x, y);
            return true;
        }

        public static bool PointOnLineSegment(Vector point, (Vector, Vector) line)
        {
            var (a, b, c) = FindTheLineClerics(line.Item1, line.Item2);
            if (Math.Abs(a * point.X + b * point.Y + c) > 1e-9) return false;
            var pointToStart = line.Item1 - point;
            var pointToEnd = line.Item2 - point;
            return pointToStart.X * pointToEnd.X + pointToStart.Y * pointToEnd.Y <= 0;
        }

        private static (double, double, double) FindTheLineClerics(Vector a, Vector b)
        {
            return (b.Y - a.Y, a.X - b.X, a.Y * b.X - a.X * b.Y);
        }

        public static bool FieldOfViewContains(this Explorer e, Game game, Vector point)
        {
            var viewRect = GetViewRectangle(e);
            if (!viewRect.Contains(point.ToPoint())) return false;

            return SectorContains(point, viewRect, e.TurnedRight) &&
                   DontCross(point, viewRect, e.TurnedRight, game.Map.Blocks);
        }

        public static bool SectorContains(Vector point, Rectangle viewRect, bool right)
        {
            var start = new Vector(right ? viewRect.Left : viewRect.Right,
                viewRect.Top + viewRect.Size.Height / 2);
            var comparativeVector = new Vector(right ? viewRect.Right : viewRect.Left, viewRect.Top) - start;
            var ray = point - start;
            return !(Math.Atan2(Math.Abs(ray.Y), Math.Abs(ray.X)) >
                     Math.Atan2(Math.Abs(comparativeVector.Y), Math.Abs(comparativeVector.X)));
        }

        public static bool DontCross(Vector point, Rectangle viewRect, bool right, IEnumerable<Rectangle> colliders)
        {
            var linesInSight = colliders.Where(b => b.IntersectsWith(viewRect)).SelectMany(b => new[]
            {
                (new Vector(b.Left, b.Top), new Vector(b.Right, b.Top)),
                (new Vector(b.Left, b.Bottom), new Vector(b.Right, b.Bottom)),
                (new Vector(b.Left, b.Top), new Vector(b.Left, b.Bottom)),
                (new Vector(b.Right, b.Top), new Vector(b.Right, b.Bottom))
            }).ToList();
            var start = new Vector(right ? viewRect.Left : viewRect.Right,
                viewRect.Top + viewRect.Size.Height / 2);
            var ray = point - start;
            return !linesInSight.Any(l => RayCross(start, ray, l, out _));
        }
    }
}