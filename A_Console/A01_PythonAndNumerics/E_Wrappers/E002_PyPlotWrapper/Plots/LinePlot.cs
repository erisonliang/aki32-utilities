

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class PyPlotWrapper
{
    /// <summary>
    /// 2D / 3D
    /// </summary>
    public class LinePlot : IPlot
    {
        // ★★★★★★★★★★★★★★★ props

        public bool Is3D { get; set; } = false;
        public string LegendLabel { get; set; } = "";
        public double Alpha { get; set; } = 1;

        public double[] X { get; set; }
        public double[] Y { get; set; }
        /// <summary>
        /// (3d option)
        /// </summary>
        public double[] Z { get; set; }

        public int LineWidth { get; set; } = 4;
        public string LineStyle { get; set; } = "solid"; // solid, dashed, dashdot, dotted, --, -, :
        public string LineColor { get; set; } = null; //green

        public string Marker { get; set; } = null; // . , o v ^ < > ...
        public int MarkerSize { get; set; } = 1;
        public int MarkerEdgeWidth { get; set; } = 0;
        public string MarkerFaceColor { get; set; } = null; //green
        public string MarkerEdgeColor { get; set; } = null;


        // ★★★★★★★★★★★★★★★ inits

        public LinePlot(double[] y) : this(null, y)
        {
        }

        public LinePlot(double[]? x, double[] y)
        {
            X = x ?? Enumerable.Range(0, y.Length).Select(i => (double)i).ToArray();
            Y = y;
            Is3D = false;
        }

        public LinePlot(double[]? x, double[] y, double[] z)
        {
            X = x ?? Enumerable.Range(0, y.Length).Select(i => (double)i).ToArray();
            Y = y;
            Z = z;
            Is3D = true;
        }


        // ★★★★★★★★★★★★★★★ methods

        public void Run(dynamic fig, dynamic ax, string FontName)
        {
            // プロット
            if (Is3D)
            {
                ax.plot(X, Y, Z,
                    label: LegendLabel,
                    alpha: Alpha,

                    linewidth: LineWidth,
                    linestyle: LineStyle,
                    color: LineColor,

                    marker: Marker,
                    markersize: MarkerSize,
                    markeredgewidth: MarkerEdgeWidth,
                    markerfacecolor: MarkerFaceColor,
                    markeredgecolor: MarkerEdgeColor
                    );
            }
            else
            {
                ax.plot(X, Y,
                    label: LegendLabel,
                    alpha: Alpha,

                    linewidth: LineWidth,
                    linestyle: LineStyle,
                    color: LineColor,

                    marker: Marker,
                    markersize: MarkerSize,
                    markeredgewidth: MarkerEdgeWidth,
                    markerfacecolor: MarkerFaceColor,
                    markeredgecolor: MarkerEdgeColor
                    );
            }

            // markerfacecoloralt マーカーの塗りつぶしの色 2。fillstyle で left, right, bottom, top を指定した際、塗りつぶされない領域が ‘markerfacecoloralt’ で指定された色となります。 (デフォルト値: ‘none’)
            // fillstyle マーカーの塗りつぶしのスタイル。‘full’ (全体), ‘left’ (左半分), ‘right’ (右半分), ‘bottom’ (下半分), ‘top’ (上半分), ‘none’ (塗りつぶしなし)から選択。
            // antialiased アンチエイリアス(線を滑らかに描画する処理) を適用するかどうか。False または True から選択。

        }

        // ★★★★★★★★★★★★★★★ 

    }
}