using Microsoft.ML;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public partial class MLNetExampleSummary : MLNetHandler
{
    private void SaveModel()
    {
        General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ Save Model", ConsoleColor.Yellow);

        switch (Scenario)
        {
            // save
            case MLNetExampleScenario.A001_Sentiment_Analysis:
            case MLNetExampleScenario.A002_Spam_Detection:
            case MLNetExampleScenario.A003_CreditCardFraudDetection:
            case MLNetExampleScenario.A004_HeartDiseasePrediction:
            case MLNetExampleScenario.B001_IssuesClassification:
            case MLNetExampleScenario.B002_IrisFlowersClassification:
            case MLNetExampleScenario.B003_MNIST:
                {
                    Context.Model.Save(Model, TrainData.Schema, ModelFile.FullName);

                    break;
                }

                // ignore
                {
                    Console.WriteLine("ignore");

                    break;
                }

            // not impleented
            case MLNetExampleScenario.A777_Auto:
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
                {
                    throw new NotImplementedException();
                }
        }

    }
}