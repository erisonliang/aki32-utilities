using Livet;
using Livet.Commands;
using Aki32Utilities.WPFAppUtilities.NodeController.Operation;
using NodeGraph.Utilities;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using Aki32Utilities.ViewModels.NodeViewModels;
using Aki32Utilities.ConsoleAppUtilities.SpecificPurposeModels.Research;
using System.IO;

namespace Aki32Utilities.UsageExamples.ResearchArticlesNodeController.ViewModels;

public class MainWindowViewModel : ViewModel
{

    // ★★★★★★★★★★★★★★★ props

    #region inner props

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

    public ResearchArticlesManager ArticlesManager { get; set; }


    // ★★★★★★★★★★★★★★★ inits

    public MainWindowViewModel()
    {
        var localDir = new DirectoryInfo($@"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\ResearchArticleDB");
        ArticlesManager = new ResearchArticlesManager(localDir);
        ArticlesManager.OpenDatabase();

        var positionCounter = 100;

        foreach (var article in ArticlesManager.ArticleDatabase)
        {
            _NodeViewModels.Add(new ResearchArticleNodeViewModel() { Name = "ResearchArticle", Article = article, Position = new Point(100, positionCounter) });
            positionCounter += 300;
        }

        foreach (var article in ArticlesManager.ArticleDatabase)
        {
            if (article.ReferenceDOIs == null || !article.ReferenceDOIs.Any())
                continue;

            var articleNode = _NodeViewModels.FirstOrDefault(x => x is ResearchArticleNodeViewModel ran && ran.Article == article);
            if (articleNode == null)
                continue;

            foreach (var referenceDOI in article.ReferenceDOIs)
            {
                var referenceArticle = ArticlesManager.ArticleDatabase.FirstOrDefault(a => a.DOI == referenceDOI || a.AOI == referenceDOI);
                if (referenceArticle == null)
                    continue;

                var referenceNode = _NodeViewModels.FirstOrDefault(x => x is ResearchArticleNodeViewModel ran && ran.Article == referenceArticle);
                if (referenceNode == null)
                    continue;

                var nodeLink = new NodeLinkViewModel
                {
                    OutputConnectorGuid = referenceNode.Outputs.ElementAt(0).Guid,
                    InputConnectorGuid = articleNode.Inputs.ElementAt(0).Guid
                };
                _NodeLinkViewModels.Add(nodeLink);

            }
        }



        //var addingArticle1 = ResearchArticle.CreateManually("タイトル1", new string[] { "著者1" }, null);
        //var addingArticle2 = ResearchArticle.CreateManually("タイトル2", new string[] { "著者2" }, null);
        //var addingArticle3 = ResearchArticle.CreateManually("タイトル3", new string[] { "著者3" }, null);

        //_NodeViewModels.Add(new ResearchArticleNodeViewModel() { Name = "ResearchArticle", Article = addingArticle1, Position = new Point(100, 100) });
        //_NodeViewModels.Add(new ResearchArticleNodeViewModel() { Name = "ResearchArticle", Article = addingArticle2, Position = new Point(600, 100) });
        //_NodeViewModels.Add(new ResearchArticleNodeViewModel() { Name = "ResearchArticle", Article = addingArticle3, Position = new Point(1100, 100) });



        //_GroupNodeViewModels.Add(new GroupNodeViewModel() { Name = "Group1" });
        //_NodeViewModels.Add(new Test1DefaultNodeViewModel() { Name = "Node1", Body = "Content1", Position = new Point(0, 100) });
        //_NodeViewModels.Add(new Test1DefaultNodeViewModel() { Name = "Node2", Body = "Content2", Position = new Point(100, 200) });
        //_NodeViewModels.Add(new Test1DefaultNodeViewModel() { Name = "Node3", Body = "Content3", Position = new Point(200, 300) });
        //_NodeViewModels.Add(new Test3DefaultNodeViewModel() { Name = "Node4", Body = "OutputsOnlyNode", Position = new Point(500, 100) });
        //_NodeViewModels.Add(new Test4DefaultNodeViewModel() { Name = "Node5", Body = "InputsOnlyNode", Position = new Point(600, 200) });





        // articles from j-stage
        {
            //var builder = new JStageArticleUriBuilder()
            //{
            //    Pubyearfrom = 2022,
            //    Issn = ISSN.Architecture_Structure,
            //    Count = 1000,
            //    //Start = 1,
            //};
            //research.PullArticleInfo(builder);

        }

        // articles from cinii
        {
            //var builder = new CiNiiArticleUriBuilder()
            //{
            //    Count = 5,
            //    ISSN = ISSN.Architecture_Structure,
            //    FreeWord = "小振幅"
            //};
            //research.PullArticleInfo(builder);

        }

        // article from crossref
        {
            //var builder = new CrossRefArticleUriBuilder()
            //{
            //    DOI = "10.3130/aijs.87.822"
            //};
            //research.PullArticleInfo(builder);

        }

        // display
        {
            //research.SaveDatabase(true);
            //research.ArticleDatabase.First(x => x.DOI == "10.3130/aijs.87.822").TryOpenPDF(research.PDFsDirectory);
        }




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
