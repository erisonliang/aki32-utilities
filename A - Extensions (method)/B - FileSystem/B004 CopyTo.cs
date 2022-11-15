

using static Microsoft.FSharp.Core.ByRefKinds;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    /// <summary>
    /// copy the entire directory
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">must not to be null</param>
    /// <returns></returns>
    public static DirectoryInfo CopyTo(this DirectoryInfo inputDir, DirectoryInfo? outputDir, bool consoleOutput = true)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, consoleOutput, inputDir.Parent!);


        // main
        outputDir.Attributes = inputDir.Attributes;

        // copy files (with overriding existing files)
        foreach (FileInfo fileInfo in inputDir.GetFiles())
            fileInfo.CopyTo(outputDir.FullName + @"\" + fileInfo.Name, true);

        // copy directories
        foreach (var inner_inputDir in inputDir.GetDirectories())
            inner_inputDir.CopyTo(new DirectoryInfo(Path.Combine(outputDir.FullName, inner_inputDir.Name)), false);


        // postprocess
        return outputDir;
    }

    /// <summary>
    /// copy a file
    /// ※ CopyTo(null) call default CopyTo... You can use CopyTo() here.
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile"></param>
    /// <returns></returns>
    public static FileInfo CopyTo(this FileInfo inputFile, FileInfo? outputFile)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, false, inputFile.Directory!, inputFile.Name);


        // main
        inputFile.CopyTo(outputFile!.FullName, true);


        // post process
        return outputFile;
    }

    /// <summary>
    /// copy a file, infer output path
    /// </summary>
    /// <param name="inputFile"></param>
    /// <returns></returns>
    public static FileInfo CopyTo(this FileInfo inputFile)
    {
        return inputFile.CopyTo(outputFile: null);
    }

}
