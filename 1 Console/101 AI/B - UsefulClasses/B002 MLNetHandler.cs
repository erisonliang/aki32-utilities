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

    public uint ExperimentTime_InSeconds = 60;


    // ★★★★★★★★★★★★★★★ methods

    public void ConnectNode<TTrans>(IEstimator<TTrans> estimator, TransformerScope scope = TransformerScope.Everything) where TTrans : class, ITransformer
        => PipeLineHead = PipeLineHead.Append(estimator, scope);

    public void ConnectCheckPoint()
        => PipeLineHead.AppendCacheCheckpoint(Context);

    public void DownloadDataFile(Uri uri, FileInfo targetDataFile)
    {
        if (!targetDataFile.Exists)
            uri.DownloadFile(targetDataFile);
    }

    public void DownloadAndExtractZipDataFile(Uri uri, FileInfo targetFileLocationAfterExtracted, FileInfo targetDataFile)
    {
        if (!targetDataFile.Exists)
        {
            var zipDir = DataDir.GetChildDirectoryInfo("zip").CreateAndPipe();
            uri.DownloadFile(zipDir.GetChildFileInfo("downloaded.zip")).Decompress_Zip(zipDir);
            targetFileLocationAfterExtracted.MoveTo(targetDataFile);
        }
    }

    public void SplitData(double testFraction = 0.2)
    {
        var split = Context.Data.TrainTestSplit(AllData, testFraction);
        TrainData = split.TrainSet;
        TestData = split.TestSet;
    }

    public (IDataView, IDataView) SplitData(IDataView splittingData, double testFraction = 0.2)
    {
        var split = Context.Data.TrainTestSplit(splittingData, testFraction);
        return (split.TrainSet, split.TestSet);
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


    // ★★★★★★★★★★★★★★★ 

}
