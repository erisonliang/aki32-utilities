using System.Xml.Linq;

using Aki32_Utilities.General;
using Aki32_Utilities.UsefulClasses;

using LibGit2Sharp;

using Newtonsoft.Json.Linq;

namespace Aki32_Utilities.SpecificPurposeModels.Research;
/// <remarks>
/// 参考：
/// https://www.jstage.jst.go.jp/static/files/ja/manual_api.pdf
/// </remarks>
public partial class ResearchArticlesManager
{

    // ★★★★★★★★★★★★★★★ paths

    public DirectoryInfo LocalDirectory { get; set; }
    private FileInfo ArticleDatabaseFileInfo => new($@"{LocalDirectory.FullName}\ResearchArticles.csv");
    public DirectoryInfo PDFsDirectory => new($@"{LocalDirectory.FullName}\PDFs");


    // ★★★★★★★★★★★★★★★ props

    public List<ResearchArticle> ArticleDatabase { get; set; } = null;

    private bool articleDatabaseUpdated = false;


    // ★★★★★★★★★★★★★★★ init

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="lineAccessToken"></param>
    public ResearchArticlesManager(DirectoryInfo baseDir)
    {
        LocalDirectory = baseDir;

        Console.WriteLine("Data Powered by");
        Console.WriteLine("+ ");
        Console.WriteLine("+ J-STAGE (https://www.jstage.jst.go.jp/browse/-char/ja)");
        Console.WriteLine("+ ");
        Console.WriteLine();

        PDFsDirectory.Create();

    }


    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// Open database. If not exist, automatically create new ones.
    /// </summary>
    public void OpenDatabase()
    {
        try
        {
            ArticleDatabase = ArticleDatabaseFileInfo.ReadObjectFromLocalCsv<ResearchArticle>();
        }
        catch (FileNotFoundException)
        {
            ArticleDatabase = new List<ResearchArticle>();
        }
    }

    public void SaveDatabase(bool forceSaveArticleDatabase = true)
    {
        if (articleDatabaseUpdated || forceSaveArticleDatabase)
        {
            ArticleDatabase.SaveAsCsv(ArticleDatabaseFileInfo);
            articleDatabaseUpdated = false;
        }
    }


    /// <summary>
    /// pull data from dataserver to local database
    /// </summary>
    /// <param name="uriBuilder"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void PullArticleInfo(IResearchUriBuilder uriBuilder)
    {
        var articles = FetchArticleInfo(uriBuilder);
        MergeArticleInfo(articles);

    }

