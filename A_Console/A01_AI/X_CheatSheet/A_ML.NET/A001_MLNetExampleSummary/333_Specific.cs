using Microsoft.ML;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public partial class MLNetExampleSummary : MLNetHandler
{
    private static void FilterMLContextLog(object sender, LoggingEventArgs e)
    {
        if (e.Message.StartsWith("[Source=ImageClassificationTrainer;"))
            Console.WriteLine(e.Message);
    }










}
