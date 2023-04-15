using Newtonsoft.Json;

namespace Aki32Utilities.ConsoleAppUtilities.General;
[JsonObject]
public class UtilTestClass2
{
    [JsonProperty("age")]
    public int Age { get; private set; }

    [JsonProperty("name")]
    public string Name { get; private set; }

    public UtilTestClass2(int age, string name)
    {
        Age = age;
        Name = name;
    }
}
