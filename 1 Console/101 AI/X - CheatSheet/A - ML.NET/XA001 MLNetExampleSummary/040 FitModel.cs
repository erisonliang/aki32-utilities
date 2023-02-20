using System.Diagnostics;

using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Data;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public partial class MLNetExampleSummary : MLNetHandler
{
    private void FitModel(IDataView? targetData = null)
    {
        General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ Fit", ConsoleColor.Yellow);

        switch (Scenario)
        {
            // simple fit
            case MLNetExampleScenario.A001_BinaryClassification_SentimentAnalysis:
            case MLNetExampleScenario.A002_BinaryClassification_SpamDetection:
            case MLNetExampleScenario.A003_BinaryClassification_CreditCardFraudDetection:
            case MLNetExampleScenario.A004_BinaryClassification_HeartDiseasePrediction:
            case MLNetExampleScenario.B002_MultiClassClassification_IrisFlowersClassification:
            case MLNetExampleScenario.B003_MultiClassClassification_MNIST:
            case MLNetExampleScenario.C001_Recommendation_ProductRecommender:
            case MLNetExampleScenario.C002_Recommendation_MovieRecommender_MatrixFactorization:
            case MLNetExampleScenario.D001_Regression_TaxiFarePrediction:
            case MLNetExampleScenario.F001_AnomalyDetection_SalesSpikeDetection_DetectIidSpike:
            case MLNetExampleScenario.F002_AnomalyDetection_PowerAnomalyDetection:
            case MLNetExampleScenario.F003_AnomalyDetection_CreditCardFraudDetection:
            case MLNetExampleScenario.G002_Clustering_IrisFlowerClustering:
            case MLNetExampleScenario.H001_Ranking_RankSearchEngineResults:
            case MLNetExampleScenario.I001_ComputerVision_ImageClassificationTraining_HighLevelAPI:
                {
                    Console.WriteLine($"fitting...");
                    var watch = Stopwatch.StartNew();
                    Model = PipeLineHead.Fit(targetData ?? TrainData);
                    watch.Stop();
                    Console.WriteLine($"fitted (took {watch.ElapsedMilliseconds / 1000} seconds)");
                    break;
                }

            // ★ Auto BinaryClassification
            case MLNetExampleScenario.A777_BinaryClassification_Auto_SentimentAnalysis:
                {
                    ExperimentTime_InSeconds = 60;

                    Console.WriteLine(@$"=======================================================");
                    Console.WriteLine(@$"Running AutoML binary classification experiment for {ExperimentTime_InSeconds} seconds...");

                    var handler = new ExperimentHandler();
                    var result = Context.Auto().CreateBinaryClassificationExperiment(ExperimentTime_InSeconds).Execute(TrainData, progressHandler: handler);

                    Console.WriteLine();
                    handler.PrintTopModels(result);

                    Model = result.BestRun.Model;

                    break;
                }

            // ★ Auto MultiClassClassification
            case MLNetExampleScenario.B777_MultiClassClassification_Auto_MNIST:
                {
                    ExperimentTime_InSeconds = 60;

                    Console.WriteLine(@$"=======================================================");
                    Console.WriteLine($"Running AutoML multiclass classification experiment for {ExperimentTime_InSeconds} seconds...");

                    var handler = new ExperimentHandler();
                    var result = Context.Auto().CreateMulticlassClassificationExperiment(ExperimentTime_InSeconds).Execute(TrainData, progressHandler: handler);

                    Console.WriteLine();
                    handler.PrintTopModels(result);

                    Model = result.BestRun.Model;

                    break;
                }

            // ★ Auto Regression
            case MLNetExampleScenario.D777_Regression_Auto_TaxiFarePrediction:
                {
                    ExperimentTime_InSeconds = 60;

                    Console.WriteLine(@$"=======================================================");
                    Console.WriteLine($"Running AutoML regression experiment for {ExperimentTime_InSeconds} seconds...");

                    var handler = new ExperimentHandler();
                    var result = Context.Auto().CreateRegressionExperiment(ExperimentTime_InSeconds).Execute(TrainData, progressHandler: handler);

                    Console.WriteLine();
                    handler.PrintTopModels(result);

                    Model = result.BestRun.Model;

                    break;
                }

            // ★ Auto Ranking
            case MLNetExampleScenario.H777_Ranking_Auto_RankSearchEngineResults:
                {
                    ExperimentTime_InSeconds = 600;
                    Ranking_OptimizationMetricTruncationLevel = 10;

                    Console.WriteLine(@$"=======================================================");
                    Console.WriteLine($"Running AutoML ranking experiment for {ExperimentTime_InSeconds} seconds...");

                    var handler = new ExperimentHandler();
                    var experimentSettings = new RankingExperimentSettings
                    {
                        MaxExperimentTimeInSeconds = ExperimentTime_InSeconds,
                        OptimizingMetric = RankingMetric.Ndcg,
                        OptimizationMetricTruncationLevel = Ranking_OptimizationMetricTruncationLevel,
                    };

                    var result = Context.Auto().CreateRankingExperiment(experimentSettings!).Execute(TrainData, progressHandler: handler);

                    Console.WriteLine();
                    handler.PrintTopModels(result, (int)Ranking_OptimizationMetricTruncationLevel);

                    Model = result.BestRun.Model;

                    break;
                }

            // empty
            case MLNetExampleScenario.F001_AnomalyDetection_SalesSpikeDetection_DetectIidChangePoint:
                {
                    Console.WriteLine($"fitting...");
                    var emptyData = Context.Data.LoadFromEnumerable(new List<F001_ProductSalesInput>());
                    Model = PipeLineHead.Fit(emptyData);
                    Console.WriteLine($"fitted");

                    break;
                }

            // from ONNX, no need for further fitting
            case MLNetExampleScenario.I004_ComputerVision_ObjectDetection_ImportONNXModel_TinyYoloV2_08:
            case MLNetExampleScenario.I004_ComputerVision_ObjectDetection_ImportONNXModel_YoloV2_09:
            case MLNetExampleScenario.I004_ComputerVision_ObjectDetection_ImportONNXModel_YoloV3_10:
                {
                    Console.WriteLine($"fitting...");
                    var emptyData = Context.Data.LoadFromEnumerable(new List<I004_YoloInput>());
                    Model = PipeLineHead.Fit(emptyData);
                    Console.WriteLine($"fitted");

                    break;
                }

            // ignore
            case MLNetExampleScenario.Z999_Ignore:
                {
                    Console.WriteLine("ignore");

                    break;
                }

            //var result = Context.Auto().CreateRecommendationExperiment.Execute(TrainData, progressHandler: handler);
            //var result = Context.Auto().CreateRankingExperiment.Execute(TrainData, progressHandler: handler);


            // not implemented
            case MLNetExampleScenario.B001_MultiClassClassification_IssuesClassification:
            case MLNetExampleScenario.C003_Recommendation_MovieRecommender_FieldAwareFactorizationMachines:
            case MLNetExampleScenario.C777_Auto_Recommendation:
            case MLNetExampleScenario.D002_Regression_SalesForecasting:
            case MLNetExampleScenario.D003_Regression_DemandPrediction:
            case MLNetExampleScenario.E001_TimeSeriesForecasting_SalesForecasting:
            case MLNetExampleScenario.G001_Clustering_CustomerSegmentation:
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