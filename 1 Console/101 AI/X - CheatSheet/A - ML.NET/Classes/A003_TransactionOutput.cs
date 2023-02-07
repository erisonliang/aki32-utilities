
namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public class A003_TransactionOutput
{
    public bool Label;
    public bool PredictedLabel;
    public float Score;
    public float Probability;

    public void PrintToConsole()
    {
        Console.WriteLine($"Predicted Label: {PredictedLabel}");
        Console.WriteLine($"Probability: {Probability}  ({Score})");
    }
}
