using System.Reflection;

namespace Domain.Enums;

public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>> where TEnum : Enumeration<TEnum>
{
    private static readonly Lazy<IDictionary<int, TEnum>> _enumerations =
        new(() => CreateEnumerationDictionary(typeof(TEnum)));

    public int Id { get; protected set; }
    public string Name { get; protected set; } = string.Empty;

    protected Enumeration(int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }
    public static TEnum? FromValue(int value)
    {
        if (_enumerations.Value.TryGetValue(value, out TEnum? enumeration))
        {
            return enumeration;
        }
        return null;
    }

    public static TEnum? FromName(string name)
    {
        return _enumerations.Value.Values
            .FirstOrDefault(x => 0 == string.Compare(x.Name, name, StringComparison.OrdinalIgnoreCase));
    }

    public static IEnumerable<TEnum> GetValues() => _enumerations.Value.Values;

    public bool Equals(Enumeration<TEnum>? other)
    {
        if (other is null)
        {
            return false;
        }

        return GetType() == other.GetType() && this.Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        return obj is Enumeration<TEnum> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    private static IDictionary<int, TEnum> CreateEnumerationDictionary(Type enumType)
    {
        return GetFieldsForType(enumType).ToDictionary(t => t.Id);
    }

    private static IEnumerable<TEnum> GetFieldsForType(Type enumType)
    {
        return enumType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
              .Where(fieldInfo => enumType.IsAssignableFrom(fieldInfo.FieldType))
              .Select(fieldInfo => (TEnum)fieldInfo.GetValue(default)!);
    }

    protected Enumeration()
    { }

}
