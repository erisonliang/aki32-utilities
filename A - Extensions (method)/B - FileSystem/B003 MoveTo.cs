

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
        var outputParentDir = outputDir!.Parent; // abnormal, only for dir.MoveTo(dir) 
        UtilPreprocessors.PreprocessOutDir(ref outputParentDir, "MoveTo", true, null);


        // main
        if (inputDir.FullName[0..3] == outputDir.FullName[0..3])
        {
            // use default MoveTo().
            inputDir.MoveTo(outputDir.FullName);
        }
        else
        {
            // For different drive, we can't use default Move().
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
        UtilPreprocessors.PreprocessOutFile(ref outputFile!, "MoveTo", false, null, null);


        // main
        if (inputFile.FullName[0..3] == outputFile.FullName[0..3])
        {
            // use default Move().
            File.Move(inputFile.FullName, outputFile.FullName, true);
        }
        else
        {
            // For different drive, we can't use default Move().
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


        // main
        var outputFile = new FileInfo(Path.Combine(outputDir.FullName, inputFile.Name));
        inputFile.MoveTo(outputFile);


        return outputFile;
    }

}
