using System;
using System.Data;
using System.Xml.Linq;

using Aki32_Utilities.General;

using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;

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

            Console.WriteLine($"★ Obtained {itemsPerPage} items out of {totalResults} matches ( From #{startIndex} to #{toIndex} )");
            Console.WriteLine();

            foreach (var entry in entries)
            {
                var article = new ResearchArticle
                {

                    Title_English = entry.Element(ExpandXml("article_title"))?.Element(ExpandXml("en"))?.Value,
                    Title_Japanese = entry.Element(ExpandXml("article_title"))?.Element(ExpandXml("ja"))?.Value,

                    Link_English = entry.Element(ExpandXml("article_link"))?.Element(ExpandXml("en"))?.Value,
                    Link_Japanese = entry.Element(ExpandXml("article_link"))?.Element(ExpandXml("ja"))?.Value,

                    Authors_English = entry.Element(ExpandXml("author"))?.Element(ExpandXml("en"))?.Elements(ExpandXml("name"))?.Select(e => e?.Value ?? "")?.ToArray(),
                    Authors_Japanese = entry.Element(ExpandXml("author"))?.Element(ExpandXml("ja"))?.Elements(ExpandXml("name"))?.Select(e => e?.Value ?? "")?.ToArray(),

                    JournalCode_JStage = entry.Element(ExpandXml("cdjournal"))?.Value,

                    MaterialTitle_English = entry.Element(ExpandXml("material_title"))?.Element(ExpandXml("en"))?.Value,
                    MaterialTitle_Japanese = entry.Element(ExpandXml("material_title"))?.Element(ExpandXml("ja"))?.Value,

                    PrintISSN = entry.Element(ExpandPrism("issn"))?.Value,
                    OnlineISSN = entry.Element(ExpandPrism("eIssn"))?.Value,

                    Volume = entry.Element(ExpandPrism("volume"))?.Value,
                    SubVolume = entry.Element(ExpandXml("cdvols"))?.Value,

                    Number = entry.Element(ExpandPrism("number"))?.Value,
                    StartingPage = entry.Element(ExpandPrism("startingPage"))?.Value,
                    EndingPage = entry.Element(ExpandPrism("endingPage"))?.Value,

                    PublishedYear = entry.Element(ExpandXml("pubyear"))?.Value,

                    JOI = entry.Element(ExpandXml("joi"))?.Value,
                    DOI = entry.Element(ExpandPrism("doi"))?.Value,

                    SystemCode = entry.Element(ExpandXml("systemcode"))?.Value,
                    SystemName = entry.Element(ExpandXml("systemname"))?.Value,

                    Title = entry.Element(ExpandXml("title"))?.Value,

                    Link = entry.Element(ExpandXml("link"))?.Value,
                    Id = entry.Element(ExpandXml("id"))?.Value,
                    UpdatedOn = entry.Element(ExpandXml("updated"))?.Value,

                    DataFrom_JStage = true,

                };

                fetchedArticles.Add(article);

            }

        }
        else if (uriBuilder is CrossRefArticleUriBuilder)
        {









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

        Console.WriteLine($"★ Merging {articles} articles in total...");


        // main
        foreach (var article in articles)
        {
            if (ArticleDatabase.Any(a => a.Id == article.Id))
            {
                // 更新・マージ
                ArticleDatabase.First(a => a.Id == article.Id).MergeInfo(article);
                if (UtilConfig.ConsoleOutput_Contents)
                    Console.WriteLine($"@@@ {article.Title_Japanese}");

                updatedCount++;
                articleDatabaseUpdated = true;
            }
            else
            {
                // 新規登録
                ArticleDatabase.Add(article);

                if (UtilConfig.ConsoleOutput_Contents)
                    Console.WriteLine($"+++ {article.Title_Japanese}");

                addedCount++;
                articleDatabaseUpdated = true;
            }
        }


        // save local
        SaveDatabase();
        Console.WriteLine($"★ {addedCount} added, {updatedCount} updated. {articles} in total.");
        Console.WriteLine();

    }


    // ★★★★★★★★★★★★★★★ helper

    private static string ExpandXml(string s) => "{http://www.w3.org/2005/Atom}" + s;
    private static string ExpandOpenSearch(string s) => "{http://a9.com/-/spec/opensearch/1.1/}" + s;
    private static string ExpandPrism(string s) => "{http://prismstandard.org/namespaces/basic/2.0/}" + s;


    // ★★★★★★★★★★★★★★★

}
