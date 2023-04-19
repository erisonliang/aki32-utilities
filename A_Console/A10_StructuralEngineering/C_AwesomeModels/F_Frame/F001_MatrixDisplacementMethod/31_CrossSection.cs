

namespace Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
public partial class MatrixDisplacementMethod
{
    /// <summary>
    /// 断面形状
    /// </summary>
    public struct CrossSection
    {

        // ★★★★★★★★★★★★★★★ props

        public MaterialType Material { get; set; }
        public double A { get; set; } //断面積
        public double E { get; set; } //ヤング係数
        //public double G { get; set; } //せん断弾性係数
        public double I { get; set; } //断面二次モーメント
        //public double J { get; set; } //断面二次極モーメント？
        public double i { get; set; } //断面二次半径
        public double Z { get; set; } //断面係数
        //public double v { get; set; } //ポアソン比
        public string Memo { get; set; }


        // ★★★★★★★★★★★★★★★ inits

        /// <summary>
        /// 断面形状の情報を生成します。
        /// </summary>
        /// <param name="A">断面積</param>
        /// <param name="E">ヤング係数</param>
        /// <param name="I">断面二次モーメント</param>
        /// <param name="Z">断面係数</param>
        public CrossSection(MaterialType material, double A, double E, double I, double? Z = null,  string memo = "")
        {
            this.Material = material;
            this.A = A;
            this.E = E;
            this.I = I;
            i = Math.Sqrt(I / A);
            this.Z = Z is not null ? Z.Value : 0;
            Memo = memo;
        }


        // ★★★★★★★★★★★★★★★ methods

        public override string ToString()
        {
            return "A = " + A.ToString() + "\r\nE = " + E.ToString() + "\r\nI  = " + I.ToString() + "\r\n";
        }


        // ★★★★★★★★★★★★★★★ 

    }

}
