

namespace Aki32_Utilities.Console_App_Utilities.SpecificPurposeModels.Research;
/// <summary>
/// https://support.nii.ac.jp/ja/cir/r_opensearch
/// </summary>
public class CiNiiArticleUriBuilder : IResearchUriBuilder
{

    // ★★★★★★★★★★★★★★★ props

    public const string BASE_URL = $@"https://cir.nii.ac.jp/opensearch/all";

    internal Uri builtUri = null;

    public int? Start { get; set; }
    public int? Count { get; set; }

    public string ISSN { get; set; }
    public string DOI { get; set; }

    public string Title { get; set; }
    public string FreeWord { get; set; }
    public string Author { get; set; }

    public int? FromYear_or_YYYYMM { get; set; }
    public int? UntilYear_or_YYYYMM { get; set; }

    public bool? HasLinkToFullText { get; set; }

    public string Affiliation { get; set; }
    public string PublicationTitle { get; set; }
    public string LanguageType { get; set; }
    public string DataSourceType { get; set; }


    public string Language { get; set; }
    public int? Sortorder { get; set; }


    // ★★★★★★★★★★★★★★★ inits

    public CiNiiArticleUriBuilder()
    {
    }


    // ★★★★★★★★★★★★★★★ methods

    /// <summary>
    /// build uri
    /// </summary>
    /// <returns></returns>
    public Uri Build()
    {
        // 参照： https://support.nii.ac.jp/ja/cir/r_opensearch

        //return new Uri("https://cir.nii.ac.jp/opensearch/all?count=5&issn=1881-8153&format=json");

        {
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
            if (Sortorder != null)
                queryList.Add("sortorder", Sortorder!.ToString()!);


            //表示件数    count   ◯	◯	◯	◯	◯	◯	×	不可 デフォルト = 20、1〜200の自然数
            //自然数でない、または、0以下は、20
            //201以上は、200
            //format = htmlの場合、1ページあたりの表示件数を換算
            //1ページあたりの表示件数は、20、50、100、200に切り上げ
            //201以上は、200
            //検索結果一覧の
            if (Count != null)
                queryList.Add("count", Count!.ToString()!);


            //開始番号    start   ◯	◯	◯	◯	◯	◯	×	不可 自然数
            //それ以外は、
            //デフォルト = 1
            //format = htmlの場合、画面のページ数に換算
            //start ÷ (1ページあたりの表示件数)
            //あまりがある場合 + 1
            if (Start != null)
                queryList.Add("start", Start!.ToString()!);


            //フリーワード q	◯	◯	◯	◯	◯	◯	○	AND
            if (!string.IsNullOrEmpty(FreeWord))
                queryList.Add("q", FreeWord!.ToString()!);


            //人物名 creator ◯	◯	◯	◯	◯	◯	○	AND
            if (!string.IsNullOrEmpty(Author))
                queryList.Add("creator", Author!.ToString()!);


            //開始年 from    ◯	◯	◯	◯	-   ◯	×	不可 YYYY
            //YYYYMM
            if (FromYear_or_YYYYMM != null)
                queryList.Add("from", FromYear_or_YYYYMM!.ToString()!);


            //終了年 until   ◯	◯	◯	◯		◯	×	不可 YYYY
            //YYYYMM
            if (UntilYear_or_YYYYMM != null)
                queryList.Add("until", UntilYear_or_YYYYMM!.ToString()!);


            //本文あり    hasLinkToFullText   ◯	◯	◯	◯	◯		×	不可 trueまたはfalseを指定して検索します。本文あり検索をしたい場合はtrueを指定してください。
            //タイトル
            if (HasLinkToFullText != null)
                queryList.Add("hasLinkToFullText", HasLinkToFullText!.ToString()!);


            //研究課題名   title   ◯	◯	◯	◯	◯	◯	○	AND
            if (!string.IsNullOrEmpty(Title))
                queryList.Add("title", Title!.ToString()!);


            //所属機関    affiliation ◯	◯	◯	- -   ◯	○	AND
            if (!string.IsNullOrEmpty(Affiliation))
                queryList.Add("affiliation", Affiliation!.ToString()!);


            //刊行物名    publicationTitle -   ◯	◯	- - -   ○	AND
            if (!string.IsNullOrEmpty(PublicationTitle))
                queryList.Add("publicationTitle", PublicationTitle!.ToString()!);


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
            var builtUriString = $"{BASE_URL}?{string.Join("&", queryList.Select(x => $"{x.Key}={x.Value}"))}";
            return builtUri = new Uri(builtUriString);

        }


        // ★★★★★★★★★★★★★★★ 

    }
}
