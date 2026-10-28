namespace RayCraft.Examples.Docs;

public partial class ExampleBox
{
    private string? _source;

    [Parameter, EditorRequired] public Type ExampleType { get; set; } = default!;

    [Parameter, EditorRequired] public string Title { get; set; } = string.Empty;

    protected override void OnParametersSet()
    {
        using var stream = typeof(ExampleBox).Assembly.GetManifestResourceStream($"examples/{ExampleType.Name}.razor");
        if (stream is null)
        {
            _source = null;
            return;
        }

        using var reader = new StreamReader(stream);
        _source = reader.ReadToEnd().TrimEnd();
    }
}
