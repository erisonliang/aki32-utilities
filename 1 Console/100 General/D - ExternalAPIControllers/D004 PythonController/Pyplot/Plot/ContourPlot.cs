

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class PythonController
{
    public partial class PyPlot
    {
        /// <summary>
        /// 2D / 3D
        /// </summary>
        public class ContourPlot : IPlot
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

            public double ZOffset { get; set; } = 0;
            public int Levels { get; set; } = 50;

            public string Color { get; set; } = null;
            public string ColorMap { get; set; } = null;

            public double? LineWidth { get; set; } = null;

            public string TargetHeightDirection { get; set; } = "z";

            /// <summary>
            /// (2d option)
            /// </summary>
            public int ContourLabelFontSize__2D { get; set; } = 0;
            /// <summary>
            /// (3d option)
            /// </summary>
            public int Stride__For3D { get; set; } = 1;
            /// <summary>
            /// (3d option)
            /// </summary>
            public bool UseFilledContour__For3D { get; set; } = false;


            // ★★★★★★★★★★★★★★★ inits

            /// <summary>
            /// 
            /// </summary>
            /// <param name="x">T[][], T[,] or NDArray. T = float or double</param>
            /// <param name="y">same as x format</param>
            /// <param name="z">same as x format</param>
            public ContourPlot(dynamic x, dynamic y, dynamic z, bool is3D, int level)
            {
                try
                {
                    X = ToCorrect2DNDArray<float>(x);
                    Y = ToCorrect2DNDArray<float>(y);
                    Z = ToCorrect2DNDArray<float>(z);
                }
                catch (Exception)
                {
                    X = ToCorrect2DNDArray<double>(x);
                    Y = ToCorrect2DNDArray<double>(y);
                    Z = ToCorrect2DNDArray<double>(z);
                }

                Is3D = is3D;
                Levels = level;
            }

            /// <summary>
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="z">/param>
            public ContourPlot(double[] x, double[] y, double[,] z, bool is3D, int level)
            {
                Z = ToCorrect2DNDArray<double>(z);
                dynamic np = Import("numpy");
                dynamic mesh = np.meshgrid(np.array(x), np.array(y));
                X = mesh[0];
                Y = mesh[1];

                Is3D = is3D;
                Levels = level;
            }

            /// <summary>
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="z">/param>
            public ContourPlot(float[] x, float[] y, float[,] z, bool is3D, int level)
            {
                Z = ToCorrect2DNDArray<float>(z);
                dynamic np = Import("numpy");
                dynamic mesh = np.meshgrid(np.array(x), np.array(y));
                X = mesh[0];
                Y = mesh[1];

                Is3D = is3D;
                Levels = level;
            }


            // ★★★★★★★★★★★★★★★ methods

            public void Run(dynamic ax)
            {
                // プロット
                dynamic cset;

                if (Is3D && UseFilledContour__For3D)
                {
                    cset = ax.contourf(X, Y, Z,
                        alpha: Alpha,

                        levels: Levels,
                        extend3d: Is3D,
                        colors: Color,
                        cmap: ColorMap,
                        stride: Stride__For3D,
                        zdir: TargetHeightDirection,
                        offset: ZOffset
                        );
                }
                else
                {
                    cset = ax.contour(X, Y, Z,
                        alpha: Alpha,

                        linewidths: LineWidth,

                        levels: Levels,
                        extend3d: Is3D,
                        colors: Color,
                        cmap: ColorMap,
                        zdir: TargetHeightDirection,
                        offset: ZOffset
                        );

                    if (ContourLabelFontSize__2D > 0)
                        ax.clabel(cset, fontsize: ContourLabelFontSize__2D, inline: 1);
                }

                // data:
                //indexable object, optional
                //If given, all parameters also accept a string s, which is interpreted as data[s] (unless this raises an exception).

            }


            // ★★★★★★★★★★★★★★★ 

        }

    }
}