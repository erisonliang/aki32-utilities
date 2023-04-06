using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Aki32Utilities.ConsoleAppUtilities.General;

using DocumentFormat.OpenXml.Presentation;

namespace Aki32Utilities.ConsoleAppUtilities.Research;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ Z999 TEMPLATE

    /// <summary>
    /// TEMPLATE
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <returns></returns>
    public static FileInfo TEMPLATE(this FileInfo inputFile, FileInfo? outputFile)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputFile.Directory!, inputFile.Name);


        // main




        // post process
        return outputFile!;
    }

    /// <summary>
    /// TEMPLATE
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <returns></returns>
    public static DirectoryInfo TEMPLATE_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir)
        => inputDir.Loop(outputDir, (inF, outF) => inF.TEMPLATE(outF),
            maxDegreeOfParallelism: 1);


    /// <summary>
    /// TEMPLATE
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <returns></returns>
    public static DirectoryInfo TEMPLATE_Loop_Manually(this DirectoryInfo inputDir, DirectoryInfo? outputDir)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, inputDir);
        var maxRetryCount = 10;
        var maxDegreeOfParallelism = 10;

        // main
        var inputFiles = inputDir.GetFiles();
        var option = new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism };

        using var progress = new ProgressManager(inputFiles.Count());
        progress.StartAutoWrite(100);

        Parallel.ForEach(inputFiles, option, inputFile =>
        {
            try
            {
                var retryCount = 0;
                while (true)
                {
                    try
                    {
                        var outputFile = outputDir!.GetChildFileInfo(inputFile.Name);
                        inputFile.TEMPLATE(outputFile);
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (++retryCount < maxRetryCount)
                        {
                            Thread.Sleep(100);
                            GC.Collect();
                            continue;
                        }
                        progress.AddErrorMessage($"{inputFile.FullName}, {ex.Message}");
                        break;
                    }
                }
            }
            finally
            {
                progress.CurrentStep++;
            }
        });

        progress.WriteDone();


        // post process
        return outputDir!;
    }


    // ★★★★★★★★★★★★★★★

}
