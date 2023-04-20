

namespace Aki32Utilities.ConsoleAppUtilities.Structure;
public partial class MatrixDisplacementMethod
{
    /// <summary>
    /// 応力
    /// </summary>
    public struct Stress
    {

        // ★★★★★★★★★★★★★★★ props

        //力
        public double N { get; set; }
        public double Q { get; set; }
        public double M1 { get; set; }
        public double M2 { get; set; }
        //応力
        public double σN { get; set; }
        public double τ { get; set; }
        public double σb1 { get; set; }
        public double σb2 { get; set; }
        //許容応力度
        public double ft { get; set; }
        public double fc { get; set; }
        public double fs { get; set; }
        public double fb { get; set; }
        //設計比
        public double Check_C { get; set; } //圧縮検定比
        public double Check_T { get; set; } //引張検定比
        public double Check_Q { get; set; } //剪断検定比
        public double Check_Max { get; set; } //検定比の最大値 


        // ★★★★★★★★★★★★★★★ inits

        /// <summary>
        /// 応力の情報を生成します。
        /// </summary>
        /// <param name="N">軸方向力</param>
        /// <param name="Q">剪断力</param>
        /// <param name="M">曲げモーメント</param>
        public Stress(double N, double Q, double M1, double M2)
        {
            this.N = N;
            this.Q = Q;
            this.M1 = M1;
            this.M2 = M2;
            ft = fc = fs = fb = 0;
            σN = τ = σb1 = σb2 = Check_C = Check_T = Check_Q = Check_Max = 0;
        }


        // ★★★★★★★★★★★★★★★ methods

        /// <summary>
        /// 応力度を更新します
        /// </summary>
        /// <param name="A">断面積</param>
        /// <param name="Z">断面係数</param>
        public void UpdateStress(double A, double Z)
        {
            σN = N / A;
            τ = Q / A;
            σb1 = M1 / Z;
            σb2 = M2 / Z;
        }

        /// <summary>
        /// 検定比の最大値を更新します。
        /// </summary>
        /// <returns>Check_Max</returns>
        public double Update_Check_Max(bool IsForTension)
        {
            if (IsForTension)
                return Check_Max = Math.Max(Check_T, 0);
            else
                return Check_Max = Math.Max(Check_C, Math.Max(Check_T, Check_Q));
        }

        /// <summary>
        /// この構造体の応力の部分を可視化する文字列を返します。
        /// </summary>
        public string Print_Stress()
        {
            return
                "N = " + N.ToString("G5") + "\t\tσN = " + σN.ToString("G5") + "\r\n" +
                "Q = " + Q.ToString("G5") + "\t\tτ = " + τ.ToString("G5") + "\r\n" +
                "M1 = " + M1.ToString("G5") + "\t\tσb1 = " + σb1.ToString("G5") + "\r\n" +
                "M2 = " + M2.ToString("G5") + "\t\tσb2 = " + σb2.ToString("G5") + "\r\n";
        }

        /// <summary>
        /// この構造体の検定比の部分を可視化する文字列を返します。
        /// </summary>
        public string Print_Check()
        {
            return "Check_C = " + Check_C + "\r\nCheck_T = " + Check_T + "\r\nCheck_Q = " + Check_Q + "\r\n";
        }

        /// <summary>
        /// この構造体の許容応力度の部分を可視化する文字列を返します。
        /// </summary>
        public string Print_f()
        {
            return "ft = " + ft + "\r\nfs = " + fs + "\r\nfc = " + fc + "\r\nfb = " + fb + "\r\n";
        }


        // ★★★★★★★★★★★★★★★ 

    }
}
