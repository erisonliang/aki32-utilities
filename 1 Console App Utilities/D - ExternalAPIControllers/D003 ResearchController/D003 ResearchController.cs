using System.Data;
using System.Xml.Linq;

using Aki32_Utilities.General;

namespace Aki32_Utilities.ExternalAPIControllers;

/// <summary>
/// </summary>
/// <remarks>
/// 
/// 参考：
/// https://www.jstage.jst.go.jp/static/files/ja/manual_api.pdf
/// 
/// </remarks>
public partial class ResearchController
{

    // ★★★★★★★★★★★★★★★ paths

    public DirectoryInfo LocalDirectory { get; set; }
    public DirectoryInfo DatabaseDirectory => new($@"{LocalDirectory.FullName}\DB");
    private FileInfo ArticleDatabaseFileInfo => new($@"{DatabaseDirectory.FullName}\articles.csv");
    private FileInfo VolumeDatabaseFileInfo => new($@"{DatabaseDirectory.FullName}\volumes.csv");


    // ★★★★★★★★★★★★★★★ props

    public List<ResearchArticle> ArticleDatabase { get; set; } = null;
    public List<ResearchVolume> VolumeDatabase { get; set; } = null;

    private bool articleDatabaseUpdated = false;
    private bool volumeDatabaseUpdated = false;

    // ★★★★★★★★★★★★★★★ init

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="lineAccessToken"></param>
    public ResearchController(DirectoryInfo baseDir)
    {
        LocalDirectory = baseDir;

        Console.WriteLine("Data Powered by");
        Console.WriteLine("+ ");
        Console.WriteLine("+ J-STAGE (https://www.jstage.jst.go.jp/browse/-char/ja)");
        Console.WriteLine("+ ");
        Console.WriteLine();

        DatabaseDirectory.Create();

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

        try
        {
            VolumeDatabase = VolumeDatabaseFileInfo.ReadObjectFromLocalCsv<ResearchVolume>();
        }
        catch (FileNotFoundException)
        {
            VolumeDatabase = new List<ResearchVolume>();
        }
    }

    public void SaveDatabase(bool forceSaveArticleDatabase = false, bool forceSaveVolumeDatabase = false)
    {
        if (articleDatabaseUpdated || forceSaveArticleDatabase)
        {
            ArticleDatabase.SaveAsCsv(ArticleDatabaseFileInfo);
            articleDatabaseUpdated = false;
        }

        if (volumeDatabaseUpdated || forceSaveVolumeDatabase)
        {
            VolumeDatabase.SaveAsCsv(VolumeDatabaseFileInfo);
            volumeDatabaseUpdated = false;
        }

    }

    /// <summary>
    /// get data from dataserver and add to local dataase
    /// </summary>
    /// <param name="uriBuilder"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void GetDataAndRenewDB(JStageArticleSearchServiceUriBuilder uriBuilder)
    {
        // preprocess
        var addedCount = 0;
        if (ArticleDatabase == null)
            throw new InvalidOperationException("Database has not been opened yet. Use OpenDataBase() first.");


        // get xml
        var uri = uriBuilder.Build();
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

            };

            if (!ArticleDatabase.Any(a => a.DOI == article.DOI))
            {
                ArticleDatabase.Add(article);

                if (UtilConfig.ConsoleOutput_Contents)
                    Console.WriteLine($" + {article.Title_Japanese}");

                addedCount++;
                articleDatabaseUpdated = true;
            }

        }

        SaveDatabase();
        Console.WriteLine($"★ Added {addedCount} items to database");

    }

    /// <summary>
    /// get data from dataserver and add to local dataase
    /// </summary>
    /// <param name="uriBuilder"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void GetDataAndRenewDB(JStageVolumeServiceUriBuilder uriBuilder)
    {
        // preprocess
        var addedCount = 0;
        if (VolumeDatabase == null)
            throw new InvalidOperationException("Database has not been opened yet. Use OpenDataBase() first.");

        // get xml
        var uri = uriBuilder.Build();
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
            var volume = new ResearchVolume
            {

                Title_English = entry.Element(ExpandXml("vols_title"))?.Element(ExpandXml("en"))?.Value,
                Title_Japanese = entry.Element(ExpandXml("vols_title"))?.Element(ExpandXml("ja"))?.Value,

                Link_English = entry.Element(ExpandXml("vols_link"))?.Element(ExpandXml("en"))?.Value,
                Link_Japanese = entry.Element(ExpandXml("vols_link"))?.Element(ExpandXml("ja"))?.Value,

                PrintISSN = entry.Element(ExpandPrism("issn"))?.Value,
                OnlineISSN = entry.Element(ExpandPrism("eIssn"))?.Value,

                PublisherName_English = entry.Element(ExpandXml("publisher"))?.Element(ExpandXml("name"))?.Element(ExpandXml("en"))?.Value,
                PublisherName_Japanese = entry.Element(ExpandXml("publisher"))?.Element(ExpandXml("name"))?.Element(ExpandXml("ja"))?.Value,
                PublisherUri_English = entry.Element(ExpandXml("publisher"))?.Element(ExpandXml("url"))?.Element(ExpandXml("en"))?.Value,
                PublisherUri_Japanese = entry.Element(ExpandXml("publisher"))?.Element(ExpandXml("url"))?.Element(ExpandXml("ja"))?.Value,

                JournalCode_JStage = entry.Element(ExpandXml("cdjournal"))?.Value,

                MaterialTitle_English = entry.Element(ExpandXml("material_title"))?.Element(ExpandXml("en"))?.Value,
                MaterialTitle_Japanese = entry.Element(ExpandXml("material_title"))?.Element(ExpandXml("ja"))?.Value,

                Volume = entry.Element(ExpandPrism("volume"))?.Value,
                SubVolume = entry.Element(ExpandXml("cdvols"))?.Value,

                Number = entry.Element(ExpandPrism("number"))?.Value,
                StartingPage = entry.Element(ExpandPrism("startingPage"))?.Value,
                EndingPage = entry.Element(ExpandPrism("endingPage"))?.Value,

                PublishedYear = entry.Element(ExpandXml("pubyear"))?.Value,

                SystemCode = entry.Element(ExpandXml("systemcode"))?.Value,
                SystemName = entry.Element(ExpandXml("systemname"))?.Value,

                Title = entry.Element(ExpandXml("title"))?.Value,
                Link = entry.Element(ExpandXml("link"))?.Value,
                Id = entry.Element(ExpandXml("id"))?.Value,
                UpdatedOn = entry.Element(ExpandXml("updated"))?.Value,

            };

            if (!VolumeDatabase.Any(v => v.Id == volume.Id))
            {
                VolumeDatabase.Add(volume);

                if (UtilConfig.ConsoleOutput_Contents)
                    Console.WriteLine($" + {volume.MaterialTitle_Japanese} {volume.Title_Japanese}");

                addedCount++;
                volumeDatabaseUpdated = true;
            }

        }

        SaveDatabase();
        Console.WriteLine($"★ Added {addedCount} items to database");

    }


    // ★★★★★★★★★★★★★★★ helper

    private static string ExpandXml(string s) => "{http://www.w3.org/2005/Atom}" + s;
    private static string ExpandOpenSearch(string s) => "{http://a9.com/-/spec/opensearch/1.1/}" + s;
    private static string ExpandPrism(string s) => "{http://prismstandard.org/namespaces/basic/2.0/}" + s;


    // ★★★★★★★★★★★★★★★

}
