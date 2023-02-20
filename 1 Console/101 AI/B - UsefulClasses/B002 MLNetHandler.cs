using Aki32Utilities.ConsoleAppUtilities.General;

using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Data;

using OpenCvSharp.ML;

namespace Aki32Utilities.ConsoleAppUtilities.AI;
public class MLNetHandler
{

    // ★★★★★★★★★★★★★★★ props

    public ITransformer Model { get; set; }
    public DataViewSchema ModelInputSchema { get; set; }

    public IEstimator<ITransformer> PipeLineHead { get; set; } = new EstimatorChain<ITransformer>();
    public MLContext Context { get; set; }

    public DirectoryInfo DataDir { get; set; }

    public IDataView AllData { get; set; }
    public IDataView TestData { get; set; }
    public IDataView TrainData { get; set; }

    public static uint ExperimentTime_InSeconds = 60;
    public static uint Ranking_OptimizationMetricTruncationLevel = 10;


    // ★★★★★★★★★★★★★★★ methods

    public void ConnectNode<TTrans>(IEstimator<TTrans> estimator, TransformerScope scope = TransformerScope.Everything) where TTrans : class, ITransformer
        => PipeLineHead = PipeLineHead.Append(estimator, scope);

    public void ConnectCheckPoint()
        => PipeLineHead.AppendCacheCheckpoint(Context);

    public void DownloadDataFile(Uri uri, FileInfo targetDataFile, bool takesTimeFlag = false)
    {
        if (!targetDataFile.Exists)
        {
            if (takesTimeFlag)
                General.ConsoleExtension.WriteLineWithColor("This may take a few minutes.", ConsoleColor.Yellow);
            uri.DownloadFile(targetDataFile);
        }
    }

    public void DownloadAndExtractZipDataFile(Uri uri, FileInfo targetDataFile, FileInfo? targetFileLocationAfterExtracted = null)
    {
        if (!targetDataFile.Exists)
        {
            var zipDir = DataDir.GetChildDirectoryInfo("zip").CreateAndPipe();
            uri.DownloadFile(zipDir.GetChildFileInfo("downloaded.zip")).Decompress_Zip(zipDir);
            targetFileLocationAfterExtracted?.MoveTo(targetDataFile);
        }
    }
    public void DownloadAndExtractZipDataFile(Uri uri, DirectoryInfo targetDataDir, DirectoryInfo? targetDirLocationAfterExtracted = null)
    {
        if (!targetDataDir.Exists)
        {
            targetDataDir.Create();
            uri.DownloadFile(targetDataDir.GetChildFileInfo("downloaded.zip")).Decompress_Zip(targetDataDir);
            targetDirLocationAfterExtracted?.MoveTo(targetDataDir, false);
        }
    }

    public (IDataView trainSet, IDataView testSet) SplitData(IDataView splittingData, double testFraction = 0.2)
    {
        var split = Context.Data.TrainTestSplit(splittingData, testFraction);
        return (split.TrainSet, split.TestSet);
    }

    public void SplitData(double testFraction = 0.3)
    {
        (TrainData, TestData) = SplitData(AllData, testFraction);
    }

    public IDataView CombineData<T>(params IDataView[] dataViews) where T : class, new()
    {
        var combinedDataEnum = Enumerable.Empty<T>();

        foreach (var dataView in dataViews)
        {
            var dataViewEnum = Context.Data.CreateEnumerable<T>(dataView, false);
            combinedDataEnum = combinedDataEnum.Concat(dataViewEnum);
        }

        var combinedData = Context.Data.LoadFromEnumerable(combinedDataEnum);
        return combinedData;
    }


    // ★★★★★★★★★★★★★★★ classes

    /// <summary>
    /// Progress handler that AutoML will invoke after each model it produces and evaluates.
    /// </summary>
    public class ExperimentHandler : IProgress<RunDetail>
    {
        private int _iterationIndex;

        public void Report(RunDetail runDetail)
        {
            if (_iterationIndex++ == 0)
                ConsoleExtension.PrintMetricsInOneLineHeader(runDetail);

            dynamic dRunDetail = runDetail;

            if (dRunDetail.Exception != null)
            {
                General.ConsoleExtension.WriteLineWithColor($"Exception during AutoML iteration:\r\n{dRunDetail.Exception}", ConsoleColor.Red);
            }
            else
            {
                //General.ConsoleExtension.ClearCurrentConsoleLine();
                ConsoleExtension.PrintMetricsInOneLine(_iterationIndex, runDetail);
            }
        }

        public void PrintTopModels(ExperimentResult<BinaryClassificationMetrics> result)
        {
            var topRuns = result.RunDetails
                    .Where(r => r.ValidationMetrics != null && !double.IsNaN(r.ValidationMetrics.Accuracy))
                    .OrderByDescending(r => r.ValidationMetrics.Accuracy)
                    .Take(3);

            Console.WriteLine(@$"=======================================================");
            Console.WriteLine("Top models ranked by accuracy");
            for (var i = 0; i < topRuns.Count(); i++)
                ConsoleExtension.PrintMetricsInOneLine(i + 1, topRuns.ElementAt(i));

            Console.WriteLine(@$"=======================================================");
        }

        public void PrintTopModels(ExperimentResult<MulticlassClassificationMetrics> result)
        {
            var topRuns = result.RunDetails
                    .Where(r => r.ValidationMetrics != null && !double.IsNaN(r.ValidationMetrics.MicroAccuracy))
                    .OrderByDescending(r => r.ValidationMetrics.MicroAccuracy)
                    .Take(3);

            Console.WriteLine(@$"=======================================================");
            Console.WriteLine("Top models ranked by accuracy");
            for (var i = 0; i < topRuns.Count(); i++)
                ConsoleExtension.PrintMetricsInOneLine(i + 1, topRuns.ElementAt(i));

            Console.WriteLine(@$"=======================================================");
        }

        public void PrintTopModels(ExperimentResult<RegressionMetrics> result)
        {
            var topRuns = result.RunDetails
                    .Where(r => r.ValidationMetrics != null && !double.IsNaN(r.ValidationMetrics.RSquared))
                    .OrderByDescending(r => r.ValidationMetrics.RSquared)
                    .Take(3);

            Console.WriteLine(@$"=======================================================");
            Console.WriteLine("Top models ranked by R-Squared");
            for (var i = 0; i < topRuns.Count(); i++)
                ConsoleExtension.PrintMetricsInOneLine(i + 1, topRuns.ElementAt(i));

            Console.WriteLine(@$"=======================================================");
        }

        public void PrintTopModels(ExperimentResult<RankingMetrics> result, int optimizationMetricTruncationLevel)
        {
            var topRuns = result.RunDetails
                    .Where(r => r.ValidationMetrics != null && !double.IsNaN(r.ValidationMetrics.NormalizedDiscountedCumulativeGains[optimizationMetricTruncationLevel - 1]))
                    .OrderByDescending(r => r.ValidationMetrics.NormalizedDiscountedCumulativeGains[(int)optimizationMetricTruncationLevel - 1])
                    .Take(5);

            Console.WriteLine(@$"=======================================================");
            Console.WriteLine($"Top models ordered by NDCG@{optimizationMetricTruncationLevel}");
            for (var i = 0; i < topRuns.Count(); i++)
                ConsoleExtension.PrintMetricsInOneLine(i + 1, topRuns.ElementAt(i));

            Console.WriteLine(@$"=======================================================");
        }

    }


    // ★★★★★★★★★★★★★★★ 

}
