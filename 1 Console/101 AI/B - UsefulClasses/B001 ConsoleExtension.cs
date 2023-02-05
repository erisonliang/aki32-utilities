using System.Data;
using System.Diagnostics;

using Microsoft.ML;
using Microsoft.ML.Data;

using Aki32Utilities.ConsoleAppUtilities.General;

using static Microsoft.ML.TrainCatalogBase;

namespace Aki32Utilities.ConsoleAppUtilities.AI;
public static class ConsoleExtension
{
    public static void PrintPrediction(string prediction)
    {
        Console.WriteLine(@$"=======================================================");
        Console.WriteLine(@$"Predicted : {prediction}");
        Console.WriteLine(@$"=======================================================");
    }

    public static void PrintRegressionPredictionVersusObserved(string predictionCount, string observedCount)
    {
        Console.WriteLine(@$"=======================================================");
        Console.WriteLine(@$"Predicted : {predictionCount}");
        Console.WriteLine(@$"Actual:     {observedCount}");
        Console.WriteLine(@$"=======================================================");
    }

    public static void PrintMetrics(string name, object metricsObj)
    {
        if (false)
        {
        }
        else if (metricsObj is RegressionMetrics regressionMetrics)
        {
            Console.WriteLine($"*************************************************");
            Console.WriteLine($"*       Metrics for {name} regression model      ");
            Console.WriteLine($"*------------------------------------------------");
            Console.WriteLine($"*       LossFn:        {regressionMetrics.LossFunction:0.##}");
            Console.WriteLine($"*       R2 Score:      {regressionMetrics.RSquared:0.##}");
            Console.WriteLine($"*       Absolute loss: {regressionMetrics.MeanAbsoluteError:#.##}");
            Console.WriteLine($"*       Squared loss:  {regressionMetrics.MeanSquaredError:#.##}");
            Console.WriteLine($"*       RMS loss:      {regressionMetrics.RootMeanSquaredError:#.##}");
            Console.WriteLine($"*************************************************");
        }
        else if (metricsObj is CalibratedBinaryClassificationMetrics binaryMetrics)
        {
            Console.WriteLine($"************************************************************");
            Console.WriteLine($"*       Metrics for {name} binary classification model      ");
            Console.WriteLine($"*-----------------------------------------------------------");
            Console.WriteLine($"*       Accuracy: {binaryMetrics.Accuracy:P2}");
            Console.WriteLine($"*       Area Under Curve:      {binaryMetrics.AreaUnderRocCurve:P2}");
            Console.WriteLine($"*       Area under Precision recall Curve:  {binaryMetrics.AreaUnderPrecisionRecallCurve:P2}");
            Console.WriteLine($"*       F1Score:  {binaryMetrics.F1Score:P2}");
            Console.WriteLine($"*       LogLoss:  {binaryMetrics.LogLoss:#.##}");
            Console.WriteLine($"*       LogLossReduction:  {binaryMetrics.LogLossReduction:#.##}");
            Console.WriteLine($"*       PositivePrecision:  {binaryMetrics.PositivePrecision:#.##}");
            Console.WriteLine($"*       PositiveRecall:  {binaryMetrics.PositiveRecall:#.##}");
            Console.WriteLine($"*       NegativePrecision:  {binaryMetrics.NegativePrecision:#.##}");
            Console.WriteLine($"*       NegativeRecall:  {binaryMetrics.NegativeRecall:P2}");
            Console.WriteLine($"************************************************************");
        }
        else if (metricsObj is AnomalyDetectionMetrics anomalyMetrics)
        {
            Console.WriteLine($"************************************************************");
            Console.WriteLine($"*       Metrics for {name} anomaly detection model      ");
            Console.WriteLine($"*-----------------------------------------------------------");
            Console.WriteLine($"*       Area Under ROC Curve:                       {anomalyMetrics.AreaUnderRocCurve:P2}");
            Console.WriteLine($"*       Detection rate at false positive count: {anomalyMetrics.DetectionRateAtFalsePositiveCount}");
            Console.WriteLine($"************************************************************");
        }
        else if (metricsObj is MulticlassClassificationMetrics multiMetrics)
        {
            Console.WriteLine($"************************************************************");
            Console.WriteLine($"*    Metrics for {name} multi-class classification model   ");
            Console.WriteLine($"*-----------------------------------------------------------");
            Console.WriteLine($"    AccuracyMacro = {multiMetrics.MacroAccuracy:0.####}, a value between 0 and 1, the closer to 1, the better");
            Console.WriteLine($"    AccuracyMicro = {multiMetrics.MicroAccuracy:0.####}, a value between 0 and 1, the closer to 1, the better");
            Console.WriteLine($"    LogLoss = {multiMetrics.LogLoss:0.####}, the closer to 0, the better");
            Console.WriteLine($"    LogLoss for class 1 = {multiMetrics.PerClassLogLoss[0]:0.####}, the closer to 0, the better");
            Console.WriteLine($"    LogLoss for class 2 = {multiMetrics.PerClassLogLoss[1]:0.####}, the closer to 0, the better");
            Console.WriteLine($"    LogLoss for class 3 = {multiMetrics.PerClassLogLoss[2]:0.####}, the closer to 0, the better");
            Console.WriteLine($"************************************************************");
        }
        else if (metricsObj is IReadOnlyList<CrossValidationResult<RegressionMetrics>> crossValidationResults)
        {
            var L1 = crossValidationResults.Select(r => r.Metrics.MeanAbsoluteError);
            var L2 = crossValidationResults.Select(r => r.Metrics.MeanSquaredError);
            var RMS = crossValidationResults.Select(r => r.Metrics.RootMeanSquaredError);
            var lossFunction = crossValidationResults.Select(r => r.Metrics.LossFunction);
            var R2 = crossValidationResults.Select(r => r.Metrics.RSquared);

            Console.WriteLine($"*************************************************************************************************************");
            Console.WriteLine($"*       Metrics for {name} Regression model      ");
            Console.WriteLine($"*------------------------------------------------------------------------------------------------------------");
            Console.WriteLine($"*       Average L1 Loss:    {L1.Average():0.###} ");
            Console.WriteLine($"*       Average L2 Loss:    {L2.Average():0.###}  ");
            Console.WriteLine($"*       Average RMS:          {RMS.Average():0.###}  ");
            Console.WriteLine($"*       Average Loss Function: {lossFunction.Average():0.###}  ");
            Console.WriteLine($"*       Average R-squared: {R2.Average():0.###}  ");
            Console.WriteLine($"*************************************************************************************************************");
        }
        else if (metricsObj is IReadOnlyList<CrossValidationResult<MulticlassClassificationMetrics>> crossValResults)
        {
            var metricsInMultipleFolds = crossValResults.Select(r => r.Metrics);

            var microAccuracyValues = metricsInMultipleFolds.Select(m => m.MicroAccuracy);
            var microAccuracyAverage = microAccuracyValues.Average();
            var microAccuraciesStdDeviation = CalculateStandardDeviation(microAccuracyValues);
            var microAccuraciesConfidenceInterval95 = CalculateConfidenceInterval95(microAccuracyValues);

            var macroAccuracyValues = metricsInMultipleFolds.Select(m => m.MacroAccuracy);
            var macroAccuracyAverage = macroAccuracyValues.Average();
            var macroAccuraciesStdDeviation = CalculateStandardDeviation(macroAccuracyValues);
            var macroAccuraciesConfidenceInterval95 = CalculateConfidenceInterval95(macroAccuracyValues);

            var logLossValues = metricsInMultipleFolds.Select(m => m.LogLoss);
            var logLossAverage = logLossValues.Average();
            var logLossStdDeviation = CalculateStandardDeviation(logLossValues);
            var logLossConfidenceInterval95 = CalculateConfidenceInterval95(logLossValues);

            var logLossReductionValues = metricsInMultipleFolds.Select(m => m.LogLossReduction);
            var logLossReductionAverage = logLossReductionValues.Average();
            var logLossReductionStdDeviation = CalculateStandardDeviation(logLossReductionValues);
            var logLossReductionConfidenceInterval95 = CalculateConfidenceInterval95(logLossReductionValues);

            Console.WriteLine($"*************************************************************************************************************");
            Console.WriteLine($"*       Metrics for {name} Multi-class Classification model      ");
            Console.WriteLine($"*------------------------------------------------------------------------------------------------------------");
            Console.WriteLine($"*       Average MicroAccuracy:    {microAccuracyAverage:0.###}  - Standard deviation: ({microAccuraciesStdDeviation:#.###})  - Confidence Interval 95%: ({microAccuraciesConfidenceInterval95:#.###})");
            Console.WriteLine($"*       Average MacroAccuracy:    {macroAccuracyAverage:0.###}  - Standard deviation: ({macroAccuraciesStdDeviation:#.###})  - Confidence Interval 95%: ({macroAccuraciesConfidenceInterval95:#.###})");
            Console.WriteLine($"*       Average LogLoss:          {logLossAverage:#.###}  - Standard deviation: ({logLossStdDeviation:#.###})  - Confidence Interval 95%: ({logLossConfidenceInterval95:#.###})");
            Console.WriteLine($"*       Average LogLossReduction: {logLossReductionAverage:#.###}  - Standard deviation: ({logLossReductionStdDeviation:#.###})  - Confidence Interval 95%: ({logLossReductionConfidenceInterval95:#.###})");
            Console.WriteLine($"*************************************************************************************************************");

        }
        else if (metricsObj is ClusteringMetrics clusteringMetrics)
        {
            Console.WriteLine($"*************************************************");
            Console.WriteLine($"*       Metrics for {name} clustering model      ");
            Console.WriteLine($"*------------------------------------------------");
            Console.WriteLine($"*       Average Distance: {clusteringMetrics.AverageDistance}");
            Console.WriteLine($"*       Davies Bouldin Index is: {clusteringMetrics.DaviesBouldinIndex}");
            Console.WriteLine($"*************************************************");
        }
        else if (metricsObj is string metrics)
        {

        }
        else if (metricsObj is string metrics)
        {

        }
    }



