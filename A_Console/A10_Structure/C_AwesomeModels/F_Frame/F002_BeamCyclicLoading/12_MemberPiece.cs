

namespace Aki32Utilities.ConsoleAppUtilities.Structure;
public partial class BeamCyclicLoading
{
    /// <summary>
    /// 梁の微小要素
    /// </summary>
    class MemberPiece
    {
        #region 単体変数

        /// <summary>
        /// 要素I,Jの素材参照
        /// </summary>
        public Steel steel { get; set; }

        /// <summary>
        /// バウシンガー部における状態を表す変数
        /// </summary>
        public BausState BausState { get; set; }

        /// <summary>
        /// 応力度歪度関係における状態を表す変数
        /// </summary>
        public SigEpsState SigEpsState { get; set; }


        /// <summary>
        /// 微小高さ
        /// </summary>
        public double dH { get; set; }
        /// <summary>
        /// 微小要素のB方向長さ
        /// </summary>
        public double B { get; set; }

        /// <summary>
        /// 真応力度-真歪度関係における骨格曲線での累積歪
        /// </summary>
        public double TotalEps_t { get; set; }

        /// <summary>
        /// 真応力度-真歪度関係における累積塑性歪
        /// </summary>
        public double TotalPlasticEps_t { get; set; }

        /// <summary>
        /// 真応力度-真歪度関係における正側骨格曲線での累積歪
        /// </summary>
        public double TotalEps_t_pos { get; set; }

        /// <summary>
        /// 弾塑性判定における応力度の誤差
        /// </summary>
        public double SigError { get; set; }

        #endregion

        #region 真公称ペア

        /// <summary>
        /// 真応力度
        /// </summary>
        public double Sig_t { get; set; }
        /// <summary>
        /// 公称応力度
        /// </summary>
        public double Sig_n { get; set; }

        /// <summary>
        /// 真歪度
        /// </summary>
        public double Eps_t { get; set; }
        /// <summary>
        /// 公称歪度
        /// </summary>
        public double Eps_n { get; set; }
        /// <summary>
        /// 公称歪度の増分
        /// </summary>
        public double dEps_n { get; set; }

        /// <summary>
        /// 真応力度-真歪度関係における接線剛性
        /// </summary>
        public double E_t { get; set; }
        /// <summary>
        /// 公称応力度-公称歪度関係における接線剛性
        /// </summary>
        public double E_n { get; set; }



        #endregion

        #region 正負ペア

        /// <summary>
        /// 真応力度-真歪度関係における正側バウシンガー部での歪量
        /// </summary>
        public double BausEps_t_pos { get; set; }
        /// <summary>
        /// 真応力度-真歪度関係における負側バウシンガー部での歪量
        /// </summary>
        public double BausEps_t_neg { get; set; }

        /// <summary>
        /// 真応力度-真歪度関係における正側バウシンガー部での折れ曲がり点応力度
        /// </summary>
        public double BausBoundSig_t_pos { get; set; }
        /// <summary>
        /// 真応力度-真歪度関係における負側バウシンガー部での折れ曲がり点応力度
        /// </summary>
        public double BausBoundSig_t_neg { get; set; }

        /// <summary>
        /// 正側骨格復帰応力度
        /// </summary>
        public double RecoverSig_pos { get; set; }
        /// <summary>
        /// 負側骨格復帰応力度
        /// </summary>
        public double RecoverSig_neg { get; set; }

        #endregion

        #region 動的

        /// <summary>
        /// 破壊判定
        /// </summary>
        public bool IsBroken
        {
            get
            {
                if (___IsBroken)
                    return true;
                return ___IsBroken = Sig_t > steel.Sig_u_t;
            }
            set
            {
                ___IsBroken = value;
            }
        }
        private bool ___IsBroken { get; set; }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MemberPiece()
        {

        }

    }

    /// <summary>
    /// 応力度歪度関係の状態
    /// </summary>
    enum SigEpsState
    {
        Elastic = 0,
        PlasticPos = 1,
        PlasticNeg = 2,
        BausFromPos = 3,
        BausFromNeg = 4
    }

    /// <summary>
    /// バウシンガー部の状態
    /// </summary>
    enum BausState
    {
        Baus1 = 1,
        Baus2 = 2,
        Baus3 = 3,
        Baus4 = 4
    }
}
