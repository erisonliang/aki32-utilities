

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public partial class PyPlotWrapper
{
    /// <summary>
    /// 2D / 3D
    /// </summary>
    public class ContourPlot : IPlot
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

        public double ZOffset { get; set; } = 0;
        public int Levels { get; set; } = 50;

        public string Color { get; set; } = null;
        public string ColorMap { get; set; } = null;

        public double? LineWidth { get; set; } = null;

        public string TargetHeightDirection { get; set; } = "z";

        /// <summary>
        /// (2d option)
        /// </summary>
        public int ContourLabelFontSize__2D { get; set; } = 0;
        /// <summary>
        /// (3d option)
        /// </summary>
        public int Stride__For3D { get; set; } = 1;
        /// <summary>
        /// (3d option)
        /// </summary>
        public bool UseFilledContour__For3D { get; set; } = false;


        // ★★★★★★★★★★★★★★★ inits

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">T[][], T[,] or NDArray. T = float or double</param>
        /// <param name="y">same as x format</param>
        /// <param name="z">same as x format</param>
        public ContourPlot(dynamic x, dynamic y, dynamic z, bool is3D)
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

            Is3D = is3D;
        }

        /// <summary>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z">/param>
        public ContourPlot(double[] x, double[] y, double[,] z, bool is3D)
        {
            (X, Y) = NumpyWrapper.GetMeshGrid(x, y);
            Z = NumpyWrapper.ToCorrect2DNDArray<double>(z);

            Is3D = is3D;
        }


        // ★★★★★★★★★★★★★★★ methods

        public void Run(dynamic fig, dynamic ax, string FontName)
        {
            // プロット
            dynamic cset;

            if (Is3D && UseFilledContour__For3D)
            {
                cset = ax.contourf(X, Y, Z,
                    alpha: Alpha,

                    levels: Levels,
                    extend3d: Is3D,
                    colors: Color,
                    cmap: ColorMap,
                    stride: Stride__For3D,
                    zdir: TargetHeightDirection,
                    offset: ZOffset
                    );
            }
            else
            {
                cset = ax.contour(X, Y, Z,
                    alpha: Alpha,

                    linewidths: LineWidth,

                    levels: Levels,
                    extend3d: Is3D,
                    colors: Color,
                    cmap: ColorMap,
                    zdir: TargetHeightDirection,
                    offset: ZOffset
                    );

                if (ContourLabelFontSize__2D > 0)
                    ax.clabel(cset, fontsize: ContourLabelFontSize__2D, inline: 1);
            }

            // data:
            //indexable object, optional
            //If given, all parameters also accept a string s, which is interpreted as data[s] (unless this raises an exception).

        }


        // ★★★★★★★★★★★★★★★ 

    }

}
