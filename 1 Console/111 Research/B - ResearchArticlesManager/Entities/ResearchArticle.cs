using System.Diagnostics;

using Aki32Utilities.ConsoleAppUtilities.General;

using ClosedXML;

using DocumentFormat.OpenXml.Spreadsheet;

namespace Aki32Utilities.ConsoleAppUtilities.Research;
public class ResearchArticle : IComparable
{

    // ★★★★★★★★★★★★★★★ field

    private static readonly Range UNSTRUCTURED_REF_STRING_RANGE = ..200;
    private static readonly Range FRIENDLY_AOI_RANGE = ^6..;


    // ★★★★★★★★★★★★★★★ prop

    private int aaa = 0;


    // ★★★★★ shared info (*main common info)

    public string? ArticleTitle
    {
        get
        {
            return null
                ?? Manual_ArticleTitle.NullIfNullOrEmpty()

                // 日本語
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

                // 日本語
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
                ?? CiNii_Description.NullIfNullOrEmpty()

                // 優先度低め
                ?? NDLSearch_Description.NullIfNullOrEmpty() // 少々変な情報入りがち

                // 英語は後回し


                // 最終手段。
                ?? null
                ;
        }
    }
    public string? MaterialTitle
    {
        get
        {
            return null
                ?? Manual_MaterialTitle.NullIfNullOrEmpty()

                // 日本語
                ?? JStage_MaterialTitle_Japanese.NullIfNullOrEmpty()

                // 英語は後回し
                ?? JStage_MaterialTitle_English.NullIfNullOrEmpty()

                // 最終手段。
                ?? null
                ;
        }
    }
    public string? MaterialVolume
    {
        get
        {
            return null
                ?? Manual_MaterialVolume.NullIfNullOrEmpty()

                ?? JStage_MaterialVolume.NullIfNullOrEmpty()
                ?? NDLSearch_MaterialVolume.NullIfNullOrEmpty()
                ?? CiNii_MaterialVolume.NullIfNullOrEmpty()

                // 最終手段。
                ?? null
                ;
        }
    }
    public string? MaterialSubVolume
    {
        get
        {
            return null
                ?? Manual_MaterialSubVolume.NullIfNullOrEmpty()

                ?? JStage_MaterialSubVolume.NullIfNullOrEmpty()
                //?? NDLSearch_MaterialSubVolume.NullIfNullOrEmpty()
                //?? CiNii_MaterialSubVolume.NullIfNullOrEmpty()

                // 最終手段。
                ?? null
                ;
        }
    }
    public string? StartingPage
    {
        get
        {
            return null
                ?? Manual_StartingPage.NullIfNullOrEmpty()

                ?? JStage_StartingPage.NullIfNullOrEmpty()
                ?? NDLSearch_StartingPage.NullIfNullOrEmpty()
                ?? CiNii_StartingPage.NullIfNullOrEmpty()

                // 最終手段。
                ?? null
                ;
        }
    }
    public string? EndingPage
    {
        get
        {
            return null
                ?? Manual_EndingPage.NullIfNullOrEmpty()

                ?? JStage_EndingPage.NullIfNullOrEmpty()
                ?? NDLSearch_EndingPage.NullIfNullOrEmpty()
                ?? CiNii_EndingPage.NullIfNullOrEmpty()

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
    [CsvIgnore]
    public (int? year, int? month, int? day)? PublishedOn_Numbers
    {
        get
        {
            var s = PublishedOn;
            int? year = null;
            int? month = null;
            int? day = null;

            if (s is null)
                return null;

            var ss = s.Split(new char[] { '/', '-', '_', ',', '.', ' ', '\\' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (ss.Length >= 1)
                if (int.TryParse(ss[0], out int temp))
                    year = temp;

            if (ss.Length >= 2)
                if (int.TryParse(ss[1], out int temp))
                    month = temp;

            if (ss.Length >= 3)
                if (int.TryParse(ss[2], out int temp))
                    day = temp;

            return (year, month, day);
        }
    }

    [CsvIgnore]
    public string? ReferenceErrorReasonString = "";
    [CsvIgnore]
    public string ReferenceStringFormat = "{Authors}: {ArticleTitle}, {MaterialTitle}, {MaterialVolume}, {MaterialSubVolume}, pp.{StartingPage}-{EndingPage}, {PublishedYear}";
    public string? ReferenceString
    {
        get
        {
            string ReferenceStringManually()
            {
                var s = ReferenceStringFormat;

                if (ReferenceStringFormat.Contains("{Authors}"))
                {
                    if (Authors is null)
                    {
                        ReferenceErrorReasonString = "※ Need to fill Authors";
                        return null;
                    }

                    var authors = string.Join(", ", Authors!);
                    s = s.Replace("{Authors}", authors);
                }
                if (ReferenceStringFormat.Contains("{ArticleTitle}"))
                {
                    if (ArticleTitle is null)
                    {
                        ReferenceErrorReasonString = "※ Need to fill ArticleTitle";
                        return null;
                    }

                    s = s.Replace("{ArticleTitle}", ArticleTitle);
                }
                if (ReferenceStringFormat.Contains("{MaterialTitle}"))
                {
                    if (MaterialTitle is null)
                    {
                        ReferenceErrorReasonString = "※ Need to fill MaterialTitle";
                        return null;
                    }

                    s = s.Replace("{MaterialTitle}", MaterialTitle);
                }

                if (ReferenceStringFormat.Contains("{MaterialVolume}"))
                {
                    if (MaterialVolume is null)
                    {
                        ReferenceErrorReasonString = "※ Need to fill MaterialVol";
                        return null;
                    }

                    if (uint.TryParse(MaterialVolume, out uint a))
                        s = s.Replace("{MaterialVolume}", $"第{MaterialVolume}巻");
                    else
                        s = s.Replace("{MaterialVolume}", $"{MaterialVolume}");
                }
                if (ReferenceStringFormat.Contains("{MaterialSubVolume}"))
                {
                    if (MaterialSubVolume is null)
                    {
                        s = s.Replace("{MaterialSubVolume}", $"");
                        s = s.Replace(", , ", $", ");
                    }
                    else
                    {
                        if (uint.TryParse(MaterialSubVolume, out uint a))
                            s = s.Replace("{MaterialSubVolume}", $"第{MaterialVolume}号");
                        else
                            s = s.Replace("{MaterialSubVolume}", $"{MaterialVolume}");
                    }
                }

                if (ReferenceStringFormat.Contains("{StartingPage}"))
                {
                    if (StartingPage is null)
                    {
                        ReferenceErrorReasonString = "※ Need to fill StartingPage";
                        return null;
                    }

                    s = s.Replace("{StartingPage}", StartingPage);
                }
                if (ReferenceStringFormat.Contains("{EndingPage}"))
                {
                    if (EndingPage is null)
                    {
                        ReferenceErrorReasonString = "※ Need to fill EndingPage";
                        return null;
                    }

                    s = s.Replace("{EndingPage}", EndingPage);
                }

                if (ReferenceStringFormat.Contains("{PublishedYear}"))
                {
                    var date = PublishedOn_Numbers;
                    if (date is null || date.Value.year is null)
                    {
                        ReferenceErrorReasonString = "※ Need to fill PublishedYear";
                        return null;
                    }

                    var dateString = $"{date.Value.year.Value}";
                    if (date.Value.month is not null)
                        dateString += $".{date.Value.month.Value}";

                    s = s.Replace("{PublishedYear}", dateString);
                }

                return s;
            }

            return null
               // 生成
               ?? ReferenceStringManually().NullIfNullOrEmpty()

               // 自動
               ?? CrossRef_UnstructuredRefString.NullIfNullOrEmpty()

               // 生成失敗理由
               ?? ReferenceErrorReasonString.NullIfNullOrEmpty()
               ?? null
               ;
        }
    }

    [UseExceptionalMerging]
    public string? DOI { get; set; }
    [UseExceptionalMerging]
    public string[]? ReferenceAOIs { get; set; }

    public string? PrintISSN { get; set; }
    public string? OnlineISSN { get; set; }


    // ★★★★★ shared links (*main common info)

    public string? DOI_Link => (string.IsNullOrEmpty(DOI)) ? null : $"https://dx.doi.org/{DOI}";

    [CsvIgnore]
    public string? CrossRefAPI_Link => (string.IsNullOrEmpty(DOI)) ? null : $"https://api.crossref.org/v1/works/{DOI}";

    /// <summary>
    /// online PDF link
    /// </summary>
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

    /// <summary>
    /// local PDF name
    /// </summary>
    [CsvIgnore]
    public string LocalPDFName => $"{AOI}.pdf";


    // ★★★★★ original meta info

    [UseExceptionalBinaryBothMerging]
    public bool? Private_Temporary { get; set; } = false;

    [UseExceptionalBinaryEitherMerging]
    public bool? Private_Favorite { get; set; } = false;
    [UseExceptionalBinaryEitherMerging]
    public bool? Private_Read { get; set; } = false;
    [UseExceptionalBinaryEitherMerging]
    public bool? Private_IsCategory1 { get; set; } = false;
    [UseExceptionalBinaryEitherMerging]
    public bool? Private_IsCategory2 { get; set; } = false;
    [UseExceptionalBinaryEitherMerging]
    public bool? Private_IsCategory3 { get; set; } = false;

    [UseExceptionalBinaryEitherMerging]
    public bool? DataFrom_Manual { get; set; } = false;
    [UseExceptionalBinaryEitherMerging]
    public bool? DataFrom_JStage { get; set; } = false;
    [UseExceptionalBinaryEitherMerging]
    public bool? DataFrom_CiNii { get; set; } = false;
    [UseExceptionalBinaryEitherMerging]
    public bool? DataFrom_CrossRef { get; set; } = false;
    [UseExceptionalBinaryEitherMerging]
    public bool? DataFrom_NDLSearch { get; set; } = false;

    /// <summary>
    /// Aki32 Object Identifier
    /// All object has its own AOI.
    /// </summary>
    /// <remarks>
    /// 全ての要素に対して発行。
    /// これをメインIDとして使う。
    /// </remarks>
    public string AOI { get; set; } = Ulid.NewUlid().ToString();
    [CsvIgnore]
    public string Friendly_AOI => AOI[FRIENDLY_AOI_RANGE];



    // ★★★★★ manual info

    public string? Manual_ArticleTitle { get; set; }
    public string[]? Manual_Authors { get; set; }

    public string? Manual_MaterialTitle { get; set; }
    public string? Manual_MaterialVolume { get; set; }
    public string? Manual_MaterialSubVolume { get; set; }

    public string? Manual_StartingPage { get; set; }
    public string? Manual_EndingPage { get; set; }

    public string? Manual_PublishedDate { get; set; }
    public string Manual_CreatedDate { get; set; } = DateTime.Today.ToLongDateString();



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

    public string? JStage_MaterialVolume { get; set; }
    public string? JStage_MaterialSubVolume { get; set; }
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

    public string? CiNii_MaterialVolume { get; set; }
    //public string? CiNii_MaterialSubVolume { get; set; }
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

    public string? NDLSearch_MaterialVolume { get; set; }
    //public string? NDLSearch_MaterialSubVolume { get; set; }
    public string? NDLSearch_Number { get; set; }
    public string? NDLSearch_StartingPage { get; set; }
    public string? NDLSearch_EndingPage { get; set; }


    // ★★★★★★★★★★★★★★★ init

    public ResearchArticle()
    {
    }

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
        // create new article
        var addingArticle = new ResearchArticle()
        {
            DataFrom_Manual = true,

            DOI = addingArticleBasicInfo.DOI,
            Private_Favorite = addingArticleBasicInfo.Private_Favorite,

            Manual_ArticleTitle = addingArticleBasicInfo.Manual_ArticleTitle,
            Manual_Authors = addingArticleBasicInfo.Manual_Authors,
            Manual_PublishedDate = addingArticleBasicInfo.Manual_PublishedDate,

            Memo = addingArticleBasicInfo.Memo,
        };

        // add ref
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

        // add pdf
        if (addingPdfFile != null && pdfStockDirectory != null)
            addingArticle.AddPDFManually(pdfStockDirectory, addingPdfFile, deleteOriginalPdfFile: deleteOriginalPdfFile);

        return addingArticle;
    }


    // ★★★★★★★★★★★★★★★ method (data handling)

    /// <summary>
    /// Add pdf manually
    /// </summary>
    /// <param name="pdfStockDirectory"></param>
    /// <param name="addingPdfFile"></param>
    public void AddPDFManually(DirectoryInfo pdfStockDirectory, FileInfo addingPdfFile,
        bool deleteOriginalPdfFile = true,
        bool forceOverwriteExistingPDF = false
        )
    {
        var destinationFile = pdfStockDirectory.GetChildFileInfo(LocalPDFName);

        if (destinationFile.Exists && !forceOverwriteExistingPDF)
            throw new IOException("PDF already exist.");

        if (deleteOriginalPdfFile)
            addingPdfFile.MoveTo(destinationFile);
        else
            addingPdfFile.CopyTo(destinationFile);

    }

    /// <summary>
    /// Add reference connection info to Article.
    /// </summary>
    /// <param name="referredArticle">
    /// Article that is being referred. 
    /// 参照される側の論文。
    /// </param>
    public void AddArticleReference(ResearchArticle referredArticle)
    {
        // Add DOI or AOI to ReferenceDOIs.
        ReferenceAOIs ??= Array.Empty<string>();

        ReferenceAOIs = ReferenceAOIs
            .Append(referredArticle.AOI)!
            .Distinct()
            .ToArray()
            ;

    }

    /// <summary>
    /// Remove reference connection info from Article.
    /// </summary>
    /// <param name="referredArticle">
    /// Article that used to be being referred. 
    /// 参照されていた側の論文。
    /// </param>
    public void RemoveArticleReference(ResearchArticle referredArticle)
    {
        // Remove DOI and AOI from ReferenceDOIs.
        if (ReferenceAOIs is null)
            return;

        ReferenceAOIs = ReferenceAOIs
            .Where(aoi => aoi != referredArticle.AOI)
            .ToArray()
            ;

    }

    /// <summary>
    /// Check if all search strings matched to this article
    /// </summary>
    /// <param name="searchFullString"></param>
    /// <param name="needMatchAll"></param>
    /// <returns></returns>
    public bool GetIfSearchStringsMatched(string[] searchStrings)
    {
        if (searchStrings.Length == 0)
            return false;

        foreach (var searchString in searchStrings)
            if (!GetIfSearchStringMatched(searchString))
                return false;

        return true;
    }
    private bool GetIfSearchStringMatched(string searchString)
    {
        if (Authors is not null && Authors.Any(a => a.Contains(searchString)))
            return true;

        if (ArticleTitle is not null && ArticleTitle.Contains(searchString))
            return true;

        if (PublishedOn is not null && PublishedOn.Contains(searchString))
            return true;

        if (Memo is not null && Memo.Contains(searchString))
            return true;

        if (AOI.Contains(searchString))
            return true;

        return false;
    }

    /// <summary>
    /// Merge two ResearchArticle instances.
    /// </summary>
    /// <remarks>
    /// 後からの情報が優先（最新）
    /// </remarks>
    /// <param name="mergingArticle">Article whose info will be extracted and eventually deleted</param>
    public void MergeArticles(ResearchArticle mergingArticle)
    {
        // main

        // DOIは存在するほうを採用。異なる場合は許容しない。
        {
            if (DOI != null && mergingArticle.DOI != null && DOI != mergingArticle.DOI)
                throw new InvalidDataException("DOIが異なる2つがマージされようとしました。");
            if (DOI == null && mergingArticle.DOI != null)
                DOI = mergingArticle.DOI;
        }

        // 引用関係は，被りなく両方から持ってくる。
        {
            if (ReferenceAOIs is null)
            {
                ReferenceAOIs = mergingArticle.ReferenceAOIs;
            }
            else
            {
                if (mergingArticle.ReferenceAOIs is null)
                {
                }
                else
                {
                    ReferenceAOIs = ReferenceAOIs
                        .Concat(mergingArticle.ReferenceAOIs)
                        .Distinct()
                        .ToArray();
                }
            }
        }

        // UseExceptionalBinaryBothMergingAttribute付きは，両方に印ついてるなら採用。
        {
            var andMixingBinaryProps = typeof(ResearchArticle)
                 .GetProperties()
                 .Where(p => p.CanWrite)
                 .Where(p => !p.HasAttribute<CsvIgnoreAttribute>())
                 .Where(p => p.HasAttribute<UseExceptionalBinaryBothMergingAttribute>())
                 ;

            foreach (var prop in andMixingBinaryProps)
            {
                var thisArticleInfoProp = prop.GetValue(this);
                var mergingArticleInfoProp = prop.GetValue(mergingArticle);

                var binaryMixedValue =
                    ((bool?)thisArticleInfoProp ?? false) &&
                    ((bool?)mergingArticleInfoProp ?? false);

                prop.SetValue(this, binaryMixedValue);
            }
        }

        // UseExceptionalBinaryEitherMergingAttribute付きは，印ついてるほうを優先して採用。
        {
            var orMixingBinaryProps = typeof(ResearchArticle)
                 .GetProperties()
                 .Where(p => p.CanWrite)
                 .Where(p => !p.HasAttribute<CsvIgnoreAttribute>())
                 .Where(p => p.HasAttribute<UseExceptionalBinaryEitherMergingAttribute>())
                 ;

            foreach (var prop in orMixingBinaryProps)
            {
                var thisArticleInfoProp = prop.GetValue(this);
                var mergingArticleInfoProp = prop.GetValue(mergingArticle);

                var binaryMixedValue =
                    ((bool?)thisArticleInfoProp ?? false) ||
                    ((bool?)mergingArticleInfoProp ?? false);

                prop.SetValue(this, binaryMixedValue);
            }
        }

        // 他は，後からの情報優先で上書き。
        {
            var latterOrientedProps = typeof(ResearchArticle)
                .GetProperties()
                .Where(p => p.CanWrite)
                .Where(p => !p.HasAttribute<CsvIgnoreAttribute>())
                .Where(p => !p.HasAttribute<UseExceptionalMergingAttribute>())
                .Where(p => !p.HasAttribute<UseExceptionalBinaryBothMergingAttribute>())
                .Where(p => !p.HasAttribute<UseExceptionalBinaryEitherMergingAttribute>())
                ;

            foreach (var prop in latterOrientedProps)
            {
                var mergingArticleInfoProp = prop.GetValue(mergingArticle);

                if (prop.PropertyType == typeof(string))
                {
                    if (mergingArticleInfoProp?.ToString().NullIfNullOrEmpty() != null)
                        prop.SetValue(this, mergingArticleInfoProp);

                }
                else
                {
                    if (mergingArticleInfoProp != null)
                        prop.SetValue(this, mergingArticleInfoProp);

                }

            }
        }


    }


    // ★★★★★★★★★★★★★★★ method (practical use)

    public async Task<bool> TryDownloadPDF(DirectoryInfo pdfStockDirectory)
    {
        UtilPreprocessors.PreprocessBasic();

        try
        {
            var outputFile = pdfStockDirectory.GetChildFileInfo(LocalPDFName);
            await new Uri(PDF_Link!).DownloadFileAsync(outputFile, true);

            return true;
        }
        catch (Exception ex)
        {
            ConsoleExtension.WriteLineWithColor($"Failed: {ex.Message}", ConsoleColor.Red);
            return false;
        }
    }

    public async Task<(bool download, bool open)> TryOpenPDF(DirectoryInfo pdfStockDirectory)
    {
        UtilPreprocessors.PreprocessBasic();
        var outputFilePath = Path.Combine(pdfStockDirectory.FullName, LocalPDFName);

        if (!File.Exists(outputFilePath))
            if (!await TryDownloadPDF(pdfStockDirectory))
                return (false, false);

        try
        {
            var p = Process.Start(new ProcessStartInfo()
            {
                FileName = outputFilePath,
                UseShellExecute = true,
            });

            return (true, true);
        }
        catch (Exception ex)
        {
            ConsoleExtension.WriteLineWithColor($"Failed: {ex.Message}", ConsoleColor.Red);
            return (true, false);
        }
    }

    public bool TryFindPDF(DirectoryInfo pdfStockDirectory)
    {
        UtilPreprocessors.PreprocessBasic();

        var outputFilePath = Path.Combine(pdfStockDirectory.FullName, LocalPDFName);
        return File.Exists(outputFilePath);
    }

    public bool TryOpenDOILink()
    {
        UtilPreprocessors.PreprocessBasic();

        try
        {
            var p = Process.Start(new ProcessStartInfo
            {
                FileName = DOI_Link!,
                UseShellExecute = true,
            });
            return true;
        }
        catch (Exception ex)
        {
            ConsoleExtension.WriteLineWithColor($"Failed: {ex.Message}", ConsoleColor.Red);
            return false;
        }
    }

    /// <summary>
    /// Predict meta info from Unstructured reference string by ChatGPT
    /// </summary>
    /// <returns></returns>
    public bool TryPredictMetaInfo_ChatGPT()
    {
        UtilPreprocessors.PreprocessBasic();

        try
        {
            var refString = ReferenceString;

            if (refString is null || refString == ReferenceErrorReasonString)
                throw new Exception("文献文字列がないため，推測が出来ません。");











            throw new NotImplementedException("未実装です…。");

            return true;
        }
        catch (Exception ex)
        {
            ConsoleExtension.WriteLineWithColor($"Failed: {ex.Message}", ConsoleColor.Red);
            return false;
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


    // ★★★★★★★★★★★★★★★ attributes

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    internal sealed class UseExceptionalMergingAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    internal sealed class UseExceptionalBinaryEitherMergingAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    internal sealed class UseExceptionalBinaryBothMergingAttribute : Attribute
    {
    }


    // ★★★★★★★★★★★★★★★ 

}
