using System.Text.RegularExpressions;

namespace RayCraft.Examples.Docs;

public static partial class ControlCatalog
{
    public static IReadOnlyList<ControlDoc> All { get; } = Build();

    public static IReadOnlyList<IGrouping<string, ControlDoc>> Groups { get; } =
        All.GroupBy(control => control.Category).ToList();

    public static ControlDoc? Find(string slug) =>
        All.FirstOrDefault(control => string.Equals(control.Slug, slug, StringComparison.OrdinalIgnoreCase));

    public static string CategoryTitle(string category)
    {
        var title = Words().Replace(category, " $1").TrimStart();
        return title[..1] + title[1..].ToLowerInvariant();
    }

    private static List<ControlDoc> Build()
    {
        var manual = ManualDocs.All.ToDictionary(doc => doc.Slug);
        var docs = new List<ControlDoc>();
        foreach (var entry in ExampleIndex.Entries)
        {
            var slug = Slugify(entry.Name);
            if (manual.TryGetValue(slug, out var own))
            {
                docs.Add(own with { Category = entry.Category, Parameters = Merge(entry.Name, own.Parameters) });
                continue;
            }

            docs.Add(new(
                slug,
                entry.Name,
                $"Live examples of {entry.Name} with their source, and its parameters.",
                entry.Examples,
                ParameterReflector.Describe(entry.Name),
                entry.Examples.Count == 0 ? ["This control has no example of its own yet."] : [],
                entry.Category));
        }

        return docs;
    }

    private static IReadOnlyList<ParameterRow> Merge(string name, IReadOnlyList<ParameterRow> written)
    {
        var known = written.ToDictionary(row => row.Name, row => row.Description);
        var result = ParameterReflector.Describe(name, known).ToList();
        result.AddRange(written.Where(row => result.All(r => r.Name != row.Name)));
        return result;
    }

    private static string Slugify(string name) => Words().Replace(name, "-$1").TrimStart('-').ToLowerInvariant();

    [GeneratedRegex("(?<!^)([A-Z])")]
    private static partial Regex Words();
}
