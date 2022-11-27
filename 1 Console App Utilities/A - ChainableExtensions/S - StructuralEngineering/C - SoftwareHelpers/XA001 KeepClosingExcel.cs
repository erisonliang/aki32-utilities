

using DocumentFormat.OpenXml.Presentation;

namespace Aki32_Utilities.ChainableExtensions.StructuralEngineering;
public static partial class StructuralEngineering_Utilities_Extensions
{

    // ★★★★★★★★★★★★★★★ sugar

    public static void KeepClosingExcel(int interval = 5000)
    {
        SoftwareHelpers.KeepClosingExcel.CheckAndKillEndless(interval);
    }


    // ★★★★★★★★★★★★★★★

}
