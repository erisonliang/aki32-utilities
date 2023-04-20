

namespace Aki32Utilities.ConsoleAppUtilities.Structure;
/// <summary>
/// Base abstruct class of Elastoplastic Characteristics
/// </summary>
public abstract class ElastoplasticCharacteristicBase : ICloneable
{

    // ★★★★★★★★★★★★★★★ props for all

    /// <summary>
    /// 初期剛性
    /// </summary>
    public double K1;

    public double NextX;
    public double NextF;
    public double NextAverageK
    {
        get
        {
            var dX = NextX - CurrentX;
            var dF = NextF - CurrentF;
            if (dX == 0)
                return CurrentK;
            else
                return Math.Max(Math.Min(K1, Math.Abs(dF / dX)), MIN_K);
        }
    }
    public double CurrentX;
    public double CurrentF;
    public double CurrentK;


    // ★★★★★★★★★★★★★★★ props presets (use only when needed)

    public double alpha = 0;
    public double beta1 = 0;

    public double Fy1 = 0;
    public double Xy1 = 0;

    public heq_EquationTypes heq_EquationType { get; set; } = heq_EquationTypes.None; // JapaneseArchitecturalLaw_S
    public double Current_heq => heq(CurrentX);
    public double Next_heq => heq(NextX);


    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// 次の変位に対する応力を算出してみる。
    /// </summary>
    /// <param name="targetX"></param>
    /// <returns></returns>
    public abstract double TryCalcNextF(double nextX);

    /// <summary>
    /// NextX, NextFの組み合わせを採用。
    /// </summary>
    public virtual void AdoptNextPoint()
    {
        CurrentK = NextAverageK;
        CurrentX = NextX;
        CurrentF = NextF;
    }

    /// <summary>
    /// 等価履歴減衰
    /// </summary>
    /// <param name="targetX"></param>
    /// <returns></returns>
    private double heq(double targetX)
    {
        // for safety
        if (Xy1 == 0)
            return 0;

        var mu = Math.Abs(NextX / Xy1);
        if (mu < 1)
            return 0;

        // TODO:
        switch (heq_EquationType)
        {
            case heq_EquationTypes.None:
                return 0;
            case heq_EquationTypes.JapaneseArchitecturalLaw_W:
            case heq_EquationTypes.JapaneseArchitecturalLaw_S:
                return 0.20 * (1 - Math.Pow(mu, -0.5));
            case heq_EquationTypes.JapaneseArchitecturalLaw_RC:
                return 0.25 * (1 - Math.Pow(mu, -0.5));
            case heq_EquationTypes.BasicModels_Simplified:
                return 2 / Math.PI * (1 - Math.Pow(mu, -0.5));
            case heq_EquationTypes.DegradingModels_Simplified:
                return 1 / Math.PI * (1 - Math.Pow(mu, -0.5));
            case heq_EquationTypes.BilinearModels_Detailed:
                {
                    var top = 2 * (1 - beta1) * (mu - Math.Pow(mu, alpha) * (1 + beta1 * (mu - 1)));
                    var bottom = Math.PI * mu * (1 + beta1 * (mu - 1)) * (1 - beta1 * Math.Pow(mu, alpha));
                    return top / bottom;
                }
            case heq_EquationTypes.CloughModels_Detailed:
                return 1 / Math.PI * (1 - (1 + beta1 * (mu - 1)) / mu * Math.Pow(mu, alpha));
            default:
                throw new NotImplementedException();
        }
    }

    public object Clone()
    {
        return MemberwiseClone();
    }


    // ★★★★★★★★★★★★★★★ const

    internal const double MIN_K = 1e-10;


    // ★★★★★★★★★★★★★★★ enum

    public enum heq_EquationTypes
    {
        /// <summary>
        /// ignore heq
        /// </summary>
        None,

        JapaneseArchitecturalLaw_W,
        JapaneseArchitecturalLaw_S,
        JapaneseArchitecturalLaw_RC,

        BasicModels_Simplified,
        DegradingModels_Simplified,

        BilinearModels_Detailed,
        CloughModels_Detailed,
    }


    // ★★★★★★★★★★★★★★★

}
