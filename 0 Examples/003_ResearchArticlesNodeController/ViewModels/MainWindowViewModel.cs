﻿using Livet;
using System.Windows;
using Aki32Utilities.ViewModels.NodeViewModels;
using Aki32Utilities.ConsoleAppUtilities.Research;
using Aki32Utilities.ConsoleAppUtilities.General;
using System.IO;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Microsoft.VisualBasic;

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
        RedrawResearchArticleNodes();
        RearrangeNodesAlignLeft();
    }

    private void RedrawResearchArticleNodes()
    {
        _NodeLinkViewModels.Clear();

        // nodes 
        {
            // all
            {
                //_NodeViewModels.Clear();

                //foreach (var article in ResearchArticlesManager.ArticleDatabase)
                //    _NodeViewModels.Add(new ResearchArticleNodeViewModel() { NodeName = "文献", Article = article, Position = new Point(0, 0) });

                ////var nodeViewModels = ResearchArticlesManager
                ////    .ArticleDatabase
                ////    .Select(article => new ResearchArticleNodeViewModel() { NodeName = "文献", Article = article, Position = new Point(0, 0) })
                ////    .ToArray();

                ////_NodeViewModels.AddRange(
                ////    ResearchArticlesManager
                ////    .ArticleDatabase
                ////    .Select(article => new ResearchArticleNodeViewModel() { NodeName = "文献", Article = article, Position = new Point(0, 0) })
                ////    );
            }

            // only changed nodes
            {
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
        }

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