

using MathNet.Numerics.Interpolation;

using OpenCvSharp;

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
            public string TitleLocation { get; set; } = "center";
            public double TitlePadding { get; set; } = 20;

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

            public int XLabelPadding { get; set; } = 20;
            public int YLabelPadding { get; set; } = 20;
            /// <summary>
            /// (3d option)
            /// </summary>
            public int ZLabelPadding { get; set; } = 20;

            public string? XScale { get; set; } = null;
            public string? YScale { get; set; } = null;
            public string? ZScale { get; set; } = null;

            public int? XScale_Base { get; set; } = 10;
            public int? YScale_Base { get; set; } = 10;
            public int? ZScale_Base { get; set; } = 10;

            public int XYZLabelTickSize
            {
                set
                {
                    XLabelTickSize = value;
                    YLabelTickSize = value;
                    ZLabelTickSize = value;
                }
            }
            public int XLabelTickSize { get; set; } = 20;
            public int YLabelTickSize { get; set; } = 20;
            /// <summary>
            /// (3d option)
            /// </summary>
            public int ZLabelTickSize { get; set; } = 20;

            public bool InvertXAxis { get; set; } = false;
            public bool InvertYAxis { get; set; } = false;
            public bool InvertZAxis { get; set; } = false;
            public bool XAxisTickTop { get; set; } = false;
            public bool YAxisTickRight { get; set; } = false;

            public (double min, double max)? XLim { get; set; }
            public (double min, double max)? YLim { get; set; }
            /// <summary>
            /// (3d option)
            /// </summary>
            public (double min, double max)? ZLim { get; set; }

            public double? GraphMargins { get; set; } = null;

            public bool HasGrid { get; set; } = true;
            public string Grid_Which { get; set; } = "major";
            public string Grid_Axis { get; set; } = "both"; // x, y

            public IPlot Plot { get; set; }
            public List<IPlot> Plots { get; set; }

            // ★★★★★★★★★★★★★★★ init

            public SubPlot(bool InitFor3D = false)
            {
                if (InitFor3D)
                {
                    // 
                }
                else
                {
                    //
                }
            }


            // ★★★★★★★★★★★★★★★ methods

            public void Run(dynamic fig, string FontName)
            {
                if (Plot is not null)
                    Plots = new List<IPlot> { Plot };
                if (Plots is null)
                    throw new Exception("Required to set either Plot or Plots");

                dynamic ax;

                var is3d = Plots.Any(p => p.Is3D);


                if (is3d)
                    ax = fig.add_subplot(FigIndex, projection: "3d");
                else
                    ax = fig.add_subplot(FigIndex);

                // ★ タイトル
                if (!string.IsNullOrEmpty(Title))
                    ax.set_title(Title, fontname: FontName, size: TitleSize, loc: TitleLocation, pad: TitlePadding);

                // ★ 軸ラベル
                if (!string.IsNullOrEmpty(XLabel))
                    ax.set_xlabel(XLabel, size: XLabelSize, fontname: FontName, labelpad: XLabelPadding);
                if (!string.IsNullOrEmpty(YLabel))
                    ax.set_ylabel(YLabel, size: YLabelSize, fontname: FontName, labelpad: YLabelPadding);
                if (is3d)
                {
                    if (!string.IsNullOrEmpty(ZLabel))
                        ax.set_zlabel(ZLabel, size: ZLabelSize, fontname: FontName, labelpad: ZLabelPadding);
                }


                // ★ 軸目盛
                ax.tick_params(axis: 'x', labelsize: XLabelTickSize);
                if (XLim.HasValue)
                    ax.set_xlim(XLim.Value.min, XLim.Value.max);
                if (!string.IsNullOrEmpty(XScale))
                    ax.set_xscale(XScale, @base: XScale_Base);
                if (InvertXAxis)
                    ax.invert_xaxis();
                if (XAxisTickTop)
                    ax.xaxis.tick_top();

                ax.tick_params(axis: 'y', labelsize: YLabelTickSize);
                if (YLim.HasValue)
                    ax.set_ylim(YLim.Value.min, YLim.Value.max);
                if (!string.IsNullOrEmpty(YScale))
                    ax.set_yscale(YScale, @base: YScale_Base);
                if (InvertYAxis)
                    ax.invert_yaxis();
                if (YAxisTickRight)
                    ax.yaxis.tick_right();

                if (is3d)
                {
                    ax.tick_params(axis: 'z', labelsize: ZLabelTickSize);
                    if (ZLim.HasValue)
                        ax.set_zlim(ZLim.Value.min, ZLim.Value.max);
                    if (!string.IsNullOrEmpty(ZScale))
                        ax.set_zscale(ZScale, @base: ZScale_Base);
                    if (InvertZAxis)
                        ax.invert_zaxis();
                }


                // ★ グラフから枠線までの距離
                if (GraphMargins.HasValue)
                    ax.margins(GraphMargins!);

                // ★ grid
                ax.grid(HasGrid, which: Grid_Which, axis: Grid_Axis);



                // ★★★★★ サブプロット，内側

                // ★ プロット
                foreach (var Plot in Plots)
                    Plot.Run(fig, ax, FontName);



                // ★★★★★ 最後に呼ぶべきもの

                // ★ 凡例
                if (Plots.Any(p => !string.IsNullOrEmpty(p.LegendLabel)))
                    ax.legend(
                        loc: LegendLocation.ToString().Replace("_", " "),
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