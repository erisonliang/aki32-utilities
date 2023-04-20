

namespace Aki32Utilities.ConsoleAppUtilities.Structure;
public partial class MatrixDisplacementMethod
{
    /// <summary>
    /// 材端
    /// </summary>
    public struct Edge
    {

        // ★★★★★★★★★★★★★★★ props

        public int Node { get; set; } //材端の節点番号
        public bool IsNodePin { get; set; } //材端がピンかどうか
        public Fix Node_Fix; //材端の拘束状況，未知数番号
        public Displacement Displacement; //材端変位


        // ★★★★★★★★★★★★★★★ inits

        /// <summary>
        /// 部材材端の情報を生成します。
        /// </summary>
        /// <param name="node">材端の節点番号</param>
        /// <param name="isNodePin">材端はピンである</param>
        public Edge(int node, bool isNodePin)
        {
            Node = node;
            IsNodePin = isNodePin;
            Node_Fix = new Fix();
            Displacement = new Displacement();
        }


        // ★★★★★★★★★★★★★★★ methods

        public override string ToString()
        {
            string A = "";
            A += "****" + (Node + 1) + "節点側\r\n";
            A += "NodeIsPin = " + IsNodePin.ToString() + "\r\n";
            A += "*拘束状況：\r\n" + Node_Fix.ToString() + "\r\n";
            A += "*変位状況：\r\n" + Displacement.ToString() + "\r\n";
            return A;
        }


        // ★★★★★★★★★★★★★★★ 

    }
}
