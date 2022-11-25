using System.Diagnostics;

namespace Aki32_Utilities.Extensions;
public class CommandPrompt : IDisposable
{
    public bool RealTimeConsoleWriteLineOutput { get; set; } = false;
    public bool OmitCurrentDirectoryDisplay { get; set; } = false;

    public Action<string>? OutputReceivedAction { get; set; } = null;
    public Action<string>? ErrorReceivedAction { get; set; } = null;

    public Process CommandPromptProcess { get; set; }
    public StreamWriter InputStream => CommandPromptProcess.StandardInput;
    public List<string> ResponseList { get; set; } = new List<string>();

    public CommandPrompt()
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

        CommandPromptProcess = new Process { StartInfo = processStartInfo };
        CommandPromptProcess.OutputDataReceived += (object _, DataReceivedEventArgs e) =>
        {
            var data = e?.Data ?? "";

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

            if (RealTimeConsoleWriteLineOutput)
                Console.WriteLine(data);

            OutputReceivedAction?.Invoke(data);

            ResponseList.Add(data);
        };
        CommandPromptProcess.ErrorDataReceived += (object _, DataReceivedEventArgs e) =>
        {
            var data = e?.Data ?? "";

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

            if (RealTimeConsoleWriteLineOutput)
                ConsoleExtension.WriteLineWithColor(data, foreground: ConsoleColor.Red);

            ErrorReceivedAction?.Invoke(data);

            ResponseList.Add(data);
        };

        CommandPromptProcess.Start();
        CommandPromptProcess.BeginOutputReadLine();
        CommandPromptProcess.BeginErrorReadLine();

        if (RealTimeConsoleWriteLineOutput)
            ConsoleExtension.WriteLineWithColor("\r\n★ Command Prompt Started\r\n", foreground: ConsoleColor.Magenta);

    }

    public void WriteLine(string command)
    {
        // for correct output and error order
        Thread.Sleep(10);
        InputStream.WriteLine(command);
    }

    public void Dispose()
    {
        InputStream.WriteLine("exit");
        InputStream.Close();

        CommandPromptProcess.WaitForExit();
        CommandPromptProcess.Close();

        if (RealTimeConsoleWriteLineOutput)
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
        using var prompt = new CommandPrompt
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