

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class PythonController
{
    public partial class PyPlot
    {
        public class SubPlot
        {

            // ★★★★★★★★★★★★★★★ props

            public int FigIndex { get; set; } = 111;

            public LegendLocation LegendLocation { get; set; } = LegendLocation.upper_right;

            public string Title { get; set; } = null;
            public int TitleSize { get; set; } = 40;

            public string XLabel { get; set; } = null;
            public string YLabel { get; set; } = null;
            /// <summary>
            /// (3d option)
            /// </summary>
            public string ZLabel { get; set; } = null;
            public int XLabelSize { get; set; } = 30;
            public int YLabelSize { get; set; } = 30;
            /// <summary>
            /// (3d option)
            /// </summary>
            public int ZLabelSize { get; set; } = 30;

            public int XLabelPadding { get; set; } = 0;
            public int YLabelPadding { get; set; } = 0;
            /// <summary>
            /// (3d option)
            /// </summary>
            public int ZLabelPadding { get; set; } = 0;

            public int XYZLabelTickSize { get; set; } = 20;

            public (int min, int max)? XLim { get; set; }
            public (int min, int max)? YLim { get; set; }
            /// <summary>
            /// (3d option)
            /// </summary>
            public (int min, int max)? ZLim { get; set; }

            public double? GraphMargins { get; set; } = null;
            public bool HasGrid { get; set; } = true;

            public List<IPlot> Plots { get; set; }

            // ★★★★★★★★★★★★★★★ init

            public SubPlot(bool InitFor3D = false)
            {
                if (InitFor3D)
                {
                    XLabelPadding = 20;
                    YLabelPadding = 20;
                    ZLabelPadding = 20;
                }
            }


            // ★★★★★★★★★★★★★★★ methods

            public void Run(dynamic fig, string FontName)
            {
                dynamic ax;

                var is3d = Plots.Any(p => p.Is3D);

                if (is3d)
                    ax = fig.add_subplot(FigIndex, projection: "3d");
                else
                    ax = fig.add_subplot(FigIndex);

                // タイトル
                if (!string.IsNullOrEmpty(Title))
                    ax.set_title(Title, fontname: FontName, size: TitleSize);

                // 軸ラベル
                if (!string.IsNullOrEmpty(XLabel))
                    ax.set_xlabel(XLabel, size: XLabelSize, fontname: FontName, labelpad: XLabelPadding);
                if (!string.IsNullOrEmpty(YLabel))
                    ax.set_ylabel(YLabel, size: YLabelSize, fontname: FontName, labelpad: YLabelPadding);
                if (is3d)
                {
                    if (!string.IsNullOrEmpty(ZLabel))
                        ax.set_zlabel(ZLabel, size: ZLabelSize, fontname: FontName, labelpad: ZLabelPadding);
                }

                // 軸目盛
                ax.tick_params(axis: 'x', labelsize: XYZLabelTickSize);
                if (XLim.HasValue)
                    ax.set_xlim(XLim.Value.min, XLim.Value.max);
                ax.tick_params(axis: 'y', labelsize: XYZLabelTickSize);
                if (YLim.HasValue)
                    ax.set_ylim(YLim.Value.min, YLim.Value.max);
                if (is3d)
                {
                    ax.tick_params(axis: 'z', labelsize: XYZLabelTickSize);
                    if (ZLim.HasValue)
                        ax.set_zlim(ZLim.Value.min, ZLim.Value.max);
                }

                // グラフから枠線までの距離
                if (GraphMargins.HasValue)
                    ax.margins(GraphMargins!);

                // grid
                ax.grid(HasGrid);


                // ★★★★★ サブプロット，内側

                // プロット
                foreach (var Plot in Plots)
                    Plot.Run(ax);


                // ★★★★★ 最後に呼ぶべきもの

                // 凡例
                if (Plots.Any(p => !string.IsNullOrEmpty(p.LegendLabel)))
                    ax.legend(
                        loc: LegendLocation.ToString().Replace("_"," "),
                        prop: new Dictionary<string, string>
                        {
                            { "family", FontName },
                        }
                        );


                // ★★★★★ 

            }


            // ★★★★★★★★★★★★★★★ 

        }

        public enum LegendLocation
        {
            best,
            upper_right,
            upper_left,
            lower_left,
            lower_right,
            right,
            center_left,
            center_right,
            lower_center,
            upper_center,
            center,
        }
    }
}