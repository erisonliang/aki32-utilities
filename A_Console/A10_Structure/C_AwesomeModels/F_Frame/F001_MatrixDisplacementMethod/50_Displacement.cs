

namespace Aki32Utilities.ConsoleAppUtilities.Structure;
public partial class MatrixDisplacementMethod
{
    /// <summary>
    /// 変位
    /// </summary>
    public struct Displacement
    {

        // ★★★★★★★★★★★★★★★ props

        public double DeltaX { get; set; }
        public double DeltaY { get; set; }
        public double ThetaZ { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        /// <summary>
        /// 変位の情報を生成します。
        /// </summary>
        /// <param name="deltaX">水平方向変位</param>
        /// <param name="deltaY">鉛直方向変位</param>
        /// <param name="thetaZ">回転方向変位</param>
        public Displacement(double deltaX, double deltaY, double thetaZ)
        {
            DeltaX = deltaX;
            DeltaY = deltaY;
            ThetaZ = thetaZ;
        }


        // ★★★★★★★★★★★★★★★ methods

        public override string ToString()
        {
            return "δX = " + DeltaX.ToString() + "\r\nδY = " + DeltaY.ToString() + "\r\nθZ = " + ThetaZ + "\r\n";
        }


        // ★★★★★★★★★★★★★★★ 

    }
}
