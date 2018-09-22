using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.EtherEngine.Utils
{
    /// <summary>
    /// Represents 2-dimentional vector
    /// </summary>
    public struct Vector2
    {
        /// <summary>
        /// Gets X axis coordinate of vector destination point
        /// </summary>
        public int X { get; internal set; }
        /// <summary>
        /// Gets Y axis coordinate of vector destination point
        /// </summary>
        public int Y { get; internal set; }

        /// <summary>
        /// Creates new <see cref="Vector2"/> instance with specified destination point coordinates
        /// </summary>
        /// <param name="x">X axis coordinate of vector destination point</param>
        /// <param name="y">Y axis coordinate of vector destination point</param>
        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Adds two vectors into one
        /// </summary>
        /// <param name="vec1">First <see cref="Vector2"/></param>
        /// <param name="vec2">Second <see cref="Vector2"/></param>
        /// <returns></returns>
        public static Vector2 operator +(Vector2 vec1, Vector2 vec2)
        {
            vec1.X += vec2.X;
            vec1.Y += vec2.Y;
            return vec1;
        }

        /// <summary>
        /// Subtracts one vector from another vector
        /// </summary>
        /// <param name="vec1">Decreasing vector</param>
        /// <param name="vec2">Subtract vector</param>
        /// <returns></returns>
        public static Vector2 operator -(Vector2 vec1, Vector2 vec2)
        {
            vec1.X -= vec2.X;
            vec1.Y -= vec2.Y;
            return vec1;
        }
    }
}
