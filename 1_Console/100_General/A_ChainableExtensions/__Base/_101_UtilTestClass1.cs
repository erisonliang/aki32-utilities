using System.Runtime.CompilerServices;
using Aki32Utilities.ConsoleAppUtilities.General;
using System.Text;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public class UtilTestClass1
{
    public string StringProp1 { get; set; }
    public string StringProp2 { get; set; }
    public string StringProp3 { get; set; }
    public int IntProp { get; set; }

    public string[] StringArrayProp { get; set; }
    public int[] IntArrayProp { get; set; }

    public List<string> StringListProp { get; set; }
    public List<int> IntListProp { get; set; }

    [JsonIgnore, XmlIgnore, CsvIgnore]
    public string StringProp_Ignored { get; set; }

    public string IntProp_InString => IntProp.ToString();

}
