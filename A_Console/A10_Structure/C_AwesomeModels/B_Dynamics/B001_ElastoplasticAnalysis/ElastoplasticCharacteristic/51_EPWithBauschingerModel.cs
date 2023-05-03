using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.Structure;
/// <summary>
/// EP Model with Bauschinger
/// </summary>
public class EPWithBauschingerModel : ElastoplasticCharacteristicBase
{

    // ★★★★★★★★★★★★★★★ props

    // ★★★★★ 単体変数

    /// <summary>
    /// 要素I,Jの素材参照
    /// </summary>
    public Material_Steel Steel { get; set; }

    /// <summary>
    /// 前ステップの状況
    /// </summary>
    public EPWithBauschingerModel PreviousState { get; set; }

    /// <summary>
    /// バウシンガー部における状態を表す変数
    /// </summary>
    public BausStates BausState { get; set; }

    /// <summary>
    /// 応力度歪度関係における状態を表す変数
    /// </summary>
    public SigEpsStates SigEpsState { get; set; }


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


    // ★★★★★★★★★★★★★★★ inits

    public EPWithBauschingerModel()
    {
        //this.beta1 = beta;
        //this.K1 = K1;
        //this.K2 = K1 * beta;
        //this.Fy1 = Fy;

        //Xy1 = Fy / K1;

        //CurrentK = K1;










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
        // 先に必要な要素を取得
        var dEps_t = Eps_t - PreviousState.Eps_t;
        Sig_n = PreviousState.Sig_n + dEps_n * PreviousState.E_n;

        // 収束計算初期状態へ戻す
        CopyPreviousToCurrent();

        // 応力状態に応じて処理
        switch (SigEpsState)
        {
            // 1]弾性域
            case SigEpsStates.Elastic:
                {
                    if (Sig_t >= RecoverSig_pos)
                    {
                        //1
                        E_t = Steel.Steps[1].E_t;
                        E_n = Steel.Steps[1].E_n_t;

                        SigEpsState = SigEpsStates.PlasticPos;
                        SigError = Sig_t - RecoverSig_pos;
                    }
                    else if (Sig_t <= RecoverSig_neg)
                    {
                        //2
                        E_t = Steel.Steps[1].E_t;
                        E_n = Steel.Steps[1].E_n_c;

                        SigEpsState = SigEpsStates.PlasticNeg;
                        SigError = Sig_t - RecoverSig_neg;
                    }
                }
                break;

            // 2]正側塑性域
            case SigEpsStates.PlasticPos:
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
                        SigEpsState = SigEpsStates.BausFromPos;

                        BausBoundSig_t_pos = Steel.ALF * RecoverSig_pos;
                        BausBoundSig_t_neg = Steel.ALF * RecoverSig_neg;
                        BausState = BausStates.Baus1;
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
            case SigEpsStates.PlasticNeg:
                {
                    TotalEps_t -= dEps_t;
                    TotalPlasticEps_t -= dEps_t;

                    if (dEps_t > 0)
                    {
                        //5
                        E_t = Steel.Steps[0].E_t;
                        E_n = Steel.Steps[0].E_n_c;

                        RecoverSig_neg = Sig_t;
                        SigEpsState = SigEpsStates.BausFromNeg;

                        BausBoundSig_t_pos = Steel.ALF * RecoverSig_pos;
                        BausBoundSig_t_neg = Steel.ALF * RecoverSig_neg;
                        BausState = BausStates.Baus1;
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
            case SigEpsStates.BausFromPos:
                {
                    switch (BausState)
                    {
                        case BausStates.Baus1:
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
                                    SigEpsState = SigEpsStates.PlasticPos;

                                }
                                else if (Sig_t <= BausBoundSig_t_neg)
                                {
                                    //8
                                    E_t = (RecoverSig_neg - BausBoundSig_t_neg) / (BausEps_t_neg - Eps_t);
                                    E_n = (RecoverSig_neg / Math.Exp(BausEps_t_neg) - BausBoundSig_t_neg / Math.Exp(Eps_t)) / ((Math.Exp(BausEps_t_neg) - 1) - (Math.Exp(Eps_t) - 1));

                                    BausState = BausStates.Baus2;
                                }
                            }
                            break;

                        case BausStates.Baus2:
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
                                    SigEpsState = SigEpsStates.PlasticNeg;
                                }
                                else if (dEps_t > 0)
                                {
                                    //10
                                    E_t = Steel.Steps[0].E_t;
                                    E_n = Steel.Steps[0].E_n_c;

                                    BausState = BausStates.Baus3;
                                    BausBoundSig_t_neg = Sig_t;
                                }
                            }
                            break;

                        case BausStates.Baus3:
                            {
                                if (Sig_t <= BausBoundSig_t_neg)
                                {
                                    BausBoundSig_t_pos = Steel.ALF * RecoverSig_pos;
                                    //11
                                    E_t = (RecoverSig_neg - BausBoundSig_t_neg) / (BausEps_t_neg - Eps_t);
                                    E_n = (RecoverSig_neg / Math.Exp(BausEps_t_neg) - BausBoundSig_t_neg / Math.Exp(Eps_t)) / ((Math.Exp(BausEps_t_neg) - 1) - (Math.Exp(Eps_t) - 1));

                                    BausState = BausStates.Baus2;
                                }
                                else if (Sig_t >= BausBoundSig_t_pos)
                                {
                                    BausBoundSig_t_neg = Steel.ALF * RecoverSig_neg;
                                    //12
                                    E_t = (RecoverSig_pos - BausBoundSig_t_pos) / (BausEps_t_pos - Eps_t);
                                    E_n = (RecoverSig_pos / Math.Exp(BausEps_t_pos) - BausBoundSig_t_pos / Math.Exp(Eps_t)) / ((Math.Exp(BausEps_t_pos) - 1) - (Math.Exp(Eps_t) - 1));

                                    BausState = BausStates.Baus4;

                                }
                            }
                            break;

                        case BausStates.Baus4:
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
                                    SigEpsState = SigEpsStates.PlasticPos;

                                }
                                else if (dEps_t < 0)
                                {
                                    //14
                                    E_t = Steel.Steps[0].E_t;
                                    E_n = Steel.Steps[0].E_n_t;

                                    BausState = BausStates.Baus3;
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
            case SigEpsStates.BausFromNeg:
                {
                    switch (BausState)
                    {
                        case BausStates.Baus1:
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
                                    SigEpsState = SigEpsStates.PlasticNeg;
                                }
                                else if (Sig_t >= BausBoundSig_t_pos)
                                {
                                    //16
                                    E_t = (RecoverSig_pos - BausBoundSig_t_pos) / (BausEps_t_pos - Eps_t);
                                    E_n = (RecoverSig_pos / Math.Exp(BausEps_t_pos) - BausBoundSig_t_pos / Math.Exp(Eps_t)) / ((Math.Exp(BausEps_t_pos) - 1) - (Math.Exp(Eps_t) - 1));

                                    BausState = BausStates.Baus2;
                                }
                            }
                            break;

                        case BausStates.Baus2:
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
                                        SigEpsState = SigEpsStates.PlasticPos;
                                    }
                                }
                                else if (dEps_t < 0)
                                {
                                    //18
                                    E_t = Steel.Steps[0].E_t;
                                    E_n = Steel.Steps[0].E_n_t;

                                    BausState = BausStates.Baus3;
                                    BausBoundSig_t_pos = Sig_t;
                                }

                            }
                            break;

                        case BausStates.Baus3:
                            {
                                if (Sig_t >= BausBoundSig_t_pos)
                                {

                                    BausBoundSig_t_neg = Steel.ALF * RecoverSig_neg;
                                    //19
                                    E_t = (RecoverSig_pos - BausBoundSig_t_pos) / (BausEps_t_pos - Eps_t);
                                    E_n = (RecoverSig_pos / Math.Exp(BausEps_t_pos) - BausBoundSig_t_pos / Math.Exp(Eps_t)) / ((Math.Exp(BausEps_t_pos) - 1) - (Math.Exp(Eps_t) - 1));

                                    BausState = BausStates.Baus2;

                                }
                                else if (Sig_t <= BausBoundSig_t_neg)
                                {

                                    BausBoundSig_t_pos = Steel.ALF * RecoverSig_pos;
                                    //20
                                    E_t = (RecoverSig_neg - BausBoundSig_t_neg) / (BausEps_t_neg - Eps_t);
                                    E_n = (RecoverSig_neg / Math.Exp(BausEps_t_neg) - BausBoundSig_t_neg / Math.Exp(Eps_t)) / ((Math.Exp(BausEps_t_neg) - 1) - (Math.Exp(Eps_t) - 1));

                                    BausState = BausStates.Baus4;
                                }
                            }
                            break;

                        case BausStates.Baus4:
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
                                    SigEpsState = SigEpsStates.PlasticNeg;

                                }
                                else if (dEps_t > 0)
                                {
                                    //22
                                    E_t = Steel.Steps[0].E_t;
                                    E_n = Steel.Steps[0].E_n_c;

                                    BausState = BausStates.Baus3;
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

    /// <summary>
    /// 応力度歪度関係の状態
    /// </summary>
    public enum SigEpsStates
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
    public enum BausStates
    {
        Baus1 = 1,
        Baus2 = 2,
        Baus3 = 3,
        Baus4 = 4
    }


    // ★★★★★★★★★★★★★★★ methods

    public override double TryCalcNextF(double nextX)
    {
        if (CurrentX == nextX)
            return CurrentF;

        NextX = nextX;

        #region Calc F

        // Refer .md file
        var dX = NextX - CurrentX;
        var dir = Math.Sign(dX);

        var f1r = K1 * dX + CurrentF;
        var f2 = K2 * (NextX - dir * Xy1) + dir * Fy1;

        // max, min
        var fs = new List<double> { f1r, f2 };
        NextF = dir > 0 ? fs.Min() : fs.Max();

        #endregion

        return NextF;
    }


    // ★★★★★★★★★★★★★★★ classes

    public class Material_Steel
    {

        /// <summary>
        /// テストピースの名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 鋼材のσ-ε関係の諸元の集合体
        /// </summary>
        public List<SteelStepInfo> Steps { get; set; }

        /// <summary>
        /// 鋼材の真応力度-真歪度関係(骨格曲線)における降伏点
        /// ※ 塑性化の開始を判断する応力度なので、降伏点ではなく比例限界を入力しておく
        /// </summary>
        public int Sig_y_t { get; set; }
        /// <summary>
        /// バウシンガー関連の計数。.md 参照
        /// </summary>
        public double BCF { get; set; }
        /// <summary>
        /// バウシンガー関連の計数。.md 参照
        /// </summary>
        public double ALF { get; set; }

        /// <summary>
        /// 鋼材の真応力度-真歪度関係(骨格曲線)のデーター数
        /// </summary>
        public int Num_of_Data => Steps.Count;

        /// <summary>
        /// 鋼材の破断応力度(真応力度最大値)
        /// </summary>
        public double Sig_u_t => Steps.Last().Sig_t;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">部材名</param>
        /// <param name="monoEPDataFile">単調載荷時の弾塑性特性データの場所</param>
        /// <param name="sig_y_t">規準となる降伏応力度の手動入力</param>
        /// <param name="BCF"></param>
        /// <param name="ALF"></param>
        public Material_Steel(string name, FileInfo monoEPDataFile, int sig_y_t, double BCF, double ALF)
        {
            Name = name;
            Steps = new List<SteelStepInfo>();

            //データ読み込み
            var EP = TimeHistory.FromCsv(monoEPDataFile);

            double prev_sig_t = 0d;
            double prev_eps_t = 0d;

            for (int i = 0; i < EP.DataRowCount; i++)
            {
                var eps_t = EP[0][i];
                var sig_t = EP[1][i];

                if (eps_t == 0)
                    continue;

                var e_t = (sig_t - prev_sig_t) / (eps_t - prev_eps_t);
                var e_n_t = (sig_t / Math.Exp(eps_t) - prev_sig_t / Math.Exp(prev_eps_t)) / ((Math.Exp(eps_t) - 1) - (Math.Exp(prev_eps_t) - 1));
                var e_n_c = (-sig_t / Math.Exp(-eps_t) + prev_sig_t / Math.Exp(-prev_eps_t)) / ((Math.Exp(-eps_t) - 1) - (Math.Exp(-prev_eps_t) - 1));

                Steps.Add(new SteelStepInfo(eps_t, sig_t, e_t, e_n_t, e_n_c));

                prev_eps_t = eps_t;
                prev_sig_t = sig_t;
            }

            Sig_y_t = sig_y_t;

            this.BCF = BCF;
            this.ALF = ALF;
        }

        public class SteelStepInfo
        {

            /// <summary>
            /// 鋼材の歪度(真応力度-真歪度関係_骨格曲線)
            /// </summary>
            public double Eps_t { get; set; }
            /// <summary>
            /// 鋼材の応力度(真応力度-真歪度関係_骨格曲線)
            /// </summary>
            public double Sig_t { get; set; }

            /// <summary>
            /// 鋼材の剛性(真応力度-真歪度関係_骨格曲線)
            /// </summary>
            public double E_t { get; set; }
            /// <summary>
            /// 鋼材の剛性(工学応力度-工学歪度関係_引張側骨格曲線)
            /// </summary>
            public double E_n_t { get; set; }
            /// <summary>
            /// 鋼材の剛性(工学応力度-工学歪度関係_圧縮側骨格曲線)
            /// </summary>
            public double E_n_c { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="eps_t"></param>
            /// <param name="sig_t"></param>
            /// <param name="e_t"></param>
            /// <param name="e_n_t"></param>
            /// <param name="e_n_c"></param>
            public SteelStepInfo(double eps_t, double sig_t, double e_t, double e_n_t, double e_n_c)
            {
                Eps_t = eps_t;
                Sig_t = sig_t;
                E_t = e_t;
                E_n_t = e_n_t;
                E_n_c = e_n_c;
            }

        }

    }


    // ★★★★★★★★★★★★★★★

}
