

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ sugar

    public static byte[] ReadAllBytes(this FileInfo inputFile) => File.ReadAllBytes(inputFile.FullName);
    public static string[] ReadAllLines(this FileInfo inputFile) => File.ReadAllLines(inputFile.FullName);
    public static string ReadAllText(this FileInfo inputFile) => File.ReadAllText(inputFile.FullName);
    public static IEnumerable<string> ReadLines(this FileInfo inputFile) => File.ReadLines(inputFile.FullName);


    // ★★★★★★★★★★★★★★★ 

}
