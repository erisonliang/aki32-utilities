using System.Text;
using System.Xml.Serialization;

using ClosedXML;

using Newtonsoft.Json;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// write data as json local file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="outputFile"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static FileInfo SaveAsJson<T>(this T data, FileInfo outputFile, bool formatted = false)
    {
        var json = JsonConvert.SerializeObject(data, formatted ? Formatting.Indented : Formatting.None);

        using var sw = new StreamWriter(outputFile.FullName, false, Encoding.UTF8);
        sw.Write(json);

        return outputFile!;
    }

    /// <summary>
    /// write data as xml local file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="outputFile"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static FileInfo SaveAsXml<T>(this T data, FileInfo outputFile)
    {
        using var sw = new StreamWriter(outputFile.FullName, false, Encoding.UTF8);

        var serializer = new XmlSerializer(typeof(T));
        serializer.Serialize(sw, data);

        return outputFile!;
    }

    /// <summary>
    /// write data as csv local file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="outputFile"></param>
    /// <param name="dataList"></param>
    /// <returns></returns>
    public static FileInfo SaveAsCsv<T>(this IEnumerable<T> dataList, FileInfo outputFile)
    {
        var csvGrid = new List<string[]>();
        var props = typeof(T)
            .GetProperties()
            .Where(p => !p.HasAttribute<CsvIgnoreAttribute>());

        // header
        {
            var csvLine = new List<string>();
            foreach (var prop in props)
                csvLine.Add(prop.HasAttribute<CsvHeaderNameAttribute>()
                    ? prop.GetAttributes<CsvHeaderNameAttribute>()[0].Name
                    : prop.Name);

            csvGrid.Add(csvLine.ToArray());
        }

        // contents
        foreach (var dataLine in dataList)
        {
            var csvLine = new List<string>();
            foreach (var prop in props)
            {
                var addingValue = "";

                try
                {
                    var value = prop.GetValue(dataLine);

                    if (value == null)
                        addingValue = "";
                    else if (value is string)
                        addingValue = value?.ToString() ?? "";
                    else
                        addingValue = JsonConvert.SerializeObject(value);

                }
                catch (Exception)
                {
                }
                finally
                {
                    csvLine.Add(addingValue);
                }
            }

            csvGrid.Add(csvLine.ToArray());
        }

        csvGrid.ToArray().SaveAsCsv_Rows(outputFile);

        return outputFile!;
    }

    /// <summary>
    /// save csv from a list of rows
    /// </summary>
    /// <param name="inputFile_Rows"></param>
    /// <param name="outputFile"></param>
    /// <param name="header"></param>
    /// <returns></returns>
    public static FileInfo SaveAsCsv_Rows<T>(this T[][] inputFile_Rows, FileInfo outputFile, string? header = null)
    {
        // preprocess
        UtilPreprocessors.PreprocessBasic(false);


        // main
        using var sw = new StreamWriter(outputFile.FullName, false, Encoding.GetEncoding("SHIFT_JIS"));

        if (!string.IsNullOrEmpty(header))
            sw.WriteLine(header);

        foreach (var row in inputFile_Rows)
        {
            var correctedLine = row
                .Select(x => x?.ToString() ?? "")
                .Select(x => x.Replace("\"", "\"\""))
                .Select(x => x.Any(x => x == ',' || x == '\"' || x == '\\' || x == '\n' || x == '\r') ? $"\"{x}\"" : x);
            sw.WriteLine(string.Join(',', correctedLine));
        }


        // post process
        return outputFile!;
    }

    /// <summary>
    /// save csv from a list of columns
    /// </summary>
    /// <param name="inputFile_Columns"></param>
    /// <param name="outputFile"></param>
    /// <param name="header"></param>
    /// <returns></returns>
    public static FileInfo SaveAsCsv_Columns<T>(this T[][] inputFile_Columns, FileInfo outputFile, string? header = null)
    {
        // main
        var inputFile_Rows = inputFile_Columns.Transpose();
        return inputFile_Rows!.SaveAsCsv_Rows(outputFile, header);
    }

}
