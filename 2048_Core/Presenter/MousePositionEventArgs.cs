using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048_Core.Presenter
{
    public sealed class MousePositionEventArgs : EventArgs
    {
        public int X { get; set; }
        public int Y { get; set; }
        public MousePositionEventArgs(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
