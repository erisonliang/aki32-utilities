using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.ConsoleAppUtilities.Structure;

namespace Aki32Utilities.Personal.LowCycleFatigueResearch;
public partial class Module
{
    public static void M053_SNAP_CalcWaveAndSpectrum_FromSingle(FileInfo originalWaveFile, List<(int PGV, double Amp)> PGVsAndAmpsList)
    {
        // ★★★★★ defs
        var outputDir = originalWaveFile.Directory!.GetChildDirectoryInfo(originalWaveFile.Name).CreateAndPipe();


        // ★★★★★ main
        var snap = new SNAPHelper.SNAPWaveData(originalWaveFile);
        var wave = snap.Accs;


        // ★★★★★ waves
        {
            var waveCsvFile = outputDir.GetChildFileInfo("Waves.csv");
            var allWaves = new TimeHistory();

            using var progress = new ProgressManager(PGVsAndAmpsList.Count);
            progress.StartAutoWrite(100);

            foreach (var (PGV, Amp) in PGVsAndAmpsList)
            {
                var index = PGV.ToString();
                if (allWaves.Columns.Length == 0)
                    allWaves["t"] = wave[0];
                allWaves[index] = wave[1].Select(a => a * Amp).ToArray();

                progress.CurrentStep++;
            }

            allWaves.SaveToCsv(waveCsvFile);
            progress.WriteDone();
        }


        // ★★★★★ spectra
        {
            var SvsCsvFile = outputDir.GetChildFileInfo("Svs.csv");
            var allSvs = new TimeHistory();

            using var progress = new ProgressManager(PGVsAndAmpsList.Count);
            progress.StartAutoWrite(100);

            foreach (var (PGV, Amp) in PGVsAndAmpsList)
            {
                var index = PGV.ToString();
                var newWave = new TimeHistory()
                {
                    t = wave.t,
                    ytt = wave.ytt.Select(a => a * Amp).ToArray()
                };

                var ep = new ElasticModel(2);
                var TList = Enumerable.Range(1, 1000).Select(x => x / 100d).ToArray();
                var hList = new double[] { 0.05 };
                //var hList = new double[] { 0.00, 0.03, 0.05, 0.10 };
                var waveAnalysisModel = new NewmarkBetaModel(0.25d);
                var spectrumResultSet = SDoFModel.CalcResponseSpectrum(TList, hList, newWave, waveAnalysisModel, ep);
                var Sv = spectrumResultSet[1];
                Sv.RenameColumn(Sv.Columns[1], "Sv");

                if (allSvs.Columns.Length == 0)
                    allSvs["T"] = Sv[0];
                allSvs[index] = Sv[1];

                progress.CurrentStep++;
            }

            allSvs.SaveToCsv(SvsCsvFile);
            progress.WriteDone();
        }

    }
}
