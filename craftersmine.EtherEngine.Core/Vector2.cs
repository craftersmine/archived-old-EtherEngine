using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.EtherEngine.Core
{
    public struct Vector2
    {
        [DefaultValue(0d)]
        public double X { get; private set; } 
        [DefaultValue(0d)]
        public double Y { get; private set; }

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector2(double x)
        {
            X = x;
            Y = 0;
        }

        public static Vector2 operator +(Vector2 vec1, Vector2 vec2)
        {
            return new Vector2(vec1.X + vec2.X, vec1.Y + vec2.Y);
        }

        public static Vector2 operator -(Vector2 vec1, Vector2 vec2)
        {
            return new Vector2(vec1.X - vec2.X, vec1.Y - vec2.Y);
        }

        public static double Scalar(Vector2 vec1, Vector2 vec2)
        {
            return (vec1.X * vec2.X) + (vec1.Y * vec2.Y);
        }

        public static Vector2 MultiplyOnNumber(Vector2 vec, double factor)
        {
            return new Vector2(vec.X * factor, vec.Y * factor);
        }

        public static readonly Vector2 Empty = new Vector2(0d, 0d);
    }
}
