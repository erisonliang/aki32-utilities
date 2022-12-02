using Aki32_Utilities.General;

using ClosedXML;

using DocumentFormat.OpenXml.Bibliography;

namespace Aki32_Utilities.SpecificPurposeModels.Research;
public class ResearchArticle : IComparable
{

    // ★★★★★★★★★★★★★★★ prop

    // ★★★★★ shared info (*main common info)

    public string? ArticleTitle
    {
        get
        {
            return null
                ?? Manual_ArticleTitle
                ?? JStage_ArticleTitle_Japanese
                ?? CrossRef_ArticleTitle
                ?? JStage_ArticleTitle_English

                // 最終手段。
                ?? UnstructuredRefString
                ?? null
                ;
        }
    }
    public string[]? Authors
    {
        get
        {
            return null
                ?? Manual_Authors
                ?? JStage_Authors_Japanese
                ?? CrossRef_Authors
                ?? JStage_Authors_English

                // 最終手段。
                ?? ((UnstructuredRefString == null) ? null : new string[] { UnstructuredRefString })
                ?? null
                ;
        }
    }

    public string? DOI { get; set; }

    private string? __UnstructuredRefString;
    public string? UnstructuredRefString
    {
        get
        {
            return __UnstructuredRefString;
        }
        set
        {
            __UnstructuredRefString = value;
        }
    }

    public string? PrintISSN { get; set; }
    public string? OnlineISSN { get; set; }




    // ★★★★★ shared links (*main common info)

    public string? DOI_Link => (DOI == null) ? null : $"https://dx.doi.org/{DOI}";

    [CsvIgnore]
    public string? CrossRefAPI_Link => (DOI == null) ? null : $"https://api.crossref.org/v1/works/{DOI}";

    public string? PDF_Link
    {
        get
        {
            if (DOI == null)
                return null;

            // get data from aij
            if (DOI.Contains("aijs"))
            {
                return JStage_Link?.Replace($"_article/-char/ja/", $"_pdf");
            }

            return null;
        }
    }

    public IEnumerable<string> LocalPDFNameCandidates
    {
        get
        {
            if (AOI != null)
                yield return AOI;

            if (DOI != null)
                yield return DOI.Replace("/", "_");

            if (PDF_Link != null)
            {
                var candidate = PDF_Link.Replace("/", "_").Replace(":", "_");
                yield return (candidate.Length > 30) ? candidate[^29..] : candidate;
            }

        }
    }




    // ★★★★★ original meta info

    /// <summary>
    /// favorite flag
    /// </summary>
    public bool? Private_Favorite { get; set; }

    public bool? DataFrom_Manual { get; set; }
    public bool? DataFrom_JStage { get; set; }
    public bool? DataFrom_CiNii { get; set; }
    public bool? DataFrom_CrossRef { get; set; }

    /// <summary>
    /// Aki32 Object Identifier
    /// When DOI does not exist, automatically create AOI to connect ref data.
    /// 
    /// Put your pdf in {LocalPath}\PDFs\Manual\{ManuallyAddedPdfName}.pdf
    /// </summary>
    /// <remarks>
    /// AOIで接続するのは，本当に最終手段。
    /// </remarks>
    public string? AOI { get; set; }


    // ★★★★★ manual info

    public string? Manual_ArticleTitle { get; set; }
    public string[]? Manual_Authors { get; set; }




    // ★★★★★ CrossRef

    public string[]? CrossRef_ReferenceDOIs { get; set; }

    public string? CrossRef_ArticleTitle { get; set; }
    public string[]? CrossRef_Authors { get; set; }

    public string? CrossRef_PublishedDate { get; set; }




    // ★★★★★ mainly from J-Stage

    public string? JStage_ArticleTitle_English { get; set; }
    public string? JStage_ArticleTitle_Japanese { get; set; }

    public string? JStage_Link_English { get; set; }
    public string? JStage_Link_Japanese { get; set; }

    public string[]? JStage_Authors_English { get; set; }
    public string[]? JStage_Authors_Japanese { get; set; }

    public string? JStage_JournalCode { get; set; }

    public string? JStage_MaterialTitle_English { get; set; }
    public string? JStage_MaterialTitle_Japanese { get; set; }


