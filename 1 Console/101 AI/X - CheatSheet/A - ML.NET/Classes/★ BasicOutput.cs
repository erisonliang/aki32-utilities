using Microsoft.ML.Data;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
class BasicOutput_Binary
{
    [ColumnName("PredictedLabel")] // no need to use when the same
    public bool PredictedLabel { get; set; }

    [ColumnName("Probability")] // no need to use when the same
    public float Probability { get; set; }

    [ColumnName("Score")] // no need to use when the same
    public float Score { get; set; }

}

class BasicOutput_Multi
{
    [ColumnName("PredictedLabel")] // no need to use when the same
    public float PredictedLabel;

    [ColumnName("Score")] // no need to use when the same
    public float[] Score;

}
