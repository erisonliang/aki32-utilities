﻿using System.Drawing;

using Aki32Utilities.ConsoleAppUtilities.General;

using Microsoft.ML;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public partial class MLNetExampleSummary : MLNetHandler
{
    private void PredictSamples()
    {
        General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ Single Prediction", ConsoleColor.Yellow);

        switch (Scenario)
        {
            case MLNetExampleScenario.A001_Sentiment_Analysis:
                {
                    var predictor = Context.Model.CreatePredictionEngine<A001_SentimentInput, A001_SentimentOutput>(Model);

                    var samples = new A001_SentimentInput[]
                    {
                        new A001_SentimentInput { Text = "I love this movie!" },
                        new A001_SentimentInput { Text = "I hate this movie! yuck!!" },
                    };

                    foreach (var sample in samples)
                    {
                        var result = predictor.Predict(sample);

                        Console.WriteLine(@$"=======================================================");
                        Console.WriteLine(@$"Text: {sample.Text}");
                        Console.WriteLine(@$"Prediction: {(Convert.ToBoolean(result.Prediction) ? "Toxic" : "Non Toxic")}");
                        Console.WriteLine(@$"Probability of being toxic: {result.Probability}");
                        Console.WriteLine();
                    }
                    Console.WriteLine(@$"=======================================================");

                    break;
                }

            case MLNetExampleScenario.A002_Spam_Detection:
                {
                    var predictor = Context.Model.CreatePredictionEngine<A002_SpamInput, A002_SpamOutput>(Model);

                    var samples = new A002_SpamInput[]
                    {
                        new A002_SpamInput { Message = "That's a great idea. It should work."},
                        new A002_SpamInput { Message = "free medicine winner! congratulations"},
                        new A002_SpamInput { Message = "Yes we should meet over the weekend!"},
                        new A002_SpamInput { Message = "you win pills and free entry vouchers" },
                    };

                    foreach (var sample in samples)
                    {
                        var result = predictor.Predict(sample);

                        Console.WriteLine(@$"=======================================================");
                        Console.WriteLine(@$"Message: {sample.Message}");
                        Console.WriteLine(@$"Result: {result.PredictedLabel}");
                        Console.WriteLine();
                    }
                    Console.WriteLine(@$"=======================================================");

                    break;
                }

            case MLNetExampleScenario.A004_HeartDiseasePrediction:
                {
                    var predictor = Context.Model.CreatePredictionEngine<A004_HeartInput, A004_HeartOutput>(Model);

                    var samples = A004_HeartSample.heartDataList;

                    foreach (var sample in samples)
                    {
                        var result = predictor.Predict(sample);

                        Console.WriteLine(@$"=======================================================");
                        Console.WriteLine(@$"Age: {sample.Age} ");
                        Console.WriteLine(@$"Sex: {sample.Sex} ");
                        Console.WriteLine(@$"Cp: {sample.Cp} ");
                        Console.WriteLine(@$"TrestBps: {sample.TrestBps} ");
                        Console.WriteLine(@$"Chol: {sample.Chol} ");
                        Console.WriteLine(@$"Fbs: {sample.Fbs} ");
                        Console.WriteLine(@$"RestEcg: {sample.RestEcg} ");
                        Console.WriteLine(@$"Thalac: {sample.Thalac} ");
                        Console.WriteLine(@$"Exang: {sample.Exang} ");
                        Console.WriteLine(@$"OldPeak: {sample.OldPeak} ");
                        Console.WriteLine(@$"Slope: {sample.Slope} ");
                        Console.WriteLine(@$"Ca: {sample.Ca} ");
                        Console.WriteLine(@$"Thal: {sample.Thal} ");
                        Console.WriteLine();
                        Console.WriteLine(@$"Answer: ? "); // sample.Label には未入力。
                        Console.WriteLine(@$"Prediction: {result.PredictedLabel}, {(result.PredictedLabel ? "A disease could be present" : "Not present disease")} ");
                        Console.WriteLine(@$"Probability: {result.Probability} ");
                        Console.WriteLine();
                    }
                    Console.WriteLine(@$"=======================================================");

                    break;
                }

            case MLNetExampleScenario.B002_IrisFlowersClassification:
                {
                    var predictor = Context.Model.CreatePredictionEngine<B002_IrisInput, B002_IrisOutput>(Model);

                    var samples = new B002_IrisInput[]
                    {
                       B002_IrisSampe.Iris1,
                       B002_IrisSampe.Iris2,
                       B002_IrisSampe.Iris3,
                    };

                    Dictionary<float, string> IrisFlowers = new()
                    {
                        { 0, "setosa" },
                        { 1, "versicolor" },
                        { 2, "virginica" }
                    };

                    foreach (var sample in samples)
                    {
                        var result = predictor.Predict(sample);

                        Console.WriteLine(@$"=======================================================");
                        Console.WriteLine($"Answer: {IrisFlowers[(int)sample.Label]}");
                        Console.WriteLine($"Prediction: {IrisFlowers[(int)result.PredictedLabel]}");
                        Console.WriteLine($"Probabilities:");
                        for (int i = 0; i < result.Score.Length; i++)
                            Console.WriteLine($"  {IrisFlowers[i],10}: {result.Score[i],-10:F4}");

                        Console.WriteLine();
                    }
                    Console.WriteLine(@$"=======================================================");

                    break;
                }

            case MLNetExampleScenario.B003_MNIST:
                {
                    var predictor = Context.Model.CreatePredictionEngine<B003_MnistInput, B003_MnistOutput>(Model);

                    var samples = new B003_MnistInput[]
                    {
                        B003_MnistSmaple.MNIST1,
                        B003_MnistSmaple.MNIST2,
                        B003_MnistSmaple.MNIST3,
                    };

                    foreach (var sample in samples)
                    {
                        var result = predictor.Predict(sample);

                        Console.WriteLine(@$"=======================================================");
                        Console.WriteLine(@$"Answer: {sample.Number}");
                        Console.WriteLine(@$"Prediction: {result.PredictedLabel}");
                        for (int i = 0; i < result.Score.Length; i++)
                            Console.WriteLine(@$"  {i}: {result.Score[i]:F4}");

                        Console.WriteLine();
                    }
                    Console.WriteLine(@$"=======================================================");

                    break;
                }

            case MLNetExampleScenario.C001_ProductRecommendation:
                {
                    var predictor = Context.Model.CreatePredictionEngine<C001_ProductInput, C001_ProductOutput>(Model);

                    var samples = new C001_ProductInput[]
                    {
                        new C001_ProductInput()
                        {
                            ProductID = 3,
                            CoPurchaseProductID = 57
                        },
                        new C001_ProductInput()
                        {
                            ProductID = 3,
                            CoPurchaseProductID = 58
                        },
                        new C001_ProductInput()
                        {
                            ProductID = 3,
                            CoPurchaseProductID = 59
                        },
                        new C001_ProductInput()
                        {
                            ProductID = 3,
                            CoPurchaseProductID = 60
                        },
                        new C001_ProductInput()
                        {
                            ProductID = 3,
                            CoPurchaseProductID = 61
                        },
                        new C001_ProductInput()
                        {
                            ProductID = 3,
                            CoPurchaseProductID = 62
                        },
                        new C001_ProductInput()
                        {
                            ProductID = 3,
                            CoPurchaseProductID = 63
                        },
                    };

                    foreach (var sample in samples)
                    {
                        var result = predictor.Predict(sample);

                        Console.WriteLine(@$"=======================================================");
                        Console.WriteLine(@$"For ProductID = {sample.ProductID} and CoPurchaseProductID = {sample.CoPurchaseProductID}");
                        Console.WriteLine(@$"The predicted score is {result.Score * 100:F0}%");

                        Console.WriteLine();
                    }
                    Console.WriteLine(@$"=======================================================");

                    break;
                }

            case MLNetExampleScenario.C002_MovieRecommender_MatrixFactorization:
                {
                    var predictor = Context.Model.CreatePredictionEngine<C002_MovieRateInput, C002_MovieRateOutput>(Model);

                    var samples = new C002_MovieRateInput[]
                    {
                       new C002_MovieRateInput()
                       {
                           userId = 6,
                           movieId = 10
                       },
                    };

                    var movieDataFile = DataDir.GetChildFileInfo("Movie.csv");
                    var movieData = movieDataFile.ReadObjectFromLocalCsv<C002_Movie>();

                    foreach (var sample in samples)
                    {
                        var result = predictor.Predict(sample);

                        var targetMovie = movieData.FirstOrDefault(m => m.movieId == sample.movieId);

                        Console.WriteLine(@$"=======================================================");
                        Console.WriteLine(@$"For userId:{sample.userId}, movie rating prediction (1 - 5 stars) for movie:{targetMovie?.movieTitle} is: {Math.Round(result.Score, 1)}");

                        Console.WriteLine();
                    }
                    Console.WriteLine(@$"=======================================================");

                    break;
                }

            case MLNetExampleScenario.I004_ObjectDetection_ONNXModelScoring_TinyYoloV2_08:
            case MLNetExampleScenario.I004_ObjectDetection_ONNXModelScoring_YoloV2_09:
            case MLNetExampleScenario.I004_ObjectDetection_ONNXModelScoring_YoloV3_10:
                {
                    var predictor = Context.Model.CreatePredictionEngine<I004_YoloInput, I004_YoloOutput>(Model);
                    var imageDir = DataDir.GetChildDirectoryInfo("Images");
                    var outputImageDir = imageDir.GetChildDirectoryInfo("Output");
                    var parser = new I004_YoloOutput.Parser();

                    switch (Scenario)
                    {
                        // for tinyyolov2-8.onnx
                        case MLNetExampleScenario.I004_ObjectDetection_ONNXModelScoring_TinyYoloV2_08:
                            {
                                parser.RowCount = 13;
                                parser.ColCount = 13;
                                parser.ChannelCount = 125;
                                parser.ClassCount = 20;

                                parser.Anchors = new float[] { 1.08F, 1.19F, 3.42F, 4.41F, 6.63F, 11.38F, 9.42F, 5.11F, 16.62F, 10.52F };
                                parser.Labels = new string[] { "aeroplane", "bicycle", "bird", "boat", "bottle", "bus", "car", "cat", "chair", "cow", "diningtable", "dog", "horse", "motorbike", "person", "pottedplant", "sheep", "sofa", "train", "tvmonitor" };
                                parser.ClassColors = new Color[]
                                {
                                    Color.Khaki,
                                    Color.Fuchsia,
                                    Color.Silver,
                                    Color.RoyalBlue,
                                    Color.Green,
                                    Color.DarkOrange,
                                    Color.Purple,
                                    Color.Gold,
                                    Color.Red,
                                    Color.Aquamarine,
                                    Color.Lime,
                                    Color.AliceBlue,
                                    Color.Sienna,
                                    Color.Orchid,
                                    Color.Tan,
                                    Color.LightPink,
                                    Color.Yellow,
                                    Color.HotPink,
                                    Color.OliveDrab,
                                    Color.SandyBrown,
                                    Color.DarkTurquoise
                                };

                            }
                            break;
                        // for yolov2-coco-9.onnx
                        case MLNetExampleScenario.I004_ObjectDetection_ONNXModelScoring_YoloV2_09:
                            {
                                parser.RowCount = 13;
                                parser.ColCount = 13;
                                parser.ChannelCount = 425;
                                parser.ClassCount = 80;

                                parser.Anchors = new float[] { 1.08F, 1.19F, 3.42F, 4.41F, 6.63F, 11.38F, 9.42F, 5.11F, 16.62F, 10.52F };
                                parser.Labels = new string[] { "person", "bicycle", "car", "motorcycle", "airplane", "bus", "train", "truck", "boat", "traffic light", "fire hydrant", "stop sign", "parking meter", "bench", "bird", "cat", "dog", "horse", "sheep", "cow", "elephant", "bear", "zebra", "giraffe", "backpack", "umbrella", "handbag", "tie", "suitcase", "frisbee", "skis", "snowboard", "sports ball", "kite", "baseball bat", "baseball glove", "skateboard", "surfboard", "tennis racket", "bottle", "wine glass", "cup", "fork", "knife", "spoon", "bowl", "banana", "apple", "sandwich", "orange", "broccoli", "carrot", "hot dog", "pizza", "donut", "cake", "chair", "couch", "potted plant", "bed", "dining table", "toilet", "tv", "laptop", "mouse", "remote", "keyboard", "cell phone", "microwave", "oven", "toaster", "sink", "refrigerator", "book", "clock", "vase", "scissors", "teddy bear", "hair drier", "toothbrush" };
                                parser.ClassColors = Enumerable.Range(0, parser.ClassCount).Select(x => Color.Red).ToArray();
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

                    var samples = imageDir
                        .GetFilesWithRegexen(General.ChainableExtensions.GetRegexen_ImageFiles())
                        .Select(f => new I004_YoloInput { ImagePath = f.FullName, FileName = f.Name });

                    foreach (var sample in samples)
                    {
                        var result = predictor.Predict(sample);

                        var objectBoxes = parser.ParseOutputs(result.PredictedLabels);
                        objectBoxes = parser.FilterBoundingBoxes(objectBoxes, 10, .6F);

                        // output as image
                        I004_YoloBoundingBox.DrawBoundingBoxToImage(imageDir, outputImageDir, sample.FileName, objectBoxes);

                        // output to console
                        Console.WriteLine(@$"=======================================================");
                        Console.WriteLine(@$"(!) Objects found in {sample.FileName}");
                        foreach (var box in objectBoxes)
                            Console.WriteLine($"{box.Confidence * 100:F0}%\t{box.Label}");

                        Console.WriteLine();
                    }
                    Console.WriteLine(@$"=======================================================");

                    break;
                }

            // ignore
            case MLNetExampleScenario.A003_CreditCardFraudDetection:
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
    }

}











