﻿using System.Drawing.Drawing2D;
using System.Drawing;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public partial class MLNetExampleSummary : MLNetHandler
{
    void I004_DrawBoundingBox(string inputImageLocation, string outputImageLocation, string imageName, IList<YoloBoundingBox> filteredBoundingBoxes)
    {
        Image image = Image.FromFile(Path.Combine(inputImageLocation, imageName));

        var originalImageHeight = image.Height;
        var originalImageWidth = image.Width;

        foreach (var box in filteredBoundingBoxes)
        {
            // Get Bounding Box Dimensions
            var x = (uint)Math.Max(box.Dimensions.X, 0);
            var y = (uint)Math.Max(box.Dimensions.Y, 0);
            var width = (uint)Math.Min(originalImageWidth - x, box.Dimensions.Width);
            var height = (uint)Math.Min(originalImageHeight - y, box.Dimensions.Height);

            // Resize To Image
            x = (uint)originalImageWidth * x / OnnxModelScorer.ImageNetSettings.imageWidth;
            y = (uint)originalImageHeight * y / OnnxModelScorer.ImageNetSettings.imageHeight;
            width = (uint)originalImageWidth * width / OnnxModelScorer.ImageNetSettings.imageWidth;
            height = (uint)originalImageHeight * height / OnnxModelScorer.ImageNetSettings.imageHeight;

            // Bounding Box Text
            string text = $"{box.Label} ({(box.Confidence * 100).ToString("0")}%)";

            using (Graphics thumbnailGraphic = Graphics.FromImage(image))
            {
                thumbnailGraphic.CompositingQuality = CompositingQuality.HighQuality;
                thumbnailGraphic.SmoothingMode = SmoothingMode.HighQuality;
                thumbnailGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;

                // Define Text Options
                Font drawFont = new Font("Arial", 12, FontStyle.Bold);
                SizeF size = thumbnailGraphic.MeasureString(text, drawFont);
                SolidBrush fontBrush = new SolidBrush(Color.Black);
                Point atPoint = new Point((int)x, (int)y - (int)size.Height - 1);

                // Define BoundingBox options
                Pen pen = new Pen(box.BoxColor, 3.2f);
                SolidBrush colorBrush = new SolidBrush(box.BoxColor);

                // Draw text on image 
                thumbnailGraphic.FillRectangle(colorBrush, (int)x, (int)(y - size.Height - 1), (int)size.Width, (int)size.Height);
                thumbnailGraphic.DrawString(text, drawFont, fontBrush, atPoint);

                // Draw bounding box on image
                thumbnailGraphic.DrawRectangle(pen, x, y, width, height);
            }
        }

        if (!Directory.Exists(outputImageLocation))
        {
            Directory.CreateDirectory(outputImageLocation);
        }

        image.Save(Path.Combine(outputImageLocation, imageName));
    }

    void I004_LogDetectedObjects(string imageName, IList<YoloBoundingBox> boundingBoxes)
    {
        Console.WriteLine($".....The objects in the image {imageName} are detected as below....");

        foreach (var box in boundingBoxes)
        {
            Console.WriteLine($"{box.Confidence * 100:F0}%\t{box.Label}");
        }

        Console.WriteLine("");
    }

}