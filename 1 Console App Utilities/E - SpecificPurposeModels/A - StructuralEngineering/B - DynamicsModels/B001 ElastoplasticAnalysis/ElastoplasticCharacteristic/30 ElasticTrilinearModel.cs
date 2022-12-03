

namespace Aki32_Utilities.Console_App_Utilities.SpecificPurposeModels.StructuralEngineering;

/// <summary>
/// Elastic Trilinear Model
/// Follows skelton curve when restoring
/// </summary>
public class ElasticTrilinearModel : ElastoplasticCharacteristicBase
{

    // ★★★★★★★★★★★★★★★ props

    public double K2;
    public double K3;

    public double Fy2;

    public double Xy2 = 0d;


    // ★★★★★★★★★★★★★★★ inits

    public ElasticTrilinearModel(double K1, double beta1, double Fy1, double beta2, double Fy2)
    {
        this.beta1 = beta1;
        this.K1 = K1;
        this.K2 = K1 * beta1;
        this.K3 = K1 * beta2;
        this.Fy1 = Fy1;
        this.Fy2 = Fy2;

        Xy1 = Fy1 / K1;
        Xy2 = Xy1 + (Fy2 - Fy1) / K2;
    }


    // ★★★★★★★★★★★★★★★ methods

    public override double TryCalcNextF(double targetX)
    {
        if (CurrentX == targetX)
            return NextF;

        NextX = targetX;

        #region Calc F

        // Refer .md file
        var sign = Math.Sign(NextX);

        var f1 = K1 * NextX;
        var f2 = K2 * (NextX - sign * Xy1) + sign * Fy1;
        var f3 = K3 * (NextX - sign * Xy2) + sign * Fy2;

        // max, min
        var fs = new List<double> { f1, f2, f3 };
        NextF = sign > 0 ? fs.Min() : fs.Max();

        #endregion

        return NextF;
    }


    // ★★★★★★★★★★★★★★★

}
