

using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class PyPlotWrapper
{
    /// <summary>
    /// 2D
    /// </summary>
    public class GridHeatMapPlot : IPlot
    {

        // ★★★★★★★★★★★★★★★ props

        public bool Is3D { get; set; } = true;
        public string LegendLabel { get; set; } = "";
        public double Alpha { get; set; } = 1;

        public string[] X { get; set; }
        public string[] Y { get; set; }
        public dynamic Z { get; set; }

        public string Interpolation { get; set; } = "nearest";
        public string ColorMap { get; set; } = "coolwarm"; //Blues
        public (double? min, double? max) ColorLim { get; set; } = (null, null);

        public bool OverwriteXAxisTickTop { get; set; } = true;
        public bool OverwriteYAxisInvert { get; set; } = false;

        public bool UseColorBar { get; set; } = true;
        public string ColorBarOrientation { get; set; } = "vertical";

        public double? XUpperThreshold { get; set; } = null;
        public double? XLowerThreshold { get; set; } = null;

        // ★★★★★★★★★★★★★★★ inits

        /// <summary>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z">/param>
        public GridHeatMapPlot(double[] x, double[] y, double[,] z)
        {
            X = x.Select(s => s.ToString()).ToArray();
            Y = y.Select(s => s.ToString()).ToArray();
            Z = NumpyWrapper.ToCorrect2DNDArray<double>(z);

            Is3D = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z">/param>
        public GridHeatMapPlot(int[] x, int[] y, double[,] z)
        {
            X = x.Select(s => s.ToString()).ToArray();
            Y = y.Select(s => s.ToString()).ToArray();
            Z = NumpyWrapper.ToCorrect2DNDArray<double>(z);

            Is3D = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z">/param>
        public GridHeatMapPlot(string[] x, string[] y, float[,] z)
        {
            X = x;
            Y = y;
            Z = NumpyWrapper.ToCorrect2DNDArray<float>(z);

            Is3D = false;
        }


        // ★★★★★★★★★★★★★★★ methods

        public void Run(dynamic fig, dynamic ax, string FontName)
        {
            // 前準備
            if (OverwriteYAxisInvert)
                ax.invert_yaxis();
            if (OverwriteXAxisTickTop)
                ax.xaxis.tick_top();


            // プロット
            var heatmap = ax.imshow(Z,
                 label: LegendLabel,
                 alpha: Alpha,

                 cmap: ColorMap,
                 vmin: ColorLim.min,
                 vmax: ColorLim.max,

                 interpolation: Interpolation
                 );

            if (UseColorBar)
                fig.colorbar(heatmap,
                    ax: ax,
                    orientation: ColorBarOrientation
                    );

            ax.set_xticks(Enumerable.Range(0, X.Length).ToArray());
            if (X is not null)
                ax.set_xticklabels(X, minor: true);

            ax.set_yticks(Enumerable.Range(0, Y.Length).ToArray());
            if (Y is not null)
                ax.set_yticklabels(Y, minor: true);

        }


        // ★★★★★★★★★★★★★★★ methods (static)

        public static FileInfo DrawSimpleGraph(double[] X, double[] Y, double[,] Z, FileInfo? outputImageFile = null, bool preview = true)
        {
            outputImageFile ??= new FileInfo(Path.GetTempFileName().GetExtensionChangedFilePath(".png"));

            return new Figure()
            {
                IsTightLayout = true,
                SubPlot = new SubPlot()
                {
                    XLabel = "X",
                    YLabel = "Y",
                    Title = "grid heatmap",
                    HasGrid = false,
                    Plot = new GridHeatMapPlot(X, Y, Z)
                    {
                        ColorMap = "cividis",
                    },
                }
            }.Run(outputImageFile, preview);
        }

        public static void RunExampleModel(FileInfo? outputImageFile = null, bool preview = true)
        {
            outputImageFile ??= new FileInfo(Path.GetTempFileName().GetExtensionChangedFilePath(".png"));

            var pi = Math.PI;
            var n = 10;
            var XX = EnumerableExtension.Range_WithStep(0, n - 1, 1).ToArray();
            var YY = EnumerableExtension.Range_WithStep(0, n - 1, 1).ToArray();
            var ZZ = Enumerable
                .SelectMany(XX, x => YY, (x, y) =>
                {
                    return (x * y);
                })
                .ToArray().ReShape(n, n);

            new PyPlotWrapper.Figure()
            {
                IsTightLayout = true,
                SubPlot = new PyPlotWrapper.SubPlot()
                {
                    XLabel = "X",
                    YLabel = "Y",
                    Title = "grid heatmap",
                    HasGrid = false,
                    Plot = new PyPlotWrapper.GridHeatMapPlot(XX, YY, ZZ)
                    {
                        ColorMap = "cividis",
                    },
                }
            }.Run(outputImageFile, preview);

        }


        // ★★★★★★★★★★★★★★★ 

    }
}
