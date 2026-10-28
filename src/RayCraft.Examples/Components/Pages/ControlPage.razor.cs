using RayCraft.Examples.Docs;

namespace RayCraft.Examples.Components.Pages;

public partial class ControlPage
{
    private ControlDoc? _control;

    [Parameter] public string Slug { get; set; } = string.Empty;

    protected override void OnParametersSet() => _control = ControlCatalog.Find(Slug);
}
