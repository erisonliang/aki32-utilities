using Livet;

namespace Aki32Utilities.UsageExamples.SampleNodeController.ViewModels;

public interface NodeConnectorViewModel
{
    Guid Guid { get; set; }
    string Label { get; set; }
    bool IsEnable { get; set; }
}

public class NodeInputViewModel : ViewModel, NodeConnectorViewModel
{
    public Guid Guid { get; set; } = Guid.NewGuid();
    public string Label { get; set; } = string.Empty;
    public bool IsEnable { get; set; } = true;
    public bool AllowToConnectMultiple { get; set; } = false;

    public NodeInputViewModel(string label, bool allowToConnectMultiple)
    {
        Label = label;
        AllowToConnectMultiple = allowToConnectMultiple;
    }
}

public class NodeOutputViewModel : ViewModel, NodeConnectorViewModel
{
    public Guid Guid { get; set; } = Guid.NewGuid();
    public string Label { get; set; } = string.Empty;
    public bool IsEnable { get; set; } = true;

    public NodeOutputViewModel(string label)
    {
        Label = label;
    }
}
