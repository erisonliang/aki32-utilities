using Microsoft.ML;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public partial class MLNetExampleSummary : MLNetHandler
{
    private void BuildPipeline()
    {
        General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ Cross Validation", ConsoleColor.Yellow);

        switch (Scenario) // data process → algorithm → data process
        {
            // ★ for binary label, 1 feature
            case MLNetExampleScenario.A001_Sentiment_Analysis:
                {
                    ConnectNode(Context.Transforms.Text.FeaturizeText("Features", nameof(A001_SentimentInput.Text))); // string → word vector
                    ConnectNode(Context.BinaryClassification.Trainers.SdcaLogisticRegression());

                    break;
                }

            // ★ for multiple label, 1 feature, using 1 vs All
            case MLNetExampleScenario.A002_Spam_Detection:
                {
                    ConnectNode(Context.Transforms.Conversion.MapValueToKey("Label", "Label"));  // string key → int value

                    var option1 = new Microsoft.ML.Transforms.Text.TextFeaturizingEstimator.Options
                    {
                        WordFeatureExtractor = new Microsoft.ML.Transforms.Text.WordBagEstimator.Options { NgramLength = 2, UseAllLengths = true },
                        CharFeatureExtractor = new Microsoft.ML.Transforms.Text.WordBagEstimator.Options { NgramLength = 3, UseAllLengths = false },
                        Norm = Microsoft.ML.Transforms.Text.TextFeaturizingEstimator.NormFunction.L2,
                    };
                    ConnectNode(Context.Transforms.Text.FeaturizeText("Features", option1, "Message"));
                    ConnectCheckPoint();

                    ConnectNode(Context.MulticlassClassification.Trainers.OneVersusAll(
                           Context.BinaryClassification.Trainers.AveragedPerceptron()));
                    ConnectNode(Context.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel")); // int value → string key

                    break;
                }

            // ★ for binary label, many features, selecting with LINQ
            case MLNetExampleScenario.A003_CreditCardFraudDetection:
                {
                    // ★ for multiple label, many features 
                    var featureColumnNames = AllData.Schema.AsQueryable()
                        .Select(column => column.Name)
                        .Where(name => name != "ExcludingColumn")
                        .Where(name => name != "IdPreservationColumn")
                        .Where(name => name != "Label")
                        .ToArray();
                    ConnectNode(Context.Transforms.Concatenate("Features", featureColumnNames));
                    ConnectNode(Context.Transforms.DropColumns(new string[] { "ExcludingColumn" }));
                    ConnectNode(Context.Transforms.NormalizeMeanVariance("NormalizedFeatures", "Features"));

                    ConnectNode(Context.BinaryClassification.Trainers.FastTree(
                        featureColumnName: "NormalizedFeatures",
                        numberOfLeaves: 20,
                        numberOfTrees: 100,
                        minimumExampleCountPerLeaf: 10,
                        learningRate: 0.2));

                    break;
                }

            // ★ for multiple label, many features, simple 
            case MLNetExampleScenario.A004_HeartDiseasePrediction:
                {
                    ConnectNode(Context.Transforms.Concatenate("Features", "Age", "Sex", "Cp", "TrestBps", "Chol", "Fbs", "RestEcg", "Thalac", "Exang", "OldPeak", "Slope", "Ca", "Thal"));
                    ConnectNode(Context.BinaryClassification.Trainers.FastTree());

                    break;
                }

            // ★ for multiple label, many features 
            case MLNetExampleScenario.B002_IrisFlowersClassification:
                {
                    ConnectNode(Context.Transforms.Conversion.MapValueToKey("Label"));
                    ConnectNode(Context.Transforms.Concatenate("Features", nameof(B002_IrisInput.SepalLength), nameof(B002_IrisInput.SepalWidth), nameof(B002_IrisInput.PetalLength), nameof(B002_IrisInput.PetalWidth)));
                    ConnectCheckPoint();
                    ConnectNode(Context.MulticlassClassification.Trainers.SdcaMaximumEntropy());
                    ConnectNode(Context.Transforms.Conversion.MapKeyToValue("Label"));
                    ConnectNode(Context.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

                    break;
                }

            // ★ for multiple label
            case MLNetExampleScenario.B003_MNIST:
                {
                    ConnectNode(Context.Transforms.Conversion.MapValueToKey("Label", "Number", keyOrdinality: ValueToKeyMappingEstimator.KeyOrdinality.ByValue));
                    ConnectNode(Context.Transforms.Concatenate("Features", nameof(B003_MnistInput.PixelValues)));
                    ConnectCheckPoint();
                    ConnectNode(Context.MulticlassClassification.Trainers.SdcaMaximumEntropy());
                    ConnectNode(Context.Transforms.Conversion.MapKeyToValue("Number", "Label"));
                    ConnectNode(Context.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

                    break;
                }

            // ★
            case MLNetExampleScenario.C001_ProductRecommendation:
                {
                    var options = new MatrixFactorizationTrainer.Options
                    {
                        MatrixColumnIndexColumnName = nameof(C001_ProductInput.ProductID),
                        MatrixRowIndexColumnName = nameof(C001_ProductInput.CoPurchaseProductID),
                        LabelColumnName = "Label",
                        LossFunction = MatrixFactorizationTrainer.LossFunctionType.SquareLossOneClass,
                        Alpha = 0.01,
                        Lambda = 0.025,

                        //// for better
                        //NumberOfIterations = 100,
                        //C = 0.00001,
                    };

                    ConnectNode(Context.Recommendation().Trainers.MatrixFactorization(options));

                    break;
                }

            case MLNetExampleScenario.C002_MovieRecommender_MatrixFactorization:
                {
                    ConnectNode(Context.Transforms.Conversion.MapValueToKey("userIdEncoded", nameof(C002_MovieRateInput.userId)));
                    ConnectNode(Context.Transforms.Conversion.MapValueToKey("movieIdEncoded", nameof(C002_MovieRateInput.movieId)));

                    var options = new MatrixFactorizationTrainer.Options
                    {
                        MatrixColumnIndexColumnName = "userIdEncoded",
                        MatrixRowIndexColumnName = "movieIdEncoded",
                        LabelColumnName = "Label",
                        NumberOfIterations = 20,
                        ApproximationRank = 100
                    };

                    ConnectNode(Context.Recommendation().Trainers.MatrixFactorization(options));

                    break;
                }

            // by ONNX (check input/output name on Netron)
            case MLNetExampleScenario.I004_ObjectDetection_ONNXModelScoring_TinyYoloV2_08:
            case MLNetExampleScenario.I004_ObjectDetection_ONNXModelScoring_YoloV2_09:
            case MLNetExampleScenario.I004_ObjectDetection_ONNXModelScoring_YoloV3_10:
                {
                    ConnectNode(Context.Transforms.CopyColumns("Label", "FileName"));
                    ConnectNode(Context.Transforms.LoadImages("Image", "", "ImagePath"));
                    ConnectNode(Context.Transforms.ResizeImages("Image", I004_Config.ImageWidth, I004_Config.ImageHeight, "Image"));
                    ConnectNode(Context.Transforms.ExtractPixels("Image"));

                    switch (Scenario)
                    {
                        // for tinyyolov2-8.onnx
                        case MLNetExampleScenario.I004_ObjectDetection_ONNXModelScoring_TinyYoloV2_08:
                            {
                                ConnectNode(Context.Transforms.CopyColumns("image", "Image"));
                                ConnectNode(Context.Transforms.ApplyOnnxModel(new[] { "grid" }, new[] { "image" }, ModelFile.FullName));
                                ConnectNode(Context.Transforms.CopyColumns("PredictedLabels", "grid"));
                            }
                            break;
                        // for yolov2-coco-9.onnx
                        case MLNetExampleScenario.I004_ObjectDetection_ONNXModelScoring_YoloV2_09:
                            {
                                ConnectNode(Context.Transforms.CopyColumns("input.1", "Image"));
                                ConnectNode(Context.Transforms.ApplyOnnxModel(new[] { "218" }, new[] { "input.1" }, ModelFile.FullName));
                                ConnectNode(Context.Transforms.CopyColumns("PredictedLabels", "218"));
                            }
                            break;
                        // for yolov3-10.onnx
                        case MLNetExampleScenario.I004_ObjectDetection_ONNXModelScoring_YoloV3_10:
                            {

                                // TODO
                                throw new NotImplementedException();

                            }
                            break;
                    }

                    break;
                }

                // ignore
                {
                    Console.WriteLine("ignore");

                    break;
                }

            // not implemented
            case MLNetExampleScenario.A777_Auto:
            case MLNetExampleScenario.B001_IssuesClassification:
            case MLNetExampleScenario.B777_Auto:
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

        try
        {
            TrainData.WriteToConsole();
        }
        catch (Exception)
        {
        }
    }
}