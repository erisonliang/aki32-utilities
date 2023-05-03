using System.Threading.Channels;

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
    /// 材料初期長さ
    /// </summary>
    public double L { get; set; }
    /// <summary>
    /// 材料断面積
    /// </summary>
    public double A { get; set; }

    /// <summary>
    /// バウシンガー関連の計数。.md 参照
    /// </summary>
    public double BCF { get; set; }
    /// <summary>
    /// バウシンガー関連の計数。.md 参照
    /// </summary>
    public double ALF { get; set; }


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
            return isBroken = CurrentF > CalcF(Steel.Sig_u_t);
        }
        set
        {
            isBroken = value;
        }
    }
    private bool isBroken { get; set; }

    public double CalcX(double eps) => eps * L;
    public double CalcF(double sig) => sig * A;


    #endregion


    // ★★★★★★★★★★★★★★★ inits

    public MultilinearWithBauschingerModel(Material.Steel steel, double L, double A, double BCF = 0.33, double ALF = 0.67)
    {
        throw new NotImplementedException("not yet implemented");

        Steel = steel;

        this.L = L;
        this.A = A;

        this.BCF = BCF;
        this.ALF = ALF;

        //this.beta1 = beta;
        //this.K1 = K1;
        //this.Fy1 = Fy;

        //Xy1 = Fy / K1;

        //CurrentK = K1;



    }


    // ★★★★★★★★★★★★★★★ methods

    public override double TryCalcNextF(double nextX)
    {
        throw new NotImplementedException("not yet implemented");

        if (CurrentX == nextX)
            return CurrentF;

        NextX = nextX;

        #region Calc F

        // Refer .md file
        var dX = NextX - CurrentX;
        var dir = Math.Sign(dX);

        var f1r = K1 * dX + CurrentF;
        var f2 = 1;

        // max, min
        var fs = new List<double> { f1r, f2 };
        NextF = dir > 0 ? fs.Min() : fs.Max();

        #endregion

        return NextF;
    }


    // ★★★★★★★★★★★★★★★

}
