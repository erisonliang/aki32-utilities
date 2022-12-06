﻿using System.Diagnostics;

using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.ConsoleAppUtilities.UsefulClasses;

using ClosedXML;

namespace Aki32Utilities.ConsoleAppUtilities.SpecificPurposeModels.Research;
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

                // 英語は後回し
                ?? CrossRef_ArticleTitle.NullIfNullOrEmpty()
                ?? JStage_ArticleTitle_English.NullIfNullOrEmpty()

                // 最終手段。
                ?? ((UnstructuredRefString.NullIfNullOrEmpty() == null) ? null : UnstructuredRefString!.Shorten(UNSTRUCTURED_REF_STRING_RANGE))
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

                // 英語は後回し
                ?? CrossRef_Authors
                ?? JStage_Authors_English

                // 最終手段。
                ?? ((UnstructuredRefString.NullIfNullOrEmpty() == null) ? null : new string[] { UnstructuredRefString!.Shorten(UNSTRUCTURED_REF_STRING_RANGE) })
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

                // 英語は後回し


                // 最終手段。
                ?? null
                ;
        }
    }

    public string? DOI { get; set; }
    public string[]? ReferenceDOIs { get; set; }
    public string? UnstructuredRefString { get; set; }

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

    public string? Manual_CreatedDate { get; set; }

    public string? Memo { get; set; }


    // ★★★★★ CrossRef

    public string? CrossRef_ArticleTitle { get; set; }
    public string[]? CrossRef_Authors { get; set; }

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



    // ★★★★★ mainly from CiNii

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

        string? title = null,
        string[]? authors = null,

        ResearchArticle[]? references = null,

        FileInfo? pdfFile = null,
        DirectoryInfo? pdfStockDirectory = null,

        bool movePdfFile = true
        )
    {
        // Create AOI first
        var aoi = Guid.NewGuid().ToString();

        // stock pdf file to database
        if (pdfFile != null)
        {
            if (pdfStockDirectory == null)
                throw new InvalidDataException("When tring initializing {pdfFile}, {pdfStockDirectory} must not be null");

            var targetFile = new FileInfo(Path.Combine(pdfStockDirectory.FullName, $"{aoi}.pdf"));

            if (movePdfFile)
                pdfFile.MoveTo(targetFile);
            else
                pdfFile.CopyTo(targetFile);

        }

        var raddingArticle = new ResearchArticle()
        {
            Manual_ArticleTitle = title,
            Manual_Authors = authors,

            Manual_CreatedDate = DateTime.Today.ToLongDateString(),

            AOI = aoi,

            DataFrom_Manual = true,
        };

        if (references != null)
        {
            foreach (var reference in references)
                reference.AddArticleReference(raddingArticle);

        }

        return raddingArticle;
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

            var outputFile = new FileInfo(Path.Combine(pdfStockDirectory.FullName, $"{LocalPDFName}.pdf"));
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
        var power = (int)Math.Pow(2, 10);

        if (!string.IsNullOrEmpty(DOI) && !string.IsNullOrEmpty(comparingArticle.DOI))
        {
            var com = DOI!.CompareTo(comparingArticle.DOI);
            if (com == 0) return 0;
            result += power * Math.Sign(com);
        }

        power /= 2;

        if (!string.IsNullOrEmpty(CrossRef_ArticleTitle) && !string.IsNullOrEmpty(comparingArticle.CrossRef_ArticleTitle))
        {
            var com = CrossRef_ArticleTitle!.CompareTo(comparingArticle.CrossRef_ArticleTitle);
            if (com == 0) return 0;
            result += power * Math.Sign(com);
        }

        power /= 2;

        if (!string.IsNullOrEmpty(JStage_Id) && !string.IsNullOrEmpty(comparingArticle.JStage_Id))
        {
            var com = JStage_Id!.CompareTo(comparingArticle.JStage_Id);
            if (com == 0) return 0;
            result += power * Math.Sign(com);
        }

        power /= 2;

        if (!string.IsNullOrEmpty(Manual_ArticleTitle) && !string.IsNullOrEmpty(comparingArticle.Manual_ArticleTitle))
        {
            var com = Manual_ArticleTitle!.CompareTo(comparingArticle.Manual_ArticleTitle);
            if (com == 0) return 0;
            result += power * Math.Sign(com);
        }

        power /= 2;

        if (!string.IsNullOrEmpty(AOI) && !string.IsNullOrEmpty(comparingArticle.AOI))
        {
            var com = AOI!.CompareTo(comparingArticle.AOI);
            if (com == 0) return 0;
            result += power * Math.Sign(com);
        }

        power /= 2;

        if (!string.IsNullOrEmpty(UnstructuredRefString) && !string.IsNullOrEmpty(comparingArticle.UnstructuredRefString))
        {
            var com = UnstructuredRefString!.CompareTo(comparingArticle.UnstructuredRefString);
            if (com == 0) return 0;
            result += power * Math.Sign(com);
        }

        return (result == 0) ? (GetHashCode() - comparingArticle.GetHashCode()) : result;
    }


    // ★★★★★★★★★★★★★★★ 

}
