using _2048_Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2048_WF
{
    internal sealed class MessageService : IMessageService
    {
        public void ShowError(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ShowMessage(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public bool WantNewGame(string text, string caption)
        {
            DialogResult dr = MessageBox.Show(text, caption, MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