    // ★★★★★★★★★★★★★★★ いる？

    public static double CalculateStandardDeviation(IEnumerable<double> values)
    {
        double average = values.Average();
        double sumOfSquaresOfDifferences = values.Select(val => (val - average) * (val - average)).Sum();
        double standardDeviation = Math.Sqrt(sumOfSquaresOfDifferences / (values.Count() - 1));
        return standardDeviation;
    }

    public static double CalculateConfidenceInterval95(IEnumerable<double> values)
    {
        double confidenceInterval95 = 1.96 * CalculateStandardDeviation(values) / Math.Sqrt((values.Count() - 1));
        return confidenceInterval95;
    }

    public static void ShowDataViewInConsole(MLContext mlContext, IDataView dataView, int numberOfRows = 4)
    {
        string msg = string.Format("Show data in DataView: Showing {0} rows with the columns", numberOfRows.ToString());
        ConsoleWriteHeader(msg);

        var preViewTransformedData = dataView.Preview(maxRows: numberOfRows);

        foreach (var row in preViewTransformedData.RowView)
        {
            var ColumnCollection = row.Values;
            string lineToPrint = "Row--> ";
            foreach (KeyValuePair<string, object> column in ColumnCollection)
            {
                lineToPrint += $"| {column.Key}:{column.Value}";
            }
            Console.WriteLine(lineToPrint + "\n");
        }
    }

