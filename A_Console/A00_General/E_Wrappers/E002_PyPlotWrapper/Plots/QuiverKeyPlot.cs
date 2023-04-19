

namespace Aki32Utilities.ConsoleAppUtilities.General;
public partial class PyPlotWrapper
{
    /// <summary>
    /// 3D
    /// </summary>
    public class QuiverKeyPlot : IPlot
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
            // TODO
            throw new NotImplementedException();

            //# ベクトルにQuiverのテキストを添える
            // ax.quiverkey(q, X = 0.75, Y = 1.009, U = 25, label = 'Quiver key, length=1', labelpos = 'E',
            //fontproperties ={ 'size': 13})

        }


        // ★★★★★★★★★★★★★★★ 

    }
}