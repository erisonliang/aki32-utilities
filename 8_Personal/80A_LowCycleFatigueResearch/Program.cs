using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.Personal.LowCycleFatigueResearch;
public partial class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine();
        ConsoleExtension.WriteLineWithColor($"★ Process Started!", ConsoleColor.Yellow);
        Console.WriteLine();
        
        UtilConfig.ReadEnvConfig(new FileInfo(@"C:\Users\aki32\Dropbox\Codes\# SharedData\MyEnvConfig.json"));
        //var line = new LINEController(""); // 送らない
        var line = new LINEController(Environment.GetEnvironmentVariable("LINE_AccessToken__General")!); // LINE Notify 全般
        _ = line.SendMessageAsync(@"処理開始");



        Executer.All(line);


        
        _ = line.SendMessageAsync(@"全ての処理終了");

        Console.WriteLine();
        ConsoleExtension.WriteLineWithColor($"★ Process Finished!", ConsoleColor.Yellow);
        Console.WriteLine();

        Console.ReadLine();
    }
}