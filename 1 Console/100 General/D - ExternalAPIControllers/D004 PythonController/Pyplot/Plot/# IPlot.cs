

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class PythonController
{
    public partial class PyPlot
    {
        /// <summary>
        /// All plot types can be found here
        /// https://www.c-sharpcorner.com/article/a-complete-python-matplotlib-tutorial/
        /// </summary>
        public interface IPlot
        {
            public bool Is3D { get; set; }
            public string LegendLabel { get; set; }
            public void Run(dynamic ax);
        }
    }
}