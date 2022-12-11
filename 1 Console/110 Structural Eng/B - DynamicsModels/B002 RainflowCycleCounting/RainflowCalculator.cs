using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
public class RainflowCalculator
{

    // ★★★★★★★★★★★★★★★ props

    public TimeHistory inputHistory { get; set; }
    public TimeHistory resultHistory { get; set; }
    private RainBranches[] AllRainBranches { get; set; }
    private double lastUsedC = 0d;
    private double lastUsedBeta = 0d;

    // ★★★★★★★★★★★★★★★ inits

    /// <summary>
    /// Forbid public  instantiate
    /// </summary>
    private RainflowCalculator() { }

    /// <summary>
    /// Construct from csv
    /// </summary>
    /// <param name="inputCsvPath"></param>
    /// <exception cref="Exception"></exception>
    public static RainflowCalculator FromCsv(FileInfo inputCsv)
    {
        var rainflow = new RainflowCalculator();
        rainflow.inputHistory = TimeHistory.FromCsv(inputCsv, new string[] { "t", "mu" });
        return rainflow;
    }

    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// Run rainflow cycle counting.
    /// </summary>
    /// <param name="C">Damage-related coefficient</param>
    /// <param name="beta">Damage-related coefficeint</param>
    public void CalcRainflow(double C, double beta, bool consoleOutput = false)
    {
        // init
        resultHistory = inputHistory.Clone();
        resultHistory.Name += "_Rainflow";
        lastUsedC = C;
        lastUsedBeta = beta;
        AllRainBranches = new RainBranches[]
        {
            new RainBranches(true),
            new RainBranches(false),
        };


        // main
        for (int i = 0; i < resultHistory.DataRowCount; i++)
        {
            if (consoleOutput)
                Console.WriteLine($"step {i}");

            var currentStep = resultHistory.GetStep(i);
            var lastStep = i > 0 ? resultHistory.GetStep(i - 1) : new TimeHistoryStep() { mu = 0 };

            currentStep["totalMu"] = 0;
            currentStep["totalDamage"] = 0;

            foreach (var RainBranches in AllRainBranches)
            {
                RainBranches.CalcNext(lastStep.mu, currentStep.mu, consoleOutput);
                currentStep["totalMu"] += RainBranches.TotalMu;
                currentStep["totalDamage"] += RainBranches.TotalDamage(C, beta);
            }

            resultHistory.SetStep(i, currentStep);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public FileInfo SaveResultHistoryToCsv(FileInfo? outputFile = null)
    {
        return resultHistory.SaveToCsv(outputFile);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public FileInfo SaveRainBranchesToCsv(FileInfo? outputFile = null)
    {
        var result = inputHistory.Clone();
        result.DropAllColumns();
        result.Name += "_RainflowBranches";

        var branches = new List<RainBranch>();
        foreach (var RainBranches in AllRainBranches)
            branches.AddRange(RainBranches.RainBranchList);

        branches = branches.OrderBy(x => x.TotalMuLength).ToList();

        foreach (var branch in branches)
        {
            var step = new TimeHistoryStep();
            step["TotalMuLength"] = branch.TotalMuLength;
            step["Damage"] = branch.Damage(lastUsedC, lastUsedBeta);
            result.AppendStep(step);
        }

        return result.SaveToCsv(outputFile);
    }


    // ★★★★★★★★★★★★★★★

}
