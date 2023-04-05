using System.Text;

using Newtonsoft.Json.Linq;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static class UtilConfig
{

    public static bool IncludeGuidToNewOutputDirName = true;

    public static bool ConsoleOutput_Contents = true;
    public static bool ConsoleOutput_Preprocess = true;
    public static Stack<bool> Queue_Stop_ConsoleOutput_Preprocess { get; set; } = new Stack<bool>();
    public static Stack<bool> Queue_Stop_ConsoleOutput_Contents { get; set; } = new Stack<bool>();

    public static int OutputPathMethodNameMaxLength = 10;

    public static void StopTemporary_ConsoleOutput_Preprocess()
    {
        Queue_Stop_ConsoleOutput_Preprocess.Push(ConsoleOutput_Preprocess);
        ConsoleOutput_Preprocess = false;
    }
    public static void TryRestart_ConsoleOutput_Preprocess()
    {
        ConsoleOutput_Preprocess = Queue_Stop_ConsoleOutput_Preprocess.TryPop(out var dequeued) ? dequeued : true;
    }

    public static void StopTemporary_ConsoleOutput_Contents()
    {
        Queue_Stop_ConsoleOutput_Contents.Push(ConsoleOutput_Contents);
        ConsoleOutput_Contents = false;
    }
    public static void TryRestart_ConsoleOutput_Contents()
    {
        ConsoleOutput_Contents = Queue_Stop_ConsoleOutput_Contents.TryPop(out var dequeued) ? dequeued : true;
    }

    /// <summary>
    /// Read json file as environment variables. Json format is as follows
    /// <code>
    /// [
    ///    {"key" : "GetHub_Username", "value" : "aki32"},
    ///    {"key" : "GetHub_Password", "value" : "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"},
    ///    {"key" : "OpenAI_SecretKey", "value" : "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"}  
    ///    {"key" : "SomeService_APIKey", "value" : "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"}  
    /// ]
    /// </code>
    /// </summary>
    public static void ReadEnvConfig(FileInfo input)
    {
        try
        {
            if (!input.Exists)
                return;

            var envs = input.ReadObjectFromLocalJson<JArray>();
            foreach (var env in envs)
                Environment.SetEnvironmentVariable(env["key"]!.ToString(), env["value"]!.ToString(), EnvironmentVariableTarget.Process);
        }
        catch (Exception)
        {
        }
    }

}
