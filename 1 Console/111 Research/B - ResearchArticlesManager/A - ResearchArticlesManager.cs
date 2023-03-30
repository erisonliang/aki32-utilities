using Aki32Utilities.ConsoleAppUtilities.General;

using ClosedXML;

using DocumentFormat.OpenXml.Spreadsheet;

using MathNet.Numerics.Distributions;

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
    public void PullArticleInfo(IResearchAPIAccessor uriBuilder)
    {
        var fetchedArticles = FetchArticleInfo(uriBuilder);
        MergeArticleInfo(fetchedArticles);
    }

    /// <summary>
    /// fetch data from dataserver
    /// </summary>
    /// <param name="apiAccessor"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public List<ResearchArticle> FetchArticleInfo(IResearchAPIAccessor apiAccessor)
    {
        // preprocess
        if (ArticleDatabase == null)
            throw new InvalidOperationException("Database has not been opened yet. Consider to call OpenDataBase() first.");


        // main
        // post process
        return apiAccessor.FetchArticles().ToList();

    }

    /// <summary>
    /// merge/add articles to local database
    /// </summary>
    /// <param name="mergingArticles"></param>
    /// <param name="forceAdd">never merge and force add all to database</param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="InvalidDataException"></exception>
    public void MergeArticleInfo(List<ResearchArticle> mergingArticles, bool forceAdd = false, bool warnMultipleMatches = false, bool save = true)
    {
        // preprocess
        if (ArticleDatabase == null)
            throw new InvalidOperationException("Database has not been opened yet. Consider to call OpenDataBase() first.");
        var addedCount = 0;
        var updatedCount = 0;


        Console.WriteLine();
        Console.WriteLine($"★ Merging {mergingArticles.Count} articles in total...");
        Console.WriteLine();


        // main
        foreach (var mergingArticle in mergingArticles)
        {
            var matchedArticles = ArticleDatabase.Where(a => a.CompareTo(mergingArticle) == 0);

            // multiple matches
            if (warnMultipleMatches && matchedArticles!.Count() > 1)
                throw new InvalidDataException($"{matchedArticles!.Count()} articles matched to {mergingArticle.ArticleTitle}");

            // merge/update
            if (matchedArticles.Count() == 1 && !forceAdd)
            {
                MergeArticles(matchedArticles.First(), mergingArticle);

                if (UtilConfig.ConsoleOutput_Contents)
                    Console.WriteLine($"@@@ {mergingArticle!.ArticleTitle}");

                updatedCount++;
                articleDatabaseUpdated = true;
            }

            // add new
            else
            {
                ArticleDatabase.Add(mergingArticle!);

                if (UtilConfig.ConsoleOutput_Contents)
                    Console.WriteLine($"+++ {mergingArticle!.ArticleTitle}");

                addedCount++;
                articleDatabaseUpdated = true;
            }
        }


        // save local
        if (save)
            SaveDatabase();
        Console.WriteLine($"★ {addedCount} added, {updatedCount} updated.");
        Console.WriteLine();

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
            // Replace previous AOI to new AOI.
            foreach (var article in ArticleDatabase)
            {
                if (article.ReferenceAOIs != null && article.ReferenceAOIs.Contains(mergingArticle.AOI))
                    article.ReferenceAOIs = article.ReferenceAOIs
                        .Select(r => r.Replace(mergingArticle.AOI, mergedArticle.AOI))
                        .ToArray();
            }

            // Replace previous PDF names to new PDF names.
            if (mergingArticle.TryFindPDF(PDFsDirectory) && !mergedArticle.TryFindPDF(PDFsDirectory))
            {
                var srcPDF = PDFsDirectory.GetChildFileInfo(mergingArticle.LocalPDFName);
                var destPDF = PDFsDirectory.GetChildFileInfo(mergedArticle.LocalPDFName);
                srcPDF.MoveTo(destPDF);
            }

            ArticleDatabase.Remove(mergingArticle);
        }

    }


    // ★★★★★★★★★★★★★★★ methods (practical use)




    // ★★★★★★★★★★★★★★★

}
