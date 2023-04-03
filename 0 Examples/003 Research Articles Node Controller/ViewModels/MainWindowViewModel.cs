﻿using Livet;
using System.Windows;
using Aki32Utilities.ViewModels.NodeViewModels;
using Aki32Utilities.ConsoleAppUtilities.Research;
using Aki32Utilities.ConsoleAppUtilities.General;
using System.IO;

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
            UtilConfig.ReadEnvConfig(new FileInfo(@"C:\Users\aki32\Dropbox\PC\★ PC間共有\MyEnvConfig.json"));
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
        _NodeLinkViewModels.Clear();
        _NodeViewModels.Clear();

        foreach (var article in ResearchArticlesManager.ArticleDatabase)
            _NodeViewModels.Add(new ResearchArticleNodeViewModel() { NodeName = "ResearchArticle", Article = article, Position = new Point(0, 0) });

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

        RearrangeNodesAlignLeft();
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