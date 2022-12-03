

namespace Aki32_Utilities.Console_App_Utilities.SpecificPurposeModels.StructuralEngineering;

/// <summary>
/// Clough Model
/// </summary>
public class CloughModel : ElastoplasticCharacteristicBase
{

    // ★★★★★★★★★★★★★★★ props

    public double K2;

    private double MaxF = 0d;
    private double MaxX = 0d;
    private double MinF = 0d;
    private double MinX = 0d;

    // ★★★★★★★★★★★★★★★ inits

    public CloughModel(double K1, double beta, double Fy)
    {
        this.beta1 = beta;
        this.K1 = K1;
        this.K2 = K1 * beta;
        this.Fy1 = Fy;

        Xy1 = Fy / K1;
        MaxF = Fy;
        MaxX = MaxF / K1;
        MinF = -Fy;
        MinX = MinF / K1;
    }

    // ★★★★★★★★★★★★★★★ methods

    public override double TryCalcNextF(double targetX)
    {
        if (CurrentX == targetX)
            return NextF;

        NextX = targetX;

        #region Calc F

        // Refer .md file
        var dX = NextX - CurrentX;
        var dir = Math.Sign(dX);

        var f1r = K1 * dX + CurrentF;
        var f2 = K2 * (NextX - dir * Xy1) + dir * Fy1;

        double fc; // fc0との兼用

        // 向かってる先でX軸をまたがない／またぐ
        if (dir * CurrentF > 0) // (dir > 0 && CurrentF > 0) || (dir < 0 && CurrentF < 0)
        {
            // fc
            if (NextX > CurrentX)
                fc = CalcF_FromPoints(CurrentX, CurrentF, MaxX, MaxF, NextX);
            else
                fc = CalcF_FromPoints(CurrentX, CurrentF, MinX, MinF, NextX);
        }
        else
        {
            // fc0
            var HitX = CurrentX + (-CurrentF / K1);

            if (NextX > CurrentX)
                fc = CalcF_FromPoints(HitX, 0, MaxX, MaxF, NextX);
            else
                fc = CalcF_FromPoints(HitX, 0, MinX, MinF, NextX);
        }

        // min, max
        var fs = new List<double> { f1r, f2, fc };
        NextF = dir > 0 ? fs.Min() : fs.Max();

        #endregion

        return NextF;
    }

    /// <summary>
    /// (X1,F1),(X2,F2)の2点を通過する直線上の targetX での F を返す。
    /// </summary>
    /// <param name="X1"></param>
    /// <param name="F1"></param>
    /// <param name="X2"></param>
    /// <param name="Y2"></param>
    /// <param name="targetX"></param>
    /// <returns></returns>
    private double CalcF_FromPoints(double X1, double F1, double X2, double Y2, double targetX)
    {
        double maxK = K1;
        double Kc;
        if (X1 == X2)
            Kc = maxK; // 最大にしておくことで，min で最終的に選ばれなくなる。
        else
            Kc = (Y2 - F1) / (X2 - X1);
        Kc = Math.Min(maxK, Kc); // for safety
        return Kc * (targetX - X1) + F1;
    }

    public override void AdoptNextPoint()
    {
        #region update min, max

        if (NextX > MaxX)
        {
            MaxX = NextX;
            MaxF = NextF;
        }
        else if (NextX < MinX)
        {
            MinX = NextX;
            MinF = NextF;
        }

        #endregion

        base.AdoptNextPoint();
    }

    // ★★★★★★★★★★★★★★★

}
