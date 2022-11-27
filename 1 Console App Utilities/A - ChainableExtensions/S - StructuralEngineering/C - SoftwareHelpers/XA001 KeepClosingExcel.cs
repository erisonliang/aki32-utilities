using Aki32_Utilities.ChainableExtensions.General;

namespace Aki32_Utilities.ChainableExtensions.StructuralEngineering;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ sugar

    public static void KeepClosingExcel(int interval = 5000)
    {
        Aki32_Utilities.StructuralEngineering.SoftwareHelpers.KeepClosingExcel.CheckAndKillEndless(interval);
    }


    // ★★★★★★★★★★★★★★★

}
