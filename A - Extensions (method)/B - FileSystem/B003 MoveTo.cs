

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    /// <summary>
    /// move the entire directory
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">must not to be null</param>
    /// <returns></returns>
    public static DirectoryInfo MoveTo(this DirectoryInfo inputDir, DirectoryInfo outputDir)
    {
        // preprocess
        if (outputDir is null)
            throw new ArgumentNullException(nameof(outputDir));
        UtilPreprocessors.PreprocessOutDir(outputDir, "MoveTo", true, null); // outputDir.Parent ??


        // main
        if (inputDir.FullName[0..3] == outputDir.FullName[0..3])
        {
            // use default MoveTo().
            inputDir.MoveTo(outputDir.FullName);
        }
        else
        {
            // We can't use MoveTo() for different drive.
            inputDir.CopyTo(outputDir, true);
            inputDir.Delete(true);
        }


        // postprocess
        return outputDir;
    }

    /// <summary>
    /// move a file
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile"></param>
    /// <returns></returns>
    public static FileInfo MoveTo(this FileInfo inputFile, FileInfo outputFile)
    {
        // preprocess
        if (outputFile is null)
            throw new ArgumentNullException(nameof(outputFile));
        UtilPreprocessors.PreprocessOutFile(outputFile, "MoveTo", false, null, null);


        // main
        if (inputFile.FullName[0..3] == outputFile.FullName[0..3])
        {
            // use default Move().
            File.Move(inputFile.FullName, outputFile.FullName, true);
        }
        else
        {
            // We can't use Move() for different drive.
            File.Copy(inputFile.FullName, outputFile.FullName, true);
            File.Delete(inputFile.FullName);
        }


        return outputFile;
    }

    /// <summary>
    /// move a file
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputDir">must not to be null</param>
    /// <returns></returns>
    public static FileInfo MoveTo(this FileInfo inputFile, DirectoryInfo outputDir)
    {
        // preprocess
        if (outputDir is null)
            throw new ArgumentNullException(nameof(outputDir));
        if (!outputDir.Exists) outputDir.Create();
        UtilPreprocessors.PreprocessOutDir(outputDir, "MoveTo", false, null);


        // main
        var name = inputFile.Name;
        var outputFilePath = Path.Combine(outputDir.FullName, name);
        var outputFile = new FileInfo(outputFilePath);
        File.Move(inputFile.FullName, outputFile.FullName, true);


        return outputFile;
    }

}
