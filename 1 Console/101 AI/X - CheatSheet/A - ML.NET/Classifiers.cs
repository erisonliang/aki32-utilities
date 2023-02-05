using Microsoft.ML;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public partial class Classifiers
{

    private static readonly FileInfo dataFile = new FileInfo(@$"../../../../Data/wikiDetoxAnnotated40kRows.tsv");
    private static readonly FileInfo modelFile = new FileInfo(@$"../../../../MLModels/SentimentModel.zip");

    static void Main()
    {

        #region ★★★★★ context

        var context = new MLContext(seed: 1);

        #endregion

        #region ★★★★★★★★★★★★★★★ data

        var data = context.Data.LoadFromTextFile<SentimentIssue>(dataFile.FullName, hasHeader: true, separatorChar: '\t');

        // ★ split
        var split = context.Data.TrainTestSplit(data, testFraction: 0.2);
        var train = split.TrainSet;
        var test = split.TestSet;

        // ★ text → feature
        var pl_DataProcess = context.Transforms.Text.FeaturizeText(outputColumnName: "Features", inputColumnName: nameof(SentimentIssue.Text));

        #endregion

        #region ★★★★★★★★★★★★★★★ algorithm

        // ★★★★★ binary
        var trainer = context.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features");

        // ★★★★★ multi

        // ★ cross validation
        //var trainer =
        //      mlContext.MulticlassClassification.Trainers.OneVersusAll(
        //          mlContext.BinaryClassification.Trainers.AveragedPerceptron(labelColumnName: "Label", numberOfIterations: 10, featureColumnName: "Features"), labelColumnName: "Label")
        //      .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));


        var pl_Trainer = pl_DataProcess.Append(trainer);

        #endregion

        #region ★★★★★★★★★★★★★★★ fit

        Console.WriteLine($"★★★★★★★★★★★★★★★ Fit");
        Console.WriteLine($"fitting...");
        var model = pl_Trainer.Fit(train);
        Console.WriteLine($"fitted");
        Console.WriteLine();

        #endregion

        #region ★★★★★★★★★★★★★★★ use for many

        Console.WriteLine($"★★★★★★★★★★★★★★★ Many Prediction");

        // ★★★★★ pred
        var test_pred = model.Transform(test);

        // ★★★★★ write
        var metrics = context.BinaryClassification.Evaluate(
            data: test_pred,
            labelColumnName: "Label",
            scoreColumnName: "Score",
            probabilityColumnName: "Probability",
            predictedLabelColumnName: "PredictedLabel"
            );
        ConsoleExtension.PrintBinaryClassificationMetrics(trainer.ToString()!, metrics);
        Console.WriteLine();

        #endregion

        #region   ★★★★★★★★★★★★★★★ save

        Console.WriteLine($"★★★★★★★★★★★★★★★ Save");
        context.Model.Save(model, train.Schema, modelFile.FullName);
        Console.WriteLine($"The model is saved to {modelFile.FullName}");
        Console.WriteLine();

        #endregion

        #region ★★★★★★★★★★★★★★★ use for many

        // ★★★★★★★★★★★★★★★ use for 1
        Console.WriteLine($"★★★★★★★★★★★★★★★ Single Prediction");

        // ★★★★★ pred
        var predEngine = context.Model.CreatePredictionEngine<SentimentIssue, SentimentPrediction>(model);

        // ★★★★★ write
        var sample = new SentimentIssue { Text = "I hate this movie! F**k!!!" };
        var sample_pred = predEngine.Predict(sample);
        Console.WriteLine($"Text: {sample.Text} | Prediction: {(Convert.ToBoolean(sample_pred.Prediction) ? "Toxic" : "Non Toxic")} sentiment | Probability of being toxic: {sample_pred.Probability} ");

        #endregion

        #region ★★★★★★★★★★★★★★★ end

        Console.WriteLine($"★★★★★★★★★★★★★★★ End of Process. Hit any key to exit");
        Console.ReadLine();

        #endregion

    }

}
