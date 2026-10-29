using System.Globalization;
using System.Reflection;

namespace RayCraft.Examples.Docs;

public static class ParameterReflector
{
    private static readonly Dictionary<string, string> CommonDescriptions = new()
    {
        ["Class"] = "Extra CSS classes for the root element.",
        ["Style"] = "Inline style for the root element.",
        ["AdditionalAttributes"] = "Extra HTML attributes copied to the root element.",
        ["ChildContent"] = "The content of the control.",
        ["Disabled"] = "Turns the control off."
    };

    private static readonly Type[] ProbeTypes = [typeof(string), typeof(int), typeof(decimal), typeof(object)];

    private static readonly Dictionary<string, Type> Components = typeof(Button).Assembly
        .GetExportedTypes()
        .Where(type => typeof(IComponent).IsAssignableFrom(type) && type.Namespace == typeof(Button).Namespace)
        .ToDictionary(type => type.IsGenericTypeDefinition ? type.Name[..type.Name.IndexOf('`')] : type.Name);

    public static IReadOnlyList<ParameterRow> Describe(string name, IReadOnlyDictionary<string, string>? known = null)
    {
        if (!Components.TryGetValue(name, out var type)) return [];

        var instance = CreateInstance(type);
        var rows = new List<ParameterRow>();
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                     .Where(p => p.GetCustomAttribute<ParameterAttribute>() is not null)
                     .OrderBy(p => Depth(p.DeclaringType!, type))
                     .ThenBy(p => p.MetadataToken))
        {
            var description = Lookup(known, property.Name)
                ?? Lookup(WrittenDocs.Parameters.GetValueOrDefault(name), property.Name)
                ?? Lookup(WrittenDocs.Common, property.Name)
                ?? CommonDescriptions.GetValueOrDefault(property.Name, "");
            rows.Add(new(property.Name, TypeName(property.PropertyType), DefaultOf(instance, property), description));
        }

        return rows;
    }

    private static string? Lookup(IReadOnlyDictionary<string, string>? map, string key) =>
        map is not null && map.TryGetValue(key, out var text) && text.Length > 0 ? text : null;

    private static int Depth(Type declaring, Type type)
    {
        var depth = 0;
        for (var current = type; current is not null && current != declaring; current = current.BaseType) depth++;
        return -depth;
    }

    private static object? CreateInstance(Type type)
    {
        if (!type.IsGenericTypeDefinition) return TryCreate(type);

        foreach (var probe in ProbeTypes)
        {
            try
            {
                var created = TryCreate(type.MakeGenericType(probe));
                if (created is not null) return created;
            }
            catch (ArgumentException)
            {
            }
        }

        return null;
    }

    private static object? TryCreate(Type type)
    {
        try
        {
            return Activator.CreateInstance(type);
        }
        catch
        {
            return null;
        }
    }

    private static bool IsCallbackOrFragment(Type type) =>
        type == typeof(EventCallback)
        || typeof(Delegate).IsAssignableFrom(type)
        || type == typeof(RenderFragment)
        || type.IsGenericType && type.GetGenericTypeDefinition() is var open
            && (open == typeof(EventCallback<>) || open == typeof(RenderFragment<>));

    private static string DefaultOf(object? instance, PropertyInfo property)
    {
        if (instance is null || IsCallbackOrFragment(property.PropertyType)) return "";

        try
        {
            var value = instance.GetType().GetProperty(property.Name)!.GetValue(instance);
            return value switch
            {
                null => "null",
                string { Length: 0 } => "\"\"",
                string text => $"\"{text}\"",
                bool flag => flag ? "true" : "false",
                System.Collections.IEnumerable list => list.Cast<object>().Any() ? "set" : "empty",
                _ => Convert.ToString(value, CultureInfo.InvariantCulture) ?? ""
            };
        }
        catch
        {
            return "";
        }
    }

    private static string TypeName(Type type)
    {
        var nullable = Nullable.GetUnderlyingType(type);
        if (nullable is not null) return TypeName(nullable) + "?";
        if (type.IsGenericParameter) return type.Name;
        if (type.IsArray) return TypeName(type.GetElementType()!) + "[]";

        if (type == typeof(string)) return "string";
        if (type == typeof(int)) return "int";
        if (type == typeof(long)) return "long";
        if (type == typeof(double)) return "double";
        if (type == typeof(decimal)) return "decimal";
        if (type == typeof(bool)) return "bool";
        if (type == typeof(object)) return "object";
        if (!type.IsGenericType) return type.Name;

        var name = type.Name[..type.Name.IndexOf('`')];
        return $"{name}<{string.Join(", ", type.GetGenericArguments().Select(TypeName))}>";
    }
}
