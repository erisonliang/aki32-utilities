using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aki32_Utilities.A___Extensions.E___Image;
/// <summary>
/// This uses a simple algorithm to try to undo the distortion of a rectangle in an image
/// due to perspective - it takes the content of the rectangle and stretches it into a
/// rectangle. This is only a simple approximation and does not guarantee accuracy (in
/// fact, it will result in an image that is slightly vertically stretched such that its
/// aspect ratio will not match the original content and a more thorough approach would
/// be necessary if this is too great an approximation)
/// </summary>
internal static class SimplePerspectiveCorrection
{
    public static Bitmap ExtractAndPerspectiveCorrect(
        Bitmap image,
        Point topLeft,
        Point topRight,
        Point bottomRight,
        Point bottomLeft)
    {
        var (projectionSize, verticalSlices) =
            GetProjectionDetails(topLeft, topRight, bottomRight, bottomLeft);

        var projection = new Bitmap(projectionSize.Width, projectionSize.Height);
        foreach (var (lineToTrace, index) in verticalSlices)
        {
            var lengthOfLineToTrace = LengthOfLine(lineToTrace);

            var pixelsOnLine = Enumerable
                .Range(0, lengthOfLineToTrace)
                .Select(j =>
                {
                    var fractionOfProgressAlongLineToTrace = (float)j / lengthOfLineToTrace;
                    var point = GetPointAlongLine(lineToTrace, fractionOfProgressAlongLineToTrace);
                    return GetAverageColour(image, point);
                });

            RenderSlice(projection, pixelsOnLine, index);
        }
        return projection;

        static Color GetAverageColour(Bitmap image, PointF point)
        {
            var (integralX, fractionalX) = GetIntegralAndFractional(point.X);
            var x0 = integralX;
            var x1 = Math.Min(integralX + 1, image.Width);

            var (integralY, fractionalY) = GetIntegralAndFractional(point.Y);
            var y0 = integralY;
            var y1 = Math.Min(integralY + 1, image.Height);

            var (topColour0, topColour1) = GetColours(new Point(x0, y0), new Point(x1, y0));
            var (bottomColour0, bottomColour1) = GetColours(new Point(x0, y1), new Point(x1, y1));

            return CombineColours(
                CombineColours(topColour0, topColour1, fractionalX),
                CombineColours(bottomColour0, bottomColour1, fractionalX),
                fractionalY
            );

            (Color c0, Color c1) GetColours(Point p0, Point p1)
            {
                var c0 = image.GetPixel(p0.X, p0.Y);
                var c1 = (p0 == p1) ? c0 : image.GetPixel(p1.X, p1.Y);
                return (c0, c1);
            }

            static (int Integral, float Fractional) GetIntegralAndFractional(float value)
            {
                var integral = (int)Math.Truncate(value);
                var fractional = value - integral;
                return (integral, fractional);
            }

            static Color CombineColours(Color x, Color y, float proportionOfSecondColour)
            {
                if ((proportionOfSecondColour == 0) || (x == y))
                    return x;

                if (proportionOfSecondColour == 1)
                    return y;

                return Color.FromArgb(
                    red: CombineComponent(x.R, y.R),
                    green: CombineComponent(x.G, y.G),
                    blue: CombineComponent(x.B, y.B),
                    alpha: CombineComponent(x.A, y.A)
                );

                int CombineComponent(int x, int y) =>
                    Math.Min(
                        (int)Math.Round(
                            (x * (1 - proportionOfSecondColour)) +
                            (y * proportionOfSecondColour)
                        ),
                        255
                    );
            }
        }
    }

    private sealed record ProjectionDetails(
        Size ProjectionSize,
        IEnumerable<((PointF From, PointF To) Line, int Index)> VerticalSlices
    );

    private static ProjectionDetails GetProjectionDetails(
        Point topLeft,
        Point topRight,
        Point bottomRight,
        Point bottomLeft)
    {
        var topEdge = (From: topLeft, To: topRight);
        var bottomEdge = (From: bottomLeft, To: bottomRight);
        var lengthOfEdgeToStartFrom = LengthOfLine(topEdge);
        var dimensions = new Size(
            width: lengthOfEdgeToStartFrom,
            height: Math.Max(
                LengthOfLine((topLeft, bottomLeft)),
                LengthOfLine((topRight, bottomRight))
            )
        );
        return new ProjectionDetails(dimensions, GetVerticalSlices());

        IEnumerable<((PointF From, PointF To) Line, int Index)> GetVerticalSlices() =>
            Enumerable
                .Range(0, lengthOfEdgeToStartFrom)
                .Select(i =>
                {
                    var fractionOfProgressAlongPrimaryEdge = (float)i / lengthOfEdgeToStartFrom;
                    return (
                        Line: (
                            GetPointAlongLine(topEdge, fractionOfProgressAlongPrimaryEdge),
                            GetPointAlongLine(bottomEdge, fractionOfProgressAlongPrimaryEdge)
                        ),
                        Index: i
                    );
                });
    }

    private static PointF GetPointAlongLine((PointF From, PointF To) line, float fraction)
    {
        var deltaX = line.To.X - line.From.X;
        var deltaY = line.To.Y - line.From.Y;
        return new PointF(
            (deltaX * fraction) + line.From.X,
            (deltaY * fraction) + line.From.Y
        );
    }

    private static int LengthOfLine((PointF From, PointF To) line)
    {
        var deltaX = line.To.X - line.From.X;
        var deltaY = line.To.Y - line.From.Y;
        return (int)Math.Round(Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY)));
    }

    private static void RenderSlice(
        Bitmap projectionBitmap,
        IEnumerable<Color> pixelsOnLine,
        int index)
    {
        var pixelsOnLineArray = pixelsOnLine.ToArray();

        using var slice = new Bitmap(1, pixelsOnLineArray.Length);
        for (var j = 0; j < pixelsOnLineArray.Length; j++)
            slice.SetPixel(0, j, pixelsOnLineArray[j]);

        using var g = Graphics.FromImage(projectionBitmap);
        g.DrawImage(
            slice,
            srcRect: new Rectangle(0, 0, slice.Width, slice.Height),
            destRect: new Rectangle(index, 0, 1, projectionBitmap.Height),
            srcUnit: GraphicsUnit.Pixel
        );
    }
}
