using Microsoft.ML.Data;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public class IrisInput
{
    [LoadColumn(0)]
    public float Label;

    [LoadColumn(1)]
    public float SepalLength;

    [LoadColumn(2)]
    public float SepalWidth;

    [LoadColumn(3)]
    public float PetalLength;

    [LoadColumn(4)]
    public float PetalWidth;
}