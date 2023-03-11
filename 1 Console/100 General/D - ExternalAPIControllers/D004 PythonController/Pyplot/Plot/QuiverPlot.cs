

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class PythonController
{
    public partial class PyPlot
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
                    X = ToCorrect2DNDArray<float>(x);
                    Y = ToCorrect2DNDArray<float>(y);
                    dX = ToCorrect2DNDArray<float>(dx);
                    dY = ToCorrect2DNDArray<float>(dy);
                }
                catch (Exception)
                {
                    X = ToCorrect2DNDArray<double>(x);
                    Y = ToCorrect2DNDArray<double>(y);
                    dX = ToCorrect2DNDArray<double>(dx);
                    dY = ToCorrect2DNDArray<double>(dy);
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
                    X = ToCorrect2DNDArray<float>(x);
                    Y = ToCorrect2DNDArray<float>(y);
                    Z = ToCorrect2DNDArray<float>(z);
                    dX = ToCorrect2DNDArray<float>(dx);
                    dY = ToCorrect2DNDArray<float>(dy);
                    dZ = ToCorrect2DNDArray<float>(dz);
                }
                catch (Exception)
                {
                    X = ToCorrect2DNDArray<double>(x);
                    Y = ToCorrect2DNDArray<double>(y);
                    Z = ToCorrect2DNDArray<double>(z);
                    dX = ToCorrect2DNDArray<double>(dx);
                    dY = ToCorrect2DNDArray<double>(dy);
                    dZ = ToCorrect2DNDArray<double>(dz);
                }

                Is3D = true;
            }


            // ★★★★★★★★★★★★★★★ methods

            public void Run(dynamic ax)
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


            // ★★★★★★★★★★★★★★★ 

        }
    }
}