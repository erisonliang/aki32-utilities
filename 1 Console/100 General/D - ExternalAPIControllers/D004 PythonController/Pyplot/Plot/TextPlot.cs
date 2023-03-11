

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class PythonController
{
    public partial class PyPlot
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

            public double X { get; set; }
            public double Y { get; set; }
            /// <summary>
            /// (3d option)
            /// </summary>
            public string Text { get; set; }

            public string? Color { get; set; } = null;
            public int? FontSize { get; set; } = null;


            // ★★★★★★★★★★★★★★★ inits

            public TextPlot(double x, double y, string text)
            {
                X = x;
                Y = y;
                Text = text;
                Is3D = false;
            }


            // ★★★★★★★★★★★★★★★ methods

            public void Run(dynamic ax)
            {
                // プロット
                ax.text(X, Y, Text,
                    color: Color,
                    size: FontSize
                    );
            }


            // ★★★★★★★★★★★★★★★ 

        }
    }
}