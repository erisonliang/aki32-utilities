
namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// rename a file path.
    /// </summary>
    /// <param name="inputPath"></param>
    /// <param name="newFileNameWithoutExtension">First, define base file name. "*" represent old name</param>
    /// <param name="replaceSets">Second, replace strings in original file name</param>
    /// <returns></returns>
    public static string GetRenamedPath(this string inputPath, string newFileNameWithoutExtension, params (string from, string to)[] replaceSets)
    {
        // main
        var ext = Path.GetExtension(inputPath);

        newFileNameWithoutExtension = newFileNameWithoutExtension
            .Replace("*", Path.GetFileNameWithoutExtension(inputPath));

        foreach (var (from, to) in replaceSets)
            newFileNameWithoutExtension = newFileNameWithoutExtension.Replace(from, to);

        if (newFileNameWithoutExtension == "")
            newFileNameWithoutExtension = "output";

        var outputFileName = $"{newFileNameWithoutExtension}{ext}";
        var outputFilePath = Path.Combine(Path.GetDirectoryName(inputPath)!, outputFileName);


        // post process
        return outputFilePath;
    }

    /// <summary>
    /// rename a file path.
    /// </summary>
    /// <param name="inputPath"></param>
    /// <param name="replaceSets">Replace strings in original file name</param>
    /// <returns></returns>
    public static string GetRenamedPath(this string inputPath, params (string from, string to)[] replaceSets) => GetRenamedPath(inputPath, "*", replaceSets);

    /// <summary>
    /// rename a file path
    /// </summary>
    /// <param name="inputPath"></param>
    /// <param name="newFileNameWithoutExtension">"*" will be replaced with old name</param>
    /// <param name="replaceSets">replace strings in original file name</param>
    /// <returns></returns>
    public static string GetExtensionChangedPath(this string inputPath, string newExtension)
    {
        // main
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(inputPath);
        newExtension = newExtension.TrimStart('.');

        var outputFileName = $"{fileNameWithoutExtension}.{newExtension}";
        var outputFilePath = Path.Combine(Path.GetDirectoryName(inputPath)!, outputFileName);


        // post process
        return outputFilePath;
    }

}
