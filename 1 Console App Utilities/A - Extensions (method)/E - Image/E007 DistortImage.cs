using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    /// <summary>
    /// DistortImage
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set to {inputFile.DirectoryName}/output_DistortImage/{inputFile.Name}</param>
    /// <param name="skipColumnCount"></param>
    /// <param name="skipRowCount"></param>
    /// <param name="header"></param>
    /// <returns></returns>
    public static FileInfo DistortImage(this FileInfo inputFile, FileInfo? outputFile, params (Point inputPoint, Point outputPoint)[] ps)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, false, inputFile.Directory!, inputFile.Name);


        // main


        throw new NotImplementedException();

        // post process
        return outputFile!;
    }


    // ★★★★★★★★★★★★★★★ universal

    ///// <summary>
    ///// DistortImage
    ///// </summary>
    ///// <param name="inputDir"></param>
    ///// <param name="outputDir">when null, automatically set to {inputDir.FullName}/output_DistortImage</param>
    ///// <param name="skipColumnCount"></param>
    ///// <param name="skipRowCount"></param>
    ///// <param name="header"></param>
    ///// <returns></returns>
    //public static DirectoryInfo DistortImage_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, int skipColumnCount = 0, int skipRowCount = 0, string? header = null)
    //{
    //    // preprocess
    //    UtilPreprocessors.PreprocessOutDir(ref outputDir, true, inputDir);


    //    // main
    //    foreach (var file in inputDir.GetFiles())
    //    {
    //        var newFilePath = Path.Combine(outputDir!.FullName, file.Name);
    //        try
    //        {
    //            file.DistortImage(new FileInfo(newFilePath), skipColumnCount, skipRowCount, header);
    //            if (UtilConfig.ConsoleOutput)
    //                Console.WriteLine($"O: {newFilePath}");
    //        }
    //        catch (Exception ex)
    //        {
    //            if (UtilConfig.ConsoleOutput)
    //                Console.WriteLine($"X: {newFilePath}, {ex.Message}");
    //        }
    //    }


    //    // post process
    //    return outputDir!;
    //}

    // ★★★★★★★★★★★★★★★

}
