using Microsoft.ML;
using Microsoft.ML.Transforms;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public partial class MLNetExampleSummary : MLNetHandler
{
    private void BuildPipeline(MLContext context)
    {
        General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ Cross Validation", ConsoleColor.Yellow);

        switch (Scenario) // data process → algorithm → data process
        {
            // ★ for binary label, 1 feature
            case MLNetExampleScenario.A001_Sentiment_Analysis:
                {
                    ConnectNode(context.Transforms.Text.FeaturizeText("Features", nameof(SentimentInput.Text))); // string → word vector
                    ConnectNode(context.BinaryClassification.Trainers.SdcaLogisticRegression());
                    break;
                }

            // ★ for multiple label, 1 feature, using 1 vs All
            case MLNetExampleScenario.A002_Spam_Detection:
                {
                    ConnectNode(context.Transforms.Conversion.MapValueToKey("Label", "Number", keyOrdinality: ValueToKeyMappingEstimator.KeyOrdinality.ByValue));  // string key → int value
                    ConnectNode(context.Transforms.Concatenate("Features", "FeatureList"));
                    ConnectCheckPoint(context);
                    ConnectNode(context.MulticlassClassification.Trainers.OneVersusAll(
                           context.BinaryClassification.Trainers.AveragedPerceptron(numberOfIterations: 10)));
                    ConnectNode(context.Transforms.Conversion.MapKeyToValue("PredictedLabel")); // int value → string key

                    break;
                }

            // ★ for multiple label, many features (#B002 Iris Flowers Classification)
            case MLNetExampleScenario.B002_IrisFlowersClassification:
                {
                    ConnectNode(context.Transforms.Conversion.MapValueToKey("Label"));
                    ConnectNode(context.Transforms.Concatenate("Features",
                        nameof(IrisInput.SepalLength), nameof(IrisInput.SepalWidth), nameof(IrisInput.PetalLength), nameof(IrisInput.PetalWidth)));
                    ConnectNode(context.MulticlassClassification.Trainers.SdcaMaximumEntropy());
                    ConnectNode(context.Transforms.Conversion.MapKeyToValue("Label"));
                    ConnectNode(context.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

                    break;
                }

            // not implemented
            case MLNetExampleScenario.A003_CreditCardFraudDetection:
            case MLNetExampleScenario.A004_HeartDiseasePrediction:
            case MLNetExampleScenario.A777_Auto:
            case MLNetExampleScenario.B001_IssuesClassification:
            case MLNetExampleScenario.B003_MNIST:
            case MLNetExampleScenario.B777_Auto:
            case MLNetExampleScenario.C001_ProductRecommendation:
            case MLNetExampleScenario.C002_MovieRecommender_MatrixFactorization:
            case MLNetExampleScenario.C003_MovieRecommender_FieldAwareFactorizationMachines:
            case MLNetExampleScenario.C777_Auto:
            case MLNetExampleScenario.D001_PricePrediction:
            case MLNetExampleScenario.D002_SalesForecasting_Regression:
            case MLNetExampleScenario.D003_DemandPrediction:
            case MLNetExampleScenario.D777_Auto:
            case MLNetExampleScenario.E001_SalesForecasting_TimeSeries:
            case MLNetExampleScenario.F001_SalesSpikeDetection:
            case MLNetExampleScenario.F002_PowerAnomalyDetection:
            case MLNetExampleScenario.F003_CreditCardFraudDetection:
            case MLNetExampleScenario.G001_CustomerSegmentation:
            case MLNetExampleScenario.G002_IrisFlowersClustering:
            case MLNetExampleScenario.H001_RankSearchEngineResults:
            case MLNetExampleScenario.I001_ImageClassificationTraining_HighLevelAPI:
            case MLNetExampleScenario.I002_ImageClassificationPredictions_PretrainedTensorFlowModelScoring:
            case MLNetExampleScenario.I003_ImageClassificationTraining_TensorFlowFeaturizerEstimator:
            case MLNetExampleScenario.I004_ObjectDetection_ONNXModelScoring:
            case MLNetExampleScenario.J001_ScalableModelOnWebAPI:
            case MLNetExampleScenario.J002_ScalableModelOnRazorWebApp:
            case MLNetExampleScenario.J003_ScalableModelOnAzureFunctions:
            case MLNetExampleScenario.J004_ScalableModelOnBlazorWebApp:
            case MLNetExampleScenario.J005_LargeDatasets:
            case MLNetExampleScenario.J006_LoadingDataWithDatabaseLoader:
            case MLNetExampleScenario.J007_LoadingDataWithLoadFromEnumerable:
            case MLNetExampleScenario.J008_ModelExplainability:
            case MLNetExampleScenario.J009_ExportToONNX:
            case MLNetExampleScenario.K777_Auto:
            default:
                Console.WriteLine("ignore");
                throw new NotImplementedException();
        }


        ////// ★ for binary label, 1 feature
        //ConnectNode(context.Transforms.Text.FeaturizeText("Features", nameof(SentimentInput.Text))); // string → word vector
        //ConnectNode(context.Transforms.Conversion.MapValueToKey("Label")); // string key → int value
        //ConnectCheckPoint(context);



        //// ★ for multiple label, many features 
        //var featureColumnNames = AllData.Schema.AsQueryable()
        //    .Select(column => column.Name)
        //    .Where(name => name != "ExcludingColumn")
        //    .ToArray();
        //ConnectNode(context.Transforms.Concatenate("Features", featureColumnNames));
        //ConnectNode(context.Transforms.DropColumns(new string[] { "ExcludingColumn" }));
        //ConnectNode(context.Transforms.NormalizeLpNorm("NormalizedFeatures", "Features"));



        TrainData.WriteToConsole();





    }
}