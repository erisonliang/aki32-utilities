

namespace Aki32Utilities.ConsoleAppUtilities.Research;
public class CrossRefArticleUriBuilder : IResearchUriBuilder
{

    // ★★★★★★★★★★★★★★★ props

    public const string BASE_URL = $@"https://api.crossref.org/v1/works";

    internal Uri builtUri = null;

    public string DOI { get; set; } = "";


    // ★★★★★★★★★★★★★★★ inits

    public CrossRefArticleUriBuilder()
    {
    }


    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// build uri
    /// </summary>
    /// <returns></returns>
    public Uri Build()
    {
        // preprocess
        if (string.IsNullOrEmpty(DOI))
            throw new InvalidDataException("DOI property is required to be filled first");


        // post process
        var builtUriString = $"{BASE_URL}/{DOI}";
        return builtUri = new Uri(builtUriString);

    }


    // ★★★★★★★★★★★★★★★ 

}
