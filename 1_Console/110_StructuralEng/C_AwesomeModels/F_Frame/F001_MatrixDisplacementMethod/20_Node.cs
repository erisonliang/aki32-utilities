using MathNet.Spatial.Euclidean;

namespace Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
public partial class MatrixDisplacementMethod
{
    /// <summary>
    /// 節点
    /// </summary>
    public class Node
    {
        // ★★★★★★★★★★★★★★★ props

        //入力情報
        public Point2D Point;
        public Fix Fix;
        /// <summary>
        /// 剛接合かどうか
        /// </summary>
        public bool IsRigid { get; set; } = false;
        public List<Load> Loads { get; set; } = new List<Load>();
        /// <summary>
        /// 疑似的に入力された節点かどうか
        /// </summary>
        public bool IsPseudNode { get; set; } = false;

        //出力情報
        public Displacement Displacement;


        // ★★★★★★★★★★★★★★★ inits

        public Node(Point2D point, Fix fix)
        {
            this.Point = point;
            this.Fix = fix;
        }


        // ★★★★★★★★★★★★★★★ methods

        /// <summary>
        /// </summary>
        /// <param name="num">節点番号</param>
        public string ToString(int num)
        {
            if (IsPseudNode)
                return "";
            string A = "";
            A += "******************************************************** " + num + "\r\n";
            A += "*座標：\r\n" + Point.ToString() + "\r\n";
            A += "*拘束：\r\n" + Fix.ToString() + "\r\n";
            A += "*荷重：\r\n";
            for (int i = 0; i < Loads.Count; i++)
            {
                A += "\r\n****荷重 " + (i + 1) + "\r\n";
                A += Loads[i].ToString() + "\r\n";
            }
            A += "*変位：\r\n" + Displacement.ToString() + "\r\n";
            return A;
        }


        // ★★★★★★★★★★★★★★★ 

    }
}
