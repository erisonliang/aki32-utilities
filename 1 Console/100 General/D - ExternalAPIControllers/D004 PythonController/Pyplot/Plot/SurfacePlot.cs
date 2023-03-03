﻿

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

            public dynamic X { get; set; }
            public dynamic Y { get; set; }
            /// <summary>
            /// (3d option)
            /// </summary>
            public dynamic Z { get; set; }

            public int LineWidth { get; set; } = 0;

            public string Color { get; set; } = "black";
            public string ColorMap { get; set; } = "coolwarm"; //Blues

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
            public SurfacePlot(dynamic x, dynamic y, dynamic z)
            {
                try
                {
                    X = ToCorrect2DNDArray<float>(x);
                    Y = ToCorrect2DNDArray<float>(y);
                    Z = ToCorrect2DNDArray<float>(z);
                }
                catch (Exception)
                {
                    X = ToCorrect2DNDArray<double>(x);
                    Y = ToCorrect2DNDArray<double>(y);
                    Z = ToCorrect2DNDArray<double>(z);
                }

                Is3D = true;
            }


            // ★★★★★★★★★★★★★★★ methods

            public void Run(dynamic ax)
            {
                // プロット
                var surf = ax.plot_surface(X, Y, Z,
                    label: LegendLabel,

                    linewidth: LineWidth,
                    cmap: ColorMap,
                    color: Color,

                    antialiased: AntiAliased
                    );

            }

            // ★★★★★★★★★★★★★★★ 

        }
    }
}