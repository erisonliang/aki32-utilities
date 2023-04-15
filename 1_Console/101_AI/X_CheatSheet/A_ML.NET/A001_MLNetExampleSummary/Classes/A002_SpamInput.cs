using Microsoft.ML.Data;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public class A002_SpamInput
{
    [LoadColumn(0)]
    public string Label { get; set; }
    [LoadColumn(1)]
    public string Message { get; set; }
}
