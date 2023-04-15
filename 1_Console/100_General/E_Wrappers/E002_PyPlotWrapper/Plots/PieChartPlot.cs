

namespace Aki32Utilities.ConsoleAppUtilities.General;
public partial class PyPlotWrapper
{
    /// <summary>
    /// 2D
    /// </summary>
    public class PieChartPlot : IPlot
    {
        // ★★★★★★★★★★★★★★★ props

        public bool Is3D { get; set; } = true;
        public string LegendLabel { get; set; } = "";
        public double Alpha { get; set; } = 1;


        // ★★★★★★★★★★★★★★★ inits


        // ★★★★★★★★★★★★★★★ methods

        public void Run(dynamic fig, dynamic ax, string FontName)
        {
            // プロット
            // TODO PieChart
            throw new NotImplementedException();
        }


        // ★★★★★★★★★★★★★★★ 

    }
}
