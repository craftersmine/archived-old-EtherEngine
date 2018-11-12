﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.EtherEngine.Core
{
    public sealed class Transform
    {
        public Rectangle Bounds { get; set; }
        public Vector2 Position { get; set; }

        public Transform(Rectangle bounds, Vector2 position)
        {
            Bounds = bounds;
            Position = position;
        }

        public Transform(Rectangle bounds) : this(bounds, Vector2.Empty)
        {}
    }
}