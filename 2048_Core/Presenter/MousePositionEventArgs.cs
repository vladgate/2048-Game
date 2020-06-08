using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048_Core.Presenter
{
    public sealed class MousePositionEventArgs : EventArgs
    {
        public double X { get; set; }
        public double Y { get; set; }
        public MousePositionEventArgs(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
