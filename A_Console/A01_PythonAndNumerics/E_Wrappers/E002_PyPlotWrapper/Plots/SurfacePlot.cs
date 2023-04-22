

using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class PyPlotWrapper
{
    /// <summary>
    /// 3D
    /// </summary>
    public class SurfacePlot : IPlot
    {

        // ★★★★★★★★★★★★★★★ props

        public bool Is3D { get; set; } = true;
        public string LegendLabel { get; set; } = "";
        public double Alpha { get; set; } = 1;

        public dynamic X { get; set; }
        public dynamic Y { get; set; }
        /// <summary>
        /// (3d option)
        /// </summary>
        public dynamic Z { get; set; }


        public int LineWidth { get; set; } = 0;

        public string Color { get; set; } = "black";
        public string ColorMap { get; set; } = "coolwarm"; //Blues

        public bool AntiAliased { get; set; } = true;

        //public double Normalize { get; set; }
        //public double UpperThreshold { get; set; }
        //public double LowerThreshold { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">T[][], T[,] or NDArray. T = float or double</param>
        /// <param name="y">same as x format</param>
        /// <param name="z">same as x format</param>
        public SurfacePlot(dynamic x, dynamic y, dynamic z)
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

            Is3D = true;
        }

        /// <summary>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z">/param>
        public SurfacePlot(double[] x, double[] y, double[,] z)
        {
            (X, Y) = NumpyWrapper.GetMeshGrid(x, y);
            Z = NumpyWrapper.ToCorrect2DNDArray<double>(z);

            Is3D = true;
        }


        // ★★★★★★★★★★★★★★★ methods

        public void Run(dynamic fig, dynamic ax, string FontName)
        {
            // プロット
            ax.plot_surface(X, Y, Z,
                label: LegendLabel,
                alpha: Alpha,

                linewidth: LineWidth,
                cmap: ColorMap,
                color: Color,

                antialiased: AntiAliased
                );

        }

        // ★★★★★★★★★★★★★★★ methods (static)

        public static FileInfo DrawSimpleGraph(double[] X, double[] Y, double[,] Z, FileInfo? outputImageFile = null, bool preview = true)
        {
            outputImageFile ??= new FileInfo(Path.GetTempFileName().GetExtensionChangedFilePath(".png"));

            return new PyPlotWrapper.Figure(true)
            {
                IsTightLayout = true,
                SubPlot = new PyPlotWrapper.SubPlot(true)
                {
                    //ZLim=(-1,1),
                    XLabel = "X",
                    YLabel = "Y",
                    ZLabel = "Z",
                    Title = "surface",
                    Plot = new SurfacePlot(X, Y, Z) { ColorMap = "cividis", Alpha = 0.5 },
                }
            }.Run(outputImageFile, preview);
        }

        public static void RunExampleModel(FileInfo outputImageFile, bool preview = true)
        {
            var pi = Math.PI;
            var n = 50;

            var XX = EnumerableExtension.Range_WithCount(-4, 4, n).ToArray();
            var YY = EnumerableExtension.Range_WithCount(-4, 4, n).ToArray();
            var ZZ = Enumerable
                .SelectMany(XX, x => YY, (x, y) =>
                {
                    var Z1 = Math.Exp(-Math.Pow(x, 2) - Math.Pow(y, 2));
                    var Z2 = Math.Exp(-Math.Pow(x - 1.2, 2) - Math.Pow(y - 0.7, 2));
                    var Z3 = Math.Exp(-Math.Pow(x + 0.5, 2) - Math.Pow(y + 1.4, 2));
                    return (Z1 - Z2 - Z3) * 2;
                })
                .ReShape(n, n);

            new PyPlotWrapper.Figure(true)
            {
                IsTightLayout = true,
                SubPlot = new PyPlotWrapper.SubPlot(true)
                {
                    //ZLim=(-1,1),
                    XLabel = "X",
                    YLabel = "Y",
                    ZLabel = "Z",
                    Title = "surface",
                    Plots = new List<PyPlotWrapper.IPlot>
                    {
                        new PyPlotWrapper.ContourPlot(XX,YY,ZZ, false){Levels=20,ColorMap="cividis", ZOffset=XX.Min(), TargetHeightDirection="x", LineWidth=4},
                        new PyPlotWrapper.ContourPlot(XX,YY,ZZ, false){Levels=20,ColorMap="cividis", ZOffset=YY.Max(), TargetHeightDirection="y", LineWidth=4},
                        new PyPlotWrapper.ContourPlot(XX,YY,ZZ, false){Levels=20,ColorMap="cividis", ZOffset=ZZ.Min(), TargetHeightDirection="z", LineWidth=4},
                        //new PyPlotWrapper.ContourPlot(XX,YY,ZZ, false){Levels=20,Colors="green", ZOffset=ZZ.Min(), ContourLabelFontSize__2D=20},

                        //new PyPlotWrapper.WireFramePlot(XX,YY,ZZ){Color="black", LineWidth=3},
                        new PyPlotWrapper.SurfacePlot(XX,YY,ZZ){ ColorMap="cividis", Alpha=0.5},

                        //new PyPlotWrapper.ScatterPlot(XX,YY,ZZ){ ColorMap="green", MarkerSize=100},
                    }
                }
            }.Run(outputImageFile, preview);

        }


        // ★★★★★★★★★★★★★★★ 



    }
}
