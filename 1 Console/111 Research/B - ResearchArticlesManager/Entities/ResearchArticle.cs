using System.Diagnostics;

using Aki32Utilities.ConsoleAppUtilities.General;

using ClosedXML;

namespace Aki32Utilities.ConsoleAppUtilities.Research;
public class ResearchArticle : IComparable
{

    // ★★★★★★★★★★★★★★★ field

    private static readonly Range UNSTRUCTURED_REF_STRING_RANGE = ..130;
    private static readonly Range GENERATED_PDF_FILE_NAME_RANGE = ^30..;


    // ★★★★★★★★★★★★★★★ prop

    // ★★★★★ shared info (*main common info)

    public string? ArticleTitle
    {
        get
        {
            return null
                ?? Manual_ArticleTitle.NullIfNullOrEmpty()
                ?? JStage_ArticleTitle_Japanese.NullIfNullOrEmpty()
                ?? CiNii_ArticleTitle.NullIfNullOrEmpty()
                ?? NDLSearch_ArticleTitle.NullIfNullOrEmpty()

                // 英語は後回し
                ?? CrossRef_ArticleTitle.NullIfNullOrEmpty()
                ?? JStage_ArticleTitle_English.NullIfNullOrEmpty()

                // 最終手段。
                ?? ((CrossRef_UnstructuredRefString.NullIfNullOrEmpty() == null) ? null : CrossRef_UnstructuredRefString!.Shorten(UNSTRUCTURED_REF_STRING_RANGE))
                ?? null
                ;
        }
    }
    public string[]? Authors
    {
        get
        {
            return null
                ?? Manual_Authors
                ?? JStage_Authors_Japanese
                ?? CiNii_Authors
                ?? NDLSearch_Authors

                // 英語は後回し
                ?? CrossRef_Authors
                ?? JStage_Authors_English

                // 最終手段。
                ?? ((CrossRef_UnstructuredRefString.NullIfNullOrEmpty() == null) ? null : new string[] { CrossRef_UnstructuredRefString!.Shorten(UNSTRUCTURED_REF_STRING_RANGE) })
                ?? null
                ;
        }
    }
    public string? Description
    {
        get
        {
            return null
                ?? Manual_Description.NullIfNullOrEmpty()
                ?? CiNii_Description.NullIfNullOrEmpty()

                // 優先度低め
                ?? NDLSearch_Description.NullIfNullOrEmpty() // 少々変な情報入りがち

                // 英語は後回し


                // 最終手段。
                ?? null
                ;
        }
    }

    /// <summary>
    /// YYYY-MM-DD.
    /// If only YYYY is given, return YYYY-00-00.
    /// </summary>
    public string? PublishedOn
    {
        get
        {
            return null
                ?? Manual_PublishedDate.NullIfNullOrEmpty()
                ?? CrossRef_PublishedDate.NullIfNullOrEmpty()
                ?? CiNii_PublishedDate.NullIfNullOrEmpty()

                // 優先度低め
                ?? NDLSearch_PublishedDate.NullIfNullOrEmpty() // 超長い形式になる可能性あり
                ?? JStage_PublishedYear.NullIfNullOrEmpty() // 年数しか来ない

                // 最終手段。
                ?? null
                ;
        }
    }

    public string? ReferenceString
    {
        get
        {
            return null
               ?? CrossRef_UnstructuredRefString.NullIfNullOrEmpty()

               // TODO: 手動作成！
               ?? null
               ;
        }
    }

    public string? DOI { get; set; }
    public string[]? ReferenceDOIs { get; set; }

    public string? PrintISSN { get; set; }
    public string? OnlineISSN { get; set; }




    // ★★★★★ shared links (*main common info)

    public string? DOI_Link => (string.IsNullOrEmpty(DOI)) ? null : $"https://dx.doi.org/{DOI}";

    [CsvIgnore]
    public string? CrossRefAPI_Link => (string.IsNullOrEmpty(DOI)) ? null : $"https://api.crossref.org/v1/works/{DOI}";

    public string? PDF_Link
    {
        get
        {
            if (string.IsNullOrEmpty(DOI))
                return null;

            // get data from aij
            if (DOI.Contains("aijs"))
            {
                if (JStage_Link_Japanese != null)
                    return JStage_Link_Japanese!.Replace($"_article/", $"_pdf/");
                if (JStage_Link_English != null)
                    return JStage_Link_English!.Replace($"_article/", $"_pdf/");
            }

            return null;
        }
    }

    public string? LocalPDFName
    {
        get
        {
            if (!string.IsNullOrEmpty(DOI))
                return DOI.Replace("/", "_");

            if (!string.IsNullOrEmpty(AOI))
                return AOI;

            if (!string.IsNullOrEmpty(PDF_Link))
            {
                var candidate = PDF_Link.Replace("/", "_").Replace(":", "_");
                return candidate.Shorten(GENERATED_PDF_FILE_NAME_RANGE);
            }

            return null;
        }
    }



