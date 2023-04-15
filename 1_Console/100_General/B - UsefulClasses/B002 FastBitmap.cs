using System.Drawing;
using System.Drawing.Imaging;

namespace Aki32Utilities.ConsoleAppUtilities.General;
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

}