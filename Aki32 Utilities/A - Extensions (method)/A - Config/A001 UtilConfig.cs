using System.Text;

namespace Aki32_Utilities.Extensions;
public static class UtilConfig
{

    public static bool ConsoleOutput = true;

    public static bool IncludeGuidToNewOutputDirName = true;

    /// <summary>
    /// For creating new output dir name
    /// </summary>
    /// <param name="MethodName"></param>
    /// <returns></returns>
    public static string GetNewOutputDirName(string MethodName)
    {
        var randomGuid = Guid.NewGuid().ToString()[0..6].ToUpper();
        if (IncludeGuidToNewOutputDirName)
            return $"output_{MethodName}_{randomGuid}";
        else
            return $"output_{MethodName}";
    }

}