    // ★★★★★ original meta info

    /// <summary>
    /// favorite flag
    /// </summary>
    public bool? Private_Favorite { get; set; }

    public bool? DataFrom_Manual { get; set; }
    public bool? DataFrom_JStage { get; set; }
    public bool? DataFrom_CiNii { get; set; }
    public bool? DataFrom_CrossRef { get; set; }
    public bool? DataFrom_NDLSearch { get; set; }

    /// <summary>
    /// Aki32 Object Identifier
    /// All object has its own AOI.
    /// </summary>
    /// <remarks>
    /// 全ての要素に対して発行。
    /// AOIで接続するのは，本当に最終手段。
    /// </remarks>
    public string? AOI { get; set; }


    // ★★★★★ manual info

    public string? Manual_ArticleTitle { get; set; }
    public string[]? Manual_Authors { get; set; }
    public string? Manual_Description { get; set; }
    public string? Manual_PublishedDate { get; set; }

    public string? Manual_CreatedDate { get; set; }


    // ★★★★★ memo info

    public string? Memo { get; set; }

    /// <summary>
    /// 【Motivation: 研究の出発点】
    /// <br/> - どんな課題や問題点を解決しようとしたのか？
    /// <br/> - 既存の研究で足りないところはどこだったのか？
    /// </summary>
    public string Memo_Motivation { get; set; } = "";

    /// <summary>
    /// 【Method: 研究手法】
    /// <br/> - どんなシステムを作ったか？なぜそのシステム設計でよいと仮定したか？
    /// <br/> - どんなアルゴリズムを作ったか？なぜそのアルゴリズム設計でよいと仮定したか？
    /// <br/> - どんな調査をしたか？なぜその調査計画でよいと仮定したか？
    /// <br/> - どんな実験をしたか？なぜその実験設計でよいと仮定したか？
    /// </summary>
    public string Memo_Method { get; set; } = "";

    /// <summary>
    /// 【Insights: 結果と知見】
    /// <br/> - どんな結果が得られたのか？どんな条件だと上手くいって，どんな場合は上手くいかなかったのか？
    /// <br/> - 新しくわかった知見はなにか？他のアプリケーションやシステムでも使えそうな知見は何か？
    /// </summary>
    public string Memo_Insights { get; set; } = "";

    /// <summary>
    /// 【Contribution Summary: 貢献を一行で！】
    /// <br/> - 「【Author】は，【Motivation】という課題解決のために【Method】を行い，【Insight】がわかった。」
    /// </summary>
    public string Memo_Contribution { get; set; } = "";


    // ★★★★★ CrossRef

    public string? CrossRef_ArticleTitle { get; set; }
    public string[]? CrossRef_Authors { get; set; }

    public string? CrossRef_UnstructuredRefString { get; set; }
    public string? CrossRef_PublishedDate { get; set; }


    // ★★★★★ mainly from J-Stage

    public string? JStage_ArticleTitle_English { get; set; }
    public string? JStage_ArticleTitle_Japanese { get; set; }

    public string[]? JStage_Authors_English { get; set; }
    public string[]? JStage_Authors_Japanese { get; set; }


    public string? JStage_Link_English { get; set; }
    public string? JStage_Link_Japanese { get; set; }

    public string? JStage_JournalCode { get; set; }

    public string? JStage_MaterialTitle_English { get; set; }
    public string? JStage_MaterialTitle_Japanese { get; set; }

    public string? JStage_PublishedYear { get; set; }

    public string? JStage_Volume { get; set; }
    public string? JStage_SubVolume { get; set; }
    public string? JStage_Number { get; set; }
    public string? JStage_StartingPage { get; set; }
    public string? JStage_EndingPage { get; set; }

    public string? JStage_JOI { get; set; }

    public string? JStage_SystemCode { get; set; }
    public string? JStage_SystemName { get; set; }

    public string? JStage_Id { get; set; }
    public string? JStage_UpdatedOn { get; set; }


    // ★★★★★ from CiNii

    public string? CiNii_ArticleTitle { get; set; }
    public string[]? CiNii_Authors { get; set; }

    public string? CiNii_Description { get; set; }

    public string? CiNii_Link { get; set; }

    public string? CiNii_Publisher { get; set; }
    public string? CiNii_PublicationName { get; set; }
    public string? CiNii_PublishedDate { get; set; }

    public string? CiNii_Volume { get; set; }
    public string? CiNii_Number { get; set; }
    public string? CiNii_StartingPage { get; set; }
    public string? CiNii_EndingPage { get; set; }


    // ★★★★★ from NDL Search

