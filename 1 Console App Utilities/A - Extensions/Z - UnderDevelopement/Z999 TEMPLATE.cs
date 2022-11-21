using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    // ★★★★★★★★★★★★★★★ 999 TEMPLATE

    /// <summary>
    /// TEMPLATE
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set to {inputFile.DirectoryName}/output_TEMPLATE/{inputFile.Name}</param>
    /// <returns></returns>
    public static FileInfo TEMPLATE(this FileInfo inputFile, FileInfo? outputFile)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, false, inputFile.Directory!, inputFile.Name);


        // main




        // post process
        return outputFile!;
    }

    /// <summary>
    /// TEMPLATE
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.FullName}/output_TEMPLATE</param>
    /// <returns></returns>
    public static DirectoryInfo TEMPLATE_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir)
        => inputDir.Loop(outputDir, (inF, outF) => TEMPLATE(inF, outF));

    /// <summary>
    /// TEMPLATE
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.FullName}/output_TEMPLATE</param>
    /// <returns></returns>
    public static DirectoryInfo TEMPLATE_Loop_Manually(this DirectoryInfo inputDir, DirectoryInfo? outputDir)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, true, inputDir);


        // main
        var inputFiles = inputDir.GetFiles();
        foreach (var inputFile in inputFiles)
        {
            try
            {
                var outputFile = new FileInfo(Path.Combine(outputDir!.FullName, inputFile.Name));
                inputFile.TEMPLATE(outputFile);

                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"O: {inputFile.FullName}");
            }
            catch (Exception ex)
            {
                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"X: {inputFile.FullName}, {ex.Message}");
            }
        }


        // post process
        return outputDir!;
    }


    // ★★★★★★★★★★★★★★★

}
