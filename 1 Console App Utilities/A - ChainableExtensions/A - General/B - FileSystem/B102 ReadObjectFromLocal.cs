using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
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
    public static List<T> ReadObjectFromLocalCsv<T>(this FileInfo inputFile, bool withHeader = true) where T : new()
    {
        var outputDataList = new List<T>();
        var csvGrid = inputFile.ReadCsv_Rows();

        // get props in order 
        PropertyInfo[] props;
        if (withHeader)
        {
            var fullProps = typeof(T).GetProperties();

            props = csvGrid[0]
                .Select(c => fullProps.FirstOrDefault(p => p.Name == c))
                .Where(p => p is not null)
                .ToArray()
                ;

            csvGrid = csvGrid[1..^0];
        }
        else
        {
            props = typeof(T).GetProperties();
        }

        // contents
        foreach (var csvLine in csvGrid)
        {
            var data = new T();
            for (int i = 0; i < props.Length; i++)
            {
                var propType = props[i].PropertyType;

                if (propType == typeof(string))
                {
                    props[i].SetValue(data, csvLine[i]);
                }
                else
                {
                    var deserializedObject = JsonConvert.DeserializeObject(csvLine[i], propType, (JsonSerializerSettings?)null);

                    // same as above
                    //
                    //var deserializedObject = typeof(JsonConvert)
                    //    .GetMethods()
                    //    .FirstOrDefault(c => { return c.Name == "DeserializeObject" && c.IsGenericMethod && c.GetParameters()[0].ParameterType == typeof(string); })?
                    //    .MakeGenericMethod(propType)
                    //    .Invoke(null, new object[] { csvLine[i] });

                    props[i].SetValue(data, deserializedObject);
                }

            }

            outputDataList.Add(data);
        }

        return outputDataList;

    }

}
