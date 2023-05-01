using System.Configuration;

namespace Aki32Utilities.ConsoleAppUtilities.CheatSheet;
internal class AppConfigの利用
{
    internal static void all()
    {
        var name = ConfigurationManager.AppSettings["Name"];
        var age = ConfigurationManager.AppSettings["Age"];
        Console.WriteLine($"{name}, {age}");
    }
}
