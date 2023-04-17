using System.Security.Cryptography;
using System.Text;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// generate hash
    /// </summary>
    /// <param name="inputPath"></param>
    /// <param name="newFileNameWithoutExtension">"*" will be replaced with old name</param>
    /// <param name="replaceSets">replace strings in original file name</param>
    /// <returns></returns>
    public static string ReplaceUnwantedCharacters(this string inputString)
    {
        // main
        return inputString
            .Replace("\u00a0", " ")
            ;
    }

}
