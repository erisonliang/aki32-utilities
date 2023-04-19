

namespace Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
public partial class MatrixDisplacementMethod
{
    /// <summary>
    /// 拘束
    /// </summary>
    public struct Fix
    {

        // ★★★★★★★★★★★★★★★ props

        public bool DeltaX { get; set; }
        public bool DeltaY { get; set; }
        public bool ThetaZ { get; set; }
        public int n_DeltaX { get; set; }
        public int n_DeltaY { get; set; }
        public int n_ThetaZ { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        /// <summary>
        /// 拘束情報を生成します。
        /// </summary>
        /// <param name="δX">水平方向に拘束</param>
        /// <param name="δY">鉛直方向に拘束</param>
        /// <param name="θZ">回転方向に拘束</param>
        public Fix(bool δX, bool δY, bool θZ)
        {
            DeltaX = δX;
            DeltaY = δY;
            ThetaZ = θZ;
            n_DeltaX = 0;
            n_DeltaY = 0;
            n_ThetaZ = 0;
        }

        // ★★★★★★★★★★★★★★★ methods

        public override string ToString()
        {
            return "δX = " + DeltaX.ToString() + "\tn_δX = " + n_DeltaX + "\r\nδY = " + DeltaY.ToString() + "\tn_δY = " + n_DeltaY + "\r\nθZ = " + ThetaZ + "\tn_θZ = " + n_ThetaZ + "\r\n";
        }


        // ★★★★★★★★★★★★★★★ 

    }
}
