using System.Data;
using System.Diagnostics;

using Microsoft.ML;
using Microsoft.ML.Data;

using Aki32Utilities.ConsoleAppUtilities.General;

using static Microsoft.ML.TrainCatalogBase;
using System.Collections.Specialized;

namespace Aki32Utilities.ConsoleAppUtilities.AI;
public static class ConsoleExtension
{

    // ★★★★★★★★★★★★★★★ print results

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

    public static void PrintMetrics(object metricsObject)
    {
        Console.WriteLine(@$"=======================================================");
        Console.WriteLine(@$"{metricsObject.GetType()}");
        Console.WriteLine($@"-------------------------------------------------------");

        if (false)
        {
        }
        else if (metricsObject is RegressionMetrics regressionMetrics)
        {
            Console.WriteLine($"*       LossFn:        {regressionMetrics.LossFunction:0.##}");
            Console.WriteLine($"*       R2 Score:      {regressionMetrics.RSquared:0.##}");
            Console.WriteLine($"*       Absolute loss: {regressionMetrics.MeanAbsoluteError:#.##}");
            Console.WriteLine($"*       Squared loss:  {regressionMetrics.MeanSquaredError:#.##}");
            Console.WriteLine($"*       RMS loss:      {regressionMetrics.RootMeanSquaredError:#.##}");
        }
        else if (metricsObject is CalibratedBinaryClassificationMetrics binaryMetrics)
        {
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
        }
        else if (metricsObject is AnomalyDetectionMetrics anomalyMetrics)
        {
            Console.WriteLine($"*       Area Under ROC Curve:                       {anomalyMetrics.AreaUnderRocCurve:P2}");
            Console.WriteLine($"*       Detection rate at false positive count: {anomalyMetrics.DetectionRateAtFalsePositiveCount}");
        }
        else if (metricsObject is MulticlassClassificationMetrics multiMetrics)
        {
            Console.WriteLine($"    AccuracyMacro = {multiMetrics.MacroAccuracy:0.####}, a value between 0 and 1, the closer to 1, the better");
            Console.WriteLine($"    AccuracyMicro = {multiMetrics.MicroAccuracy:0.####}, a value between 0 and 1, the closer to 1, the better");
            Console.WriteLine($"    LogLoss = {multiMetrics.LogLoss:0.####}, the closer to 0, the better");
            try
            {
                Console.WriteLine($"    LogLoss for class 1 = {multiMetrics.PerClassLogLoss[0]:0.####}, the closer to 0, the better");
                Console.WriteLine($"    LogLoss for class 2 = {multiMetrics.PerClassLogLoss[1]:0.####}, the closer to 0, the better");
                Console.WriteLine($"    LogLoss for class 3 = {multiMetrics.PerClassLogLoss[2]:0.####}, the closer to 0, the better");
            }
            catch (Exception)
            {
            }
        }
        else if (metricsObject is IReadOnlyList<CrossValidationResult<RegressionMetrics>> crossValidationResults)
        {
            var L1 = crossValidationResults.Select(r => r.Metrics.MeanAbsoluteError);
            var L2 = crossValidationResults.Select(r => r.Metrics.MeanSquaredError);
            var RMS = crossValidationResults.Select(r => r.Metrics.RootMeanSquaredError);
            var lossFunction = crossValidationResults.Select(r => r.Metrics.LossFunction);
            var R2 = crossValidationResults.Select(r => r.Metrics.RSquared);


            Console.WriteLine($"*       Average L1 Loss:    {L1.Average():0.###} ");
            Console.WriteLine($"*       Average L2 Loss:    {L2.Average():0.###}  ");
            Console.WriteLine($"*       Average RMS:          {RMS.Average():0.###}  ");
            Console.WriteLine($"*       Average Loss Function: {lossFunction.Average():0.###}  ");
            Console.WriteLine($"*       Average R-squared: {R2.Average():0.###}  ");
        }
        else if (metricsObject is IReadOnlyList<CrossValidationResult<MulticlassClassificationMetrics>> crossValResults)
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

            Console.WriteLine($"*       Average MicroAccuracy:    {microAccuracyAverage:0.###}  - Standard deviation: ({microAccuraciesStdDeviation:#.###})  - Confidence Interval 95%: ({microAccuraciesConfidenceInterval95:#.###})");
            Console.WriteLine($"*       Average MacroAccuracy:    {macroAccuracyAverage:0.###}  - Standard deviation: ({macroAccuraciesStdDeviation:#.###})  - Confidence Interval 95%: ({macroAccuraciesConfidenceInterval95:#.###})");
            Console.WriteLine($"*       Average LogLoss:          {logLossAverage:#.###}  - Standard deviation: ({logLossStdDeviation:#.###})  - Confidence Interval 95%: ({logLossConfidenceInterval95:#.###})");
            Console.WriteLine($"*       Average LogLossReduction: {logLossReductionAverage:#.###}  - Standard deviation: ({logLossReductionStdDeviation:#.###})  - Confidence Interval 95%: ({logLossReductionConfidenceInterval95:#.###})");

        }
        else if (metricsObject is ClusteringMetrics clusteringMetrics)
        {
            Console.WriteLine($"*       Average Distance: {clusteringMetrics.AverageDistance}");
            Console.WriteLine($"*       Davies Bouldin Index is: {clusteringMetrics.DaviesBouldinIndex}");
        }
        else
        {
            General.ConsoleExtension.WriteLineWithColor($"Display method not defined", ConsoleColor.Red);
        }

        Console.WriteLine(@$"=======================================================");
    }

    private static double CalculateStandardDeviation(IEnumerable<double> values)
    {
        double average = values.Average();
        double sumOfSquaresOfDifferences = values.Select(val => (val - average) * (val - average)).Sum();
        double standardDeviation = Math.Sqrt(sumOfSquaresOfDifferences / (values.Count() - 1));
        return standardDeviation;
    }

    private static double CalculateConfidenceInterval95(IEnumerable<double> values)
    {
        double confidenceInterval95 = 1.96 * CalculateStandardDeviation(values) / Math.Sqrt((values.Count() - 1));
        return confidenceInterval95;
    }


    // ★★★★★★★★★★★★★★★ print data

    public static void WriteToConsole(this IDataView dataView, int numberOfRows = 10, int cellWidth = 15)
    {
        General.ConsoleExtension.WriteLineWithColor($"\r\nShow data in DataView: Showing {numberOfRows} rows with the columns");

        var preViewTransformedData = dataView.Preview(maxRows: numberOfRows);

        // header
        {
            var ColumnCollection = preViewTransformedData.RowView[0].Values;
            var line = "";
            foreach (KeyValuePair<string, object> column in ColumnCollection)
                line += $"{column.Key.Shorten(..cellWidth).PadRight(cellWidth, ' ')}|";
            Console.WriteLine(line);
        }

        // content
        foreach (var row in preViewTransformedData.RowView)
        {
            var ColumnCollection = row.Values;
            string line = "";
            foreach (KeyValuePair<string, object> column in ColumnCollection)
                line += $"{column.Value.ToString()?.Shorten(..cellWidth).PadRight(cellWidth, ' ')}|";
            Console.WriteLine(line);
        }

        Console.WriteLine();

    }


    // ★★★★★★★★★★★★★★★ 以上

}
