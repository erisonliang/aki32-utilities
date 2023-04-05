

using System.Xml.Linq;
using System;

namespace Aki32Utilities.ConsoleAppUtilities.Research;
public class JStage_Main_ArticleAPIAccessor : IResearchAPIAccessor
{

    // ★★★★★★★★★★★★★★★ props

    public const string BASE_URL = $@"http://api.jstage.jst.go.jp/searchapi/do";

    internal Uri builtUri = null;


    public string ArticleTitle { get; set; }
    public string AuthorName { get; set; }
    public string Affiliation { get; set; }
    public string MaterialTitle { get; set; }
    public string KeyWord { get; set; }
    public string Abstract { get; set; }
    public string Text { get; set; }

    /// <summary>
    /// YYYY
    /// </summary>
    public int? PublishedFrom { get; set; } = null;
    /// <summary>
    /// YYYY
    /// </summary>
    public int? PublishedUntil { get; set; } = null;
    public int? RecordStart { get; set; }
    public int? RecordCount { get; set; }

    public string ISSN { get; set; }
    public string JournalCode { get; set; }
    public bool? IsAscendingOrder { get; set; } = null;
    public string MaterialVolume { get; set; }
    public string MaterialSubVolume { get; set; }


    // ★★★★★★★★★★★★★★★ inits

    public JStage_Main_ArticleAPIAccessor()
    {
    }


    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// build uri
    /// </summary>
    /// <returns></returns>
    public Uri BuildUri()
    {
        //1 service 必須 利用する機能を指定します論文検索結果取得は 3 を指定
        var queryList = new Dictionary<string, string>
        {
            { "service", "3" }
        };

        //2 pubyearfrom 任意 発行年の範囲（From）を指定します西暦 4 桁
        if (PublishedFrom != null)
            queryList.Add("pubyearfrom", PublishedFrom!.ToString()!);

        //3 pubyearto 任意 発行年の範囲（To）を指定します西暦 4 桁
        if (PublishedUntil != null)
            queryList.Add("pubyearto", PublishedUntil!.ToString()!);

        //4 material 任意 資料名の検索語句を指定します中間一致検索大文字・小文字、全角・半角は区別しない
        if (!string.IsNullOrEmpty(MaterialTitle))
            queryList.Add("material", MaterialTitle);

        //5 article 任意 論文タイトルの検索語句を指定します中間一致検索大文字・小文字、全角・半角は区別しない
        if (!string.IsNullOrEmpty(ArticleTitle))
            queryList.Add("article", ArticleTitle);

        //6 author 任意 著者名の検索語句を指定します中間一致検索大文字・小文字、全角・半角は区別しない
        if (!string.IsNullOrEmpty(AuthorName))
            queryList.Add("author", AuthorName);

        //7 affil 任意 著者所属機関の検索語句を指定します中間一致検索大文字・小文字、全角・半角は区別しない
        if (!string.IsNullOrEmpty(Affiliation))
            queryList.Add("affil", Affiliation);

        //8 keyword 任意 キーワードの検索語句を指定します中間一致検索大文字・小文字、全角・半角は区別しない
        if (!string.IsNullOrEmpty(KeyWord))
            queryList.Add("keyword", KeyWord);

        //9 abst 任意 抄録の検索語句を指定します中間一致検索大文字・小文字、全角・半角は区別しない
        if (!string.IsNullOrEmpty(Abstract))
            queryList.Add("abst", Abstract);

        //10 text 任意 全文の検索語句を指定します中間一致検索大文字・小文字、全角・半角は区別しない
        if (!string.IsNullOrEmpty(Text))
            queryList.Add("text", Text);

        //11 issn 任意 Online ISSN またはPrint ISSN を指定します完全一致検索XXXX - XXXX 形式
        if (!string.IsNullOrEmpty(ISSN))
            queryList.Add("issn", ISSN);

        //12 cdjournal 任意 資料コードを指定しますJ - STAGE で付与される資料を識別するコード
        if (!string.IsNullOrEmpty(JournalCode))
            queryList.Add("cdjournal", JournalCode);

        //13 sortflg 任意 検索結果の並び順を指定します1:検索結果のスコア順にソートする2:巻、分冊、号、開始ページでソートする未指定の場合は 1
        if (IsAscendingOrder != null)
            queryList.Add("sortflg", IsAscendingOrder.Value ? "1" : "2");

        //14 vol 任意 巻を指定します 完全一致
        if (!string.IsNullOrEmpty(MaterialVolume))
            queryList.Add("vol", MaterialVolume);

        //15 no 任意 号を指定します 完全一致
        if (!string.IsNullOrEmpty(MaterialSubVolume))
            queryList.Add("no", MaterialSubVolume);

        //16 start 任意 検索結果の中から取得を開始する件数を指定します※
        if (RecordStart != null)
            queryList.Add("start", RecordStart!.Value.ToString());

        //17 count 任意 取得件数を指定します※ 最大 1,000 件まで取得可能
        if (RecordCount != null)
            queryList.Add("count", RecordCount!.Value.ToString());


        // post process
        var query = new FormUrlEncodedContent(queryList).ReadAsStringAsync().Result;
        return builtUri = new Uri($"{BASE_URL}?{query}");

    }

