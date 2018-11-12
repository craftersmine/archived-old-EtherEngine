using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace craftersmine.EtherEngine.Input
{
    [Serializable]
    public sealed class MouseEventArguments
    {
        public MouseButtonState LeftButtonState { get; set; }
        public MouseButtonState RightButtonState { get; set; }
        public MouseButtonState MiddleButtonState { get; set; }
        public double XPosition { get; set; }
        public double YPosition { get; set; }

        public override string ToString()
        {
            return "LB: " + LeftButtonState + " | RB: " + RightButtonState + " | MB: " + MiddleButtonState + " | X = " + XPosition + " | Y = " + YPosition;
        }
    }

    [Serializable]
    public sealed class MouseWheelEventArguments
    {
        public MouseWheelRotationType MouseWheelRotationType { get; set; }
        public double XPosition { get; set; }
        public double YPosition { get; set; }

        public override string ToString()
        {
            return "WRT: " + MouseWheelRotationType + " | X = " + XPosition + " | Y = " + YPosition;
        }
    }

    [Serializable]
    public enum MouseWheelRotationType
    {
        NoRotation, Forward, Backward, ToUser = Backward, FromUser = Forward
    }
}
