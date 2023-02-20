using System.Text.Json.Serialization;

using Aki32Utilities.ConsoleAppUtilities.General;

using Microsoft.ML.Data;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public class C002_MovieRateInput
{
    [LoadColumn(0)]
    public float userId;

    [LoadColumn(1)]
    public float movieId;
    
    [LoadColumn(2)]
    public float Label;
}
