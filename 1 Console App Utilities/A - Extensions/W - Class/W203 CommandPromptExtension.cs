using System.Diagnostics;

namespace Aki32_Utilities.Extensions;
public static class CommandPromptExtension
{

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
    public static string Execute(
        bool realTimeConsoleWriteLineOutput = true,
        bool omitCurrentDirectoryDisplay = false,
        Action<string>? outputReceivedAction = null,
        Action<string>? errorReceivedAction = null,
        params string[] commands
        )
    {
        // preprocess
        var results = new List<string>();

        // main
        var processStartInfo = new ProcessStartInfo()
        {
            FileName = Environment.GetEnvironmentVariable("ComSpec"),
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };

        var process = new Process { StartInfo = processStartInfo };
        process.OutputDataReceived += (object _, DataReceivedEventArgs e) =>
        {
            var data = e?.Data ?? "";

            if (omitCurrentDirectoryDisplay)
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

            if (realTimeConsoleWriteLineOutput)
                Console.WriteLine(data);

            outputReceivedAction?.Invoke(data);

            results.Add(data);
        };
        process.ErrorDataReceived += (object _, DataReceivedEventArgs e) =>
        {
            var data = e?.Data ?? "";

            if (omitCurrentDirectoryDisplay)
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

            if (realTimeConsoleWriteLineOutput)
                ConsoleExtension.WriteLineWithColor(data, foreground: ConsoleColor.Red);

            errorReceivedAction?.Invoke(data);

            results.Add(data);
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        if (realTimeConsoleWriteLineOutput)
            ConsoleExtension.WriteLineWithColor("\r\n★ Command Prompt Output Started\r\n", foreground: ConsoleColor.Magenta);

        var sw = process.StandardInput;
        if (sw.BaseStream.CanWrite)
        {
            foreach (var command in commands)
            {
                Thread.Sleep(20); // for correct output and error order
                sw.WriteLine(command);
            }
            sw.WriteLine("exit");
        }
        sw.Close();

        process.WaitForExit();
        process.Close();

        if (realTimeConsoleWriteLineOutput)
            ConsoleExtension.WriteLineWithColor("\r\n★ Command Prompt Output Finished\r\n", foreground: ConsoleColor.Magenta);

        return string.Join("\r\n", results);
    }
}