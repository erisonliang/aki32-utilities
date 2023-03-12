

using DocumentFormat.OpenXml.Drawing.Charts;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class PythonController
{
    public partial class PyPlot
    {
        /// <summary>
        /// 2D
        /// </summary>
        public class Bar3DPlot : IPlot
        {
            // ★★★★★★★★★★★★★★★ props

            public bool Is3D { get; set; } = true;
            public string LegendLabel { get; set; } = "";
            public double Alpha { get; set; } = 1;

            /// <summary>
            /// flatten mesh grid
            /// </summary>
            public double[] X { get; set; }
            public double[] Y { get; set; }
            public double[] Z { get; set; }
            public double[] dX { get; set; }
            public double[] dY { get; set; }
            public double[] dZ { get; set; }

            public string Color { get; set; } = null;

            public bool Shade { get; set; } = true;

            // ★★★★★★★★★★★★★★★ inits

            public Bar3DPlot(double[] x, double[] y, double dx, double dy, double[,] dz, bool isCentered)
                : this(x, y, null, Enumerable.Repeat(dx, x.Length).ToArray(), Enumerable.Repeat(dy, y.Length).ToArray(), dz, isCentered)
            {
            }

            public Bar3DPlot(double[] x, double[] y, double[,]? z, double[] dx, double[] dy, double[,] dz, bool isCentered)
            {
                if (isCentered)
                {
                    for (int i = 0; i < x.Length; i++)
                        x[i] -= dx[i] / 2;
                    for (int i = 0; i < y.Length; i++)
                        y[i] -= dy[i] / 2;
                }

                var (XGrid, YGrid) = GetMeshGrid(x, y);
                X = XGrid.ReShape();
                Y = YGrid.ReShape();
                Z = z is not null ? z.ReShape() : EnumerableExtension.Range_WithCount(0, 0, dz.Length).ToArray();

                var (dXGrid, dYGrid) = GetMeshGrid(dx, dy);
                dX = dXGrid.ReShape();
                dY = dYGrid.ReShape();
                dZ = dz.ReShape();

                Is3D = true;
            }


            // ★★★★★★★★★★★★★★★ methods

            public void Run(dynamic fig, dynamic ax, string FontName)
            {
                // プロット
                ax.bar3d(
                    x: X, y: Y, z: Z,
                    dx: dX, dy: dY, dz: dZ,

                    label: LegendLabel,
                    alpha: Alpha,

                    color: Color,
                    shade: Shade
                );
            }


            // ★★★★★★★★★★★★★★★ 

        }
    }
}