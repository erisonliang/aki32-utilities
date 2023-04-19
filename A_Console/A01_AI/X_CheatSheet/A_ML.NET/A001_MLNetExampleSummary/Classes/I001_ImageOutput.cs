using Microsoft.ML.Data;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public class I001_ImageOutput
{
    [ColumnName("Score")]
    public float[] Score;

    [ColumnName("PredictedLabel")]
    public string PredictedLabel;
}
