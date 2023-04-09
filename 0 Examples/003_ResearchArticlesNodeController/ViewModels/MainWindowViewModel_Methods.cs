using Livet;
using Aki32Utilities.WPFAppUtilities.NodeController.Operation;
using System.Collections;
using System.Windows;
using Aki32Utilities.ViewModels.NodeViewModels;
using Aki32Utilities.ConsoleAppUtilities.Research;
using System.Windows.Input;
using Aki32Utilities.ConsoleAppUtilities.General;
using Microsoft.Win32;
using System.IO;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace Aki32Utilities.UsageExamples.ResearchArticlesNodeController.ViewModels;
public partial class MainWindowViewModel : ViewModel
{

    // ★★★★★★★★★★★★★★★ general

    void NotifyResearchArticlesPropertiesChanged()
    {
        var articleNodes = _NodeViewModels
            .Select(n => (n is ResearchArticleNodeViewModel run) ? run : null)
            .Where(run => run != null)
            ;

        foreach (var articleNode in articleNodes)
            articleNode!.NotifyArticleUpdated();

    }


    // ★★★★★★★★★★★★★★★ ヘッダー内

    async Task Save()
    {
        if (IsSaveBusy)
            return;

        try
        {
            IsSaveBusy = true;
            ResearchArticlesManager.SaveDatabase(true, true);
            await Task.Delay(100);
            Console.WriteLine("💾 保存完了");
            var successAnimationTask = Task.Run(async () =>
            {
                IsSaveDone = true;
                await Task.Delay(2222);
                if (!IsSaveBusy)
                    IsSaveDone = false;
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ 保存失敗：{ex.Message}");
            //MessageBox.Show($"失敗しました。\r\nﾒｯｾｰｼﾞ: {ex.Message}", "保存", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsSaveBusy = false;
        }

    }
    async Task Test()
    {
        //ResearchArticlesManager.ArticleDatabase.Add(new ResearchArticle { Manual_ArticleTitle = DateTime.Now.ToLongTimeString() });
        //ResearchArticlesManager.ArticleDatabase.RemoveAt(3);

        //ResearchArticlesManager.MergeIfMergeableForAll(false);

        RedrawResearchArticleNodes();

        //MoveCanvasToTargetArticle(NodeViewModels.FirstOrDefault());
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


    // ★★★★★★★★★★★★★★★ 右クリック

    void AddNewArticleNode()
    {
        var posOnCanvas = Mouse.GetPosition(ParentView.NodeGraph.Canvas);
        var posOnContextMenu = Mouse.GetPosition(ParentView.Resources["NodeGraphContextMenu"] as IInputElement);

        var offsetPosOnCanvasX = posOnCanvas.X - CanvasOffset.X - posOnContextMenu.X / Scale;
        var offsetPosOnCanvasY = posOnCanvas.Y - CanvasOffset.Y - posOnContextMenu.Y / Scale;
        var addingPosition = new Point(offsetPosOnCanvasX, offsetPosOnCanvasY);

        AddNewArticleNode(addingPosition);
    }
    void AddNewArticleNode(Point addingPosition)
    {
        // 作って，新しいNodeも作ってあてがう。
        var newArticle = ResearchArticle.CreateManually(new ResearchArticle_ManualInitInfo { });
        newArticle.Manual_ArticleTitle = $"新しい文献 {newArticle.Friendly_AOI}";

        ResearchArticlesManager.MergeArticleInfo(new List<ResearchArticle> { newArticle }, forceAdd: true, save: false);
        var addingNode = new ResearchArticleNodeViewModel() { NodeName = "文献", Article = newArticle };
        _NodeViewModels.Add(addingNode);
        addingNode.Position = addingPosition;
        var saveTask = Save();
    }

    void RearrangeNodesChronologicallyAlignLeft()
    {
        var rearrangingNodes = new List<DefaultNodeViewModel>(_NodeViewModels);
        OrderByGroup(ref rearrangingNodes);
        var basePosition = new Point(NODE_MARGIN_LEFT, NODE_MARGIN_TOP);
        RearrangeNodesChronologicallyAlignLeft(rearrangingNodes, basePosition, true);
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
    void RearrangeNodesAlignLeft(List<DefaultNodeViewModel> rearrangingNodes, Point basePosition, bool toLowerDirection)
    {
        var horizontalCoef = 1;
        var verticalCoef = toLowerDirection ? 1 : -1;


        // ★★★★★ 整列対象の要素に関連するリンクのみに対して実行。
        var rearrangingNodeGuids = rearrangingNodes.Select(node => node.Guid).ToArray();
        var targetLinkLinks = _NodeLinkViewModels.Where(link => rearrangingNodeGuids.Contains(link.InputConnectorNodeGuid) && rearrangingNodeGuids.Contains(link.OutputConnectorNodeGuid)).ToArray();
        var allInputConnectorNodeGuids = targetLinkLinks.Select(link => link.InputConnectorNodeGuid).ToArray();
        var InputConnectorNodeGuids = rearrangingNodes.Select(node => node.Guid).Where(guid => allInputConnectorNodeGuids.Contains(guid)).ToArray();
        var allOutputConnectorNodeGuids = targetLinkLinks.Select(link => link.OutputConnectorNodeGuid).ToArray();
        var OutputConnectorNodeGuids = rearrangingNodes.Select(node => node.Guid).Where(guid => allOutputConnectorNodeGuids.Contains(guid)).ToArray();


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


        // ★★★★★ 整列対象の要素に関連するリンクのみに対して実行。
        var rearrangingNodeGuids = rearrangingNodes.Select(node => node.Guid).ToArray();
        var targetLinkLinks = _NodeLinkViewModels.Where(link => rearrangingNodeGuids.Contains(link.InputConnectorNodeGuid) && rearrangingNodeGuids.Contains(link.OutputConnectorNodeGuid)).ToArray();
        var allInputConnectorNodeGuids = targetLinkLinks.Select(link => link.InputConnectorNodeGuid).ToArray();
        var InputConnectorNodeGuids = rearrangingNodes.Select(node => node.Guid).Where(guid => allInputConnectorNodeGuids.Contains(guid)).ToArray();
        var allOutputConnectorNodeGuids = targetLinkLinks.Select(link => link.OutputConnectorNodeGuid).ToArray();
        var OutputConnectorNodeGuids = rearrangingNodes.Select(node => node.Guid).Where(guid => allOutputConnectorNodeGuids.Contains(guid)).ToArray();


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
    void RearrangeNodesChronologicallyAlignLeft(List<DefaultNodeViewModel> rearrangingNodes, Point basePosition, bool toLowerDirection)
    {
        RearrangeNodesAlignLeft(rearrangingNodes, basePosition, toLowerDirection);
        MessageBox.Show($"申し訳ありません。\r\n未実装です…。", "", MessageBoxButton.OK, MessageBoxImage.Error);
    }
    void RearrangeNodesToEdge(List<DefaultNodeViewModel>? rearrangingNodes, bool isLeft = true, bool isTop = true)
    {
        if (rearrangingNodes is null)
            return;

        var NodesMinX = _NodeViewModels.Min(n => n.Position.X);
        var NodesMaxX = _NodeViewModels.Max(n => n.Position.X);
        var NodesMinY = _NodeViewModels.Min(n => n.Position.Y);
        var NodesMaxY = _NodeViewModels.Max(n => n.Position.Y);

        double X = isLeft ? NodesMinX - NODE_HORIZONTAL_SPAN : NodesMaxX + NODE_HORIZONTAL_SPAN;
        double Y = isTop ? NodesMinY - NODE_VERTICAL_SPAN : NodesMaxY + NODE_VERTICAL_SPAN;
        Point basePosition = new(X, Y);

        RearrangeNodesAlignLeft(rearrangingNodes, basePosition, !isTop);
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

    void RemoveSelectingArticleNodes()
    {
        var removingNodes = _NodeViewModels
            .Where(node => node.IsSelected)
            .Where(node => node is ResearchArticleNodeViewModel)
            .Cast<ResearchArticleNodeViewModel>()
            .ToArray();

        if (removingNodes.Length == 0)
            return;

        var result = MessageBox.Show($"選択中の{removingNodes.Length}個の文献を全て削除しようとしています。\r\n本当によろしいですか？", "確認", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
        if (result is not MessageBoxResult.OK)
            return;

        RemoveArticleNodes(removingNodes);

        var saveTask = Save();
    }

    void RemoveTempArticleNodes()
    {
        var removingNodes = _NodeViewModels
            .Where(node => node is ResearchArticleNodeViewModel)
            .Cast<ResearchArticleNodeViewModel>()
            .Where(node => node.Article.Private_Temporary ?? false)
            .ToArray();

        if (removingNodes.Length == 0)
            return;

        var result = MessageBox.Show($"{removingNodes.Length}個の一時的な文献データを全て削除しようとしています。\r\n本当によろしいですか？", "確認", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
        if (result is not MessageBoxResult.OK)
            return;

        // remove from node controller
        RemoveArticleNodes(removingNodes);

        var saveTask = Save();
    }

    void RemoveArticleNodes(ResearchArticleNodeViewModel[] removingNodes)
    {
        // remove from node controller
        foreach (var removingNode in removingNodes)
        {
            // remove all node links...
            var removingNodeLinks = NodeLinkViewModels
                .Where(arg => arg.InputConnectorNodeGuid == removingNode.Guid || arg.OutputConnectorNodeGuid == removingNode.Guid)
                .ToArray();
            foreach (var removingNodeLink in removingNodeLinks)
                _NodeLinkViewModels.Remove(removingNodeLink);

            // remove node
            _NodeViewModels.Remove(removingNode);
        }

        // remove from database
        ResearchArticlesManager.RemoveArticleInfo(removingNodes.Select(n => n.Article).ToList(), save: false);
    }

    void MergeIfMergeableForAll()
    {
        ResearchArticlesManager.MergeIfMergeableForAll(false);
        var saveTask = Save();
        RedrawResearchArticleNodes();
    }

    void AcceptSelectingArticleNodes()
    {
        _NodeViewModels
            .Where(node => node.IsSelected)
            .Where(node => node is ResearchArticleNodeViewModel)
            .Cast<ResearchArticleNodeViewModel>()
            .ForEach(n => n.IsTemp = false);

        var saveTask = Save();
    }

    void MergeSelectingTwoArticleNodes()
    {
        var selectingNodes = _NodeViewModels
          .Where(node => node.IsSelected)
          .Where(node => node is ResearchArticleNodeViewModel)
          .Cast<ResearchArticleNodeViewModel>()
          .ToArray();

        if (selectingNodes.Length != 2)
        {
            MessageBox.Show($"呼び出すには，ちょうど2つ文献を選択してください。", "2つの文献をマージ", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        ResearchArticlesManager.MergeArticles(selectingNodes[0].Article, selectingNodes[1].Article);
        selectingNodes[0].NotifyArticleUpdated();
        RedrawResearchArticleNodes();

        var saveTask = Save();
    }

    // ★★★★★★★★★★★★★★★ Node Controller 内

    void SelectionChanged(IList list)
    {
        var hitArticleNodeFlag = false;
        SelectedNodeViewModel = null;

        foreach (var item in list)
        {
            if (item is ResearchArticleNodeViewModel articleNode)
            {
                if (hitArticleNodeFlag)
                    SelectedNodeViewModel = null;
                else
                    SelectedNodeViewModel = articleNode;

                hitArticleNodeFlag = true;
            }
        }

        if (SelectedNodeViewModel is not null)
            ParentView.TabControl_RightPanel.SelectedItem = ParentView.TabItem_SelectingNode;

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


    // ★★★★★★★★★★★★★★★ 右パネル内 → 選択中の文献

    async Task PullReferenceInfo()
    {
        if (IsPullReferenceInfoBusy)
            return;
        ResearchArticleNodeViewModel selectedNode = null;

        try
        {
            selectedNode = SelectedNodeViewModel ?? throw new Exception("文献が選択されていません。");
            selectedNode.IsNodeBusy = true;
            IsPullReferenceInfoBusy = true;

            if (string.IsNullOrEmpty(selectedNode.Article.DOI))
                throw new Exception("DOIがない文献はこの機能を使うことができません。");

            var pulledArticles = new List<ResearchArticle>();

            // access until match any
            var accessorInfos = new Dictionary<string, IResearchAPIAccessor>()
            {
                { "CrossRef", new CrossRef_DOI_APIAccessor() { DOI = selectedNode.Article.DOI! } },
                { "J-Stage", new JStage_DOI_ArticleAPIAccessor() { DOI = selectedNode.Article.DOI! } },
            };
            foreach (var accessorInfo in accessorInfos)
            {
                var siteName = accessorInfo.Key;
                var accessor = accessorInfo.Value;

                try
                {
                    Console.WriteLine($"★ {siteName} にアクセス中…");
                    pulledArticles = await ResearchArticlesManager.PullArticleInfo(accessor, asTempArticles: true, save: false);
                    if (!pulledArticles.Any())
                        throw new Exception("マッチするデータがありませんでした。");
                }
                catch (Exception ex1)
                {
                    Console.WriteLine($"{ex1}\r\n{siteName} がこの文献に対応していない可能性があります。");
                }

                if (pulledArticles is not null && pulledArticles.Any())
                    break;

            }

            // not found
            if (pulledArticles is null || pulledArticles.Count == 0)
                throw new Exception($"CrossRef および J-Stage がこの文献に対応していない，もしくは引用関係が存在しない資料である可能性があります。");

            // ノードを，対象のやつの左側に，下方向に追加列挙！
            var rearrangeNodesAction = (List<DefaultNodeViewModel> pulledTempArticleNodes) =>
            {
                var selectedNodePosition = selectedNode.Position;
                Point basePosition = new(selectedNodePosition.X - NODE_HORIZONTAL_SPAN, selectedNodePosition.Y);

                var nodes = pulledTempArticleNodes
                .Where(n => n != selectedNode)
                .ToList();

                RearrangeNodesAlignRight(nodes, basePosition, true);
            };
            InternetSearchPostProcess(pulledArticles, false, rearrangeNodesAction);
            selectedNode.NotifyArticleUpdated();

            // 全てに対して GPT予測！？？
            var pulledCount = pulledArticles.Count;
            var result_UseGPT = MessageBox.Show($"取得に成功した {pulledCount} 件の文献に対して，AIによるメタ情報推定を行いますか？", "引用関係を取得", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            if (result_UseGPT == MessageBoxResult.OK)
            {
                var currentFriendlyIndex = 0;
                var failCount = 0;

                GetNodesFromArticles(pulledArticles).ForEach(n => n.IsNodeBusy = true);

                foreach (var targetArticle in pulledArticles)
                {
                    currentFriendlyIndex++;

                    Console.WriteLine($"{currentFriendlyIndex}/{pulledCount}件を処理中…");
                    ResearchArticle? mergeResult = null;

                    try
                    {
                        if (ResearchArticlesManager.ArticleDatabase.Contains(targetArticle))
                        {
                            var predictResult = await targetArticle.TryPredictMetaInfo_ChatGPT();
                            if (!predictResult)
                                throw new Exception("推測に失敗しました。");

                            mergeResult = ResearchArticlesManager.MergeIfMergeable(targetArticle);
                            RedrawResearchArticleNodes();
                            if (mergeResult is not null)
                                Console.WriteLine("同一の文献を発見したため，マージしました。");

                        }
                        else
                        {
                            // 既にマージされたものの可能性が高い。成功扱い。
                            Console.WriteLine("既に他の文献にマージされた文献です。");
                        }

                        Console.WriteLine($"成功。(成功 {currentFriendlyIndex - failCount}件，失敗 {failCount}件，残り {pulledCount - currentFriendlyIndex}件)");
                    }
                    catch (Exception ex)
                    {
                        failCount++;
                        Console.WriteLine($"失敗。(成功: {currentFriendlyIndex - failCount}/{currentFriendlyIndex}件) ﾒｯｾｰｼﾞ: {ex.Message}");
                    }
                    finally
                    {
                        var targetArticleNode = GetNodeFromArticle(mergeResult ?? targetArticle);
                        if (targetArticleNode is not null)
                        {
                            targetArticleNode.NotifyArticleUpdated();
                            targetArticleNode.IsNodeBusy = false;
                        }
                    }
                }

                Console.WriteLine($"★ メタ情報の収集が完了しました。(成功: {pulledCount - failCount}/{pulledCount}件)");
            }

            SelectedEmphasizePropertyItem = ViewModels.EmphasizePropertyItems.一時ﾃﾞｰﾀ;
            var saveTask = Save();

            var successAnimationTask = Task.Run(async () =>
            {
                IsPullReferenceInfoDone = true;
                await Task.Delay(2222);
                if (!IsPullReferenceInfoBusy)
                    IsPullReferenceInfoDone = false;
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"失敗しました。\r\nﾒｯｾｰｼﾞ: {ex.Message}", "引用関係を取得", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsPullReferenceInfoBusy = false;
            if (selectedNode is not null)
                selectedNode.IsNodeBusy = false;
        }
    }

    async Task PullNormalMetaInfo()
    {
        if (IsPullNormalMetaInfoBusy)
            return;
        ResearchArticleNodeViewModel selectedNode = null;

        try
        {
            selectedNode = SelectedNodeViewModel ?? throw new Exception("文献が選択されていません。");
            selectedNode.IsNodeBusy = true;
            IsPullNormalMetaInfoBusy = true;

            // 対象のサイトからの情報
            // TODO
            throw new NotImplementedException("申し訳ありません。\r\n未実装です…。");
            {
                var accessor = new CrossRef_DOI_APIAccessor()
                {
                    DOI = selectedNode.Article.DOI!,
                };

                var pulledArticles = await ResearchArticlesManager.PullArticleInfo(accessor, asTempArticles: true, save: false);
                if (pulledArticles.Count == 0)
                    throw new Exception("マッチするデータがありませんでした。");
            }

            RedrawResearchArticleNodes();
            SelectedEmphasizePropertyItem = ViewModels.EmphasizePropertyItems.一時ﾃﾞｰﾀ;


            selectedNode.NotifyArticleUpdated();

            var saveTask = Save();

            var successAnimationTask = Task.Run(async () =>
            {
                IsPullNormalMetaInfoDone = true;
                await Task.Delay(2222);
                if (!IsPullNormalMetaInfoBusy)
                    IsPullNormalMetaInfoDone = false;
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"失敗しました。\r\nﾒｯｾｰｼﾞ: {ex.Message}", "メタ情報を取得", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsPullNormalMetaInfoBusy = false;
            if (selectedNode is not null)
                selectedNode.IsNodeBusy = false;
        }
    }

    async Task PullAIMetaInfo()
    {
        if (IsPullAIMetaInfoBusy)
            return;
        ResearchArticleNodeViewModel selectedNode = null;
        ResearchArticle? mergeResult = null;

        try
        {
            selectedNode = SelectedNodeViewModel ?? throw new Exception("文献が選択されていません。");
            selectedNode.IsNodeBusy = true;
            IsPullAIMetaInfoBusy = true;

            // ChatGPTによる推定
            var selectedArticle = selectedNode.Article;
            var predictResult = await selectedArticle.TryPredictMetaInfo_ChatGPT();
            if (!predictResult)
                throw new Exception("推測に失敗しました。");

            mergeResult = ResearchArticlesManager.MergeIfMergeable(selectedArticle);
            if (mergeResult is not null)
                Console.WriteLine("同一の文献を発見したため，マージしました。");

            var saveTask = Save();

            var successAnimationTask = Task.Run(async () =>
            {
                IsPullAIMetaInfoDone = true;
                await Task.Delay(2222);
                if (!IsPullAIMetaInfoBusy)
                    IsPullAIMetaInfoDone = false;
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"失敗しました。\r\nﾒｯｾｰｼﾞ: {ex.Message}", "メタ情報を推測", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsPullAIMetaInfoBusy = false;
            var targetArticleNode = mergeResult is not null ? GetNodeFromArticle(mergeResult) : selectedNode;
            if (targetArticleNode is not null)
            {
                targetArticleNode.NotifyArticleUpdated();
                targetArticleNode.IsNodeBusy = false;
            }
        }
    }

    async Task OpenPDF()
    {
        if (IsOpenPDFBusy)
            return;
        ResearchArticleNodeViewModel selectedNode = null;

        try
        {
            selectedNode = SelectedNodeViewModel ?? throw new Exception("文献が選択されていません。");
            selectedNode.IsNodeBusy = true;
            IsOpenPDFBusy = true;

            var result = await selectedNode.Article.TryOpenPDF(ResearchArticlesManager.PDFsDirectory);
            if (!result.download)
                throw new Exception("PDFのダウンロードに失敗しました。\r\nこの情報提供元に対応していない可能性があります。");
            if (!result.download)
                throw new Exception("PDFを開くのに失敗しました。\r\n他のプロセスによって使われていないか確認してください。");

            var successAnimationTask = Task.Run(async () =>
            {
                IsOpenPDFDone = true;
                await Task.Delay(2222);
                if (!IsOpenPDFBusy)
                    IsOpenPDFDone = false;
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"失敗しました。\r\nﾒｯｾｰｼﾞ: {ex.Message}", "Webサイトを表示", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsOpenPDFBusy = false;
            if (selectedNode is not null)
                selectedNode.IsNodeBusy = false;
        }
    }

    void OpenDOIWebSite()
    {
        if (IsOpenDOIWebSiteBusy)
            return;
        ResearchArticleNodeViewModel selectedNode = null;

        try
        {
            selectedNode = SelectedNodeViewModel ?? throw new Exception("文献が選択されていません。");
            selectedNode.IsNodeBusy = true;
            IsOpenDOIWebSiteBusy = true;

            var result = selectedNode.Article.TryOpenDOILink();
            if (!result)
                throw new Exception("DOIを保持していない文献の可能性があります。");

            var successAnimationTask = Task.Run(async () =>
            {
                IsOpenDOIWebSiteDone = true;
                await Task.Delay(2222);
                if (!IsOpenDOIWebSiteBusy)
                    IsOpenDOIWebSiteDone = false;
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"失敗しました。\r\nﾒｯｾｰｼﾞ: {ex.Message}", "Webサイトを表示", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsOpenDOIWebSiteBusy = false;
            if (selectedNode is not null)
                selectedNode.IsNodeBusy = false;
        }
    }

    void ExecuteAll()
    {
        if (IsExecuteAllBusy)
            return;
        ResearchArticleNodeViewModel selectedNode = null;

        try
        {
            selectedNode = SelectedNodeViewModel ?? throw new Exception("文献が選択されていません。");
            selectedNode.IsNodeBusy = true;
            IsExecuteAllBusy = true;

            throw new NotImplementedException("申し訳ありません。\r\n未実装です…。");

            var successAnimationTask = Task.Run(async () =>
            {
                IsExecuteAllDone = true;
                await Task.Delay(2222);
                if (!IsExecuteAllBusy)
                    IsExecuteAllDone = false;
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"失敗しました。\r\nﾒｯｾｰｼﾞ: {ex.Message}", "AIにまとめてもらう", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsExecuteAllBusy = false;
            if (selectedNode is not null)
                selectedNode.IsNodeBusy = false;
        }
    }

    void AISummary()
    {
        if (IsAISummaryBusy)
            return;
        ResearchArticleNodeViewModel selectedNode = null;

        try
        {
            selectedNode = SelectedNodeViewModel ?? throw new Exception("文献が選択されていません。");
            selectedNode.IsNodeBusy = true;
            IsAISummaryBusy = true;

            var successAnimationTask = Task.Run(async () =>
            {
                IsAISummaryDone = true;
                await Task.Delay(2222);
                if (!IsAISummaryBusy)
                    IsAISummaryDone = false;
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"失敗しました。\r\nﾒｯｾｰｼﾞ: {ex.Message}", "AIにまとめてもらう", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsAISummaryBusy = false;
            if (selectedNode is not null)
                selectedNode.IsNodeBusy = false;
        }
    }

    void ManuallyAddPDF()
    {
        if (IsManuallyAddPDFBusy)
            return;
        ResearchArticleNodeViewModel selectedNode = null;

        try
        {
            selectedNode = SelectedNodeViewModel ?? throw new Exception("文献が選択されていません。");
            selectedNode.IsNodeBusy = true;
            IsManuallyAddPDFBusy = true;

            //
            var dialog = new OpenFileDialog
            {
                Filter = "PDFファイル (*.pdf)|*.pdf"
            };
            if (dialog.ShowDialog() != true)
                return;

            //
            var addingPDFFile = new FileInfo(dialog.FileName);
            ManuallyAddPDF_FromFileInfo(addingPDFFile);

            var successAnimationTask = Task.Run(async () =>
            {
                IsManuallyAddPDFDone = true;
                await Task.Delay(2222);
                if (!IsManuallyAddPDFBusy)
                    IsManuallyAddPDFDone = false;
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"失敗しました。\r\nﾒｯｾｰｼﾞ: {ex.Message}", "PDF手動追加", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsManuallyAddPDFBusy = false;
            if (selectedNode is not null)
                selectedNode.IsNodeBusy = false;
        }
    }
    public void Button_ManuallyAddPDF_Drop(object sender, DragEventArgs e)
    {
        if (IsManuallyAddPDFBusy)
            return;
        ResearchArticleNodeViewModel selectedNode = null;

        try
        {
            selectedNode = SelectedNodeViewModel ?? throw new Exception("文献が選択されていません。");
            selectedNode.IsNodeBusy = true;
            IsManuallyAddPDFBusy = true;

            //
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            var droppedFileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (droppedFileNames.Length > 1)
                throw new Exception("複数のファイルがドロップされました。");

            var droppedFileName = droppedFileNames[0];
            if (Path.GetExtension(droppedFileName) != ".pdf")
                if (Path.GetExtension(droppedFileName) != ".PDF")
                    throw new Exception(".pdf ファイルのみ受付可能です。");


            //
            var addingPDFFile = new FileInfo(droppedFileName);
            ManuallyAddPDF_FromFileInfo(addingPDFFile);

            var successAnimationTask = Task.Run(async () =>
            {
                IsManuallyAddPDFDone = true;
                await Task.Delay(2222);
                if (!IsManuallyAddPDFBusy)
                    IsManuallyAddPDFDone = false;
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"失敗しました。\r\nﾒｯｾｰｼﾞ: {ex.Message}", "PDF手動追加", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsManuallyAddPDFBusy = false;
            if (selectedNode is not null)
                selectedNode.IsNodeBusy = false;
        }
    }
    private void ManuallyAddPDF_FromFileInfo(FileInfo addingPDFFile)
    {
        //
        var selectedNode = SelectedNodeViewModel ?? throw new Exception("文献が選択されていません。");

        if (selectedNode.Article.TryFindPDF(ResearchArticlesManager.PDFsDirectory))
        {
            var result_ForceOverwrite = MessageBox.Show($"この文献には既にPDFが存在していて，上書きしようとしています。\r\n本当によろしいですか？", "PDF手動追加", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result_ForceOverwrite is not MessageBoxResult.OK)
                return;
        }

        //
        var result_DeleteOriginal = MessageBox.Show($"コピー元のPDFを削除しますか？", "PDF手動追加", MessageBoxButton.OKCancel, MessageBoxImage.Information);
        var deleteOriginalPdfFile = result_DeleteOriginal is MessageBoxResult.OK;

        //
        selectedNode.Article.AddPDFManually(ResearchArticlesManager.PDFsDirectory,
            addingPDFFile, deleteOriginalPdfFile,
            forceOverwriteExistingPDF: true);

        //
        MessageBox.Show($"追加に成功しました。", "PDF手動追加", MessageBoxButton.OK, MessageBoxImage.Information);

    }

    async Task UndefinedButton1()
    {
        if (IsUndefinedButton1Busy)
            return;
        ResearchArticleNodeViewModel selectedNode = null;

        try
        {
            selectedNode = SelectedNodeViewModel ?? throw new Exception("文献が選択されていません。");
            selectedNode.IsNodeBusy = true;
            IsUndefinedButton1Busy = true;


            await Task.Delay(2222);


            var successAnimationTask = Task.Run(async () =>
            {
                IsUndefinedButton1Done = true;
                await Task.Delay(2222);
                if (!IsUndefinedButton1Busy)
                    IsUndefinedButton1Done = false;
            });
        }
        catch (Exception ex)
        {
        }
        finally
        {
            IsUndefinedButton1Busy = false;
            if (selectedNode is not null)
                selectedNode.IsNodeBusy = false;
        }
    }


    // ★★★★★★★★★★★★★★★ 右パネル内 → 検索

    async Task OpenLocalSearch()
    {
        ParentView.TabControl_RightPanel.SelectedItem = ParentView.TabItem_Search;
        await Task.Delay(10);
        ParentView.TextBox_LocalSearch.Focus();
    }

    async Task InternetSearch_CiNii_A_()
    {
        if (IsInternetSearch_CiNii_A_Busy)
            return;

        try
        {
            IsInternetSearch_CiNii_A_Busy = true;

            var accessor = new CiNii_Main_ArticleAPIAccessor();
            {
                //accessor.ISSN = ISSN.Architecture_Structure;

                if (string.IsNullOrEmpty(InternetSearch_FreeWord))
                    throw new InvalidDataException("フリーワードを埋めてください。");
                accessor.FreeWord = InternetSearch_FreeWord;

                if (!int.TryParse(InternetSearch_DataMaxCount, out int dataMaxCount))
                    throw new InvalidDataException("「最大データ数」を正しい数値形式で入力してください。");
                accessor.RecordCount = dataMaxCount;

                if (!string.IsNullOrEmpty(InternetSearch_PublishedFrom))
                {
                    if (!int.TryParse(InternetSearch_PublishedFrom, out int publishedFrom))
                        throw new InvalidDataException("「何年から」を正しい数値形式で入力してください。\r\nもしくは空欄にしてください。");
                    accessor.PublishedFrom = publishedFrom;
                }
                if (!string.IsNullOrEmpty(InternetSearch_PublishedUntil))
                {
                    if (!int.TryParse(InternetSearch_PublishedUntil, out int publishedUntil))
                        throw new InvalidDataException("「何年まで」を正しい数値形式で入力してください。\r\nもしくは空欄にしてください。");
                    accessor.PublishedUntil = publishedUntil;
                }

            }

            await InternetSearchPullProcess(accessor);
            var saveTask = Save();
            var successAnimationTask = Task.Run(async () =>
            {
                IsInternetSearch_CiNii_A_Done = true;
                await Task.Delay(2222);
                if (!IsInternetSearch_CiNii_A_Busy)
                    IsInternetSearch_CiNii_A_Done = false;
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"失敗しました。\r\nﾒｯｾｰｼﾞ: {ex.Message}", "CiNiiで検索", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsInternetSearch_CiNii_A_Busy = false;
        }
    }
    async Task InternetSearch_NDLSearch_A_()
    {
        if (IsInternetSearch_NDLSearch_A_Busy)
            return;

        try
        {
            IsInternetSearch_NDLSearch_A_Busy = true;

            var accessor = new NDLSearch_Main_APIAccessor();
            {
                //accessor.ISSN = ISSN.Architecture_Structure;

                if (string.IsNullOrEmpty(InternetSearch_FreeWord))
                    throw new InvalidDataException("フリーワードを埋めてください。");
                accessor.FreeWord = InternetSearch_FreeWord;

                if (!int.TryParse(InternetSearch_DataMaxCount, out int dataMaxCount))
                    throw new InvalidDataException("「最大データ数」を正しい数値形式で入力してください。");
                accessor.RecordCount = dataMaxCount;

                if (!string.IsNullOrEmpty(InternetSearch_PublishedFrom))
                {
                    if (!int.TryParse(InternetSearch_PublishedFrom, out int publishedFrom))
                        throw new InvalidDataException("「何年から」を正しい数値形式で入力してください。\r\nもしくは空欄にしてください。");
                    accessor.PublishedFrom = publishedFrom;
                }

                if (!string.IsNullOrEmpty(InternetSearch_PublishedUntil))
                {
                    if (!int.TryParse(InternetSearch_PublishedUntil, out int publishedUntil))
                        throw new InvalidDataException("「何年まで」を正しい数値形式で入力してください。\r\nもしくは空欄にしてください。");
                    accessor.PublishedUntil = publishedUntil;
                }

            }

            await InternetSearchPullProcess(accessor);
            var saveTask = Save();
            var successAnimationTask = Task.Run(async () =>
            {
                IsInternetSearch_NDLSearch_A_Done = true;
                await Task.Delay(2222);
                if (!IsInternetSearch_NDLSearch_A_Busy)
                    IsInternetSearch_NDLSearch_A_Done = false;
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"失敗しました。\r\nﾒｯｾｰｼﾞ: {ex.Message}", "NDLSearchで検索", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsInternetSearch_NDLSearch_A_Busy = false;
        }
    }

    async Task InternetSearch_JStage_B_()
    {
        if (IsInternetSearch_JStage_B_Busy)
            return;

        try
        {
            IsInternetSearch_JStage_B_Busy = true;

            var accessor = new JStage_Main_ArticleAPIAccessor();
            {
                //accessor.ISSN = ISSN.Architecture_Structure;

                if (!string.IsNullOrEmpty(InternetSearch_Authors))
                    accessor.AuthorName = InternetSearch_Authors;

                if (!string.IsNullOrEmpty(InternetSearch_ArticleTitle))
                    accessor.ArticleTitle = InternetSearch_ArticleTitle;

                if (!string.IsNullOrEmpty(InternetSearch_MaterialTitle))
                    accessor.MaterialTitle = InternetSearch_MaterialTitle;

                if (!string.IsNullOrEmpty(InternetSearch_MaterialVolume))
                {
                    if (!int.TryParse(InternetSearch_MaterialVolume, out int _))
                        throw new InvalidDataException("「巻」を正しい数値形式で入力してください。\r\nもしくは空欄にしてください。");
                    accessor.MaterialVolume = InternetSearch_MaterialVolume;
                }

                if (!int.TryParse(InternetSearch_DataMaxCount, out int dataMaxCount))
                    throw new InvalidDataException("「最大データ数」を正しい数値形式で入力してください。");
                accessor.RecordCount = dataMaxCount;

                if (!string.IsNullOrEmpty(InternetSearch_PublishedFrom))
                {
                    if (!int.TryParse(InternetSearch_PublishedFrom, out int publishedFrom))
                        throw new InvalidDataException("「何年から」を正しい数値形式で入力してください。\r\nもしくは空欄にしてください。");
                    accessor.PublishedFrom = publishedFrom;
                }

                if (!string.IsNullOrEmpty(InternetSearch_PublishedUntil))
                {
                    if (!int.TryParse(InternetSearch_PublishedUntil, out int publishedUntil))
                        throw new InvalidDataException("「何年まで」を正しい数値形式で入力してください。\r\nもしくは空欄にしてください。");
                    accessor.PublishedUntil = publishedUntil;
                }

                if (!string.IsNullOrEmpty(InternetSearch_Keywords))
                    accessor.KeyWord = InternetSearch_Keywords;

            }

            await InternetSearchPullProcess(accessor);
            var saveTask = Save();
            var successAnimationTask = Task.Run(async () =>
            {
                IsInternetSearch_JStage_B_Done = true;
                await Task.Delay(2222);
                if (!IsInternetSearch_JStage_B_Busy)
                    IsInternetSearch_JStage_B_Done = false;
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"失敗しました。\r\nﾒｯｾｰｼﾞ: {ex.Message}", "J-Stageで検索", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsInternetSearch_JStage_B_Busy = false;
        }
    }
    async Task InternetSearch_CiNii_B_()
    {
        if (IsInternetSearch_CiNii_B_Busy)
            return;

        try
        {
            IsInternetSearch_CiNii_B_Busy = true;

            var accessor = new CiNii_Main_ArticleAPIAccessor();
            {
                //accessor.ISSN = ISSN.Architecture_Structure;

                if (!string.IsNullOrEmpty(InternetSearch_Authors))
                    accessor.AuthorName = InternetSearch_Authors;

                if (!string.IsNullOrEmpty(InternetSearch_ArticleTitle))
                    accessor.ArticleTitle = InternetSearch_ArticleTitle;

                if (!string.IsNullOrEmpty(InternetSearch_MaterialTitle))
                    accessor.MaterialTitle = InternetSearch_MaterialTitle;

                // ※ 検索項目にない。
                //if (!string.IsNullOrEmpty(InternetSearch_MaterialVolume))
                //{
                //    if (!int.TryParse(InternetSearch_MaterialVolume, out int _))
                //        throw new InvalidDataException("「巻」を正しい数値形式で入力してください。\r\nもしくは空欄にしてください。");
                //    accessor.MaterialVolume = InternetSearch_MaterialVolume;
                //}

                if (!int.TryParse(InternetSearch_DataMaxCount, out int dataMaxCount))
                    throw new InvalidDataException("「最大データ数」を正しい数値形式で入力してください。");
                accessor.RecordCount = dataMaxCount;

                if (!string.IsNullOrEmpty(InternetSearch_PublishedFrom))
                {
                    if (!int.TryParse(InternetSearch_PublishedFrom, out int publishedFrom))
                        throw new InvalidDataException("「何年から」を正しい数値形式で入力してください。\r\nもしくは空欄にしてください。");
                    accessor.PublishedFrom = publishedFrom;
                }

                if (!string.IsNullOrEmpty(InternetSearch_PublishedUntil))
                {
                    if (!int.TryParse(InternetSearch_PublishedUntil, out int publishedUntil))
                        throw new InvalidDataException("「何年まで」を正しい数値形式で入力してください。\r\nもしくは空欄にしてください。");
                    accessor.PublishedUntil = publishedUntil;
                }

                // ※ キーワードをフリーワードに入れてる。
                if (!string.IsNullOrEmpty(InternetSearch_Keywords))
                    accessor.FreeWord = InternetSearch_Keywords;

            }

            await InternetSearchPullProcess(accessor);
            var saveTask = Save();
            var successAnimationTask = Task.Run(async () =>
            {
                IsInternetSearch_CiNii_B_Done = true;
                await Task.Delay(2222);
                if (!IsInternetSearch_CiNii_B_Busy)
                    IsInternetSearch_CiNii_B_Done = false;
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"失敗しました。\r\nﾒｯｾｰｼﾞ: {ex.Message}", "CiNiiで検索", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsInternetSearch_CiNii_B_Busy = false;
        }
    }
    async Task InternetSearch_NDLSearch_B_()
    {
        if (IsInternetSearch_NDLSearch_B_Busy)
            return;

        try
        {
            IsInternetSearch_NDLSearch_B_Busy = true;

            var accessor = new NDLSearch_Main_APIAccessor();
            {
                //accessor.ISSN = ISSN.Architecture_Structure;

                if (!string.IsNullOrEmpty(InternetSearch_Authors))
                    accessor.AuthorName = InternetSearch_Authors;

                if (!string.IsNullOrEmpty(InternetSearch_ArticleTitle))
                    accessor.ArticleTitle = InternetSearch_ArticleTitle;

                // ※ 検索項目にない。
                //if (!string.IsNullOrEmpty(InternetSearch_MaterialTitle))
                //    accessor.MaterialTitle = InternetSearch_MaterialTitle;

                // ※ 検索項目にない。
                //if (!string.IsNullOrEmpty(InternetSearch_MaterialVolume))
                //{
                //    if (!int.TryParse(InternetSearch_MaterialVolume, out int _))
                //        throw new InvalidDataException("「巻」を正しい数値形式で入力してください。\r\nもしくは空欄にしてください。");
                //    accessor.MaterialVolume = InternetSearch_MaterialVolume;
                //}

                if (!int.TryParse(InternetSearch_DataMaxCount, out int dataMaxCount))
                    throw new InvalidDataException("「最大データ数」を正しい数値形式で入力してください。");
                accessor.RecordCount = dataMaxCount;

                if (!string.IsNullOrEmpty(InternetSearch_PublishedFrom))
                {
                    if (!int.TryParse(InternetSearch_PublishedFrom, out int publishedFrom))
                        throw new InvalidDataException("「何年から」を正しい数値形式で入力してください。\r\nもしくは空欄にしてください。");
                    accessor.PublishedFrom = publishedFrom;
                }

                if (!string.IsNullOrEmpty(InternetSearch_PublishedUntil))
                {
                    if (!int.TryParse(InternetSearch_PublishedUntil, out int publishedUntil))
                        throw new InvalidDataException("「何年まで」を正しい数値形式で入力してください。\r\nもしくは空欄にしてください。");
                    accessor.PublishedUntil = publishedUntil;
                }

                // ※ キーワードをフリーワードに入れてる。
                if (!string.IsNullOrEmpty(InternetSearch_Keywords))
                    accessor.FreeWord = InternetSearch_Keywords;

            }

            await InternetSearchPullProcess(accessor);
            var saveTask = Save();
            var successAnimationTask = Task.Run(async () =>
            {
                IsInternetSearch_NDLSearch_B_Done = true;
                await Task.Delay(2222);
                if (!IsInternetSearch_NDLSearch_B_Busy)
                    IsInternetSearch_NDLSearch_B_Done = false;
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"失敗しました。\r\nﾒｯｾｰｼﾞ: {ex.Message}", "NDLSearchで検索", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsInternetSearch_NDLSearch_B_Busy = false;
        }
    }

    async Task InternetSearch_JStage_C_()
    {
        if (IsInternetSearch_JStage_C_Busy)
            return;

        try
        {
            IsInternetSearch_JStage_C_Busy = true;

            var accessor = new JStage_DOI_ArticleAPIAccessor();
            {
                if (string.IsNullOrEmpty(InternetSearch_DOI))
                    throw new InvalidDataException("DOIを埋めてください。");
                accessor.DOI = InternetSearch_DOI;
            }

            await InternetSearchPullProcess(accessor);
            var saveTask = Save();
            var successAnimationTask = Task.Run(async () =>
            {
                IsInternetSearch_JStage_C_Done = true;
                await Task.Delay(2222);
                if (!IsInternetSearch_JStage_C_Busy)
                    IsInternetSearch_JStage_C_Done = false;
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"失敗しました。\r\nﾒｯｾｰｼﾞ: {ex.Message}", "J-Stageで検索", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsInternetSearch_JStage_C_Busy = false;
        }
    }
    async Task InternetSearch_CiNii_C_()
    {
        if (IsInternetSearch_CiNii_C_Busy)
            return;

        try
        {
            IsInternetSearch_CiNii_C_Busy = true;

            var accessor = new CiNii_Main_ArticleAPIAccessor();
            {
                if (string.IsNullOrEmpty(InternetSearch_DOI))
                    throw new InvalidDataException("DOIを埋めてください。");
                accessor.DOI = InternetSearch_DOI;
            }

            await InternetSearchPullProcess(accessor);
            var saveTask = Save();
            var successAnimationTask = Task.Run(async () =>
            {
                IsInternetSearch_CiNii_C_Done = true;
                await Task.Delay(2222);
                if (!IsInternetSearch_CiNii_C_Busy)
                    IsInternetSearch_CiNii_C_Done = false;
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"失敗しました。\r\nﾒｯｾｰｼﾞ: {ex.Message}", "CiNiiで検索", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsInternetSearch_CiNii_C_Busy = false;
        }
    }
    async Task InternetSearch_CrossRef_C_()
    {
        if (IsInternetSearch_CrossRef_C_Busy)
            return;

        try
        {
            IsInternetSearch_CrossRef_C_Busy = true;

            var accessor = new CrossRef_DOI_APIAccessor();
            {
                if (string.IsNullOrEmpty(InternetSearch_DOI))
                    throw new InvalidDataException("DOIを埋めてください。");
                accessor.DOI = InternetSearch_DOI;
            }

            await InternetSearchPullProcess(accessor);
            var saveTask = Save();
            var successAnimationTask = Task.Run(async () =>
            {
                IsInternetSearch_CrossRef_C_Done = true;
                await Task.Delay(2222);
                if (!IsInternetSearch_CrossRef_C_Busy)
                    IsInternetSearch_CrossRef_C_Done = false;
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"失敗しました。\r\nﾒｯｾｰｼﾞ: {ex.Message}", "CrossRefで検索", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsInternetSearch_CrossRef_C_Busy = false;
        }
    }

    async Task InternetSearchPullProcess(IResearchAPIAccessor accessor)
    {
        // プルして再描画した後に，一時データになってるやつだけ右上に固めるように動かす。
        var pulledArticles = await ResearchArticlesManager.PullArticleInfo(accessor, asTempArticles: true, save: false);
        InternetSearchPostProcess(pulledArticles, true, null);
    }
    void InternetSearchPostProcess(List<ResearchArticle>? pulledArticles, bool moveToFirstItem = true, Action<List<DefaultNodeViewModel>>? rearrangeNodesActionOverwrite = null)
    {
        if (pulledArticles is null || pulledArticles.Count == 0)
            throw new Exception("マッチするデータがありませんでした。");

        RedrawResearchArticleNodes();

        var pulledTempArticles = pulledArticles
            .Where(n => n.Private_Temporary ?? false)
            .ToList();

        var pulledTempArticleNodes = GetNodesFromArticles(pulledTempArticles!)
            ?.Cast<DefaultNodeViewModel>()
            ?.ToList();

        if (rearrangeNodesActionOverwrite is not null)
            rearrangeNodesActionOverwrite(pulledTempArticleNodes!);
        else
            RearrangeNodesToEdge(pulledTempArticleNodes, false, true);

        if (moveToFirstItem)
            MoveCanvasToTargetArticleNode(pulledTempArticleNodes!.FirstOrDefault());

        SelectedEmphasizePropertyItem = ViewModels.EmphasizePropertyItems.一時ﾃﾞｰﾀ;
    }


    // ★★★★★★★★★★★★★★★ 右パネル内 → 設定

    void InsertToFormat(object a)
    {
        // ボタン押したらフォーマット挿入！


    }


    // ★★★★★★★★★★★★★★★ not using

    void Memos()
    {
        // コピペ用に残しとく
        MessageBox.Show("NotImplemented");

        // void MoveTestNodes()
        {
            if (_NodeLinkViewModels.Any())
                _NodeViewModels[0].Position = new Point(0, 0);
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

    void NodesMoved(EndMoveNodesOperationEventArgs param)
    {
    }


    // ★★★★★★★★★★★★★★★ auto

    void PreviewConnect(PreviewConnectLinkOperationEventArgs args)
    {
        var inputNode = NodeViewModels.First(arg => arg.Guid == args.ConnectToEndNodeGuid);
        var inputConnector = inputNode.FindConnector(args.ConnectToEndConnectorGuid);
        //args.CanConnect = inputConnector.Label == "Limited Input" == false;
    }

    // ★★★★★★★★★★★★★★★ utils

    ResearchArticleNodeViewModel? GetNodeFromArticle(ResearchArticle? targetArticle)
    {
        if (targetArticle is null)
            return null;

        var targetArticleNode = _NodeViewModels
            .Where(n => n is ResearchArticleNodeViewModel)
            .Cast<ResearchArticleNodeViewModel>()
            .FirstOrDefault(n => n.Article == targetArticle);

        return targetArticleNode;
    }
    List<ResearchArticleNodeViewModel> GetNodesFromArticles(List<ResearchArticle> targetArticles)
    {
        var targetArticleNodes = _NodeViewModels
            .Where(n => n is ResearchArticleNodeViewModel)
            .Cast<ResearchArticleNodeViewModel>()
            .Where(n => targetArticles.Contains(n.Article))
            .ToList()
            ;

        return targetArticleNodes;
    }

    void MoveCanvasToTargetArticle(ResearchArticle? targetArticle)
    {
        var targetArticleNode = GetNodeFromArticle(targetArticle);
        MoveCanvasToTargetArticleNode(targetArticleNode);
    }
    void MoveCanvasToTargetArticleNode(DefaultNodeViewModel? targetNode)
    {
        if (targetNode == null)
            return;

        var targetArticleNodePosition = targetNode.Position;

        MoveCanvasToPosition(targetArticleNodePosition.X, targetArticleNodePosition.Y);
    }
    void MoveCanvasToPosition(double x = 0, double y = 0)
    {
        CanvasOffset = new Point(-x, -y);
    }


    // ★★★★★★★★★★★★★★★ 

}
