using Aki32Utilities.ConsoleAppUtilities.Research;
using Aki32Utilities.UsageExamples.SampleNodeController.ViewModels;
using System.Collections.ObjectModel;

namespace Aki32Utilities.ViewModels.NodeViewModels;

public class ResearchArticleNodeViewModel : DefaultNodeViewModel
{
    public ResearchArticle Article { get; set; }

    public string Name { get; set; }

    public string Memo
    {
        get => _Memo;
        set => RaisePropertyChangedIfSet(ref _Memo, value);
    }
    string _Memo = string.Empty;

    public string Title
    {
        get => _Title;
        set => RaisePropertyChangedIfSet(ref _Title, value);
    }
    string _Title = string.Empty;

    public string Author
    {
        get => _Author;
        set => RaisePropertyChangedIfSet(ref _Author, value);
    }
    string _Author = string.Empty;

    public string DOI
    {
        get => _DOI;
        set => RaisePropertyChangedIfSet(ref _DOI, value);
    }
    string _DOI = string.Empty;


    public override IEnumerable<NodeConnectorViewModel> Inputs => _Inputs;
    readonly ObservableCollection<NodeInputViewModel> _Inputs = new();

    public override IEnumerable<NodeConnectorViewModel> Outputs => _Outputs;
    readonly ObservableCollection<NodeOutputViewModel> _Outputs = new();

    public ResearchArticleNodeViewModel()
    {
        _Inputs.Add(new NodeInputViewModel("", true)); // 引用
        _Outputs.Add(new NodeOutputViewModel(""));// 被引用
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
}
