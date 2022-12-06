using Aki32Utilities.UsageExamples.ResearchArticlesNodeController.ViewModels;

using System.Collections.ObjectModel;

namespace Aki32Utilities.ViewModels.NodeViewModels;

public class Test3DefaultNodeViewModel : DefaultNodeViewModel
{
    public string Name { get; set; }
    public string Memo { get; set; }

    public override IEnumerable<NodeConnectorViewModel> Inputs => _Inputs;
    readonly ObservableCollection<NodeInputViewModel> _Inputs = new();

    public override IEnumerable<NodeConnectorViewModel> Outputs => _Outputs;
    readonly ObservableCollection<NodeOutputViewModel> _Outputs = new();

    public Test3DefaultNodeViewModel()
    {
        for (int i = 0; i < 2; ++i)
        {
            _Outputs.Add(new NodeOutputViewModel($"Output{i}"));
        }
    }

    public override NodeConnectorViewModel FindConnector(Guid guid)
    {
        return Outputs.FirstOrDefault(arg => arg.Guid == guid);
    }

}
