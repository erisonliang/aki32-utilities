using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aki32_Utilities.OverridingUtils;
public static class ImageUtil
{

    // ★★★★★★★★★★★★★★★ 311 CropImage

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
        if (outputFile == null)
            outputFile = new FileInfo(Path.Combine(inputFile.DirectoryName, "output_CropImage", $"{Path.GetFileNameWithoutExtension(inputFile.Name)} - {crop.ToString()}.png"));
        if (!outputFile.Directory.Exists)
            outputFile.Directory.Create();

        // main
        try
        {
            using (var inputImg = Image.FromFile(inputFile.FullName))
            {
                var img = CropImage(inputImg, crop);
                img.Save(outputFile.FullName);
            }
            if (UtilConfig.ConsoleOutput)
                Console.WriteLine($"O 成功 : {inputFile.FullName}");
        }
        catch (Exception e)
        {
            if (UtilConfig.ConsoleOutput)
                Console.WriteLine($"X 失敗 : {inputFile.FullName}, {e.Message}");
        }

        return outputFile;
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
        if (UtilConfig.ConsoleOutput)
            Console.WriteLine("\r\n** CropImage_Loop() Called");
        if (outputDir == null)
            outputDir = new DirectoryInfo(Path.Combine(inputDir.FullName, "output_CropImage"));
        if (!outputDir.Exists)
            outputDir.Create();

        // main
        foreach (var inputFile in inputDir.GetFiles())
        {
            var outputFile = new FileInfo(Path.Combine(outputDir.FullName, inputFile.Name));
            inputFile.CropImage(outputFile, crop);
        }

        return outputDir;
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

