using Aki32Utilities.ConsoleAppUtilities.General;

using Newtonsoft.Json.Linq;

namespace Aki32Utilities.ConsoleAppUtilities.Research;
public class CrossRef_DOI_APIAccessor : IResearchAPIAccessor
{

    // ★★★★★★★★★★★★★★★ props

    public const string BASE_URL = $@"https://api.crossref.org/v1/works";

    internal Uri builtUri = null;

    public string DOI { get; set; } = "";


    // ★★★★★★★★★★★★★★★ inits

    public CrossRef_DOI_APIAccessor()
    {
    }


    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// build uri
    /// </summary>
    /// <returns></returns>
    public Uri BuildUri()
    {
        // preprocess
        if (string.IsNullOrEmpty(DOI))
            throw new InvalidDataException("DOI property is required to be filled first");


        // post process
        var builtUriString = $"{BASE_URL}/{DOI}";
        return builtUri = new Uri(builtUriString);

    }

    /// <summary>
    /// fetch articles
    /// </summary>
    /// <returns></returns>
    public async Task<List<ResearchArticle>> FetchArticles()
    {
        return await Task.Run(() => _FetchArticles().Reverse().ToList()); // need Reverse() to move addingMainArticle to top.
    }
    private IEnumerable<ResearchArticle> _FetchArticles()
    {
        // get json
        var uri = BuildUri();
        dynamic json = uri.CallAPIAsync_ForJsonData<dynamic>(HttpMethod.Get).Result;

        //if (json == null || json!["status"].ToString() != "ok"){}

        var addingMainArticle = new ResearchArticle();
        {
            addingMainArticle.DataFrom_CrossRef_DOI = true;

            addingMainArticle.DOI = json?["message"]?["DOI"]?.ToString();

            addingMainArticle.CrossRef_ArticleTitle = (json?["message"]?["title"] as JArray)?.FirstOrDefault()?.ToString();

            addingMainArticle.CrossRef_Authors = (json?["message"]?["author"] as JArray)?.Select(x => $"{x?["given"]?.ToString().Trim()} {x?["family"]?.ToString().Trim()}").ToArray();

            addingMainArticle.PrintISSN = (json?["message"]?["issn-type"] as JArray)?.FirstOrDefault(i => (dynamic)i?["type"]?.ToString()! == "print")?["value"]?.ToString();
            addingMainArticle.OnlineISSN = (json?["message"]?["issn-type"] as JArray)?.FirstOrDefault(i => (dynamic)i?["type"]?.ToString()! == "electronic")?["value"]?.ToString();

            var publishedDateArray = (json?["message"]?["published"]?["date-parts"] as JArray)?.FirstOrDefault();
            addingMainArticle.CrossRef_PublishedDate = (publishedDateArray == null) ? null : string.Join('/', publishedDateArray!.AsEnumerable());

            // add references
            if (json?["message"]?["reference"] is JArray references)
            {
                foreach (var reference in references)
                {
                    // Create and add referred article
                    var addingSubArticle = new ResearchArticle
                    {
                        DataFrom_CrossRef_SimpleRef = true,
                        DOI = reference?["DOI"]?.ToString(),
                        CrossRef_UnstructuredRefString = ResearchArticle.CleanUp_UnstructuredRefString(reference?["unstructured"]?.ToString())
                    };
                    addingMainArticle.AddArticleReference(addingSubArticle);

                    yield return addingSubArticle;
                }
            }
        }

        // return 
        yield return addingMainArticle;

    }


    // ★★★★★★★★★★★★★★★ 

}
