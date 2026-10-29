namespace RayCraft.Examples.Docs;

public partial class ExampleBox
{
    private string? _source;

    [Parameter, EditorRequired] public Type ExampleType { get; set; } = default!;

    [Parameter, EditorRequired] public string Title { get; set; } = string.Empty;

    protected override void OnParametersSet() => _source = ExampleSource.Read(ExampleType);
}
