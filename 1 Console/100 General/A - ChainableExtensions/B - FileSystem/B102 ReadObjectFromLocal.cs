using System.Reflection;
using System.Xml.Serialization;

using ClosedXML;

using Newtonsoft.Json;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// read json from local
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ReadObjectFromLocalJson<T>(this FileInfo inputFile) where T : new()
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
    public static T ReadObjectFromLocalXml<T>(this FileInfo inputFile) where T : new()
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
    public static List<T> ReadObjectFromLocalCsv<T>(this FileInfo inputFile) where T : new()
    {
        var outputDataList = new List<T>();
        var csvGrid = inputFile.ReadCsv_Rows();

        // get props in order

        var fullProps = typeof(T).GetProperties();

        var headerProps = csvGrid[0]
            .Select(c => fullProps.FirstOrDefault(p => (p.Name == c) || (p.HasAttribute<CsvHeaderNameAttribute>() && p.GetAttributes<CsvHeaderNameAttribute>()[0].Name == c)))
            .ToArray();

        var propIndexPair = new List<(int index, PropertyInfo prop)>();
        for (int i = 0; i < headerProps.Length; i++)
        {
            var p = headerProps[i];
            if (p == null)
                continue;

            if (p.HasAttribute<CsvIgnoreAttribute>())
                continue;

            if (!p.CanWrite)
                continue;

            propIndexPair.Add((i, p));
        }

        csvGrid = csvGrid[1..];

        // contents
        foreach (var csvLine in csvGrid)
        {
            var data = new T();
            foreach (var (index, prop) in propIndexPair)
            {
                var propType = prop!.PropertyType;

                object settingItem;

                if (propType == typeof(string))
                {
                    // Since JsonConvert.DeserializeObject does not accept string.
                    settingItem = csvLine[index];
                }
                else if (propType == typeof(bool?))
                {
                    // If csv has been processed by Excel, true/false automatically turned into TRUE/FALSE...
                    // and since JsonConvert.DeserializeObject does not accept TRUE/FALSE, explicitly defined to convert to true/false.
                    settingItem = JsonConvert.DeserializeObject(csvLine[index].ToLower(), propType, (JsonSerializerSettings?)null)!;
                }
                else
                {
                    // General
                    settingItem = JsonConvert.DeserializeObject(csvLine[index], propType, (JsonSerializerSettings?)null)!;

                    // same as above
                    //
                    //var deserializedObject = typeof(JsonConvert)
                    //    .GetMethods()
                    //    .FirstOrDefault(c => { return c.Name == "DeserializeObject" && c.IsGenericMethod && c.GetParameters()[0].ParameterType == typeof(string); })?
                    //    .MakeGenericMethod(propType)
                    //    .Invoke(null, new object[] { csvLine[i] });

                }

                prop!.SetValue(data, settingItem);
            }

            outputDataList.Add(data);
        }

        return outputDataList;
    }

}
