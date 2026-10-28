namespace RayCraft.Examples.Docs;

public record ExampleRef(string Title, Type Type);

public record ParameterRow(string Name, string Type, string Default, string Description);

public record ControlDoc(
    string Slug,
    string Name,
    string Lead,
    IReadOnlyList<ExampleRef> Examples,
    IReadOnlyList<ParameterRow> Parameters,
    IReadOnlyList<string> Notes);
