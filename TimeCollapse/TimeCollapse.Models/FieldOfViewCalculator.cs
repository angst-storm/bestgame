using System;
using System.Collections.Generic;
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
            result = new Vector(rayStart.X + rayVector.X, rayStart.Y + rayVector.Y);

            var (a, b, c) = FindTheLineClerics(line.Item1, line.Item2);

            var v = rayVector.X;
            var w = rayVector.Y;

            var result1 = a * rayStart.X + b * rayStart.Y + c;
            var result2 = a * (rayStart + rayVector).X + b * (rayStart + rayVector).Y + c;

            if (result1 == 0 && result2 == 0)
            {
                Console.WriteLine("a");
                return false;
            }

            if (a * v + b * w == 0)
            {
                Console.WriteLine("b");
                return false;
            }

            var t = (-a * rayStart.X - b * rayStart.Y - c) / (a * v + b * w);

            if (t < 0)
            {
                Console.WriteLine("c");
                return false;
            }

            var resultX = rayStart.X + v * t;
            var resultY = rayStart.Y + w * t;

            if (!PointOnLineSegment(new Vector(resultX, resultY), line) || !PointOnLineSegment(new Vector(resultX, resultY), (rayStart, rayStart+rayVector))) return false;

            result = new Vector(resultX, resultY);
            return true;
        }

        private static (double, double, double) FindTheLineClerics(Vector a, Vector b)
        {
            return (b.Y - a.Y, a.X - b.X, a.Y * b.X - a.X * b.Y);
        }

        public static bool PointOnLineSegment(Vector point, (Vector, Vector) line)
        {
            var x = point.X;
            var y = point.Y;
            var x1 = line.Item1.X;
            var y1 = line.Item1.Y;
            var x2 = line.Item2.X;
            var y2 = line.Item2.Y;
            var ab = Math.Sqrt((x2-x1)*(x2-x1)+(y2-y1)*(y2-y1));
            var ap = Math.Sqrt((x-x1)*(x-x1)+(y-y1)*(y-y1));
            var pb = Math.Sqrt((x2-x)*(x2-x)+(y2-y)*(y2-y));
            return Math.Abs(ab - (ap + pb)) < 1e-9;
        }
    }
}