        public string ToString() => $"{l:F2},{t:F2},{r:F2},{b:F2}";

    }

    // ★★★★★★★★★★★★★★★ 312 ConvertImageColorAndSave

    /// <summary>
    /// Convert image color to targetColor and save
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set to {inputFile.DirectoryName}/output_ConvertImageColor/{targetColor.Name}.png</param>
    /// <param name="targetColor"></param>
    /// <returns></returns>
    public static FileInfo ConvertImageColor(this FileInfo inputFile, FileInfo? outputFile, Color targetColor)
    {
        // preprocess
        if (outputFile == null)
            outputFile = new FileInfo(Path.Combine(inputFile.DirectoryName, "output_ConvertImageColor", inputFile.Name));
        if (!outputFile.Directory.Exists)
            outputFile.Directory.Create();

        // main
        try
        {
            using (var inputImg = Image.FromFile(inputFile.FullName))
            {
                var img = ConvertImageColor(inputImg, targetColor);
                img.Save(outputFile.FullName, ImageFormat.Png);
            }
            if (UtilConfig.ConsoleOutput)
                Console.WriteLine($"O 成功 : {inputFile.FullName}");
        }
        catch (Exception e)
        {
            if (UtilConfig.ConsoleOutput)
                Console.WriteLine($"X 失敗 : {inputFile.FullName}, {e.Message}");
        }

        return outputFile;
    }
    /// <summary>
    /// Convert image color to targetColor and save
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputDir">when null, automatically set to {inputFile.DirectoryName}/output_ConvertImageColor/{fileName}.png</param>
    /// <param name="targetInfos"></param>
    /// <returns></returns>
    public static DirectoryInfo ConvertImageColor_Loop(this FileInfo inputFile, DirectoryInfo? outputDir, params (string fileName, Color targetColor)[] targetInfos)
    {
        // preprocess
        if (UtilConfig.ConsoleOutput)
            Console.WriteLine("\r\n** ConvertImageColor_Loop() Called");
        if (outputDir == null)
            outputDir = new DirectoryInfo(Path.Combine(inputFile.DirectoryName, "output_ConvertImageColor"));
        if (!outputDir.Exists)
            outputDir.Create();

        // main
        Parallel.ForEach(targetInfos, targetInfos =>
        {
            var (targetName, targetColor) = targetInfos;
            var outputFile = new FileInfo(Path.Combine(outputDir.FullName, $"{targetName.Replace(".png", "")}.png"));
            inputFile.ConvertImageColor(outputFile, targetColor);
        });

        return outputDir;
    }
    /// <summary>
    /// Convert image color to targetColor and save
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputDir">when null, automatically set to {inputFile.DirectoryName}/ConvertImageColorOutput/{targetColor.Name}.png</param>
    /// <param name="targetColors"></param>
    /// <returns></returns>
    public static DirectoryInfo ConvertImageColor_Loop(this FileInfo inputFile, DirectoryInfo? outputDir, params Color[] targetColors)
    {
        var targetInfos = targetColors.Select(c => (c.IsNamedColor ? c.Name : c.ToArgb().ToString(), c)).ToArray();
        inputFile.ConvertImageColor_Loop(outputDir, targetInfos);
        return outputDir;
    }
    /// <summary>
    /// Convert image color to targetColor
    /// </summary>
    /// <param name="inputImg"></param>
    /// <param name="targetColor"></param>
    /// <returns></returns>
    public static Image ConvertImageColor(Image inputImg, Color targetColor)
    {
        var resultBmp = new Bitmap(inputImg); //var resultBmp = new Bitmap(inputImg.Width,inputImg.Height,PixelFormat.Format32bppArgb);

        using (Graphics g = Graphics.FromImage(inputImg))
        {
            var bmpP = new FastBitmap(resultBmp);

            bmpP.BeginAccess();

            for (int i = 0; i < resultBmp.Width; i++)
            {
                for (int j = 0; j < resultBmp.Height; j++)
                {
                    var bmpCol = bmpP.GetPixel(i, j);
                    bmpP.SetPixel(i, j, Color.FromArgb(bmpCol.A, targetColor));
                }
            }

            bmpP.EndAccess();
        }

        return resultBmp;
    }
    /// <summary>
    /// faster Bitmap with direct memory access
    /// </summary>
    public class FastBitmap
    {
        private Bitmap original = null;
        private BitmapData fastImage = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="original"></param>
        public FastBitmap(Bitmap original)
        {
            this.original = original;
        }

        /// <summary>
        /// build fastImage
        /// </summary>
        public void BeginAccess()
        {
            // Bitmapに直接アクセスするためのオブジェクト取得(LockBits)
            // A が必要ないときは， PixelFormat.Format24bppRgb で十分。
            // A が欲しいときは， PixelFormat.Format32bppArgb
            fastImage = original.LockBits(new Rectangle(0, 0, original.Width, original.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        }

        /// <summary>
        /// kill fastImage
        /// </summary>
        public void EndAccess()
        {
            if (fastImage != null)
            {
                // Bitmapに直接アクセスするためのオブジェクト開放(UnlockBits)
                original.UnlockBits(fastImage);
                fastImage = null;
            }
        }

        /// <summary>
        /// fast GetPixel()
        /// </summary>
        /// <param name="x">Ｘ座標</param>
        /// <param name="y">Ｙ座標</param>
        /// <returns>Colorオブジェクト</returns>
        public Color GetPixel(int x, int y)
        {
            // original GetPixel() when fastImage not built
            if (fastImage == null)
                return original.GetPixel(x, y);

            // Direct memory access
            IntPtr adr = fastImage.Scan0;

            // Format24bppArgb
            //int pos = x * 3 + _img.Stride * y;
            //byte b = System.Runtime.InteropServices.Marshal.ReadByte(adr, pos + 0);
            //byte g = System.Runtime.InteropServices.Marshal.ReadByte(adr, pos + 1);
            //byte r = System.Runtime.InteropServices.Marshal.ReadByte(adr, pos + 2);

            // Format32bppArgb
            int pos = x * 4 + fastImage.Stride * y;
            byte b = System.Runtime.InteropServices.Marshal.ReadByte(adr, pos + 0);
            byte g = System.Runtime.InteropServices.Marshal.ReadByte(adr, pos + 1);
            byte r = System.Runtime.InteropServices.Marshal.ReadByte(adr, pos + 2);
            byte a = System.Runtime.InteropServices.Marshal.ReadByte(adr, pos + 3);

            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// fast SetPixel
        /// </summary>
        /// <param name="x">Ｘ座標</param>
        /// <param name="y">Ｙ座標</param>
        /// <param name="col">Colorオブジェクト</param>
        public void SetPixel(int x, int y, Color col)
        {
            // original SetPixel() when fastImage not built
            if (fastImage == null)
            {
                original.SetPixel(x, y, col);
                return;
            }

            // Direct memory access
            IntPtr adr = fastImage.Scan0;

            // Format24bppArgb
            //int pos = x * 3 + _img.Stride * y;
            //System.Runtime.InteropServices.Marshal.WriteByte(adr, pos + 0, col.B);
            //System.Runtime.InteropServices.Marshal.WriteByte(adr, pos + 1, col.G);
            //System.Runtime.InteropServices.Marshal.WriteByte(adr, pos + 2, col.R);

            // Format32bppArgb
            int pos = x * 4 + fastImage.Stride * y;
            System.Runtime.InteropServices.Marshal.WriteByte(adr, pos + 0, col.B);
            System.Runtime.InteropServices.Marshal.WriteByte(adr, pos + 1, col.G);
            System.Runtime.InteropServices.Marshal.WriteByte(adr, pos + 2, col.R);
            System.Runtime.InteropServices.Marshal.WriteByte(adr, pos + 3, col.A);
        }

    }

    // ★★★★★★★★★★★★★★★

}
