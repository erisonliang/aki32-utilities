

namespace Aki32Utilities.ConsoleAppUtilities.SpecificPurposeModels.StructuralEngineering;
public class RainBranches
{
    public List<RainBranch> RainBranchList { get; init; }

    private bool IsTargetPositive { get; set; }
    private string ITPString => IsTargetPositive ? "+" : "-";


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="isTargetPositive"></param>
    public RainBranches(bool isTargetPositive)
    {
        RainBranchList = new List<RainBranch>();
        IsTargetPositive = isTargetPositive;
    }


    /// <summary>
    /// Calc Next Step
    /// </summary>
    /// <param name="lastMu"></param>
    /// <param name="currentMu"></param>
    public void CalcNext(double lastMu, double currentMu, bool consoleOutput = false)
    {
        if (!IsTargetPositive)
        {
            // If this instance is for negative branches, reverse all input data on axis.
            currentMu = -currentMu;
            lastMu = -lastMu;
        }

        if (currentMu - lastMu <= 0)
        {
            // Skip if orientation is not for this branches.
            return;
        }
        else if (RainBranchList.Count == 0)
        {
            // For initial iniput, create the first branch 
            RainBranchList.Add(new RainBranch(lastMu, currentMu));
            if (consoleOutput)
            {
                Console.WriteLine($"  branch {ITPString}{RainBranchList.Count - 1}, created");
                Console.WriteLine($"  branch {ITPString}{RainBranchList.Count - 1}, {ITPString}{currentMu - lastMu}");
            }
        }
        else
        {
            // Connect to existing branches

            // If there is no appendable branch, create one
            if (!RainBranchList.Where(x => x.IsAlive).Any(x => x.CurrentMu == lastMu))
            {
                RainBranchList.Add(new RainBranch(lastMu, lastMu));
                if (consoleOutput)
                    Console.WriteLine($"  branch {ITPString}{RainBranchList.Count - 1}, created");
            }

            // List all branches ending within [lastMu, currentMu]
            var candidateBranches = RainBranchList
                .Where(x => x.IsAlive)
                .Where(x => x.CurrentMu >= lastMu)
                .Where(x => x.CurrentMu <= currentMu)
                .ToList();

            // Sort
            candidateBranches.Sort((x, y) => x.CurrentMu.CompareTo(y.CurrentMu));

            var calculatingMu = lastMu;
            while (true)
            {

                // If tehre is no one to fight, connect the rest of mu.
                if (candidateBranches.Count == 1)
                {
                    if (consoleOutput)
                        Console.WriteLine($"  branch {ITPString}{RainBranchList.IndexOf(candidateBranches[0])}, {ITPString}{currentMu - candidateBranches[0].CurrentMu}");
                    candidateBranches[0].CurrentMu = currentMu;
                    break;
                }

                // Connect to next fight point
                var branch0 = candidateBranches[0];
                var branch1 = candidateBranches[1];

                calculatingMu = candidateBranches[1].CurrentMu;
                if (consoleOutput)
                    Console.WriteLine($"  branch {ITPString}{RainBranchList.IndexOf(branch0)}, {ITPString}{calculatingMu - branch0.CurrentMu}");
                branch0.CurrentMu = calculatingMu;

                // Fight
                if (branch0.TotalMuLength > branch1.TotalMuLength)
                {
                    if (consoleOutput)
                        Console.WriteLine($"  branch {ITPString}{RainBranchList.IndexOf(branch1)}, inactivated");
                    branch1.IsAlive = false;
                    candidateBranches.Remove(branch1);
                }
                else
                {
                    if (consoleOutput)
                        Console.WriteLine($"  branch {ITPString}{RainBranchList.IndexOf(branch0)}, inactivated");
                    branch0.IsAlive = false;
                    candidateBranches.Remove(branch0);
                }
            }
        }
    }

    /// <summary>
    /// Get accumulated mu
    /// </summary>
    public double TotalMu => RainBranchList.Sum(x => x.TotalMuLength);

    /// <summary>
    /// Get accumulated damage
    /// </summary>
    /// <param name="C"></param>
    /// <param name="beta"></param>
    /// <returns></returns>
    public double TotalDamage(double C, double beta) => RainBranchList.Sum(x => x.Damage(C, beta));

}
