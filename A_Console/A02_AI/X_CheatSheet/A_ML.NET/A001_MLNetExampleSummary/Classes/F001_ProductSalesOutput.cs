using Microsoft.ML.Data;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public class F001_ProductSalesOutput
{
    //vector to hold alert,score,p-value values
    [VectorType(3)]
    public double[] Prediction { get; set; }
}
