using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.ConsoleAppUtilities.Structure;

namespace Aki32Utilities.Personal.LowCycleFatigueResearch;
public partial class Module
{
    public static void M040_EQResponse()
    {
        var baseDir = @"C:\Users\aki32\Dropbox\Apps\建築系\SNAP（任意形状の解析）\SNAP共有データ\SharedWave\元csv";

        var waveNames = new string[]{
            "Kenken-KA1",
            //"Kenken-SZ1",
            //"Kenken-SZ2",
            //"Kenken-SZ3",
            //"Kenken-OS1",
            //"Kenken-OS2",
            //"Kenken-OS3",
            //"Kenken-CH1",
            //"Kenken-CH2",
            //"Kenken-CH3",
            //"Trough4-E4E",
        };

        foreach (var waveName in waveNames)
        {
            var waveCsv = new FileInfo($@"{baseDir}\{waveName}.csv");

            var wave = TimeHistory.FromCsv(waveCsv, new string[] { "t", "ytt" });

            // 地震動 変位 → 速度・加速度
            //wave.CalcIntegral_Simple("ytt", "yt", "y");
            //wave.SaveToCsv(waveCsv.GetRenamedFileInfo("*-Integrated"));

            // スペクトル出力
            var ep = new ElasticModel(2);
            var TList = Enumerable.Range(1, 1000).Select(x => x / 100d).ToArray();
            //var hList = new double[] { 0.05 };
            var hList = new double[] { 0.00, 0.03, 0.05, 0.10 };

            var waveAnalysisModel = new NewmarkBetaModel(0.25d);
            var spectrumResultSet = SDoFModel.CalcResponseSpectrum(TList, hList, wave, waveAnalysisModel, ep);
            spectrumResultSet.SaveToExcel(waveCsv.GetRenamedFileInfo("*-Spectrum").GetExtensionChangedFileInfo(".xlsx"));

        }

    }
}
