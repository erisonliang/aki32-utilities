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

    /// <summary>
    /// Imgs2PDF
    ///  - Currently png only
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.FullName}/output_TEMPLATE</param>
    /// <param name="skipColumnCount"></param>
    /// <param name="skipRowCount"></param>
    /// <param name="header"></param>
    /// <returns></returns>
    public static FileInfo Imgs2PDF(this DirectoryInfo inputDir, FileInfo? outputFile)
    {
        // preprocess
        if (UtilConfig.ConsoleOutput) Console.WriteLine("\r\n(This takes time...)");
        UtilPreprocessors.PreprocessOutFile(ref outputFile, true, inputDir!, "output.pdf");
        if (!outputFile!.Name.EndsWith(".pdf"))
            throw new Exception("outputFile name must end with .pdf");


        // main
        using var doc = new Document();
        PdfWriter.GetInstance(doc, new FileStream(outputFile.FullName, FileMode.Create));

        var pngFIs = inputDir
            .GetFiles("*.png", SearchOption.TopDirectoryOnly)
            .Sort()
            .ToArray();

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

}
