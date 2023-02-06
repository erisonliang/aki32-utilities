using Aki32Utilities.ConsoleAppUtilities.General;

using DocumentFormat.OpenXml.Drawing.Charts;

using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

using OpenCvSharp;

using static Aki32Utilities.ConsoleAppUtilities.AI.MLNETExtension;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public static class Classifiers
{

    public static FileInfo DataFile { get; set; }
    public static FileInfo ModelFile { get; set; }

    public static IDataView AllData { get; set; }
    public static IDataView TestData { get; set; }
    public static IDataView TrainData { get; set; }

    public static void Run_ManuallyDefinedShema(FileInfo dataFile, FileInfo modelFile)
    {

        // ★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★ main

        General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ Called", ConsoleColor.Yellow);
        DataFile = dataFile;
        ModelFile = modelFile;


        var context = new MLContext(seed: 1);

        LoadData(context);
        BuildAndSaveModel(context);
        TestSomePredictions(context);


        // ★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★

    }

    private static void LoadData(MLContext context)
    {

        // ★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★ load data

        General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ Load Data", ConsoleColor.Yellow);


        // ★★★★★ download zip
        //if (!DataFile.Exists)
        //{
        //    new Uri("https://archive.ics.uci.edu/ml/machine-learning-databases/00228/smsspamcollection.zip")
        //       .DownloadFile(new FileInfo(DataFile.Directory!.FullName + ".zip"))
        //       .Decompress_Zip(DataFile.Directory);
        //}


        // ★★★★★ load data

        // ★ from class array
        //AllData = context.Data.LoadFromTextFile<SentimentInput>(DataFile.FullName, hasHeader: true, separatorChar: '\t');
        AllData = context.Data.LoadFromTextFile<IrisInput>(DataFile.FullName, hasHeader: true, separatorChar: '\t');

        // ★ many features, the same type
        //var data = context.Data.CreateTextLoader(
        //           columns: new[]
        //           {
        //                    new TextLoader.Column("FeatureList", DataKind.Single, 0, 63), // 64 single values
        //                    new TextLoader.Column("Number", DataKind.Single, 64) // 1 single value
        //           },
        //           hasHeader: false,
        //           separatorChar: ','
        //           ).Load(DataFile.FullName);


        // ★★★★★ split
        var split = context.Data.TrainTestSplit(AllData, testFraction: 0.2);
        TrainData = split.TrainSet;
        TestData = split.TestSet;


        // ★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★ 


    }

    private static void BuildAndSaveModel(MLContext context)
    {

        // ★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★ init

        IEstimator<ITransformer> pipeline = new EstimatorChain<ITransformer>();


        // ★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★ create data pipeline

        //// ★ for binary label, 1 feature
        //pipeline = pipeline
        //    .Append(context.Transforms.Text.FeaturizeText("Features", nameof(SentimentInput.Text))) // string → word vector
        //    ;


        //// ★ for binary label, 1 feature
        //pipeline = pipeline
        //    .Append(context.Transforms.Text.FeaturizeText("Features", nameof(SentimentIssue.Text))) // string → word vector
        //    .Append(context.Transforms.Conversion.MapValueToKey("Label")) // string key → int value
        //    .AppendCacheCheckpoint(context);


        //// ★ for multiple label, 1 feature (using 1 vs All)
        //pipeline = pipeline
        //    .Append(context.Transforms.Conversion.MapValueToKey("Label", "Number", keyOrdinality: ValueToKeyMappingEstimator.KeyOrdinality.ByValue))  // string key → int value
        //    .Append(context.Transforms.Concatenate("Features", "FeatureList"))
        //    .AppendCacheCheckpoint(context);


        // ★ for multiple label, many features 
        pipeline = pipeline
            .Append(context.Transforms.Conversion.MapValueToKey("Label"))
            .Append(context.Transforms.Concatenate("Features",
                nameof(IrisInput.SepalLength),
                nameof(IrisInput.SepalWidth),
                nameof(IrisInput.PetalLength),
                nameof(IrisInput.PetalWidth))
            );


        //// ★ for multiple label, many features 
        //var featureColumnNames = AllData.Schema.AsQueryable()
        //    .Select(column => column.Name)
        //    .Where(name => name != "ExcludingColumn")
        //    .ToArray();
        //pipeline = pipeline
        //    .Append(context.Transforms.Concatenate("Features", featureColumnNames))
        //    .Append(context.Transforms.DropColumns(new string[] { "ExcludingColumn" }))
        //    .Append(context.Transforms.NormalizeLpNorm("NormalizedFeatures", "Features"));


        TrainData.WriteToConsole();


        // ★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★ add algorithm

        // ★★★★★ build trainer

        //// ★ for binary label
        //pipeline = pipeline
        //    .Append(context.BinaryClassification.Trainers.SdcaLogisticRegression());


        //// ★ for multiple label (using 1 vs All)
        //pipeline = pipeline
        //   .Append(context.MulticlassClassification.Trainers.OneVersusAll(
        //       context.BinaryClassification.Trainers.AveragedPerceptron(numberOfIterations: 10))
        //   .Append(context.Transforms.Conversion.MapKeyToValue("PredictedLabel"))) // int value → string key
        //   ;


        //// ★ for multiple label
        //pipeline = pipeline
        //    .Append(context.MulticlassClassification.Trainers.SdcaMaximumEntropy()
        //    .Append(context.Transforms.Conversion.MapKeyToValue("Label"))
        //    .Append(context.Transforms.Conversion.MapKeyToValue("PredictedLabel")))
        //    ;


        // ★ for multiple label
        pipeline = pipeline
            .Append(context.MulticlassClassification.Trainers.SdcaMaximumEntropy())
            .Append(context.Transforms.Conversion.MapKeyToValue("Label"))
            .Append(context.Transforms.Conversion.MapKeyToValue("PredictedLabel"))
            ;



        // ★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★ cross validation

        //General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ Cross Validation", ConsoleColor.Yellow);
        //var crossValMetrics = context.MulticlassClassification.CrossValidate(data: data, estimator: pipeline, numberOfFolds: 5);
        //ConsoleExtension.PrintMetrics(crossValMetrics);


        // ★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★ fit

        General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ Fit", ConsoleColor.Yellow);
        Console.WriteLine($"fitting...");

        var model = pipeline.Fit(TrainData);

        Console.WriteLine($"fitted");



        // ★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★ save model

        General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ Save Model", ConsoleColor.Yellow);
        context.Model.Save(model, TrainData.Schema, ModelFile.FullName);


        // ★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★ 

    }

    private static void TestSomePredictions(MLContext context)
    {
        // ★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★ load model

        var model = context.Model.Load(ModelFile.FullName, out var _);


        // ★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★ predict many

        General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ Test Data Prediction", ConsoleColor.Yellow);

        // ★★★★★ pred
        var predictedTestData = model.Transform(TestData);


        // ★★★★★ write
        predictedTestData.WriteToConsole();
        var metrics = context.MulticlassClassification.Evaluate(predictedTestData);
        //var metrics = context.BinaryClassification.Evaluate(predictedTestData);
        ConsoleExtension.PrintMetrics(metrics);




        // ★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★ predict 1

        General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ Single Prediction", ConsoleColor.Yellow);

        // ★★★★★ pred
        var predictor = context.Model.CreatePredictionEngine<IrisInput, IrisOutput>(model);
        //var predictor = context.Model.CreatePredictionEngine<MNISTInputData, MNISTOutPutData>(model);


        // ★★★★★ write
        // ★
        {



        }

        // ★ Iris example
        {
            var sample = IrisSampe.Iris1;
            var sample_pred = predictor.Predict(sample);
            Dictionary<float, string> IrisFlowers = new()
            {
                { 0, "setosa" },
                { 1, "versicolor" },
                { 2, "virginica" }
            };
            Console.WriteLine($"Answer: {IrisFlowers[(int)sample.Label]}");
            Console.WriteLine($"Prediction: {IrisFlowers[(int)sample_pred.PredictedLabel]}");
            Console.WriteLine($"Probabilities:");
            for (int i = 0; i < sample_pred.Score.Length; i++)
                Console.WriteLine($"  {IrisFlowers[i],10}: {sample_pred.Score[i],-10:F4}");

        }





        // ★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★ 

    }


}
