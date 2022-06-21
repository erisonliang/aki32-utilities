using System.Text;

namespace Aki32_Utilities.OverridingUtils;
public static class UtilConfig
{

    // ★★★★★★★★★★★★★★★

    /// <summary>
    /// Console output for for-loop process
    /// </summary>
    public static bool ConsoleOutput = true;

    /// <summary>
    /// For creating new output dir name
    /// </summary>
    /// <param name="MethodName"></param>
    /// <returns></returns>
    public static string GetNewFileName(string MethodName)
    {
        var randomGuid = Guid.NewGuid().ToString()[0..6].ToUpper();
        return $"output_{MethodName}_{randomGuid}";
    }

    // ★★★★★★★★★★★★★★★

}
