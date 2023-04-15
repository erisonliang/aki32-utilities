using Microsoft.ML.Data;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public class H777_SearchResultInput
{
    [LoadColumn(0)]
    public float Label { get; set; }

    [LoadColumn(1)]
    public int GroupId { get; set; }

    [LoadColumn(2, 133)]
    [VectorType(132)]
    public float[] Features { get; set; }
}
