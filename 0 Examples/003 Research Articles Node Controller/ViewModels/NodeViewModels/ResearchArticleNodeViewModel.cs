using Aki32Utilities.ConsoleAppUtilities.Research;
using Aki32Utilities.UsageExamples.ResearchArticlesNodeController.ViewModels;

using DocumentFormat.OpenXml.Drawing;

using Newtonsoft.Json;

using PropertyChanged;

using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Aki32Utilities.ViewModels.NodeViewModels;

public class ResearchArticleNodeViewModel : DefaultNodeViewModel
{

    // ★★★★★★★★★★★★★★★ props

    public ResearchArticle Article { get; set; } = new();

    [AlsoNotifyFor(nameof(Name), nameof(ArticleTitle), nameof(Authors), nameof(TopAuthor), nameof(Memo), nameof(DOI), nameof(PublishedOn),
        nameof(Memo_Motivation), nameof(Memo_Method), nameof(Memo_Insights), nameof(Memo_Contribution),
        nameof(IsFavorite), nameof(IsRead), nameof(IsCategory1), nameof(IsCategory2), nameof(IsCategory3),
        nameof(ArticleHeaderColor)
        )]
    private int NotifyArticleUpdatedBridge { get; set; } = 0;

    public string Name { get; set; }

    public string ArticleTitle
    {
        get => Article.ArticleTitle ?? "不明";
        set
        {
            var temp = Article.ArticleTitle;
            if (RaisePropertyChangedIfSet(ref temp, value))
                Article.Manual_ArticleTitle = temp;
        }
    }

    [AlsoNotifyFor(nameof(TopAuthor))]
    public string Authors
    {
        get => (Article.Authors == null) ? "不明" : string.Join("; ", Article.Authors);
        set
        {
            var temp = JsonConvert.SerializeObject(Article.Authors);
            if (RaisePropertyChangedIfSet(ref temp, value) && temp is not null)
                Article.Manual_Authors = temp.Split(new char[] { ';', '；' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
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

    public string PublishedOn
    {
        get => Article.PublishedOn ?? "不明";
        set
        {
            var temp = Article.PublishedOn;
            if (RaisePropertyChangedIfSet(ref temp, value))
                Article.Manual_PublishedDate = temp!.Replace("/", "-");
        }
    }

    public string Memo
    {
        get
        {
            if (Article.Memo is not null)
                return Article.Memo;

            if (!string.IsNullOrEmpty(Article.Description))
                return $"【概要】{Article.Description}";

            return "";
        }
        set
        {
            var temp = Article.Memo;
            if (RaisePropertyChangedIfSet(ref temp, value))
                Article.Memo = temp;
        }
    }
    public string Memo_Motivation
    {
        get => Article.Memo_Motivation;
        set
        {
            var temp = Article.Memo_Motivation;
            if (RaisePropertyChangedIfSet(ref temp, value))
                Article.Memo_Motivation = temp;
        }
    }
    public string Memo_Method
    {
        get => Article.Memo_Method;
        set
        {
            var temp = Article.Memo_Method;
            if (RaisePropertyChangedIfSet(ref temp, value))
                Article.Memo_Method = temp;
        }
    }
    public string Memo_Insights
    {
        get => Article.Memo_Insights;
        set
        {
            var temp = Article.Memo_Insights;
            if (RaisePropertyChangedIfSet(ref temp, value))
                Article.Memo_Insights = temp;
        }
    }
    public string Memo_Contribution
    {
        get => Article.Memo_Contribution;
        set
        {
            var temp = Article.Memo_Contribution;
            if (RaisePropertyChangedIfSet(ref temp, value))
                Article.Memo_Contribution = temp;
        }
    }

    public string DOI
    {
        get => Article.DOI ?? "";
        set
        {
            var temp = Article.DOI;
            if (RaisePropertyChangedIfSet(ref temp, value))
                Article.DOI = temp;
        }
    }

    public string Tags
    {
        get
        {
            var ss = new List<string>();

            if (Article.TryFindPDF(MainWindowViewModel.ResearchArticlesManager.PDFsDirectory))
                ss.Add("PDF");

            if (Article.DataFrom_JStage ?? false)
                ss.Add("JStage");

            if (Article.DataFrom_CiNii ?? false)
                ss.Add("CiNii");

            if (Article.DataFrom_CrossRef ?? false)
                ss.Add("CrossRef");

            if (Article.DataFrom_NDLSearch ?? false)
                ss.Add("NDLSearch");

            return string.Join(", ", ss.Select(s => $"[{s}]"));
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
                Article.Private_Favorite = temp;
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
                Article.Private_Read = temp;
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
                Article.Private_Temporary = temp;
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
                Article.Private_IsCategory1 = temp;
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
                Article.Private_IsCategory2 = temp;
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
                Article.Private_IsCategory3 = temp;
        }
    }
    public Brush ArticleHeaderColor
    {
        get
        {
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
                        return Brushes.LightGreen;
                    break;
                case EmphasizePropertyItems.検索結果:
                    if (IsLocalSearchMatched)
                        return Brushes.Aqua;
                    break;
                case EmphasizePropertyItems.一時ﾃﾞｰﾀ:
                    if (IsTemp)
                        return Brushes.DarkOrange;
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

    public override IEnumerable<NodeConnectorViewModel> Inputs => _Inputs;
    readonly ObservableCollection<NodeInputViewModel> _Inputs = new();

    public override IEnumerable<NodeConnectorViewModel> Outputs => _Outputs;
    readonly ObservableCollection<NodeOutputViewModel> _Outputs = new();


    // ★★★★★★★★★★★★★★★ init

    public ResearchArticleNodeViewModel()
    {
        _Inputs.Add(new NodeInputViewModel("引用", true));
        _Outputs.Add(new NodeOutputViewModel($"被引用"));
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
