

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ from index

    /// <summary>
    /// Create TimeHistory instance and return
    /// </summary>
    /// <param name="inputFile"></param>
    /// <returns>pipe inputFile</returns>
    public static FileInfo DrawGraph(this FileInfo inputFile, int hAxisIndex, int vAxisIndex, TimeHistory.ChartType type = TimeHistory.ChartType.Line)
    {
        var th = inputFile.GetTimeHistoryFromFile();

        th.DrawGraph(th.Columns[vAxisIndex], th.Columns[hAxisIndex], type);

        return inputFile;
    }


    // ★★★★★★★★★★★★★★★ from name

    /// <summary>
    /// Create TimeHistory instance and return
    /// </summary>
    /// <param name="inputFile"></param>
    /// <returns>pipe inputFile</returns>
    public static FileInfo DrawGraph(this FileInfo inputFile, string hAxisName, string vAxisName, TimeHistory.ChartType type = TimeHistory.ChartType.Line)
    {
        inputFile
            .GetTimeHistoryFromFile()
            .DrawGraph(vAxisName, hAxisName, type);

        return inputFile;
    }


    // ★★★★★★★★★★★★★★★ for all

    /// <summary>
    /// Create TimeHistory instance and return
    /// </summary>
    /// <param name="inputFile"></param>
    /// <returns>pipe inputFile</returns>
    public static FileInfo DrawGraph_ForAll(this FileInfo inputFile, TimeHistory.ChartType type = TimeHistory.ChartType.Line)
    {
        var th = inputFile.GetTimeHistoryFromFile();

        for (int i = 0; i < th.Columns.Length; i++)
            for (int j = i + 1; j < th.Columns.Length; j++)
                th.DrawGraph(th.Columns[j], th.Columns[i], type);

        return inputFile;
    }


    // ★★★★★★★★★★★★★★★

}
