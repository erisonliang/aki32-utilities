﻿using Microsoft.ML.Data;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public class A001_SentimentInput
{
    [LoadColumn(0)]
    public bool Label { get; set; }
    [LoadColumn(2)]
    public string Text { get; set; }
}
