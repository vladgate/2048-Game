using _2048_Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _2048_WPF
{
    internal sealed class WpfMessageService : IMessageService
    {
        public void ShowError(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public void ShowMessage(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public bool WantNewGame(string text, string caption)
        {
            MessageBoxResult dr = MessageBox.Show(text, caption, MessageBoxButton.YesNo);
            if (dr == MessageBoxResult.Yes)
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
