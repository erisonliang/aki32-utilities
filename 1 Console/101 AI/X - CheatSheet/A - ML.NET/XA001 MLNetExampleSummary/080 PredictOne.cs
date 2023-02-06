using DocumentFormat.OpenXml.EMMA;

using Microsoft.ML;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public partial class MLNetExampleSummary : MLNetHandler
{
    private void PredictSamples()
    {
        General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ Single Prediction", ConsoleColor.Yellow);

        switch (Scenario)
        {
            case MLNetExampleScenario.A001_Sentiment_Analysis:
                {
                    var predictor = Context.Model.CreatePredictionEngine<A001_SentimentInput, A001_SentimentOutput>(Model);

                    var samples = new A001_SentimentInput[]
                    {
                        new A001_SentimentInput { Text = "I love this movie!" },
                        new A001_SentimentInput { Text = "I hate this movie! yuck!!" },
                    };

                    foreach (var sample in samples)
                    {
                        var result = predictor.Predict(sample);

                        Console.WriteLine(@$"=======================================================");
                        Console.WriteLine(@$"Text: {sample.Text}");
                        Console.WriteLine(@$"Prediction: {(Convert.ToBoolean(result.Prediction) ? "Toxic" : "Non Toxic")}");
                        Console.WriteLine(@$"Probability of being toxic: {result.Probability}");
                        Console.WriteLine();
                    }
                    Console.WriteLine(@$"=======================================================");

                    break;
                }

            case MLNetExampleScenario.A002_Spam_Detection:
                {
                    var predictor = Context.Model.CreatePredictionEngine<A002_SpamInput, A002_SpamOutput>(Model);

                    var samples = new A002_SpamInput[]
                    {
                        new A002_SpamInput { Message = "That's a great idea. It should work."},
                        new A002_SpamInput { Message =  "free medicine winner! congratulations"               },
                        new A002_SpamInput { Message =  "Yes we should meet over the weekend!"},
                        new A002_SpamInput { Message = "you win pills and free entry vouchers" },
                    };

                    foreach (var sample in samples)
                    {
                        var result = predictor.Predict(sample);

                        Console.WriteLine(@$"=======================================================");
                        Console.WriteLine(@$"Message: {sample.Message}");
                        Console.WriteLine(@$"Result: {result.PredictedLabel}");
                        Console.WriteLine();
                    }
                    Console.WriteLine(@$"=======================================================");

                    break;
                }

            case MLNetExampleScenario.B002_IrisFlowersClassification:
                {
                    var predictor = Context.Model.CreatePredictionEngine<B002_IrisInput, B002_IrisOutput>(Model);

                    var samples = new B002_IrisInput[]
                    {
                       B002_IrisSampe.Iris1,
                       B002_IrisSampe.Iris2,
                       B002_IrisSampe.Iris3,
                    };

                    Dictionary<float, string> IrisFlowers = new()
                    {
                        { 0, "setosa" },
                        { 1, "versicolor" },
                        { 2, "virginica" }
                    };

                    foreach (var sample in samples)
                    {
                        var result = predictor.Predict(sample);

                        Console.WriteLine(@$"=======================================================");
                        Console.WriteLine($"Answer: {IrisFlowers[(int)sample.Label]}");
                        Console.WriteLine($"Prediction: {IrisFlowers[(int)result.PredictedLabel]}");
                        Console.WriteLine($"Probabilities:");
                        for (int i = 0; i < result.Score.Length; i++)
                            Console.WriteLine($"  {IrisFlowers[i],10}: {result.Score[i],-10:F4}");

                        Console.WriteLine();
                    }
                    Console.WriteLine(@$"=======================================================");

                    break;
                }

            case MLNetExampleScenario.B003_MNIST:
                {
                    var predictor = Context.Model.CreatePredictionEngine<MnistInput, MnistOutput>(Model);

                    break;
                }

            // ignore
            case MLNetExampleScenario.A003_CreditCardFraudDetection:
            case MLNetExampleScenario.A004_HeartDiseasePrediction:
            case MLNetExampleScenario.A777_Auto:
            case MLNetExampleScenario.B001_IssuesClassification:
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
                {
                    Console.WriteLine("ignore");

                    break;
                }

            // not implemented
            default:
                {
                    throw new NotImplementedException();
                }
        }
    }
}