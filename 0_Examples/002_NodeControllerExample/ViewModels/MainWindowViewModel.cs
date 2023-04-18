using Livet;
using Livet.Commands;
using Aki32Utilities.WPFAppUtilities.NodeController.Operation;
using NodeGraph.Utilities;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using Aki32Utilities.ViewModels.NodeViewModels;

namespace Aki32Utilities.UsageExamples.SampleNodeController.ViewModels;

public class MainWindowViewModel : ViewModel
{

    // ★★★★★★★★★★★★★★★ props

    #region props

    public double Scale { get; set; } = 1d;

    public ViewModelCommand AddNodeCommand => _AddNodeCommand.Get(AddNode);
    ViewModelCommandHandler _AddNodeCommand = new();

    public ViewModelCommand AddGroupNodeCommand => _AddGroupNodeCommand.Get(AddGroupNode);
    ViewModelCommandHandler _AddGroupNodeCommand = new();

    public ViewModelCommand RemoveNodesCommand => _RemoveNodesCommand.Get(RemoveNodes);
    ViewModelCommandHandler _RemoveNodesCommand = new();

    public ListenerCommand<PreviewConnectLinkOperationEventArgs> PreviewConnectLinkCommand => _PreviewConnectLinkCommand.Get(PreviewConnect);
    ViewModelCommandHandler<PreviewConnectLinkOperationEventArgs> _PreviewConnectLinkCommand = new();

    public ListenerCommand<ConnectedLinkOperationEventArgs> ConnectedLinkCommand => _ConnectedLinkCommand.Get(Connected);
    ViewModelCommandHandler<ConnectedLinkOperationEventArgs> _ConnectedLinkCommand = new();

    public ListenerCommand<DisconnectedLinkOperationEventArgs> DisconnectedLinkCommand => _DisconnectedLinkCommand.Get(Disconnected);
    ViewModelCommandHandler<DisconnectedLinkOperationEventArgs> _DisconnectedLinkCommand = new();

    public ListenerCommand<EndMoveNodesOperationEventArgs> EndMoveNodesCommand => _EndMoveNodesCommand.Get(NodesMoved);
    ViewModelCommandHandler<EndMoveNodesOperationEventArgs> _EndMoveNodesCommand = new();

    public ListenerCommand<IList> SelectionChangedCommand => _SelectionChangedCommand.Get(SelectionChanged);
    ViewModelCommandHandler<IList> _SelectionChangedCommand = new();

    public ViewModelCommand AddTestNodeLinkCommand => _AddTestNodeLinkCommand.Get(AddTestNodeLink);
    ViewModelCommandHandler _AddTestNodeLinkCommand = new();

    public ViewModelCommand MoveTestNodesCommand => _MoveTestNodesCommand.Get(MoveTestNodes);
    ViewModelCommandHandler _MoveTestNodesCommand = new();

    public ViewModelCommand ClearNodesCommand => _ClearNodesCommand.Get(ClearNodes);
    ViewModelCommandHandler _ClearNodesCommand = new();

    public ViewModelCommand ClearNodeLinksCommand => _ClearNodeLinksCommand.Get(ClearNodeLinks);
    ViewModelCommandHandler _ClearNodeLinksCommand = new();

    public ViewModelCommand MoveGroupNodeCommand => _MoveGroupNodeCommand.Get(MoveGroupNode);
    ViewModelCommandHandler _MoveGroupNodeCommand = new();

    public ViewModelCommand ChangeGroupInnerSizeCommand => _ChangeGroupInnerSizeCommand.Get(ChangeGroupInnerSize);
    ViewModelCommandHandler _ChangeGroupInnerSizeCommand = new();

    public ViewModelCommand ChangeGroupInnerPositionCommand => _ChangeGroupInnerPositionCommand.Get(ChangeGroupInnerPosition);
    ViewModelCommandHandler _ChangeGroupInnerPositionCommand = new();

