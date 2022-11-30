using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

using DocumentFormat.OpenXml.Drawing;

using Newtonsoft.Json;

namespace Aki32_Utilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// read json from local
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ReadJsonFromLocal<T>(this FileInfo inputFile) where T : new()
    {
        using var sr = new StreamReader(inputFile.FullName);
        var json = sr.ReadToEnd();

        var data = JsonConvert.DeserializeObject<T>(json);

        return data;
    }

    /// <summary>
    /// read xml from local
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ReadXmlFromLocal<T>(this FileInfo inputFile) where T : new()
    {
        using var sr = new StreamReader(inputFile.FullName);

        var serializer = new XmlSerializer(typeof(T));
        var data = (T)serializer.Deserialize(sr)!;

        return data;
    }

    /// <summary>
    /// read csv from local
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ReadCsvFromLocal<T>(this FileInfo inputFile) where T : new()
    {
        throw new NotImplementedException();

        using var sr = new StreamReader(inputFile.FullName);

        var serializer = new XmlSerializer(typeof(T));
        var data = (T)serializer.Deserialize(sr)!;

        return data;






        //csvGrid.ToArray().SaveCsv_Rows(outputFile);




        //var csvGrid = inputFile.read   new List<string[]>();
        //var props = typeof(T).GetProperties();

        //// header
        //if (withHeader)
        //{
        //    var csvLine = new List<string>();
        //    foreach (var prop in props)
        //        csvLine.Add(prop.Name);

        //    csvGrid.Add(csvLine.ToArray());
        //}

        //// contents
        //foreach (var dataLine in dataList)
        //{
        //    var csvLine = new List<string>();
        //    foreach (var prop in props)
        //    {
        //        var value = prop.GetValue(dataLine);

        //        if (value is IEnumerable enumProp)
        //        {
        //            csvLine.Add(JsonConvert.SerializeObject(enumProp));
        //        }
        //        else
        //        {
        //            csvLine.Add(value?.ToString()!);
        //        }
        //    }

        //    csvGrid.Add(csvLine.ToArray());
        //}

        //csvGrid.ToArray().SaveCsv_Rows(outputFile);

        //return outputFile;










    }

}
