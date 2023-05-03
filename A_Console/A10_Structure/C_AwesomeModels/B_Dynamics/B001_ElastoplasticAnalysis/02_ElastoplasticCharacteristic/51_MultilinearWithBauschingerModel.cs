using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.Structure;
/// <summary>
/// EP Model with Bauschinger
/// </summary>
public class MultilinearWithBauschingerModel : ElastoplasticCharacteristicBase
{

    // ★★★★★★★★★★★★★★★ props

    #region props

    // ★★★★★ 単体変数

    /// <summary>
    /// 素材
    /// </summary>
    public Material.Steel Steel { get; set; }

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
    /// 弾塑性判定における応力度の誤差
    /// </summary>
    public double SigError { get; set; }

    /// <summary>
    /// バウシンガー関連の計数。.md 参照
    /// </summary>
    public double BCF { get; set; }
    /// <summary>
    /// バウシンガー関連の計数。.md 参照
    /// </summary>
    public double ALF { get; set; }


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


    #endregion


    // ★★★★★★★★★★★★★★★ inits

    public MultilinearWithBauschingerModel(double K1, double beta, double Fy)
    {
        this.beta1 = beta;
        this.K1 = K1;
        this.K2 = K1 * beta;
        this.Fy1 = Fy;

        Xy1 = Fy / K1;

        CurrentK = K1;
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


    // ★★★★★★★★★★★★★★★

}
