

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class PyPlotWrapper
{
    /// <summary>
    /// 3D
    /// </summary>
    public class WireFramePlot : IPlot
    {

        // ★★★★★★★★★★★★★★★ props

        public bool Is3D { get; set; } = true;
        public string LegendLabel { get; set; } = "";
        public double Alpha { get; set; } = 1;

        public dynamic X { get; set; }
        public dynamic Y { get; set; }
        /// <summary>
        /// (3d option)
        /// </summary>
        public dynamic Z { get; set; }

        public int LineWidth { get; set; } = 5;

        public string Color { get; set; } = "green";

        public bool AntiAliased { get; set; } = true;

        //public double Normalize { get; set; }
        //public double UpperThreshold { get; set; }
        //public double LowerThreshold { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">T[][], T[,] or NDArray. T = float or double</param>
        /// <param name="y">same as x format</param>
        /// <param name="z">same as x format</param>
        public WireFramePlot(dynamic x, dynamic y, dynamic z)
        {
            try
            {
                X = NumpyWrapper.ToCorrect2DNDArray<float>(x);
                Y = NumpyWrapper.ToCorrect2DNDArray<float>(y);
                Z = NumpyWrapper.ToCorrect2DNDArray<float>(z);
            }
            catch (Exception)
            {
                X = NumpyWrapper.ToCorrect2DNDArray<double>(x);
                Y = NumpyWrapper.ToCorrect2DNDArray<double>(y);
                Z = NumpyWrapper.ToCorrect2DNDArray<double>(z);
            }

            Is3D = true;
        }

        /// <summary>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z">/param>
        public WireFramePlot(double[] x, double[] y, double[,] z)
        {
            (X, Y) = NumpyWrapper.GetMeshGrid(x, y);
            Z = NumpyWrapper.ToCorrect2DNDArray<double>(z);

            Is3D = true;
        }


        // ★★★★★★★★★★★★★★★ methods

        public void Run(dynamic fig, dynamic ax, string FontName)
        {
            // プロット
            var surf = ax.plot_wireframe(X, Y, Z,
                label: LegendLabel,
                alpha: Alpha,

                linewidth: LineWidth,
                color: Color,

                antialiased: AntiAliased
                );

        }


        // ★★★★★★★★★★★★★★★ 

    }

}
