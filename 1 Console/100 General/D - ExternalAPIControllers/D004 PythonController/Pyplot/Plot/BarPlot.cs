﻿

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class PythonController
{
    public partial class PyPlot
    {
        /// <summary>
        /// 2D / 3D
        /// </summary>
        public class BarPlot : IPlot
        {
            // ★★★★★★★★★★★★★★★ props

            public bool Is3D { get; set; } = true;
            public string LegendLabel { get; set; } = "";
            public double Alpha { get; set; } = 1;


            // ★★★★★★★★★★★★★★★ inits


            // ★★★★★★★★★★★★★★★ methods

            public void Run(dynamic ax)
            {
                // プロット
                // TODO Bar
                throw new NotImplementedException();
            }


            // ★★★★★★★★★★★★★★★ 

        }
    }
}