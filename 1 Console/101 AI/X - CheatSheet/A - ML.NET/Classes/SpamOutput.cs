using Microsoft.ML.Data;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public class SpamOutput
{
    [ColumnName("PredictedLabel")]
    public string isSpam { get; set; }
}
