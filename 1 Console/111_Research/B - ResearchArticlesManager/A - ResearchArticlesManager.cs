using Aki32Utilities.ConsoleAppUtilities.General;

using LibGit2Sharp;

using Org.BouncyCastle.Crypto.Prng;

namespace Aki32Utilities.ConsoleAppUtilities.Research;
public partial class ResearchArticlesManager
{

    // ★★★★★★★★★★★★★★★ paths

    public DirectoryInfo LocalDirectory { get; set; }
    private FileInfo ArticleDatabaseFileInfo => LocalDirectory.GetChildFileInfo("ResearchArticles.csv");
    private FileInfo ArticleDatabaseBackUpFileInfo => new(Path.Combine(LocalDirectory.FullName, "ResearchArticles.csv.bak", $"ResearchArticles_{DateTime.Now:s}-{DateTime.Now:ffff}.csv".Replace(':', '-')));
    public DirectoryInfo PDFsDirectory => new($@"{LocalDirectory.FullName}\PDFs");



    // ★★★★★★★★★★★★★★★ props

    public List<ResearchArticle> ArticleDatabase { get; set; } = null;

    private bool articleDatabaseUpdated = false;


    // ★★★★★★★★★★★★★★★ init

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="lineAccessToken"></param>
    public ResearchArticlesManager(DirectoryInfo baseDir)
    {
        LocalDirectory = baseDir;

        Console.WriteLine("Data Powered by");
        Console.WriteLine("+ ");
        Console.WriteLine("+ Crossref (https://www.crossref.org/)");
        Console.WriteLine("+ J-STAGE (https://www.jstage.jst.go.jp/)");
        Console.WriteLine("+ CiNii (https://cir.nii.ac.jp/)");
        Console.WriteLine("+ NDL Search: (https://iss.ndl.go.jp/information/api/)");
        Console.WriteLine("+ ");
        Console.WriteLine();

        LocalDirectory.Create();
        PDFsDirectory.Create();
    }


    // ★★★★★★★★★★★★★★★ methods (data handle)

    /// <summary>
    /// Open database. If not exist, automatically create new ones.
    /// </summary>
    public void OpenDatabase()
    {
        try
        {
            ArticleDatabase = ArticleDatabaseFileInfo.ReadObjectFromLocalCsv<ResearchArticle>();
        }
        catch (FileNotFoundException)
        {
            ArticleDatabase = new List<ResearchArticle>();
        }
    }

    public void SaveDatabase(bool forceSaveArticleDatabase = true, bool withBackUp = true)
    {
        if (articleDatabaseUpdated || forceSaveArticleDatabase)
        {
            ArticleDatabase.SaveAsCsv(ArticleDatabaseFileInfo);
            if (withBackUp)
            {
                ArticleDatabaseBackUpFileInfo.Directory!.Create();
                ArticleDatabaseFileInfo.CopyTo(ArticleDatabaseBackUpFileInfo);
            }
            articleDatabaseUpdated = false;
        }
    }

    /// <summary>
    /// pull data from dataserver to local database
    /// </summary>
    /// <param name="uriBuilder"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<List<ResearchArticle>> PullArticleInfo(IResearchAPIAccessor uriBuilder, bool asTempArticles = false, bool save = true)
    {
        var fetchedArticles = await FetchArticleInfo(uriBuilder);
        var mergedArticles = MergeArticleInfo(fetchedArticles, warnMultipleMatches: false, asTempArticles: asTempArticles, save: save);
        return mergedArticles;
    }

    /// <summary>
    /// fetch data from dataserver
    /// </summary>
    /// <param name="apiAccessor"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<List<ResearchArticle>> FetchArticleInfo(IResearchAPIAccessor apiAccessor)
    {
        // preprocess
        if (ArticleDatabase == null)
            throw new InvalidOperationException("Database has not been opened yet. Consider to call OpenDataBase() first.");


        // main
        // post process
        try
        {
            return await apiAccessor.FetchArticles();
        }
        catch (Exception ex)
        {
            throw new Exception("情報のフェッチに失敗しました。検索条件を見直してください。");
        }
    }

    /// <summary>
    /// merge/add articles to local database
    /// </summary>
    /// <param name="mergingArticles"></param>
    /// <param name="forceAdd">never merge and force add all to database</param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="InvalidDataException"></exception>
    public List<ResearchArticle> MergeArticleInfo(List<ResearchArticle> mergingArticles,
        bool asTempArticles = false,
        bool warnMultipleMatches = false,
        bool forceAdd = false,
        bool save = true
        )
    {
        // preprocess
        if (ArticleDatabase == null)
            throw new InvalidOperationException("Database has not been opened yet. Consider to call OpenDataBase() first.");
        var addedCount = 0;
        var updatedCount = 0;
        var mergedArticles = new List<ResearchArticle>();

        // temp
        if (asTempArticles)
            mergingArticles.ForEach(a => a.Private_Temporary = true);

        Console.WriteLine();
        Console.WriteLine($"★ Merging {mergingArticles.Count} article(s) in total...");
        Console.WriteLine();

        // add all first
        ArticleDatabase.AddRange(mergingArticles);

        // if mergeable, merge all
        foreach (var mergingArticle in mergingArticles)
        {
            // merge/update
            if (!forceAdd)
            {
                var mergeResult = MergeIfMergeable(mergingArticle, warnMultipleMatches);
                if (mergeResult is not null)
                {
                    if (UtilConfig.ConsoleOutput_Contents)
                        Console.WriteLine($"@@@ {mergeResult!.ArticleTitle}");

                    mergedArticles.Add(mergeResult!);
                    updatedCount++;
                    articleDatabaseUpdated = true;

                    continue;
                }
            }

            // add new
            {
                if (UtilConfig.ConsoleOutput_Contents)
                    Console.WriteLine($"+++ {mergingArticle!.ArticleTitle}");

                mergedArticles.Add(mergingArticle);
                addedCount++;
                articleDatabaseUpdated = true;
            }
        }


        // save local
        if (save)
            SaveDatabase();
        Console.WriteLine($"★ {addedCount} added, {updatedCount} updated.");
        Console.WriteLine();

        return mergedArticles;
    }

