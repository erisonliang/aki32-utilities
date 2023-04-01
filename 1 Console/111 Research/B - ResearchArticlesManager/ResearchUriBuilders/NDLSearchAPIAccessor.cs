

using System.Xml.Linq;

using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.Properties;

using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Aki32Utilities.ConsoleAppUtilities.Research;
public class NDLSearchAPIAccessor : IResearchAPIAccessor
{

    // ★★★★★★★★★★★★★★★ props

    public const string BASE_URL = $@"https://iss.ndl.go.jp/api/opensearch";

    internal Uri builtUri = null;


    /// <summary>
    /// 資料種別//国立国会図書館サーチの詳細検索の資料種別に対応
    /// “1”：本
    /// “2”：記事・論文
    /// “3”：新聞
    /// “4”：児童書
    /// “5”：レファレンス情報
    /// “6”：デジタル資料
    /// “7”：その他
    /// “8”：障害者向け資料（障害者向け検索対象資料）
    /// “9”：立法情報
    /// </summary>
    public int? MediaType { get; set; } = 2;


    public string FreeWord { get; set; }
    public string ArticleTitle { get; set; }
    public string AuthorName { get; set; }
    public string PublisherName { get; set; }
    public string DigitizedPublisherName { get; set; }


    /// <summary>
    /// YYYY, YYYYMM or YYYYMMDD
    /// </summary>
    public int? PublishedFrom { get; set; } = null;
    /// <summary>
    /// YYYY, YYYYMM or YYYYMMDD
    /// </summary>
    public int? PublishedUntil { get; set; } = null;
    public int? RecordCount { get; set; } = null;
    public int? RecordStart { get; set; } = null;


    public string DataProviderId { get; set; }
    public string DataProviderGroupId { get; set; }
    public string NDC { get; set; }
    public string ISBN { get; set; }


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
        //mediatype=2&cnt=3&title=低サイク
        //return new Uri(@"https://iss.ndl.go.jp/api/opensearch?mediatype=2&cnt=3&title=低サイク");

        var queryList = new Dictionary<string, string>();


        // 1//dpid//データプロバイダID//完全一致//○
        if (!string.IsNullOrEmpty(DataProviderId))
            queryList.Add("dpid", DataProviderId);

        // 2//dpgroupid//データプロバイダグループID//完全一致//×
        if (!string.IsNullOrEmpty(DataProviderGroupId))
            queryList.Add("dpgroupid", DataProviderGroupId);

        // 3//any//すべての項目を対象に検索//部分一致//○
        if (!string.IsNullOrEmpty(FreeWord))
            queryList.Add("any", FreeWord);

        // 4//title//タイトル//部分一致//○
        if (!string.IsNullOrEmpty(ArticleTitle))
            queryList.Add("title", ArticleTitle);

        // 5//creator//作成者//部分一致//○
        if (!string.IsNullOrEmpty(AuthorName))
            queryList.Add("creator", AuthorName);

        // 6//publisher//出版者//部分一致//○
        if (!string.IsNullOrEmpty(PublisherName))
            queryList.Add("publisher", PublisherName);

        // 7//digitized_publisher//デジタル化した製作者//部分一致//○
        if (!string.IsNullOrEmpty(DigitizedPublisherName))
            queryList.Add("digitized_publisher", DigitizedPublisherName);

        // 8//ndc//分類（NDC）//前方一致//×
        if (!string.IsNullOrEmpty(NDC))
            queryList.Add("ndc", NDC);

        // 9//from//開始出版年月日//（YYYY、YYYY-MM、YYYY-MM-DD）//×
        if (PublishedFrom != null)
        {
            var fromString = PublishedFrom.ToString()!;
            if (PublishedFrom > 999999)
                fromString = fromString.Insert(6, "-");
            if (PublishedFrom > 9999)
                fromString = fromString.Insert(4, "-");

            queryList.Add("from", fromString);
        }

        // 10//until//終了出版年月日//（YYYY、YYYY-MM、YYYY-MM-DD）//×
        if (PublishedUntil != null)
        {
            var untilString = PublishedUntil.ToString()!;
            if (PublishedUntil > 999999)
                untilString = untilString.Insert(6, "-");
            if (PublishedUntil > 9999)
                untilString = untilString.Insert(4, "-");

            queryList.Add("until", untilString);
        }

        // 11//cnt//出力レコード上限値（省略時は200 とする）//×
        if (RecordCount != null)
            queryList.Add("cnt", RecordCount!.ToString()!);

