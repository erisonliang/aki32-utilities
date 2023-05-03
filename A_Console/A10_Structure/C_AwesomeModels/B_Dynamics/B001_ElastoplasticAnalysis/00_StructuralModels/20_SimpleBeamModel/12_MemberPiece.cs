

using static Aki32Utilities.ConsoleAppUtilities.Structure.MultilinearWithBauschingerModel;

namespace Aki32Utilities.ConsoleAppUtilities.Structure;
public partial class SimpleBeamModel
{
    /// <summary>
    /// 梁の微小要素
    /// </summary>
    class MemberPiece
    {

        // ★★★★★ 単体変数

        /// <summary>
        /// 要素I,Jの素材参照
        /// </summary>
        public Steel Steel { get; set; }

        /// <summary>
        /// 前ステップの状況
        /// </summary>
        public MemberPiece PreviousState { get; set; }

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


        // ★★★★★ 真公称ペア

        /// <summary>
        /// 真応力度
        /// </summary>
        public double Sig_t => Sig_n * (1 + Eps_n);
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


        // ★★★★★ 正負ペア

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


        // ★★★★★ 動的

        /// <summary>
        /// 破壊判定
        /// </summary>
        public bool IsBroken
        {
            get
            {
                if (isBroken)
                    return true;
                return isBroken = Sig_t > Steel.Sig_u_t;
            }
            set
            {
                isBroken = value;
            }
        }
        private bool isBroken { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MemberPiece()
        {
        }

        public void CopyPreviousToCurrent()
        {
            SigEpsState = PreviousState.SigEpsState;
            BausState = PreviousState.BausState;

            TotalEps_t = PreviousState.TotalEps_t;
            TotalPlasticEps_t = PreviousState.TotalPlasticEps_t;
            TotalEps_t_pos = PreviousState.TotalEps_t_pos;
            SigError = PreviousState.SigError;

            Sig_n = PreviousState.Sig_n; ///これだけ特殊！
            // Eps_t
            // Eps_n
            // dEps_n
            E_t = PreviousState.E_t;
            E_n = PreviousState.E_n;

            BausEps_t_pos = PreviousState.BausEps_t_pos;
            BausEps_t_neg = PreviousState.BausEps_t_neg;
            BausBoundSig_t_pos = PreviousState.BausBoundSig_t_pos;
            BausBoundSig_t_neg = PreviousState.BausBoundSig_t_neg;
            RecoverSig_pos = PreviousState.RecoverSig_pos;
            RecoverSig_neg = PreviousState.RecoverSig_neg;
        }

        public void CopyCurrentToPrevious()
        {
            PreviousState.SigEpsState = SigEpsState;
            PreviousState.BausState = BausState;

            PreviousState.TotalEps_t = TotalEps_t;
            //PreviousState.TotalPlasticEps_t = TotalPlasticEps_t;
            //PreviousState.TotalEps_t_pos = TotalEps_t_pos;
            PreviousState.SigError = SigError;

            PreviousState.Sig_n = Sig_n;
            PreviousState.Eps_t = Eps_t;
            PreviousState.Eps_n = Eps_n;
            //PreviousState.dEps_n = dEps_n;
            PreviousState.E_t = E_t;
            PreviousState.E_n = E_n;

            PreviousState.BausEps_t_pos = BausEps_t_pos;
            PreviousState.BausEps_t_neg = BausEps_t_neg;
            PreviousState.BausBoundSig_t_pos = BausBoundSig_t_pos;
            PreviousState.BausBoundSig_t_neg = BausBoundSig_t_neg;
            PreviousState.RecoverSig_pos = RecoverSig_pos;
            PreviousState.RecoverSig_neg = RecoverSig_neg;
        }

        /// <summary>
        /// 前のステップの情報から次のステップの情報を算出します。
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void CalcNext()
        {

            // 収束計算初期状態へ戻す
            CopyPreviousToCurrent();
            Sig_n = PreviousState.Sig_n + dEps_n * PreviousState.E_n; ///これだけ特殊！

            // 差を保存しておく
            var dEps_t = Eps_t - PreviousState.Eps_t;

            // 応力状態に応じて処理
            switch (SigEpsState)
            {
                // 1]弾性域
                case SigEpsState.Elastic:
                    {
                        if (Sig_t >= RecoverSig_pos)
                        {
                            //1
                            E_t = Steel.Steps[1].E_t;
                            E_n = Steel.Steps[1].E_n_t;

                            SigEpsState = SigEpsState.PlasticPos;
                            SigError = Sig_t - RecoverSig_pos;
                        }
                        else if (Sig_t <= RecoverSig_neg)
                        {
                            //2
                            E_t = Steel.Steps[1].E_t;
                            E_n = Steel.Steps[1].E_n_c;

                            SigEpsState = SigEpsState.PlasticNeg;
                            SigError = Sig_t - RecoverSig_neg;
                        }
                    }
                    break;

                // 2]正側塑性域
                case SigEpsState.PlasticPos:
                    {
                        TotalEps_t += dEps_t;
                        TotalEps_t_pos += dEps_t;
                        TotalPlasticEps_t += dEps_t;

                        if (dEps_t < 0)
                        {
                            //3
                            E_t = Steel.Steps[0].E_t;
                            E_n = Steel.Steps[0].E_n_t;

                            RecoverSig_pos = Sig_t;
                            SigEpsState = SigEpsState.BausFromPos;

                            BausBoundSig_t_pos = Steel.ALF * RecoverSig_pos;
                            BausBoundSig_t_neg = Steel.ALF * RecoverSig_neg;
                            BausState = BausState.Baus1;
                            BausEps_t_pos = Eps_t;
                            BausEps_t_neg = Eps_t + ((RecoverSig_neg - RecoverSig_pos) / Steel.Steps[0].E_t) - Steel.BCF * TotalEps_t;

                        }
                        // 2次剛性の変更
                        else
                        {
                            for (int i = 0; i < Steel.Num_of_Data; i++)
                            {
                                if (Sig_t >= Steel.Steps[i].Sig_t)
                                {
                                    //4
                                    E_t = Steel.Steps[i + 1].E_t;
                                    E_n = Steel.Steps[i + 1].E_n_t;
                                }
                            }
                        }

                    }
                    break;

                // 3]負側塑性域
                case SigEpsState.PlasticNeg:
                    {
                        TotalEps_t -= dEps_t;
                        TotalPlasticEps_t -= dEps_t;

                        if (dEps_t > 0)
                        {
                            //5
                            E_t = Steel.Steps[0].E_t;
                            E_n = Steel.Steps[0].E_n_c;

                            RecoverSig_neg = Sig_t;
                            SigEpsState = SigEpsState.BausFromNeg;

                            BausBoundSig_t_pos = Steel.ALF * RecoverSig_pos;
                            BausBoundSig_t_neg = Steel.ALF * RecoverSig_neg;
                            BausState = BausState.Baus1;
                            BausEps_t_pos = Eps_t + (RecoverSig_pos - RecoverSig_neg) / Steel.Steps[0].E_t + Steel.BCF * TotalEps_t;
                            BausEps_t_neg = Eps_t;

                        }
                        // 2次剛性の変更
                        else
                        {
                            for (int i = 0; i < Steel.Num_of_Data; i++)
                            {
                                if (Sig_t <= -Steel.Steps[i].Sig_t)
                                {
                                    //6
                                    E_t = Steel.Steps[i + 1].E_t;
                                    E_n = Steel.Steps[i + 1].E_n_c;
                                }
                            }
                        }
                    }
                    break;

                // 4]除荷＆バウシンガ[正→負]
                case SigEpsState.BausFromPos:
                    {
                        switch (BausState)
                        {
                            case BausState.Baus1:
                                {

                                    if (Sig_t <= 0)
                                    {
                                        RecoverSig_pos -= SigError;
                                        SigError = 0.0;
                                    }

                                    if (Sig_t >= RecoverSig_pos)
                                    {
                                        for (int i = 0; i < Steel.Num_of_Data; i++)
                                        {
                                            if (Sig_t >= Steel.Steps[i].Sig_t)
                                            {
                                                //7
                                                E_t = Steel.Steps[i + 1].E_t;
                                                E_n = Steel.Steps[i + 1].E_n_t;
                                            }
                                        }

                                        SigError = Sig_t - RecoverSig_pos;
                                        SigEpsState = SigEpsState.PlasticPos;

                                    }
                                    else if (Sig_t <= BausBoundSig_t_neg)
                                    {
                                        //8
                                        E_t = (RecoverSig_neg - BausBoundSig_t_neg) / (BausEps_t_neg - Eps_t);
                                        E_n = (RecoverSig_neg / Math.Exp(BausEps_t_neg) - BausBoundSig_t_neg / Math.Exp(Eps_t)) / ((Math.Exp(BausEps_t_neg) - 1) - (Math.Exp(Eps_t) - 1));

                                        BausState = BausState.Baus2;
                                    }
                                }
                                break;

                            case BausState.Baus2:
                                {
                                    TotalPlasticEps_t -= dEps_t;

                                    if (Sig_t <= RecoverSig_neg)
                                    {
                                        for (int i = 0; i < Steel.Num_of_Data; i++)
                                        {
                                            if (Sig_t <= -1 * Steel.Steps[i].Sig_t)
                                            {
                                                //9
                                                E_t = Steel.Steps[i + 1].E_t;
                                                E_n = Steel.Steps[i + 1].E_n_c;
                                            }
                                        }

                                        SigError = Sig_t - RecoverSig_neg;
                                        SigEpsState = SigEpsState.PlasticNeg;
                                    }
                                    else if (dEps_t > 0)
                                    {
                                        //10
                                        E_t = Steel.Steps[0].E_t;
                                        E_n = Steel.Steps[0].E_n_c;

                                        BausState = BausState.Baus3;
                                        BausBoundSig_t_neg = Sig_t;
                                    }
                                }
                                break;

                            case BausState.Baus3:
                                {
                                    if (Sig_t <= BausBoundSig_t_neg)
                                    {
                                        BausBoundSig_t_pos = Steel.ALF * RecoverSig_pos;
                                        //11
                                        E_t = (RecoverSig_neg - BausBoundSig_t_neg) / (BausEps_t_neg - Eps_t);
                                        E_n = (RecoverSig_neg / Math.Exp(BausEps_t_neg) - BausBoundSig_t_neg / Math.Exp(Eps_t)) / ((Math.Exp(BausEps_t_neg) - 1) - (Math.Exp(Eps_t) - 1));

                                        BausState = BausState.Baus2;
                                    }
                                    else if (Sig_t >= BausBoundSig_t_pos)
                                    {
                                        BausBoundSig_t_neg = Steel.ALF * RecoverSig_neg;
                                        //12
                                        E_t = (RecoverSig_pos - BausBoundSig_t_pos) / (BausEps_t_pos - Eps_t);
                                        E_n = (RecoverSig_pos / Math.Exp(BausEps_t_pos) - BausBoundSig_t_pos / Math.Exp(Eps_t)) / ((Math.Exp(BausEps_t_pos) - 1) - (Math.Exp(Eps_t) - 1));

                                        BausState = BausState.Baus4;

                                    }
                                }
                                break;

                            case BausState.Baus4:
                                {
                                    TotalPlasticEps_t += dEps_t;

                                    if (Sig_t >= RecoverSig_pos)
                                    {
                                        for (int IB = 0; IB < Steel.Num_of_Data; IB++)
                                        {
                                            if (Sig_t >= Steel.Steps[IB].Sig_t)
                                            {
                                                //13
                                                E_t = Steel.Steps[IB + 1].E_t;
                                                E_n = Steel.Steps[IB + 1].E_n_t;
                                            }
                                        }

                                        SigError = Sig_t - RecoverSig_pos;
                                        SigEpsState = SigEpsState.PlasticPos;

                                    }
                                    else if (dEps_t < 0)
                                    {
                                        //14
                                        E_t = Steel.Steps[0].E_t;
                                        E_n = Steel.Steps[0].E_n_t;

                                        BausState = BausState.Baus3;
                                        BausBoundSig_t_pos = Sig_t;
                                    }
                                }
                                break;

                            default:
                                throw new Exception("BausState未定義状態");
                        }
                    }
                    break;

                // 5]除荷＆バウシンガー[負→正]
                case SigEpsState.BausFromNeg:
                    {
                        switch (BausState)
                        {
                            case BausState.Baus1:
                                {
                                    if (Sig_t >= 0)
                                    {
                                        RecoverSig_neg -= SigError;
                                        SigError = 0;
                                    }

                                    if (Sig_t <= RecoverSig_neg)
                                    {
                                        for (int i = 0; i < Steel.Num_of_Data; i++)
                                        {
                                            if (Sig_t < -1 * Steel.Steps[i].Sig_t)
                                            {
                                                //15
                                                E_t = Steel.Steps[i + 1].E_t;
                                                E_n = Steel.Steps[i + 1].E_n_c;
                                            }
                                        }

                                        SigError = Sig_t - RecoverSig_neg;
                                        SigEpsState = SigEpsState.PlasticNeg;
                                    }
                                    else if (Sig_t >= BausBoundSig_t_pos)
                                    {
                                        //16
                                        E_t = (RecoverSig_pos - BausBoundSig_t_pos) / (BausEps_t_pos - Eps_t);
                                        E_n = (RecoverSig_pos / Math.Exp(BausEps_t_pos) - BausBoundSig_t_pos / Math.Exp(Eps_t)) / ((Math.Exp(BausEps_t_pos) - 1) - (Math.Exp(Eps_t) - 1));

                                        BausState = BausState.Baus2;
                                    }
                                }
                                break;

                            case BausState.Baus2:
                                {
                                    TotalPlasticEps_t += dEps_t;

                                    if (Sig_t >= RecoverSig_pos)
                                    {
                                        for (int i = 0; i < Steel.Num_of_Data; i++)
                                        {
                                            if (Sig_t >= Steel.Steps[i].Sig_t)
                                            {
                                                //17
                                                E_t = Steel.Steps[i + 1].E_t;
                                                E_n = Steel.Steps[i + 1].E_n_t;
                                            }
                                            SigError = Sig_t - RecoverSig_pos;
                                            SigEpsState = SigEpsState.PlasticPos;
                                        }
                                    }
                                    else if (dEps_t < 0)
                                    {
                                        //18
                                        E_t = Steel.Steps[0].E_t;
                                        E_n = Steel.Steps[0].E_n_t;

                                        BausState = BausState.Baus3;
                                        BausBoundSig_t_pos = Sig_t;
                                    }

                                }
                                break;

                            case BausState.Baus3:
                                {
                                    if (Sig_t >= BausBoundSig_t_pos)
                                    {

                                        BausBoundSig_t_neg = Steel.ALF * RecoverSig_neg;
                                        //19
                                        E_t = (RecoverSig_pos - BausBoundSig_t_pos) / (BausEps_t_pos - Eps_t);
                                        E_n = (RecoverSig_pos / Math.Exp(BausEps_t_pos) - BausBoundSig_t_pos / Math.Exp(Eps_t)) / ((Math.Exp(BausEps_t_pos) - 1) - (Math.Exp(Eps_t) - 1));

                                        BausState = BausState.Baus2;

                                    }
                                    else if (Sig_t <= BausBoundSig_t_neg)
                                    {

                                        BausBoundSig_t_pos = Steel.ALF * RecoverSig_pos;
                                        //20
                                        E_t = (RecoverSig_neg - BausBoundSig_t_neg) / (BausEps_t_neg - Eps_t);
                                        E_n = (RecoverSig_neg / Math.Exp(BausEps_t_neg) - BausBoundSig_t_neg / Math.Exp(Eps_t)) / ((Math.Exp(BausEps_t_neg) - 1) - (Math.Exp(Eps_t) - 1));

                                        BausState = BausState.Baus4;
                                    }
                                }
                                break;

                            case BausState.Baus4:
                                {
                                    TotalPlasticEps_t -= dEps_t;

                                    if (Sig_t <= RecoverSig_neg)
                                    {
                                        for (int i = 0; i < Steel.Num_of_Data; i++)
                                        {
                                            if (Sig_t <= -1 * Steel.Steps[i].Sig_t)
                                            {
                                                //21
                                                E_t = Steel.Steps[i + 1].E_t;
                                                E_n = Steel.Steps[i + 1].E_n_c;
                                            }
                                        }

                                        SigError = Sig_t - RecoverSig_neg;
                                        SigEpsState = SigEpsState.PlasticNeg;

                                    }
                                    else if (dEps_t > 0)
                                    {
                                        //22
                                        E_t = Steel.Steps[0].E_t;
                                        E_n = Steel.Steps[0].E_n_c;

                                        BausState = BausState.Baus3;
                                        BausBoundSig_t_neg = Sig_t;
                                    }
                                }
                                break;

                            default:
                                throw new Exception("BausState未定義状態");
                        }
                    }
                    break;

                default:
                    throw new Exception("SigEpsState未定義状態");
            }

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
