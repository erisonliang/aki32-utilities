using Livet;
using System.Windows;
using Aki32Utilities.ViewModels.NodeViewModels;
using Aki32Utilities.ConsoleAppUtilities.Research;
using Aki32Utilities.ConsoleAppUtilities.General;
using System.IO;
using System.Diagnostics;
using System.Collections.ObjectModel;

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

    }

    private void InitResearchArticlesManager()
    {
        ResearchArticlesManager = new ResearchArticlesManager(databaseDir);
        ResearchArticlesManager.OpenDatabase();
        RedrawResearchArticlesManager();
    }

    private void RedrawResearchArticlesManager()
    {
        var starttime = DateTime.Now;

        _NodeLinkViewModels.Clear();
        _NodeViewModels.Clear();

        Debug.WriteLine(DateTime.Now - starttime);

        // nodes 
        {
            // all
            {
                //foreach (var article in ResearchArticlesManager.ArticleDatabase)
                //    _NodeViewModels.Add(new ResearchArticleNodeViewModel() { NodeName = "ResearchArticle", Article = article, Position = new Point(0, 0) });

                ////var nodeViewModels = ResearchArticlesManager
                ////    .ArticleDatabase
                ////    .Select(article => new ResearchArticleNodeViewModel() { NodeName = "ResearchArticle", Article = article, Position = new Point(0, 0) })
                ////    .ToArray();

                ////_NodeViewModels.AddRange(
                ////    ResearchArticlesManager
                ////    .ArticleDatabase
                ////    .Select(article => new ResearchArticleNodeViewModel() { NodeName = "ResearchArticle", Article = article, Position = new Point(0, 0) })
                ////    );
            }

            // only changed nodes
            {
                // TODO
                throw new NotImplementedException();

                // データベースに存在しないのに存在してるノードを全削除




                // データベースに存在してるのにノードが存在してない場合，全追加





            }
        }


        Debug.WriteLine(DateTime.Now - starttime);

        // all links
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

        //_NodeViewModels.AddRange(nodeViewModels);

        Debug.WriteLine(DateTime.Now - starttime);

        RearrangeNodesAlignLeft();
        ParentView?.UpdateLayout();

        Debug.WriteLine(DateTime.Now - starttime);

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