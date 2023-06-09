﻿using System.Drawing;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ main

    /// <summary>
    /// Get corrent image.
    /// Default Image.FromFile() creates image instance with no rotation applied; Use this method instead to get correct one.
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <param name="pps">List of the ratio of original point and target points. from 0.0-1.0. Min length 1, max length 3</param>
    /// <returns></returns>
    public static Image GetImageFromFile(this FileInfo inputFile)
    {
        using var inputImage = Image.FromFile(inputFile.FullName);

        try
        {
            var property = inputImage.PropertyItems.FirstOrDefault(p => p.Id == 0x0112);

            if (property != null)
            {
                var rotation = RotateFlipType.RotateNoneFlipNone;

                var orientation = (ExifOrientation)BitConverter.ToUInt16(property.Value, 0);

                switch (orientation)
                {
                    case ExifOrientation.TopLeft:
                        break;
                    case ExifOrientation.TopRight:
                        rotation = RotateFlipType.RotateNoneFlipX;
                        break;
                    case ExifOrientation.BottomRight:
                        rotation = RotateFlipType.Rotate180FlipNone;
                        break;
                    case ExifOrientation.BottomLeft:
                        rotation = RotateFlipType.RotateNoneFlipY;
                        break;
                    case ExifOrientation.LeftTop:
                        rotation = RotateFlipType.Rotate270FlipY;
                        break;
                    case ExifOrientation.RightTop:
                        rotation = RotateFlipType.Rotate90FlipNone;
                        break;
                    case ExifOrientation.RightBottom:
                        rotation = RotateFlipType.Rotate90FlipY;
                        break;
                    case ExifOrientation.LeftBottom:
                        rotation = RotateFlipType.Rotate270FlipNone;
                        break;
                }

                inputImage.RotateFlip(rotation);

                property.Value = BitConverter.GetBytes((ushort)ExifOrientation.TopLeft);
                inputImage.SetPropertyItem(property);
            }
        }
        catch (Exception)
        {
            // ignore
        }

        return (Image)inputImage.Clone();
    }


    // ★★★★★★★★★★★★★★★ background

    private enum ExifOrientation : ushort
    {
        TopLeft = 1,
        TopRight = 2,
        BottomRight = 3,
        BottomLeft = 4,
        LeftTop = 5,
        RightTop = 6,
        RightBottom = 7,
        LeftBottom = 8
    }


    // ★★★★★★★★★★★★★★★

}