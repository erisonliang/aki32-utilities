using System.Diagnostics.CodeAnalysis;

using Aki32Utilities.ConsoleAppUtilities.General;

using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;

using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
/// <summary>
/// 
/// Big summary of https://github.com/dotnet/machinelearning-samples
/// 
/// </summary>
/// <remarks>
/// 
/// Almost all the examples and data is form here https://github.com/dotnet/machinelearning-samples
/// Examples are tagged by me as found in enum MLNetExampleScenario 
/// 
/// </remarks>
public partial class MLNetExampleSummary : MLNetHandler
{

    // ★★★★★★★★★★★★★★★ props

    public MLNetExampleScenario Scenario { get; set; }

    public ITransformer Model { get; set; }

    public DirectoryInfo DataDir { get; set; }
    public FileInfo DataFile { get; set; }
    public FileInfo ModelFile { get; set; }

    public IDataView AllData { get; set; }
    public IDataView TestData { get; set; }
    public IDataView TrainData { get; set; }


    // ★★★★★★★★★★★★★★★ init

    public MLNetExampleSummary(MLNetExampleScenario scenario, DirectoryInfo baseDir)
    {
        Scenario = scenario;
        DataDir = new DirectoryInfo(Path.Combine(baseDir.FullName, scenario.ToString().Shorten(..4)));
        DataDir.Create();
    }


    // ★★★★★★★★★★★★★★★ methods

    public void Run()
    {
        General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ Run Called", ConsoleColor.Yellow);

        // def
        var context = new MLContext(seed: 1);

        // main
        LoadData(context);

        BuildPipeline(context);
        CrossValidate(context);
        FitModel();
        SaveModel(context);

        LoadModel(context);
        PredictTestData(context);

    }


    // ★★★★★★★★★★★★★★★

}
