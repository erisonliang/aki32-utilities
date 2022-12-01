

namespace Aki32_Utilities.SpecificPurposeModels.Research;
public class ResearchArticle
{

    // ★★★★★★★★★★★★★★★ prop

    // ★ private info

    public bool? Private_Favorite { get; set; }


    // ★ meta info

    public bool? DataFrom_Manual { get; set; }
    public bool? DataFrom_JStage { get; set; }
    public bool? DataFrom_CiNii { get; set; }
    public bool? DataFrom_CrossRef { get; set; }


    // ★ Links
    public string? DOI_Link => (DOI == null) ? null : $"https://doi.org/{DOI}";
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
                return Link?.Replace($"_article/-char/ja/", $"_pdf");
            }

            return null;
        }
    }


    // ★ mainly from CrossRef (Think this as main common info)








    // ★ mainly from J-Stage

    public string? Title_English { get; set; }
    public string? Title_Japanese { get; set; }

    public string? Link_English { get; set; }
    public string? Link_Japanese { get; set; }

    public string[]? Authors_English { get; set; }
    public string[]? Authors_Japanese { get; set; }

    public string? JournalCode_JStage { get; set; }

    public string? MaterialTitle_English { get; set; }
    public string? MaterialTitle_Japanese { get; set; }

    public string? PrintISSN { get; set; }
    public string? OnlineISSN { get; set; }

    public string? Volume { get; set; }
    public string? SubVolume { get; set; }

    public string? Number { get; set; }
    public string? StartingPage { get; set; }
    public string? EndingPage { get; set; }

    public string? PublishedYear { get; set; }

    public string? JOI { get; set; }
    public string? DOI { get; set; }

    public string? SystemCode { get; set; }
    public string? SystemName { get; set; }

    public string? Title { get; set; }
    public string? Link { get; set; }
    public string? Id { get; set; }
    public string? UpdatedOn { get; set; }


    // ★ mainly from CiNii




    // ★★★★★★★★★★★★★★★ method

    public void MergeInfo(ResearchArticle addingArticleInfo)
    {
        // TODO define!!!
        throw new NotImplementedException();

        // nullじゃないほうを採用。
        // 両方nullじゃないなら，後からの情報が優先（最新）





    }


    // ★★★★★★★★★★★★★★★ 

}
