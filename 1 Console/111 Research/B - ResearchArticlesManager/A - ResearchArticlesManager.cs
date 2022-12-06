using System.Xml.Linq;

using Aki32Utilities.ConsoleAppUtilities.General;

using Newtonsoft.Json.Linq;

namespace Aki32Utilities.ConsoleAppUtilities.SpecificPurposeModels.Research;
/// <remarks>
/// 参考：
/// Crossref: https://www.crossref.org/documentation/retrieve-metadata/rest-api/
/// J-Stage:  https://www.jstage.jst.go.jp/static/files/ja/manual_api.pdf
/// CiNii:    https://support.nii.ac.jp/ja/cir/r_opensearch
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
        Console.WriteLine("+ Crossref (https://www.crossref.org/)");
        Console.WriteLine("+ J-STAGE (https://www.jstage.jst.go.jp/)");
        Console.WriteLine("+ CiNii (https://cir.nii.ac.jp/)");
        Console.WriteLine("+ ");
        Console.WriteLine();

        PDFsDirectory.Create();

    }


    // ★★★★★★★★★★★★★★★ methods (data handle)

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


        if (uriBuilder is JStageArticleUriBuilder)
        {
            // get xml
            var xml = XElement.Load(uri.AbsoluteUri);


            // analyse 
            var totalResults = xml.Element(ExpandOpenSearch("totalResults"))!.Value;
            var startIndex = xml.Element(ExpandOpenSearch("startIndex"))!.Value;
            var itemsPerPage = xml.Element(ExpandOpenSearch("itemsPerPage"))!.Value;
            var toIndex = int.Parse(startIndex) + int.Parse(itemsPerPage) - 1;
            var entities = xml.Elements(ExpandXml("entry"));

            Console.WriteLine();
            Console.WriteLine($"★ Obtained {itemsPerPage} items out of {totalResults} matches ( From #{startIndex} to #{toIndex} )");
            Console.WriteLine();

            foreach (var entity in entities)
            {
                var article = new ResearchArticle()
                {
                    DataFrom_JStage = true
                };

                {

                    article.JStage_ArticleTitle_English = entity.Element(ExpandXml("article_title"))?.Element(ExpandXml("en"))?.Value;
                    article.JStage_ArticleTitle_Japanese = entity.Element(ExpandXml("article_title"))?.Element(ExpandXml("ja"))?.Value;

                    article.JStage_Link_English = entity.Element(ExpandXml("article_link"))?.Element(ExpandXml("en"))?.Value;
                    article.JStage_Link_Japanese = entity.Element(ExpandXml("article_link"))?.Element(ExpandXml("ja"))?.Value;

                    article.JStage_Authors_English = entity.Element(ExpandXml("author"))?.Element(ExpandXml("en"))?.Elements(ExpandXml("name"))?.Select(e => e?.Value ?? "")?.ToArray();
                    article.JStage_Authors_Japanese = entity.Element(ExpandXml("author"))?.Element(ExpandXml("ja"))?.Elements(ExpandXml("name"))?.Select(e => e?.Value ?? "")?.ToArray();

                    article.JStage_JournalCode = entity.Element(ExpandXml("cdjournal"))?.Value;

                    article.JStage_MaterialTitle_English = entity.Element(ExpandXml("material_title"))?.Element(ExpandXml("en"))?.Value;
                    article.JStage_MaterialTitle_Japanese = entity.Element(ExpandXml("material_title"))?.Element(ExpandXml("ja"))?.Value;

                    article.PrintISSN = entity.Element(ExpandPrism("issn"))?.Value;
                    article.OnlineISSN = entity.Element(ExpandPrism("eIssn"))?.Value;

                    article.JStage_Volume = entity.Element(ExpandPrism("volume"))?.Value;
                    article.JStage_SubVolume = entity.Element(ExpandXml("cdvols"))?.Value;

                    article.JStage_Number = entity.Element(ExpandPrism("number"))?.Value;
                    article.JStage_StartingPage = entity.Element(ExpandPrism("startingPage"))?.Value;
                    article.JStage_EndingPage = entity.Element(ExpandPrism("endingPage"))?.Value;

                    article.JStage_PublishedYear = entity.Element(ExpandXml("pubyear"))?.Value;

                    article.JStage_JOI = entity.Element(ExpandXml("joi"))?.Value;
                    article.DOI = entity.Element(ExpandPrism("doi"))?.Value;

                    article.JStage_SystemCode = entity.Element(ExpandXml("systemcode"))?.Value;
                    article.JStage_SystemName = entity.Element(ExpandXml("systemname"))?.Value;

                    //article.JStage_ArticleTitle = entity.Element(ExpandXml("title"))?.Value;

                    //article.JStage_Link = entity.Element(ExpandXml("link"))?.Value;
                    article.JStage_Id = entity.Element(ExpandXml("id"))?.Value;
                    article.JStage_UpdatedOn = entity.Element(ExpandXml("updated"))?.Value;

                }

                fetchedArticles.Add(article);

            }

        }
        else if (uriBuilder is CiNiiArticleUriBuilder)
        {
            dynamic json = uri.CallAPIAsync_ForJsonData<dynamic>(HttpMethod.Get).Result;


            // analyse 
            var totalResults = json?["opensearch:totalResults"]?.ToString();
            var startIndex = json?["opensearch:startIndex"]?.ToString();
            var itemsPerPage = json?["opensearch:itemsPerPage"]?.ToString();
            var toIndex = int.Parse(startIndex) + int.Parse(itemsPerPage) - 1;
            var entities = json?["items"] as JArray;

            Console.WriteLine();
            Console.WriteLine($"★ Obtained {itemsPerPage} items out of {totalResults} matches ( From #{startIndex} to #{toIndex} )");
            Console.WriteLine();

            foreach (var entity in entities)
            {
                var article = new ResearchArticle()
                {
                    DataFrom_CiNii = true
                };

                {

                    article.CiNii_ArticleTitle = entity?["title"]?.ToString();

                    article.CiNii_Link = entity?["link"]?["@id"]?.ToString();

                    article.CiNii_Authors = (entity?["dc:creator"] as JArray)?.Select(a => a.ToString())?.ToArray();

                    article.CiNii_Publisher = entity?["dc:publisher"]?.ToString();
                    article.CiNii_PublicationName = entity?["prism:publicationName"]?.ToString();

                    article.PrintISSN = entity?["prism:issn"]?.ToString();

                    article.CiNii_Volume = entity?["prism:volume"]?.ToString();
                    article.CiNii_Number = entity?["prism:number"]?.ToString();
                    article.CiNii_StartingPage = entity?["prism:startingPage"]?.ToString();
                    article.CiNii_EndingPage = entity?["prism:endingPage"]?.ToString();
                    article.CiNii_PublishedDate = entity?["prism:publicationDate"]?.ToString();


                    article.CiNii_Description = entity?["description"]?.ToString();

                    article.DOI = (entity?["dc:identifier"] as JArray)?.FirstOrDefault(x => x?["@type"]?.ToString() == "cir:DOI")?["@value"]?.ToString();

                }

                // add article
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
                matchedArticles.First().MergeArticles(article!, ArticleDatabase);

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


    // ★★★★★★★★★★★★★★★ methods (practical use)




    // ★★★★★★★★★★★★★★★ methods (helper)

    private static string ExpandXml(string s) => "{http://www.w3.org/2005/Atom}" + s;
    private static string ExpandOpenSearch(string s) => "{http://a9.com/-/spec/opensearch/1.1/}" + s;
    private static string ExpandPrism(string s) => "{http://prismstandard.org/namespaces/basic/2.0/}" + s;


    // ★★★★★★★★★★★★★★★

}
