

using XPlot.Plotly;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class PythonController
{
    public class PyPlot
    {
        public class Figure
        {

            // ★★★★★★★★★★★★★★★ props

            public string FontName { get; set; } = "MS Gothic";

            public double FigHeight { get; set; } = 10;
            public double FigWidth { get; set; } = 15;

            public double PlotWSpace { get; set; } = 0;
            public double PlotHSpace { get; set; } = 0;

            public bool IsTightLayout { get; set; } = false;

            public List<SubPlot> SubPlots { get; set; }


            // ★★★★★★★★★★★★★★★ methods

            public FileInfo Run(FileInfo outputFile)
            {
                // preprocess
                if (!Activated)
                    throw new Exception("Required to call PythonController.Initialize() first");
                if (SubPlots is null)
                {
                    throw new Exception("Required to SubPlots");
                }

                // ★★★★★ 全体

                dynamic plt = PythonController.Import("matplotlib.pyplot");

                plt.subplots_adjust(wspace: PlotWSpace, hspace: PlotHSpace);

                dynamic fig = plt.figure(figsize: new double[] { FigWidth, FigHeight });

                // ★★★★★ サブプロット，外側
                foreach (var SubPlot in SubPlots)
                    SubPlot.Run(fig, FontName);


                // ★★★★★ 最後に呼ぶべきもの

                // レイアウト詰め
                if (IsTightLayout)
                    fig.tight_layout();

                // 保存，解放
                plt.savefig(outputFile.FullName);
                plt.clf();
                plt.close();

                return outputFile;
            }


            // ★★★★★★★★★★★★★★★ 

        }

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

        public interface IPlot
        {
            public bool Is3D { get; set; }
            public string LegendLabel { get; set; }
            public void Run(dynamic ax);
        }

        public class LinePlot : IPlot
        {

            // ★★★★★★★★★★★★★★★ props

            public bool Is3D { get; set; } = false;

            public string LegendLabel { get; set; } = "";

            public double[] X { get; set; }
            public double[] Y { get; set; }
            /// <summary>
            /// (3d option)
            /// </summary>
            public double[] Z { get; set; }

            public int LineWidth { get; set; } = 4;
            public string LineStyle { get; set; } = "solid"; // solid, dashed, dashdot, dotted, --, -, :
            public string LineColor { get; set; } = "green";

            public string Marker { get; set; } = "."; // . , o v ^ < > ...
            public int MarkerSize { get; set; } = 1;
            public int MarkerEdgeWidth { get; set; } = 0;
            public string MarkerFaceColor { get; set; } = "green";
            public string MarkerEdgeColor { get; set; } = "green";

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

            public void Run(dynamic ax)
            {
                // プロット
                if (Is3D)
                {
                    ax.plot(X, Y, Z,
                       label: LegendLabel,

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

        // plot_surface
        // contour

        public class ScatterPlot : IPlot
        {

            // ★★★★★★★★★★★★★★★ props

            public bool Is3D { get; set; } = false;

            public string LegendLabel { get; set; } = "";

            public double[] X { get; set; }
            public double[] Y { get; set; }
            /// <summary>
            /// (3d option)
            /// </summary>
            public double[] Z { get; set; }

            /// <summary>
            /// When this is null, all plot size is the same. <br/>
            /// When not, express this in size.
            /// </summary>
            public double[]? Z_Size { get; set; }

            /// <summary>
            /// When this is null, all plot color is monotonic. <br/>
            /// When not, express this in color.
            /// </summary>
            public double[]? Z_Color { get; set; }

            public string Marker { get; set; } = "o"; // . , o v ^ < > ...
            public int MarkerSize { get; set; } = 20;
            public double Alpha { get; set; } = 1;
            public string LineWidths { get; set; } = "2";

            public string MarkerColor { get; set; } = "green";
            public string EdgeColors { get; set; } = "black";
            public string ColorMap { get; set; } = "Blues";
            //public double Normalize { get; set; }
            public double UpperThreshold { get; set; }
            public double LowerThreshold { get; set; }

            // ★★★★★★★★★★★★★★★ inits

            public ScatterPlot(double[] y) : this(null, y)
            {
            }

            public ScatterPlot(double[]? x, double[] y)
            {
                X = x ?? Enumerable.Range(0, y.Length).Select(i => (double)i).ToArray();
                Y = y;
                Is3D = false;
            }

            public ScatterPlot(double[]? x, double[] y, double[] z)
            {
                X = x ?? Enumerable.Range(0, y.Length).Select(i => (double)i).ToArray();
                Y = y;
                Z = z;
                Is3D = true;
            }


            // ★★★★★★★★★★★★★★★ methods

            public void Run(dynamic ax)
            {
                // プロット
                if (Is3D)
                {
                    if (Z_Size is not null)
                    {
                        ax.scatter(X, Y, Z,
                            label: LegendLabel,

                            s: Z_Size,
                            c: MarkerColor,
                            marker: Marker,
                            alpha: Alpha,
                            linewidths: LineWidths,
                            edgecolors: EdgeColors
                        );
                    }
                    else if (Z_Color is not null)
                    {
                        ax.scatter(X, Y, Z,
                            label: LegendLabel,

                            s: MarkerSize,
                            c: Z_Color,
                            marker: Marker,
                            alpha: Alpha,
                            linewidths: LineWidths,
                            edgecolors: EdgeColors,

                            cmap: ColorMap,
                            //norm : Normalize,
                            vmin: LowerThreshold,
                            vmax: UpperThreshold
                        );
                    }
                    else
                    {
                        ax.scatter(X, Y, Z,
                            label: LegendLabel,

                            s: MarkerSize,
                            c: MarkerColor,
                            marker: Marker,
                            alpha: Alpha,
                            linewidths: LineWidths,
                            edgecolors: EdgeColors
                        );

                    }
                }
                else
                {
                    if (Z_Size is not null)
                    {
                        ax.scatter(X, Y,
                            label: LegendLabel,

                            s: Z_Size,
                            c: MarkerColor,
                            marker: Marker,
                            alpha: Alpha,
                            linewidths: LineWidths,
                            edgecolors: EdgeColors
                        );
                    }
                    else if (Z_Color is not null)
                    {
                        ax.scatter(X, Y,
                            label: LegendLabel,

                            s: MarkerSize,
                            c: Z_Color,
                            marker: Marker,
                            alpha: Alpha,
                            linewidths: LineWidths,
                            edgecolors: EdgeColors,

                            cmap: ColorMap,
                            //norm : Normalize,
                            vmin: LowerThreshold,
                            vmax: UpperThreshold
                        );
                    }
                    else
                    {
                        ax.scatter(X, Y,
                            label: LegendLabel,

                            s: MarkerSize,
                            c: MarkerColor,
                            marker: Marker,
                            alpha: Alpha,
                            linewidths: LineWidths,
                            edgecolors: EdgeColors
                        );

                    }
                }
             
            }


            // ★★★★★★★★★★★★★★★ 

        }
    }
}