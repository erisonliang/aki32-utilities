

namespace Aki32_Utilities.Extensions;
public class ConsoleExtension
{

    /// <summary>
    /// Clear contents in the last line of Console
    /// </summary>
    public static void ClearCurrentConsoleLine()
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', currentLineCursor)); //Console.WindowWidth
        Console.SetCursorPosition(0, currentLineCursor);
    }

}
