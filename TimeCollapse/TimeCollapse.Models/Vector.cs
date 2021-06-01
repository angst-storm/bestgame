using System;
using System.Drawing;

namespace TimeCollapse.Models
{
    public class Vector
    {
        private const double Epsilon = 1e-6;
        public static readonly Vector Zero = new(0, 0);

        public readonly double X;
        public readonly double Y;

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector(Point point)
        {
            X = point.X;
            Y = point.Y;
        }

        public Vector((Point, Point) section)
        {
            var (start, end) = section;
            X = end.X - start.X;
            Y = end.Y - start.X;
        }

        public double Length => Math.Sqrt(X * X + Y * Y);

        private static bool DoubleEquals(double a, double b)
        {
            return Math.Abs(a - b) < Epsilon;
        }

        public Point ToPoint()
        {
            return new((int) Math.Ceiling(X), (int) Math.Ceiling(Y));
        }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }

        private bool Equals(Vector other)
        {
            return DoubleEquals(X, other.X) && DoubleEquals(Y, other.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Vector) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        public static Vector operator *(Vector a, double k)
        {
            return new(a.X * k, a.Y * k);
        }

        public static Vector operator *(double k, Vector a)
        {
            return a * k;
        }

        public static Vector operator /(Vector a, double k)
        {
            return new(a.X / k, a.Y / k);
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new(a.X + b.X, a.Y + b.Y);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new(a.X - b.X, a.Y - b.Y);
        }

        public Vector Normalize()
        {
            return Length > 0 ? this * (1 / Length) : this;
        }

        public bool IsZero()
        {
            return Math.Abs(X) + Math.Abs(Y) <= Epsilon;
        }

        public bool Cross(Vector other)
        {
            return Math.Abs(X * other.Y - Y * other.X) > Epsilon;
        }

        public Vector PerpendicularVector(bool clockwise)
        {
            return clockwise ? new Vector(Y, -X) : new Vector(-Y, X);
        }

        public Vector Turn(double angle)
        {
            return new(X * Math.Cos(angle) + Y * Math.Sin(angle), Y * Math.Cos(angle) - X * Math.Sin(angle));
        }
    }
}