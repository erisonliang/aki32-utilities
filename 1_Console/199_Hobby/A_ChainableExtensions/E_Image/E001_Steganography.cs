using System.Drawing;
using System.Drawing.Imaging;

using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.Hobby;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ chainable

    /// <summary>
    /// Hide image in image
    /// </summary>
    /// <param name="inputImageFileToShow"></param>
    /// <param name="inputImageFileToHide"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <param name="bits">Using bits per color</param>
    /// <returns></returns>
    public static FileInfo SteganographyEncryptImage(this FileInfo inputImageFileToShow, FileInfo inputImageFileToHide, FileInfo? outputFile, int bits = 2, bool resizeHidingImageWhenTooBig = true)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputImageFileToShow.Directory!, inputImageFileToShow.Name);


        // main
        using var inputImageToShow = inputImageFileToShow.GetImageFromFile();
        var inputImageToHide = inputImageFileToHide.GetImageFromFile();
        if (resizeHidingImageWhenTooBig)
        {
            var ratio = Math.Max((double)inputImageToHide.Width / inputImageToShow.Width, (double)inputImageToHide.Height / inputImageToShow.Height);
            if (ratio > 1)
                inputImageToHide = inputImageToHide.ResizeImage(new Size((int)(inputImageToHide.Width / ratio), (int)(inputImageToHide.Height / ratio)));
        }
        var outputImage = SteganographyEncryptImage((Bitmap)inputImageToShow, (Bitmap)inputImageToHide, bits, resizeHidingImageWhenTooBig);
        outputImage.Save(outputFile!.FullName, ImageFormat.Png);
        inputImageToHide.Dispose();
        GC.Collect();


        // post process
        return outputFile!;
    }

    /// <summary>
    /// Find hidden image in image
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="inputImageFileToHide"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <param name="bits">Using bits per color</param>
    /// <returns></returns>
    public static FileInfo SteganographyDecryptImage(this FileInfo inputFile, FileInfo? outputFile, int bits = 2)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputFile.Directory!, inputFile.Name);


        // main
        using var inputImage = inputFile.GetImageFromFile();
        var outputImage = SteganographyDecryptImage((Bitmap)inputImage, bits);
        outputImage.Save(outputFile!.FullName, ImageFormat.Png);
        GC.Collect();


        // post process
        return outputFile!;
    }


    // ★★★★★★★★★★★★★★★ image process

    public static Bitmap SteganographyEncryptImage(Bitmap imgShow, Bitmap imgHide, int bits = 2, bool resizeHidingImageWhenTooBig = true)
    {
        var result = (Bitmap)imgShow.Clone();

        for (int y = 0; y < Math.Min(imgShow.Height, imgHide.Height); y++)
        {
            for (int x = 0; x < Math.Min(imgShow.Width, imgHide.Width); x++)
            {
                try
                {
                    var v_color = imgShow.GetPixel(x, y);
                    var iv_color = imgHide.GetPixel(x, y);

                    var A = v_color.A / bits * bits + (iv_color.A + 0) * bits / 256;
                    if (A > 255)
                        A -= bits;
                    var R = v_color.R / bits * bits + (iv_color.R + 0) * bits / 256;
                    if (R > 255)
                        R -= bits;
                    var G = v_color.G / bits * bits + (iv_color.G + 0) * bits / 256;
                    if (G > 255)
                        G -= bits;
                    var B = v_color.B / bits * bits + (iv_color.B + 0) * bits / 256;
                    if (B > 255)
                        B -= bits;

                    result.SetPixel(x, y, Color.FromArgb(A, R, G, B));

                }
                catch (Exception ex)
                {
                }
            }
        }

        return result;

    }

    public static Bitmap SteganographyDecryptImage(Bitmap source, int bits = 2)
    {
        var result = (Bitmap)source.Clone();

        for (int y = 0; y < source.Height; y++)
        {
            for (int x = 0; x < source.Width; x++)
            {
                var color = source.GetPixel(x, y);

                var A = color.A % bits;
                var R = color.R % bits;
                var G = color.G % bits;
                var B = color.B % bits;

                var coef = (float)bits / (bits - 1);
                result.SetPixel(x, y, Color.FromArgb(
                    (int)(coef * A * 255 / bits),
                    (int)(coef * R * 255 / bits),
                    (int)(coef * G * 255 / bits),
                    (int)(coef * B * 255 / bits)
                    ));
            }
        }

        return result;
    }


    // ★★★★★★★★★★★★★★★

}
