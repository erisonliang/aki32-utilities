using Aki32Utilities.ConsoleAppUtilities.General;

using Microsoft.ML;
using Microsoft.ML.Data;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public partial class MLNetExampleSummary : MLNetHandler
{
    private void LoadData()
    {
        General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ Load Data", ConsoleColor.Yellow);

        void DownloadDataFile(Uri uri, FileInfo targetDataFile)
        {
            if (!targetDataFile.Exists)
                uri.DownloadFile(targetDataFile);
        }

        void DownloadAndExtractZipDataFile(Uri uri, FileInfo targetFileLocationAfterExtracted, FileInfo targetDataFile)
        {
            if (!targetDataFile.Exists)
            {
                var zipDir = DataDir.GetChildDirectoryInfo("zip").CreateAndPipe();
                uri.DownloadFile(zipDir.GetChildFileInfo("downloaded.zip")).Decompress_Zip(zipDir);
                targetFileLocationAfterExtracted.MoveTo(targetDataFile);
            }
        }

        void SplitData(double testFraction = 0.2)
        {
            var split = Context.Data.TrainTestSplit(AllData, testFraction);
            TrainData = split.TrainSet;
            TestData = split.TestSet;
        }

        switch (Scenario)
        {
            // normal download
            case MLNetExampleScenario.A001_BinaryClassification_SentimentAnalysis:
                {
                    var allDataFile = DataDir.GetChildFileInfo("Sentiment.tsv");
                    ModelFile = DataDir.GetChildFileInfo("Sentiment-Model.zip");

                    var uri = new Uri("https://github.com/dotnet/machinelearning-samples/blob/main/samples/csharp/getting-started/BinaryClassification_SentimentAnalysis/SentimentAnalysis/Data/wikiDetoxAnnotated40kRows.tsv?raw=true");
                    DownloadDataFile(uri, allDataFile);

                    AllData = Context.Data.LoadFromTextFile<A001_SentimentInput>(allDataFile.FullName, hasHeader: true, separatorChar: '\t');
                    SplitData();

                    break;
                }

            // zip
            case MLNetExampleScenario.A002_BinaryClassification_SpamDetection:
                {
                    var allDataFile = DataDir.GetChildFileInfo("Spam.tsv");
                    ModelFile = DataDir.GetChildFileInfo("Spam-Model.zip");
                    var targetFileLocationAfterExtracted = DataDir.GetChildDirectoryInfo("zip").GetChildFileInfo("SMSSpamCollection");

                    var uri = new Uri("https://archive.ics.uci.edu/ml/machine-learning-databases/00228/smsspamcollection.zip");
                    DownloadAndExtractZipDataFile(uri, targetFileLocationAfterExtracted, allDataFile);

                    AllData = Context.Data.LoadFromTextFile<A002_SpamInput>(allDataFile.FullName, hasHeader: false, separatorChar: '\t');
                    SplitData();

                    break;
                }

            case MLNetExampleScenario.A003_BinaryClassification_CreditCardFraudDetection:
                {
                    var allDataFile = DataDir.GetChildFileInfo("Transaction.csv");
                    ModelFile = DataDir.GetChildFileInfo("Transaction-Model.zip");
                    var targetFileLocationAfterExtracted = DataDir.GetChildDirectoryInfo("zip").GetChildFileInfo("creditcard.csv");

                    var uri = new Uri("https://github.com/dotnet/machinelearning-samples/blob/main/samples/csharp/getting-started/BinaryClassification_CreditCardFraudDetection/CCFraudDetection.Trainer/assets/input/creditcardfraud-dataset.zip?raw=true");
                    DownloadAndExtractZipDataFile(uri, targetFileLocationAfterExtracted, allDataFile);

                    AllData = Context.Data.LoadFromTextFile<A003_TransactionInput>(allDataFile.FullName, hasHeader: true, separatorChar: ',');
                    SplitData();

                    break;
                }

            case MLNetExampleScenario.A004_BinaryClassification_HeartDiseasePrediction:
                {
                    var trainDataFile = DataDir.GetChildFileInfo("Heart-Train.csv");
                    var trainDataUri = new Uri("https://raw.githubusercontent.com/dotnet/machinelearning-samples/main/samples/csharp/getting-started/BinaryClassification_HeartDiseaseDetection/HeartDiseaseDetection/Data/HeartTraining.csv");
                    DownloadDataFile(trainDataUri, trainDataFile);
                    TrainData = Context.Data.LoadFromTextFile<A004_HeartInput>(trainDataFile.FullName, hasHeader: true, separatorChar: ';');

                    var testDataFile = DataDir.GetChildFileInfo("Heart-Test.csv");
                    var testDataUri = new Uri("https://raw.githubusercontent.com/dotnet/machinelearning-samples/main/samples/csharp/getting-started/BinaryClassification_HeartDiseaseDetection/HeartDiseaseDetection/Data/HeartTest.csv");
                    DownloadDataFile(testDataUri, testDataFile);
                    TestData = Context.Data.LoadFromTextFile<A004_HeartInput>(testDataFile.FullName, hasHeader: true, separatorChar: ';');

                    ModelFile = DataDir.GetChildFileInfo("Heart-Model.zip");

                    break;
                }

            case MLNetExampleScenario.A777_BinaryClassification_Auto_SentimentAnalysis:
                {
                    var trainDataFile = DataDir.GetChildFileInfo("Sentiment-Train.tsv");
                    var trainDataUri = new Uri("https://github.com/dotnet/machinelearning-samples/raw/main/datasets/wikipedia-detox-250-line-data.tsv");
                    DownloadDataFile(trainDataUri, trainDataFile);
                    TrainData = Context.Data.LoadFromTextFile<A777_AutoSentimentInput>(trainDataFile.FullName, hasHeader: true);

                    var testDataFile = DataDir.GetChildFileInfo("Sentiment-Test.tsv");
                    var testDataUri = new Uri("https://github.com/dotnet/machinelearning-samples/raw/main/datasets/wikipedia-detox-250-line-test.tsv");
                    DownloadDataFile(testDataUri, testDataFile);
                    TestData = Context.Data.LoadFromTextFile<A777_AutoSentimentInput>(testDataFile.FullName, hasHeader: true);

                    ModelFile = DataDir.GetChildFileInfo("Sentiment-Model.zip");

                    break;
                }

            case MLNetExampleScenario.B002_MultiClassClassification_IrisFlowersClassification:
                {
                    var allDataFile = DataDir.GetChildFileInfo("Iris.txt");
                    ModelFile = DataDir.GetChildFileInfo("Iris-Model.zip");

                    var uri = new Uri("https://raw.githubusercontent.com/dotnet/machinelearning-samples/main/samples/csharp/getting-started/MulticlassClassification_Iris/IrisClassification/Data/iris-full.txt");
                    DownloadDataFile(uri, allDataFile);

                    AllData = Context.Data.LoadFromTextFile<B002_IrisInput>(allDataFile.FullName, hasHeader: true, separatorChar: '\t');
                    SplitData();

                    break;
                }

            // many features, the same type ((!)Trying 2 types of reading way)
            case MLNetExampleScenario.B003_MultiClassClassification_MNIST:
                {
                    var trainDataFile = DataDir.GetChildFileInfo("MNIST-Train.csv");
                    var trainDataUri = new Uri("https://raw.githubusercontent.com/dotnet/machinelearning-samples/main/samples/csharp/getting-started/MulticlassClassification_MNIST/MNIST/Data/optdigits-train.csv");
                    DownloadDataFile(trainDataUri, trainDataFile);
                    TrainData = Context.Data.LoadFromTextFile<B003_MnistInput>(trainDataFile.FullName, hasHeader: false, separatorChar: ',');

                    var testDataFile = DataDir.GetChildFileInfo("MNIST-Test.csv");
                    var testDataUri = new Uri("https://raw.githubusercontent.com/dotnet/machinelearning-samples/main/samples/csharp/getting-started/MulticlassClassification_MNIST/MNIST/Data/optdigits-val.csv");
                    DownloadDataFile(testDataUri, testDataFile);
                    TestData = Context.Data.LoadFromTextFile(testDataFile.FullName,
                        columns: new[]
                        {
                            new TextLoader.Column(nameof(B003_MnistInput.PixelValues), DataKind.Single, 0, 63), // 64 single values
                            new TextLoader.Column(nameof(B003_MnistInput.Number), DataKind.Single, 64) // 1 single value
                        },
                        hasHeader: false,
                        separatorChar: ','
                        );

                    ModelFile = DataDir.GetChildFileInfo("MNIST-Model.zip");

                    break;
                }

            case MLNetExampleScenario.C001_Recommendation_ProductRecommender:
                {
                    var allDataFile = DataDir.GetChildFileInfo("Product.txt");
                    ModelFile = DataDir.GetChildFileInfo("Product-Model.zip");

                    var uri = new Uri("https://github.com/dotnet/machinelearning-samples/blob/main/samples/csharp/getting-started/MatrixFactorization_ProductRecommendation/ProductRecommender/Data/Amazon0302.txt?raw=true");
                    DownloadDataFile(uri, allDataFile);

                    AllData = Context.Data.LoadFromTextFile(allDataFile.FullName,
                        columns: new[]
                        {
                            new TextLoader.Column("Label", DataKind.Single, 0),
                            new TextLoader.Column(name:nameof(C001_ProductInput.ProductID), dataKind:DataKind.UInt32, source: new [] { new TextLoader.Range(0) }, keyCount: new KeyCount(262111)),
                            new TextLoader.Column(name:nameof(C001_ProductInput.CoPurchaseProductID), dataKind:DataKind.UInt32, source: new [] { new TextLoader.Range(1) }, keyCount: new KeyCount(262111))
                        },
                        hasHeader: true,
                        separatorChar: '\t');

                    TrainData = AllData;

                    break;
                }

            case MLNetExampleScenario.C002_Recommendation_MovieRecommender_MatrixFactorization:
                {
                    var movieDataFile = DataDir.GetChildFileInfo("Movie.csv");
                    var movieDataUri = new Uri("https://raw.githubusercontent.com/dotnet/machinelearning-samples/main/samples/csharp/getting-started/MatrixFactorization_MovieRecommendation/Data/recommendation-movies.csv");
                    DownloadDataFile(movieDataUri, movieDataFile);

                    var trainDataFile = DataDir.GetChildFileInfo("MovieRate-Train.csv");
                    var trainDataUri = new Uri("https://raw.githubusercontent.com/dotnet/machinelearning-samples/main/samples/csharp/getting-started/MatrixFactorization_MovieRecommendation/Data/recommendation-ratings-train.csv");
                    DownloadDataFile(trainDataUri, trainDataFile);
                    TrainData = Context.Data.LoadFromTextFile<C002_MovieRateInput>(trainDataFile.FullName, hasHeader: true, separatorChar: ',');

                    var testDataFile = DataDir.GetChildFileInfo("MovieRate-Test.csv");
                    var testDataUri = new Uri("https://raw.githubusercontent.com/dotnet/machinelearning-samples/main/samples/csharp/getting-started/MatrixFactorization_MovieRecommendation/Data/recommendation-ratings-test.csv");
                    DownloadDataFile(testDataUri, testDataFile);
                    TestData = Context.Data.LoadFromTextFile<C002_MovieRateInput>(testDataFile.FullName, hasHeader: true, separatorChar: ',');

                    ModelFile = DataDir.GetChildFileInfo("MovieRate-Model.zip");

                    break;
                }

            // ONNX
            case MLNetExampleScenario.I004_ComputerVision_ObjectDetection_ImportONNXModel_TinyYoloV2_08:
            case MLNetExampleScenario.I004_ComputerVision_ObjectDetection_ImportONNXModel_YoloV2_09:
            case MLNetExampleScenario.I004_ComputerVision_ObjectDetection_ImportONNXModel_YoloV3_10:
                {
                    // yolo model is available here https://github.com/onnx/models/tree/main/vision/object_detection_segmentation

                    switch (Scenario)
                    {
                        // for tinyyolov2-8.onnx
                        case MLNetExampleScenario.I004_ComputerVision_ObjectDetection_ImportONNXModel_TinyYoloV2_08:
                            {
                                ModelFile = DataDir.GetChildDirectoryInfo("Model").GetChildFileInfo("tinyyolov2-8.onnx");
                                var modelUri = new Uri("https://github.com/onnx/models/raw/main/vision/object_detection_segmentation/tiny-yolov2/model/tinyyolov2-8.onnx");
                                DownloadDataFile(modelUri, ModelFile);
                            }
                            break;
                        // for yolov2-coco-9.onnx
                        case MLNetExampleScenario.I004_ComputerVision_ObjectDetection_ImportONNXModel_YoloV2_09:
                            {
                                ModelFile = DataDir.GetChildDirectoryInfo("Model").GetChildFileInfo("yolov2-coco-9.onnx");
                                var modelUri = new Uri("https://github.com/onnx/models/raw/main/vision/object_detection_segmentation/yolov2-coco/model/yolov2-coco-9.onnx");
                                DownloadDataFile(modelUri, ModelFile);
                            }
                            break;
                        // for yolov3-10.onnx
                        case MLNetExampleScenario.I004_ComputerVision_ObjectDetection_ImportONNXModel_YoloV3_10:
                            {
                                ModelFile = DataDir.GetChildDirectoryInfo("Model").GetChildFileInfo("yolov3-10.onnx");
                                var modelUri = new Uri("https://github.com/onnx/models/blob/main/vision/object_detection_segmentation/yolov3/model/yolov3-10.onnx");
                                DownloadDataFile(modelUri, ModelFile);
                            }
                            break;
                    }

                    var imageDir = DataDir.GetChildDirectoryInfo("Images");
                    for (int i = 1; i <= 4; i++)
                    {
                        var imageFile = imageDir.GetChildFileInfo($"Image{i}.jpg");
                        var imageUri = new Uri($"https://github.com/dotnet/machinelearning-samples/raw/main/samples/csharp/getting-started/DeepLearning_ObjectDetection_Onnx/ObjectDetectionConsoleApp/assets/images/image{i}.jpg");
                        DownloadDataFile(imageUri, imageFile);
                    }

                    break;
                }

            // ignore
            case MLNetExampleScenario.Z999_Ignore:
                {
                    Console.WriteLine("ignore");

                    break;
                }

            // not implemented
            case MLNetExampleScenario.B777_MultiClassClassification_Auto_MNIST:
            case MLNetExampleScenario.C003_Recommendation_MovieRecommender_FieldAwareFactorizationMachines:
            case MLNetExampleScenario.C777_Auto_Recommendation:
            case MLNetExampleScenario.D001_Regression_PricePrediction:
            case MLNetExampleScenario.D002_Regression_SalesForecasting:
            case MLNetExampleScenario.D003_Regression_DemandPrediction:
            case MLNetExampleScenario.D777_Regression_Auto_TaxiFarePrediction:
            case MLNetExampleScenario.E001_TimeSeriesForecasting_SalesForecasting:
            case MLNetExampleScenario.F001_AnomalyDetection_SalesSpikeDetection:
            case MLNetExampleScenario.F002_AnomalyDetection_PowerAnomalyDetection:
            case MLNetExampleScenario.F003_AnomalyDetection_CreditCardFraudDetection:
            case MLNetExampleScenario.G001_Clustering_CustomerSegmentation:
            case MLNetExampleScenario.G002_Clustering_IrisFlowerClustering:
            case MLNetExampleScenario.H001_Ranking_RankSearchEngineResults:
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

            // not going to implement
            case MLNetExampleScenario.B001_MultiClassClassification_IssuesClassification:
                {
                    throw new NotImplementedException();
                }
        }

        Console.WriteLine("done");
    }
}