    /// <summary>
    /// fetch articles
    /// </summary>
    /// <returns></returns>
    public async Task<List<ResearchArticle>> FetchArticles()
    {
        return await Task.Run(() => _FetchArticles().ToList());
    }
    private IEnumerable<ResearchArticle> _FetchArticles()
    {
        // get xml
        var uri = BuildUri();
        var xml = XElement.Load(uri.AbsoluteUri);


        // analyse 
        var totalResults = xml.Element(ExpandOpenSearch("totalResults"))!.Value;
        var startIndex = xml.Element(ExpandOpenSearch("startIndex"))!.Value;
        var itemsPerPage = xml.Element(ExpandOpenSearch("itemsPerPage"))!.Value;
        var toIndex = int.Parse(startIndex) + int.Parse(itemsPerPage) - 1;

        Console.WriteLine();
        Console.WriteLine($"★ Obtained {itemsPerPage} item(s) out of {totalResults} matches ( From #{startIndex} to #{toIndex} )");
        Console.WriteLine();

        var entities = xml.Elements(ExpandAtom("entry"));
        foreach (var entity in entities)
        {
            var article = new ResearchArticle();
            {
                article.DataFrom_JStage = true;

                article.JStage_ArticleTitle_English = entity.Element(ExpandAtom("article_title"))?.Element(ExpandAtom("en"))?.Value;
                article.JStage_ArticleTitle_Japanese = entity.Element(ExpandAtom("article_title"))?.Element(ExpandAtom("ja"))?.Value;

                article.JStage_Link_English = entity.Element(ExpandAtom("article_link"))?.Element(ExpandAtom("en"))?.Value;
                article.JStage_Link_Japanese = entity.Element(ExpandAtom("article_link"))?.Element(ExpandAtom("ja"))?.Value;

                article.JStage_Authors_English = entity.Element(ExpandAtom("author"))?.Element(ExpandAtom("en"))?.Elements(ExpandAtom("name"))?.Select(e => e?.Value ?? "")?.ToArray();
                article.JStage_Authors_Japanese = entity.Element(ExpandAtom("author"))?.Element(ExpandAtom("ja"))?.Elements(ExpandAtom("name"))?.Select(e => e?.Value ?? "")?.ToArray();

                article.JStage_JournalCode = entity.Element(ExpandAtom("cdjournal"))?.Value;

                article.JStage_MaterialTitle_English = entity.Element(ExpandAtom("material_title"))?.Element(ExpandAtom("en"))?.Value;
                article.JStage_MaterialTitle_Japanese = entity.Element(ExpandAtom("material_title"))?.Element(ExpandAtom("ja"))?.Value;

                article.PrintISSN = entity.Element(ExpandPrism("issn"))?.Value;
                article.OnlineISSN = entity.Element(ExpandPrism("eIssn"))?.Value;

                article.JStage_MaterialVolume = entity.Element(ExpandPrism("volume"))?.Value;
                article.JStage_MaterialSubVolume = null
                    ?? entity.Element(ExpandPrism("number"))?.Value
                    ?? entity.Element(ExpandAtom("cdvols"))?.Value
                    ;

                article.JStage_StartingPage = entity.Element(ExpandPrism("startingPage"))?.Value;
                article.JStage_EndingPage = entity.Element(ExpandPrism("endingPage"))?.Value;

                article.JStage_PublishedYear = entity.Element(ExpandAtom("pubyear"))?.Value;

                article.JStage_JOI = entity.Element(ExpandAtom("joi"))?.Value;
                article.DOI = entity.Element(ExpandPrism("doi"))?.Value;

                article.JStage_SystemCode = entity.Element(ExpandAtom("systemcode"))?.Value;
                article.JStage_SystemName = entity.Element(ExpandAtom("systemname"))?.Value;

                //article.JStage_ArticleTitle = entity.Element(ExpandXml("title"))?.Value;

                //article.JStage_Link = entity.Element(ExpandXml("link"))?.Value;
                article.JStage_Id = entity.Element(ExpandAtom("id"))?.Value;
                article.JStage_UpdatedOn = entity.Element(ExpandAtom("updated"))?.Value;

            }

            yield return article;

        }
    }


    // ★★★★★★★★★★★★★★★ method helper

    private static string ExpandAtom(string s) => "{http://www.w3.org/2005/Atom}" + s;
    private static string ExpandOpenSearch(string s) => "{http://a9.com/-/spec/opensearch/1.1/}" + s;
    private static string ExpandPrism(string s) => "{http://prismstandard.org/namespaces/basic/2.0/}" + s;

    // ★★★★★★★★★★★★★★★ 

}
