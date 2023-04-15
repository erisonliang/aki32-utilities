using Aki32Utilities.ConsoleAppUtilities.General;

using Microsoft.ML;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
/// <summary>
/// 
/// Big summary of https://github.com/dotnet/machinelearning-samples
/// Almost all the examples and data is form above.
/// Examples are tagged by me as found in enum MLNetExampleScenario 
/// 
/// </summary>
public partial class MLNetExampleSummary : MLNetHandler
{
    // ★★★★★★★★★★★★★★★ init

    public MLNetExampleSummary(MLNetExampleScenario scenario, DirectoryInfo baseDir)
    {
        Scenario = scenario;
        DataDir = baseDir.GetChildDirectoryInfo(scenario.ToString().Shorten(..4));
        DataDir.Create();
    }


    // ★★★★★★★★★★★★★★★ methods

    public void RunAll()
    {
        General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ Run Called", ConsoleColor.Yellow);

        // def
        Context = new MLContext(seed: 1);
        Context.Log += FilterMLContextLog;

        // main
        LoadData();

        BuildPipeline();
        CrossValidate();
        FitModel();
        SaveModel();

        LoadModel();
        PredictTestData();
        PredictSamples();

    }

    public void RunPrediction()
    {
        General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ Run Called", ConsoleColor.Yellow);

        // def
        Context = new MLContext(seed: 1);
        Context.Log += FilterMLContextLog;

        // main
        LoadData();
        LoadModel();
        PredictTestData();
        PredictSamples();

    }

    // ★★★★★★★★★★★★★★★

}
