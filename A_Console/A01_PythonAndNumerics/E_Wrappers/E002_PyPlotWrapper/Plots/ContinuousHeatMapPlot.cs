

using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class PyPlotWrapper
{
    /// <summary>
    /// 2D
    /// </summary>
    public class ContinuousHeatMapPlot : IPlot
    {

        // ★★★★★★★★★★★★★★★ props

        public bool Is3D { get; set; } = true;
        public string LegendLabel { get; set; } = "";
        public double Alpha { get; set; } = 1;

        public dynamic X { get; set; }
        public dynamic Y { get; set; }
        public dynamic Z { get; set; }

        public string ColorMap { get; set; } = "coolwarm"; //Blues
        public (double? min, double? max) ColorLim { get; set; } = (null, null);

        public bool OverwriteXAxisTickTop { get; set; } = false;
        public bool OverwriteYAxisInvert { get; set; } = false;

        public bool UseColorBar { get; set; } = true;
        public string ColorBarOrientation { get; set; } = "vertical";

        // ★★★★★★★★★★★★★★★ inits

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">T[][], T[,] or NDArray. T = float or double</param>
        /// <param name="y">same as x format</param>
        /// <param name="z">same as x format</param>
        public ContinuousHeatMapPlot(dynamic x, dynamic y, dynamic z)
        {
            try
            {
                X = NumpyWrapper.ToCorrect2DNDArray<float>(x);
                Y = NumpyWrapper.ToCorrect2DNDArray<float>(y);
                Z = NumpyWrapper.ToCorrect2DNDArray<float>(z);
            }
            catch (Exception)
            {
                X = NumpyWrapper.ToCorrect2DNDArray<double>(x);
                Y = NumpyWrapper.ToCorrect2DNDArray<double>(y);
                Z = NumpyWrapper.ToCorrect2DNDArray<double>(z);
            }

            Is3D = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z">/param>
        public ContinuousHeatMapPlot(double[] x, double[] y, double[,] z)
        {
            (X, Y) = NumpyWrapper.GetMeshGrid(x, y);
            Z = NumpyWrapper.ToCorrect2DNDArray<double>(z);

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
            var heatmap = ax.pcolormesh(X, Y, Z,
                 label: LegendLabel,
                 alpha: Alpha,

                 cmap: ColorMap,
                 vmin: ColorLim.min,
                 vmax: ColorLim.max
                 );

            if (UseColorBar)
                fig.colorbar(heatmap,
                    ax: ax,
                    orientation: ColorBarOrientation
                    );

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
                    Title = "continuous heatmap",
                    HasGrid = false,
                    Plot = new ContinuousHeatMapPlot(X, Y, Z)
                    {
                        ColorMap = "cividis",
                    },
                }
            }.Run(outputImageFile, preview);
        }

        public static void RunExampleModel(FileInfo outputImageFile, bool preview = true)
        {
            var pi = Math.PI;
            var n = 333;
            var XX = EnumerableExtension.Range_WithCount(0, 5, n).ToArray();
            var YY = EnumerableExtension.Range_WithCount(0, 5, n).ToArray();
            var ZZ = Enumerable
                .SelectMany(XX, x => YY, (x, y) =>
                {
                    return (x * y);
                })
                .ToArray().ReShape(n, n);

            // ★★★★★ 
            new PyPlotWrapper.Figure()
            {
                IsTightLayout = true,
                SubPlot = new PyPlotWrapper.SubPlot()
                {
                    XLabel = "X",
                    YLabel = "Y",
                    Title = "continuous heatmap",
                    HasGrid = false,
                    Plot = new PyPlotWrapper.ContinuousHeatMapPlot(XX, YY, ZZ)
                    {
                        ColorMap = "cividis",
                    },
                }
            }.Run(outputImageFile, preview);
        }


        // ★★★★★★★★★★★★★★★ 

    }
}
