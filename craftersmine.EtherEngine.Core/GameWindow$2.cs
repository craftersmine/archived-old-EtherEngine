using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using craftersmine.EtherEngine.Input;

namespace craftersmine.EtherEngine.Core
{
    public partial class GameWindow
    {
        public virtual void OnMouseUp(MouseEventArguments args)
        { }

        public virtual void OnMouseDown(MouseEventArguments args)
        { }

        public virtual void OnMouseHover(MouseEventArguments args)
        { }

        public virtual void OnMouseWheel(MouseWheelEventArguments args)
        { }
    }
}
