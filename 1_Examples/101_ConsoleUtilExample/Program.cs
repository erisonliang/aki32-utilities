using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.UsageExamples.ConsoleAppUtilities;
public partial class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine();
        ConsoleExtension.WriteLineWithColor($"★ Process Started!", ConsoleColor.Yellow);
        Console.WriteLine();


        UtilConfig.ReadEnvConfig(new FileInfo(@"C:\Users\aki32\Dropbox\Codes\# SharedData\MyEnvConfig.json"));
        ExampleExecuter.All();


        Console.WriteLine();
        ConsoleExtension.WriteLineWithColor($"★ Process Finished!", ConsoleColor.Yellow);
        Console.WriteLine();

        Console.ReadLine();
    }
}