    /// <summary>
    /// merge target article if there are matched articles in database
    /// </summary>
    /// <param name="mergingArticle"></param>
    /// <param name="warnMultipleMatches"></param>
    /// <returns>Return result articles if merged; otherwise, null</returns>
    /// <exception cref="InvalidDataException"></exception>
    public ResearchArticle? MergeIfMergeable(ResearchArticle mergingArticle,
        bool warnMultipleMatches = false
        )
    {
        var matchedArticles = ArticleDatabase
             .Where(a => a.CompareTo(mergingArticle) == 0)
             .Where(a => a != mergingArticle)
             .ToList();

        // multiple matches
        if (warnMultipleMatches && matchedArticles!.Count > 1)
            throw new InvalidDataException($"Expected only 1 article matched but {matchedArticles!.Count} matched to {mergingArticle.ArticleTitle}");

        // merge/update
        if (matchedArticles!.Any())
        {
            var currentLeftArticle = mergingArticle;

            foreach (var matchedArticle in matchedArticles)
            {
                MergeArticles(matchedArticle, currentLeftArticle);
                currentLeftArticle = matchedArticle;
            }

            return currentLeftArticle;
        }

        return null;
    }

    /// <summary>
    /// merge all articles in database if there are matched articles.
    /// </summary>
    /// <param name="warnMultipleMatches"></param>
    /// <returns>Return result articles if merged; otherwise, null</returns>
    /// <exception cref="InvalidDataException"></exception>
    public List<ResearchArticle> MergeIfMergeableForAll(bool warnMultipleMatches = false)
    {
        var results = new List<ResearchArticle>();

        for (int i = ArticleDatabase.Count - 1; i >= 0; i--)
        {
            if (ArticleDatabase.Count >= i)
                continue;

            var targetArticle = ArticleDatabase[i];
            var result = MergeIfMergeable(targetArticle, warnMultipleMatches);
            if (result is not null)
            {
                i--;
                results.Add(result);
            }
        }

        return results;
    }


    /// <summary>
    /// merge articles in ArticleDatabase
    /// </summary>
    /// <param name="mergedArticle">Article that will be merged and stay</param>
    /// <param name="mergingArticle">Article whose info will be extracted and eventually deleted</param>
    public void MergeArticles(ResearchArticle mergedArticle, ResearchArticle mergingArticle)
    {
        // prerocess
        if (!ArticleDatabase!.Contains(mergedArticle))
            throw new InvalidDataException("{mergedArticle} need to be in {ArticleDatabase}");


        // main
        mergedArticle.MergeArticles(mergingArticle);


        // delete and update database 
        if (ArticleDatabase.Contains(mergingArticle))
        {
            // Ref整合性。
            // Replace Reference AOIs.
            foreach (var article in ArticleDatabase)
            {
                if (article.ReferenceAOIs != null && article.ReferenceAOIs.Contains(mergingArticle.AOI))
                    article.ReferenceAOIs = article.ReferenceAOIs
                        .Select(r => r.Replace(mergingArticle.AOI, mergedArticle.AOI))
                        .Distinct()
                        .ToArray();
            }

            // Replace Local PDF.
            if (!mergedArticle.TryFindPDF(PDFsDirectory) && mergingArticle.TryFindPDF(PDFsDirectory))
            {
                var srcPDF = PDFsDirectory.GetChildFileInfo(mergingArticle.LocalPDFName);
                var destPDF = PDFsDirectory.GetChildFileInfo(mergedArticle.LocalPDFName);
                srcPDF.MoveTo(destPDF);
            }

            ArticleDatabase.Remove(mergingArticle);
        }

    }

    /// <summary>
    /// remove articles from local database
    /// </summary>
    /// <param name="mergingArticles"></param>
    /// <param name="forceAdd">never merge and force add all to database</param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="InvalidDataException"></exception>
    public void RemoveArticleInfo(List<ResearchArticle> removingArticles, bool save = true)
    {
        // preprocess
        if (ArticleDatabase == null)
            throw new InvalidOperationException("Database has not been opened yet. Consider to call OpenDataBase() first.");
        var removedCount = 0;

        Console.WriteLine();
        Console.WriteLine($"★ Removing {removingArticles.Count} articles in total...");
        Console.WriteLine();


        // main
        foreach (var removingArticle in removingArticles)
        {

            if (!ArticleDatabase.Contains(removingArticle))
            {
                Console.WriteLine($"(!) Could not find the article to remove (AOI:{removingArticle.AOI}) in database.");
                continue;
            }

            // remove
            else
            {
                // remove ref first...
                foreach (var article in ArticleDatabase)
                {
                    if (article.ReferenceAOIs != null && article.ReferenceAOIs.Contains(removingArticle.AOI))
                        article.ReferenceAOIs = article.ReferenceAOIs
                            .Where(aoi => aoi != removingArticle.AOI)
                            .ToArray();
                }

                // remove article
                ArticleDatabase.Remove(removingArticle);

                if (UtilConfig.ConsoleOutput_Contents)
                    Console.WriteLine($"--- {removingArticle!.ArticleTitle}");

                removedCount++;
                articleDatabaseUpdated = true;
            }
        }


        // save local
        if (save)
            SaveDatabase();
        Console.WriteLine($"★ {removedCount} removed.");
        Console.WriteLine();
    }


    // ★★★★★★★★★★★★★★★

}
