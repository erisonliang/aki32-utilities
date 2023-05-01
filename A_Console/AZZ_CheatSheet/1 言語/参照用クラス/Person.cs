using Newtonsoft.Json;

namespace Aki32Utilities.ConsoleAppUtilities.CheatSheet;
[JsonObject]
public class Person
{
    [JsonProperty("age")]
    public int Age { get; private set; }

    [JsonProperty("name")]
    public string Name { get; private set; }

    public Person(int age, string name)
    {
        Age = age;
        Name = name;
    }
}
