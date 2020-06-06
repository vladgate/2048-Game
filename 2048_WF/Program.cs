using _2048_Core;
using _2048_Core.Presenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2048_WF
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm mainForm = new MainForm();
            Presenter presenter = new Presenter(mainForm, new MessageService());
            Application.Run(mainForm);
        }
    }
}
