﻿using System.Runtime.CompilerServices;
using System.Text;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    /// <summary>
    /// Turn file-utility-methods into loop method
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.FullName}/{methodName}</param>
    /// <returns></returns>
    public static DirectoryInfo Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, Action<FileInfo, FileInfo> targetAction,
        string[] searchRegexen = null,
        SearchOption targetFilesOption = SearchOption.TopDirectoryOnly,
        [CallerMemberName] string methodName = "",
        FileInfo overrideOutputFile = null
        )
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, true, inputDir, methodName: methodName);
        searchRegexen ??= new string[] { ".*" };


        // main
        var inputFiles = inputDir.GetFilesWithRegexen(targetFilesOption, searchRegexen);
        foreach (var inputFile in inputFiles)
        {
            try
            {
                var outputFile = overrideOutputFile ?? new FileInfo(Path.Combine(outputDir!.FullName, inputFile.Name));
                targetAction(inputFile, outputFile);

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


}
