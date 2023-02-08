using System.Drawing;
using System.Drawing.Drawing2D;

using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public class BoundingBoxDimensions : I004_DimensionsBase { }

public class I004_YoloBoundingBox
{
    public BoundingBoxDimensions Dimensions { get; set; }

    public string Label { get; set; }

    public float Confidence { get; set; }

    public RectangleF Rect => new(Dimensions.X, Dimensions.Y, Dimensions.Width, Dimensions.Height);

    public Color BoxColor { get; set; }


    public static void DrawBoundingBoxToImage(DirectoryInfo inputImage, DirectoryInfo outputImage, string imageName, IList<I004_YoloBoundingBox> boxes)
    {
        var image = Image.FromFile(inputImage.GetChildFileInfo(imageName).FullName);
        var oriH = image.Height;
        var oriW = image.Width;

        foreach (var box in boxes)
        {
            // Get Bounding Box Dimensions
            var x = (uint)Math.Max(box.Dimensions.X, 0);
            var y = (uint)Math.Max(box.Dimensions.Y, 0);
            var width = (uint)Math.Min(oriW - x, box.Dimensions.Width);
            var height = (uint)Math.Min(oriH - y, box.Dimensions.Height);

            // Resize To Image
            x = (uint)oriW * x / I004_Config.ImageWidth;
            y = (uint)oriH * y / I004_Config.ImageHeight;
            width = (uint)oriW * width / I004_Config.ImageWidth;
            height = (uint)oriH * height / I004_Config.ImageHeight;

            // Bounding Box Text
            string text = $"{box.Label} ({box.Confidence * 100:0}%)";

            using var g = Graphics.FromImage(image);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Define Text Options
            var drawFont = new Font("Arial", 12, FontStyle.Bold);
            var size = g.MeasureString(text, drawFont);
            var fontBrush = new SolidBrush(Color.Black);
            var atPoint = new Point((int)x, (int)y - (int)size.Height - 1);

            // Define BoundingBox options
            var pen = new Pen(box.BoxColor, 3.2f);
            var colorBrush = new SolidBrush(box.BoxColor);

            // Draw text on image 
            g.FillRectangle(colorBrush, (int)x, (int)(y - size.Height - 1), (int)size.Width, (int)size.Height);
            g.DrawString(text, drawFont, fontBrush, atPoint);

            // Draw bounding box on image
            g.DrawRectangle(pen, x, y, width, height);
        }

        outputImage.Create();
        image.Save(outputImage.GetChildFileInfo(imageName).FullName);

    }
}
