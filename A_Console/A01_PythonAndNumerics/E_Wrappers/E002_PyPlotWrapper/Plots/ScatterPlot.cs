﻿using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class PyPlotWrapper
{
    /// <summary>
    /// 2D / 3D
    /// </summary>
    public class ScatterPlot : IPlot
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
        //public string LineWidths { get; set; } = "2";
        public int LineWidth { get; set; } = 0;

        public string MarkerColor { get; set; } = "green";
        public string EdgeColors { get; set; } = "black";
        public string ColorMap { get; set; } = "Blues";
        //public double Normalize { get; set; }
        public (double? min, double? max) ColorLim { get; set; } = (null, null);


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

        /// <summary>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z">/param>
        public ScatterPlot(double[] x, double[] y, double[,] z)
        {
            Z = z.ReShape();
            X = x.SelectMany(x => y, (x, y) => y).ToArray();
            Y = x.SelectMany(x => y, (x, y) => x).ToArray();
            Is3D = true;
        }


        // ★★★★★★★★★★★★★★★ methods

        public void Run(dynamic fig, dynamic ax, string FontName)
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
                        linewidth: LineWidth,
                        edgecolors: EdgeColors
                    );
                }
                else if (Z_Color is not null)
                {
                    ax.scatter(X, Y, Z,
                        label: LegendLabel,
                        alpha: Alpha,

                        s: MarkerSize,
                        c: Z_Color,
                        marker: Marker,
                        linewidth: LineWidth,
                        edgecolors: EdgeColors,

                        cmap: ColorMap,
                        //norm : Normalize,
                        vmin: ColorLim.min,
                        vmax: ColorLim.max
                    );
                }
                else
                {
                    ax.scatter(X, Y, Z,
                        label: LegendLabel,
                        alpha: Alpha,

                        s: MarkerSize,
                        c: MarkerColor,
                        marker: Marker,
                        linewidth: LineWidth,
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
                        alpha: Alpha,

                        s: Z_Size,
                        c: MarkerColor,
                        marker: Marker,
                        linewidth: LineWidth,
                        edgecolors: EdgeColors
                    );
                }
                else if (Z_Color is not null)
                {
                    ax.scatter(X, Y,
                        label: LegendLabel,
                        alpha: Alpha,

                        s: MarkerSize,
                        c: Z_Color,
                        marker: Marker,
                        linewidth: LineWidth,
                        edgecolors: EdgeColors,

                        cmap: ColorMap,
                        //norm : Normalize,
                        vmin: ColorLim.min,
                        vmax: ColorLim.max
                    );
                }
                else
                {
                    ax.scatter(X, Y,
                        label: LegendLabel,
                        alpha: Alpha,

                        s: MarkerSize,
                        c: MarkerColor,
                        marker: Marker,
                        linewidth: LineWidth,
                        edgecolors: EdgeColors
                    );

                }
            }

        }


        // ★★★★★★★★★★★★★★★ methods (static)

        public static FileInfo DrawSimpleGraph(double[] X, double[] Y, FileInfo? outputImageFile = null, bool preview = true)
        {
            outputImageFile ??= new FileInfo(Path.GetTempFileName().GetExtensionChangedFilePath(".png"));

            return new Figure
            {
                IsTightLayout = true,
                SubPlot = new SubPlot()
                {
                    XLabel = "x",
                    YLabel = "y",
                    Title = "scatter",
                    Plot = new ScatterPlot(X, Y)
                    {
                        MarkerSize = 50
                    },
                }
            }.Run(outputImageFile, preview);
        }

        public static FileInfo DrawSimpleGraph(double[] X, double[] Y, double[,] Z, FileInfo? outputImageFile = null, bool preview = true)
        {
            outputImageFile ??= new FileInfo(Path.GetTempFileName().GetExtensionChangedFilePath(".png"));

            return new Figure
            {
                IsTightLayout = true,
                SubPlot = new SubPlot()
                {
                    XLabel = "x",
                    YLabel = "y",
                    ZLabel = "z",
                    Title = "scatter",
                    Plot = new ScatterPlot(X, Y, Z)
                    {
                        MarkerSize = 50
                    },
                }
            }.Run(outputImageFile, preview);
        }


        // ★★★★★★★★★★★★★★★ 

    }
}