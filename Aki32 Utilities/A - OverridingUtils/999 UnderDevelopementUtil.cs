using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aki32_Utilities.OverridingUtils;
public static class UnderDevelopementUtil
{

    // ★★★★★★★★★★★★★★★


    // ★★★★★★★★★★★★★★★


    // ★★★★★★★★★★★★★★★


    // ★★★★★★★★★★★★★★★ 999 TEMPLATE

    /// <summary>
    /// TEMPLATE
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set to {inputFile.DirectoryName}/output_TEMPLATE/{inputFile.Name}</param>
    /// <param name="skipColumnCount"></param>
    /// <param name="skipRowCount"></param>
    /// <param name="header"></param>
    /// <returns></returns>
    public static FileInfo TEMPLATE(this FileInfo inputFile, FileInfo? outputFile, int skipColumnCount = 0, int skipRowCount = 0, string header = null)
    {
        // preprocess
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // to handle Shift-JIS
        if (outputFile is null)
            outputFile = new FileInfo(Path.Combine(inputFile.DirectoryName, "output_TEMPLATE", inputFile.Name));
        if (!outputFile.Directory.Exists) outputFile.Directory.Create();
        if (outputFile.Exists) outputFile.Delete();


        // main
        var inputCsv = inputFile.ReadCsv_Rows(skipColumnCount, skipRowCount);


        // post process
        return outputFile;
    }
    /// <summary>
    /// TEMPLATE
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.FullName}/output_TEMPLATE</param>
    /// <param name="skipColumnCount"></param>
    /// <param name="skipRowCount"></param>
    /// <param name="header"></param>
    /// <returns></returns>
    public static DirectoryInfo TEMPLATE_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, int skipColumnCount = 0, int skipRowCount = 0, string header = null)
    {
        // preprocess
        if (UtilConfig.ConsoleOutput)
            Console.WriteLine("\r\n** TEMPLATE_Loop() Called");
        if (outputDir is null)
            outputDir = new DirectoryInfo(Path.Combine(inputDir.FullName, "output_TEMPLATE"));
        if (!outputDir.Exists) outputDir.Create();


        // main
        foreach (var file in inputDir.GetFiles())
        {
            var newFilePath = Path.Combine(outputDir.FullName, file.Name);
            try
            {
                file.TEMPLATE(new FileInfo(newFilePath), skipColumnCount, skipRowCount, header);
                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"O: {newFilePath}");
            }
            catch (Exception ex)
            {
                if (UtilConfig.ConsoleOutput)
                    Console.WriteLine($"X: {newFilePath}, {ex.Message}");
            }
        }


        // post process
        return outputDir;
    }

    // ★★★★★★★★★★★★★★★

}
