using Aki32Utilities.ConsoleAppUtilities.General;

using ClosedXML;

namespace Aki32Utilities.ConsoleAppUtilities.SpecificPurposeModels.Research;
public static class ResearchArticleExtension
{

    // ★★★★★★★★★★★★★★★ method

    /// <summary>
    /// 
    /// Merge two ResearchArticle instances.
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// 後からの情報が優先（最新）
    /// 
    /// </remarks>
    /// <param name="articles">Article List</param>
    /// <param name="baseArticle">Article that will be eventually remained</param>
    /// <param name="mergingArticle">Article that will be eventually deleted</param>
    public static void MergeArticles(this List<ResearchArticle> articles, ResearchArticle baseArticle, ResearchArticle mergingArticle)
    {
        // prerocess
        if (!articles.Contains(baseArticle))
            throw new InvalidDataException("{mergedArticle} need to be in {articles}");


        // main
        var props = typeof(ResearchArticle)
            .GetProperties()
            .Where(p => !p.HasAttribute<CsvIgnoreAttribute>())
            .Where(p => p.CanWrite)
            ;


        // 後からの情報優先で上書き。
        foreach (var prop in props)
        {
            var addingArticleInfoProp = prop.GetValue(mergingArticle);


            if (prop.PropertyType == typeof(string))
            {
                if (addingArticleInfoProp?.ToString().NullIfNullOrEmpty() != null)
                    prop.SetValue(baseArticle, addingArticleInfoProp);

            }
            else
            {
                if (addingArticleInfoProp != null)
                    prop.SetValue(baseArticle, addingArticleInfoProp);

            }

        }

        //delete
        if (articles.Contains(mergingArticle))
        {
            // TODO: Ref整合性。消す場合はRefから消したい。




            articles.Remove(mergingArticle);
        }

    }


    // ★★★★★★★★★★★★★★★ 

}
