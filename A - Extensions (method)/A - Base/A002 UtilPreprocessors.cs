﻿using System.Runtime.CompilerServices;
using System.Text;

namespace Aki32_Utilities.Extensions;
public static class UtilPreprocessors
{
    public static void PreprocessOutDir(
        ref DirectoryInfo? outputDirReference,
        bool consoleOut,
        DirectoryInfo targetOutputDirWhenNull,
        [CallerMemberName] string methodName = ""
        )
    {
        PreprocessBasic(methodName, consoleOut);

        // outputDir initialize
        if (outputDirReference is null)
            outputDirReference = new DirectoryInfo(
                Path.Combine(
                    targetOutputDirWhenNull.FullName,
                    UtilConfig.GetNewOutputDirName(methodName)
                    )
                );
        if (!outputDirReference.Exists)
            outputDirReference.Create();

    }

    public static void PreprocessOutFile(
          ref FileInfo? outputFileReference,
          bool consoleOut,
          DirectoryInfo targetOutputDirWhenNull,
          string fileNameCandidate,
          [CallerMemberName] string methodName = ""
          )
    {
        PreprocessBasic(methodName, consoleOut);

        // outputFile initialize
        if (outputFileReference is null)
            outputFileReference = new FileInfo(
                Path.Combine(
                    targetOutputDirWhenNull.FullName!,
                    UtilConfig.GetNewOutputDirName(methodName),
                    fileNameCandidate
                    )
                );
        if (!outputFileReference.Directory!.Exists)
            outputFileReference.Directory.Create();

        if (outputFileReference.Exists) outputFileReference.Delete();
    }

    public static void PreprocessBasic(string methodName, bool consoleOut)
    {
        // ConsoleOut
        if (UtilConfig.ConsoleOutput && consoleOut)
            Console.WriteLine($"\r\n** {methodName} Method Called");

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // to handle Shift-JIS
    }

}
