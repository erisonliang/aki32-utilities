using System.IO.Compression;

namespace Aki32_Utilities.ChainableExtensions;
public static partial class ChainableExtensions
{

    /// <summary>
    /// Zip
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <returns></returns>
    public static FileInfo Zip(this DirectoryInfo inputDir, FileInfo? outputFile)
    {
        //// preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, true, inputDir.Parent!, "output.zip");


        // main
        ZipFile.CreateFromDirectory(inputDir.FullName, outputFile.FullName);


        // post process
        return outputFile!;
    }


    /// <summary>
    /// Unzip
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <returns></returns>
    public static DirectoryInfo Unzip(this FileInfo inputFile, DirectoryInfo? outputDir)
    {
        //// preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, true, inputFile.Directory);


        // main
        ZipFile.ExtractToDirectory(inputFile.FullName, outputDir.FullName, true);


        // post process
        return outputDir!;
    }
}
