using Aki32_Utilities.Console_App_Utilities.UsageExamples;
using Aki32_Utilities.Console_App_Utilities.UsefulClasses;

namespace Aki32_Utilities;
public partial class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine();
        ConsoleExtension.WriteLineWithColor($"★ Process Started!", ConsoleColor.Yellow);
        Console.WriteLine();


        TestHelper.All();


        Console.WriteLine();
        ConsoleExtension.WriteLineWithColor($"★ Process Finished!", ConsoleColor.Yellow);
        Console.WriteLine();

        Console.ReadLine();
    }

}