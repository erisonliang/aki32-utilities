using System.Runtime.CompilerServices;
using Aki32Utilities.ConsoleAppUtilities.General;
using System.Text;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static class UtilPreprocessors
{
    public static void PreprocessOutDir(
        ref DirectoryInfo? outputDirReference,
        DirectoryInfo targetOutputDirWhenNull,
        bool consoleOut = true,
        [CallerMemberName] string methodName = "",
        bool takesTimeFlag = false,
        bool deletingExistingOutputDir = false
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

        if (deletingExistingOutputDir && outputDirReference.Exists)
            outputDirReference.Delete(true);

        if (!outputDirReference.Exists)
            outputDirReference.Create();

    }

    public static void PreprocessOutFile(
          ref FileInfo? outputFileReference,
          DirectoryInfo targetOutputDirWhenNull,
          string fileNameCandidate,
          bool consoleOut = true,
          [CallerMemberName] string methodName = "",
          bool takesTimeFlag = false,
          bool deletingExistingOutputFile = true
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

        if (deletingExistingOutputFile && outputFileReference.Exists)
            outputFileReference.Delete();

        if (!outputFileReference.Directory!.Exists)
            outputFileReference.Directory.Create();

    }

    public static void PreprocessBasic(
        bool consoleOut = true,
        [CallerMemberName] string methodName = "",
        bool takesTimeFlag = false
        )
    {
        // ConsoleOut
        if (UtilConfig.ConsoleOutput_Preprocess && consoleOut)
            ConsoleExtension.WriteLineWithColor($"\r\n** {methodName} Method Called{(takesTimeFlag ? " (* This method is time-consuming. Please be patient...)" : "")}");

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // to handle Shift-JIS
    }

    public static bool ConfirmIfExecute(
        [CallerMemberName] string methodName = ""
        )
    {
        // ConsoleOut
        ConsoleExtension.WriteLineWithColor($"\r\nTrying to call strong {methodName} method. Are you sure? Enter \"yes\" to execute.", ConsoleColor.DarkYellow);
        var input = Console.ReadLine();
        if (input == "yes")
            return true;
        return false;
    }


    /// <summary>
    /// For creating new output dir name
    /// </summary>
    /// <param name="MethodName"></param>
    /// <returns></returns>
    public static string GetNewOutputDirName(string MethodName)
    {
        if (UtilConfig.IncludeGuidToNewOutputDirName)
        {
            var randomGuid = Guid.NewGuid().ToString()[0..6].ToUpper();
            return $"output_{MethodName.Shorten(0..UtilConfig.OutputPathMethodNameMaxLength)}_{randomGuid}";
        }
        else
        {
            return $"output_{MethodName.Shorten(0..UtilConfig.OutputPathMethodNameMaxLength)}";
        }
    }

}
