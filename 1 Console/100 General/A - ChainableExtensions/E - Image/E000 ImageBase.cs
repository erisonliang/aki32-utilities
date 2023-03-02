using System.Drawing.Imaging;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ FileSystemInfo chain process

    public static ImageFormat DecideImageFormatIfNull(this ImageFormat? imageFormat, FileInfo? inputFile)
    {
        if (imageFormat != null)
            return imageFormat;

        if (inputFile == null)
            return ImageFormat.Png;

        var extension = Path.GetExtension(inputFile.Name).ToLower();

        switch (extension)
        {
            case ".png":
                return ImageFormat.Png;

            case ".jpg":
            case ".jpeg":
                return ImageFormat.Jpeg;

            case ".bmp":
                return ImageFormat.Bmp;

            case ".emf":
                return ImageFormat.Emf;

            case ".exif":
                return ImageFormat.Exif;

            case ".gif":
                return ImageFormat.Gif;

            case ".ico":
            case ".icon":
                return ImageFormat.Icon;

            case ".mbmp":
                return ImageFormat.MemoryBmp;

            case ".tiff":
                return ImageFormat.Tiff;

            case ".wmf":
                return ImageFormat.Wmf;

            default:
                break;
        }

        return imageFormat!;
    }

    /// <summary>
    /// Guess and return file extension from ImageFormat
    /// </summary>
    /// <param name="imageFormat"></param>
    /// <returns></returns>
    public static string GetExtension(this ImageFormat imageFormat)
    {
        return $".{imageFormat.ToString().ToLower()}";
    }


    // ★★★★★★★★★★★★★★★

}
