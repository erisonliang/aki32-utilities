﻿using Aki32Utilities.ConsoleAppUtilities.General;

using Microsoft.ML;
using Microsoft.ML.Data;

using static Microsoft.ML.Transforms.ValueToKeyMappingEstimator;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public partial class MLNetExampleSummary : MLNetHandler
{
    private void LoadData()
    {
        General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ Load Data", ConsoleColor.Yellow);

        switch (Scenario)
        {
            // normal download
            case MLNetExampleScenario.A001_BinaryClassification_SentimentAnalysis:
            case MLNetExampleScenario.A777_BinaryClassification_Auto_SentimentAnalysis:
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
                    var targetFileLocationAfterExtracted = DataDir.GetChildDirectoryInfo("zip").GetChildFileInfo("SMSSpamCollection");
                    ModelFile = DataDir.GetChildFileInfo("Spam-Model.zip");

                    var uri = new Uri("https://archive.ics.uci.edu/ml/machine-learning-databases/00228/smsspamcollection.zip");
                    DownloadAndExtractZipDataFile(uri, allDataFile, targetFileLocationAfterExtracted);

                    AllData = Context.Data.LoadFromTextFile<A002_SpamInput>(allDataFile.FullName, hasHeader: false, separatorChar: '\t');
                    SplitData();

                    break;
                }

            case MLNetExampleScenario.A003_BinaryClassification_CreditCardFraudDetection:
            case MLNetExampleScenario.F003_AnomalyDetection_CreditCardFraudDetection:
                {
                    var allDataFile = DataDir.GetChildFileInfo("Transaction.csv");
                    ModelFile = DataDir.GetChildFileInfo("Transaction-Model.zip");
                    var targetFileLocationAfterExtracted = DataDir.GetChildDirectoryInfo("zip").GetChildFileInfo("creditcard.csv");

                    var uri = new Uri("https://github.com/dotnet/machinelearning-samples/blob/main/samples/csharp/getting-started/BinaryClassification_CreditCardFraudDetection/CCFraudDetection.Trainer/assets/input/creditcardfraud-dataset.zip?raw=true");
                    DownloadAndExtractZipDataFile(uri, allDataFile, targetFileLocationAfterExtracted);

                    switch (Scenario)
                    {
                        case MLNetExampleScenario.A003_BinaryClassification_CreditCardFraudDetection:
                            AllData = Context.Data.LoadFromTextFile<A003_TransactionInput>(allDataFile.FullName, hasHeader: true, separatorChar: ',');
                            break;
                        case MLNetExampleScenario.F003_AnomalyDetection_CreditCardFraudDetection:
                            AllData = Context.Data.LoadFromTextFile<F003_TransactionInput>(allDataFile.FullName, hasHeader: true, separatorChar: ',');
                            break;
                        default:
                            break;
                    }

                    SplitData();

                    break;
                }

            case MLNetExampleScenario.A004_BinaryClassification_HeartDiseasePrediction:
                {
                    {
                        var trainDataFile = DataDir.GetChildFileInfo("Heart-Train.csv");
                        var trainDataUri = new Uri("https://raw.githubusercontent.com/dotnet/machinelearning-samples/main/samples/csharp/getting-started/BinaryClassification_HeartDiseaseDetection/HeartDiseaseDetection/Data/HeartTraining.csv");
                        DownloadDataFile(trainDataUri, trainDataFile);
                        TrainData = Context.Data.LoadFromTextFile<A004_HeartInput>(trainDataFile.FullName, hasHeader: true, separatorChar: ';');
                    }
                    {
                        var testDataFile = DataDir.GetChildFileInfo("Heart-Test.csv");
                        var testDataUri = new Uri("https://raw.githubusercontent.com/dotnet/machinelearning-samples/main/samples/csharp/getting-started/BinaryClassification_HeartDiseaseDetection/HeartDiseaseDetection/Data/HeartTest.csv");
                        DownloadDataFile(testDataUri, testDataFile);
                        TestData = Context.Data.LoadFromTextFile<A004_HeartInput>(testDataFile.FullName, hasHeader: true, separatorChar: ';');
                    }
                    {
                        ModelFile = DataDir.GetChildFileInfo("Heart-Model.zip");
                    }

                    break;
                }

            case MLNetExampleScenario.B002_MultiClassClassification_IrisFlowersClassification:
            case MLNetExampleScenario.G002_Clustering_IrisFlowerClustering:
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
            case MLNetExampleScenario.B777_MultiClassClassification_Auto_MNIST:
                {
                    {
                        var trainDataFile = DataDir.GetChildFileInfo("MNIST-Train.csv");
                        var trainDataUri = new Uri("https://raw.githubusercontent.com/dotnet/machinelearning-samples/main/samples/csharp/getting-started/MulticlassClassification_MNIST/MNIST/Data/optdigits-train.csv");
                        //var trainDataUri = new Uri("https://github.com/dotnet/machinelearning-samples/raw/main/datasets/optdigits-train.csv"); // same data set
                        DownloadDataFile(trainDataUri, trainDataFile);
                        TrainData = Context.Data.LoadFromTextFile<B003_MnistInput>(trainDataFile.FullName, hasHeader: false, separatorChar: ',');
                    }
                    {
                        var testDataFile = DataDir.GetChildFileInfo("MNIST-Test.csv");
                        var testDataUri = new Uri("https://raw.githubusercontent.com/dotnet/machinelearning-samples/main/samples/csharp/getting-started/MulticlassClassification_MNIST/MNIST/Data/optdigits-val.csv");
                        //var testDataUri = new Uri("https://github.com/dotnet/machinelearning-samples/raw/main/datasets/optdigits-test.csv"); // same data set
                        DownloadDataFile(testDataUri, testDataFile);
                        TestData = Context.Data.LoadFromTextFile(testDataFile.FullName,
                            columns: new[]
                            {
                                new TextLoader.Column("PixelValues", DataKind.Single, 0, 63), // 64 single values
                                new TextLoader.Column("Label", DataKind.Single, 64) // 1 single value
                            },
                            hasHeader: false,
                            separatorChar: ','
                            );
                    }
                    {
                        ModelFile = DataDir.GetChildFileInfo("MNIST-Model.zip");
                    }

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
            case MLNetExampleScenario.C777_Recommendation_Auto_MovieRecommender:
                {
                    {
                        var movieDataFile = DataDir.GetChildFileInfo("Movie.csv");
                        var movieDataUri = new Uri("https://raw.githubusercontent.com/dotnet/machinelearning-samples/main/samples/csharp/getting-started/MatrixFactorization_MovieRecommendation/Data/recommendation-movies.csv");
                        DownloadDataFile(movieDataUri, movieDataFile);
                    }
                    {
                        var trainDataFile = DataDir.GetChildFileInfo("MovieRate-Train.csv");
                        var trainDataUri = new Uri("https://raw.githubusercontent.com/dotnet/machinelearning-samples/main/samples/csharp/getting-started/MatrixFactorization_MovieRecommendation/Data/recommendation-ratings-train.csv");
                        DownloadDataFile(trainDataUri, trainDataFile);
                        TrainData = Context.Data.LoadFromTextFile<C002_MovieRateInput>(trainDataFile.FullName, hasHeader: true, separatorChar: ',');
                    }
                    {
                        var testDataFile = DataDir.GetChildFileInfo("MovieRate-Test.csv");
                        var testDataUri = new Uri("https://raw.githubusercontent.com/dotnet/machinelearning-samples/main/samples/csharp/getting-started/MatrixFactorization_MovieRecommendation/Data/recommendation-ratings-test.csv");
                        DownloadDataFile(testDataUri, testDataFile);
                        TestData = Context.Data.LoadFromTextFile<C002_MovieRateInput>(testDataFile.FullName, hasHeader: true, separatorChar: ',');
                    }
                    {
                        ModelFile = DataDir.GetChildFileInfo("MovieRate-Model.zip");
                    }

                    break;
                }

            case MLNetExampleScenario.D001_Regression_TaxiFarePrediction:
            case MLNetExampleScenario.D777_Regression_Auto_TaxiFarePrediction:
                {
                    var allDataFile = DataDir.GetChildFileInfo("TaxiFare.csv");
                    ModelFile = DataDir.GetChildFileInfo("TaxiFare-Model.zip");

                    var uri = new Uri("https://github.com/dotnet/machinelearning-samples/blob/main/samples/csharp/getting-started/Regression_TaxiFarePrediction/TaxiFarePrediction/Data/taxi-fare-full.csv?raw=true");
                    DownloadDataFile(uri, allDataFile);

                    AllData = Context.Data.LoadFromTextFile<D001_TaxiFareInput>(allDataFile.FullName, hasHeader: true, separatorChar: ',');
                    SplitData();
                    TrainData = Context.Data.FilterRowsByColumn(TrainData, "Label", lowerBound: 1, upperBound: 150);

                    break;
                }

            case MLNetExampleScenario.F001_AnomalyDetection_SalesSpikeDetection_DetectIidSpike:
            case MLNetExampleScenario.F001_AnomalyDetection_SalesSpikeDetection_DetectIidChangePoint:
                {
                    var allDataFile = DataDir.GetChildFileInfo("ProductSales.csv");
                    ModelFile = DataDir.GetChildFileInfo("ProductSales-Model.zip");

                    var uri = new Uri("https://github.com/dotnet/machinelearning-samples/raw/main/samples/csharp/getting-started/AnomalyDetection_Sales/SpikeDetection/Data/product-sales.csv");
                    DownloadDataFile(uri, allDataFile);

                    AllData = Context.Data.LoadFromTextFile<F001_ProductSalesInput>(allDataFile.FullName, hasHeader: true, separatorChar: ',');
                    TestData = AllData;
                    TrainData = AllData;

                    break;
                }

            case MLNetExampleScenario.F002_AnomalyDetection_PowerAnomalyDetection:
                {
                    var allDataFile = DataDir.GetChildFileInfo("PowerMeter.csv");
                    ModelFile = DataDir.GetChildFileInfo("PowerMeter-Model.zip");

                    var uri = new Uri("https://github.com/dotnet/machinelearning-samples/raw/main/samples/csharp/getting-started/AnomalyDetection_PowerMeterReadings/PowerAnomalyDetection/Data/power-export_min.csv");
                    DownloadDataFile(uri, allDataFile);

                    AllData = Context.Data.LoadFromTextFile<F002_PowerMeterInput>(allDataFile.FullName, hasHeader: true, separatorChar: ',');
                    TestData = AllData;
                    TrainData = AllData;

                    break;
                }

            case MLNetExampleScenario.H001_Ranking_RankSearchEngineResults:
                {
                    {
                        var trainDataFile = DataDir.GetChildFileInfo("SearchResult-Train.tsv");
                        var trainDataUri = new Uri("https://aka.ms/mlnet-resources/benchmarks/MSLRWeb10KTrain720kRows.tsv");
                        DownloadDataFile(trainDataUri, trainDataFile, true);
                        TrainData = Context.Data.LoadFromTextFile<H001_SearchResultInput>(trainDataFile.FullName, hasHeader: true);
                    }
                    {
                        var trainDataFile = DataDir.GetChildFileInfo("SearchResult-Train2.tsv");
                        var trainDataUri = new Uri("https://aka.ms/mlnet-resources/benchmarks/MSLRWeb10KValidate240kRows.tsv");
                        DownloadDataFile(trainDataUri, trainDataFile, true);
                        var additionalTrainData = Context.Data.LoadFromTextFile<H001_SearchResultInput>(trainDataFile.FullName, hasHeader: false);
                        TrainData = CombineData<H001_SearchResultInput>(TrainData, additionalTrainData);
                    }
                    {
                        var testDataFile = DataDir.GetChildFileInfo("SearchResult-Test.tsv");
                        var testDataUri = new Uri("https://aka.ms/mlnet-resources/benchmarks/MSLRWeb10KTest240kRows.tsv");
                        DownloadDataFile(testDataUri, testDataFile, true);
                        TestData = Context.Data.LoadFromTextFile<H001_SearchResultInput>(testDataFile.FullName, hasHeader: false);
                    }
                    {
                        ModelFile = DataDir.GetChildFileInfo("SearchResult-Model.zip");
                    }

                    break;
                }

            case MLNetExampleScenario.H777_Ranking_Auto_RankSearchEngineResults:
                {
                    var textLoaderOptions = new TextLoader.Options
                    {
                        Separators = new[] { '\t' },
                        HasHeader = true,
                        Columns = new[]
                        {
                            new TextLoader.Column("Label", DataKind.Single, 0),
                            new TextLoader.Column("GroupId", DataKind.Int32, 1),
                            new TextLoader.Column("Features", DataKind.Single, 2, 133),
                        }
                    };
                    var textLoader = Context.Data.CreateTextLoader(textLoaderOptions);

                    {
                        var trainDataFile = DataDir.GetChildFileInfo("SearchResult-Train.tsv");
                        var trainDataUri = new Uri("https://aka.ms/mlnet-resources/benchmarks/MSLRWeb10KTrain720kRows.tsv");
                        DownloadDataFile(trainDataUri, trainDataFile, true);
                        TrainData = textLoader.Load(trainDataFile.FullName);
                    }
                    {
                        var trainDataFile = DataDir.GetChildFileInfo("SearchResult-Train2.tsv");
                        var trainDataUri = new Uri("https://aka.ms/mlnet-resources/benchmarks/MSLRWeb10KValidate240kRows.tsv");
                        DownloadDataFile(trainDataUri, trainDataFile, true);
                        var additionalTrainData = textLoader.Load(trainDataFile.FullName);
                        TrainData = CombineData<H777_SearchResultInput>(TrainData, additionalTrainData);
                    }
                    {
                        var testDataFile = DataDir.GetChildFileInfo("SearchResult-Test.tsv");
                        var testDataUri = new Uri("https://aka.ms/mlnet-resources/benchmarks/MSLRWeb10KTest240kRows.tsv");
                        DownloadDataFile(testDataUri, testDataFile, true);
                        TestData = textLoader.Load(testDataFile.FullName);
                    }
                    {
                        ModelFile = DataDir.GetChildFileInfo("SearchResult-Model.zip");
                    }

                    break;
                }

            case MLNetExampleScenario.I001_ComputerVision_ImageClassificationTraining_HighLevelAPI:
                {
                    {
                        var dataDir = DataDir.GetChildDirectoryInfo("Images");
                        var targetFileLocationAfterExtracted = dataDir.GetChildDirectoryInfo("flower_photos_small_set");
                        var dataUri = new Uri("https://github.com/dotnet/machinelearning-samples/blob/main/samples/csharp/getting-started/DeepLearning_ImageClassification_Training/ImageClassification.Train/assets/inputs/images/flower_photos_small_set.zip?raw=true");
                        DownloadAndExtractZipDataFile(dataUri, dataDir, targetFileLocationAfterExtracted);

                        //SINGLE FULL FLOWERS IMAGESET (3,600 files)
                        //string url = $"http://download.tensorflow.org/example_images/flower_photos.tgz";
                        //Compress.ExtractTGZ(Path.Join(imagesDownloadFolder, fileName), imagesDownloadFolder);


                        var images = I001_ImageInput.LoadInMemoryImagesFromDirectory(dataDir, useFolderNameAsLabel: true);
                        AllData = Context.Data.LoadFromEnumerable(images);
                        AllData = Context.Data.ShuffleRows(AllData);
                        AllData = Context.Transforms.Conversion.MapValueToKey("LabelAsKey", "Label", keyOrdinality: KeyOrdinality.ByValue)
                            .Fit(AllData)
                            .Transform(AllData);

                        //AllData =
                        //    Context.Transforms.Conversion.MapValueToKey("LabelAsKey", "Label", keyOrdinality: KeyOrdinality.ByValue)
                        //    .Append(Context.Transforms.LoadRawImageBytes("Image", dataDir.FullName, "ImageFileName"))
                        //    .Fit(AllData)
                        //    .Transform(AllData);

                        SplitData();
                    }
                    {
                        var dataDir = DataDir.GetChildDirectoryInfo("Images-Test").CreateAndPipe();

                        {
                            //var testDataFile = testDataDir.GetChildFileInfo("test1.png");
                            var dataFile = dataDir.GetChildFileInfo("RareThreeSpiraledRose.png");
                            var dataUri = new Uri("https://github.com/dotnet/machinelearning-samples/raw/main/samples/csharp/getting-started/DeepLearning_ImageClassification_Training/ImageClassification.Train/assets/inputs/test-images/RareThreeSpiralledRose.png");
                            DownloadDataFile(dataUri, dataFile);
                        }
                        {
                            //var testDataFile = testDataDir.GetChildFileInfo("test2.png");
                            var dataFile = dataDir.GetChildFileInfo("StrangeBlackRose.png");
                            var dataUri = new Uri("https://github.com/dotnet/machinelearning-samples/raw/main/samples/csharp/getting-started/DeepLearning_ImageClassification_Training/ImageClassification.Train/assets/inputs/test-images/StrangeBlackRose.png");
                            DownloadDataFile(dataUri, dataFile);
                        }
                    }
                    {
                        ModelFile = DataDir.GetChildFileInfo("Spam-Model.zip");
                    }

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

            // not going to implement
            case MLNetExampleScenario.B001_MultiClassClassification_IssuesClassification:
                {
                    throw new NotImplementedException();
                }
        }

        Console.WriteLine("done");
    }
}