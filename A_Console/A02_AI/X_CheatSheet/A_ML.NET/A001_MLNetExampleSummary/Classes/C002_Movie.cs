using Aki32Utilities.ConsoleAppUtilities.General;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public class C002_Movie
{
    [CsvHeaderName("movieId")]
    public int movieId { get; set; }

    [CsvHeaderName("title")]
    public string movieTitle { get; set; }

    //[CsvColumnProperty("genres")]
    //public string movieGenres { get; set; }

}
