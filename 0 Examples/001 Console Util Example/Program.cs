using Aki32Utilities.ConsoleAppUtilities.UsefulClasses;

namespace Aki32Utilities.UsageExamples.ConsoleAppUtilities;
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