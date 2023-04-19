using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
public class RainflowCalculator
{

    // ★★★★★★★★★★★★★★★ props

    public TimeHistory InputHistory { get; set; }
    public TimeHistory ResultHistory { get; set; }
    private RainBranchSet[] RainBranchSets { get; set; }
    public List<RainBranch> RainBranches
    {
        get
        {
            var branches = new List<RainBranch>();
            foreach (var RainBranches in RainBranchSets)
                branches.AddRange(RainBranches.RainBranchList);
            return branches;
        }
    }
    private double lastUsedC = 0d;
    private double lastUsedBeta = 0d;


    // ★★★★★★★★★★★★★★★ inits

    /// <summary>
    /// forbidden
    /// </summary>
    private RainflowCalculator()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputTimeHistoryCsv"></param>
    public RainflowCalculator(FileInfo inputTimeHistoryCsv)
    {
        InputHistory = TimeHistory.FromCsv(inputTimeHistoryCsv, new string[] { "t", "mu" });
    }


    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// Run rainflow cycle counting.
    /// </summary>
    /// <param name="C">Damage-related coefficient</param>
    /// <param name="beta">Damage-related coefficeint</param>
    public void CalcRainflow(double C, double beta, bool consoleOutput = false)
    {
        // preprocess
        if (InputHistory is null)
            throw new InvalidOperationException("Must initialize \"InputHistory\" property with TimeHistory with \"t\" and \"mu\" columns first.");

        // init
        ResultHistory = InputHistory.Clone();
        ResultHistory.Name += "_Rainflow";
        lastUsedC = C;
        lastUsedBeta = beta;
        RainBranchSets = new RainBranchSet[]
        {
            new RainBranchSet(true),
            new RainBranchSet(false),
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

            foreach (var RainBranchSet in RainBranchSets)
            {
                RainBranchSet.CalcNext(lastStep.mu, currentStep.mu, consoleOutput);
                currentStep["totalMu"] += RainBranchSet.TotalMu;
                currentStep["totalDamage"] += RainBranchSet.TotalDamage(C, beta);
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

        var branches = RainBranches.OrderBy(x => x.TotalMuLength).ToList();

        foreach (var branch in branches)
        {
            var step = new TimeHistoryStep();
            step["TotalMuLength"] = branch.TotalMuLength;
            step["Damage"] = branch.Damage(lastUsedC, lastUsedBeta);
            result.AppendStep(step);
        }

        return result.SaveToCsv(outputFile);
    }

    // ★★★★★★★★★★★★★★★ samples

    public static RainflowCalculator SampleModel1
    {
        get
        {
            var inputHistory = new TimeHistory();
            inputHistory["t"] = new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            inputHistory["mu"] = new double[] { 0, 2, -1, 3, -3, 1, -2, -1, 0, 2, 3, 4, -4, 1, 0 };
            return new RainflowCalculator { InputHistory = inputHistory };
        }
    }





    // ★★★★★★★★★★★★★★★

}
