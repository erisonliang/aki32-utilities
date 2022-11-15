using System.Drawing;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set to {inputFile.DirectoryName}/output_CropImage/{inputFile.Name} - {crop.ToString()}.png</param>
    /// <param name="crop"></param>a
    /// <returns></returns>
    public static FileInfo CropImage(this FileInfo inputFile, FileInfo? outputFile, CropSize crop)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, false, inputFile.Directory!, $"{Path.GetFileNameWithoutExtension(inputFile.Name)} - {crop.ToString()}.png");

        // main
        try
        {
            using (var inputImg = Image.FromFile(inputFile.FullName))
            {
                var img = CropImage(inputImg, crop);
                img.Save(outputFile!.FullName);
            }
            if (UtilConfig.ConsoleOutput)
                Console.WriteLine($"O: {inputFile.FullName}");
        }
        catch (Exception e)
        {
            if (UtilConfig.ConsoleOutput)
                Console.WriteLine($"X: {inputFile.FullName}, {e.Message}");
        }


        // post process
        return outputFile!;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.Fullname}/output_CropImage/{inputFile.Name}.png</param>
    /// <param name="crop"></param>
    /// <returns></returns>
    public static DirectoryInfo CropImage(this FileInfo inputFile, DirectoryInfo? outputDir, CropSize[] crops)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, false, inputFile.Directory!);


        // main
        foreach (var crop in crops)
        {
            var outputFile = new FileInfo(Path.Combine(outputDir!.FullName, $"{Path.GetFileNameWithoutExtension(inputFile.Name)} - {crop.ToString()}.png"));
            inputFile.CropImage(outputFile, crop);
        }


        return outputDir!;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set to {inputDir.Fullname}/output_CropImage/{inputFile.Name}.png</param>
    /// <param name="crop"></param>
    /// <returns></returns>
    public static DirectoryInfo CropImage_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, CropSize crop)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutDir(ref outputDir, true, inputDir!);


        // main
        foreach (var inputFile in inputDir.GetFiles())
        {
            var outputFile = new FileInfo(Path.Combine(outputDir!.FullName, inputFile.Name));
            inputFile.CropImage(outputFile, crop);
        }


        // post process
        return outputDir!;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="crop"></param>
    /// <returns></returns>
    public static Image CropImage(this FileInfo inputFile, CropSize crop)
    {
        using var inputImg = Image.FromFile(inputFile.FullName);
        return CropImage(inputImg, crop);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputImg"></param>
    /// <param name="crop"></param>
    /// <returns></returns>
    public static Image CropImage(this Image inputImg, CropSize crop)
    {
        var outputBmp = ((Bitmap)inputImg).Clone(crop.GetImageCropRect(inputImg), inputImg.PixelFormat);
        return outputBmp;
    }

    /// <summary>
    /// 
    /// </summary>
    public class CropSize
    {
        public double l { get; set; }
        public double t { get; set; }
        public double r { get; set; }
        public double b { get; set; }

        public CropSize(double l, double t, double r, double b)
        {
            if (l < 0 && 1 < l)
            {
                throw new InvalidDataException("l, t, r, b must be in range [0.0, 1.0]");
            }

            this.l = l;
            this.t = t;
            this.r = r;
            this.b = b;
        }

        public Rectangle GetImageCropRect(Image img)
        {
            var cx = (int)(l * img.Width);
            var cy = (int)(t * img.Height);
            var cw = (int)((1 - l - r) * img.Width);
            var ch = (int)((1 - t - b) * img.Height);

            return new Rectangle(cx, cy, cw, ch);
        }

        public new string ToString() => $"{l:F2},{t:F2},{r:F2},{b:F2}";

    }

}
