﻿using Microsoft.ML.Data;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public class I004_YoloInput
{
    [LoadColumn(0)]
    public string ImagePath;

    [LoadColumn(1)]
    public string FileName;
}
