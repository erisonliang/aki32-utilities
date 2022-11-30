

namespace Aki32_Utilities.ExternalAPIControllers;
public class JStageVolumeServiceUriBuilder : IResearchUriBuilder
{

    // ★★★★★★★★★★★★★★★ props

    public const string BASE_URL = $@"http://api.jstage.jst.go.jp/searchapi/do";

    internal Uri builtUri = null;

    public string Pubyearfrom { get; set; } = "";
    public string Pubyearto { get; set; } = "";
    public string Material { get; set; } = "";
    public string Issn { get; set; } = "";
    public string Cdjournal { get; set; } = "";

    public bool? AscendingOrder { get; set; } = null;


    // ★★★★★★★★★★★★★★★ inits

    public JStageVolumeServiceUriBuilder()
    {
    }


    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// build uri
    /// </summary>
    /// <returns></returns>
    public Uri Build()
    {
        //1 service 必須  利用する機能を指定 巻号一覧取得は2、論文検索結果取得は3を指定
        var queryList = new Dictionary<string, string>
        {
            { "service", "2" }
        };

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


        // post process
        var builtUriString = $"{BASE_URL}?{string.Join("&", queryList.Select(x => $"{x.Key}={x.Value}"))}";
        return builtUri = new Uri(builtUriString);

    }


    // ★★★★★★★★★★★★★★★ 

}
