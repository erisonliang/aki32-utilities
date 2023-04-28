using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;

using static Aki32Utilities.ConsoleAppUtilities.General.TimeHistory;

namespace Aki32Utilities.Personal.LowCycleFatigueResearch;
public partial class Module
{
    public static void M052_SNAP_DrawGraphs_FromMultiple(DirectoryInfo baseDir,
        Dictionary<string, string>? IdAndDisplayNameRelationalInfo = null,
        string FileNamePrefix = "All",
        bool executeSimpleWave = true,
        bool executeSpectra = true
        )
    {
        // ★★★★★ defs
        var snapWavesDir = baseDir.GetChildDirectoryInfo(@"SnapWaves");


        // ★★★★★ wave
        if (executeSimpleWave)
        {
            var allWaves = new TimeHistory();

            var wavesDir = snapWavesDir.GetChildDirectoryInfo(@"SimpleWaves").CreateAndPipe();
            var waveImageFile = snapWavesDir.GetChildDirectoryInfo(@"SimpleWavesImage").CreateAndPipe().GetChildFileInfo($"★ {FileNamePrefix}Waves.png");
            var targetWaveFiles = wavesDir
                .GetFilesWithRegexen(@$".*\.csv$")
                .ToArray();

            if (IdAndDisplayNameRelationalInfo is not null)
            {
                // PGV大きい順に並べる。
                targetWaveFiles = IdAndDisplayNameRelationalInfo
                    .Keys
                    .Select(k => wavesDir.GetChildFileInfo($"{k}.csv"))
                    .ToArray();
            }

            using var progress = new ProgressManager(targetWaveFiles.Length);
            progress.StartAutoWrite(100);

            foreach (var targetWaveFile in targetWaveFiles)
            {
                string index = Path.GetFileNameWithoutExtension(targetWaveFile.Name);
                if (IdAndDisplayNameRelationalInfo != null)
                    index = IdAndDisplayNameRelationalInfo[index];

                var wave = FromCsv(targetWaveFile);
                wave.DrawGraph_OnPyPlot(waveImageFile, "t", "ytt", TimeHistory.ChartType.Line, preview: false);
                if (allWaves.Columns.Length == 0)
                    allWaves["t"] = wave[0];
                allWaves[index] = wave[1];

                progress.CurrentStep++;
            }
            {
                var xLabel = "t [s]";
                var yLabel = "a [cm/s²]";

                var x = allWaves["t"];
                var plots = new List<PyPlotWrapper.IPlot>();

                var i = 0;
                foreach (var targetWaveFile in targetWaveFiles)
                {
                    string index = Path.GetFileNameWithoutExtension(targetWaveFile.Name);
                    if (IdAndDisplayNameRelationalInfo != null)
                        index = IdAndDisplayNameRelationalInfo[index];

                    var y = allWaves[index].Select(x => x - i * 400).ToArray();

                    plots.Add(new PyPlotWrapper.LinePlot(x, y)
                    {
                        LegendLabel = index,
                        LineWidth = 1,
                        LineColor = null,
                        Marker = null,
                    });

                    i++;
                }

                new PyPlotWrapper.Figure
                {
                    IsTightLayout = true,
                    SubPlots = new List<PyPlotWrapper.SubPlot>()
                    {
                        new PyPlotWrapper.SubPlot()
                        {
                            XLabel = xLabel,
                            YLabel = yLabel,
                            Title = null,
                            Plots = plots,
                        }
                    }
                }.Run(waveImageFile, preview: false);
            }

            progress.WriteDone();

        }


        // ★★★★★ spectra
        if (executeSpectra)
        {
            var allSpectra = new TimeHistory();

            var spectrumDir = snapWavesDir.GetChildDirectoryInfo(@"Spectrum").CreateAndPipe();
            var spectraImageFile = snapWavesDir.GetChildDirectoryInfo(@"SpectrumImage").CreateAndPipe().GetChildFileInfo($"★ {FileNamePrefix}Spectra.png");

            var targetSpectraFiles = spectrumDir
                .GetFilesWithRegexen(@$".*\.xlsx$")
                .ToArray();

            if (IdAndDisplayNameRelationalInfo is not null)
            {
                targetSpectraFiles = IdAndDisplayNameRelationalInfo
                    .Keys
                    .Select(k => spectrumDir.GetChildFileInfo($"{k}.xlsx"))
                    .ToArray();
            }

            using var progress = new ProgressManager(targetSpectraFiles.Length);
            progress.StartAutoWrite(100);

            foreach (var targetSpectraFile in targetSpectraFiles)
            {
                string index = Path.GetFileNameWithoutExtension(targetSpectraFile.Name);
                if (IdAndDisplayNameRelationalInfo != null)
                    index = IdAndDisplayNameRelationalInfo[index];

                var Sv = FromExcel(targetSpectraFile, "Sv");
                Sv.DrawGraph_OnPyPlot(spectraImageFile, "T", Sv.Columns[1], ChartType.Line, preview: false);
                if (allSpectra.Columns.Length == 0)
                    allSpectra["T"] = Sv[0];
                allSpectra[index] = Sv[1];

                progress.CurrentStep++;
            }
            {
                allSpectra.DrawGraph_OnPyPlot(spectraImageFile, "T", allSpectra.Columns.Where(x => x != "T").ToArray(),
                    type: ChartType.Line,
                    xLabel: "T [s]",
                    yLabel: "Sv [cm]",
                    preview: true
                    );
            }

            progress.WriteDone();
        }

    }
}
