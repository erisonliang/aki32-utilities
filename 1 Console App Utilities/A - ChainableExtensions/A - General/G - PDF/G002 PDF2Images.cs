using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

using Newtonsoft.Json.Serialization;

namespace Aki32_Utilities.Console_App_Utilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// Images2PDF (currently, png to pdf only)
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <returns></returns>
    public static DirectoryInfo PDF2Images(this FileInfo inputFile, DirectoryInfo? outputDir)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, inputFile.Directory!, takesTimeFlag: true);
        if (!inputFile.Name.EndsWith(".pdf"))
            throw new Exception("inputFile name must end with .pdf");


        // main
        throw new NotImplementedException();


        // post process
        return outputDir!;
    }

}
