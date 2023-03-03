

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class PythonController
{
    public partial class PyPlot
    {
        /// <summary>
        /// 3D
        /// </summary>
        public class FilledContourPlot : IPlot
        {
            // ★★★★★★★★★★★★★★★ props

            public bool Is3D { get; set; } = true;
            public string LegendLabel { get; set; } = "";


            // ★★★★★★★★★★★★★★★ inits


            // ★★★★★★★★★★★★★★★ methods

            public void Run(dynamic ax)
            {
                // プロット
                // TODO Filled Contour
                throw new NotImplementedException();
            }


            // ★★★★★★★★★★★★★★★ 

        }
    }
}