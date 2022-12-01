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

    public void SaveDatabase(bool forceSaveArticleDatabase = false)
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

                    article.ArticleTitle_English_JS = entry.Element(ExpandXml("article_title"))?.Element(ExpandXml("en"))?.Value;
                    article.ArticleTitle_Japanese_JS = entry.Element(ExpandXml("article_title"))?.Element(ExpandXml("ja"))?.Value;

                    article.Link_English_JS = entry.Element(ExpandXml("article_link"))?.Element(ExpandXml("en"))?.Value;
                    article.Link_Japanese_JS = entry.Element(ExpandXml("article_link"))?.Element(ExpandXml("ja"))?.Value;

                    article.Authors_English_JS = entry.Element(ExpandXml("author"))?.Element(ExpandXml("en"))?.Elements(ExpandXml("name"))?.Select(e => e?.Value ?? "")?.ToArray();
                    article.Authors_Japanese_JS = entry.Element(ExpandXml("author"))?.Element(ExpandXml("ja"))?.Elements(ExpandXml("name"))?.Select(e => e?.Value ?? "")?.ToArray();

                    article.JournalCode_JS = entry.Element(ExpandXml("cdjournal"))?.Value;

                    article.MaterialTitle_English_JS = entry.Element(ExpandXml("material_title"))?.Element(ExpandXml("en"))?.Value;
                    article.MaterialTitle_Japanese_JS = entry.Element(ExpandXml("material_title"))?.Element(ExpandXml("ja"))?.Value;

                    article.PrintISSN = entry.Element(ExpandPrism("issn"))?.Value;
                    article.OnlineISSN = entry.Element(ExpandPrism("eIssn"))?.Value;

                    article.Volume_JS = entry.Element(ExpandPrism("volume"))?.Value;
                    article.SubVolume_JS = entry.Element(ExpandXml("cdvols"))?.Value;

                    article.Number_JS = entry.Element(ExpandPrism("number"))?.Value;
                    article.StartingPage_JS = entry.Element(ExpandPrism("startingPage"))?.Value;
                    article.EndingPage_JS = entry.Element(ExpandPrism("endingPage"))?.Value;

                    article.PublishedYear_JS = entry.Element(ExpandXml("pubyear"))?.Value;

                    article.JOI_JS = entry.Element(ExpandXml("joi"))?.Value;
                    article.DOI = entry.Element(ExpandPrism("doi"))?.Value;

                    article.SystemCode_JS = entry.Element(ExpandXml("systemcode"))?.Value;
                    article.SystemName_JS = entry.Element(ExpandXml("systemname"))?.Value;

                    article.Title_JS = entry.Element(ExpandXml("title"))?.Value;

                    article.Link_JS = entry.Element(ExpandXml("link"))?.Value;
                    article.Id_JS = entry.Element(ExpandXml("id"))?.Value;
                    article.UpdatedOn_JS = entry.Element(ExpandXml("updated"))?.Value;

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

                article.ArticleTitle_CR = (json?["message"]?["title"] as JArray)?.FirstOrDefault()?.ToString();

                article.Authors_CR = (json?["message"]?["author"] as JArray)?.Select(x => $"{x?["given"]?.ToString()} {x?["family"]?.ToString()}").ToArray();

                article.PrintISSN = (json?["message"]?["issn-type"] as JArray)?.FirstOrDefault(i => (dynamic)i?["type"]?.ToString()! == "print")?["value"]?.ToString();
                article.OnlineISSN = (json?["message"]?["issn-type"] as JArray)?.FirstOrDefault(i => (dynamic)i?["type"]?.ToString()! == "electronic")?["value"]?.ToString();

                var publishedDateAray = (json?["message"]?["published"]?["date-parts"] as JArray)?.FirstOrDefault();
                article.PublishedDate_CR = (publishedDateAray == null) ? null : string.Join('/', publishedDateAray!.AsEnumerable());


                // add reference
                {
                    _ = (json?["message"]?["reference"] as JArray)?
                        .Select(x =>
                        {
                            // Get referred article
                            var addingArticle = new ResearchArticle
                            {
                                DOI = x?["DOI"]?.ToString(),
                                UnstructuredRefString = ResearchArticle.CleanUp_UnstructuredRefString(x?["unstructured"]?.ToString())
                            };

                            // Add DOI or AOI to ReferenceDOIs.
                            article.ReferenceDOIs = (article.ReferenceDOIs ?? Array.Empty<string>())
                            .Append(addingArticle.DOI ?? (addingArticle.AOI = Guid.NewGuid().ToString()))!
                            .ToArray();

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
            IEnumerable<ResearchArticle?> matchedArticleFinder()
            {
                yield return ArticleDatabase.FirstOrDefault(a => a.DOI != null && a.DOI == article!.DOI);

                yield return ArticleDatabase.FirstOrDefault(a => a.ArticleTitle_CR != null && a.ArticleTitle_CR == article!.ArticleTitle_CR);
                yield return ArticleDatabase.FirstOrDefault(a => a.Id_JS != null && a.Id_JS == article!.Id_JS);
                yield return ArticleDatabase.FirstOrDefault(a => a.ArticleTitle_Manual != null && a.ArticleTitle_Manual == article!.ArticleTitle_Manual);

                // 最終手段。
                yield return ArticleDatabase.FirstOrDefault(a => a.AOI != null && a.AOI == article!.AOI);

            }

            void UpdateOrCretae()
            {
                foreach (var matchedArticle in matchedArticleFinder())
                {
                    // 更新・マージ
                    if (matchedArticle != null)
                    {
                        matchedArticle.MergeInfo(article!);
                        if (UtilConfig.ConsoleOutput_Contents)
                            Console.WriteLine($"@@@ {article!.ArticleTitle_CR}");

                        updatedCount++;
                        articleDatabaseUpdated = true;
                        return;
                    }
                }

                // 新規登録
                {
                    ArticleDatabase.Add(article!);

                    if (UtilConfig.ConsoleOutput_Contents)
                        Console.WriteLine($"+++ {article!.ArticleTitle_CR}");

                    addedCount++;
                    articleDatabaseUpdated = true;
                }

            };

            UpdateOrCretae();

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
