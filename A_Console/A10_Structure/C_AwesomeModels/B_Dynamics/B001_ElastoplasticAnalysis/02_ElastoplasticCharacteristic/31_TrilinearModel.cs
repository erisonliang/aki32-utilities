

namespace Aki32Utilities.ConsoleAppUtilities.Structure;
/// <summary>
/// Normal Trilinear Model with Kinematic Hardening (移動硬化)
/// </summary>
public class TrilinearModel : ElastoplasticCharacteristicBase
{

    // ★★★★★★★★★★★★★★★ props

    public double K2;
    public double K3;

    public double Fy2;

    public double Xy2 = 0d;


    // ★★★★★★★★★★★★★★★ inits

    public TrilinearModel(double K1, double beta1, double Fy1, double beta2, double Fy2)
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
        var f2r = K2 * dX + CurrentF;

        var TempX = CurrentX + dir * (Xy2 - Xy1) * 2;
        var TempF = CurrentF + dir * (Fy2 - Fy1) * 2;
        var HitX = (K1 * TempX - TempF + dir * (Fy2 - K3 * Xy2)) / (K1 - K3);
        var HitF = TempF + K1 * (HitX - TempX);
        var f2c = K2 * (NextX - HitX) + HitF;

        var f3 = K3 * (NextX - dir * Xy2) + dir * Fy2;

        // max, min
        var f2rcList = new List<double> { f2, f2r, f2c };
        var f2rc = dir > 0 ? f2rcList.Max() : f2rcList.Min();
        var fs = new List<double> { f1r, f2rc, f3 };
        NextF = dir > 0 ? fs.Min() : fs.Max();

        #endregion

        return NextF;
    }


    // ★★★★★★★★★★★★★★★

}
