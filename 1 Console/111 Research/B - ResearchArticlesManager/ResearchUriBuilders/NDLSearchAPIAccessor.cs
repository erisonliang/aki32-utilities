

using System.Xml.Linq;

using Aki32Utilities.ConsoleAppUtilities.General;

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


    public string SearchFreeWord { get; set; }
    public string SearchTitle { get; set; }
    public string SearchAuthorName { get; set; }
    public string SearchPublisherName { get; set; }
    public string SearchDigitizedPublisherName { get; set; }


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
        if (!string.IsNullOrEmpty(SearchFreeWord))
            queryList.Add("any", SearchFreeWord);

        // 4//title//タイトル//部分一致//○
        if (!string.IsNullOrEmpty(SearchTitle))
            queryList.Add("title", SearchTitle);

        // 5//creator//作成者//部分一致//○
        if (!string.IsNullOrEmpty(SearchAuthorName))
            queryList.Add("creator", SearchAuthorName);

        // 6//publisher//出版者//部分一致//○
        if (!string.IsNullOrEmpty(SearchPublisherName))
            queryList.Add("publisher", SearchPublisherName);

        // 7//digitized_publisher//デジタル化した製作者//部分一致//○
        if (!string.IsNullOrEmpty(SearchDigitizedPublisherName))
            queryList.Add("digitized_publisher", SearchDigitizedPublisherName);

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

                article.NDLSearch_ArticleTitle = entity.Element("title")?.Value;
                article.NDLSearch_Link = entity.Element("link")?.Value;

                article.NDLSearch_Description = entity.Element("description")?.Value
                    ?? entity.Elements(ExpandDC("description"))?.Select(e => e?.Value ?? "")?.Join(", ");

                article.NDLSearch_PublishedDate = entity.Element(ExpandDcTerms("issued"))?.Value
                    ?? entity.Element("pubDate")?.Value;

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

            }

            yield return article;

        }
    }


    // ★★★★★★★★★★★★★★★ method helper

    private static string ExpandDC(string s) => "{http://purl.org/dc/elements/1.1/}" + s;
    private static string ExpandOpenSearch(string s) => "{http://a9.com/-/spec/opensearchrss/1.0/}" + s;
    private static string ExpandDcTerms(string s) => "{http://purl.org/dc/terms/}" + s;


    // ★★★★★★★★★★★★★★★

}
