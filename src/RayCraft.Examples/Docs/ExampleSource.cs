namespace RayCraft.Examples.Docs;

public static class ExampleSource
{
    public static string? Read(Type example)
    {
        using var stream = typeof(ExampleSource).Assembly.GetManifestResourceStream($"examples/{example.Name}.razor");
        if (stream is null) return null;

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd().TrimEnd();
    }
}
