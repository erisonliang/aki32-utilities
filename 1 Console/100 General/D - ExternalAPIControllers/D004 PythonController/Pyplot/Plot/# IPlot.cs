

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class PythonController
{
    public partial class PyPlot
    {
        /// <summary>
        /// All plot types can be found here
        /// 
        /// axis grid: https://matplotlib.org/stable/tutorials/toolkits/axes_grid.html
        /// artists: https://matplotlib.org/stable/tutorials/toolkits/axisartist.html
        ///
        /// 2D plots: https://www.c-sharpcorner.com/article/a-complete-python-matplotlib-tutorial/
        /// 3D plots: https://matplotlib.org/stable/tutorials/toolkits/mplot3d.html
        /// 
        /// </summary>
        public interface IPlot
        {
            public bool Is3D { get; set; }
            public string LegendLabel { get; set; }
            public double Alpha { get; set; }

            public void Run(dynamic fig, dynamic ax, string FontName);
        }
    }
}