using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.ConsoleAppUtilities.AI;
using Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
using Aki32Utilities.ConsoleAppUtilities.Research;

using ClosedXML;

using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using NumSharp;

using Newtonsoft.Json;
using Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
using iTextSharp.text.pdf.codec.wmf;
using MathNet.Numerics.LinearAlgebra;
using ICSharpCode.SharpZipLib.Zip;

using Thickness = Aki32Utilities.ConsoleAppUtilities.General.ChainableExtensions.Thickness;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;
using DocumentFormat.OpenXml.Bibliography;
using System.Xml.Linq;
using XPlot.Plotly;
using DocumentFormat.OpenXml.Drawing;
using Org.BouncyCastle.Crypto.Macs;
using NumSharp.Utilities;
using DocumentFormat.OpenXml.Office2013.Excel;
using MathNet.Numerics;
using MathNet.Spatial.Euclidean;
using System.Globalization;
using Aki32Utilities.ConsoleAppUtilities.Hobby;
using ClosedXML.Excel;

namespace Aki32Utilities.Apps.ConsoleMiniApps;
public static partial class MiniAppsExecuter
{
    static readonly DirectoryInfo BASE_DIR = new($@"..\..\..\# SampleData\");

    public static void All()
    {
        // TEST
        {


        }

        // M_AllMiniApps
        {
            var baseDir_M = BASE_DIR.GetChildDirectoryInfo($@"M_AllMiniApps");

            // M001_Books2PDF
            {
                //var outputDir = baseDir_M.GetChildDirectoryInfo($@"M001_Books2PDF");
                //MiniApps.Books2PDF(outputDir, 10);

            }

            // M002_CropBookPDF
            {
                //var input = new FileInfo(@"C:\Users\aki32\Dropbox\Documents\13 読み物\0 アーカイブ\01 建築\04 構造\11 S\『日本材料学会編・疲労設計便覧1995』.pdf");
                //MiniApps.DividePDFPages(input, DPI: 200);

                ////var input = new DirectoryInfo(@"C:\Users\aki32\Dropbox\Documents\13 読み物\0 アーカイブ\01 建築\04 構造\11 S\output_PDF2Images_35A4F9\output_CropImageF_FA4F33");
                //// input.Images2PDF(null);

            }

            //
            {



            }

        }

    }
}
