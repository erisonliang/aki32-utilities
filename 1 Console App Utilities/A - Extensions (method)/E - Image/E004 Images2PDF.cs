using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    // ★★★★★★★★★★★★★★★ FileSystemInfo chain process

    /// <summary>
    /// Images2PDF (currently, png to pdf only)
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set to {inputDir.FullName}/output_Images2PDF/output.pdf</param>
    /// <returns></returns>
    public static FileInfo Images2PDF(this DirectoryInfo inputDir, FileInfo? outputFile)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, true, inputDir!, "output.pdf", takesTimeFlag: true);
        if (!outputFile!.Name.EndsWith(".pdf"))
            throw new Exception("outputFile name must end with .pdf");


        // main
        var pngFIs = inputDir
            .GetFilesWithRegexen(SearchOption.TopDirectoryOnly, GetImageFilesRegexen(png: true, jpg: true, bmp: true))
            .Sort()
            .ToArray();

        using var doc = new Document();
        PdfWriter.GetInstance(doc, new FileStream(outputFile.FullName, FileMode.Create));

        foreach (var pngFI in pngFIs)
        {
            var png = iTextSharp.text.Image.GetInstance(pngFI.FullName);

            doc.SetPageSize(png);

            if (pngFIs.First() == pngFI)
                doc.Open();
            else
                doc.NewPage();

            png.ScaleToFit(doc.PageSize);
            png.SetAbsolutePosition(0, 0);
            doc.Add(png);
        }

        if (doc.IsOpen())
            doc.Close();


        // post process
        return outputFile!;
    }

    // ★★★★★★★★★★★★★★★ 

}
