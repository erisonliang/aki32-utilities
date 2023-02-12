using Microsoft.ML.Data;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public partial class MLNetExampleSummary : MLNetHandler
{
    private void PredictTestData()
    {
        General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ Test Data Prediction", ConsoleColor.Yellow);

        switch (Scenario)
        {
            // ★★★★★ general

            // for BinaryClassification
            case MLNetExampleScenario.A001_BinaryClassification_SentimentAnalysis:
            case MLNetExampleScenario.A003_BinaryClassification_CreditCardFraudDetection:
            case MLNetExampleScenario.A004_BinaryClassification_HeartDiseasePrediction:
            case MLNetExampleScenario.A777_BinaryClassification_Auto_SentimentAnalysis:
                {
                    var predictedTestData = Model.Transform(TestData);
                    predictedTestData.WriteToConsole();
                    var metrics = Context.BinaryClassification.Evaluate(predictedTestData);
                    ConsoleExtension.PrintMetrics(metrics);

                    break;
                }

            // for MulticlassClassification
            case MLNetExampleScenario.A002_BinaryClassification_SpamDetection:
            case MLNetExampleScenario.B002_MultiClassClassification_IrisFlowersClassification:
            case MLNetExampleScenario.B003_MultiClassClassification_MNIST:
            case MLNetExampleScenario.B777_MultiClassClassification_Auto_MNIST:
                {
                    var predictedTestData = Model.Transform(TestData);
                    predictedTestData.WriteToConsole();
                    var metrics = Context.MulticlassClassification.Evaluate(predictedTestData);
                    ConsoleExtension.PrintMetrics(metrics);

                    break;
                }

            // for Regression
            case MLNetExampleScenario.C002_Recommendation_MovieRecommender_MatrixFactorization:
            case MLNetExampleScenario.D001_Regression_PricePrediction:
            case MLNetExampleScenario.D777_Regression_Auto_TaxiFarePrediction:
                {
                    var predictedTestData = Model.Transform(TestData);
                    predictedTestData.WriteToConsole();
                    var metrics = Context.Regression.Evaluate(predictedTestData);
                    ConsoleExtension.PrintMetrics(metrics);

                    break;
                }

            // for AnomalyDetection (Specific)
            case MLNetExampleScenario.F002_AnomalyDetection_PowerAnomalyDetection:
                {
                    var predictedTestData = Model.Transform(TestData);
                    predictedTestData.WriteToConsole();

                    // Getting the data of the newly created column as an IEnumerable
                    var predictions = Context.Data.CreateEnumerable<F002_PowerMeterOutput>(predictedTestData, false);

                    var colTime = TestData.GetColumn<DateTime>("time").ToArray();
                    var colCDN = TestData.GetColumn<float>("Label").ToArray();

                    // Output the input data and predictions
                    Console.WriteLine("======Displaying anomalies in the Power meter data=========");
                    Console.WriteLine("Date              \tReadingDiff\tAlert\tScore\tP-Value");

                    int i = 0;
                    foreach (var p in predictions)
                    {
                        var message = $"{colTime[i]}\t{colCDN[i]:0.0000}\t{p.Prediction[0]:0.00}\t{p.Prediction[1]:0.00}\t{p.Prediction[2]:0.00}";

                        if (p.Prediction[0] == 1)
                            General.ConsoleExtension.WriteLineWithColor(message, ConsoleColor.Black, ConsoleColor.Yellow);
                        else
                            Console.WriteLine(message, ConsoleColor.Black, ConsoleColor.Yellow);

                        i++;
                    }

                    break;
                }

            // for AnomalyDetection
            case MLNetExampleScenario.F003_AnomalyDetection_CreditCardFraudDetection:
                {
                    var predictedTestData = Model.Transform(TestData);
                    predictedTestData.WriteToConsole();
                    var metrics = Context.AnomalyDetection.Evaluate(predictedTestData);
                    ConsoleExtension.PrintMetrics(metrics);

                    break;
                }

            // for Clustering
            case MLNetExampleScenario.G002_Clustering_IrisFlowerClustering:
                {
                    var predictedTestData = Model.Transform(TestData);
                    predictedTestData.WriteToConsole();
                    var metrics = Context.Clustering.Evaluate(predictedTestData);
                    ConsoleExtension.PrintMetrics(metrics);

                    break;
                }

            // for Ranking
            case MLNetExampleScenario.H001_Ranking_RankSearchEngineResults:
                {
                    var predictedTestData = Model.Transform(TestData);
                    predictedTestData.WriteToConsole();
                    var metrics = Context.Ranking.Evaluate(predictedTestData);
                    ConsoleExtension.PrintMetrics(metrics);

                    break;
                }

            // ignore
            case MLNetExampleScenario.C001_Recommendation_ProductRecommender:
            case MLNetExampleScenario.I004_ComputerVision_ObjectDetection_ImportONNXModel_TinyYoloV2_08:
            case MLNetExampleScenario.I004_ComputerVision_ObjectDetection_ImportONNXModel_YoloV2_09:
            case MLNetExampleScenario.I004_ComputerVision_ObjectDetection_ImportONNXModel_YoloV3_10:
            case MLNetExampleScenario.Z999_Ignore:
                {
                    Console.WriteLine("ignore");

                    break;
                }

            // not implemented
            case MLNetExampleScenario.B001_MultiClassClassification_IssuesClassification:
            case MLNetExampleScenario.C003_Recommendation_MovieRecommender_FieldAwareFactorizationMachines:
            case MLNetExampleScenario.C777_Auto_Recommendation:
            case MLNetExampleScenario.D002_Regression_SalesForecasting:
            case MLNetExampleScenario.D003_Regression_DemandPrediction:
            case MLNetExampleScenario.E001_TimeSeriesForecasting_SalesForecasting:
            case MLNetExampleScenario.F001_AnomalyDetection_SalesSpikeDetection:
            case MLNetExampleScenario.G001_Clustering_CustomerSegmentation:
            case MLNetExampleScenario.I001_ComputerVision_ImageClassificationTraining_HighLevelAPI:
            case MLNetExampleScenario.I002_ComputerVision_ImageClassificationPredictions_PretrainedTensorFlowModelScoring:
            case MLNetExampleScenario.I003_ComputerVision_ImageClassificationTraining_TensorFlowFeaturizerEstimator:
            case MLNetExampleScenario.J001_CrossCuttingScenarios_ScalableModelOnWebAPI:
            case MLNetExampleScenario.J002_CrossCuttingScenarios_ScalableModelOnRazorWebApp:
            case MLNetExampleScenario.J003_CrossCuttingScenarios_ScalableModelOnAzureFunctions:
            case MLNetExampleScenario.J004_CrossCuttingScenarios_ScalableModelOnBlazorWebApp:
            case MLNetExampleScenario.J005_CrossCuttingScenarios_LargeDatasets:
            case MLNetExampleScenario.J006_CrossCuttingScenarios_LoadingDataWithDatabaseLoader:
            case MLNetExampleScenario.J007_CrossCuttingScenarios_LoadingDataWithLoadFromEnumerable:
            case MLNetExampleScenario.J008_CrossCuttingScenarios_ModelExplainability:
            case MLNetExampleScenario.J009_CrossCuttingScenarios_ExportToONNX:
            case MLNetExampleScenario.K777_Auto:
            default:
                {
                    throw new NotImplementedException();
                }
        }


    }
}