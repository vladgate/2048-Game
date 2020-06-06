using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048_Core.Presenter
{
    public sealed class GameParametersEventArgs : EventArgs
    {
        public byte Width { get; set; }
        public byte Height { get; set; }
        public GameParametersEventArgs(byte width, byte height)
        {
            Width = width;
            Height = height;
        }
    }
}
