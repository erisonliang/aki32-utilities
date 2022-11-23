using Aki32_Utilities.Extensions;
using Aki32_Utilities.OwesomeModels;
using Aki32_Utilities.ExternalAPIControllers;

using System.Drawing;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Aki32_Utilities;
public partial class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine();
        ConsoleExtension.WriteLineWithColor($"★ Process Started!", ConsoleColor.Yellow);
        Console.WriteLine();


        Test.All();


        Console.WriteLine();
        ConsoleExtension.WriteLineWithColor($"★ Process Finished!", ConsoleColor.Yellow);
        Console.WriteLine();

        Console.ReadLine();
    }

}