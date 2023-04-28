using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Aki32Utilities.ConsoleAppUtilities.Structure;

namespace Aki32Utilities.Personal.LowCycleFatigueResearch;
public partial class Module
{
    /// <summary>
    /// SNAPエクセルキラー（初めてキルした後，連続で指定回数止まった時に次の処理に移行！）
    /// </summary>
    public static void M030_KeepClosingExcel(int interval = 5000, int terminateAfterNotKilled = 10)
    {
        Console.WriteLine("Excel閉じるやつ開始！");
        Console.WriteLine();

        bool flag = false;
        int terminateCounter = 0;

        while (true)
        {
            Thread.Sleep(interval);
            Console.WriteLine("監視中…");
            var killed = SNAPHelper.CheckAndCloseExcelOnce();
            if (killed)
            {
                terminateCounter = 0;
                flag = true;
            }
            else
            {
                if (flag == false)
                    continue;

                terminateCounter++;
                Console.WriteLine($"terminateCounter = {terminateCounter}");
                if (terminateCounter >= terminateAfterNotKilled)
                {
                    break;
                }
            }
        }

    }
}
