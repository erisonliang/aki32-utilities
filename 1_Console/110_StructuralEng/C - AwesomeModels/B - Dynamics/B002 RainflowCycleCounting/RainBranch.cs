

namespace Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
public class RainBranch
{
    public double SourceMu { get; set; }
    public double CurrentMu { get; set; }
    public bool IsAlive { get; set; } = true;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="sourceMu"></param>
    /// <param name="currentMu"></param>
    public RainBranch(double sourceMu, double currentMu)
    {
        SourceMu = sourceMu;
        CurrentMu = currentMu;
    }

    /// <summary>
    /// Total mu length of this branch
    /// </summary>
    public double TotalMuLength => CurrentMu - SourceMu;

    /// <summary>
    /// Damage by this branch. (Di)
    /// Based on Miner's Law
    /// ※ Divided by 2 since this branch is a half cycle. 
    /// </summary>
    /// <param name="C"></param>
    /// <param name="beta"></param>
    /// <returns></returns>
    public double Damage(double C, double beta) => 1 / RequiredCyclesToCollapse(C, beta) / 2;

    /// <summary>
    /// Number of cycles to collapse. (Nf)
    /// ※ Mu is divided by 2 since referred equation is based on a half amplitude 
    /// </summary>
    /// <param name="C"></param>
    /// <param name="beta"></param>
    /// <returns></returns>
    public double RequiredCyclesToCollapse(double C, double beta) => Math.Pow(TotalMuLength / 2 / C, -1 / beta);

}
