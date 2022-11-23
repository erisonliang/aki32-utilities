using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    /// <summary>
    /// 
    /// </summary>
    public class Thickness
    {
        public double L { get; set; }
        public double T { get; set; }
        public double R { get; set; }
        public double B { get; set; }

        public Thickness(double l, double t, double r, double b)
        {
            if (l < 0 && 1 < l)
                throw new InvalidDataException("L, T, R, B must be in range [0.0, 1.0]");

            L = l;
            T = t;
            R = r;
            B = b;
        }

        public Rectangle GetImageRectangle(Image img)
        {
            var cx = (int)(L * img.Width);
            var cy = (int)(T * img.Height);
            var cw = (int)((1 - L - R) * img.Width);
            var ch = (int)((1 - T - B) * img.Height);

            return new Rectangle(cx, cy, cw, ch);
        }

        public new string ToString() => $"{L:F2},{T:F2},{R:F2},{B:F2}";

    }

}
