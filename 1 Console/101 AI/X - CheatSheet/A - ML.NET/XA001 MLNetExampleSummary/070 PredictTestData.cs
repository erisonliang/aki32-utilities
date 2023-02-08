using Aki32Utilities.ConsoleAppUtilities.General;

using Microsoft.ML;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public partial class MLNetExampleSummary : MLNetHandler
{
    private void PredictTestData()
    {
        General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ Test Data Prediction", ConsoleColor.Yellow);

        switch (Scenario)
        {
            // for BinaryClassification
            case MLNetExampleScenario.A001_Sentiment_Analysis:
            case MLNetExampleScenario.A003_CreditCardFraudDetection:
            case MLNetExampleScenario.A004_HeartDiseasePrediction:
            case MLNetExampleScenario.A777_Auto:
                {
                    var predictedTestData = Model.Transform(TestData);
                    predictedTestData.WriteToConsole();
                    var metrics = Context.BinaryClassification.Evaluate(predictedTestData);
                    ConsoleExtension.PrintMetrics(metrics);

                    break;
                }

            // for MulticlassClassification
            case MLNetExampleScenario.A002_Spam_Detection:
            case MLNetExampleScenario.B002_IrisFlowersClassification:
            case MLNetExampleScenario.B003_MNIST:
            case MLNetExampleScenario.B777_Auto:
                {
                    var predictedTestData = Model.Transform(TestData);
                    predictedTestData.WriteToConsole();
                    var metrics = Context.MulticlassClassification.Evaluate(predictedTestData);
                    ConsoleExtension.PrintMetrics(metrics);

                    break;
                }

            // for Regression
            case MLNetExampleScenario.C002_MovieRecommender_MatrixFactorization:
                {
                    var predictedTestData = Model.Transform(TestData);
                    predictedTestData.WriteToConsole();
                    var metrics = Context.Regression.Evaluate(predictedTestData);
                    ConsoleExtension.PrintMetrics(metrics);

                    break;
                }

            case MLNetExampleScenario.I004_ObjectDetection_ONNXModelScoring:
                {


                    var modelScorer = new OnnxModelScorer(I004_ImagesDir.FullName, ModelFile.FullName, Context);



                    IEnumerable<float[]> probabilities = modelScorer.Score(TestData);



                    var parser = new YoloOutputParser();
                    var boundingBoxes = probabilities
                        .Select(probability => parser.ParseOutputs(probability))
                        .Select(boxes => parser.FilterBoundingBoxes(boxes, 5, .5F));


                    // Draw bounding boxes for detected objects in each of the images
                    for (var i = 0; i < I004_Images.Count(); i++)
                    {
                        string imageFileName = I004_Images.ElementAt(i).Label;
                        IList<YoloBoundingBox> detectedObjects = boundingBoxes.ElementAt(i);

                        var outputDir = I004_ImagesDir.GetChildDirectoryInfo("Output");
                        I004_DrawBoundingBox(I004_ImagesDir.FullName, outputDir.FullName, imageFileName, detectedObjects);

                        I004_LogDetectedObjects(imageFileName, detectedObjects);
                    }

                    break;
                }

            // ignore
            case MLNetExampleScenario.C001_ProductRecommendation:
                {
                    Console.WriteLine("ignore");

                    break;
                }

            // not implemented
            case MLNetExampleScenario.B001_IssuesClassification:
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