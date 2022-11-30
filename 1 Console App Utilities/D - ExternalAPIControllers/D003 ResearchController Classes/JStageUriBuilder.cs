

namespace Aki32_Utilities.ExternalAPIControllers;
public class JStageUriBuilder : IResearchUriBuilder
{

    // ★★★★★★★★★★★★★★★ props

    public const string BASE_URL = $@"http://api.jstage.jst.go.jp/searchapi/do";

    internal Uri builtUri = null;

    public JStageWebAPIService Service { get; set; }

    public string Pubyearfrom { get; set; } = "";
    public string Pubyearto { get; set; } = "";
    public string Material { get; set; } = "";
    public string Issn { get; set; } = "";
    public string Cdjournal { get; set; } = "";


    public string Article { get; set; }
    public string Abst { get; set; }
    public string Text { get; set; }
    public string Keyword { get; set; }

    public string Affil { get; set; }
    public string Author { get; set; }

    public string Start { get; set; } = "";
    public string Count { get; set; } = "";

    public string No { get; set; }
    public string Vol { get; set; }

    public bool? AscendingOrder { get; set; } = null;

    // ★★★★★★★★★★★★★★★ inits

    public JStageUriBuilder(JStageWebAPIService service)
    {
        Service = service;
    }


    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// build uri
    /// </summary>
    /// <returns></returns>
    public Uri Build()
    {
        var queryList = new Dictionary<string, string>();

        switch (Service)
        {
            case JStageWebAPIService.GetVolumeListService:
                {

                    //1 service 必須  利用する機能を指定 巻号一覧取得は2、論文検索結果取得は3を指定
                    queryList.Add("service", "2");

                    //2 pubyearfrom 任意  発行年を指定(from)    西暦４桁
                    if (!string.IsNullOrEmpty(Pubyearfrom))
                        queryList.Add("pubyearfrom", Pubyearfrom);

                    //3 pubyearto 任意  発行年を指定(to)  西暦４桁
                    if (!string.IsNullOrEmpty(Pubyearto))
                        queryList.Add("pubyearto", Pubyearto);

                    //4 material 任意  資料名の検索語句を指定 完全一致検索
                    if (!string.IsNullOrEmpty(Material))
                        queryList.Add("material", Material);

                    //5 issn 任意  onlineISSN またはPrintISSNを指定  完全一致検索 xxxx-xxxx形式
                    if (!string.IsNullOrEmpty(Issn))
                        queryList.Add("issn", Issn);

                    //6 cdjournal 任意  資料コードを指定 J-STAGEで付与される資料を識別するコード
                    if (!string.IsNullOrEmpty(Cdjournal))
                        queryList.Add("cdjournal", Cdjournal);

                    //7 volorder 任意  巻の並び順を指定    1:昇順, 2:降順, 未指定時は1
                    if (AscendingOrder != null)
                        queryList.Add("volorder", AscendingOrder.Value ? "1" : "2");

                }
                break;
            case JStageWebAPIService.GetArticleSearchService:
                {

                    //1 service 必須 利用する機能を指定します論文検索結果取得は 3 を指定
                    queryList.Add("service", "3");

                    //2 pubyearfrom 任意 発行年の範囲（From）を指定します西暦 4 桁
                    if (!string.IsNullOrEmpty(Pubyearfrom))
                        queryList.Add("pubyearfrom", Pubyearfrom);

                    //3 pubyearto 任意 発行年の範囲（To）を指定します西暦 4 桁
                    if (!string.IsNullOrEmpty(Pubyearto))
                        queryList.Add("pubyearto", Pubyearto);

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
                    if (!string.IsNullOrEmpty(Start))
                        queryList.Add("start", Start);

                    //17 count 任意 取得件数を指定します※ 最大 1,000 件まで取得可能
                    if (!string.IsNullOrEmpty(Count))
                        queryList.Add("count", Count);

                }
                break;
            default:
                throw new NotImplementedException();
        }


        var builtUriString = $"{BASE_URL}?{string.Join("&", queryList.Select(x => $"{x.Key}={x.Value}"))}";

        return builtUri = new Uri(builtUriString);

    }


    // ★★★★★★★★★★★★★★★ sub classes


    public static class ISSN
    {

        public const string Architecture_Structure = $@"1881-8153";

        public const string Architecture_Environment = $@"1881-817X";

        public const string Architecture_Plan = $@"1881-8161";

    }


    // ★★★★★★★★★★★★★★★ 

}

public enum JStageWebAPIService
{
    GetVolumeListService,
    GetArticleSearchService,
}