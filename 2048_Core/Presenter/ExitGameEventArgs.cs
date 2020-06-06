using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048_Core.Presenter
{
    public sealed class ExitGameEventArgs : EventArgs
    {
        public bool NeedCloseView { get; set; } // при выходе нужно закрыть форму
        public ExitGameEventArgs(bool needCloseView)
        {
            NeedCloseView = needCloseView;
        }
    }
}
