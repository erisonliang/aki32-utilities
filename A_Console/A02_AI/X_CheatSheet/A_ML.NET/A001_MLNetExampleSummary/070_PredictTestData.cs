﻿using System.Diagnostics;

using Microsoft.ML.AutoML;
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
            case MLNetExampleScenario.C777_Recommendation_Auto_MovieRecommender:
            case MLNetExampleScenario.D001_Regression_TaxiFarePrediction:
            case MLNetExampleScenario.D777_Regression_Auto_TaxiFarePrediction:
                {
                    var predictedTestData = Model.Transform(TestData);
                    predictedTestData.WriteToConsole();
                    var metrics = Context.Regression.Evaluate(predictedTestData);
                    ConsoleExtension.PrintMetrics(metrics);

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
            case MLNetExampleScenario.H777_Ranking_Auto_RankSearchEngineResults:
                {
                    var predictedTestData = Model.Transform(TestData);
                    predictedTestData.WriteToConsole();

                    var rankingEvaluatorOptions = new RankingEvaluatorOptions
                    {
                        DcgTruncationLevel = Math.Min(10, (int)Ranking_OptimizationMetricTruncationLevel * 2)
                    };

                    var metrics = Context.Ranking.Evaluate(predictedTestData, rankingEvaluatorOptions);
                    ConsoleExtension.PrintMetrics(metrics);

                    break;
                }

            case MLNetExampleScenario.I001_ComputerVision_ImageClassificationTraining_HighLevelAPI:
                {
                    // Measuring time
                    var watch = Stopwatch.StartNew();

                    var predictedTestData = Model.Transform(TestData);
                    predictedTestData.WriteToConsole();
                    var metrics = Context.MulticlassClassification.Evaluate(predictedTestData, labelColumnName: "LabelAsKey", predictedLabelColumnName: "PredictedLabel");
                    ConsoleExtension.PrintMetrics(metrics);

                    watch.Stop();
                    Console.WriteLine($"(took {watch.ElapsedMilliseconds / 1000} seconds)");

                    break;
                }

            // ignore
            case MLNetExampleScenario.C001_Recommendation_ProductRecommender:
            case MLNetExampleScenario.F001_AnomalyDetection_SalesSpikeDetection_DetectIidSpike:
            case MLNetExampleScenario.F001_AnomalyDetection_SalesSpikeDetection_DetectIidChangePoint:
            case MLNetExampleScenario.F002_AnomalyDetection_PowerAnomalyDetection:
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