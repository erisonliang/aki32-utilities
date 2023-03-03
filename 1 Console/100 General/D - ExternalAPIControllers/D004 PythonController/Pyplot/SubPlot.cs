

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class PythonController
{
    public partial class PyPlot
    {
        public class SubPlot
        {

            // ★★★★★★★★★★★★★★★ props

            public int FigIndex { get; set; } = 111;

            public string Title { get; set; } = "";
            public int TitleSize { get; set; } = 40;

            public string XLabel { get; set; } = "";
            public string YLabel { get; set; } = "";
            /// <summary>
            /// (3d option)
            /// </summary>
            public string ZLabel { get; set; } = "";
            public int XLabelSize { get; set; } = 30;
            public int YLabelSize { get; set; } = 30;
            /// <summary>
            /// (3d option)
            /// </summary>
            public int ZLabelSize { get; set; } = 30;

            public int XYZLabelTickSize { get; set; } = 20;

            public Range? XLim { get; set; }
            public Range? YLim { get; set; }
            /// <summary>
            /// (3d option)
            /// </summary>
            public Range? ZLim { get; set; }

            public double? GraphMargins { get; set; } = null;
            public bool HasGrid { get; set; } = true;

            public List<IPlot> Plots { get; set; }


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
                    ax.set_xlabel(XLabel, size: XLabelSize, fontname: FontName);
                if (!string.IsNullOrEmpty(YLabel))
                    ax.set_ylabel(YLabel, size: YLabelSize, fontname: FontName);
                if (is3d)
                {
                    if (!string.IsNullOrEmpty(ZLabel))
                        ax.set_zlabel(ZLabel, size: ZLabelSize, fontname: FontName);
                }

                // 軸目盛
                ax.tick_params(axis: 'x', labelsize: XYZLabelTickSize);
                if (XLim.HasValue)
                    ax.set_xlim(XLim.Value.Start.Value, XLim.Value.End.Value);
                ax.tick_params(axis: 'y', labelsize: XYZLabelTickSize);
                if (YLim.HasValue)
                    ax.set_ylim(YLim.Value.Start.Value, YLim.Value.End.Value);
                if (is3d)
                {
                    ax.tick_params(axis: 'z', labelsize: XYZLabelTickSize);
                    if (ZLim.HasValue)
                        ax.set_ylim(ZLim.Value.Start.Value, ZLim.Value.End.Value);
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
                    ax.legend(prop: new Dictionary<string, string> { { "family", FontName } });


                // ★★★★★ 

            }


            // ★★★★★★★★★★★★★★★ 

        }
    }
}