

namespace Aki32Utilities.ConsoleAppUtilities.Research;
public class ResearchArticle_ManualInitInfo
{

    // ★★★★★★★★★★★★★★★ prop

    public string? DOI { get; set; }


    // ★★★★★ original meta info

    /// <summary>
    /// favorite flag
    /// </summary>
    public bool? Private_Favorite { get; set; }


    // ★★★★★ manual info

    public string? Manual_ArticleTitle { get; set; }
    public string[]? Manual_Authors { get; set; }
    public string? Manual_Description { get; set; }
    public string? Manual_PublishedDate { get; set; }

    public string? Memo { get; set; }


    // ★★★★★★★★★★★★★★★ init

    public ResearchArticle_ManualInitInfo()
    {
    }


    // ★★★★★★★★★★★★★★★ 

}
