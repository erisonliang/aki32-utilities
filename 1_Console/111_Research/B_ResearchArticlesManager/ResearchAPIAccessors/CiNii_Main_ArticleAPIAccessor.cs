using Aki32Utilities.ConsoleAppUtilities.General;

using Newtonsoft.Json.Linq;

namespace Aki32Utilities.ConsoleAppUtilities.Research;
/// <see cref="https://support.nii.ac.jp/ja/cir/r_opensearch"/>
public class CiNii_Main_ArticleAPIAccessor : IResearchAPIAccessor
{

    // ★★★★★★★★★★★★★★★ props

    public const string BASE_URL = $@"https://cir.nii.ac.jp/opensearch/all";

    internal Uri builtUri = null;


    public string ArticleTitle { get; set; }
    public string FreeWord { get; set; }
    public string AuthorName { get; set; }
    public string Affiliation { get; set; }
    public string MaterialTitle { get; set; }


    /// <summary>
    /// YYYY or YYYYMM
    /// </summary>
    public int? PublishedFrom { get; set; }
    /// <summary>
    /// YYYY or YYYYMM
    /// </summary>
    public int? PublishedUntil { get; set; }
    public int? RecordStart { get; set; }
    public int? RecordCount { get; set; }


    public string DOI { get; set; }
    public string ISSN { get; set; }
    public bool? HasLinkToFullText { get; set; }
    public string LanguageType { get; set; }
    public string DataSourceType { get; set; }
    public string Language { get; set; }
    public int? SortOrder { get; set; }


    // ★★★★★★★★★★★★★★★ inits

