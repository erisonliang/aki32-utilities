using Aki32Utilities.ConsoleAppUtilities.Research;
using Aki32Utilities.UsageExamples.ResearchArticlesNodeController.ViewModels;

using Newtonsoft.Json;

using PropertyChanged;

using System.Collections.ObjectModel;

namespace Aki32Utilities.ViewModels.NodeViewModels;

public class ResearchArticleNodeViewModel : DefaultNodeViewModel
{

    // ★★★★★★★★★★★★★★★ props

    public ResearchArticle Article { get; set; } = new();

    [AlsoNotifyFor(nameof(Name), nameof(ArticleTitle), nameof(Authors), nameof(TopAuthor), nameof(Memo), nameof(DOI), nameof(PublishedOn),
        nameof(Memo_Motivation), nameof(Memo_Method), nameof(Memo_Insights), nameof(Memo_Contribution))]
    private int NotifyArticleUpdatedBridge { get; set; } = 0;

    public string Name { get; set; }

    public string ArticleTitle
    {
        get => Article.ArticleTitle ?? "不明";
        set
        {
            var temp = Article.ArticleTitle;
            RaisePropertyChangedIfSet(ref temp, value);
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
            RaisePropertyChangedIfSet(ref temp, value);
            if (temp is not null)
                Article.Manual_Authors = temp.Split(';', StringSplitOptions.TrimEntries & StringSplitOptions.RemoveEmptyEntries);
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
            RaisePropertyChangedIfSet(ref temp, value);
            Article.Manual_PublishedDate = temp;
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
            RaisePropertyChangedIfSet(ref temp, value);
            Article.Memo = temp;
        }
    }

    public string Memo_Motivation
    {
        get => Article.Memo_Motivation;
        set
        {
            var temp = Article.Memo_Motivation;
            RaisePropertyChangedIfSet(ref temp, value);
            Article.Memo_Motivation = temp;
        }
    }

    public string Memo_Method
    {
        get => Article.Memo_Method;
        set
        {
            var temp = Article.Memo_Method;
            RaisePropertyChangedIfSet(ref temp, value);
            Article.Memo_Method = temp;
        }
    }

    public string Memo_Insights
    {
        get => Article.Memo_Insights;
        set
        {
            var temp = Article.Memo_Insights;
            RaisePropertyChangedIfSet(ref temp, value);
            Article.Memo_Insights = temp;
        }
    }

    public string Memo_Contribution
    {
        get => Article.Memo_Contribution;
        set
        {
            var temp = Article.Memo_Contribution;
            RaisePropertyChangedIfSet(ref temp, value);
            Article.Memo_Contribution = temp;
        }
    }

    public string DOI
    {
        get => Article.DOI ?? "";
        set
        {
            var temp = Article.DOI;
            RaisePropertyChangedIfSet(ref temp, value);
            Article.DOI = temp;
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
