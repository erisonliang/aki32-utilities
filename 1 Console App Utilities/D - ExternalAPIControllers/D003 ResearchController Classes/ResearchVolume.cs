

namespace Aki32_Utilities.ExternalAPIControllers;
/// <summary>
/// 
/// </summary>
/// <remarks>
/// ref 
/// https://www.jstage.jst.go.jp/static/files/ja/manual_api.pdf
/// </remarks>
public class ResearchVolume
{

    public string? Title_English { get; set; }
    public string? Title_Japanese { get; set; }

    public string? Link_English { get; set; }
    public string? Link_Japanese { get; set; }

    public string? PrintISSN { get; set; }
    public string? OnlineISSN { get; set; }

    public string? PublisherName_English { get; set; }
    public string? PublisherName_Japanese { get; set; }
    public string? PublisherUri_English { get; set; }
    public string? PublisherUri_Japanese { get; set; }

    public string? JournalCode_JStage { get; set; }

    public string? MaterialTitle_English { get; set; }
    public string? MaterialTitle_Japanese { get; set; }

    public string? Volume { get; set; }
    public string? SubVolume { get; set; }

    public string? Number { get; set; }
    public string? StartingPage { get; set; }
    public string? EndingPage { get; set; }

    public string? PublishedYear { get; set; }

    public string? SystemCode { get; set; }
    public string? SystemName { get; set; }

    public string? Title { get; set; }
    public string? Link { get; set; }
    public string? Id { get; set; }
    public string? UpdatedOn { get; set; }

}
