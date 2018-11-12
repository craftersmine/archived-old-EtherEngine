using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.EtherEngine.Core
{
    public struct Vector2
    {
        public double X { get; private set; }
        public double Y { get; private set; }

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static readonly Vector2 Empty = new Vector2(0d, 0d);
    }
}
