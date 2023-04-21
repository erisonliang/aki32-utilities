using System.Data;

using Microsoft.ML;
using Microsoft.ML.Data;

using Aki32Utilities.ConsoleAppUtilities.General;

using static Microsoft.ML.TrainCatalogBase;
using Microsoft.ML.AutoML;

namespace Aki32Utilities.ConsoleAppUtilities.AI;
public static class ConsoleExtension
{
    // ★★★★★★★★★★★★★★★ props

    public static int ConsoleMaxWidth { get; set; } = 250;


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
        else if (metricsObject is BinaryClassificationMetrics binMetrics)
        {
            Console.WriteLine($"Accuracy: {binMetrics.Accuracy:P2}");
            Console.WriteLine($"Area Under Curve:      {binMetrics.AreaUnderRocCurve:P2}");
            Console.WriteLine($"Area under Precision recall Curve:  {binMetrics.AreaUnderPrecisionRecallCurve:P2}");
            Console.WriteLine($"F1Score:  {binMetrics.F1Score:P2}");
            Console.WriteLine($"PositivePrecision:  {binMetrics.PositivePrecision:#.##}");
            Console.WriteLine($"PositiveRecall:  {binMetrics.PositiveRecall:#.##}");
            Console.WriteLine($"NegativePrecision:  {binMetrics.NegativePrecision:#.##}");
            Console.WriteLine($"NegativeRecall:  {binMetrics.NegativeRecall:P2}");

            Console.WriteLine("Confusion Matrix:");
            Console.WriteLine(binMetrics.ConfusionMatrix.GetFormattedConfusionTable());
        }
        else if (metricsObject is CalibratedBinaryClassificationMetrics caliBinMetrics)
        {
            Console.WriteLine($"Accuracy: {caliBinMetrics.Accuracy:P2}");
            Console.WriteLine($"Area Under Curve:      {caliBinMetrics.AreaUnderRocCurve:P2}");
            Console.WriteLine($"Area under Precision recall Curve:  {caliBinMetrics.AreaUnderPrecisionRecallCurve:P2}");
            Console.WriteLine($"F1Score:  {caliBinMetrics.F1Score:P2}");
            Console.WriteLine($"LogLoss:  {caliBinMetrics.LogLoss:#.##}");
            Console.WriteLine($"LogLossReduction:  {caliBinMetrics.LogLossReduction:#.##}");
            Console.WriteLine($"PositivePrecision:  {caliBinMetrics.PositivePrecision:#.##}");
            Console.WriteLine($"PositiveRecall:  {caliBinMetrics.PositiveRecall:#.##}");
            Console.WriteLine($"NegativePrecision:  {caliBinMetrics.NegativePrecision:#.##}");
            Console.WriteLine($"NegativeRecall:  {caliBinMetrics.NegativeRecall:P2}");
            Console.WriteLine("Confusion Matrix:");
            Console.WriteLine(caliBinMetrics.ConfusionMatrix.GetFormattedConfusionTable());

        }
        else if (metricsObject is MulticlassClassificationMetrics multiMetrics)
        {
            Console.WriteLine($"AccuracyMacro = {multiMetrics.MacroAccuracy:0.####}, a value between 0 and 1, the closer to 1, the better");
            Console.WriteLine($"AccuracyMicro = {multiMetrics.MicroAccuracy:0.####}, a value between 0 and 1, the closer to 1, the better");
            Console.WriteLine($"LogLoss = {multiMetrics.LogLoss:0.####}, the closer to 0, the better");
            try
            {
                Console.WriteLine($" LogLoss for class 1 = {multiMetrics.PerClassLogLoss[0]:0.####}, the closer to 0, the better");
                Console.WriteLine($" LogLoss for class 2 = {multiMetrics.PerClassLogLoss[1]:0.####}, the closer to 0, the better");
                Console.WriteLine($" LogLoss for class 3 = {multiMetrics.PerClassLogLoss[2]:0.####}, the closer to 0, the better");
            }
            catch (Exception)
            {
            }

            Console.WriteLine("Confusion Matrix:");
            Console.WriteLine(multiMetrics.ConfusionMatrix.GetFormattedConfusionTable());
        }
        else if (metricsObject is RegressionMetrics regressionMetrics)
        {
            Console.WriteLine($"*       LossFn:        {regressionMetrics.LossFunction:0.##}");
            Console.WriteLine($"*       R2 Score:      {regressionMetrics.RSquared:0.##}");
            Console.WriteLine($"*       Absolute loss: {regressionMetrics.MeanAbsoluteError:#.##}");
            Console.WriteLine($"*       Squared loss:  {regressionMetrics.MeanSquaredError:#.##}");
            Console.WriteLine($"*       RMS loss:      {regressionMetrics.RootMeanSquaredError:#.##}");
        }
        else if (metricsObject is AnomalyDetectionMetrics anomalyMetrics)
        {
            Console.WriteLine($"*       Area Under ROC Curve:                       {anomalyMetrics.AreaUnderRocCurve:P2}");
            Console.WriteLine($"*       Detection rate at false positive count: {anomalyMetrics.DetectionRateAtFalsePositiveCount}");
        }
        else if (metricsObject is ClusteringMetrics clusteringMetrics)
        {
            Console.WriteLine($"*       Average Distance: {clusteringMetrics.AverageDistance}");
            Console.WriteLine($"*       Davies Bouldin Index is: {clusteringMetrics.DaviesBouldinIndex}");
        }
        else if (metricsObject is RankingMetrics rankMetrics)
        {
            Console.WriteLine($"DCG : {string.Join(", ", rankMetrics.DiscountedCumulativeGains.Select((d, i) => $"@{i + 1}:{d:F4}").ToArray())}");
            Console.WriteLine($"NDCG: {string.Join(", ", rankMetrics.NormalizedDiscountedCumulativeGains.Select((d, i) => $"@{i + 1}:{d:F4}").ToArray())}\n");

            //var th = new TimeHistory();
            //th["#"] = rankMetrics.DiscountedCumulativeGains.Select((d, i) => i + 1d).ToArray();
            //th["DCG"] = rankMetrics.DiscountedCumulativeGains.ToArray();
            //th["NDCG"] = rankMetrics.NormalizedDiscountedCumulativeGains.Select((d, i) => d).ToArray();
            //th.WriteToConsole();
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
        else
        {
            General.ConsoleExtension.WriteLineWithColor($"Display method not defined", ConsoleColor.Red);
        }

        Console.WriteLine(@$"=======================================================");
    }

