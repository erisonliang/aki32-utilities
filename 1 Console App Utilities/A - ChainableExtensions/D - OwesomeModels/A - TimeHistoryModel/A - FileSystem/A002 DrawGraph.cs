

namespace Aki32_Utilities.OwesomeModels.ChainableExtensions;
public static partial class TimeHistoryExensions
{

    // ★★★★★★★★★★★★★★★ from index

    /// <summary>
    /// Create TimeHistory instance and return
    /// </summary>
    /// <param name="inputFile"></param>
    /// <returns></returns>
    public static FileInfo DrawLineGraph(this FileInfo inputFile, int hAxisIndex, int vAxisIndex)
    {
        var th = inputFile.GetTimeHistoryFromFile();

        th.DrawLineGraph(th.Columns[hAxisIndex], th.Columns[vAxisIndex]);

        return inputFile;
    }

    /// <summary>
    /// Create TimeHistory instance and return
    /// </summary>
    /// <param name="inputFile"></param>
    /// <returns></returns>
    public static FileInfo DrawScatterGraph(this FileInfo inputFile, int hAxisIndex, int vAxisIndex)
    {
        var th = inputFile.GetTimeHistoryFromFile();

        th.DrawScatterGraph(th.Columns[hAxisIndex], th.Columns[vAxisIndex]);

        return inputFile;
    }


    // ★★★★★★★★★★★★★★★ from name

    /// <summary>
    /// Create TimeHistory instance and return
    /// </summary>
    /// <param name="inputFile"></param>
    /// <returns></returns>
    public static FileInfo DrawLineGraph(this FileInfo inputFile, string hAxisName, string vAxisName)
    {
        inputFile
            .GetTimeHistoryFromFile()
            .DrawLineGraph(vAxisName, hAxisName);

        return inputFile;
    }

    /// <summary>
    /// Create TimeHistory instance and return
    /// </summary>
    /// <param name="inputFile"></param>
    /// <returns></returns>
    public static FileInfo DrawScatterGraph(this FileInfo inputFile, string hAxisName, string vAxisName)
    {
        inputFile
            .GetTimeHistoryFromFile()
            .DrawScatterGraph(vAxisName, hAxisName);

        return inputFile;
    }


    // ★★★★★★★★★★★★★★★ for all

    /// <summary>
    /// Create TimeHistory instance and return
    /// </summary>
    /// <param name="inputFile"></param>
    /// <returns></returns>
    public static FileInfo DrawLineGraph_ForAll(this FileInfo inputFile)
    {
        var th = inputFile.GetTimeHistoryFromFile();

        for (int i = 0; i < th.Columns.Length; i++)
            for (int j = i + 1; j < th.Columns.Length; j++)
                th.DrawLineGraph(th.Columns[j], th.Columns[i]);

        return inputFile;
    }



    // ★★★★★★★★★★★★★★★

}
