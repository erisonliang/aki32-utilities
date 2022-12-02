using Aki32_Utilities.General;

using ClosedXML;

namespace Aki32_Utilities.SpecificPurposeModels.Research;
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
    /// <param name="mergedArticle">Article that will be eventually remained</param>
    /// <param name="mergingArticle">Article that will be eventually deleted</param>
    public static void MergeArticles(this List<ResearchArticle> articles, ResearchArticle mergedArticle, ResearchArticle mergingArticle)
    {
        // prerocess
        if (!articles.Contains(mergedArticle))
            throw new InvalidDataException("{mergedArticle} and {mergingArticle} has to be in {articles}");


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
                    prop.SetValue(mergedArticle, addingArticleInfoProp);

            }
            else
            {
                if (addingArticleInfoProp != null)
                    prop.SetValue(mergedArticle, addingArticleInfoProp);

            }

        }


        //delete

        if (articles.Contains(mergingArticle))
            articles.Remove(mergingArticle);

    }


    // ★★★★★★★★★★★★★★★ 

}