    [Conditional("DEBUG")]
    public static void PeekDataViewInConsole(MLContext mlContext, IDataView dataView, IEstimator<ITransformer> pipeline, int numberOfRows = 4)
    {
        string msg = string.Format("Peek data in DataView: Showing {0} rows with the columns", numberOfRows.ToString());
        ConsoleWriteHeader(msg);

        //https://github.com/dotnet/machinelearning/blob/main/docs/code/MlNetCookBook.md#how-do-i-look-at-the-intermediate-data
        var transformer = pipeline.Fit(dataView);
        var transformedData = transformer.Transform(dataView);

        // 'transformedData' is a 'promise' of data, lazy-loading. call Preview
        //and iterate through the returned collection from preview.

        var preViewTransformedData = transformedData.Preview(maxRows: numberOfRows);

        foreach (var row in preViewTransformedData.RowView)
        {
            var ColumnCollection = row.Values;
            string lineToPrint = "Row--> ";
            foreach (KeyValuePair<string, object> column in ColumnCollection)
            {
                lineToPrint += $"| {column.Key}:{column.Value}";
            }
            Console.WriteLine(lineToPrint + "\n");
        }
    }

    [Conditional("DEBUG")]
    public static void PeekVectorColumnDataInConsole(MLContext mlContext, string columnName, IDataView dataView, IEstimator<ITransformer> pipeline, int numberOfRows = 4)
    {
        string msg = string.Format("Peek data in DataView: : Show {0} rows with just the '{1}' column", numberOfRows, columnName);
        ConsoleWriteHeader(msg);

        var transformer = pipeline.Fit(dataView);
        var transformedData = transformer.Transform(dataView);

        // Extract the 'Features' column.
        var someColumnData = transformedData.GetColumn<float[]>(columnName)
                                                    .Take(numberOfRows).ToList();

        // print to console the peeked rows

        int currentRow = 0;
        someColumnData.ForEach(row =>
        {
            currentRow++;
            String concatColumn = String.Empty;
            foreach (float f in row)
            {
                concatColumn += f.ToString();
            }

            Console.WriteLine();
            string rowMsg = string.Format("**** Row {0} with '{1}' field value ****", currentRow, columnName);
            Console.WriteLine(rowMsg);
            Console.WriteLine(concatColumn);
            Console.WriteLine();
        });
    }

