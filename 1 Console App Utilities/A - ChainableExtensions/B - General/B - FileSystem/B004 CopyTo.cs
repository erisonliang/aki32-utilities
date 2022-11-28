

namespace Aki32_Utilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// copy the entire directory
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">must not to be null</param>
    /// <returns></returns>
    public static DirectoryInfo CopyTo(this DirectoryInfo inputDir, DirectoryInfo? outputDir,
        bool consoleOutput = true,
        bool overwriteExistingFile = true
        )
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, inputDir.Parent!, consoleOutput);


        // main
        outputDir!.Attributes = inputDir.Attributes;

        // copy files
        foreach (FileInfo fileInfo in inputDir.GetFiles())
            fileInfo.CopyTo(Path.Combine(outputDir.FullName, fileInfo.Name), overwrite: overwriteExistingFile);

        // copy directories
        foreach (var inner_inputDir in inputDir.GetDirectories())
            inner_inputDir.CopyTo(new DirectoryInfo(Path.Combine(outputDir.FullName, inner_inputDir.Name)),
                consoleOutput: false,
                overwriteExistingFile: overwriteExistingFile
                );


        // postprocess
        return outputDir;
    }

    /// <summary>
    /// copy a file
    /// ※ CopyTo(null) induce default CopyTo(string)... You can use CopyTo() here.
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile"></param>
    /// <returns></returns>
    public static FileInfo CopyTo(this FileInfo inputFile, FileInfo? outputFile,
        bool overwriteExistingFile = true
        )
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputFile.Directory!, inputFile.Name);


        // main
        inputFile.CopyTo(outputFile!.FullName, overwrite: overwriteExistingFile);


        // post process
        return outputFile;
    }

    /// <summary>
    /// Syntax sugar for default CopyTo() with inferring output path
    /// </summary>
    /// <param name="inputFile"></param>
    /// <returns></returns>
    public static FileInfo CopyTo(this FileInfo inputFile,
        bool overwriteExistingFile = true
        )
    {
        return inputFile.CopyTo(
            outputFile: null,
            overwriteExistingFile: overwriteExistingFile);
    }

}
