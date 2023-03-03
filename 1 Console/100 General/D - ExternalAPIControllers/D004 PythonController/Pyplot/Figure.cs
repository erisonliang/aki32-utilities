

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class PythonController
{
    public partial class PyPlot
    {
        public class Figure
        {

            // ★★★★★★★★★★★★★★★ props

            public string FontName { get; set; } = "MS Gothic";

            public double FigHeight { get; set; } = 10;
            public double FigWidth { get; set; } = 15;

            public double PlotWSpace { get; set; } = 0;
            public double PlotHSpace { get; set; } = 0;

            public bool IsTightLayout { get; set; } = false;

            public List<SubPlot> SubPlots { get; set; }


            // ★★★★★★★★★★★★★★★ methods

            public FileInfo Run(FileInfo outputFile, bool preview = false)
            {
                // preprocess
                if (!Activated)
                    throw new Exception("Required to call PythonController.Initialize() first");
                if (SubPlots is null)
                {
                    throw new Exception("Required to SubPlots");
                }
                outputFile.Directory!.Create();

                // ★★★★★ 全体

                dynamic plt = PythonController.Import("matplotlib.pyplot");

                plt.subplots_adjust(wspace: PlotWSpace, hspace: PlotHSpace);

                dynamic fig = plt.figure(figsize: new double[] { FigWidth, FigHeight });

                // ★★★★★ サブプロット，外側
                foreach (var SubPlot in SubPlots)
                    SubPlot.Run(fig, FontName);


                // ★★★★★ 最後に呼ぶべきもの

                // レイアウト詰め
                if (IsTightLayout)
                    fig.tight_layout();

                // 保存，解放
                plt.savefig(outputFile.FullName);
                plt.clf();
                plt.close();

                if (preview)
                    outputFile.ShowImage_OnDefaultApp(false);

                return outputFile;
            }

            public FileInfo Run(DirectoryInfo outputDir, bool preview = false) => Run(outputDir.GetChildFileInfo("output.png"), preview);


            // ★★★★★★★★★★★★★★★ 

        }
    }
}