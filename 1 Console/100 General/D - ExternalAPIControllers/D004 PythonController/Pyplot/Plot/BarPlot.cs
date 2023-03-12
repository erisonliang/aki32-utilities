

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class PythonController
{
    public partial class PyPlot
    {
        /// <summary>
        /// 2D
        /// </summary>
        public class BarPlot<T_XLabel> : IPlot
        {
            // ★★★★★★★★★★★★★★★ props

            public bool Is3D { get; set; } = true;
            public string LegendLabel { get; set; } = "";
            public double Alpha { get; set; } = 1;

            public object[] X { get; set; }
            public double[] Height { get; set; }

            public double[]? Bottoms { get; set; } = null;
            public double Width { get; set; } = 0.8;
            public double? LineWidth { get; set; } = null;

            public string Alignment { get; set; } = "center";

            // ★★★★★★★★★★★★★★★ inits

            public BarPlot(T_XLabel[]? x, double[] height)
            {
                X = x?.Select(o => (object)o!)!.ToArray()!;
                Height = height;
                Is3D = false;
            }


            // ★★★★★★★★★★★★★★★ methods

            public void Run(dynamic fig, dynamic ax, string FontName)
            {
                // プロット
                ax.bar(X, Height,
                      label: LegendLabel,
                      alpha: Alpha,

                      linewidth: LineWidth,
                      width: Width,
                      bottom: Bottoms,

                      align: Alignment
                );

            }


            // ★★★★★★★★★★★★★★★ 

        }
    }
}