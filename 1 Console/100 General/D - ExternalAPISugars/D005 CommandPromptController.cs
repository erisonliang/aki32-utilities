using System.Diagnostics;

using Aki32Utilities.ConsoleAppUtilities.UsefulClasses;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public class CommandPromptController : IDisposable
{
    public bool RealTimeConsoleWriteLineOutput { get; set; }
    public bool OmitCurrentDirectoryDisplay { get; set; } = false;

    public Action<string>? OutputReceivedAction { get; set; } = null;
    public Action<string>? ErrorReceivedAction { get; set; } = null;

    public Process CommandPromptProcess { get; set; }
    public StreamWriter InputStream => CommandPromptProcess.StandardInput;
    public List<string> ResponseList { get; set; } = new List<string>();


    public CommandPromptController()
    {
        var processStartInfo = new ProcessStartInfo()
        {
            FileName = Environment.GetEnvironmentVariable("ComSpec"),
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };

        var OmitCurrentDirectoryString = (string data) =>
        {
            if (OmitCurrentDirectoryDisplay)
            {
                try
                {
                    if (data[1] == ':')
                        data = data.Substring(data.IndexOf(">"));
                }
                catch (Exception)
                {
                }
            }

            return data;
        };
        CommandPromptProcess = new Process { StartInfo = processStartInfo };
        CommandPromptProcess.OutputDataReceived += (object _, DataReceivedEventArgs e) =>
        {
            var data = e?.Data ?? "";
            data = OmitCurrentDirectoryString(data);

            if (RealTimeConsoleWriteLineOutput)
                Console.WriteLine(data);

            OutputReceivedAction?.Invoke(data);

            ResponseList.Add(data);
        };
        CommandPromptProcess.ErrorDataReceived += (object _, DataReceivedEventArgs e) =>
        {
            var data = e?.Data ?? "";
            data = OmitCurrentDirectoryString(data);

            if (RealTimeConsoleWriteLineOutput)
                ConsoleExtension.WriteLineWithColor(data, foreground: ConsoleColor.Red);

            ErrorReceivedAction?.Invoke(data);

            ResponseList.Add(data);
        };

        if (UtilConfig.ConsoleOutput_Contents)
            ConsoleExtension.WriteLineWithColor("\r\n★ Command Prompt Started\r\n", foreground: ConsoleColor.Magenta);

        CommandPromptProcess.Start();
        CommandPromptProcess.BeginOutputReadLine();
        CommandPromptProcess.BeginErrorReadLine();

    }

    public void WriteLine(string command)
    {
        // for correct output and error order
        Thread.Sleep(10);
        InputStream.WriteLine(command);
    }

    public void Wait()
    {
        InputStream.WriteLine(@"echo wait flag");
        while (true)
        {
            try
            {
                Thread.Sleep(10);
                if (ResponseList[^1] == "wait flag")
                    break;
                if (ResponseList[^2] == "wait flag")
                    break;
            }
            catch (Exception)
            {
            }
        }
    }

    public void Dispose()
    {
        InputStream.WriteLine("exit");
        InputStream.Close();

        CommandPromptProcess.WaitForExit();
        CommandPromptProcess.Close();

        if (UtilConfig.ConsoleOutput_Contents)
            ConsoleExtension.WriteLineWithColor("\r\n★ Command Prompt Closed\r\n", foreground: ConsoleColor.Magenta);

        GC.SuppressFinalize(this);
    }


    /// <summary>
    /// execute commands in command prompt
    /// </summary>
    /// <param name="realTimeConsoleWriteLineOutput"></param>
    /// <param name="outputReceivedAction"></param>
    /// <param name="commands"></param>
    /// <remarks>
    /// ref: https://dobon.net/vb/dotnet/process/standardoutput.html
    /// </remarks>
    /// <returns></returns>
    public static string[] Execute(
        bool realTimeConsoleWriteLineOutput = true,
        bool omitCurrentDirectoryDisplay = false,
        Action<string>? outputReceivedAction = null,
        Action<string>? errorReceivedAction = null,
        params string[] commands
        )
    {
        using var prompt = new CommandPromptController
        {
            RealTimeConsoleWriteLineOutput = realTimeConsoleWriteLineOutput,
            OmitCurrentDirectoryDisplay = omitCurrentDirectoryDisplay,
            OutputReceivedAction = outputReceivedAction,
            ErrorReceivedAction = errorReceivedAction,
        };

        foreach (var command in commands)
            prompt.WriteLine(command);

        return prompt.ResponseList.ToArray();

    }

}