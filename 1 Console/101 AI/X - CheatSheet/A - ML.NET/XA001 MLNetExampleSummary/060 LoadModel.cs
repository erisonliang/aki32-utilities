

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public partial class MLNetExampleSummary : MLNetHandler
{
    private void LoadModel()
    {
        General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ Load Model", ConsoleColor.Yellow);

        switch (Scenario)
        {
            // from zip
            case MLNetExampleScenario.A001_BinaryClassification_SentimentAnalysis:
            case MLNetExampleScenario.A002_BinaryClassification_SpamDetection:
            case MLNetExampleScenario.A003_BinaryClassification_CreditCardFraudDetection:
            case MLNetExampleScenario.A004_BinaryClassification_HeartDiseasePrediction:
            case MLNetExampleScenario.A777_BinaryClassification_Auto_SentimentAnalysis:
            case MLNetExampleScenario.B001_MultiClassClassification_IssuesClassification:
            case MLNetExampleScenario.B002_MultiClassClassification_IrisFlowersClassification:
            case MLNetExampleScenario.B003_MultiClassClassification_MNIST:
            case MLNetExampleScenario.B777_MultiClassClassification_Auto_MNIST:
            case MLNetExampleScenario.C001_Recommendation_ProductRecommender:
            case MLNetExampleScenario.C002_Recommendation_MovieRecommender_MatrixFactorization:
            case MLNetExampleScenario.D001_Regression_TaxiFarePrediction:
            case MLNetExampleScenario.D777_Regression_Auto_TaxiFarePrediction:
            case MLNetExampleScenario.F001_AnomalyDetection_SalesSpikeDetection_DetectIidSpike:
            case MLNetExampleScenario.F001_AnomalyDetection_SalesSpikeDetection_DetectIidChangePoint:
            case MLNetExampleScenario.F002_AnomalyDetection_PowerAnomalyDetection:
            case MLNetExampleScenario.F003_AnomalyDetection_CreditCardFraudDetection:
            case MLNetExampleScenario.G002_Clustering_IrisFlowerClustering:
            case MLNetExampleScenario.H001_Ranking_RankSearchEngineResults:
            case MLNetExampleScenario.I001_ComputerVision_ImageClassificationTraining_HighLevelAPI:
                {
                    Model = Context.Model.Load(ModelFile.FullName, out var inputSchema);
                    ModelInputSchema = inputSchema;
                    Console.WriteLine("done");

                    break;
                }

            // ignore
            case MLNetExampleScenario.I004_ComputerVision_ObjectDetection_ImportONNXModel_TinyYoloV2_08:
            case MLNetExampleScenario.I004_ComputerVision_ObjectDetection_ImportONNXModel_YoloV2_09:
            case MLNetExampleScenario.I004_ComputerVision_ObjectDetection_ImportONNXModel_YoloV3_10:
                {
                    Console.WriteLine("ignore");

                    break;
                }

            // not implemented
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