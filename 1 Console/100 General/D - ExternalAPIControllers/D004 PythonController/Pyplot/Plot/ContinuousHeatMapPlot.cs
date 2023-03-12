

using MathNet.Numerics.Interpolation;

using OpenCvSharp;

using XPlot.Plotly;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class PythonController
{
    public partial class PyPlot
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

                Is3D = false;
            }

            /// <summary>
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="z">/param>
            public ContinuousHeatMapPlot(double[] x, double[] y, double[,] z)
            {
                Z = ToCorrect2DNDArray<double>(z);
                dynamic np = Import("numpy");
                dynamic mesh = np.meshgrid(np.array(x), np.array(y));
                X = mesh[0];
                Y = mesh[1];

                Is3D = false;
            }

            /// <summary>
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="z">/param>
            public ContinuousHeatMapPlot(float[] x, float[] y, float[,] z)
            {
                Z = ToCorrect2DNDArray<float>(z);
                dynamic np = Import("numpy");
                dynamic mesh = np.meshgrid(np.array(x), np.array(y));
                X = mesh[0];
                Y = mesh[1];

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

            // ★★★★★★★★★★★★★★★ 

        }
    }
}