

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class PyPlotWrapper
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

        public double[] Width { get; set; }
        public object[] Y { get; set; }

        public double[]? Lefts { get; set; } = null;
        public double? Height { get; set; } = 0.8;
        public double? LineWidth { get; set; } = null;

        public string Alignment { get; set; } = "center";

        // ★★★★★★★★★★★★★★★ inits

        public HorizontalBarPlot(T_YLabel[]? y, double[] width)
        {
            Y = y?.Select(o => (object)o!)!.ToArray()!;
            Width = width;
            Is3D = false;
        }


        // ★★★★★★★★★★★★★★★ methods

        public void Run(dynamic fig, dynamic ax, string FontName)
        {
            // プロット
            ax.barh(Y, Width,
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