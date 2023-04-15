

namespace Aki32Utilities.ConsoleAppUtilities.Research;
public interface IResearchAPIAccessor
{

    /// <summary>
    /// build uri
    /// </summary>
    /// <returns></returns>
    public Uri BuildUri();

    /// <summary>
    /// fetch articles
    /// </summary>
    /// <returns></returns>
    public Task<List<ResearchArticle>> FetchArticles();

}
