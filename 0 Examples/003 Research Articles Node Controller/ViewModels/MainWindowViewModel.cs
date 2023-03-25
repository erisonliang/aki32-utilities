using Livet;
using Livet.Commands;
using Aki32Utilities.WPFAppUtilities.NodeController.Operation;
using NodeGraph.Utilities;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using Aki32Utilities.ViewModels.NodeViewModels;
using Aki32Utilities.ConsoleAppUtilities.Research;
using System.IO;
using DocumentFormat.OpenXml.Office.CustomUI;
using PropertyChanged;
using System.IO.Packaging;

namespace Aki32Utilities.UsageExamples.ResearchArticlesNodeController.ViewModels;

public class MainWindowViewModel : ViewModel
{

    // ★★★★★★★★★★★★★★★ felds

    const int NODE_MARGIN_LEFT = 0;
    const int NODE_MARGIN_TOP = 0;
    const int NODE_VERTICAL_SPAN = 250;
    const int NODE_HORIZONTAL_SPAN = 500;

    public static DirectoryInfo databaseDir = new($@"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\ResearchArticleDB");


    // ★★★★★★★★★★★★★★★ props

    #region inner props

    public string InfoMessage { get; set; }
    private List<string> InfoMessageBuffer = new List<string>();

    public double Scale { get; set; } = 1d;

    public ViewModelCommand AddNodeCommand => _AddNodeCommand.Get(AddNewArticleNode);
    ViewModelCommandHandler _AddNodeCommand = new();

    public ViewModelCommand RemoveNodesCommand => _RemoveNodesCommand.Get(RemoveArticleNodes);
    ViewModelCommandHandler _RemoveNodesCommand = new();

    public ListenerCommand<PreviewConnectLinkOperationEventArgs> PreviewConnectLinkCommand => _PreviewConnectLinkCommand.Get(PreviewConnect);
    ViewModelCommandHandler<PreviewConnectLinkOperationEventArgs> _PreviewConnectLinkCommand = new();

    public ListenerCommand<ConnectedLinkOperationEventArgs> ConnectedLinkCommand => _ConnectedLinkCommand.Get(NodeConnected);
    ViewModelCommandHandler<ConnectedLinkOperationEventArgs> _ConnectedLinkCommand = new();

    public ListenerCommand<DisconnectedLinkOperationEventArgs> DisconnectedLinkCommand => _DisconnectedLinkCommand.Get(NodeDisconnected);
    ViewModelCommandHandler<DisconnectedLinkOperationEventArgs> _DisconnectedLinkCommand = new();

    public ListenerCommand<EndMoveNodesOperationEventArgs> EndMoveNodesCommand => _EndMoveNodesCommand.Get(NodesMoved);
    ViewModelCommandHandler<EndMoveNodesOperationEventArgs> _EndMoveNodesCommand = new();

    public ListenerCommand<IList> SelectionChangedCommand => _SelectionChangedCommand.Get(SelectionChanged);
    ViewModelCommandHandler<IList> _SelectionChangedCommand = new();

    public ViewModelCommand RearrangeNodesAlignLeftCommand => _RearrangeNodesAlignLeftCommand.Get(RearrangeNodesAlignLeft);
    ViewModelCommandHandler _RearrangeNodesAlignLeftCommand = new();

    public ViewModelCommand RearrangeNodesAlignRightCommand => _RearrangeNodesAlignRightCommand.Get(RearrangeNodesAlignRight);
    ViewModelCommandHandler _RearrangeNodesAlignRightCommand = new();

    public ViewModelCommand SaveCommand => _SaveCommand.Get(Save);
    ViewModelCommandHandler _SaveCommand = new();

    public IEnumerable<DefaultNodeViewModel> NodeViewModels => _NodeViewModels;
    ObservableCollection<DefaultNodeViewModel> _NodeViewModels = new();

    public IEnumerable<NodeLinkViewModel> NodeLinkViewModels => _NodeLinkViewModels;
    ObservableCollection<NodeLinkViewModel> _NodeLinkViewModels = new();

    public IEnumerable<GroupNodeViewModel> GroupNodeViewModels => _GroupNodeViewModels;
    ObservableCollection<GroupNodeViewModel> _GroupNodeViewModels = new();

