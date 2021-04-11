using System;
using System.Drawing;

namespace TimeCollapse.Models
{
    public class VectorF
    {
        public readonly float X;
        public readonly float Y;

        public VectorF(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float Length => (float) Math.Sqrt(X * X + Y * Y);
        public float Angle => (float) Math.Atan2(Y, X);
        public static VectorF Zero => new(0, 0);

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }

        public bool Equals(VectorF other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            return !ReferenceEquals(null, obj) &&
                   (ReferenceEquals(this, obj) ||
                    obj.GetType() == GetType() &&
                    Equals((VectorF) obj));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        public static VectorF operator -(VectorF a, VectorF b)
        {
            return new(a.X - b.X, a.Y - b.Y);
        }

        public static VectorF operator *(VectorF a, float k)
        {
            return new(a.X * k, a.Y * k);
        }

        public static VectorF operator /(VectorF a, float k)
        {
            return new(a.X / k, a.Y / k);
        }

        public static VectorF operator *(float k, VectorF a)
        {
            return a * k;
        }

        public static VectorF operator +(VectorF a, VectorF b)
        {
            return new(a.X + b.X, a.Y + b.Y);
        }
        
        public static PointF operator +(PointF a, VectorF b)
        {
            return new(a.X + b.X, a.Y + b.Y);
        }
        
        public static PointF operator +(VectorF a, PointF b)
        {
            return b + a;
        }

        public VectorF Normalize()
        {
            return Length > 0 ? this * (1 / Length) : this;
        }
    }
}