    /// <summary>
    /// fetch data from dataserver
    /// </summary>
    /// <param name="uriBuilder"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public IEnumerable<ResearchArticle> FetchArticleInfo(IResearchUriBuilder uriBuilder)
    {
        // preprocess
        var uri = uriBuilder.Build();
        if (ArticleDatabase == null)
            throw new InvalidOperationException("Database has not been opened yet. Use OpenDataBase() first.");

        var fetchedArticles = new List<ResearchArticle>();


        // main
        if (uriBuilder is JStageArticleUriBuilder)
        {
            // get xml
            var xml = XElement.Load(uri.AbsoluteUri);


            // analyse 
            var totalResults = xml.Element(ExpandOpenSearch("totalResults"))!.Value;
            var startIndex = xml.Element(ExpandOpenSearch("startIndex"))!.Value;
            var itemsPerPage = xml.Element(ExpandOpenSearch("itemsPerPage"))!.Value;
            var toIndex = int.Parse(startIndex) + int.Parse(itemsPerPage) - 1;
            var entries = xml.Elements(ExpandXml("entry"));

            Console.WriteLine();
            Console.WriteLine($"★ Obtained {itemsPerPage} items out of {totalResults} matches ( From #{startIndex} to #{toIndex} )");
            Console.WriteLine();

            foreach (var entry in entries)
            {
                var article = new ResearchArticle()
                {
                    DataFrom_JStage = true
                };

                {

                    article.JStage_ArticleTitle_English = entry.Element(ExpandXml("article_title"))?.Element(ExpandXml("en"))?.Value;
                    article.JStage_ArticleTitle_Japanese = entry.Element(ExpandXml("article_title"))?.Element(ExpandXml("ja"))?.Value;

                    article.JStage_Link_English = entry.Element(ExpandXml("article_link"))?.Element(ExpandXml("en"))?.Value;
                    article.JStage_Link_Japanese = entry.Element(ExpandXml("article_link"))?.Element(ExpandXml("ja"))?.Value;

                    article.JStage_Authors_English = entry.Element(ExpandXml("author"))?.Element(ExpandXml("en"))?.Elements(ExpandXml("name"))?.Select(e => e?.Value ?? "")?.ToArray();
                    article.JStage_Authors_Japanese = entry.Element(ExpandXml("author"))?.Element(ExpandXml("ja"))?.Elements(ExpandXml("name"))?.Select(e => e?.Value ?? "")?.ToArray();

                    article.JStage_JournalCode = entry.Element(ExpandXml("cdjournal"))?.Value;

                    article.JStage_MaterialTitle_English = entry.Element(ExpandXml("material_title"))?.Element(ExpandXml("en"))?.Value;
                    article.JStage_MaterialTitle_Japanese = entry.Element(ExpandXml("material_title"))?.Element(ExpandXml("ja"))?.Value;

                    article.PrintISSN = entry.Element(ExpandPrism("issn"))?.Value;
                    article.OnlineISSN = entry.Element(ExpandPrism("eIssn"))?.Value;

                    article.JStage_Volume = entry.Element(ExpandPrism("volume"))?.Value;
                    article.JStage_SubVolume = entry.Element(ExpandXml("cdvols"))?.Value;

                    article.JStage_Number = entry.Element(ExpandPrism("number"))?.Value;
                    article.JStage_StartingPage = entry.Element(ExpandPrism("startingPage"))?.Value;
                    article.JStage_EndingPage = entry.Element(ExpandPrism("endingPage"))?.Value;

                    article.JStage_PublishedYear = entry.Element(ExpandXml("pubyear"))?.Value;

                    article.JStage_JOI = entry.Element(ExpandXml("joi"))?.Value;
                    article.DOI = entry.Element(ExpandPrism("doi"))?.Value;

                    article.JStage_SystemCode = entry.Element(ExpandXml("systemcode"))?.Value;
                    article.JStage_SystemName = entry.Element(ExpandXml("systemname"))?.Value;

                    article.JStage_Title = entry.Element(ExpandXml("title"))?.Value;

                    article.JStage_Link = entry.Element(ExpandXml("link"))?.Value;
                    article.JStage_Id = entry.Element(ExpandXml("id"))?.Value;
                    article.JStage_UpdatedOn = entry.Element(ExpandXml("updated"))?.Value;

                }

                fetchedArticles.Add(article);

            }

        }
        else if (uriBuilder is CrossRefArticleUriBuilder)
        {
            dynamic json = uri.CallAPIAsync_ForJsonData<dynamic>(HttpMethod.Get).Result;

            //if (json == null || json!["status"].ToString() != "ok"){}

            var article = new ResearchArticle()
            {
                DataFrom_CrossRef = true
            };

            {

                article.DOI = json?["message"]?["DOI"]?.ToString();

                article.CrossRef_ArticleTitle = (json?["message"]?["title"] as JArray)?.FirstOrDefault()?.ToString();

                article.CrossRef_Authors = (json?["message"]?["author"] as JArray)?.Select(x => $"{x?["given"]?.ToString()} {x?["family"]?.ToString()}").ToArray();

                article.PrintISSN = (json?["message"]?["issn-type"] as JArray)?.FirstOrDefault(i => (dynamic)i?["type"]?.ToString()! == "print")?["value"]?.ToString();
                article.OnlineISSN = (json?["message"]?["issn-type"] as JArray)?.FirstOrDefault(i => (dynamic)i?["type"]?.ToString()! == "electronic")?["value"]?.ToString();

                var publishedDateAray = (json?["message"]?["published"]?["date-parts"] as JArray)?.FirstOrDefault();
                article.CrossRef_PublishedDate = (publishedDateAray == null) ? null : string.Join('/', publishedDateAray!.AsEnumerable());


                // add reference
                {
                    _ = (json?["message"]?["reference"] as JArray)?
                        .Select(x =>
                        {
                            // Create and add referred article
                            var addingArticle = new ResearchArticle
                            {
                                DOI = x?["DOI"]?.ToString(),
                                UnstructuredRefString = ResearchArticle.CleanUp_UnstructuredRefString(x?["unstructured"]?.ToString())
                            };
                            article.AddArticleReference(addingArticle);

                            fetchedArticles.Add(addingArticle);

                            return "";
                        })
                        .ToArray();
                }
            }

            // add article
            fetchedArticles.Add(article);

        }
        else
        {
            throw new NotImplementedException();
        }


        // post process
        return fetchedArticles;

    }

    /// <summary>
    /// merge articles to local database
    /// </summary>
    /// <param name="uriBuilder"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void MergeArticleInfo(IEnumerable<ResearchArticle> articles)
    {
        // preprocess
        var addedCount = 0;
        var updatedCount = 0;
        if (ArticleDatabase == null)
            throw new InvalidOperationException("Database has not been opened yet. Call OpenDataBase() first.");


        Console.WriteLine();
        Console.WriteLine($"★ Merging {articles.Count()} articles in total...");
        Console.WriteLine();

        // main
        foreach (var article in articles)
        {
            var matchedArticles = ArticleDatabase.Where(a => a.CompareTo(article) == 0);

            // 更新・マージ
            if (matchedArticles != null && matchedArticles.Count() == 1)
            {
                ArticleDatabase.MergeArticles(matchedArticles.First(), article!);

                if (UtilConfig.ConsoleOutput_Contents)
                    Console.WriteLine($"@@@ {article!.ArticleTitle}");

                updatedCount++;
                articleDatabaseUpdated = true;
            }

            // おかしい
            else if (matchedArticles!.Count() > 1)
                throw new InvalidDataException($"{matchedArticles!.Count()} articles matched to {article.ArticleTitle}");

            // 新規登録
            else
            {
                ArticleDatabase.Add(article!);

                if (UtilConfig.ConsoleOutput_Contents)
                    Console.WriteLine($"+++ {article!.ArticleTitle}");

                addedCount++;
                articleDatabaseUpdated = true;
            }
        }


        // save local
        SaveDatabase();
        Console.WriteLine($"★ {addedCount} added, {updatedCount} updated.");
        Console.WriteLine();

    }


    // ★★★★★★★★★★★★★★★ helper

    private static string ExpandXml(string s) => "{http://www.w3.org/2005/Atom}" + s;
    private static string ExpandOpenSearch(string s) => "{http://a9.com/-/spec/opensearch/1.1/}" + s;
    private static string ExpandPrism(string s) => "{http://prismstandard.org/namespaces/basic/2.0/}" + s;


    // ★★★★★★★★★★★★★★★

}