    public string? JStage_Volume { get; set; }
    public string? JStage_SubVolume { get; set; }

    public string? JStage_Number { get; set; }
    public string? JStage_StartingPage { get; set; }
    public string? JStage_EndingPage { get; set; }

    public string? JStage_PublishedYear { get; set; }

    public string? JStage_JOI { get; set; }

    public string? JStage_SystemCode { get; set; }
    public string? JStage_SystemName { get; set; }

    public string? JStage_Title { get; set; }
    public string? JStage_Link { get; set; }
    public string? JStage_Id { get; set; }
    public string? JStage_UpdatedOn { get; set; }




    // ★★★★★ mainly from CiNii








    // ★★★★★★★★★★★★★★★ method

    /// <summary>
    /// Merge two ResearchArticle instances.
    /// </summary>
    /// <param name="addingArticleInfo"></param>
    public void MergeInfo(ResearchArticle addingArticleInfo)
    {
        // nullじゃないほうを採用。
        // 両方nullじゃないなら，後からの情報が優先（最新）

        var props = GetType()
            .GetProperties()
            .Where(p => !p.HasAttribute<CsvIgnoreAttribute>())
            .Where(p => p.CanWrite)
            ;

        // addingのほうを上書きして，元のを書き換える。
        foreach (var prop in props)
        {
            var addingArticleInfoProp = prop.GetValue(addingArticleInfo);
            if (addingArticleInfoProp != null)
                prop.SetValue(this, addingArticleInfoProp);

        }

    }

    /// <summary>
    /// Create ResearchArticle instance manually.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static ResearchArticle CreateManually(

        string? title = null,
        string[]? authors = null,


        FileInfo? pdfFile = null,
        DirectoryInfo? pdfStockDirectory = null
        )
    {
        // AOI自動生成。
        // 参照先リスト指定可能にする。。

        throw new NotImplementedException();

        if (pdfFile != null)
        {
            if (pdfStockDirectory == null)
                throw new InvalidDataException("When try initializing {pdfFile}, {pdfStockDirectory} must not be null");


            // TODO

        }



        return new ResearchArticle()
        {
            Manual_ArticleTitle = title,
            Manual_Authors = authors,

            AOI = Guid.NewGuid().ToString(),

            DataFrom_Manual = true,
        };
    }


    public static string? CleanUp_UnstructuredRefString(string? rawUnstructuredRefString, int checkCount = 4)
    {
        if (rawUnstructuredRefString == null)
            return rawUnstructuredRefString;

        var unstructuredRefString = rawUnstructuredRefString;

        for (int i = 0; i < checkCount; i++)
        {
            if (unstructuredRefString.Trim() == unstructuredRefString[1..].Trim())
                return unstructuredRefString.Trim();
            else
                unstructuredRefString = unstructuredRefString[1..];
        }

        return rawUnstructuredRefString;
    }

    public int CompareTo(object? obj)
    {
        var comparingArticle = (ResearchArticle)obj!;

        var result = 0;
        var power = (int)Math.Pow(2, 10);

        if (comparingArticle.DOI != null)
        {
            var com = DOI!.CompareTo(comparingArticle.DOI);
            if (com == 0) return 0;
            result += power * Math.Sign(com);
        }

        power /= 2;

        if (comparingArticle.DOI != null)
        {
            var com = CrossRef_ArticleTitle!.CompareTo(comparingArticle.CrossRef_ArticleTitle);
            if (com == 0) return 0;
            result += power * Math.Sign(com);
        }

        power /= 2;

        if (comparingArticle.DOI != null)
        {
            var com = JStage_Id!.CompareTo(comparingArticle.JStage_Id);
            if (com == 0) return 0;
            result += power * Math.Sign(com);
        }

        power /= 2;

        if (comparingArticle.DOI != null)
        {
            var com = Manual_ArticleTitle!.CompareTo(comparingArticle.Manual_ArticleTitle);
            if (com == 0) return 0;
            result += power * Math.Sign(com);
        }

        power /= 2;

        if (comparingArticle.DOI != null)
        {
            var com = AOI!.CompareTo(comparingArticle.AOI);
            if (com == 0) return 0;
            result += power * Math.Sign(com);
        }

        return result;
    }


    // ★★★★★★★★★★★★★★★ 

}
