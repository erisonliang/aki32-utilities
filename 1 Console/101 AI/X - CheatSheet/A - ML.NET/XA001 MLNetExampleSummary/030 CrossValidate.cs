using Microsoft.ML;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public partial class MLNetExampleSummary : MLNetHandler
{
    private void CrossValidate(IDataView? targetData = null)
    {
        General.ConsoleExtension.WriteLineWithColor($"\r\n★★★★★★★★★★★★★★★ Cross Validation", ConsoleColor.Yellow);

        switch (Scenario)
        {
            case MLNetExampleScenario.A002_Spam_Detection:
                {
                    var crossValMetrics = Context.MulticlassClassification.CrossValidate(data: (targetData ?? AllData), estimator: PipeLine, numberOfFolds: 5);
                    ConsoleExtension.PrintMetrics(crossValMetrics);

                    break;
                }

            // ignore
            default:
                {
                    Console.WriteLine("ignore");

                    break;
                }
        }
    }
}
