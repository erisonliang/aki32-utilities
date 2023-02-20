

namespace Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;

/// <summary>
/// Normal Bilinear Model with Kinematic Hardening (移動硬化)
/// </summary>
public class BilinearModel : ElastoplasticCharacteristicBase
{

    // ★★★★★★★★★★★★★★★ props

    public double K2;


    // ★★★★★★★★★★★★★★★ inits

    public BilinearModel(double K1, double beta, double Fy)
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
