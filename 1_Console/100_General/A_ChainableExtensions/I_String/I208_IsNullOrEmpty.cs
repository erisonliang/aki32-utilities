

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    public static bool IsNullOrEmpty(this string? input)
    {
        return string.IsNullOrEmpty(input);
    }

    public static bool IsNullOrWhiteSpace(this string? input)
    {
        return string.IsNullOrWhiteSpace(input);
    }

    public static bool IsNullOrWhiteSpaceOrNewLine(this string? input)
    {
        return input.IsNullOrWhiteSpace() || input!.Replace("\r", "").Replace("\n", "").IsNullOrWhiteSpace();
    }

}
