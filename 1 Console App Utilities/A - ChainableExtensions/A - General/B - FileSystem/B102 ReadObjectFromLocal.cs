using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Xml.Serialization;

using ClosedXML;

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
            .Select(c => fullProps.FirstOrDefault(p => p.Name == c))
            .ToArray();

        //csvGrid[0]
        //    .Select(c => fullProps.FirstOrDefault(p => p.Name == c))
        //    .Where(p => p is not null)
        //    .Where(p => !p.HasAttribute<CsvIgnoreAttribute>())
        //    .Where(p => p.CanWrite)
        //    .ToArray()
        //    ;

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

        csvGrid = csvGrid[1..^0];

        // contents
        foreach (var csvLine in csvGrid)
        {
            var data = new T();
            foreach (var pi in propIndexPair)
            {
                var propType = pi.prop!.PropertyType;

                Console.WriteLine(pi.prop!.Name);

                if (propType == typeof(string))
                {
                    pi.prop!.SetValue(data, csvLine[pi.index]);
                }
                else
                {
                    var deserializedObject = JsonConvert.DeserializeObject(csvLine[pi.index], propType, (JsonSerializerSettings?)null);

                    // same as above
                    //
                    //var deserializedObject = typeof(JsonConvert)
                    //    .GetMethods()
                    //    .FirstOrDefault(c => { return c.Name == "DeserializeObject" && c.IsGenericMethod && c.GetParameters()[0].ParameterType == typeof(string); })?
                    //    .MakeGenericMethod(propType)
                    //    .Invoke(null, new object[] { csvLine[i] });

                    pi.prop!.SetValue(data, deserializedObject);
                }

            }

            outputDataList.Add(data);
        }

        return outputDataList;
    }

}
