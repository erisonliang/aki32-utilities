using IronPdf;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// PDF2Images<br/>
    /// sugar of https://ironpdf.com/examples/rasterize-a-pdf-to-images/
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <returns></returns>
    public static DirectoryInfo PDF2Images(this FileInfo inputFile, DirectoryInfo? outputDir,
        string outputExtension = ".jpg",
        int DPI = 300

        )
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, inputFile.Directory!, takesTimeFlag: true);
        if (!inputFile.Name.EndsWith(".pdf"))
            throw new Exception("inputFile name must end with .pdf");


        // main
        var pdf = PdfDocument.FromFile(inputFile.FullName);
        pdf.RasterizeToImageFiles(outputDir!.GetChildFileInfo(@$"*{outputExtension}").FullName, DPI: DPI);


        // post process
        return outputDir!;
    }

}
