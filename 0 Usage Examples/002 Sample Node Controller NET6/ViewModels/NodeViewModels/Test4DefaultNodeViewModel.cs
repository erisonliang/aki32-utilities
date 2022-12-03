using Aki32Utilities.UsageExamples.SampleNodeController.ViewModels;

using System.Collections.ObjectModel;

namespace Aki32Utilities.ViewModels.NodeViewModels;

public class Test4DefaultNodeViewModel : DefaultNodeViewModel
{
    public string Name { get; set; }
    public string Memo { get; set; }

    public override IEnumerable<NodeConnectorViewModel> Inputs => _Inputs;
    readonly ObservableCollection<NodeInputViewModel> _Inputs = new();

    public override IEnumerable<NodeConnectorViewModel> Outputs => _Outputs;
    readonly ObservableCollection<NodeOutputViewModel> _Outputs = new();

    public Test4DefaultNodeViewModel()
    {
        for (int i = 0; i < 5; ++i)
        {
            var label = $"Input{i}";
            if (i > 2)
            {
                label += " Allow to connect multiple";
            }
            _Inputs.Add(new NodeInputViewModel(label, i > 2));
        }
    }

    public override NodeConnectorViewModel FindConnector(Guid guid)
    {
        return Inputs.FirstOrDefault(arg => arg.Guid == guid);
    }
}
