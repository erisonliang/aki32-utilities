using System;
using System.Windows.Forms;

namespace Aki32Utilities.WinFormAppUtilities.CheatSheet
{
    internal static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form_OxyPlot());
        }
    }
}