    public RangeSelectionMode[] RangeSelectionModes { get; } = Enum.GetValues(typeof(RangeSelectionMode)).OfType<RangeSelectionMode>().ToArray();

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
    bool _IsEnableAllNodeConnectors = false;

    #endregion

    public static ResearchArticlesManager ResearchArticlesManager { get; set; }

    public ResearchArticleNodeViewModel? SelectingNodeViewModel { get; set; } = null;

    public static string _LocalSearchString;
    public string LocalSearchString
    {
        get => _LocalSearchString;
        set
        {
            if (RaisePropertyChangedIfSet(ref _LocalSearchString, value))
            {
                NotifyResearchArticlesPropertiesChanged();
                _LocalSearchString = value;
            }
        }
    }



    // ★★★★★★★★★★★★★★★ inits

    public MainWindowViewModel()
    {
        InitResearchArticlesManager();

        IsLockedAllNodeLinks = true;
        IsEnableAllNodeConnectors = true;
    }

    private void InitResearchArticlesManager()
    {
        ResearchArticlesManager = new ResearchArticlesManager(databaseDir);
        ResearchArticlesManager.OpenDatabase();
        RedrawResearchArticlesManager();
    }

    private void RedrawResearchArticlesManager()
    {
        _NodeViewModels = new ObservableCollection<DefaultNodeViewModel>();
        _NodeLinkViewModels = new ObservableCollection<NodeLinkViewModel>();

        foreach (var article in ResearchArticlesManager.ArticleDatabase)
            _NodeViewModels.Add(new ResearchArticleNodeViewModel() { Name = "ResearchArticle", Article = article, Position = new Point(0, 0) });

        foreach (var article in ResearchArticlesManager.ArticleDatabase)
        {
            if (article.ReferenceDOIs == null || !article.ReferenceDOIs.Any())
                continue;

            var articleNode = _NodeViewModels.FirstOrDefault(x => x is ResearchArticleNodeViewModel ran && ran.Article == article);
            if (articleNode == null)
                continue;

            foreach (var referenceDOI in article.ReferenceDOIs)
            {
                var referenceArticle = ResearchArticlesManager.ArticleDatabase.FirstOrDefault(a => a.DOI == referenceDOI || a.AOI == referenceDOI);
                if (referenceArticle == null)
                    continue;

                var referenceNode = _NodeViewModels.FirstOrDefault(x => x is ResearchArticleNodeViewModel ran && ran.Article == referenceArticle);
                if (referenceNode == null)
                    continue;

                var nodeLink = new NodeLinkViewModel
                {
                    OutputConnectorNodeGuid = referenceNode.Guid,
                    OutputConnectorGuid = referenceNode.Outputs.ElementAt(0).Guid,
                    InputConnectorNodeGuid = articleNode.Guid,
                    InputConnectorGuid = articleNode.Inputs.ElementAt(0).Guid,
                };
                _NodeLinkViewModels.Add(nodeLink);


            }
        }

        RearrangeNodesAlignLeft();
    }


    // ★★★★★★★★★★★★★★★ methods (general)

    void UpdateInfoMesssage(string message)
    {
        var now = DateTime.Now;
        var newMessage = $" {now:HH:mm:ss}, {message}";
        InfoMessageBuffer.Add(newMessage);

        if (InfoMessageBuffer.Count > 5)
            InfoMessageBuffer = InfoMessageBuffer.TakeLast(5).ToList();

        InfoMessage = string.Join("\r\n", InfoMessageBuffer);
    }


    // ★★★★★★★★★★★★★★★ methods (research manager handling)

    #region research manager methods

    void ResearchManagerHandlingSample()
    {
        // コピペ用に残しとく

        MessageBox.Show("NotImplemented");

    }

