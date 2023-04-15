

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ main

    /// <summary>
    /// Transpose csv columns and rows and save as file
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <returns></returns>
    public static FileInfo TransposeCsv(this FileInfo inputFile, FileInfo? outputFile)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputFile.Directory!, inputFile.Name);


        // main
        var csv = ReadCsv_Rows(inputFile);
        csv.SaveCsv_Columns(outputFile!);


        // post process
        return outputFile!;
    }


    // ★★★★★★★★★★★★★★★ loop sugar

    /// <summary>
    /// Transpose csv columns and rows and save as file for all files in dir
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <param name="extractingColumns"></param>
    /// <param name="skipRowCount"></param>
    /// <param name="header"></param>
    /// <returns></returns>
    public static DirectoryInfo TransposeCsv_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir)
        => inputDir.Loop(outputDir, (inF, outF) => inF.TransposeCsv(outF));


    // ★★★★★★★★★★★★★★★ 

}
