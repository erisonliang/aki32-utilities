using System;
using System.Windows.Forms;

namespace Aki32Utilities.UsageExamples.WinFormControlExample
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