    public string? NDLSearch_ArticleTitle { get; set; }
    public string[]? NDLSearch_Authors { get; set; }

    public string? NDLSearch_Description { get; set; }

    public string? NDLSearch_Link { get; set; }

    public string? NDLSearch_Publisher { get; set; }
    public string? NDLSearch_PublicationName { get; set; }
    public string? NDLSearch_PublishedDate { get; set; }

    public string? NDLSearch_Volume { get; set; }
    public string? NDLSearch_Number { get; set; }
    public string? NDLSearch_StartingPage { get; set; }
    public string? NDLSearch_EndingPage { get; set; }


    // ★★★★★★★★★★★★★★★ init

    public ResearchArticle()
    {
        AOI = Guid.NewGuid().ToString();
    }


    // ★★★★★★★★★★★★★★★ method (data handling)

    /// <summary>
    /// Create ResearchArticle instance manually.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static ResearchArticle CreateManually(
        ResearchArticle_ManualInitInfo addingArticleBasicInfo,
        ResearchArticle[]? references_parents = null,
        ResearchArticle[]? references_children = null,

        DirectoryInfo? pdfStockDirectory = null,
        FileInfo? addingPdfFile = null,
        bool deleteOriginalPdfFile = true
        )
    {
        // Create AOI first
        var addingArticle = new ResearchArticle()
        {
            AOI = Guid.NewGuid().ToString(),
            Manual_CreatedDate = DateTime.Today.ToLongDateString(),
            DataFrom_Manual = true,

            DOI = addingArticleBasicInfo.DOI,
            Private_Favorite = addingArticleBasicInfo.Private_Favorite,

            Manual_ArticleTitle = addingArticleBasicInfo.Manual_ArticleTitle,
            Manual_Authors = addingArticleBasicInfo.Manual_Authors,
            Manual_Description = addingArticleBasicInfo.Manual_Description,
            Manual_PublishedDate = addingArticleBasicInfo.Manual_PublishedDate,

            Memo = addingArticleBasicInfo.Memo,
        };

        if (references_parents != null)
        {
            foreach (var references_parent in references_parents)
                addingArticle.AddArticleReference(references_parent);
        }
        if (references_children != null)
        {
            foreach (var references_child in references_children)
                references_child.AddArticleReference(addingArticle);
        }

        // stock pdf file to database
        if (addingPdfFile != null)
        {
            if (pdfStockDirectory == null)
                throw new InvalidDataException("When tring to add PDF file, {pdfStockDirectory} must not be null");

            var targetFile = pdfStockDirectory.GetChildFileInfo($"{addingArticle.LocalPDFName}.pdf");

            if (deleteOriginalPdfFile)
                addingPdfFile.MoveTo(targetFile);
            else
                addingPdfFile.CopyTo(targetFile);

        }

        return addingArticle;
    }

    /// <summary>
    /// Add reference connection info to Article.
    /// </summary>
    /// <param name="referredArticle">
    /// Article that is begin referred. 
    /// 参照される側の論文。
    /// </param>
    public void AddArticleReference(ResearchArticle referredArticle)
    {
        // Add DOI or AOI to ReferenceDOIs.
        ReferenceDOIs ??= Array.Empty<string>();

        ReferenceDOIs = ReferenceDOIs
            .Append(referredArticle.DOI ?? (referredArticle.AOI ??= Guid.NewGuid().ToString()))!
            .Distinct()
            .ToArray()
            ;

    }


    // ★★★★★★★★★★★★★★★ method (practical use)

    public void TryDownloadPDF(DirectoryInfo pdfStockDirectory)
    {
        UtilPreprocessors.PreprocessBasic();

        try
        {
            if (LocalPDFName == null)
                throw new Exception("Local PDF Name cannot be implied.");

            var outputFile = pdfStockDirectory.GetChildFileInfo($"{LocalPDFName}.pdf");
            new Uri(PDF_Link!).DownloadFileAsync(outputFile, true).Wait();
        }
        catch (Exception ex)
        {
            ConsoleExtension.WriteLineWithColor($"Failed: {ex.Message}", ConsoleColor.Red);
        }
    }

    public void TryOpenPDF(DirectoryInfo pdfStockDirectory)
    {
        UtilPreprocessors.PreprocessBasic();

        try
        {
            if (LocalPDFName == null)
                throw new Exception("Local PDF Name couldn't be implied.");

            var outputFilePath = Path.Combine(pdfStockDirectory.FullName, $"{LocalPDFName}.pdf");

            if (!File.Exists(outputFilePath))
                TryDownloadPDF(pdfStockDirectory);

            var p = Process.Start(new ProcessStartInfo()
            {
                FileName = outputFilePath,
                UseShellExecute = true,
            });

            return;

        }
        catch (Exception ex)
        {
            ConsoleExtension.WriteLineWithColor($"Failed: {ex.Message}", ConsoleColor.Red);
        }
    }

    public void TryOpenDOI()
    {
        UtilPreprocessors.PreprocessBasic();

        try
        {
            var p = Process.Start(DOI_Link!);
        }
        catch (Exception ex)
        {
            ConsoleExtension.WriteLineWithColor($"Failed: {ex.Message}", ConsoleColor.Red);
        }
    }

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
    /// <param name="mergingArticle">Article that will be merged and eventually deleted</param>
    /// <param name="articles">If given, this method will make sure to delete {mergingArticle} from this list for you</param>
    public void MergeArticles(ResearchArticle mergingArticle, List<ResearchArticle>? articles = null)
    {
        // prerocess
        if (articles != null && !articles!.Contains(this))
            throw new InvalidDataException("{mergedArticle} need to be in {articles}");


        // main
        var props = typeof(ResearchArticle)
            .GetProperties()
            .Where(p => !p.HasAttribute<CsvIgnoreAttribute>())
            .Where(p => p.CanWrite)
            .Where(p => p.Name != "DOI")
            .Where(p => p.Name != "AOI")
            ;


        // DOIが存在する場合は，最優先で採用。
        // AOIは，前の情報を正とする。
        if (DOI != null)
        {
            if (mergingArticle.DOI != null && DOI != mergingArticle.DOI)
                throw new InvalidDataException("DOIが異なる2つがマージされようとしました。");
        }
        else if (mergingArticle.DOI != null)
            DOI = mergingArticle.DOI;


        // 後からの情報優先で上書き。
        foreach (var prop in props)
        {
            var addingArticleInfoProp = prop.GetValue(mergingArticle);

            if (prop.PropertyType == typeof(string))
            {
                if (addingArticleInfoProp?.ToString().NullIfNullOrEmpty() != null)
                    prop.SetValue(this, addingArticleInfoProp);

            }
            else
            {
                if (addingArticleInfoProp != null)
                    prop.SetValue(this, addingArticleInfoProp);

            }

        }

        //delete
        if (articles != null && articles.Contains(mergingArticle))
        {
            // Ref整合性。消す場合はRefから消したい。つまり，前のやつのAOIを今のAOIに書き換える。

            articles = articles.Select(a =>
            {
                if (a.ReferenceDOIs != null && a.ReferenceDOIs.Contains(mergingArticle.AOI))
                    a.ReferenceDOIs = a.ReferenceDOIs.Select(r => r.Replace(mergingArticle.AOI, AOI)).ToArray();
                return a;
            }).ToList();

            articles.Remove(mergingArticle);
        }

    }


    // ★★★★★★★★★★★★★★★ method (helper)

    public static string? CleanUp_UnstructuredRefString(string? rawUnstructuredRefString, int checkCount = 4)
    {
        if (rawUnstructuredRefString == null)
            return rawUnstructuredRefString;

        var unstructuredRefString = rawUnstructuredRefString;

        for (int i = 0; i < checkCount; i++)
        {
            if (unstructuredRefString.Trim() == unstructuredRefString[1..].Trim())
                return unstructuredRefString.Trim();
            else
                unstructuredRefString = unstructuredRefString[1..];
        }

        return rawUnstructuredRefString;
    }

    public int CompareTo(object? obj)
    {
        var comparingArticle = (ResearchArticle)obj!;

        var result = 0;
        var power = (int)Math.Pow(2, 20);

        static IEnumerable<string> Targets()
        {
            // id
            yield return nameof(DOI);
            yield return nameof(AOI);
            yield return nameof(JStage_Id);

            // title
            yield return nameof(JStage_ArticleTitle_Japanese);
            yield return nameof(JStage_ArticleTitle_English);
            yield return nameof(CrossRef_ArticleTitle);
            yield return nameof(CiNii_ArticleTitle);
            yield return nameof(NDLSearch_ArticleTitle);
            yield return nameof(Manual_ArticleTitle);
            yield return nameof(ArticleTitle);

            // others
            yield return nameof(CrossRef_UnstructuredRefString);

        }

        foreach (var target in Targets())
        {
            var prop = GetType().GetProperty(target);
            var value1 = prop?.GetValue(this)?.ToString();
            var value2 = prop?.GetValue(comparingArticle)?.ToString();

            if (!string.IsNullOrEmpty(value1) && !string.IsNullOrEmpty(value2))
            {
                var com = value1!.CompareTo(value2);
                if (com == 0)
                    return 0;
                result += power * Math.Sign(com);
            }

            power /= 2;
        }

        return (result == 0) ? (GetHashCode() - comparingArticle.GetHashCode()) : result;
    }


    // ★★★★★★★★★★★★★★★ 

}
