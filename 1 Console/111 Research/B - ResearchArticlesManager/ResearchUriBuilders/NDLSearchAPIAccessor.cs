

using System.Xml.Linq;

namespace Aki32Utilities.ConsoleAppUtilities.Research;
public class NDLSearchAPIAccessor : IResearchAPIAccessor
{

    // ★★★★★★★★★★★★★★★ props

    public const string BASE_URL = $@"http://api.jstage.jst.go.jp/searchapi/do";

    internal Uri builtUri = null;

    public int? Pubyearfrom { get; set; } = null;
    public int? Pubyearto { get; set; } = null;
    public string Material { get; set; } = "";

    public string Article { get; set; }
    public string Author { get; set; }
    public string Affil { get; set; }

    public string Keyword { get; set; }
    public string Abst { get; set; }
    public string Text { get; set; }

    public string Issn { get; set; } = "";
    public string Cdjournal { get; set; } = "";

    public bool? AscendingOrder { get; set; } = null;

    public string Vol { get; set; }
    public string No { get; set; }

    public int? Start { get; set; }
    public int? Count { get; set; }


    // ★★★★★★★★★★★★★★★ inits

    public NDLSearchAPIAccessor()
    {
    }


    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// build uri
    /// </summary>
    /// <returns></returns>
    public Uri BuildUri()
    {
        return new Uri(@"https://iss.ndl.go.jp/api/opensearch?mediatype=2&cnt=3&title=低サイク");


        //1 service 必須 利用する機能を指定します論文検索結果取得は 3 を指定
        var queryList = new Dictionary<string, string>
        {
            { "service", "3" }
        };

        //2 pubyearfrom 任意 発行年の範囲（From）を指定します西暦 4 桁
        if (Pubyearfrom != null)
            queryList.Add("pubyearfrom", Pubyearfrom!.ToString()!);

        //3 pubyearto 任意 発行年の範囲（To）を指定します西暦 4 桁
        if (Pubyearto != null)
            queryList.Add("pubyearto", Pubyearto!.ToString()!);

        //4 material 任意 資料名の検索語句を指定します中間一致検索大文字・小文字、全角・半角は区別しない
        if (!string.IsNullOrEmpty(Material))
            queryList.Add("material", Material);

        //5 article 任意 論文タイトルの検索語句を指定します中間一致検索大文字・小文字、全角・半角は区別しない
        if (!string.IsNullOrEmpty(Article))
            queryList.Add("article", Article);

        //6 author 任意 著者名の検索語句を指定します中間一致検索大文字・小文字、全角・半角は区別しない
        if (!string.IsNullOrEmpty(Author))
            queryList.Add("author", Author);

        //7 affil 任意 著者所属機関の検索語句を指定します中間一致検索大文字・小文字、全角・半角は区別しない
        if (!string.IsNullOrEmpty(Affil))
            queryList.Add("affil", Affil);

        //8 keyword 任意 キーワードの検索語句を指定します中間一致検索大文字・小文字、全角・半角は区別しない
        if (!string.IsNullOrEmpty(Keyword))
            queryList.Add("keyword", Keyword);

        //9 abst 任意 抄録の検索語句を指定します中間一致検索大文字・小文字、全角・半角は区別しない
        if (!string.IsNullOrEmpty(Abst))
            queryList.Add("abst", Abst);

        //10 text 任意 全文の検索語句を指定します中間一致検索大文字・小文字、全角・半角は区別しない
        if (!string.IsNullOrEmpty(Text))
            queryList.Add("text", Text);

        //11 issn 任意 Online ISSN またはPrint ISSN を指定します完全一致検索XXXX - XXXX 形式
        if (!string.IsNullOrEmpty(Issn))
            queryList.Add("issn", Issn);

        //12 cdjournal 任意 資料コードを指定しますJ - STAGE で付与される資料を識別するコード
        if (!string.IsNullOrEmpty(Cdjournal))
            queryList.Add("cdjournal", Cdjournal);

        //13 sortflg 任意 検索結果の並び順を指定します1:検索結果のスコア順にソートする2:巻、分冊、号、開始ページでソートする未指定の場合は 1
        if (AscendingOrder != null)
            queryList.Add("sortflg", AscendingOrder.Value ? "1" : "2");

        //14 vol 任意 巻を指定します 完全一致
        if (!string.IsNullOrEmpty(Vol))
            queryList.Add("vol", Vol);

        //15 no 任意 号を指定します 完全一致
        if (!string.IsNullOrEmpty(No))
            queryList.Add("no", No);

        //16 start 任意 検索結果の中から取得を開始する件数を指定します※
        if (Start != null)
            queryList.Add("start", Start!.Value.ToString());

        //17 count 任意 取得件数を指定します※ 最大 1,000 件まで取得可能
        if (Count != null)
            queryList.Add("count", Count!.Value.ToString());


        // post process
        var builtUriString = $"{BASE_URL}?{string.Join("&", queryList.Select(x => $"{x.Key}={x.Value}"))}";
        return builtUri = new Uri(builtUriString);

    }

    /// <summary>
    /// fetch articles
    /// </summary>
    /// <returns></returns>
    public IEnumerable<ResearchArticle> FetchArticles()
    {
        // get xml
        var xml = XElement.Load(BuildUri().AbsoluteUri);


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
            var article = new ResearchArticle();
            {
                article.DataFrom_JStage = true;

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

            yield return article;

        }
    }


    // ★★★★★★★★★★★★★★★ method helper

    public static string ExpandXml(string s) => "{http://www.w3.org/2005/Atom}" + s;
    public static string ExpandOpenSearch(string s) => "{http://a9.com/-/spec/opensearch/1.1/}" + s;
    public static string ExpandPrism(string s) => "{http://prismstandard.org/namespaces/basic/2.0/}" + s;


    // ★★★★★★★★★★★★★★★

}