    void ResearchManagerAAA()
    {

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

    void NotifyResearchArticlesPropertiesChanged()
    {
        var articleNodes = _NodeViewModels.Select(n => (n is ResearchArticleNodeViewModel run) ? run : null).Where(run => run != null);
        foreach (var articleNode in articleNodes)
            articleNode!.NotifyArticleUpdated();

    }

    void AddNewArticleNode()
    {
        // 作って，新しいNodeも作ってあてがう。

        var newArticle = ResearchArticle.CreateManually(new ResearchArticle_ManualInitInfo
        {
            Manual_ArticleTitle = "■ 未入力",
            Manual_Authors = new string[] { "■ 未入力" },
            Manual_PublishedDate = "■ 未入力",
            Memo = "■ 未入力",
        });

        ResearchArticlesManager.MergeArticleInfo(new List<ResearchArticle> { newArticle });
        _NodeViewModels.Add(new ResearchArticleNodeViewModel() { Article = newArticle, Name = "ResearchArticle" });
    }

    void RemoveArticleNodes()
    {
        var removeNodes = _NodeViewModels.Where(arg => arg.IsSelected).ToArray();
        foreach (var removeNode in removeNodes)
        {
            _NodeViewModels.Remove(removeNode);

            var removeNodeLink = NodeLinkViewModels.FirstOrDefault(arg => arg.InputConnectorNodeGuid == removeNode.Guid || arg.OutputConnectorNodeGuid == removeNode.Guid);
            _NodeLinkViewModels.Remove(removeNodeLink);
        }
    }

    void NodeConnected(ConnectedLinkOperationEventArgs param)
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

        // 引用関係をデータベースに追加
        var parentNode = (ResearchArticleNodeViewModel)NodeViewModels.First(node => node.Guid == nodeLink.OutputConnectorNodeGuid);
        var childNode = (ResearchArticleNodeViewModel)NodeViewModels.First(node => node.Guid == nodeLink.InputConnectorNodeGuid);
        childNode.Article.AddArticleReference(parentNode.Article);
    }

    void NodeDisconnected(DisconnectedLinkOperationEventArgs param)
    {
        var nodeLink = _NodeLinkViewModels.First(arg => arg.Guid == param.NodeLinkGuid);
        _NodeLinkViewModels.Remove(nodeLink);

        // 引用関係をデータベースから削除
        var parentNode = (ResearchArticleNodeViewModel)NodeViewModels.First(node => node.Guid == nodeLink.OutputConnectorNodeGuid);
        var childNode = (ResearchArticleNodeViewModel)NodeViewModels.First(node => node.Guid == nodeLink.InputConnectorNodeGuid);
        childNode.Article.RemoveArticleReference(parentNode.Article);
    }

    void Save()
    {
        UpdateInfoMesssage("保存開始");
        ResearchArticlesManager.SaveDatabase(true, true);
        UpdateInfoMesssage("保存完了");
    }

    #endregion


    // ★★★★★★★★★★★★★★★ methods (node handling)

    #region node handling methods

    void NodeHandlingSample()
    {
        // コピペ用に残しとく

        // void AddTestNodeLink()
        {
            if (_NodeViewModels.Count < 2)
            {
                return;
            }
            var nodeLink = new NodeLinkViewModel
            {
                OutputConnectorNodeGuid = _NodeViewModels[0].Guid,
                OutputConnectorGuid = _NodeViewModels[0].Outputs.ElementAt(0).Guid,
                InputConnectorNodeGuid = _NodeViewModels[1].Guid,
                InputConnectorGuid = _NodeViewModels[1].Inputs.ElementAt(0).Guid,
            };
            _NodeLinkViewModels.Add(nodeLink);
        }

        // void MoveTestNodes()
        {
            if (_NodeLinkViewModels.Any())
                _NodeViewModels[0].Position = new Point(0, 0);
        }

        //void ClearNodes()
        {
            _NodeLinkViewModels.Clear();
            _NodeViewModels.Clear();
        }

        //void ClearNodeLinks()
        {
            _NodeLinkViewModels.Clear();
        }

        //void MoveGroupNode()
        {
            _GroupNodeViewModels[0].InterlockPosition = new Point(0, 0);
        }

        //void ChangeGroupInnerPosition()
        {
            _GroupNodeViewModels[0].InnerPosition = new Point(0, 0);
        }

        //void ChangeGroupInnerSize()
        {
            _GroupNodeViewModels[0].InnerWidth = 300;
            _GroupNodeViewModels[0].InnerHeight = 300;
        }

    }

    void ResetScale()
    {
        Scale = 1.0f;
    }

    void UpdateIsLockedAllNodeLinksProperty(bool value)
    {
        _IsLockedAllNodeLinks = value;

        foreach (var nodeLink in _NodeLinkViewModels)
        {
            nodeLink.IsLocked = _IsLockedAllNodeLinks;
        }

        RaisePropertyChanged(nameof(IsLockedAllNodeLinks));
    }

