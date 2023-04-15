

namespace Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
/// <summary>
/// Elastic Tetralinear Model
/// Follows skelton curve when restoring
/// </summary>
public class ElasticTetralinearModel : ElastoplasticCharacteristicBase
{

    // ★★★★★★★★★★★★★★★ props

    public double K2;
    public double K3;
    public double K4;

    public double Fy2;
    public double Fy3;

    public double Xy2 = 0d;
    public double Xy3 = 0d;


    // ★★★★★★★★★★★★★★★ inits

    public ElasticTetralinearModel(double K1, double beta1, double Fy1, double beta2, double Fy2, double beta3, double Fy3)
    {
        this.beta1 = beta1;
        this.K1 = K1;
        this.K2 = K1 * beta1;
        this.K3 = K1 * beta2;
        this.K4 = K1 * beta3;
        this.Fy1 = Fy1;
        this.Fy2 = Fy2;
        this.Fy3 = Fy3;

        Xy1 = Fy1 / K1;
        Xy2 = Xy1 + (Fy2 - Fy1) / K2;
        Xy3 = Xy2 + (Fy3 - Fy2) / K3;
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
        var f4 = K4 * (NextX - sign * Xy3) + sign * Fy3;

        // max, min
        var fs = new List<double> { f1, f2, f3, f4 };
        NextF = sign > 0 ? fs.Min() : fs.Max();

        #endregion

        return NextF;
    }


    // ★★★★★★★★★★★★★★★

}
