using System.Drawing;

using Aki32Utilities.ConsoleAppUtilities.General;

using General = Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.UsageExamples.ConsoleAppUtilities;
public partial class MiniApps
{
    public enum CropBookPDFType
    {
        _2in1,
        _4in1,
    }

    /// <summary>
    /// Make 2in1 page pdf to 1in1 page pdf
    /// </summary>
    public static void CropBookPDF(FileInfo inputFile,
        int DPI = 300,
        CropBookPDFType type = CropBookPDFType._2in1
        )
    {
        // pre process
        ConsoleExtension.WriteLineWithColor("\r\n★★★★★★★★★★★★★★★ 処理開始（時間がかかります）\r\n");


        // main
        ChainableExtensions.Thickness[] crops;

        switch (type)
        {
            case CropBookPDFType._2in1:
                {
                    crops = new ChainableExtensions.Thickness[]
                    {
                        new ChainableExtensions.Thickness(0, 0, 0.5, 0) { Name = "1" },
                        new ChainableExtensions.Thickness(0.5, 0, 0, 0) { Name = "2" },
                    };
                }
                break;
            case CropBookPDFType._4in1:
                {
                    crops = new ChainableExtensions.Thickness[]
                    {
                        new ChainableExtensions.Thickness(0, 0, 0.5, 0.5) { Name = "1" },
                        new ChainableExtensions.Thickness(0.5, 0, 0, 0.5) { Name = "2" },
                        new ChainableExtensions.Thickness(0, 0.5, 0.5, 0) { Name = "3" },
                        new ChainableExtensions.Thickness(0.5, 0.5, 0, 0) { Name = "4" },
                    };
                }
                break;
            default:
                throw new NotImplementedException();
        }

        inputFile
            .PDF2Images(null, DPI: DPI)
            .CropImageForMany_Loop(null, crops)
            .Images2PDF(null);


        // post process
        ConsoleExtension.WriteLineWithColor("\r\n★★★★★★★★★★★★★★★ 以上\r\n");

    }

}