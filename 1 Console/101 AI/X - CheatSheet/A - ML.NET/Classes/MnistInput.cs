using Microsoft.ML.Data;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
class MnistInput
{
    [ColumnName("PixelValues")]
    [VectorType(64)]
    public float[] PixelValues;

    [LoadColumn(64)]
    public float Number;
}
