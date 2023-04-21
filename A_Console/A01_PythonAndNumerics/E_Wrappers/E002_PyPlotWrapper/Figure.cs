using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class PyPlotWrapper
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

        public SubPlot SubPlot { get; set; }
        public List<SubPlot> SubPlots { get; set; }


        // ★★★★★★★★★★★★★★★ init

        public Figure(bool InitFor3D = false)
        {
            if (InitFor3D)
            {
                FigHeight = 15;
                FigWidth = 15;
            }
        }


        // ★★★★★★★★★★★★★★★ methods

        public FileInfo Run(FileInfo outputFile, bool preview = false)
        {
            // ★★★★★ preprocess
            if (!PythonController.Activated)
                throw new Exception("Required to call PythonController.Initialize() first");

            if (SubPlot is not null)
                SubPlots = new List<SubPlot> { SubPlot };
            if (SubPlots is null)
                throw new Exception("Required to set either SubPlot or SubPlots");
            outputFile.Directory!.Create();


            // ★★★★★ 全体
            dynamic plt = PythonController.Import("matplotlib.pyplot");
            plt.subplots_adjust(wspace: PlotWSpace, hspace: PlotHSpace);
            dynamic fig = plt.figure(figsize: new double[] { FigWidth, FigHeight });


            // ★★★★★ サブプロット
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
