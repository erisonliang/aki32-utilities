using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class PyPlotWrapper
{
    /// <summary>
    /// 2D
    /// </summary>
    public class FillBetweenPlot : IPlot
    {
        // ★★★★★★★★★★★★★★★ props

        public bool Is3D { get; set; } = false;
        public string LegendLabel { get; set; } = "";
        public double Alpha { get; set; } = 1;

        public double[] X { get; set; }
        public double[] Y1 { get; set; }
        public double[] Y2 { get; set; }

        public bool[] Where { get; set; } = null;

        public string FillColor { get; set; } = "black";
        public string Hatch { get; set; } = null; // "//"


        // ★★★★★★★★★★★★★★★ inits

        public FillBetweenPlot(double[]? x, double[] y1, double[] y2, bool[]? where = null)
        {
            X = x;
            Y1 = y1;
            Y2 = y2;
            Where = where;
            if (where is not null)
                throw new NotImplementedException("sorry not implemented");
            Is3D = false;
        }


        // ★★★★★★★★★★★★★★★ methods

        public void Run(dynamic fig, dynamic ax, string FontName)
        {
            // プロット
            ax.fill_between(X, Y1, Y2,

                label: LegendLabel,
                alpha: Alpha,

                //where: Where,
                hatch: Hatch,

                color: FillColor
                );

        }


        // ★★★★★★★★★★★★★★★ methods (static)

        public static FileInfo DrawSimpleGraph(double[] X, double[] Y1, double[] Y2, FileInfo? outputImageFile = null, bool preview = true)
        {
            outputImageFile ??= new FileInfo(Path.GetTempFileName().GetExtensionChangedFilePath(".png"));

            return new Figure
            {
                IsTightLayout = true,
                SubPlot = new SubPlot()
                {
                    XLabel = "x",
                    YLabel = "y",
                    Title = "fill between",
                    Plot = new FillBetweenPlot(X, Y1, Y2) { Alpha = 0.6 },
                }
            }.Run(outputImageFile, preview);
        }

        public static void RunExampleModel(FileInfo outputImageFile, bool preview = true)
        {
            var pi = Math.PI;
            var n = 256;

            var x = EnumerableExtension.Range_WithCount(-10, 10, 100).ToArray();
            var y1 = x.Select(x => -0.1 * (x * x) + 10).ToArray();
            var y2 = x.Select(x => 0.05 * (x * x) + 1).ToArray();

            new PyPlotWrapper.Figure
            {
                IsTightLayout = true,
                SubPlot = new PyPlotWrapper.SubPlot()
                {
                    XLabel = "x",
                    YLabel = "y",
                    Title = "fill between",
                    Plot = new PyPlotWrapper.FillBetweenPlot(x, y1, y2) { Alpha = 0.6, Hatch = "o" },
                }
            }.Run(outputImageFile, preview);

        }


        // ★★★★★★★★★★★★★★★ 




    }
}
