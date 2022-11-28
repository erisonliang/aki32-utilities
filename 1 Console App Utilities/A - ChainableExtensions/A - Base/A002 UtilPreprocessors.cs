using System.Runtime.CompilerServices;
using System.Text;

namespace Aki32_Utilities.General;
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
        outputDirReference ??= new DirectoryInfo(
            Path.Combine(
                targetOutputDirWhenNull.FullName,
                GetNewOutputDirName(methodName)
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
        outputFileReference ??= new FileInfo(
            Path.Combine(
                targetOutputDirWhenNull.FullName!,
                GetNewOutputDirName(methodName),
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
        if (UtilConfig.ConsoleOutput_Preprocess && consoleOut)
            ConsoleExtension.WriteLineWithColor($"\r\n\r\n** {methodName} Method Called{(takesTimeFlag ? " (* This method is time-consuming. Please be patient...)" : "")}");

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // to handle Shift-JIS
    }

    /// <summary>
    /// For creating new output dir name
    /// </summary>
    /// <param name="MethodName"></param>
    /// <returns></returns>
    public static string GetNewOutputDirName(string MethodName)
    {
        var randomGuid = Guid.NewGuid().ToString()[0..6].ToUpper();
        if (UtilConfig.IncludeGuidToNewOutputDirName)
            return $"output_{MethodName}_{randomGuid}";
        else
            return $"output_{MethodName}";
    }

}
