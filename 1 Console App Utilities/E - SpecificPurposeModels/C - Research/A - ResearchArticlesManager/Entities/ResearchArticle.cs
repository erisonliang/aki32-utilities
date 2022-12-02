using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Aki32_Utilities.General;

using ClosedXML;

using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Ink;

namespace Aki32_Utilities.SpecificPurposeModels.Research;
public class ResearchArticle : IComparable
{

    // ★★★★★★★★★★★★★★★ prop

    // ★★★★★ shared info (*main common info)

    public string? ArticleTitle
    {
        get
        {
            return null
                ?? Manual_ArticleTitle.NullIfNullOrEmpty()
                ?? JStage_ArticleTitle_Japanese.NullIfNullOrEmpty()
                ?? CrossRef_ArticleTitle.NullIfNullOrEmpty()
                ?? JStage_ArticleTitle_English.NullIfNullOrEmpty()

                // 最終手段。
                ?? ((UnstructuredRefString.NullIfNullOrEmpty() == null) ? null : UnstructuredRefString!.Shorten(..30))
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
                ?? CrossRef_Authors
                ?? JStage_Authors_English

                // 最終手段。
                ?? ((UnstructuredRefString.NullIfNullOrEmpty() == null) ? null : new string[] { UnstructuredRefString!.Shorten(..30) })
                ?? null
                ;
        }
    }

    public string? DOI { get; set; }
    public string[]? ReferenceDOIs { get; set; }

    private string? __UnstructuredRefString;
    public string? UnstructuredRefString
    {
        get
        {
            return __UnstructuredRefString;
        }
        set
        {
            __UnstructuredRefString = value;
        }
    }

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
                return JStage_Link?.Replace($"_article/-char/ja/", $"_pdf");
            }

            return null;
        }
    }

    public IEnumerable<string> LocalPDFNameCandidates
    {
        get
        {
            if (!string.IsNullOrEmpty(AOI))
                yield return AOI;

            if (!string.IsNullOrEmpty(DOI))
                yield return DOI.Replace("/", "_");

            if (!string.IsNullOrEmpty(PDF_Link))
            {
                var candidate = PDF_Link.Replace("/", "_").Replace(":", "_");
                yield return candidate.Shorten(^29..);
            }

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
    /// When DOI does not exist, automatically create AOI to connect ref data.
    /// 
    /// Put your pdf in {LocalPath}\PDFs\Manual\{ManuallyAddedPdfName}.pdf
    /// </summary>
    /// <remarks>
    /// AOIで接続するのは，本当に最終手段。
    /// </remarks>
    public string? AOI { get; set; }


    // ★★★★★ manual info

    public string? Manual_ArticleTitle { get; set; }
    public string[]? Manual_Authors { get; set; }




    // ★★★★★ CrossRef


    public string? CrossRef_ArticleTitle { get; set; }
    public string[]? CrossRef_Authors { get; set; }

    public string? CrossRef_PublishedDate { get; set; }




    // ★★★★★ mainly from J-Stage

    public string? JStage_ArticleTitle_English { get; set; }
    public string? JStage_ArticleTitle_Japanese { get; set; }

    public string? JStage_Link_English { get; set; }
    public string? JStage_Link_Japanese { get; set; }

    public string[]? JStage_Authors_English { get; set; }
    public string[]? JStage_Authors_Japanese { get; set; }

    public string? JStage_JournalCode { get; set; }

    public string? JStage_MaterialTitle_English { get; set; }
    public string? JStage_MaterialTitle_Japanese { get; set; }


    public string? JStage_Volume { get; set; }
    public string? JStage_SubVolume { get; set; }

    public string? JStage_Number { get; set; }
    public string? JStage_StartingPage { get; set; }
    public string? JStage_EndingPage { get; set; }

    public string? JStage_PublishedYear { get; set; }

    public string? JStage_JOI { get; set; }

    public string? JStage_SystemCode { get; set; }
    public string? JStage_SystemName { get; set; }

    public string? JStage_Title { get; set; }
    public string? JStage_Link { get; set; }
    public string? JStage_Id { get; set; }
    public string? JStage_UpdatedOn { get; set; }




    // ★★★★★ mainly from CiNii









    // ★★★★★★★★★★★★★★★ method

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
        // AOI creation
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

    public void AddArticleReference(ResearchArticle addingArticle)
    {
        // Add DOI or AOI to ReferenceDOIs.
        ReferenceDOIs ??= Array.Empty<string>();

        ReferenceDOIs = ReferenceDOIs
            .Append(addingArticle.DOI ?? (addingArticle.AOI ??= Guid.NewGuid().ToString()))!
            .Distinct()
            .ToArray()
            ;

    }


    // ★★★★★★★★★★★★★★★ helper

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
