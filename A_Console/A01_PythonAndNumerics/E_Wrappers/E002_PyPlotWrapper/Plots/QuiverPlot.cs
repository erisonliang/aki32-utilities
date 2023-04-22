

using System;

using Aki32Utilities.ConsoleAppUtilities.General;

using DocumentFormat.OpenXml.Wordprocessing;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class PyPlotWrapper
{
    /// <summary>
    /// 2D / 3D
    /// </summary>
    public class QuiverPlot : IPlot
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
        public dynamic dX { get; set; }
        public dynamic dY { get; set; }
        /// <summary>
        /// (3d option)
        /// </summary>
        public dynamic dZ { get; set; }


        public string? Color { get; set; } = null;
        public string? ColorMap { get; set; } = null; //Blues

        public string Angles { get; set; } = "xy";
        public string ScaleUnits { get; set; } = "xy";
        /// <summary>
        /// divide by this value
        /// </summary>
        public double Scale { get; set; } = 1;

        public int LineWidth { get; set; } = 1;
        public string LineStyle { get; set; } = "solid"; // solid, dashed, dashdot, dotted, --, -, :
        public string LineColor { get; set; } = null; //green

        public double HeadAxisWidth { get; set; } = 0.005;
        public double HeadAxisLength { get; set; } = 3;
        public double HeadWidth { get; set; } = 5;
        public double HeadLength { get; set; } = 5;


        // ★★★★★★★★★★★★★★★ inits

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public QuiverPlot(double[] x, double[] y, double[] dx, double[] dy)
        {
            X = x;
            Y = y;
            dX = dx;
            dY = dy;
            Is3D = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="dz"></param>
        public QuiverPlot(double[] x, double[] y, double[] z, double[] dx, double[] dy, double[] dz)
        {
            X = x;
            Y = y;
            Z = z;
            dX = dx;
            dY = dy;
            dZ = dz;
            Is3D = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">T[][], T[,] or NDArray. T = float or double</param>
        /// <param name="y"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public QuiverPlot(dynamic x, dynamic y, dynamic dx, dynamic dy)
        {
            try
            {
                X = NumpyWrapper.ToCorrect2DNDArray<float>(x);
                Y = NumpyWrapper.ToCorrect2DNDArray<float>(y);
                dX = NumpyWrapper.ToCorrect2DNDArray<float>(dx);
                dY = NumpyWrapper.ToCorrect2DNDArray<float>(dy);
            }
            catch (Exception)
            {
                X = NumpyWrapper.ToCorrect2DNDArray<double>(x);
                Y = NumpyWrapper.ToCorrect2DNDArray<double>(y);
                dX = NumpyWrapper.ToCorrect2DNDArray<double>(dx);
                dY = NumpyWrapper.ToCorrect2DNDArray<double>(dy);
            }

            Is3D = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">T[][], T[,] or NDArray. T = float or double</param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="dz"></param>
        public QuiverPlot(dynamic x, dynamic y, dynamic z, dynamic dx, dynamic dy, dynamic dz)
        {
            try
            {
                X = NumpyWrapper.ToCorrect2DNDArray<float>(x);
                Y = NumpyWrapper.ToCorrect2DNDArray<float>(y);
                Z = NumpyWrapper.ToCorrect2DNDArray<float>(z);
                dX = NumpyWrapper.ToCorrect2DNDArray<float>(dx);
                dY = NumpyWrapper.ToCorrect2DNDArray<float>(dy);
                dZ = NumpyWrapper.ToCorrect2DNDArray<float>(dz);
            }
            catch (Exception)
            {
                X = NumpyWrapper.ToCorrect2DNDArray<double>(x);
                Y = NumpyWrapper.ToCorrect2DNDArray<double>(y);
                Z = NumpyWrapper.ToCorrect2DNDArray<double>(z);
                dX = NumpyWrapper.ToCorrect2DNDArray<double>(dx);
                dY = NumpyWrapper.ToCorrect2DNDArray<double>(dy);
                dZ = NumpyWrapper.ToCorrect2DNDArray<double>(dz);
            }

            Is3D = true;
        }


        // ★★★★★★★★★★★★★★★ methods

        public void Run(dynamic fig, dynamic ax, string FontName)
        {
            // プロット
            if (Is3D)
            {
                //ax.quiver(X, Y, Z, dX, dY, dZ,
                //    label: LegendLabel,
                //    alpha: Alpha,

                //    linewidth: LineWidth,
                //    cmap: ColorMap,
                //    color: Color,

                //    antialiased: AntiAliased
                //    );
            }
            else
            {
                ax.quiver(X, Y, dX, dY,
                    label: LegendLabel,
                    alpha: Alpha,

                    color: Color,
                    cmap: ColorMap,

                    angles: Angles,
                    scale_units: ScaleUnits,
                    scale: Scale,

                    linewidth: LineWidth,
                    linestyle: LineStyle,

                    headaxislength: HeadAxisLength,
                    headwidth: HeadWidth,
                    headlength: HeadLength,
                    width: HeadAxisWidth
                    );

            }
        }


        // ★★★★★★★★★★★★★★★ methods (static)

        public static FileInfo DrawSimpleGraph(double[] X, double[] Y, double[] dX, double[] dY, FileInfo? outputImageFile = null, bool preview = true)
        {
            outputImageFile ??= new FileInfo(Path.GetTempFileName().GetExtensionChangedFilePath(".png"));

            return new Figure
            {
                IsTightLayout = true,
                SubPlot = new SubPlot()
                {
                    XLim = (-8, 8),
                    YLim = (-8, 8),
                    HasGrid = true,
                    XLabel = "x",
                    YLabel = "y",
                    ZLabel = "z",
                    Title = "quiver",
                    Plot = new QuiverPlot(X, Y, dX, dY)
                    {
                        Scale = 10,
                    },
                }

            }.Run(outputImageFile, preview);
        }

        public static FileInfo DrawSimpleGraph(double[] X, double[] Y, double[] Z, double[] dX, double[] dY, double[] dZ, FileInfo? outputImageFile = null, bool preview = true)
        {
            outputImageFile ??= new FileInfo(Path.GetTempFileName().GetExtensionChangedFilePath(".png"));

            return new Figure
            {
                IsTightLayout = true,
                SubPlot = new SubPlot()
                {
                    XLim = (-8, 8),
                    YLim = (-8, 8),
                    HasGrid = true,
                    XLabel = "x",
                    YLabel = "y",
                    ZLabel = "z",
                    Title = "quiver",
                    Plot = new QuiverPlot(X, Y, Z, dX, dY, dZ)
                    {
                        Scale = 10,
                    },
                }

            }.Run(outputImageFile, preview);
        }

        public static void RunExampleModel(FileInfo outputImageFile, bool preview = true)
        {
            var pi = Math.PI;
            var n = 10;

            var X = EnumerableExtension.Range_WithCount(-5, 5, n).ToArray();
            var Y = EnumerableExtension.Range_WithCount(-5, 5, n).ToArray();

            var XYGrid = Enumerable.SelectMany(X, x => Y, (x, y) => (x, y));
            var XGrid = XYGrid.Select(xy => xy.x).ToArray().ReShape(n, n);
            var YGrid = XYGrid.Select(xy => xy.y).ToArray().ReShape(n, n);

            var U = X.Select(x => 2 * x);
            var V = Y.Select(y => 3 * y);

            var UVGrid = Enumerable.SelectMany(U, u => V, (u, v) => (u, v));
            var UGrid = UVGrid.Select(uv => uv.u).ToArray().ReShape(n, n);
            var VGrid = UVGrid.Select(uv => uv.v).ToArray().ReShape(n, n);

            new PyPlotWrapper.Figure
            {
                IsTightLayout = true,
                SubPlot = new PyPlotWrapper.SubPlot()
                {
                    XLim = (-8, 8),
                    YLim = (-8, 8),
                    HasGrid = true,
                    XLabel = "x",
                    YLabel = "y",
                    ZLabel = "z",
                    Title = "quiver",
                    Plot = new PyPlotWrapper.QuiverPlot(XGrid, YGrid, UGrid, VGrid)
                    {
                        Scale = 10,
                    },
                }

            }.Run(outputImageFile, preview);
        }


        // ★★★★★★★★★★★★★★★ 

    }
}
