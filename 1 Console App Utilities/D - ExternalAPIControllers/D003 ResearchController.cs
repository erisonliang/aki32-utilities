using System.Collections.Generic;
using System.Data;
using System.Net.Http.Headers;
using System.Xml.Linq;

using Aki32_Utilities.General;
using Aki32_Utilities.UsefulClasses;

using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;

using static Aki32_Utilities.ExternalAPIControllers.JStageUriBuilder;

using static ClosedXML.Excel.XLPredefinedFormat;

namespace Aki32_Utilities.ExternalAPIControllers;

/// <summary>
/// </summary>
/// <remarks>
/// 
/// 参考：
/// https://www.jstage.jst.go.jp/static/files/ja/manual_api.pdf
/// https://ooooooha.hatenablog.com/entry/2017/05/03/023516
/// 
/// </remarks>
public partial class ResearchController
{

    // ★★★★★★★★★★★★★★★ paths

    public DirectoryInfo LocalDirectory { get; set; }
    public DirectoryInfo DatabaseDirectory => new($@"{LocalDirectory.FullName}\DB");
    public FileInfo VolumeDatabase => new($@"{DatabaseDirectory.FullName}\volumes.xml");
    public FileInfo ArticleDB => new($@"{DatabaseDirectory.FullName}\articles.xml");

    public const string TEST_URL = "http://api.jstage.jst.go.jp/searchapi/do?service=3&pubyearfrom=2022&issn=1881-8153&count=5";


    // ★★★★★★★★★★★★★★★ props

    public List<ResearchArticle> Articles { get; set; } = new List<ResearchArticle>();
    public List<ResearchVolume> Volumes { get; set; } = new List<ResearchVolume>();

    // ★★★★★★★★★★★★★★★ init

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="lineAccessToken"></param>
    public ResearchController(DirectoryInfo baseDir)
    {
        LocalDirectory = baseDir;

        Console.WriteLine("Data Powered by");
        Console.WriteLine("+ J-STAGE (https://www.jstage.jst.go.jp/browse/-char/ja)");
        Console.WriteLine("+ ");
        Console.WriteLine("+ ");
        Console.WriteLine();

        DatabaseDirectory.Create();

    }


    // ★★★★★★★★★★★★★★★ methods

    public void GetDataAndRenewDB(JStageUriBuilder jsUriBuilder)
    {
        // get xml
        var uri = jsUriBuilder.Build();
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
            switch (jsUriBuilder.Service)
            {
                case JStageWebAPIService.GetVolumeListService:
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


                        Console.WriteLine(" + " + volume.MaterialTitle_Japanese + volume.Title_Japanese);

                    }
                    break;
                case JStageWebAPIService.GetArticleSearchService:
                    {

                        var article = new ResearchArticle
                        {

                            Title_English = entry.Element(ExpandXml("article_title"))?.Element(ExpandXml("en"))?.Value,
                            Title_Japanese = entry.Element(ExpandXml("article_title"))?.Element(ExpandXml("ja"))?.Value,

                            Link_English = entry.Element(ExpandXml("article_link"))?.Element(ExpandXml("en"))?.Value,
                            Link_Japanese = entry.Element(ExpandXml("article_link"))?.Element(ExpandXml("ja"))?.Value,

                            Authors_English = entry.Element(ExpandXml("author"))?.Element(ExpandXml("en"))?.Elements("name")?.Select(e => e?.Value ?? "")?.ToArray(),
                            Authors_Japanese = entry.Element(ExpandXml("author"))?.Element(ExpandXml("ja"))?.Elements("name")?.Select(e => e?.Value ?? "")?.ToArray(),

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

                        Console.WriteLine(" - " + article.Title_Japanese);

                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

    }

    public void OpenArticleDBFromLocal()
    {
        var db = ArticleDB.ReadObjectFromLocalXml<object>();


    }



    private string ExpandXml(string s) => "{http://www.w3.org/2005/Atom}" + s;
    private string ExpandOpenSearch(string s) => "{http://a9.com/-/spec/opensearch/1.1/}" + s;
    private string ExpandPrism(string s) => "{http://prismstandard.org/namespaces/basic/2.0/}" + s;

    // ★★★★★★★★★★★★★★★

}
