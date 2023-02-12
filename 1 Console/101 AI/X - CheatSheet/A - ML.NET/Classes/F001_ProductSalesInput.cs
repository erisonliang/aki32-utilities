using Microsoft.ML.Data;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public class F001_ProductSalesInput
{
    [LoadColumn(0)]
    public string Month;

    [LoadColumn(1)]
    public float numSales;
}
