

namespace Aki32_Utilities.StructuralEngineering.DynamicsModels;

[Obsolete]
/// <summary>
/// バイリニアと同じ感じで作ったトリリニア。
/// なんか考え方が違うっポイ
/// </summary>
public class TrilinearModel_Obsolete : ElastoplasticCharacteristicBase
{

    // ★★★★★★★★★★★★★★★ props

    public double K2;
    public double K3;

    public double Fy2;

    public double Xy2 = 0d;


    // ★★★★★★★★★★★★★★★ inits

    public TrilinearModel_Obsolete(double K1, double beta1, double Fy1, double beta2, double Fy2)
    {
        this.beta1 = beta1;
        this.K1 = K1;
        this.K2 = K1 * beta1;
        this.K3 = K1 * beta2;
        this.Fy1 = Fy1;
        this.Fy2 = Fy2;

        Xy1 = Fy1 / K1;
        Xy2 = Xy1 + (Fy2 - Fy1) / K2;

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
        var f3 = K3 * (NextX - dir * Xy2) + dir * Fy2;

        // max, min
        var fs = new List<double> { f1r, f2, f3 };
        NextF = dir > 0 ? fs.Min() : fs.Max();

        #endregion

        return NextF;
    }


    // ★★★★★★★★★★★★★★★

}
