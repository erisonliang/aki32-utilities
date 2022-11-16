using System.Runtime.CompilerServices;
using System.Text;

namespace Aki32_Utilities.Extensions;
public static class UtilPreprocessors
{
    public static void PreprocessOutDir(
        ref DirectoryInfo? outputDirReference,
        bool consoleOut,
        DirectoryInfo targetOutputDirWhenNull,
        [CallerMemberName] string methodName = "",
          bool takesTimeFlag = false
        )
    {
        PreprocessBasic(consoleOut, methodName: methodName, takesTimeFlag: takesTimeFlag);

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
          [CallerMemberName] string methodName = "",
          bool takesTimeFlag = false
          )
    {
        PreprocessBasic(consoleOut, methodName: methodName, takesTimeFlag: takesTimeFlag);

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

    public static void PreprocessBasic(bool consoleOut,
        [CallerMemberName] string methodName = "",
        bool takesTimeFlag = false
        )
    {
        // ConsoleOut
        if (UtilConfig.ConsoleOutput && consoleOut)
            Console.WriteLine($"\r\n** {methodName} Method Called{(takesTimeFlag ? " (* This is time-consuming method. Please be patient...)" : "")}");

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // to handle Shift-JIS
    }

}
