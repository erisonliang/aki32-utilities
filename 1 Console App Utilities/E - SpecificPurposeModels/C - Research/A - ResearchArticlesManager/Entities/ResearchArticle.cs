

using Aki32_Utilities.General;

using Newtonsoft.Json;

namespace Aki32_Utilities.SpecificPurposeModels.Research;
public class ResearchArticle
{

    // ★★★★★★★★★★★★★★★ prop

    // ★ Links
    public string? DOI_Link => (DOI == null) ? null : $"https://dx.doi.org/{DOI}";
    public string? CrossRef_Link => (DOI == null) ? null : $"https://api.crossref.org/v1/works/{DOI}";
    public string? PDF_Link
    {
        get
        {
            if (DOI == null)
                return null;

            if (DOI.Contains("aijs"))
            {
                // get data from aij
                return Link_JS?.Replace($"_article/-char/ja/", $"_pdf");
            }

            return null;
        }
    }


    // ★ original meta info (1/2 of main common info)

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
    /// </summary>
    public string? AOI { get; set; }


    // ★ mainly from CrossRef (2/2 of main common info)

    public string? DOI { get; set; }
    public string[]? ReferenceDOIs { get; set; }

    public string? UnstructuredRefString { get; set; }

    public string? PrintISSN { get; set; }
    public string? OnlineISSN { get; set; }

    public string? ArticleTitle_CR { get; set; }
    public string[]? Authors_CR { get; set; }

    public string? PublishedDate_CR { get; set; }


    // ★ mainly from J-Stage

    public string? ArticleTitle_English_JS { get; set; }
    public string? ArticleTitle_Japanese_JS { get; set; }

    public string? Link_English_JS { get; set; }
    public string? Link_Japanese_JS { get; set; }

    public string[]? Authors_English_JS { get; set; }
    public string[]? Authors_Japanese_JS { get; set; }

    public string? JournalCode_JS { get; set; }

    public string? MaterialTitle_English_JS { get; set; }
    public string? MaterialTitle_Japanese_JS { get; set; }


    public string? Volume_JS { get; set; }
    public string? SubVolume_JS { get; set; }

    public string? Number_JS { get; set; }
    public string? StartingPage_JS { get; set; }
    public string? EndingPage_JS { get; set; }

    public string? PublishedYear_JS { get; set; }

    public string? JOI_JS { get; set; }

    public string? SystemCode_JS { get; set; }
    public string? SystemName_JS { get; set; }

    public string? Title_JS { get; set; }
    public string? Link_JS { get; set; }
    public string? Id_JS { get; set; }
    public string? UpdatedOn_JS { get; set; }


    // ★ mainly from CiNii






    // ★★★★★★★★★★★★★★★ method

    public void MergeInfo(ResearchArticle addingArticleInfo)
    {
        // TODO define!!!
        throw new NotImplementedException();

        // nullじゃないほうを採用。
        // 両方nullじゃないなら，後からの情報が優先（最新）





    }

    public static string? CleanUp_UnstructuredRefString(string? rawUnstructuredRefString, int checkCount = 4)
    {
        if (rawUnstructuredRefString == null)
            return rawUnstructuredRefString;

        var unstructuredRefString = rawUnstructuredRefString;

        for (int i = 0; i < checkCount; i++)
        {
            if (unstructuredRefString.Trim() == unstructuredRefString[1..].Trim())
                return unstructuredRefString;
            else
                unstructuredRefString = unstructuredRefString[1..];
        }

        return rawUnstructuredRefString;
    }


    // ★★★★★★★★★★★★★★★ 

}
