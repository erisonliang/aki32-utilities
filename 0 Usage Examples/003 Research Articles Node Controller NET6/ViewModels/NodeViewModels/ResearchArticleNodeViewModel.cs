using Aki32Utilities.ConsoleAppUtilities.SpecificPurposeModels.Research;
using Aki32Utilities.UsageExamples.ResearchArticlesNodeController.ViewModels;

using DocumentFormat.OpenXml.Spreadsheet;

using Newtonsoft.Json;

using PropertyChanged;

using System.Collections.ObjectModel;
using System.Text.Json.Nodes;

namespace Aki32Utilities.ViewModels.NodeViewModels;

public class ResearchArticleNodeViewModel : DefaultNodeViewModel
{

    // ★★★★★★★★★★★★★★★ props

    public ResearchArticle Article { get; set; } = new();

    [AlsoNotifyFor(nameof(Name), nameof(ArticleTitle), nameof(Authors), nameof(Description), nameof(Memo), nameof(DOI))]
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

    public string Authors
    {
        get => (Article.Authors == null) ? "不明" : JsonConvert.SerializeObject(Article.Authors);
        set
        {
            var temp = JsonConvert.SerializeObject(Article.Authors);
            RaisePropertyChangedIfSet(ref temp, value);
            Article.Manual_Authors = JsonConvert.DeserializeObject<string[]>(temp);
        }
    }

    public string Description
    {
        get => Article.Description ?? "";
        set
        {
            var temp = Article.Description;
            RaisePropertyChangedIfSet(ref temp, value);
            Article.Manual_Description = temp;
        }
    }

    public string Memo
    {
        get => Article.Memo ?? "";
        set
        {
            var temp = Article.Memo;
            RaisePropertyChangedIfSet(ref temp, value);
            Article.Memo = temp;
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
        {
            return input;
        }

        var output = Outputs.FirstOrDefault(arg => arg.Guid == guid);
        return output;
    }

    // ★★★★★★★★★★★★★★★

}