    void UpdateIsEnableAllNodeConnectorsProperty(bool value)
    {
        _IsEnableAllNodeConnectors = value;

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

    void NodesMoved(EndMoveNodesOperationEventArgs param)
    {

    }

    void PreviewConnect(PreviewConnectLinkOperationEventArgs args)
    {
        var inputNode = NodeViewModels.First(arg => arg.Guid == args.ConnectToEndNodeGuid);
        var inputConnector = inputNode.FindConnector(args.ConnectToEndConnectorGuid);
        args.CanConnect = inputConnector.Label == "Limited Input" == false;
    }

    void SelectionChanged(IList list)
    {
        var hitArticleNodeFlag = false;
        SelectingNodeViewModel = null;

        foreach (var item in list)
        {
            if (item is ResearchArticleNodeViewModel articleNode)
            {
                if (hitArticleNodeFlag)
                    SelectingNodeViewModel = null;
                else
                    SelectingNodeViewModel = articleNode;

                hitArticleNodeFlag = true;
            }
        }

    }

    void RearrangeNodesAlignLeft()
    {
        var rearrangingNodes = new List<DefaultNodeViewModel>(_NodeViewModels);
        OrderByGroup(ref rearrangingNodes);
        var basePosition = new Point(NODE_MARGIN_LEFT, NODE_MARGIN_TOP);
        RearrangeNodesAlignLeft(rearrangingNodes, basePosition, true);
    }

    void RearrangeNodesAlignRight()
    {
        var rearrangingNodes = new List<DefaultNodeViewModel>(_NodeViewModels);
        OrderByGroup(ref rearrangingNodes);
        var basePosition = new Point(NODE_MARGIN_LEFT, NODE_MARGIN_TOP);
        RearrangeNodesAlignRight(rearrangingNodes, basePosition, true);
    }


    void RearrangeNodesAlignLeft(List<DefaultNodeViewModel> rearrangingNodes, Point basePosition, bool toLowerDirection)
    {
        var horizontalCoef = 1;
        var verticalCoef = toLowerDirection ? 1 : -1;
        var InputConnectorNodeGuids = _NodeLinkViewModels.Select(link => link.InputConnectorNodeGuid).ToArray();
        var OutputConnectorNodeGuids = _NodeLinkViewModels.Select(link => link.OutputConnectorNodeGuid).ToArray();

        // ★★★★★ 用いる変数のリセット
        foreach (var rearrangingNode in rearrangingNodes)
        {
            rearrangingNode.__InnerMemo = 0;
            rearrangingNode.Position = new Point(0, 0);
        }


        // ★★★★★ 最初にinputNodeにもoutputNodeにも何もない人をかき集めて問答無用で並べておく。
        var noConnectionNodes = rearrangingNodes.Where(n => !InputConnectorNodeGuids.Contains(n.Guid) && !OutputConnectorNodeGuids.Contains(n.Guid)).ToArray();

        for (int i = 0; i < noConnectionNodes.Length; i++)
        {
            var noConnectionNode = noConnectionNodes[i];
            rearrangingNodes.Remove(noConnectionNode);
            noConnectionNode.Position = new Point(
                basePosition.X + horizontalCoef * 0 * NODE_HORIZONTAL_SPAN,
                basePosition.Y + verticalCoef * i * NODE_VERTICAL_SPAN);

        }


        // ★★★★★ 水平方向の座標候補を算出。
        var noInputNodes = rearrangingNodes.Where(n => !InputConnectorNodeGuids.Contains(n.Guid)).ToList();
        var withInputNodes = rearrangingNodes.Where(n => InputConnectorNodeGuids.Contains(n.Guid)).ToList();

        // ★ inputNodeに何もないNodeを1とする。
        for (int i = 0; i < noInputNodes.Count; i++)
            noInputNodes[i].__InnerMemo = 1;

        // ★ そいつらを最初の親として，全てのNodeに対して，一番経路の長いものを算定する。
        var parentNodeQueue = new Queue<DefaultNodeViewModel>(noInputNodes);

        while (parentNodeQueue.Count > 0)
        {
            var parentNode = parentNodeQueue.Dequeue();
            var childrenNodes = GetChildrenNodes(parentNode, withInputNodes);
            foreach (var childNode in childrenNodes)
            {
                childNode.__InnerMemo = Math.Max(childNode.__InnerMemo, parentNode.__InnerMemo + 1);
                parentNodeQueue.Enqueue(childNode);
            }
        }


        // ★★★★★ どんどん右に接続していく。深さ優先で。最深まで達したら次の列に進む。

        var currentVerticalIndex = 0;

        void ProcessOne(DefaultNodeViewModel parentNode)
        {
            parentNode.Position = new Point(
                basePosition.X + horizontalCoef * parentNode.__InnerMemo * NODE_HORIZONTAL_SPAN,
                basePosition.Y + verticalCoef * currentVerticalIndex * NODE_VERTICAL_SPAN);

            var childrenNodes = GetChildrenNodes(parentNode, withInputNodes);
            if (childrenNodes.Any())
            {
                foreach (var childNode in childrenNodes)
                {
                    ProcessOne(childNode);
                    withInputNodes.Remove(childNode);
                }
            }
            else
            {
                currentVerticalIndex++;
            }
        }

        foreach (var noInputNode in noInputNodes)
            ProcessOne(noInputNode);

    }

    void RearrangeNodesAlignRight(List<DefaultNodeViewModel> rearrangingNodes, Point basePosition, bool toLowerDirection)
    {
        var horizontalCoef = -1;
        var verticalCoef = toLowerDirection ? 1 : -1;
        var InputConnectorNodeGuids = _NodeLinkViewModels.Select(link => link.InputConnectorNodeGuid).ToArray();
        var OutputConnectorNodeGuids = _NodeLinkViewModels.Select(link => link.OutputConnectorNodeGuid).ToArray();

        // ★★★★★ 用いる変数のリセット
        foreach (var rearrangingNode in rearrangingNodes)
        {
            rearrangingNode.__InnerMemo = 0;
            rearrangingNode.Position = new Point(0, 0);
        }


        // ★★★★★ 最初にinputNodeにもoutputNodeにも何もない人をかき集めて問答無用で並べておく。
        var noConnectionNodes = rearrangingNodes.Where(n => !InputConnectorNodeGuids.Contains(n.Guid) && !OutputConnectorNodeGuids.Contains(n.Guid)).ToArray();

        for (int i = 0; i < noConnectionNodes.Length; i++)
        {
            var noConnectionNode = noConnectionNodes[i];
            rearrangingNodes.Remove(noConnectionNode);
            noConnectionNode.Position = new Point(
                basePosition.X + horizontalCoef * 0 * NODE_HORIZONTAL_SPAN,
                basePosition.Y + verticalCoef * i * NODE_VERTICAL_SPAN);

        }


        // ★★★★★ 水平方向の座標候補を算出。
        var noOutputNodes = rearrangingNodes.Where(n => !OutputConnectorNodeGuids.Contains(n.Guid)).ToList();
        var withOutputNodes = rearrangingNodes.Where(n => OutputConnectorNodeGuids.Contains(n.Guid)).ToList();

        // ★ outputNodeに何もないNodeを1とする。
        for (int i = 0; i < noOutputNodes.Count; i++)
            noOutputNodes[i].__InnerMemo = 1;

        // ★ そいつらを最初の親として，全てのNodeに対して，一番経路の長いものを算定する。
        var childrenNodeQueue = new Queue<DefaultNodeViewModel>(noOutputNodes);

        while (childrenNodeQueue.Count > 0)
        {
            var childNode = childrenNodeQueue.Dequeue();
            var parentNodes = GetParentNodes(childNode, withOutputNodes);
            foreach (var parentNode in parentNodes)
            {
                parentNode.__InnerMemo = Math.Max(parentNode.__InnerMemo, childNode.__InnerMemo + 1);
                childrenNodeQueue.Enqueue(parentNode);
            }
        }


        // ★★★★★ どんどん左に接続していく。深さ優先で。最深まで達したら次の列に進む。

        var currentVerticalIndex = 0;

        void ProcessOne(DefaultNodeViewModel childNode)
        {
            childNode.Position = new Point(
                basePosition.X + horizontalCoef * childNode.__InnerMemo * NODE_HORIZONTAL_SPAN,
                basePosition.Y + verticalCoef * currentVerticalIndex * NODE_VERTICAL_SPAN);

            var parentNodes = GetParentNodes(childNode, withOutputNodes);
            if (parentNodes.Any())
            {
                foreach (var parentNode in parentNodes)
                {
                    ProcessOne(parentNode);
                    withOutputNodes.Remove(parentNode);
                }
            }
            else
            {
                currentVerticalIndex++;
            }
        }

        foreach (var noOutputNode in noOutputNodes)
            ProcessOne(noOutputNode);

    }

    void OrderByGroup(ref List<DefaultNodeViewModel> targetNodes)
    {
        var InputConnectorNodeGuids = _NodeLinkViewModels.Select(link => link.InputConnectorNodeGuid).ToArray();
        var OutputConnectorNodeGuids = _NodeLinkViewModels.Select(link => link.OutputConnectorNodeGuid).ToArray();

        // ★★★★★ 用いる変数のリセット
        foreach (var targetNode in targetNodes)
        {
            targetNode.__InnerMemo = -1;
        }


        // ★★★★★ 最初にinputNodeにもoutputNodeにも何もない人に番号を振る。
        {
            var currentTargetNodes = targetNodes.Where(node => node.__InnerMemo < 0);
            var nextGroupNum = targetNodes.Max(node => node.__InnerMemo) + 1;

            var noConnectionNodes = currentTargetNodes.Where(n => !InputConnectorNodeGuids.Contains(n.Guid) && !OutputConnectorNodeGuids.Contains(n.Guid)).ToArray();
            foreach (var noConnectionNode in noConnectionNodes)
                noConnectionNode.__InnerMemo = nextGroupNum++;
        }


        // ★★★★★ どんどん，適当にピックしてそのNodeに関係あるNodeを全て同じグループとしてグループ付けする。
        while (true)
        {
            var currentTargetNodes = targetNodes.Where(node => node.__InnerMemo < 0).ToList();
            var nextGroupNum = targetNodes.Max(node => node.__InnerMemo) + 1;
            if (!currentTargetNodes.Any())
                break;

            var currentTargetNodeQueue = new Queue<DefaultNodeViewModel>();
            currentTargetNodeQueue.Enqueue(currentTargetNodes.First());

            while (currentTargetNodeQueue.Count > 0)
            {
                var targetNode = currentTargetNodeQueue.Dequeue();
                targetNode.__InnerMemo = nextGroupNum;
                currentTargetNodes.Remove(targetNode);

                var parentNodes = GetParentNodes(targetNode, currentTargetNodes);
                foreach (var parentNode in parentNodes)
                    currentTargetNodeQueue.Enqueue(parentNode);

                var childrenNodes = GetChildrenNodes(targetNode, currentTargetNodes);
                foreach (var childNodes in childrenNodes)
                    currentTargetNodeQueue.Enqueue(childNodes);

            }

        }

        targetNodes = targetNodes.OrderBy(node => node.__InnerMemo).ToList();

    }

    IEnumerable<DefaultNodeViewModel> GetChildrenNodes(DefaultNodeViewModel targetNode, IEnumerable<DefaultNodeViewModel> fromThisList = null)
    {
        var childrenNodes = new List<DefaultNodeViewModel>();

        var childNodeGuids = NodeLinkViewModels.Where(link => link.OutputConnectorNodeGuid == targetNode.Guid).Select(link => link.InputConnectorNodeGuid);
        foreach (var childNodeGuid in childNodeGuids)
        {
            var childNode = (fromThisList ?? NodeViewModels).FirstOrDefault(n => n.Guid == childNodeGuid);
            if (childNode == null)
                continue;

            childrenNodes.Add(childNode);
        }

        return childrenNodes;
    }

    IEnumerable<DefaultNodeViewModel> GetParentNodes(DefaultNodeViewModel targetNode, IEnumerable<DefaultNodeViewModel> fromThisList = null)
    {
        var parentNodes = new List<DefaultNodeViewModel>();

        var parentNodeGuids = NodeLinkViewModels.Where(link => link.InputConnectorNodeGuid == targetNode.Guid).Select(link => link.OutputConnectorNodeGuid);
        foreach (var parentNodeGuid in parentNodeGuids)
        {
            var parentNode = (fromThisList ?? NodeViewModels).FirstOrDefault(n => n.Guid == parentNodeGuid);
            if (parentNode == null)
                continue;

            parentNodes.Add(parentNode);
        }

        return parentNodes;
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