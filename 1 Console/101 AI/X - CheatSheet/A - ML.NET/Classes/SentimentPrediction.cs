using Microsoft.ML.Data;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public class SentimentPrediction
{
    [ColumnName("PredictedLabel")]
    public bool Prediction { get; set; }

    [ColumnName("Probability")] // no need to use when the same
    public float Probability { get; set; }

    [ColumnName("Score")] // no need to use when the same
    public float Score { get; set; }
}