    public static void PrintMetricsInOneLineHeader(RunDetail runDetail)
    {
        var message = "";

        if (false)
        {
        }
        else if (runDetail is RunDetail<BinaryClassificationMetrics>)
        {
            message = $"{"",-4} {"Trainer",-35} {"Accuracy",9} {"AUC",8} {"AUPRC",8} {"F1-score",9} {"Duration",9}";
        }
        else if (runDetail is RunDetail<MulticlassClassificationMetrics>)
        {
            message = $"{"",-4} {"Trainer",-35} {"MicroAccuracy",14} {"MacroAccuracy",14} {"Duration",9}";
        }
        else if (runDetail is RunDetail<RegressionMetrics>)
        {
            message = $"{"",-4} {"Trainer",-35} {"RSquared",8} {"Absolute-loss",13} {"Squared-loss",12} {"RMS-loss",8} {"Duration",9}";
        }

        else if (runDetail is RunDetail<RankingMetrics>)
        {
            message = $"{"",-4} {"Trainer",-15} {"NDCG@1",9} {"NDCG@3",9} {"NDCG@10",9} {"DCG@10",9} {"Duration",9}";
        }

        Console.WriteLine(message.Shorten(..ConsoleMaxWidth));
    }

    public static void PrintMetricsInOneLine(int index, RunDetail runDetail)
    {
        //var trainerName = runDetail.TrainerName;
        var trainerName = runDetail.TrainerName.Split("=>").Where(x => x != "Unknown").LastOrDefault();
        var runtimeInSeconds = runDetail.RuntimeInSeconds;
        var message = "";

        if (false)
        {
        }
        else if (runDetail is RunDetail<BinaryClassificationMetrics> binMetricsDetail)
        {
            var metrics = binMetricsDetail.ValidationMetrics;
            message = $"{index,-4} {trainerName,-35} {metrics?.Accuracy ?? double.NaN,9:F4} {metrics?.AreaUnderRocCurve ?? double.NaN,8:F4} {metrics?.AreaUnderPrecisionRecallCurve ?? double.NaN,8:F4} {metrics?.F1Score ?? double.NaN,9:F4} {runtimeInSeconds,9:F1}";
        }
        else if (runDetail is RunDetail<MulticlassClassificationMetrics> multiMetricsDetail)
        {
            var metrics = multiMetricsDetail.ValidationMetrics;
            message = $"{index,-4} {trainerName,-35} {metrics?.MicroAccuracy ?? double.NaN,14:F4} {metrics?.MacroAccuracy ?? double.NaN,14:F4} {runtimeInSeconds,9:F1}";
        }
        else if (runDetail is RunDetail<RegressionMetrics> regressionMetricsDetail)
        {
            var metrics = regressionMetricsDetail.ValidationMetrics;
            message = $"{index,-4} {trainerName,-35} {metrics?.RSquared ?? double.NaN,8:F4} {metrics?.MeanAbsoluteError ?? double.NaN,13:F2} {metrics?.MeanSquaredError ?? double.NaN,12:F2} {metrics?.RootMeanSquaredError ?? double.NaN,8:F2} {runtimeInSeconds,9:F1}";
        }

        else if (runDetail is RunDetail<RankingMetrics> clusteringMetricsDetail)
        {
            var metrics = clusteringMetricsDetail.ValidationMetrics;
            message = $"{index,-4} {trainerName,-15} {metrics?.NormalizedDiscountedCumulativeGains[0] ?? double.NaN,9:F4} {metrics?.NormalizedDiscountedCumulativeGains[2] ?? double.NaN,9:F4} {metrics?.NormalizedDiscountedCumulativeGains[9] ?? double.NaN,9:F4} {metrics?.DiscountedCumulativeGains[9] ?? double.NaN,9:F4} {runtimeInSeconds,9:F1}";
        }

        Console.WriteLine(message.Shorten(..ConsoleMaxWidth));
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

    public static void WriteToConsole(this IDataView dataView, int displayRows = 7, int cellWidth = 15)
    {
        General.ConsoleExtension.WriteLineWithColor($"\r\nShowing dataView: {displayRows} rows");
        var preViewTransformedData = dataView.Preview(maxRows: displayRows);

        // header
        {
            var ColumnCollection = preViewTransformedData.RowView[0].Values;
            var line = "";
            foreach (KeyValuePair<string, object> column in ColumnCollection)
                line += $"{column.Key.Shorten(..cellWidth).PadRight(cellWidth, ' ')}|";
            Console.WriteLine(line.Shorten(..ConsoleMaxWidth));
        }

        // content
        foreach (var row in preViewTransformedData.RowView)
        {
            var ColumnCollection = row.Values;
            string line = "";
            foreach (KeyValuePair<string, object> column in ColumnCollection)
                line += $"{column.Value.ToString()?.Shorten(..cellWidth).PadRight(cellWidth, ' ')}|";
            Console.WriteLine(line.Shorten(..ConsoleMaxWidth));
        }

        Console.WriteLine();

    }


    // ★★★★★★★★★★★★★★★ 以上

}
