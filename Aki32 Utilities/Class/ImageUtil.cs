using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aki32_Utilities.Class;
internal class ImageUtil
{
    // still coding...






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

    #endregion

}
