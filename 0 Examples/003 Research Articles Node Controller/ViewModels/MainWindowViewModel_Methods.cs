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

namespace Aki32Utilities.UsageExamples.ResearchArticlesNodeController.ViewModels;
public partial class MainWindowViewModel : ViewModel
{

    // ★★★★★★★★★★★★★★★ general

    void NotifyResearchArticlesPropertiesChanged()
    {
        var articleNodes = _NodeViewModels.Select(n => (n is ResearchArticleNodeViewModel run) ? run : null).Where(run => run != null);
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
            MessageBox.Show($"失敗しました。\r\nﾒｯｾｰｼﾞ: {ex.Message}", "保存", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsSaveBusy = false;
        }

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

        var offsetPosOnCanvasX = posOnCanvas.X - Offset.X - posOnContextMenu.X / Scale;
        var offsetPosOnCanvasY = posOnCanvas.Y - Offset.Y - posOnContextMenu.Y / Scale;
        var addingPosition = new Point(offsetPosOnCanvasX, offsetPosOnCanvasY);

        AddNewArticleNode(addingPosition);
    }
    void AddNewArticleNode(Point addingPosition)
    {
        // 作って，新しいNodeも作ってあてがう。
        var newArticle = ResearchArticle.CreateManually(new ResearchArticle_ManualInitInfo
        {
            Manual_ArticleTitle = "■ 未入力",
            Manual_Authors = new string[] { "■ 未入力" },
            Manual_PublishedDate = "■ 未入力",
            Memo = "■ 未入力",
        });

        ResearchArticlesManager.MergeArticleInfo(new List<ResearchArticle> { newArticle }, forceAdd: true, save: false);
        var addingNode = new ResearchArticleNodeViewModel() { Article = newArticle, NodeName = "ResearchArticle" };
        _NodeViewModels.Add(addingNode);
        addingNode.Position = addingPosition;
        var saveTask = Save();
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


    // ★★★★★★★★★★★★★★★ Node Controller 内

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

        if (SelectingNodeViewModel is not null)
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

    async Task PullCrossRefInfo()
    {
        if (IsPullCrossRefInfoBusy)
            return;

        try
        {
            IsPullCrossRefInfoBusy = true;

            if (SelectingNodeViewModel is null)
                throw new Exception("文献が選択されていません。");

            // crossref
            if (string.IsNullOrEmpty(SelectingNodeViewModel.Article.DOI))
                throw new Exception("DOIがない文献はこの機能を使うことができません。");

            var accessor = new CrossRef_DOI_APIAccessor()
            {
                DOI = SelectingNodeViewModel.Article.DOI!,
            };

            var pulledArticles = new List<ResearchArticle>();
            try
            {
                pulledArticles = await ResearchArticlesManager.PullArticleInfo(accessor, asTempArticles: true, save: false);
                if (pulledArticles.Count == 0)
                    throw new Exception("マッチするデータがありませんでした。");
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex}\r\nCrossRef側がこの文献に対応していない可能性があります。");
            }

            // 全てに対して GPT予測！？？
            var pulledCount = pulledArticles.Count;
            var result_UseGPT = MessageBox.Show($"取得に成功した {pulledCount} 件の文献に対して，AIによるメタ情報推定を行いますか？", "引用関係を取得", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            if (result_UseGPT == MessageBoxResult.OK)
            {
                var failCount = 0;

                foreach (var pulledArticle in pulledArticles)
                {
                    try
                    {
                        var result = await pulledArticle.TryPredictMetaInfo_ChatGPT();
                        if (!result)
                            throw new Exception("推測に失敗しました。");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"失敗しました。\r\nﾒｯｾｰｼﾞ: {ex.Message}", "メタ情報を推測", MessageBoxButton.OK, MessageBoxImage.Error);
                        failCount++;
                    }
                }

                Console.WriteLine($"★ メタ情報の収集が完了しました。(成功: {pulledCount - failCount} / {pulledCount})");
            }

            RedrawResearchArticlesManager();
            MoveCanvasToTargetArticle(SelectingNodeViewModel);
            SelectedEmphasizePropertyItem = ViewModels.EmphasizePropertyItems.一時ﾃﾞｰﾀ;
            var saveTask = Save();

            var successAnimationTask = Task.Run(async () =>
            {
                IsPullCrossRefInfoDone = true;
                await Task.Delay(2222);
                if (!IsPullCrossRefInfoBusy)
                    IsPullCrossRefInfoDone = false;
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"失敗しました。\r\nﾒｯｾｰｼﾞ: {ex.Message}", "引用関係を取得", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsPullCrossRefInfoBusy = false;
        }
    }

    async Task PullNormalMetaInfo()
    {
        if (IsPullNormalMetaInfoBusy)
            return;

        try
        {
            IsPullNormalMetaInfoBusy = true;

            if (SelectingNodeViewModel is null)
                throw new Exception("文献が選択されていません。");

            // 対象のサイトからの情報
            // TODO
            throw new NotImplementedException("申し訳ありません。\r\n未実装です…。");
            {
                var accessor = new CrossRef_DOI_APIAccessor()
                {
                    DOI = SelectingNodeViewModel.Article.DOI!,
                };

                var pulledArticles = await ResearchArticlesManager.PullArticleInfo(accessor, asTempArticles: true, save: false);
                if (pulledArticles.Count == 0)
                    throw new Exception("マッチするデータがありませんでした。");
            }

            RedrawResearchArticlesManager();
            MoveCanvasToTargetArticle(SelectingNodeViewModel);
            SelectedEmphasizePropertyItem = ViewModels.EmphasizePropertyItems.一時ﾃﾞｰﾀ;
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
        }
    }

    async Task PullAIMetaInfo()
    {
        if (IsPullAIMetaInfoBusy)
            return;

        try
        {
            IsPullAIMetaInfoBusy = true;

            if (SelectingNodeViewModel is null)
                throw new Exception("文献が選択されていません。");

            // ChatGPTによる推定
            var result = await SelectingNodeViewModel.Article.TryPredictMetaInfo_ChatGPT();
            if (!result)
                throw new Exception("推測に失敗しました。");

            SelectingNodeViewModel.NotifyArticleUpdated();
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
        }
    }

    async Task OpenPDF()
    {
        if (IsOpenPDFBusy)
            return;

        try
        {
            IsOpenPDFBusy = true;

            if (SelectingNodeViewModel is null)
                throw new Exception("文献が選択されていません。");

            var result = await SelectingNodeViewModel.Article.TryOpenPDF(ResearchArticlesManager.PDFsDirectory);
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
        }
    }

    void OpenDOIWebSite()
    {
        if (IsOpenDOIWebSiteBusy)
            return;

        try
        {
            IsOpenDOIWebSiteBusy = true;

            if (SelectingNodeViewModel is null)
                throw new Exception("文献が選択されていません。");

            var result = SelectingNodeViewModel.Article.TryOpenDOILink();
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
        }
    }

    void ExecuteAll()
    {
        if (IsExecuteAllBusy)
            return;

        try
        {
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
        }
    }

    void AISummary()
    {
        if (IsAISummaryBusy)
            return;

        try
        {
            IsAISummaryBusy = true;

            throw new NotImplementedException("申し訳ありません。\r\n未実装です…。");

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
        }
    }

    void ManuallyAddPDF()
    {
        if (IsManuallyAddPDFBusy)
            return;

        try
        {
            IsManuallyAddPDFBusy = true;

            if (SelectingNodeViewModel is null)
                throw new Exception("文献が選択されていません。");

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
        }
    }
    public void Button_ManuallyAddPDF_Drop(object sender, DragEventArgs e)
    {
        if (IsManuallyAddPDFBusy)
            return;

        try
        {
            IsManuallyAddPDFBusy = true;

            if (SelectingNodeViewModel is null)
                throw new Exception("文献が選択されていません。");

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
        }
    }
    private void ManuallyAddPDF_FromFileInfo(FileInfo addingPDFFile)
    {
        //
        if (SelectingNodeViewModel!.Article.TryFindPDF(ResearchArticlesManager.PDFsDirectory))
        {
            var result_ForceOverwrite = MessageBox.Show($"この文献には既にPDFが存在していて，上書きしようとしています。\r\n本当によろしいですか？", "PDF手動追加", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result_ForceOverwrite is not MessageBoxResult.OK)
                return;
        }

        //
        var result_DeleteOriginal = MessageBox.Show($"コピー元のPDFを削除しますか？", "PDF手動追加", MessageBoxButton.OKCancel, MessageBoxImage.Information);
        var deleteOriginalPdfFile = result_DeleteOriginal is MessageBoxResult.OK;

        //
        SelectingNodeViewModel.Article.AddPDFManually(ResearchArticlesManager.PDFsDirectory,
            addingPDFFile, deleteOriginalPdfFile,
            forceOverwriteExistingPDF: true);

        //
        MessageBox.Show($"追加に成功しました。", "PDF手動追加", MessageBoxButton.OK, MessageBoxImage.Information);



    }

    async Task UndefinedButton1()
    {
        if (IsUndefinedButton1Busy)
            return;

        try
        {
            IsUndefinedButton1Busy = true;

            await Task.Delay(2222);
            //throw new NotImplementedException("申し訳ありません。\r\n未実装です…。");

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
            //MessageBox.Show($"失敗しました。\r\nﾒｯｾｰｼﾞ: {ex.Message}", "AIにまとめてもらう", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsUndefinedButton1Busy = false;
        }
    }


    // ★★★★★★★★★★★★★★★ 右パネル内 → 検索

    void aaa()
    {

        ////// ★ articles from cinii
        ////var accessor = new CiNiiArticleAPIAccessor()
        ////{
        ////    RecordCount = 5,
        ////    ISSN = ISSN.Architecture_Structure,
        ////    SearchFreeWord = "小振幅"
        ////};
        ////research.PullArticleInfo(accessor);


        ////// ★ article from crossref
        ////var accessor = new CrossRefAPIAccessor()
        ////{
        ////    DOI = "10.3130/aijs.87.822"
        ////};
        ////research.PullArticleInfo(accessor);


        ////// ★ articles from ndl search
        ////var accessor = new NDLSearchAPIAccessor()
        ////{
        ////    RecordCount = 5,
        ////    SearchFreeWord = "空間情報を表現するグラフ構造",
        ////};
        ////research.PullArticleInfo(accessor);

    }

    async Task OpenLocalSearch()
    {
        ParentView.TabControl_RightPanel.SelectedItem = ParentView.TabItem_Search;
        //ParentView.TextBox_LocalSearch.Text = "";
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

            var pulledArticles = await ResearchArticlesManager.PullArticleInfo(accessor, asTempArticles: true, save: false);
            if (pulledArticles.Count == 0)
                throw new Exception("マッチするデータがありませんでした。");

            RedrawResearchArticlesManager();
            MoveCanvasToTargetArticle(pulledArticles.FirstOrDefault());
            SelectedEmphasizePropertyItem = ViewModels.EmphasizePropertyItems.一時ﾃﾞｰﾀ;
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

            var pulledArticles = await ResearchArticlesManager.PullArticleInfo(accessor, asTempArticles: true, save: false);
            if (pulledArticles.Count == 0)
                throw new Exception("マッチするデータがありませんでした。");

            RedrawResearchArticlesManager();
            MoveCanvasToTargetArticle(pulledArticles.FirstOrDefault());
            SelectedEmphasizePropertyItem = ViewModels.EmphasizePropertyItems.一時ﾃﾞｰﾀ;
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

    async Task InternetSearchJStage_B_()
    {
        if (IsInternetSearchJStage_B_Busy)
            return;

        try
        {
            IsInternetSearchJStage_B_Busy = true;

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

            var pulledArticles = await ResearchArticlesManager.PullArticleInfo(accessor, asTempArticles: true, save: false);
            if (pulledArticles.Count == 0)
                throw new Exception("マッチするデータがありませんでした。");

            RedrawResearchArticlesManager();
            MoveCanvasToTargetArticle(pulledArticles.FirstOrDefault());
            SelectedEmphasizePropertyItem = ViewModels.EmphasizePropertyItems.一時ﾃﾞｰﾀ;
            var saveTask = Save();

            var successAnimationTask = Task.Run(async () =>
            {
                IsInternetSearchJStage_B_Done = true;
                await Task.Delay(2222);
                if (!IsInternetSearchJStage_B_Busy)
                    IsInternetSearchJStage_B_Done = false;
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"失敗しました。\r\nﾒｯｾｰｼﾞ: {ex.Message}", "J-Stageで検索", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsInternetSearchJStage_B_Busy = false;
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

            var pulledArticles = await ResearchArticlesManager.PullArticleInfo(accessor, asTempArticles: true, save: false);
            if (pulledArticles.Count == 0)
                throw new Exception("マッチするデータがありませんでした。");

            RedrawResearchArticlesManager();
            MoveCanvasToTargetArticle(pulledArticles.FirstOrDefault());
            SelectedEmphasizePropertyItem = ViewModels.EmphasizePropertyItems.一時ﾃﾞｰﾀ;
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

            var pulledArticles = await ResearchArticlesManager.PullArticleInfo(accessor, asTempArticles: true, save: false);
            if (pulledArticles.Count == 0)
                throw new Exception("マッチするデータがありませんでした。");

            RedrawResearchArticlesManager();
            MoveCanvasToTargetArticle(pulledArticles.FirstOrDefault());
            SelectedEmphasizePropertyItem = ViewModels.EmphasizePropertyItems.一時ﾃﾞｰﾀ;
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

    async Task InternetSearch_CiNii_C_()
    {
        if (IsInternetSearch_CiNii_C_Busy)
            return;

        try
        {
            IsInternetSearch_CiNii_C_Busy = true;

            var accessor = new CiNii_Main_ArticleAPIAccessor();
            {
                //accessor.ISSN = ISSN.Architecture_Structure;

                if (string.IsNullOrEmpty(InternetSearch_DOI))
                    throw new InvalidDataException("DOIを埋めてください。");
                accessor.DOI = InternetSearch_DOI;

            }

            var pulledArticles = await ResearchArticlesManager.PullArticleInfo(accessor, asTempArticles: true, save: false);
            if (pulledArticles.Count == 0)
                throw new Exception("マッチするデータがありませんでした。");

            RedrawResearchArticlesManager();
            MoveCanvasToTargetArticle(pulledArticles.FirstOrDefault());
            SelectedEmphasizePropertyItem = ViewModels.EmphasizePropertyItems.一時ﾃﾞｰﾀ;
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
                //accessor.ISSN = ISSN.Architecture_Structure;

                if (string.IsNullOrEmpty(InternetSearch_DOI))
                    throw new InvalidDataException("DOIを埋めてください。");
                accessor.DOI = InternetSearch_DOI;
            }

            var pulledArticles = await ResearchArticlesManager.PullArticleInfo(accessor, asTempArticles: true, save: false);
            if (pulledArticles.Count == 0)
                throw new Exception("マッチするデータがありませんでした。");

            RedrawResearchArticlesManager();
            MoveCanvasToTargetArticle(pulledArticles.FirstOrDefault());
            SelectedEmphasizePropertyItem = ViewModels.EmphasizePropertyItems.一時ﾃﾞｰﾀ;
            var saveTask = Save();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"失敗しました。\r\nﾒｯｾｰｼﾞ: {ex.Message}", "CrossRefで検索", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsInternetSearch_CrossRef_C_Busy = false;
            IsInternetSearch_CrossRef_C_Done = true;
            await Task.Delay(2222);
            if (!IsInternetSearch_CrossRef_C_Busy)
                IsInternetSearch_CrossRef_C_Done = false;
        }
    }



    // ★★★★★★★★★★★★★★★ not using

    void Memos()
    {
        // コピペ用に残しとく
        MessageBox.Show("NotImplemented");

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

    void NodesMoved(EndMoveNodesOperationEventArgs param)
    {
    }


    // ★★★★★★★★★★★★★★★ auto


    void PreviewConnect(PreviewConnectLinkOperationEventArgs args)
    {
        var inputNode = NodeViewModels.First(arg => arg.Guid == args.ConnectToEndNodeGuid);
        var inputConnector = inputNode.FindConnector(args.ConnectToEndConnectorGuid);
        args.CanConnect = inputConnector.Label == "Limited Input" == false;
    }

    // ★★★★★★★★★★★★★★★ utils

    void MoveCanvasToTargetArticle(ResearchArticle? targetArticle)
    {
        if (targetArticle == null)
            return;

        var targetArticleNode = _NodeViewModels
            .Where(n => n is ResearchArticleNodeViewModel)
            .Cast<ResearchArticleNodeViewModel>()
            .FirstOrDefault(n => n.Article == targetArticle);

        MoveCanvasToTargetArticle(targetArticleNode);
    }
    void MoveCanvasToTargetArticle(ResearchArticleNodeViewModel? targetArticleNode)
    {
        if (targetArticleNode == null)
            return;

        var targetArticleNodePosition = targetArticleNode.Position;
        Offset = new Point(-targetArticleNodePosition.X, -targetArticleNodePosition.Y);
    }


    // ★★★★★★★★★★★★★★★ 

}
