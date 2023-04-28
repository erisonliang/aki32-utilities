using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.ConsoleAppUtilities.Structure;

namespace Aki32Utilities.Personal.LowCycleFatigueResearch;
public partial class Module
{
    public static void Small()
    {

        // FFT 
        var input = new FileInfo($@"C:\Users\aki32\Dropbox\Documents\02 東大関連\1 研究室\77 別プロジェクト\SNAP解析\e-defenseモデル\calc\計算\フーリエ\exp40_X1.csv");
        TimeHistory
            .FromCsv(input, new string[] { "t", "x" })
            .FFT("x")
            .SaveToCsv();


    }
}
