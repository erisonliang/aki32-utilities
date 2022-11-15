using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    /// <summary>
    /// Imgs2PDF
    /// png only
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.FullName}/output_TEMPLATE</param>
    /// <param name="skipColumnCount"></param>
    /// <param name="skipRowCount"></param>
    /// <param name="header"></param>
    /// <returns></returns>
    public static DirectoryInfo Imgs2PDF(this DirectoryInfo inputDir, DirectoryInfo? outputDir, int skipColumnCount = 0, int skipRowCount = 0, string? header = null)
    {
        // preprocess
        if (UtilConfig.ConsoleOutput) Console.WriteLine("\r\n(This takes time...)");
        UtilPreprocessors.PreprocessOutDir(ref outputDir, true, inputDir);


        // main
        var pngs = inputDir
            .GetFiles("*.png", SearchOption.TopDirectoryOnly)
            .Sort()
            .ToArray();




        // post process
        return outputDir!;
    }









    // ★★★★★★★★★★★★★★★






    //private async void Button_Start_Pngs2PDF_Click(object sender, EventArgs e)
    //{
    //    var dialogResult = MessageBox.Show("時間がかかりますが実行しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
    //    if (DialogResult.Yes != dialogResult)
    //        return;

    //    try
    //    {
    //        Inform("★★★★★ 処理開始（しばらくお待ちください）");
    //        Button_Start_Pngs2PDF.Enabled = false;
    //        Button_Start_Pngs2PDF.Text = "処理中…";
    //        Button_Start.Enabled = false;

    //        await Task.Run(() => CreatePDF(SaveDirPath));
    //    }
    //    finally
    //    {
    //        Inform("★★★★★ 処理終了");
    //        Button_Start_Pngs2PDF.Enabled = true;
    //        Button_Start_Pngs2PDF.Text = "全画像 → PDF";
    //        Button_Start.Enabled = true;
    //    }
    //}

    //public static void CreatePDF(string targetDir)
    //{
    //    var savePdfPath = Path.Combine(targetDir, "merged.pdf");

    //    using (var doc = new Document())
    //    {
    //        PdfWriter.GetInstance(doc, new FileStream(savePdfPath, FileMode.Create));

    //        var pngPaths = Directory.GetFiles(targetDir).Where(x => x.EndsWith(".png"));

    //        foreach (var pngPath in pngPaths)
    //        {
    //            var png = iTextSharp.text.Image.GetInstance(pngPath);

    //            doc.SetPageSize(png);

    //            if (pngPaths.First() == pngPath)
    //                doc.Open();
    //            else
    //                doc.NewPage();

    //            png.ScaleToFit(doc.PageSize);
    //            png.SetAbsolutePosition(0, 0);
    //            doc.Add(png);
    //        }

    //        doc.Close();
    //    }
    //}






}