    public ViewModelCommand ResetScaleCommand => _ResetScaleCommand.Get(ResetScale);
    ViewModelCommandHandler _ResetScaleCommand = new();



    public IEnumerable<DefaultNodeViewModel> NodeViewModels => _NodeViewModels;
    ObservableCollection<DefaultNodeViewModel> _NodeViewModels = new();

    public IEnumerable<NodeLinkViewModel> NodeLinkViewModels => _NodeLinkViewModels;
    ObservableCollection<NodeLinkViewModel> _NodeLinkViewModels = new();

    public IEnumerable<GroupNodeViewModel> GroupNodeViewModels => _GroupNodeViewModels;
    ObservableCollection<GroupNodeViewModel> _GroupNodeViewModels = new();

    public GroupIntersectType[] GroupIntersectTypes { get; } = Enum.GetValues(typeof(GroupIntersectType)).OfType<GroupIntersectType>().ToArray();
    public RangeSelectionMode[] RangeSelectionModes { get; } = Enum.GetValues(typeof(RangeSelectionMode)).OfType<RangeSelectionMode>().ToArray();

    public GroupIntersectType SelectedGroupIntersectType { get; set; }

    public RangeSelectionMode SelectedRangeSelectionMode { get; set; } = RangeSelectionMode.ContainVMDefine;

    public bool IsLockedAllNodeLinks
    {
        get => _IsLockedAllNodeLinks;
        set => UpdateIsLockedAllNodeLinksProperty(value);
    }
    bool _IsLockedAllNodeLinks = false;

    public bool IsEnableAllNodeConnectors
    {
        get => _IsEnableAllNodeConnectors;
        set => UpdateIsEnableAllNodeConnectorsProperty(value);
    }
    bool _IsEnableAllNodeConnectors = true;

    #endregion


    // ★★★★★★★★★★★★★★★ inits

    public MainWindowViewModel()
    {
        _GroupNodeViewModels.Add(new GroupNodeViewModel() { Name = "Group1" });
        _NodeViewModels.Add(new Test1DefaultNodeViewModel() { Name = "Node1", Memo = "Content1", Position = new Point(0, 100) });
        _NodeViewModels.Add(new Test1DefaultNodeViewModel() { Name = "Node2", Memo = "Content2", Position = new Point(100, 200) });
        _NodeViewModels.Add(new Test1DefaultNodeViewModel() { Name = "Node3", Memo = "Content3", Position = new Point(200, 300) });
        _NodeViewModels.Add(new Test3DefaultNodeViewModel() { Name = "Node4", Memo = "OutputsOnlyNode", Position = new Point(500, 100) });
        _NodeViewModels.Add(new Test4DefaultNodeViewModel() { Name = "Node5", Memo = "InputsOnlyNode", Position = new Point(600, 200) });
        _NodeViewModels.Add(new ResearchArticleNodeViewModel() { Name = "ResearchArticle", Memo = "ここに題名", Position = new Point(1100, 100) });

    }


    // ★★★★★★★★★★★★★★★ methods

    #region methods

    void AddNode()
    {
        _NodeViewModels.Add(new ResearchArticleNodeViewModel() { Name = "ResearchArticle", Memo = "ここに題名" });
    }

    void AddGroupNode()
    {
        _GroupNodeViewModels.Add(new GroupNodeViewModel() { Name = "Group" });
    }

    void RemoveNodes()
    {
        var removeNodes = _NodeViewModels.Where(arg => arg.IsSelected).ToArray();
        foreach (var removeNode in removeNodes)
        {
            _NodeViewModels.Remove(removeNode);

            var removeNodeLink = NodeLinkViewModels.FirstOrDefault(arg => arg.InputConnectorNodeGuid == removeNode.Guid || arg.OutputConnectorNodeGuid == removeNode.Guid);
            _NodeLinkViewModels.Remove(removeNodeLink);
        }
    }

