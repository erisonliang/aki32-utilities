using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
public class RainflowCalculator
{

    // ★★★★★★★★★★★★★★★ props

    public TimeHistory InputHistory { get; set; }
    public TimeHistory ResultHistory { get; set; }
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
        return new RainflowCalculator { InputHistory = TimeHistory.FromCsv(inputCsv, new string[] { "t", "mu" }) };
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
        ResultHistory = InputHistory.Clone();
        ResultHistory.Name += "_Rainflow";
        lastUsedC = C;
        lastUsedBeta = beta;
        AllRainBranches = new RainBranches[]
        {
            new RainBranches(true),
            new RainBranches(false),
        };


        // main
        for (int i = 0; i < ResultHistory.DataRowCount; i++)
        {
            if (consoleOutput)
                Console.WriteLine($"step {i}");

            var currentStep = ResultHistory.GetStep(i);
            var lastStep = i > 0 ? ResultHistory.GetStep(i - 1) : new TimeHistoryStep() { mu = 0 };

            currentStep["totalMu"] = 0;
            currentStep["totalDamage"] = 0;

            foreach (var RainBranches in AllRainBranches)
            {
                RainBranches.CalcNext(lastStep.mu, currentStep.mu, consoleOutput);
                currentStep["totalMu"] += RainBranches.TotalMu;
                currentStep["totalDamage"] += RainBranches.TotalDamage(C, beta);
            }

            ResultHistory.SetStep(i, currentStep);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public FileInfo SaveResultHistoryToCsv(FileInfo? outputFile = null)
    {
        return ResultHistory.SaveToCsv(outputFile);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public FileInfo SaveRainBranchesToCsv(FileInfo? outputFile = null)
    {
        var result = InputHistory.Clone();
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
