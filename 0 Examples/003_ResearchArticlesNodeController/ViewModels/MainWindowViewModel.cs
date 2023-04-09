using Livet;
using System.Windows;
using Aki32Utilities.ViewModels.NodeViewModels;
using Aki32Utilities.ConsoleAppUtilities.Research;
using Aki32Utilities.ConsoleAppUtilities.General;
using System.IO;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Microsoft.VisualBasic;
using System.Windows.Threading;
using Aki32Utilities.WPFAppUtilities.NodeController.Extensions;
using Org.BouncyCastle.Asn1.Esf;

namespace Aki32Utilities.UsageExamples.ResearchArticlesNodeController.ViewModels;
public partial class MainWindowViewModel : ViewModel
{

    // ★★★★★★★★★★★★★★★ inits

    public MainWindowViewModel()
    {
        InitResearchArticlesManager();

        IsLockedAllNodeLinks = true;
        IsEnableAllNodeConnectors = true;

        try
        {
            // Set API keys
            UtilConfig.ReadEnvConfig(new FileInfo(@"C:\Users\aki32\Dropbox\PC\★ PC間共有\Programming\MyEnvConfig.json"));
        }
        catch (Exception)
        {
        }

        DynamicNodeDistancingTimer.Elapsed += DynamicNodeDistancingTimer_Elapsed;
        DynamicNodeDistancingTimer.Start();
    }

    private void DynamicNodeDistancingTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        try
        {
            if (!IsDynamicNodeDistancingAvailable)
                return;

            var w_global_Links = 1;
            var w_global_OverWrap = 5;
            var w_global_NodeRadius = 1.1;


            _NodeViewModels.ForEach(node =>
            {
                if (node.IsSelected)
                    return;

                // ★★★★★ ノード情報抽出

                // ★★★★★ 
                // TODO
                var addX = 0d;
                var addY = 0d;


                // ★★★★★ 相乗効果
                foreach (var otherNode in _NodeViewModels)
                {
                    // ★ 前処理
                    if (otherNode == node)
                        continue;


                    // ★ 引用関係がつながってたら，引き合う。
                    // TODO
                    addX += 0;
                    addY += 0;



                    // ★ 重なってたら，外の方向に力をかける。
                    // 重なりを精密に計算！矩形が重なってたら，重心同士を反発させる。
                    // 同心円状に遅くなる性質上，横方向の動きが遅い。でも仕方ないか。
                    var nodeV = node.Center.Sub(otherNode.Center).ToVector();
                    var threX = (node.Width + otherNode.Width) / 2;
                    var threY = (node.Height + otherNode.Height) / 2;
                    var overWrapX = threX - Math.Abs(nodeV.X) / w_global_NodeRadius;
                    var overWrapY = threY - Math.Abs(nodeV.Y) / w_global_NodeRadius;
                    var dir = nodeV.NormalizeTo();
                    if (overWrapX > 0 && overWrapY > 0)
                    {
                        var threR = node.Radius + otherNode.Radius;
                        var overWrap = threR - nodeV.Length / w_global_NodeRadius;
                        if (overWrapX > 0)
                            addX += dir.X * overWrap / threR * w_global_OverWrap;
                        if (overWrapY > 0)
                            addY += dir.Y * overWrap / threR * w_global_OverWrap;
                    }

                    
                    // ★

                }

                // ★★★★★ 定数動かす。
                // TODO
                addX += 0;
                addY += 0;


                // ★★★★★ 速度をメモしておいて，加速を一定以上にしないように制御。
                // TODO
                addX += 0;
                addY += 0;


                // ★★★★★ 上書き！
                node.Position = new Point(node.Position.X + addX, node.Position.Y + addY);

            });


            //ParentView.Dispatcher.Invoke(() =>
            //{
            //    //Console.WriteLine(DateTime.Now);
            //});
        }
        catch (Exception ex)
        {
        }
    }

    private void InitResearchArticlesManager()
    {
        ResearchArticlesManager = new ResearchArticlesManager(databaseDir);
        ResearchArticlesManager.OpenDatabase();
        RedrawResearchArticleNodes();
        RearrangeNodesAlignLeft();
    }

    private void RedrawResearchArticleNodes()
    {
        _NodeLinkViewModels.Clear();

        // ★★★★★ nodes 
        {
            // ★ all
            //_NodeViewModels.Clear();
            //foreach (var article in ResearchArticlesManager.ArticleDatabase)
            //    _NodeViewModels.Add(new ResearchArticleNodeViewModel() { NodeName = "文献", Article = article, Position = new Point(0, 0) });


            // ★ only changed nodes
            // データベースに存在しないのに存在してるノードを全削除
            var removed = 0;
            for (int i = _NodeViewModels.Count - 1; i >= 0; i--)
                if (_NodeViewModels[i] is ResearchArticleNodeViewModel node)
                    if (!ResearchArticlesManager.ArticleDatabase.Contains(node.Article))
                    {
                        _NodeViewModels.RemoveAt(i);
                        removed++;
                    }

            // データベースに存在してるのにノードが存在してない場合，全追加
            var nodeArticles = _NodeViewModels
                .Where(n => n is ResearchArticleNodeViewModel)
                .Cast<ResearchArticleNodeViewModel>()
                .Select(x => x.Article);

            var diffs = ResearchArticlesManager.ArticleDatabase.Except(nodeArticles).ToList();

            foreach (var addingArticle in diffs)
                _NodeViewModels.Add(new ResearchArticleNodeViewModel() { NodeName = "文献", Article = addingArticle, Position = new Point(0, 0) });

            Console.WriteLine($"nodes updated: added {diffs.Count}, removed {removed}");
        }

        // ★★★★★ all links
        foreach (var article in ResearchArticlesManager.ArticleDatabase)
        {
            if (article.ReferenceAOIs == null || !article.ReferenceAOIs.Any())
                continue;

            var articleNode = _NodeViewModels.FirstOrDefault(x => x is ResearchArticleNodeViewModel ran && ran.Article == article);
            if (articleNode == null)
                continue;

            foreach (var referenceAOI in article.ReferenceAOIs)
            {
                var referenceArticle = ResearchArticlesManager.ArticleDatabase.FirstOrDefault(a => a.AOI == referenceAOI);
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

    }


    // ★★★★★★★★★★★★★★★ 

}

public enum GroupIntersectType
{
    CursorPointVMDefine,
    BoundingBoxVMDefine,
}

public enum RangeSelectionMode
{
    包含,
    接触,
}

public enum EmphasizePropertyItems
{
    なし,
    お気に入り,
    既読,
    検索結果,
    一時ﾃﾞｰﾀ,
    ﾒﾓ1,
    ﾒﾓ2,
    ﾒﾓ3,
}