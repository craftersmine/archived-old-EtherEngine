using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.EtherEngine.Utils
{
    public struct Vector2
    {
        public int X { get; internal set; }
        public int Y { get; internal set; }

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 operator +(Vector2 vec1, Vector2 vec2)
        {
            vec1.X += vec2.X;
            vec1.Y += vec2.Y;
            return vec1;
        }

        public static Vector2 operator -(Vector2 vec1, Vector2 vec2)
        {
            vec1.X -= vec2.X;
            vec1.Y -= vec2.Y;
            return vec1;
        }
    }
}
