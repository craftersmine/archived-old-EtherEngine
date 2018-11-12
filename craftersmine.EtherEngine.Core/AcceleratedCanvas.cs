using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace craftersmine.EtherEngine.Core
{
    public partial class AcceleratedCanvas : UserControl
    {
        internal AcceleratedXamlCanvasBase Canvas { get { return acceleratedCanvasBase; } }

        public AcceleratedCanvas()
        {
            InitializeComponent();
            acceleratedCanvasBase._base.Background = new SolidColorBrush(Color.FromRgb(0,0,0));
        }

        internal void AttachRenderer(Renderer renderer)
        {
            renderer.AcceleratedCanvas = this;
        }
    }
}
