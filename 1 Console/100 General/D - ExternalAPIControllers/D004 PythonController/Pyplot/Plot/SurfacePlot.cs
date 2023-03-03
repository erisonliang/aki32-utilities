

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class PythonController
{
    public partial class PyPlot
    {
        /// <summary>
        /// 3D
        /// </summary>
        public class SurfacePlot : IPlot
        {

            // ★★★★★★★★★★★★★★★ props

            public bool Is3D { get; set; } = true;
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

            public SurfacePlot(double[]? x, double[] y, double[] z)
            {
                X = x ?? Enumerable.Range(0, y.Length).Select(i => (double)i).ToArray();
                Y = y;
                Z = z;
                Is3D = true;
            }


            // ★★★★★★★★★★★★★★★ methods

            public void Run(dynamic ax)
            {
                // plot_surface


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