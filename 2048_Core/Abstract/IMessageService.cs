using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048_Core.Abstract
{
    public interface IMessageService
    {
        void ShowMessage(string message, string caption);
        void ShowError(string message, string caption);
        bool WantNewGame(string text, string caption);

    }
}
