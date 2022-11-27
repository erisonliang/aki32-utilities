

using DocumentFormat.OpenXml.Presentation;

namespace StructuralEngineering_Utilities.SoftwareHelpers;
public static partial class StructuralEngineering_Utilities_Extensions
{

    // ★★★★★★★★★★★★★★★ sugar

    public static void KeepClosingExcel(int interval = 5000)
    {
        SoftwareHelpers.KeepClosingExcel.CheckAndKillEndless(interval);
    }


    // ★★★★★★★★★★★★★★★

}
