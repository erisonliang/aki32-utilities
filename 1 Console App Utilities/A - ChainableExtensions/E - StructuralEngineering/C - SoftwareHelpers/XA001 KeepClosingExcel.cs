using Aki32_Utilities.StructuralEngineering;

namespace Aki32_Utilities.StructuralEngineering.ChainableExtensions;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ sugar

    public static void KeepClosingExcel(int interval = 5000)
    {
        StructuralEngineering.KeepClosingExcel.CheckAndKillEndless(interval);
    }


    // ★★★★★★★★★★★★★★★

}
