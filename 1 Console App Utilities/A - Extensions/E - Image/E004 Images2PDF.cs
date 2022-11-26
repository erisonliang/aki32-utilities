﻿using Aki32_Utilities.UsefulClasses;

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
    /// <param name="outputFile">when null, automatically set</param>
    /// <returns></returns>
    public static FileInfo Images2PDF(this DirectoryInfo inputDir, FileInfo? outputFile)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, true, inputDir!, "output.pdf", takesTimeFlag: true);
        if (!outputFile!.Name.EndsWith(".pdf"))
            throw new Exception("outputFile name must end with .pdf");


        // main
        var pngFIs = inputDir
            .GetFilesWithRegexen(SearchOption.TopDirectoryOnly, GetImageFilesRegexen())
            .Sort()
            .ToArray();

        using var doc = new Document();
        PdfWriter.GetInstance(doc, new FileStream(outputFile.FullName, FileMode.Create));

        var maxCount = pngFIs.Length;
        var progress = new ProgressManager(maxCount);
        progress.StartAutoWrite();

        for (int i = 0; i < maxCount; i++)
        {
            var pngFI = pngFIs[i];
            var png = iTextSharp.text.Image.GetInstance(pngFI.FullName);

            doc.SetPageSize(png);

            if (pngFIs.First() == pngFI)
                doc.Open();
            else
                doc.NewPage();

            png.ScaleToFit(doc.PageSize);
            png.SetAbsolutePosition(0, 0);
            doc.Add(png);

            progress.CurrentStep = i;
        }

        if (doc.IsOpen())
            doc.Close();

        progress.WriteDone();


        // post process
        return outputFile!;
    }

    // ★★★★★★★★★★★★★★★ 

}