        // 12 //idx //レコード取得開始位置（省略時は1 とする） //×
        if (RecordStart != null)
            queryList.Add("idx", RecordStart!.ToString()!);

        // 13//isbn//ISBN
        // 10桁または13桁で入力した場合は、10桁、13桁の両方に変換して完全一致検索を行う。
        // それ以外の桁で入力した場合は前方一致検索を行う。
        // 完全一致または前方一致//×
        if (!string.IsNullOrEmpty(ISBN))
            queryList.Add("isbn", ISBN);

        // 14//mediatype//資料種別//国立国会図書館サーチの詳細検索の資料種別に対応
        // “1”：本
        // “2”：記事・論文
        // “3”：新聞
        // “4”：児童書
        // “5”：レファレンス情報
        // “6”：デジタル資料
        // “7”：その他
        // “8”：障害者向け資料（障害者向け検索対象資料）
        // “9”：立法情報
        // 完全一致//○
        if (MediaType != null)
            queryList.Add("mediatype", MediaType!.ToString()!);


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
        var xml = XElement.Load(BuildUri().AbsoluteUri);


        // analyse 
        var channel = xml.Element("channel")!;
        var totalResults = channel.Element(ExpandOpenSearch("totalResults"))!.Value;
        var startIndex = channel.Element(ExpandOpenSearch("startIndex"))!.Value;
        var itemsPerPage = channel.Element(ExpandOpenSearch("itemsPerPage"))!.Value;
        var toIndex = int.Parse(startIndex) + int.Parse(itemsPerPage) - 1;

        Console.WriteLine();
        Console.WriteLine($"★ Obtained {itemsPerPage} items out of {totalResults} matches ( From #{startIndex} to #{toIndex} )");
        Console.WriteLine();

        var entities = channel.Elements("item");
        foreach (var entity in entities)
        {
            var article = new ResearchArticle();
            {
                article.DataFrom_NDLSearch = true;

                // タイトルはいつもここ
                article.NDLSearch_ArticleTitle = entity.Element("title")?.Value;

                // １，NDL Onlineなどの，より詳細に近い情報に到達する。
                // ２，検索情報。
                article.NDLSearch_Link = entity.Element(ExpandRdfs("seeAlso"))?.Attribute(ExpandRdf("resource"))?.Value
                    ?? entity.Element("link")?.Value;

                // １，より内容の説明に触れている。
                // ２，出版社などの情報
                article.NDLSearch_Description = entity.Element("description")?.Value
                    ?? entity.Elements(ExpandDC("description"))?.Select(e => e?.Value ?? "")?.Join(", ");

                // １，ほぼ数値のみ。
                // ２，ほぼ数値のみ。
                // ３，長い表現文字列になっている。
                article.NDLSearch_PublishedDate = entity.Element(ExpandDcTerms("issued"))?.Value
                    ?? entity.Element(ExpandDC("date"))?.Value
                    ?? entity.Element("pubDate")?.Value;

                // １，著者リストの文字列
                // ２，著者がElementsとして保存。
                article.NDLSearch_Authors = entity.Element("author")?.Value?
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)?
                    .Select(a => a.Trim())?
                    .Where(a => !string.IsNullOrEmpty(a))?
                    .Distinct()?
                    .ToArray()
                    ?? entity.Elements(ExpandDC("creator"))?
                    .Select(e => e?.Value ?? "")?
                    .Where(a => !string.IsNullOrEmpty(a))?
                    .ToArray();

                // DOIのタグはここ！
                // 詳細： https://www.ndl.go.jp/jp/dlib/cooperation/doi.html
                article.DOI = entity.Elements(ExpandDC("identifier"))?.FirstOrDefault(i => i.Attribute(ExpandXsi("type"))?.Value == "dcndl:DOI")?.Value;

            }

            yield return article;

        }
    }


    // ★★★★★★★★★★★★★★★ method helper

    private static string ExpandDC(string s) => "{http://purl.org/dc/elements/1.1/}" + s;
    private static string ExpandOpenSearch(string s) => "{http://a9.com/-/spec/opensearchrss/1.0/}" + s;
    private static string ExpandDcTerms(string s) => "{http://purl.org/dc/terms/}" + s;
    private static string ExpandRdfs(string s) => "{http://www.w3.org/2000/01/rdf-schema#}" + s;
    private static string ExpandRdf(string s) => "{http://www.w3.org/1999/02/22-rdf-syntax-ns#}" + s;
    private static string ExpandXsi(string s) => "{http://www.w3.org/2001/XMLSchema-instance}" + s;


    // ★★★★★★★★★★★★★★★

}
