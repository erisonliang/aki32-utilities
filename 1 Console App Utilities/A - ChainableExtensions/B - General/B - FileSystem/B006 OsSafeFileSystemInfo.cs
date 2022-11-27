using System.Text;

namespace Aki32_Utilities.ChainableExtensions.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// Get FileInfo with OS default path for mac, windows and linux 
    /// </summary>
    public static FileInfo OsSafe(this FileInfo targetFileInfo)
    {
        var safePath = targetFileInfo.FullName
           .Replace('\\', Path.DirectorySeparatorChar)
           .Replace('/', Path.DirectorySeparatorChar)
           ;

        targetFileInfo = new FileInfo(safePath);
        return targetFileInfo;
    }

    /// <summary>
    /// Get DirectryInfo with OS default path for mac, windows and linux 
    /// </summary>
    public static DirectoryInfo OsSafe(this DirectoryInfo targetDirectoryInfo)
    {
        var safePath = targetDirectoryInfo.FullName
           .Replace('\\', Path.DirectorySeparatorChar)
           .Replace('/', Path.DirectorySeparatorChar)
           ;

        targetDirectoryInfo = new DirectoryInfo(safePath);
        return targetDirectoryInfo;
    }

}
