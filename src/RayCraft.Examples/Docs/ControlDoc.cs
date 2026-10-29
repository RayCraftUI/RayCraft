namespace RayCraft.Examples.Docs;

public record ExampleRef(string Title, Type Type);

public record IndexEntry(string Category, string Name, IReadOnlyList<ExampleRef> Examples, ExampleRef? Usage = null, bool UsageBorrowed = false);

public record ParameterRow(string Name, string Type, string Default, string Description);

public record ControlDoc(
    string Slug,
    string Name,
    string Lead,
    IReadOnlyList<ExampleRef> Examples,
    IReadOnlyList<ParameterRow> Parameters,
    IReadOnlyList<string> Notes,
    string Category = "",
    ExampleRef? Usage = null,
    bool UsageBorrowed = false);
