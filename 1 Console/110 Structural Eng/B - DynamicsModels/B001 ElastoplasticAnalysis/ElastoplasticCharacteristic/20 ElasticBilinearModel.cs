

using DocumentFormat.OpenXml.Wordprocessing;

namespace Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;

/// <summary>
/// Elastic Bilinear Model
/// Follows skelton curve when restoring
/// </summary>
public class ElasticBilinearModel : ElastoplasticCharacteristicBase
{

    // ★★★★★★★★★★★★★★★ props

    public double K2;


    // ★★★★★★★★★★★★★★★ inits

    public ElasticBilinearModel(double K1, double beta, double Fy)
    {
        this.beta1 = beta;
        this.K1 = K1;
        this.K2 = K1 * beta;
        this.Fy1 = Fy;

        Xy1 = Fy / K1;
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

        // max, min
        var fs = new List<double> { f1, f2 };
        NextF = sign > 0 ? fs.Min() : fs.Max();

        #endregion

        return NextF;
    }


    // ★★★★★★★★★★★★★★★

}