    public static void ConsoleWriteHeader(params string[] lines)
    {
        var defaultColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" ");
        foreach (var line in lines)
        {
            Console.WriteLine(line);
        }
        var maxLength = lines.Select(x => x.Length).Max();
        Console.WriteLine(new string('#', maxLength));
        Console.ForegroundColor = defaultColor;
    }

    public static void ConsoleWriterSection(params string[] lines)
    {
        var defaultColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(" ");
        foreach (var line in lines)
        {
            Console.WriteLine(line);
        }
        var maxLength = lines.Select(x => x.Length).Max();
        Console.WriteLine(new string('-', maxLength));
        Console.ForegroundColor = defaultColor;
    }

    public static void ConsolePressAnyKey()
    {
        Console.WriteLine();
        General.ConsoleExtension.WriteLineWithColor("Press any key to finish.");
        Console.ReadKey();
    }

    public static void ConsoleWriteException(params string[] lines)
    {
        var defaultColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        const string exceptionTitle = "EXCEPTION";
        Console.WriteLine(" ");
        Console.WriteLine(exceptionTitle);
        Console.WriteLine(new string('#', exceptionTitle.Length));
        Console.ForegroundColor = defaultColor;
        foreach (var line in lines)
        {
            Console.WriteLine(line);
        }
    }

    public static void ConsoleWriteWarning(params string[] lines)
    {
        var defaultColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        const string warningTitle = "WARNING";
        Console.WriteLine(" ");
        Console.WriteLine(warningTitle);
        Console.WriteLine(new string('#', warningTitle.Length));
        Console.ForegroundColor = defaultColor;
        foreach (var line in lines)
        {
            Console.WriteLine(line);
        }
    }
}
