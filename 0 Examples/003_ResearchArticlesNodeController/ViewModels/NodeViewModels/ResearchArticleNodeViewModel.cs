using Aki32Utilities.ConsoleAppUtilities.Research;
using Aki32Utilities.UsageExamples.ResearchArticlesNodeController.ViewModels;

using Newtonsoft.Json;

using PropertyChanged;

using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Aki32Utilities.ViewModels.NodeViewModels;

public class ResearchArticleNodeViewModel : DefaultNodeViewModel
{

    // ★★★★★★★★★★★★★★★ props

    public ResearchArticle Article { get; set; } = new();

    [AlsoNotifyFor(nameof(NodeName), nameof(Article),
        nameof(ArticleHeaderColor), nameof(IsFavorite), nameof(IsRead), nameof(WillRead), nameof(IsGrouping), nameof(IsTemp), nameof(IsLocalSearchMatched), nameof(IsCategory1), nameof(IsCategory2), nameof(IsCategory3),
        nameof(Authors), nameof(ArticleTitle), nameof(TopAuthor), nameof(PublishedOn), nameof(DataFrom)
        )]
    private int NotifyArticleUpdatedBridge { get; set; } = 0;


    // ★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★

    public string NodeName { get; set; }

    public bool IsNodeBusy { get; set; } = false;


    // ★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★

    public bool RecommendPullReferenceInfo
    {
        get
        {
            if (string.IsNullOrEmpty(Article.DOI))
                return false;
            if ((Article.DataFrom_CrossRef_DOI ?? false) || (Article.DataFrom_JStage_DOI ?? false))
                return false;
            return true;
        }
    }
    public bool RecommendPullNormalMetaInfo
    {
        get
        {
            // 自動生成されたものでない非構造データがあれば，まだ情報追加の余地があるからオススメする。
            _ = Article.ReferenceString;
            if (Article.IsLastReferenceStringFromFormatGeneration)
                return false;

            // もし情報提供元の候補が無ければ，諦める。
            if (Article.GetAvailableDataSources().Count == 0)
                return false;

            return true;
        }
    }
    public bool RecommendPullAIMetaInfo
    {
        get
        {
            // 一回でもAI推定した過去があればもう希望無しとする。
            if (Article.DataFrom_AI_PredictFromRefString ?? false)
                return false;


            // 自動生成されたものでない非構造データがあれば，まだ情報追加の余地があるからオススメする。
            if (string.IsNullOrEmpty(Article.ReferenceString_FromUnstructured))
                return false;
            _ = Article.ReferenceString;
            if (Article.IsLastReferenceStringFromFormatGeneration)
                return false;

            return true;
        }
    }
    public bool RecommendOpenPDF
    {
        get
        {
            // PDFあるなら開けるからOK
            if (Article.TryFindPDF(MainWindowViewModel.ResearchArticlesManager.PDFsDirectory))
                return true;
            // ないなら，ダウンロード出来ないと話にならないからオススメできない。
            if (string.IsNullOrEmpty(Article.PDF_Link))
                return false;
            return true;
        }
    }
    public bool RecommendOpenDOIWebSite
    {
        get
        {
            if (string.IsNullOrEmpty(Article.DOI_Link))
                return false;
            return true;
        }
    }
    public bool RecommendExecuteAll
    {
        get
        {
            // TODO
            return false;
        }
    }
    public bool RecommendAISummary
    {
        get
        {
            // TODO
            return false;
        }
    }
    public bool RecommendManuallyAddPDF
    {
        get
        {
            if (Article.TryFindPDF(MainWindowViewModel.ResearchArticlesManager.PDFsDirectory))
                return false;
            return true;
        }
    }
    public bool RecommendKeep
    {
        get
        {
            return false;
        }
    }


    // ★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★

    public string ArticleTitle
    {
        get => Article.ArticleTitle ?? "";
        set
        {
            var temp = Article.ArticleTitle;
            if (RaisePropertyChangedIfSet(ref temp, value))
                Article.Manual_ArticleTitle = value;
        }
    }

