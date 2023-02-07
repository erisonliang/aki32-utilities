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

    // ★★★★★★★★★★★★★★★ props

    public MLNetExampleScenario Scenario { get; set; }

    public ITransformer Model { get; set; }

    public DirectoryInfo DataDir { get; set; }
    public FileInfo ModelFile { get; set; }

    public IDataView AllData { get; set; }
    public IDataView TestData { get; set; }
    public IDataView TrainData { get; set; }


    // ★★★★★★★★★★★★★★★ init

    public MLNetExampleSummary(MLNetExampleScenario scenario, DirectoryInfo baseDir)
    {
        Scenario = scenario;
        DataDir = baseDir.GetChildDirectoryInfo(scenario.ToString().Shorten(..4));
        DataDir.Create();
    }


    // ★★★★★★★★★★★★★★★ methods

    public void RunPrediction()
    {
        General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ Run Called", ConsoleColor.Yellow);

        // def
        Context = new MLContext(seed: 1);

        // main
        LoadData();
        LoadModel();
        PredictTestData();
        PredictSamples();
    }

    public void RunAll()
    {
        General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ Run Called", ConsoleColor.Yellow);

        // def
        Context = new MLContext(seed: 1);

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


    // ★★★★★★★★★★★★★★★

}
