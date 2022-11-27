
namespace Aki32_Utilities.ChainableExtensions.General;
public class ConsoleExtension
{

    /// <summary>
    /// Clear all contents in the last line of Console
    /// </summary>
    public static void ClearCurrentConsoleLine()
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        //Console.Write(new string(' ', currentLineCursor));
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, currentLineCursor);
    }

    /// <summary>
    /// ConsoleWriteLine with Colors
    /// </summary>
    /// <param name="value"></param>
    /// <param name="foreground"></param>
    /// <param name="backgroud"></param>
    public static void WriteWithColor(object value, ConsoleColor foreground = ConsoleColor.Green, ConsoleColor backgroud = ConsoleColor.Black)
    {
        Console.ForegroundColor = foreground;
        Console.BackgroundColor = backgroud;
        Console.Write(value);
        Console.ResetColor();
    }

    /// <summary>
    /// ConsoleWriteLine with Colors
    /// </summary>
    /// <param name="value"></param>
    /// <param name="foreground"></param>
    /// <param name="backgroud"></param>
    public static void WriteLineWithColor(object value, ConsoleColor foreground = ConsoleColor.Green, ConsoleColor backgroud = ConsoleColor.Black)
    {
        WriteWithColor($"{value}\r\n", foreground, backgroud);
    }

}