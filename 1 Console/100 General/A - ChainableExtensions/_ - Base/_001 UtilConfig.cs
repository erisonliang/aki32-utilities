using System.Text;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static class UtilConfig
{

    public static bool IncludeGuidToNewOutputDirName = true;

    public static bool ConsoleOutput_Contents = true;
    public static bool ConsoleOutput_Preprocess = true;
    public static Stack<bool> Queue_Stop_ConsoleOutput_Preprocess { get; set; } = new Stack<bool>();

    public static void StopTemporary_ConsoleOutput_Preprocess()
    {
        Queue_Stop_ConsoleOutput_Preprocess.Push(ConsoleOutput_Preprocess);
        ConsoleOutput_Preprocess = false;
    }
    public static void TryRestart_ConsoleOutput_Preprocess()
    {
        ConsoleOutput_Preprocess = Queue_Stop_ConsoleOutput_Preprocess.TryPop(out var dequeued) ? dequeued : true;
    }

}
