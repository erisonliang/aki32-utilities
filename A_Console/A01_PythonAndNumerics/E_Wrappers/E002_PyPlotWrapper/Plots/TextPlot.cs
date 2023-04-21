

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class PyPlotWrapper
{
    /// <summary>
    /// 2D / 3D
    /// </summary>
    public class TextPlot : IPlot
    {
        // ★★★★★★★★★★★★★★★ props

        public bool Is3D { get; set; } = true;
        public string LegendLabel { get; set; } = "";
        public double Alpha { get; set; } = 1;

        public string Text { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
        public double Rotation { get; set; }

        public string? OverwriteFontName { get; set; } = null;
        public string? FontColor { get; set; } = null;
        public int? FontSize { get; set; } = 20;

        public string HorizontalAlignment { get; set; } = "left";
        public string VerticalAlignment { get; set; } = "bottom";


        // ★★★★★★★★★★★★★★★ inits

        public TextPlot(double x, double y, string text)
        {
            X = x;
            Y = y;
            Text = text;
            Is3D = false;
        }


        // ★★★★★★★★★★★★★★★ methods

        public void Run(dynamic fig, dynamic ax, string FontName)
        {
            if (!string.IsNullOrEmpty(OverwriteFontName))
                FontName = OverwriteFontName;

            // プロット
            ax.text(X, Y, Text,

                //transform: ax.transAxes // Relative to ax grid 0-1

                fontname: FontName,
                size: FontSize,
                color: FontColor,

                horizontalalignment: HorizontalAlignment,
                verticalalignment: VerticalAlignment,

                rotation: Rotation

                );
        }


        // ★★★★★★★★★★★★★★★ 

    }
}
