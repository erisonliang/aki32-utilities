using System.Drawing;
using System.Drawing.Imaging;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    // ★★★★★★★★★★★★★★★ FileSystemInfo chain process

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
        UtilPreprocessors.PreprocessOutFile(ref outputFile, false, inputFile.Directory!, inputFile.Name);


        // main
        try
        {
            using (var inputImage = Image.FromFile(inputFile.FullName))
            {
                var img = ConvertImageColor(inputImage, targetColor);
                img.Save(outputFile!.FullName, ImageFormat.Png);
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
            outputDir = new DirectoryInfo(Path.Combine(inputFile.DirectoryName!, UtilConfig.GetNewOutputDirName("ConvertImageColor")));
        if (!outputDir.Exists)
            outputDir.Create();


        // main
        Parallel.ForEach(targetInfos, targetInfos =>
        {
            var (targetName, targetColor) = targetInfos;
            var outputFile = new FileInfo(Path.Combine(outputDir.FullName, $"{targetName.Replace(".png", "")}.png"));
            inputFile.ConvertImageColor(outputFile, targetColor);
        });


        // post process
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
        return outputDir!;
    }


    // ★★★★★★★★★★★★★★★ Image process

    /// <summary>
    /// Convert image color to targetColor
    /// </summary>
    /// <param name="inputImage"></param>
    /// <param name="targetColor"></param>
    /// <returns></returns>
    public static Image ConvertImageColor(Image inputImage, Color targetColor)
    {
        var outputBitmap = new Bitmap(inputImage); //var resultBmp = new Bitmap(inputImage.Width,inputImage.Height,PixelFormat.Format32bppArgb);

        using (Graphics g = Graphics.FromImage(inputImage))
        {
            var bmpP = new FastBitmap(outputBitmap);

            bmpP.BeginAccess();

            for (int i = 0; i < outputBitmap.Width; i++)
            {
                for (int j = 0; j < outputBitmap.Height; j++)
                {
                    var bmpCol = bmpP.GetPixel(i, j);
                    bmpP.SetPixel(i, j, Color.FromArgb(bmpCol.A, targetColor));
                }
            }

            bmpP.EndAccess();
        }


        return (Image)outputBitmap.Clone();
    }

    /// <summary>
    /// faster Bitmap with direct memory access
    /// </summary>
    public class FastBitmap
    {
        private Bitmap original;
        private BitmapData? fastImage = null;

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

        // ★★★★★★★★★★★★★★★

    }

}
