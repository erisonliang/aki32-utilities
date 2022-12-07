using System.Text;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// rename a file path
    /// </summary>
    /// <param name="inputPath"></param>
    /// <param name="newNameWithoutExtension">"*" will be replaced with old name</param>
    /// <returns></returns>
    public static string GetRenamedPath(this string inputPath, string newNameWithoutExtension)
    {
        // main
        var ext = Path.GetExtension(inputPath);
        newNameWithoutExtension = newNameWithoutExtension.Replace("*", Path.GetFileNameWithoutExtension(inputPath));
        var outputFileName = $"{newNameWithoutExtension}{ext}";
        var outputFilePath = Path.Combine(Path.GetDirectoryName(inputPath)!, outputFileName);


        // post process
        return outputFilePath;
    }

}
