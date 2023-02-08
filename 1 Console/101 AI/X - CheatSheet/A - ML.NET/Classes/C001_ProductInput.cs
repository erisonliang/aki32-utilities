using Microsoft.ML.Data;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public class C001_ProductInput
{
    [KeyType(count: 262111)]
    public uint ProductID { get; set; }

    [KeyType(count: 262111)]
    public uint CoPurchaseProductID { get; set; }
}
