

namespace Aki32_Utilities.ExternalAPIControllers;
public class ResearchArticle
{

    // ★★★★★★★★★★★★★★★ prop

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
    public string? DOI_Link => DOI == null ? null : $"https://doi.org/{DOI}";

    public string? SystemCode { get; set; }
    public string? SystemName { get; set; }

    public string? Title { get; set; }
    public string? Link { get; set; }
    public string? Id { get; set; }
    public string? UpdatedOn { get; set; }

    public bool RefInfo_JStage { get; set; } = false;
    public bool RefInfo_CiNii { get; set; } = false;
    public bool RefInfo_CrossRef { get; set; } = false;


    // ★★★★★★★★★★★★★★★ method

    public void ConvoluteInfo(ResearchArticle addingArticleInfo)
    {




    }


    // ★★★★★★★★★★★★★★★ 

}
