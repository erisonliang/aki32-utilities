

namespace Aki32Utilities.ConsoleAppUtilities.General;
/// <summary>
/// Unified default classes for handling each objects.
/// </summary>
public class UnifiedDefaultTypesDeclaration
{

    // ★★★★★★★★★★★★★★★ basic types
    public static Type File => typeof(System.IO.FileInfo);
    public static Type Directory => typeof(System.IO.DirectoryInfo);
    public static Type Uri => typeof(System.Uri);
    public static Type Image => typeof(System.Drawing.Image);
    public static Type Sound => typeof(MediaToolkit.Model.MediaFile);
    public static Type Video => typeof(OpenCvSharp.VideoCapture);
    public static Type Excel => typeof(ClosedXML.Excel.IXLWorkbook);
    public static Type DrawingPoint => typeof(System.Drawing.PointF);


    // ★★★★★★★★★★★★★★★ applied types
    public static Type Margins => typeof(Aki32Utilities.ConsoleAppUtilities.General.ChainableExtensions.Thickness);


    // ★★★★★★★★★★★★★★★ numerical calculation types
    public static Type Point2D => typeof(MathNet.Spatial.Euclidean.Point2D);
    public static Type Point3D => typeof(MathNet.Spatial.Euclidean.Point3D);
    public static Type Vector => typeof(MathNet.Numerics.LinearAlgebra.Double.DenseVector);
    public static Type Matrix => typeof(MathNet.Numerics.LinearAlgebra.Double.DenseMatrix);
    public static Type TimeHistory => typeof(Aki32Utilities.ConsoleAppUtilities.General.TimeHistory);


    // ★★★★★★★★★★★★★★★ 

}