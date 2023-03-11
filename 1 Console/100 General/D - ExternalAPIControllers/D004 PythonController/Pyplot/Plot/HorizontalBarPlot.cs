

using DocumentFormat.OpenXml.Drawing.Charts;

using XPlot.Plotly;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class PythonController
{
    public partial class PyPlot
    {
        /// <summary>
        /// 2D
        /// </summary>
        public class HorizontalBarPlot<T_YLabel> : IPlot
        {
            // ★★★★★★★★★★★★★★★ props

            public bool Is3D { get; set; } = false;
            public string LegendLabel { get; set; } = "";
            public double Alpha { get; set; } = 1;

            public double[] X { get; set; }
            public object[] Y { get; set; }

            public double[]? Lefts { get; set; } = null;
            public double? Height { get; set; } = null;
            public double? LineWidth { get; set; } = null;

            public string Alignment { get; set; } = "center";

            // ★★★★★★★★★★★★★★★ inits

            public HorizontalBarPlot(T_YLabel[]? y, double[] x)
            {
                X = x;
                Y = y?.Select(o => (object)o!)!.ToArray()!;
                Is3D = false;
            }


            // ★★★★★★★★★★★★★★★ methods

            public void Run(dynamic ax)
            {
                // プロット
                ax.barh(Y, X,
                      label: LegendLabel,
                      alpha: Alpha,

                      linewidth: LineWidth,
                      height: Height,
                      left: Lefts,

                      align: Alignment
                );

                // 残り barh(   fc, ec, xerr, capsize, log)

            }


            // ★★★★★★★★★★★★★★★ 

        }
    }
}