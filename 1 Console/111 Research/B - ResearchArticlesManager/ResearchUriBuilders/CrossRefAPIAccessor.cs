

using Aki32Utilities.ConsoleAppUtilities.UsefulClasses;

using Newtonsoft.Json.Linq;

using System;

using XPlot.Plotly;

namespace Aki32Utilities.ConsoleAppUtilities.Research;
public class CrossRefAPIAccessor : IResearchAPIAccessor
{

    // ★★★★★★★★★★★★★★★ props

    public const string BASE_URL = $@"https://api.crossref.org/v1/works";

    internal Uri builtUri = null;

    public string DOI { get; set; } = "";


    // ★★★★★★★★★★★★★★★ inits

    public CrossRefAPIAccessor()
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
    public IEnumerable<ResearchArticle> FetchArticles()
    {
        // get json
        dynamic json = BuildUri().CallAPIAsync_ForJsonData<dynamic>(HttpMethod.Get).Result;

        //if (json == null || json!["status"].ToString() != "ok"){}

        var article = new ResearchArticle();
        {
            article.DataFrom_CrossRef = true;

            article.DOI = json?["message"]?["DOI"]?.ToString();

            article.CrossRef_ArticleTitle = (json?["message"]?["title"] as JArray)?.FirstOrDefault()?.ToString();

            article.CrossRef_Authors = (json?["message"]?["author"] as JArray)?.Select(x => $"{x?["given"]?.ToString()} {x?["family"]?.ToString()}").ToArray();

            article.PrintISSN = (json?["message"]?["issn-type"] as JArray)?.FirstOrDefault(i => (dynamic)i?["type"]?.ToString()! == "print")?["value"]?.ToString();
            article.OnlineISSN = (json?["message"]?["issn-type"] as JArray)?.FirstOrDefault(i => (dynamic)i?["type"]?.ToString()! == "electronic")?["value"]?.ToString();

            var publishedDateAray = (json?["message"]?["published"]?["date-parts"] as JArray)?.FirstOrDefault();
            article.CrossRef_PublishedDate = (publishedDateAray == null) ? null : string.Join('/', publishedDateAray!.AsEnumerable());

            // add reference
            if (json?["message"]?["reference"] is JArray references)
            {
                foreach (var reference in references)
                {
                    // Create and add referred article
                    var addingArticle = new ResearchArticle
                    {
                        DOI = reference?["DOI"]?.ToString(),
                        UnstructuredRefString = ResearchArticle.CleanUp_UnstructuredRefString(reference?["unstructured"]?.ToString())
                    };
                    article.AddArticleReference(addingArticle);

                    yield return addingArticle;
                }
            }
        }

        // return 
        yield return article;

    }


    // ★★★★★★★★★★★★★★★ 

}