    public CiNii_Main_ArticleAPIAccessor()
    {
    }


    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// build uri
    /// </summary>
    /// <returns></returns>
    public Uri BuildUri()
    {
        // 参照： https://support.nii.ac.jp/ja/cir/r_opensearch

        //レスポンスフォーマット format      html(default), rss, atom, json
        var queryList = new Dictionary<string, string>
            {
                { "format", "json" },
            };

        //検索結果の言語 lang    ◯	◯	◯	◯	◯	◯	×	不可 出力フォーマットの記載言語指定, ja（デフォルト）, en
        if (!string.IsNullOrEmpty(Language))
            queryList.Add("lang", Language);


        //出力時のソート順 sortorder
        //0：出版年、学位授与年、研究開始年：新しい順,
        //1：出版年、学位授与年、研究開始年：古い順,
        //4：関連度順(デフォルト),
        //10：被引用件数：多い順(論文のみ), ページ当たり
        if (SortOrder != null)
            queryList.Add("sortorder", SortOrder!.ToString()!);


        //表示件数    count   ◯	◯	◯	◯	◯	◯	×	不可 デフォルト = 20、1〜200の自然数
        //自然数でない、または、0以下は、20
        //201以上は、200
        //format = htmlの場合、1ページあたりの表示件数を換算
        //1ページあたりの表示件数は、20、50、100、200に切り上げ
        //201以上は、200
        //検索結果一覧の
        if (RecordCount != null)
            queryList.Add("count", RecordCount!.ToString()!);


        //開始番号    start   ◯	◯	◯	◯	◯	◯	×	不可 自然数
        //それ以外は、
        //デフォルト = 1
        //format = htmlの場合、画面のページ数に換算
        //start ÷ (1ページあたりの表示件数)
        //あまりがある場合 + 1
        if (RecordStart != null)
            queryList.Add("start", RecordStart!.ToString()!);


        //フリーワード q	◯	◯	◯	◯	◯	◯	○	AND
        if (!string.IsNullOrEmpty(FreeWord))
            queryList.Add("q", FreeWord!.ToString()!);


        //人物名 creator ◯	◯	◯	◯	◯	◯	○	AND
        if (!string.IsNullOrEmpty(AuthorName))
            queryList.Add("creator", AuthorName!.ToString()!);


        //開始年 from    ◯	◯	◯	◯	-   ◯	×	不可 YYYY
        //YYYYMM
        if (PublishedFrom != null)
            queryList.Add("from", PublishedFrom!.ToString()!);


        //終了年 until   ◯	◯	◯	◯		◯	×	不可 YYYY
        //YYYYMM
        if (PublishedUntil != null)
            queryList.Add("until", PublishedUntil!.ToString()!);


        //本文あり    hasLinkToFullText   ◯	◯	◯	◯	◯		×	不可 trueまたはfalseを指定して検索します。本文あり検索をしたい場合はtrueを指定してください。
        //タイトル
        if (HasLinkToFullText != null)
            queryList.Add("hasLinkToFullText", HasLinkToFullText!.ToString()!);


        //研究課題名   title   ◯	◯	◯	◯	◯	◯	○	AND
        if (!string.IsNullOrEmpty(ArticleTitle))
            queryList.Add("title", ArticleTitle!.ToString()!);


        //所属機関    affiliation ◯	◯	◯	- -   ◯	○	AND
        if (!string.IsNullOrEmpty(Affiliation))
            queryList.Add("affiliation", Affiliation!.ToString()!);


        //刊行物名    publicationTitle -   ◯	◯	- - -   ○	AND
        if (!string.IsNullOrEmpty(MaterialTitle))
            queryList.Add("publicationTitle", MaterialTitle!.ToString()!);


        //ISSN    issn    ◯	◯	◯	◯	- -   ×	OR
        if (!string.IsNullOrEmpty(ISSN))
            queryList.Add("issn", ISSN!.ToString()!);


        //DOI doi ◯	◯	◯	-   ◯	-   ×	OR
        if (!string.IsNullOrEmpty(DOI))
            queryList.Add("doi", DOI!.ToString()!);


        //言語種別 languageType	◯	◯	◯	◯	◯	◯	×	OR ISO-639 - 1で指定する
        if (!string.IsNullOrEmpty(LanguageType))
            queryList.Add("languageType", LanguageType!.ToString()!);


        //データソース種別    dataSourceType  ◯	◯	◯	◯	◯	◯	×	不可 下記の文字列
        if (!string.IsNullOrEmpty(DataSourceType))
            queryList.Add("dataSourceType", DataSourceType!.ToString()!);


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
        // get json
        var uri = BuildUri();
        dynamic json = uri.CallAPIAsync_ForJsonData<dynamic>(HttpMethod.Get).Result;


        // analyse 
        var totalResults = json?["opensearch:totalResults"]?.ToString();
        var startIndex = json?["opensearch:startIndex"]?.ToString();
        var itemsPerPage = json?["opensearch:itemsPerPage"]?.ToString();
        var toIndex = int.Parse(startIndex) + int.Parse(itemsPerPage) - 1;

        Console.WriteLine();
        Console.WriteLine($"★ Obtained {itemsPerPage} item(s) out of {totalResults} matches ( From #{startIndex} to #{toIndex} )");
        Console.WriteLine();

        if (json?["items"] is JArray entities)
        {
            foreach (var entity in entities)
            {
                var article = new ResearchArticle();
                {
                    article.DataFrom_CiNii_Main = true;

                    article.CiNii_ArticleTitle = entity?["title"]?.ToString();

                    article.CiNii_Link = entity?["link"]?["@id"]?.ToString();

                    article.CiNii_Authors = (entity?["dc:creator"] as JArray)?.Select(a => a.ToString())?.ToArray();

                    article.CiNii_Publisher = entity?["dc:publisher"]?.ToString();
                    article.CiNii_PublicationName = entity?["prism:publicationName"]?.ToString();

                    article.PrintISSN = entity?["prism:issn"]?.ToString();

                    article.CiNii_MaterialVolume = entity?["prism:volume"]?.ToString();
                    article.CiNii_Number = entity?["prism:number"]?.ToString();
                    article.CiNii_StartingPage = entity?["prism:startingPage"]?.ToString();
                    article.CiNii_EndingPage = entity?["prism:endingPage"]?.ToString();
                    article.CiNii_PublishedDate = entity?["prism:publicationDate"]?.ToString();


                    article.CiNii_Description = entity?["description"]?.ToString();

                    article.DOI = (entity?["dc:identifier"] as JArray)?.FirstOrDefault(x => x?["@type"]?.ToString() == "cir:DOI")?["@value"]?.ToString();
                }

                yield return article;
            }
        }
    }


    // ★★★★★★★★★★★★★★★

}