    void ClearNodes()
    {
        _NodeLinkViewModels.Clear();
        _NodeViewModels.Clear();
    }

    void ClearNodeLinks()
    {
        _NodeLinkViewModels.Clear();
    }

    void MoveGroupNode()
    {
        _GroupNodeViewModels[0].InterlockPosition = new Point(0, 0);
    }

    void ChangeGroupInnerSize()
    {
        _GroupNodeViewModels[0].InnerWidth = 300;
        _GroupNodeViewModels[0].InnerHeight = 300;
    }

    void ChangeGroupInnerPosition()
    {
        _GroupNodeViewModels[0].InnerPosition = new Point(0, 0);
    }

    void ResetScale()
    {
        Scale = 1.0f;
    }

    void UpdateIsLockedAllNodeLinksProperty(bool value)
    {
        _IsLockedAllNodeLinks = !_IsLockedAllNodeLinks;

        foreach (var nodeLink in _NodeLinkViewModels)
        {
            nodeLink.IsLocked = _IsLockedAllNodeLinks;
        }

        RaisePropertyChanged(nameof(IsLockedAllNodeLinks));
    }

    void UpdateIsEnableAllNodeConnectorsProperty(bool value)
    {
        _IsEnableAllNodeConnectors = !_IsEnableAllNodeConnectors;

        foreach (var node in _NodeViewModels)
        {
            foreach (var input in node.Inputs)
            {
                input.IsEnable = _IsEnableAllNodeConnectors;
            }
            foreach (var output in node.Outputs)
            {
                output.IsEnable = _IsEnableAllNodeConnectors;
            }
        }

        RaisePropertyChanged(nameof(IsEnableAllNodeConnectors));
    }

    void PreviewConnect(PreviewConnectLinkOperationEventArgs args)
    {
        var inputNode = NodeViewModels.First(arg => arg.Guid == args.ConnectToEndNodeGuid);
        var inputConnector = inputNode.FindConnector(args.ConnectToEndConnectorGuid);
        args.CanConnect = inputConnector.Label == "Limited Input" == false;
    }

    void Connected(ConnectedLinkOperationEventArgs param)
    {
        var nodeLink = new NodeLinkViewModel()
        {
            OutputConnectorGuid = param.OutputConnectorGuid,
            OutputConnectorNodeGuid = param.OutputConnectorNodeGuid,
            InputConnectorGuid = param.InputConnectorGuid,
            InputConnectorNodeGuid = param.InputConnectorNodeGuid,
            IsLocked = IsLockedAllNodeLinks,
        };
        _NodeLinkViewModels.Add(nodeLink);
    }

    void Disconnected(DisconnectedLinkOperationEventArgs param)
    {
        var nodeLink = _NodeLinkViewModels.First(arg => arg.Guid == param.NodeLinkGuid);
        _NodeLinkViewModels.Remove(nodeLink);
    }

    void NodesMoved(EndMoveNodesOperationEventArgs param)
    {

    }

    void SelectionChanged(IList list)
    {

    }

    void AddTestNodeLink()
    {
        if (_NodeViewModels.Count < 2)
        {
            return;
        }
        var nodeLink = new NodeLinkViewModel();
        nodeLink.OutputConnectorGuid = _NodeViewModels[0].Outputs.ElementAt(0).Guid;
        nodeLink.InputConnectorGuid = _NodeViewModels[1].Inputs.ElementAt(0).Guid;
        _NodeLinkViewModels.Add(nodeLink);
    }

    void MoveTestNodes()
    {
        if (_NodeLinkViewModels.Count > 0)
        {
            _NodeViewModels[0].Position = new Point(0, 0);
        }
    }

    #endregion


    // ★★★★★★★★★★★★★★★ 

}

public enum GroupIntersectType
{
    CursorPointVMDefine,
    BoundingBoxVMDefine,
}

public enum RangeSelectionMode
{
    ContainVMDefine,
    IntersectVMDefine,
}
