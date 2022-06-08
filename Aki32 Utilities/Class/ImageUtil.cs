using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aki32_Utilities.Class;
internal class ImageUtil
{
    // still coding...



    // ★★★★★★★★★★★★★★★ Crop



    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="rootDirPath"></param>
    static void CropImageAndSave(string inputFilePath, CropSize crop)
    {
        throw new NotImplementedException();

        // こうやって使う！
        // CropImageAndSave(f.FullName, new CropSize(0.114, 0.31, 0.5, 0.21));







        var rootDir = Path.GetDirectoryName(inputFilePath);

        var saveDirPath = Path.Combine(rootDir, "result");
        if (!Directory.Exists(saveDirPath))
            Directory.CreateDirectory(saveDirPath);

        var saveFilePath = Path.Combine(saveDirPath, Path.GetFileName(inputFilePath));
        var str = $"{Path.GetFileName(inputFilePath)}";

        try
        {
            using (var inputImg = Image.FromFile(inputFilePath))
                CropImage(inputImg, crop).Save(saveFilePath);

            Console.WriteLine($"O 成功 : {str}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"X 失敗 : {str}, {e.Message}");
        }


    }

    static Bitmap CropImage(Image inputImg, CropSize crop)
    {




        //var resultBmp = new Bitmap(inputImg.Width,inputImg.Height,PixelFormat.Format32bppArgb);
        var resultBmp = new Bitmap(inputImg);

        try
        {
            // 画像を切り抜く
            Bitmap outputBmp = (inputImg as Bitmap).Clone(crop.GetImageCropRect(inputImg), inputImg.PixelFormat);
            return outputBmp;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return resultBmp;
    }



    // ★★★★★★★★★★★★★★★ ConvertImageColors

    private void main()
    {
        // ルートのフォルダ。
        var rootDir = @"C:\Users\zilli\Dropbox\Codes\@CSharp\@ALL\@Main\泡ベビ\UNO_AwaBaby\UNO_AwaBaby\UNO_AwaBaby.Shared\Assets\Bubbles";
        //var rootDir = @"C:\Users\zilli\Desktop\あああ";
        string[] files = Directory.GetFiles(rootDir, "*", SearchOption.TopDirectoryOnly);

        // 変換情報
        var targetInfoList = new List<TargetInfo>
            {

                //new TargetInfo("R", Color.Red),
                //new TargetInfo("G", Color.Green),
                new TargetInfo("B", Color.Blue),

                //new TargetInfo("P", Color.Purple),
                //new TargetInfo("O", Color.Orange),

                //new TargetInfo("Y", Color.Yellow),
                //new TargetInfo("M", Color.Magenta),
                //new TargetInfo("C", Color.Cyan),

                new TargetInfo("W", Color.White),
                new TargetInfo("X", Color.Black),
            };

        // 処理
        Console.WriteLine($"★開始！");

        foreach (var targetInfo in targetInfoList)
        {
            var saveDirPath = Path.Combine(rootDir, targetInfo.Name);
            if (!Directory.Exists(saveDirPath))
                Directory.CreateDirectory(saveDirPath);

            Parallel.ForEach(
                files,
                //new ParallelOptions() { MaxDegreeOfParallelism = 10 },
                inputFilePath => ConvertImageColorAndSave(inputFilePath, saveDirPath, targetInfo));
        }

        Console.WriteLine($"★終了！");
        Console.ReadLine();
    }



    /// <summary>
    /// 1枚の画像を，変換して保存。
    /// </summary>
    /// <param name="inputFilePath"></param>
    /// <param name="targetInfo"></param>
    private static void ConvertImageColorAndSave(string inputFilePath, string saveDirPath, TargetInfo targetInfo)
    {
        var saveFilePath = Path.Combine(saveDirPath, Path.GetFileName(inputFilePath));
        var str = $"{targetInfo.Name}, {Path.GetFileName(inputFilePath)}";

        try
        {
            using (var inputImg = Image.FromFile(inputFilePath))
                ConvertImageColor(inputImg, targetInfo).Save(saveFilePath);

            Console.WriteLine($"O 成功 : {str}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"X 失敗 : {str}, {e.Message}");
        }
    }

    /// <summary>
    /// 1枚の画像の色を高速変換。
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="targetInfo"></param>
    private static Bitmap ConvertImageColor(Image inputImg, TargetInfo targetInfo)
    {
        //var resultBmp = new Bitmap(inputImg.Width,inputImg.Height,PixelFormat.Format32bppArgb);
        var resultBmp = new Bitmap(inputImg);

        using (Graphics g = Graphics.FromImage(inputImg))
        {
            // Bitmap処理の高速化開始
            BitmapPlus bmpP = new BitmapPlus(resultBmp);

            bmpP.BeginAccess();

            for (int i = 0; i < resultBmp.Width; i++)
            {
                for (int j = 0; j < resultBmp.Height; j++)
                {
                    // Bitmapの色取得
                    Color bmpCol = bmpP.GetPixel(i, j);
                    bmpP.SetPixel(i, j, Color.FromArgb(bmpCol.A, targetInfo.Color));
                }
            }

            // Bitmap処理の高速化終了
            bmpP.EndAccess();
        }

        return resultBmp;
    }




    // ★★★★★★★★★★★★★★★ Classes

    #region 関連クラス

    private class CropSize
    {
        public double l { get; set; }
        public double t { get; set; }
        public double r { get; set; }
        public double b { get; set; }

        public CropSize(double l, double t, double r, double b)
        {
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
    }

    /// <summary>
    /// Bitmap処理を高速化するためのクラス
    /// </summary>
    class BitmapPlus
    {
        /// <summary>
        /// オリジナルのBitmapオブジェクト
        /// </summary>
        private Bitmap _bmp = null;

        /// <summary>
        /// Bitmapに直接アクセスするためのオブジェクト
        /// </summary>
        private BitmapData _img = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="original"></param>
        public BitmapPlus(Bitmap original)
        {
            // オリジナルのBitmapオブジェクトを保存
            _bmp = original;
        }

        /// <summary>
        /// Bitmap処理の高速化開始
        /// </summary>
        public void BeginAccess()
        {
            // Bitmapに直接アクセスするためのオブジェクト取得(LockBits)
            // A が必要ないときは， PixelFormat.Format24bppRgb で十分。
            // A が欲しいときは， PixelFormat.Format32bppArgb
            _img = _bmp.LockBits(new Rectangle(0, 0, _bmp.Width, _bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        }

        /// <summary>
        /// Bitmap処理の高速化終了
        /// </summary>
        public void EndAccess()
        {
            if (_img != null)
            {
                // Bitmapに直接アクセスするためのオブジェクト開放(UnlockBits)
                _bmp.UnlockBits(_img);
                _img = null;
            }
        }

        /// <summary>
        /// BitmapのGetPixel同等
        /// </summary>
        /// <param name="x">Ｘ座標</param>
        /// <param name="y">Ｙ座標</param>
        /// <returns>Colorオブジェクト</returns>
        public Color GetPixel(int x, int y)
        {
            if (_img == null)
            {
                // Bitmap処理の高速化を開始していない場合はBitmap標準のGetPixel
                return _bmp.GetPixel(x, y);
            }

            // Bitmap処理の高速化を開始している場合はBitmapメモリへの直接アクセス
            IntPtr adr = _img.Scan0;
            //int pos = x * 3 + _img.Stride * y;
            //byte b = System.Runtime.InteropServices.Marshal.ReadByte(adr, pos + 0);
            //byte g = System.Runtime.InteropServices.Marshal.ReadByte(adr, pos + 1);
            //byte r = System.Runtime.InteropServices.Marshal.ReadByte(adr, pos + 2);

            int pos = x * 4 + _img.Stride * y;
            byte b = System.Runtime.InteropServices.Marshal.ReadByte(adr, pos + 0);
            byte g = System.Runtime.InteropServices.Marshal.ReadByte(adr, pos + 1);
            byte r = System.Runtime.InteropServices.Marshal.ReadByte(adr, pos + 2);
            byte a = System.Runtime.InteropServices.Marshal.ReadByte(adr, pos + 3);

            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// BitmapのSetPixel同等
        /// </summary>
        /// <param name="x">Ｘ座標</param>
        /// <param name="y">Ｙ座標</param>
        /// <param name="col">Colorオブジェクト</param>
        public void SetPixel(int x, int y, Color col)
        {
            if (_img == null)
            {
                // Bitmap処理の高速化を開始していない場合はBitmap標準のSetPixel
                _bmp.SetPixel(x, y, col);
                return;
            }

            // Bitmap処理の高速化を開始している場合はBitmapメモリへの直接アクセス
            IntPtr adr = _img.Scan0;
            //int pos = x * 3 + _img.Stride * y;
            //System.Runtime.InteropServices.Marshal.WriteByte(adr, pos + 0, col.B);
            //System.Runtime.InteropServices.Marshal.WriteByte(adr, pos + 1, col.G);
            //System.Runtime.InteropServices.Marshal.WriteByte(adr, pos + 2, col.R);

            int pos = x * 4 + _img.Stride * y;
            System.Runtime.InteropServices.Marshal.WriteByte(adr, pos + 0, col.B);
            System.Runtime.InteropServices.Marshal.WriteByte(adr, pos + 1, col.G);
            System.Runtime.InteropServices.Marshal.WriteByte(adr, pos + 2, col.R);
            System.Runtime.InteropServices.Marshal.WriteByte(adr, pos + 3, col.A);
        }

    }


    private class TargetInfo
    {
        public string Name { get; set; }
        public Color Color { get; set; }

        public TargetInfo(string name, Color color)
        {
            Name = name;
            Color = color;
        }
    }

    #endregion

}
