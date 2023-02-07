using Microsoft.ML.Data;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
class B003_MnistInput
{
    [LoadColumn(0, 63)]
    [VectorType(64)]
    public float[] PixelValues;

    [LoadColumn(64)]
    public float Number;
}
