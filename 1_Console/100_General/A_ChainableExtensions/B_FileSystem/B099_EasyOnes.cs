using System.IO.Compression;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{
    /// <summary>
    /// Return a FileInfo (inputDir\\fileName)
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static FileInfo GetChildFileInfo(this DirectoryInfo inputDir, string fileName)
    {
        // main
        return new FileInfo(Path.Combine(inputDir.FullName, fileName));
    }

    /// <summary>
    /// Return a DirectoryInfo (inputDir\\dirName)
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="dirName"></param>
    /// <returns></returns>
    public static DirectoryInfo GetChildDirectoryInfo(this DirectoryInfo inputDir, string dirName)
    {
        // main
        return new DirectoryInfo(Path.Combine(inputDir.FullName, dirName));
    }

    /// <summary>
    /// Call inputDir.Create(); and return inputDir
    /// </summary>
    /// <param name="inputDir"></param>
    /// <returns></returns>
    public static DirectoryInfo CreateAndPipe(this DirectoryInfo inputDir)
    {
        // main
        inputDir.Create();
        return inputDir;
    }

}
