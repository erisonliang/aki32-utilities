using Aki32Utilities.ConsoleAppUtilities.General;

using Newtonsoft.Json.Linq;

namespace Aki32Utilities.ConsoleAppUtilities.Research;
/// <see cref="https://www.crossref.org/"/>
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
            addingMainArticle.CrossRef_PublishedDate = (publishedDateArray == null) ? null : string.Join('-', publishedDateArray!.AsEnumerable());

            string? pageString = json?["message"]?["page"]?.ToString();
            if (pageString is not null)
            {
                var pageArray = pageString.Split(',', StringSplitOptions.TrimEntries);
                if (pageArray.Length >= 1)
                    addingMainArticle.CrossRef_StartingPage = pageArray[0];
                if (pageArray.Length >= 2)
                    addingMainArticle.CrossRef_EndingPage = pageArray[1];
            }


            // add references
            if (json?["message"]?["reference"] is JArray references)
            {
                foreach (var reference in references)
                {
                    // Create and add referred article
                    var addingSubArticle = new ResearchArticle();
                    {
                        addingSubArticle.DataFrom_CrossRef_SimpleRef = true;

                        addingSubArticle.DOI = reference?["DOI"]?.ToString();

                        addingSubArticle.CrossRef_ArticleTitle = reference?["series-title"]?.ToString();
                        addingSubArticle.CrossRef_Authors_Simple = reference?["author"]?.ToString().Split(',', StringSplitOptions.TrimEntries); // 1人分しかくれない？？

                        addingSubArticle.CrossRef_MaterialTitle = reference?["journal-title"]?.ToString();
                        addingSubArticle.CrossRef_MaterialVolume = reference?["volume"]?.ToString();

                        addingSubArticle.CrossRef_PublishedYear = reference?["year"]?.ToString();
                        addingSubArticle.CrossRef_StartingPage = reference?["first-page"]?.ToString();
                        addingSubArticle.CrossRef_EndingPage = reference?["last-page"]?.ToString();

                        addingSubArticle.CrossRef_UnstructuredRefString = ResearchArticle.CleanUp_UnstructuredRefString(reference?["unstructured"]?.ToString());
                    }
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
