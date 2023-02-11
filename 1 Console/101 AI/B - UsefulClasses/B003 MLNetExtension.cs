using Microsoft.ML.AutoML;
using Microsoft.ML.Data;

namespace Aki32Utilities.ConsoleAppUtilities.AI;
public class MLNetExtension
{


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
                General.ConsoleExtension.WriteLineWithColor($"Exception during AutoML iteration: {dRunDetail.Exception}", ConsoleColor.Red);
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




    }

}
