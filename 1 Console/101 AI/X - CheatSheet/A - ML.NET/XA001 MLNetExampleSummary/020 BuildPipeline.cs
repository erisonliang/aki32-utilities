using System.Data;

using Microsoft.ML;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using Microsoft.ML.Vision;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public partial class MLNetExampleSummary : MLNetHandler
{
    private void BuildPipeline()
    {
        General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ BuildPipeline", ConsoleColor.Yellow);

        switch (Scenario) // data process → algorithm → data process
        {
            // ★ for binary label, 1 feature
            case MLNetExampleScenario.A001_BinaryClassification_SentimentAnalysis:
                {
                    ConnectNode(Context.Transforms.Text.FeaturizeText("Features", nameof(A001_SentimentInput.Text))); // string → word vector
                    ConnectNode(Context.BinaryClassification.Trainers.SdcaLogisticRegression());

                    break;
                }

            // ★ for multiple label, 1 feature, using 1 vs All
            case MLNetExampleScenario.A002_BinaryClassification_SpamDetection:
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
            case MLNetExampleScenario.A003_BinaryClassification_CreditCardFraudDetection:
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
            case MLNetExampleScenario.A004_BinaryClassification_HeartDiseasePrediction:
                {
                    ConnectNode(Context.Transforms.Concatenate("Features", "Age", "Sex", "Cp", "TrestBps", "Chol", "Fbs", "RestEcg", "Thalac", "Exang", "OldPeak", "Slope", "Ca", "Thal"));
                    ConnectNode(Context.BinaryClassification.Trainers.FastTree());

                    break;
                }

            // ★ for multiple label, many features 
            case MLNetExampleScenario.B002_MultiClassClassification_IrisFlowersClassification:
                {
                    ConnectNode(Context.Transforms.Conversion.MapValueToKey("Label"));
                    ConnectNode(Context.Transforms.Concatenate("Features", nameof(B002_IrisInput.SepalLength), nameof(B002_IrisInput.SepalWidth), nameof(B002_IrisInput.PetalLength), nameof(B002_IrisInput.PetalWidth)));
                    ConnectCheckPoint();
                    ConnectNode(Context.MulticlassClassification.Trainers.SdcaMaximumEntropy());
                    ConnectNode(Context.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

                    break;
                }

            // ★ for multiple label
            case MLNetExampleScenario.B003_MultiClassClassification_MNIST:
                {
                    ConnectNode(Context.Transforms.Conversion.MapValueToKey("Label", "Label", keyOrdinality: ValueToKeyMappingEstimator.KeyOrdinality.ByValue));
                    ConnectNode(Context.Transforms.Concatenate("Features", nameof(B003_MnistInput.PixelValues)));
                    ConnectCheckPoint();
                    ConnectNode(Context.MulticlassClassification.Trainers.SdcaMaximumEntropy());
                    ConnectNode(Context.Transforms.Conversion.MapKeyToValue("Number", "Label"));
                    ConnectNode(Context.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

                    break;
                }

            // ★
            case MLNetExampleScenario.C001_Recommendation_ProductRecommender:
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

            case MLNetExampleScenario.C002_Recommendation_MovieRecommender_MatrixFactorization:
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

            case MLNetExampleScenario.D001_Regression_TaxiFarePrediction:
                {
                    ConnectNode(Context.Transforms.Categorical.OneHotEncoding(outputColumnName: "VendorIdEncoded", inputColumnName: nameof(D001_TaxiFareInput.VendorId)));
                    ConnectNode(Context.Transforms.Categorical.OneHotEncoding(outputColumnName: "RateCodeEncoded", inputColumnName: nameof(D001_TaxiFareInput.RateCode)));
                    ConnectNode(Context.Transforms.Categorical.OneHotEncoding(outputColumnName: "PaymentTypeEncoded", inputColumnName: nameof(D001_TaxiFareInput.PaymentType)));
                    ConnectNode(Context.Transforms.NormalizeMeanVariance(outputColumnName: nameof(D001_TaxiFareInput.PassengerCount)));
                    ConnectNode(Context.Transforms.NormalizeMeanVariance(outputColumnName: nameof(D001_TaxiFareInput.TripTime)));
                    ConnectNode(Context.Transforms.NormalizeMeanVariance(outputColumnName: nameof(D001_TaxiFareInput.TripDistance)));
                    ConnectNode(Context.Transforms.Concatenate("Features",
                        "VendorIdEncoded", "RateCodeEncoded", "PaymentTypeEncoded", nameof(D001_TaxiFareInput.PassengerCount), nameof(D001_TaxiFareInput.TripTime), nameof(D001_TaxiFareInput.TripDistance))); ;

                    ConnectNode(Context.Regression.Trainers.Sdca(labelColumnName: "Label", featureColumnName: "Features"));

                    break;
                }

            case MLNetExampleScenario.F001_AnomalyDetection_SalesSpikeDetection_DetectIidSpike:
                {
                    ConnectNode(Context.Transforms.DetectIidSpike(
                        outputColumnName: nameof(F001_ProductSalesOutput.Prediction),
                        inputColumnName: nameof(F001_ProductSalesInput.numSales),
                        confidence: 95d,
                        pvalueHistoryLength: SpikeDitectionSize / 4));

                    break;
                }

            case MLNetExampleScenario.F001_AnomalyDetection_SalesSpikeDetection_DetectIidChangePoint:
                {
                    ConnectNode(Context.Transforms.DetectIidChangePoint(
                        outputColumnName: nameof(F001_ProductSalesOutput.Prediction),
                        inputColumnName: nameof(F001_ProductSalesInput.numSales),
                        confidence: 95d,
                        changeHistoryLength: SpikeDitectionSize / 4));

                    break;
                }

            case MLNetExampleScenario.F002_AnomalyDetection_PowerAnomalyDetection:
                {
                    ConnectNode(Context.Transforms.DetectSpikeBySsa(
                        outputColumnName: nameof(F002_PowerMeterOutput.Prediction),
                        inputColumnName: "Label",
                        confidence: 98d,
                        pvalueHistoryLength: 30,
                        trainingWindowSize: 90,
                        seasonalityWindowSize: 30));

                    break;
                }

            case MLNetExampleScenario.F003_AnomalyDetection_CreditCardFraudDetection:
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

                    var options = new RandomizedPcaTrainer.Options
                    {
                        FeatureColumnName = "NormalizedFeatures",   // The name of the feature column. The column data must be a known-sized vector of Single.
                        ExampleWeightColumnName = null,             // The name of the example weight column (optional). To use the weight column, the column data must be of type Single.
                        Rank = 22,                                  // The number of components in the PCA.
                        Oversampling = 10,                          // Oversampling parameter for randomized PCA training.
                        EnsureZeroMean = true,                      // If enabled, data is centered to be zero mean.
                        Seed = 1                                    // The seed for random number generation.
                    };

                    ConnectNode(Context.AnomalyDetection.Trainers.RandomizedPca(options: options));

                    //TrainData = Context.Data.FilterRowsByColumn(TrainData, "Label", lowerBound: 0, upperBound: 1 + double.Epsilon);

                    break;
                }

            case MLNetExampleScenario.G002_Clustering_IrisFlowerClustering:
                {
                    ConnectNode(Context.Transforms.Conversion.MapValueToKey("Label"));
                    ConnectNode(Context.Transforms.Concatenate("Features", nameof(B002_IrisInput.SepalLength), nameof(B002_IrisInput.SepalWidth), nameof(B002_IrisInput.PetalLength), nameof(B002_IrisInput.PetalWidth)));
                    ConnectNode(Context.Clustering.Trainers.KMeans(numberOfClusters: 3));
                    //ConnectNode(Context.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

                    break;
                }

            case MLNetExampleScenario.H001_Ranking_RankSearchEngineResults:
                {
                    var featureCols = TrainData.Schema.AsQueryable()
                        .Select(s => s.Name)
                        .Where(c => c != nameof(H001_SearchResultInput.Label))
                        .Where(c => c != nameof(H001_SearchResultInput.GroupId))
                        .ToArray();

                    ConnectNode(Context.Transforms.Concatenate("Features", featureCols));
                    ConnectNode(Context.Transforms.Conversion.MapValueToKey(nameof(H001_SearchResultInput.Label)));
                    ConnectNode(Context.Transforms.Conversion.Hash(nameof(H001_SearchResultInput.GroupId), nameof(H001_SearchResultInput.GroupId), numberOfBits: 20));
                    ConnectNode(Context.Ranking.Trainers.LightGbm(rowGroupColumnName: nameof(H001_SearchResultInput.GroupId)));

                    break;
                }

            case MLNetExampleScenario.I001_ComputerVision_ImageClassificationTraining_HighLevelAPI:
                {
                    //ConnectNode(Context.Transforms.Conversion.MapValueToKey("LabelAsKey", "Label", keyOrdinality: KeyOrdinality.ByValue));

                    var options = new ImageClassificationTrainer.Options()
                    {
                        FeatureColumnName = "Image",
                        LabelColumnName = "LabelAsKey",

                        // Just by changing/selecting InceptionV3/MobilenetV2/ResnetV250
                        // you can try a different DNN architecture (TensorFlow pre-trained model). 
                        Arch = ImageClassificationTrainer.Architecture.ResnetV250,
                        Epoch = 50,       //100
                        BatchSize = 10,
                        LearningRate = 0.01f,
                        MetricsCallback = (metrics) => Console.WriteLine(metrics),
                        ValidationSet = TestData
                    };

                    ConnectNode(Context.MulticlassClassification.Trainers.ImageClassification(options));

                    // or simplly
                    {
                        //ConnectNode(Context.MulticlassClassification.Trainers.ImageClassification(
                        //    featureColumnName: "Image", labelColumnName: "LabelAsKey", validationSet: TestData));
                    }

                    ConnectNode(Context.Transforms.Conversion.MapKeyToValue(outputColumnName: "PredictedLabel", inputColumnName: "PredictedLabel"));

                    break;
                }

            // by ONNX (check input/output name on Netron)
            case MLNetExampleScenario.I004_ComputerVision_ObjectDetection_ImportONNXModel_TinyYoloV2_08:
            case MLNetExampleScenario.I004_ComputerVision_ObjectDetection_ImportONNXModel_YoloV2_09:
            case MLNetExampleScenario.I004_ComputerVision_ObjectDetection_ImportONNXModel_YoloV3_10:
                {
                    ConnectNode(Context.Transforms.CopyColumns("Label", "FileName"));
                    ConnectNode(Context.Transforms.LoadImages("Image", "", "ImagePath"));
                    ConnectNode(Context.Transforms.ResizeImages("Image", I004_Config.ImageWidth, I004_Config.ImageHeight, "Image"));
                    ConnectNode(Context.Transforms.ExtractPixels("Image"));

                    switch (Scenario)
                    {
                        // for tinyyolov2-8.onnx
                        case MLNetExampleScenario.I004_ComputerVision_ObjectDetection_ImportONNXModel_TinyYoloV2_08:
                            {
                                ConnectNode(Context.Transforms.CopyColumns("image", "Image"));
                                ConnectNode(Context.Transforms.ApplyOnnxModel(new[] { "grid" }, new[] { "image" }, ModelFile.FullName));
                                ConnectNode(Context.Transforms.CopyColumns("PredictedLabels", "grid"));
                            }
                            break;
                        // for yolov2-coco-9.onnx
                        case MLNetExampleScenario.I004_ComputerVision_ObjectDetection_ImportONNXModel_YoloV2_09:
                            {
                                ConnectNode(Context.Transforms.CopyColumns("input.1", "Image"));
                                ConnectNode(Context.Transforms.ApplyOnnxModel(new[] { "218" }, new[] { "input.1" }, ModelFile.FullName));
                                ConnectNode(Context.Transforms.CopyColumns("PredictedLabels", "218"));
                            }
                            break;
                        // for yolov3-10.onnx
                        case MLNetExampleScenario.I004_ComputerVision_ObjectDetection_ImportONNXModel_YoloV3_10:
                            {

                                // TODO
                                throw new NotImplementedException();

                            }
                            break;
                    }

                    break;
                }

            // ignore
            case MLNetExampleScenario.A777_BinaryClassification_Auto_SentimentAnalysis:
            case MLNetExampleScenario.B777_MultiClassClassification_Auto_MNIST:
            case MLNetExampleScenario.D777_Regression_Auto_TaxiFarePrediction:
            case MLNetExampleScenario.H777_Ranking_Auto_RankSearchEngineResults:
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

        try
        {
            TrainData.WriteToConsole();
        }
        catch (Exception)
        {
        }
    }
}