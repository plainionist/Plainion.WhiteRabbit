using System;
using System.Windows.Forms;
using Plainion.WhiteRabbit.Presentation;

namespace Plainion.WhiteRabbit
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var ctrl = new Controller(typeof(MainUI));

            Application.Run(ctrl.MainView as Form);
        }
    }
}