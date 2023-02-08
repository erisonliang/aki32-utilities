
namespace Aki32Utilities.ConsoleAppUtilities.General;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class CsvIgnoreAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class CsvHeaderNameAttribute : Attribute
{
    public string Name { get; }

    public CsvHeaderNameAttribute(string name)
    {
        Name = name;
    }

}
