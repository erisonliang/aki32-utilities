using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.Research;
public partial class ResearchArticlesManager
{

    // ★★★★★★★★★★★★★★★ paths

    public DirectoryInfo LocalDirectory { get; set; }
    private FileInfo ArticleDatabaseFileInfo => new(Path.Combine(LocalDirectory.FullName, "ResearchArticles.csv"));
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
    public IEnumerable<ResearchArticle> FetchArticleInfo(IResearchAPIAccessor apiAccessor)
    {
        // preprocess
        if (ArticleDatabase == null)
            throw new InvalidOperationException("Database has not been opened yet. Consider to call OpenDataBase() first.");


        // main
        // post process
        return apiAccessor.FetchArticles();

    }

    /// <summary>
    /// merge articles to local database
    /// </summary>
    /// <param name="uriBuilder"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void MergeArticleInfo(IEnumerable<ResearchArticle> articles)
    {
        // preprocess
        if (ArticleDatabase == null)
            throw new InvalidOperationException("Database has not been opened yet. Consider to call OpenDataBase() first.");
        var addedCount = 0;
        var updatedCount = 0;


        Console.WriteLine();
        Console.WriteLine($"★ Merging {articles.Count()} articles in total...");
        Console.WriteLine();


        // main
        foreach (var article in articles)
        {
            var matchedArticles = ArticleDatabase.Where(a => a.CompareTo(article) == 0);

            // 更新・マージ
            if (matchedArticles != null && matchedArticles.Count() == 1)
            {
                matchedArticles.First().MergeArticles(article!, ArticleDatabase);

                if (UtilConfig.ConsoleOutput_Contents)
                    Console.WriteLine($"@@@ {article!.ArticleTitle}");

                updatedCount++;
                articleDatabaseUpdated = true;
            }

            // おかしい
            else if (matchedArticles!.Count() > 1)
                throw new InvalidDataException($"{matchedArticles!.Count()} articles matched to {article.ArticleTitle}");

            // 新規登録
            else
            {
                ArticleDatabase.Add(article!);

                if (UtilConfig.ConsoleOutput_Contents)
                    Console.WriteLine($"+++ {article!.ArticleTitle}");

                addedCount++;
                articleDatabaseUpdated = true;
            }
        }


        // save local
        SaveDatabase();
        Console.WriteLine($"★ {addedCount} added, {updatedCount} updated.");
        Console.WriteLine();

    }


    // ★★★★★★★★★★★★★★★ methods (practical use)




    // ★★★★★★★★★★★★★★★ methods (helper)

    private static string ExpandXml(string s) => "{http://www.w3.org/2005/Atom}" + s;
    private static string ExpandOpenSearch(string s) => "{http://a9.com/-/spec/opensearch/1.1/}" + s;
    private static string ExpandPrism(string s) => "{http://prismstandard.org/namespaces/basic/2.0/}" + s;


    // ★★★★★★★★★★★★★★★

}
