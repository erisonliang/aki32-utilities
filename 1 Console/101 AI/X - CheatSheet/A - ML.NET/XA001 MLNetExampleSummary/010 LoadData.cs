using Aki32Utilities.ConsoleAppUtilities.General;

using Microsoft.ML;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public partial class MLNetExampleSummary : MLNetHandler
{
    private void LoadData()
    {
        General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ Load Data", ConsoleColor.Yellow);

        void DownloadDataFile(Uri uri)
        {
            if (!DataFile.Exists)
                uri.DownloadFile(DataFile);
        }

        void DownloadAndExtractZipDataFile(Uri uri, FileInfo targetFileLocationAfterExtracted)
        {
            if (!DataFile.Exists)
            {
                var zipDir = DataDir.GetChildDirectoryInfo("zip").CreateAndPipe();
                uri.DownloadFile(zipDir.GetChildFileInfo("downloaded.zip")).Decompress_Zip(zipDir);
                targetFileLocationAfterExtracted.MoveTo(DataFile);
            }
        }

        void SplitData()
        {
            var split = Context.Data.TrainTestSplit(AllData, testFraction: 0.2);
            TrainData = split.TrainSet;
            TestData = split.TestSet;
        }

        // ★ from class array

        // ★ many features, the same type
        //var data = Context.Data.CreateTextLoader(
        //           columns: new[]
        //           {
        //                    new TextLoader.Column("FeatureList", DataKind.Single, 0, 63), // 64 single values
        //                    new TextLoader.Column("Number", DataKind.Single, 64) // 1 single value
        //           },
        //           hasHeader: false,
        //           separatorChar: ','
        //           ).Load(DataFile.FullName);

        switch (Scenario)
        {
            case MLNetExampleScenario.A001_Sentiment_Analysis:
                {
                    DataFile = DataDir.GetChildFileInfo("Sentiment.tsv");
                    ModelFile = DataDir.GetChildFileInfo("Sentiment-Model.zip");

                    var uri = new Uri("https://github.com/dotnet/machinelearning-samples/blob/main/samples/csharp/getting-started/BinaryClassification_SentimentAnalysis/SentimentAnalysis/Data/wikiDetoxAnnotated40kRows.tsv?raw=true");
                    DownloadDataFile(uri);

                    AllData = Context.Data.LoadFromTextFile<A001_SentimentInput>(DataFile.FullName, hasHeader: true, separatorChar: '\t');
                    SplitData();

                    break;
                }

            case MLNetExampleScenario.A002_Spam_Detection:
                {
                    DataFile = DataDir.GetChildFileInfo("Spam.tsv");
                    ModelFile = DataDir.GetChildFileInfo("Spam-Model.zip");
                    var targetFileLocationAfterExtracted = DataDir.GetChildDirectoryInfo("zip").GetChildFileInfo("SMSSpamCollection");

                    var uri = new Uri("https://archive.ics.uci.edu/ml/machine-learning-databases/00228/smsspamcollection.zip");
                    DownloadAndExtractZipDataFile(uri, targetFileLocationAfterExtracted);

                    AllData = Context.Data.LoadFromTextFile<A002_SpamInput>(DataFile.FullName, hasHeader: false, separatorChar: '\t');
                    SplitData();

                    break;
                }

            case MLNetExampleScenario.B002_IrisFlowersClassification:
                {
                    DataFile = DataDir.GetChildFileInfo("Iris.txt");
                    ModelFile = DataDir.GetChildFileInfo("Iris-Model.zip");

                    var uri = new Uri("https://raw.githubusercontent.com/dotnet/machinelearning-samples/main/samples/csharp/getting-started/MulticlassClassification_Iris/IrisClassification/Data/iris-full.txt");
                    DownloadDataFile(uri);

                    AllData = Context.Data.LoadFromTextFile<B002_IrisInput>(DataFile.FullName, hasHeader: true, separatorChar: '\t');
                    SplitData();

                    break;
                }

            // not implemented
            case MLNetExampleScenario.A003_CreditCardFraudDetection:
            case MLNetExampleScenario.A004_HeartDiseasePrediction:
            case MLNetExampleScenario.A777_Auto:
            case MLNetExampleScenario.B001_IssuesClassification:
            case MLNetExampleScenario.B003_MNIST:
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