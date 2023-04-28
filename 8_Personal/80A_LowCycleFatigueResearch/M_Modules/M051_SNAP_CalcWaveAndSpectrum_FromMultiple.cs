using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.ConsoleAppUtilities.Structure;

namespace Aki32Utilities.Personal.LowCycleFatigueResearch;
public partial class Module
{
    public static void M051_SNAP_CalcWaveAndSpectrum_FromMultiple(DirectoryInfo baseDir, Dictionary<string, string>? IdAndDisplayNameRelationalInfo = null)
    {
        // ★★★★★ defs
        var snapWavesDir = baseDir.GetChildDirectoryInfo(@"SnapWaves");

        var snapWaveFiles = snapWavesDir
            .GetFilesWithRegexen(SearchOption.AllDirectories, @$".*\.wv$")
            .Where(x =>
            {
                if (IdAndDisplayNameRelationalInfo == null)
                    return true;
                return IdAndDisplayNameRelationalInfo.ContainsKey(Path.GetFileNameWithoutExtension(x.Name));
            })
            .ToArray();

        var wavesDir = snapWavesDir.GetChildDirectoryInfo(@"SimpleWaves").CreateAndPipe();
        var waveImagesDir = snapWavesDir.GetChildDirectoryInfo(@"SimpleWavesImage").CreateAndPipe();
        var spectrumDir = snapWavesDir.GetChildDirectoryInfo(@"Spectrum").CreateAndPipe();
        var spectrumImageDir = snapWavesDir.GetChildDirectoryInfo(@"SpectrumImage").CreateAndPipe();


        // ★★★★★ main

        using var progress = new ProgressManager(snapWaveFiles.Length);
        progress.StartAutoWrite(100);

        for (int i = 0; i < snapWaveFiles.Length; i++)
        {
            var input = snapWaveFiles[i];
            var snap = new SNAPHelper.SNAPWaveData(input);

            // ★★★★★ waves
            var wave = snap.Accs;
            {
                var waveFile = wavesDir.GetChildFileInfo(snap.FILENAME + ".csv");
                var waveImageFile = waveImagesDir.GetChildFileInfo(snap.FILENAME + ".png");

                wave.SaveToCsv(waveFile);
            }

            // ★★★★★ spectra
            {
                var spectraFile = spectrumDir.GetChildFileInfo(snap.FILENAME + ".xlsx");
                var spectraImageFile = spectrumImageDir.GetChildFileInfo(snap.FILENAME + ".png");

                var ep = new ElasticModel(2);
                var TList = Enumerable.Range(1, 1000).Select(x => x / 100d).ToArray();
                var hList = new double[] { 0.05 };
                //var hList = new double[] { 0.00, 0.03, 0.05, 0.10 };
                var waveAnalysisModel = new NewmarkBetaModel(0.25d);
                var spectrumResultSet = SDoFModel.CalcResponseSpectrum(TList, hList, wave, waveAnalysisModel, ep);
                spectrumResultSet.SaveToExcel(spectraFile);
            }

            progress.CurrentStep++;
        }

        progress.WriteDone();

    }
}
