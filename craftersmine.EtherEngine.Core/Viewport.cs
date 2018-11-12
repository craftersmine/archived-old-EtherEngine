using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.EtherEngine.Core
{
    public sealed class Viewport
    {
        public Transform Transform { get; private set; }

        public Viewport(Transform transform)
        {
            Transform = transform;
        }
    }
}