    [AlsoNotifyFor(nameof(TopAuthor), nameof(Article))]
    public string Authors
    {
        get => (Article.Authors == null) ? "" : string.Join("; ", Article.Authors);
        set
        {
            var temp = JsonConvert.SerializeObject(Article.Authors);
            if (RaisePropertyChangedIfSet(ref temp, value) && value is not null)
                Article.Manual_Authors = value.Split(new char[] { ';', '；' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        }
    }
    public string TopAuthor
    {
        get
        {
            if (Article.Authors is null || Article.Authors.Length == 0)
                return "不明";

            if (Article.Authors.Length == 1)
                return $"{Article.Authors[0]}";

            return $"{Article.Authors[0]} ら";
        }
    }

    public string DataFrom
    {
        get
        {
            var ss = new List<string>();

            if (Article.DataFrom_Manual ?? false)
                ss.Add("Manual");

            if ((Article.DataFrom_JStage_Main ?? false) || (Article.DataFrom_JStage_DOI ?? false))
                ss.Add("JStage");
            else if (Article.DataFrom_JStage_SimpleRef ?? false)
                ss.Add("JStage (Simple)");

            if (Article.DataFrom_CrossRef_DOI ?? false)
                ss.Add("CrossRef");
            else if (Article.DataFrom_CrossRef_SimpleRef ?? false)
                ss.Add("CrossRef (Simple)");

            if (Article.DataFrom_CiNii_Main ?? false)
                ss.Add("CiNii");

            if (Article.DataFrom_NDLSearch_Main ?? false)
                ss.Add("NDLSearch");

            if (Article.DataFrom_AI_PredictFromRefString ?? false)
                ss.Add("AI Prediction");

            return string.Join(", ", ss.Select(s => $"[{s}]"));
        }
    }

    public Brush ArticleHeaderColor
    {
        get
        {
            if (IsGrouping)
                return Brushes.Yellow;

            switch (MainWindowViewModel._SelectedEmphasizePropertyItem)
            {
                case EmphasizePropertyItems.なし:
                    break;
                case EmphasizePropertyItems.お気に入り:
                    if (IsFavorite)
                        return new SolidColorBrush(Color.FromArgb(0xFF, 0xE9, 0x5B, 0x6B));
                    break;
                case EmphasizePropertyItems.既読:
                    if (IsRead)
                        return Brushes.YellowGreen;
                    break;
                case EmphasizePropertyItems.積読:
                    if (WillRead)
                        return Brushes.LightGreen;
                    break;
                case EmphasizePropertyItems.一時ﾃﾞｰﾀ:
                    if (IsTemp)
                        return Brushes.DarkOrange;
                    break;
                case EmphasizePropertyItems.ｸﾞﾙｰﾌﾟ:
                    if (IsGrouping)
                        return Brushes.Yellow;
                    break;
                case EmphasizePropertyItems.検索結果:
                    if (IsLocalSearchMatched)
                        return Brushes.Aqua;
                    break;
                case EmphasizePropertyItems.ﾒﾓ1:
                    if (IsCategory1)
                        return Brushes.White;
                    break;
                case EmphasizePropertyItems.ﾒﾓ2:
                    if (IsCategory2)
                        return Brushes.Pink;
                    break;
                case EmphasizePropertyItems.ﾒﾓ3:
                    if (IsCategory3)
                        return Brushes.Purple;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return new SolidColorBrush(Color.FromArgb(0xFF, 0x66, 0x66, 0x66));
        }
    }
    [AlsoNotifyFor(nameof(ArticleHeaderColor))]
    public bool IsFavorite
    {
        get => Article.Private_Favorite ?? false;
        set
        {
            var temp = Article.Private_Favorite;
            if (RaisePropertyChangedIfSet(ref temp, value))
                Article.Private_Favorite = value;
        }
    }
    [AlsoNotifyFor(nameof(ArticleHeaderColor))]
    public bool IsRead
    {
        get => Article.Private_Read ?? false;
        set
        {
            var temp = Article.Private_Read;
            if (RaisePropertyChangedIfSet(ref temp, value))
                Article.Private_Read = value;
        }
    }
    [AlsoNotifyFor(nameof(ArticleHeaderColor))]
    public bool WillRead
    {
        get => Article.Private_WillRead ?? false;
        set
        {
            var temp = Article.Private_WillRead;
            if (RaisePropertyChangedIfSet(ref temp, value))
                Article.Private_WillRead = value;
        }
    }
    [AlsoNotifyFor(nameof(ArticleHeaderColor))]
    public bool IsGrouping
    {
        get => Article.Private_IsGrouping ?? false;
        set
        {
            var temp = Article.Private_IsGrouping;
            if (RaisePropertyChangedIfSet(ref temp, value))
                Article.Private_IsGrouping = value;
        }
    }
    [AlsoNotifyFor(nameof(ArticleHeaderColor))]
    public bool IsTemp
    {
        get => Article.Private_Temporary ?? false;
        set
        {
            var temp = Article.Private_Temporary;
            if (RaisePropertyChangedIfSet(ref temp, value))
                Article.Private_Temporary = value;
        }
    }
    [AlsoNotifyFor(nameof(ArticleHeaderColor))]
    public bool IsLocalSearchMatched
    {
        get
        {
            var searchFullString = MainWindowViewModel._LocalSearchString;
            if (searchFullString is null)
                return false;

            var searchStrings = searchFullString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (searchStrings.Length == 0)
                return false;

            return Article.GetIfSearchStringsMatched(searchStrings);
        }
    }
    [AlsoNotifyFor(nameof(ArticleHeaderColor))]
    public bool IsCategory1
    {
        get => Article.Private_IsCategory1 ?? false;
        set
        {
            var temp = Article.Private_IsCategory1;
            if (RaisePropertyChangedIfSet(ref temp, value))
                Article.Private_IsCategory1 = value;
        }
    }
    [AlsoNotifyFor(nameof(ArticleHeaderColor))]
    public bool IsCategory2
    {
        get => Article.Private_IsCategory2 ?? false;
        set
        {
            var temp = Article.Private_IsCategory2;
            if (RaisePropertyChangedIfSet(ref temp, value))
                Article.Private_IsCategory2 = value;
        }
    }
    [AlsoNotifyFor(nameof(ArticleHeaderColor))]
    public bool IsCategory3
    {
        get => Article.Private_IsCategory3 ?? false;
        set
        {
            var temp = Article.Private_IsCategory3;
            if (RaisePropertyChangedIfSet(ref temp, value))
                Article.Private_IsCategory3 = value;
        }
    }

    public string PublishedOn
    {
        get => Article.PublishedOn ?? "";
        set
        {
            var temp = Article.PublishedOn;
            if (RaisePropertyChangedIfSet(ref temp, value))
                Article.Manual_PublishedDate = value!.Replace("/", "-").Replace(".", "-");
        }
    }

    public string MaterialTitle
    {
        get => Article.MaterialTitle ?? "";
        set
        {
            var temp = Article.MaterialTitle;
            if (RaisePropertyChangedIfSet(ref temp, value))
                Article.Manual_MaterialTitle = value;
        }
    }
    [AlsoNotifyFor(nameof(Article))]
    public string MaterialVolume
    {
        get => Article.MaterialVolume ?? "";
        set
        {
            var temp = Article.MaterialVolume;
            if (RaisePropertyChangedIfSet(ref temp, value))
                Article.Manual_MaterialVolume = value;
        }
    }
    [AlsoNotifyFor(nameof(Article))]
    public string MaterialSubVolume
    {
        get => Article.MaterialSubVolume ?? "";
        set
        {
            var temp = Article.MaterialSubVolume;
            if (RaisePropertyChangedIfSet(ref temp, value))
                Article.Manual_MaterialSubVolume = value;
        }
    }
    public string StartingPage
    {
        get => Article.StartingPage ?? "";
        set
        {
            var temp = Article.StartingPage;
            if (RaisePropertyChangedIfSet(ref temp, value))
                Article.Manual_StartingPage = value;
        }
    }
    public string EndingPage
    {
        get => Article.EndingPage ?? "";
        set
        {
            var temp = Article.EndingPage;
            if (RaisePropertyChangedIfSet(ref temp, value))
                Article.Manual_EndingPage = value;
        }
    }

    public override IEnumerable<NodeConnectorViewModel> Inputs => _Inputs;
    readonly ObservableCollection<NodeInputViewModel> _Inputs = new();

    public override IEnumerable<NodeConnectorViewModel> Outputs => _Outputs;
    readonly ObservableCollection<NodeOutputViewModel> _Outputs = new();


    // ★★★★★★★★★★★★★★★ init

    public ResearchArticleNodeViewModel()
    {
        _Inputs.Add(new NodeInputViewModel("", true, "この文献が引用している文献 ← ")); // 引用
        _Outputs.Add(new NodeOutputViewModel("", " → この文献を引用している文献"));// 被引用
    }


    // ★★★★★★★★★★★★★★★ methods

    public void NotifyArticleUpdated()
    {
        NotifyArticleUpdatedBridge++;
    }

    public override NodeConnectorViewModel FindConnector(Guid guid)
    {
        var input = Inputs.FirstOrDefault(arg => arg.Guid == guid);
        if (input != null)
            return input;

        var output = Outputs.FirstOrDefault(arg => arg.Guid == guid);
        return output;
    }

    // ★★★★★★★★★★★★★★★

}
