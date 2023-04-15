using Microsoft.ML.Data;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
class F002_PowerMeterInput
{
    [LoadColumn(0)]
    public string name { get; set; }
    [LoadColumn(1)]
    public DateTime time { get; set; }
    [LoadColumn(2), ColumnName("Label")]
    public float ConsumptionDiffNormalized { get; set; }
}
