using Microsoft.ML;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public partial class MLNetExampleSummary : MLNetHandler
{
    private void CrossValidate(MLContext context)
    {
        General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ Cross Validation", ConsoleColor.Yellow);

        switch (Scenario)
        {
            case MLNetExampleScenario.A002_Spam_Detection:

                var crossValMetrics = context.MulticlassClassification.CrossValidate(data: AllData, estimator: PipeLine, numberOfFolds: 5);
                ConsoleExtension.PrintMetrics(crossValMetrics);

                break;

            // ignore
            default:
                Console.WriteLine("ignore");
                break;
        }
    }
}
