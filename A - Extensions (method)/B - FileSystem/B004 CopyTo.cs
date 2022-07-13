

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    /// <summary>
    /// copy the entire directory
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">must not to be null</param>
    /// <returns></returns>
    public static DirectoryInfo CopyTo(this DirectoryInfo inputDir, DirectoryInfo outputDir, bool consoleOutput = true)
    {
        // preprocess
        if (outputDir is null)
            throw new ArgumentNullException(nameof(outputDir));
        UtilPreprocessors.PreprocessOutDir(ref outputDir, "CopyTo", consoleOutput, null);


        // main
        // create new directory with the same attribtues
        if (!outputDir.Exists)
        {
            outputDir.Create();
            outputDir.Attributes = inputDir.Attributes;
        }

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
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile"></param>
    /// <returns></returns>
    public static FileInfo CopyTo(this FileInfo inputFile, FileInfo outputFile)
    {
        // preprocess
        if (outputFile is null)
            throw new ArgumentNullException(nameof(outputFile));
        UtilPreprocessors.PreprocessOutFile(ref outputFile, "CopyTo", false, null, null);


        // main
        inputFile.CopyTo(outputFile.FullName, true);


        // post process
        return outputFile;
    }

}
