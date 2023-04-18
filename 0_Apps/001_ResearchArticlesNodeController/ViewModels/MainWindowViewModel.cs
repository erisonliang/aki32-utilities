using Livet;
using System.Windows;
using Aki32Utilities.ViewModels.NodeViewModels;
using Aki32Utilities.ConsoleAppUtilities.Research;
using Aki32Utilities.ConsoleAppUtilities.General;
using System.IO;

namespace Aki32Utilities.Apps.ResearchArticlesNodeController.ViewModels;
public partial class MainWindowViewModel : ViewModel
{

    // ★★★★★★★★★★★★★★★ inits

    public MainWindowViewModel()
    {
        IsLockedAllNodeLinks = true;
        IsEnableAllNodeConnectors = true;
        ExecuteDynamicLinkAttractions = false;
        ExecuteDynamicNodeRepulsions = false;

        try
        {
            // Set API keys
            UtilConfig.ReadEnvConfig(new FileInfo(@"C:\Users\aki32\Dropbox\Codes\# SharedData\MyEnvConfig.json"));
        }
        catch (Exception)
        {
        }
    }

    internal void InitResearchArticlesManager()
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

            Console.WriteLine($"Nodes UI updated (added {diffs.Count}, removed {removed})");
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
    デフォルト,
    なし,
    お気に入り,
    既読,
    積読,
    検索結果,
    グループ,
    ウェブ,
    一時データ,
    メモ1,
    メモ2,
    メモ3,
}