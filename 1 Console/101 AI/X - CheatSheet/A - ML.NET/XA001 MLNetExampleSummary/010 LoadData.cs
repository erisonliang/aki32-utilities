﻿using Aki32Utilities.ConsoleAppUtilities.General;

using iTextSharp.text.pdf.codec.wmf;

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

        void SplitData()
        {
            var split = Context.Data.TrainTestSplit(AllData, testFraction: 0.2);
            TrainData = split.TrainSet;
            TestData = split.TestSet;
        }

        switch (Scenario)
        {
            case MLNetExampleScenario.A001_Sentiment_Analysis:
                {
                    var allDataFile = DataDir.GetChildFileInfo("Sentiment.tsv");
                    ModelFile = DataDir.GetChildFileInfo("Sentiment-Model.zip");

                    var uri = new Uri("https://github.com/dotnet/machinelearning-samples/blob/main/samples/csharp/getting-started/BinaryClassification_SentimentAnalysis/SentimentAnalysis/Data/wikiDetoxAnnotated40kRows.tsv?raw=true");
                    DownloadDataFile(uri, allDataFile);

                    AllData = Context.Data.LoadFromTextFile<A001_SentimentInput>(allDataFile.FullName, hasHeader: true, separatorChar: '\t');
                    SplitData();

                    break;
                }

            case MLNetExampleScenario.A002_Spam_Detection:
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

            case MLNetExampleScenario.A003_CreditCardFraudDetection:
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

            case MLNetExampleScenario.A004_HeartDiseasePrediction:
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

            case MLNetExampleScenario.B002_IrisFlowersClassification:
                {
                    var allDataFile = DataDir.GetChildFileInfo("Iris.txt");
                    ModelFile = DataDir.GetChildFileInfo("Iris-Model.zip");

                    var uri = new Uri("https://raw.githubusercontent.com/dotnet/machinelearning-samples/main/samples/csharp/getting-started/MulticlassClassification_Iris/IrisClassification/Data/iris-full.txt");
                    DownloadDataFile(uri, allDataFile);

                    AllData = Context.Data.LoadFromTextFile<B002_IrisInput>(allDataFile.FullName, hasHeader: true, separatorChar: '\t');
                    SplitData();

                    break;
                }

            // ★ many features, the same type ((!)Trying 2 types of reading way)
            case MLNetExampleScenario.B003_MNIST:
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

            // not implemented
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

            // not implementing
            case MLNetExampleScenario.B001_IssuesClassification:
                {
                    throw new NotImplementedException();
                }
        }
    }
}