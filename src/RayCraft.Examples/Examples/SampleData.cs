namespace RayCraft.Examples.Examples;

public record Person(string Name, string Department, int Age, decimal Salary, bool Active);

public record Folder(string Name, List<Folder> Children);

public static class SampleData
{
    public static readonly List<Person> People =
    [
        new("Ivan Petrov", "Engineering", 34, 5200m, true),
        new("Maria Georgieva", "Design", 29, 4300m, true),
        new("Georgi Dimitrov", "Sales", 41, 4800m, false),
        new("Elena Stoyanova", "Engineering", 27, 4600m, true),
        new("Nikolay Kolev", "Support", 38, 3100m, true),
        new("Desislava Ivanova", "Marketing", 31, 3900m, true),
        new("Stefan Angelov", "Engineering", 45, 6400m, true),
        new("Viktoria Todorova", "Design", 26, 3700m, false),
        new("Plamen Hristov", "Sales", 36, 4500m, true),
        new("Ralitsa Marinova", "Support", 33, 3300m, true),
        new("Boris Yordanov", "Engineering", 30, 5000m, true),
        new("Katya Vasileva", "Marketing", 39, 4100m, false),
        new("Dimitar Popov", "Sales", 28, 3600m, true),
        new("Silvia Nikolova", "Design", 35, 4700m, true),
        new("Hristo Mladenov", "Support", 42, 3400m, true),
        new("Tsvetelina Kirova", "Engineering", 24, 3900m, true),
        new("Aleksandar Zlatev", "Marketing", 37, 4400m, true),
        new("Mihaela Dobreva", "Sales", 32, 4200m, false),
        new("Kiril Borisov", "Engineering", 40, 5800m, true),
        new("Radoslava Pavlova", "Design", 28, 4000m, true),
        new("Emil Stanchev", "Support", 25, 2900m, true),
        new("Gergana Ilieva", "Marketing", 34, 4000m, true),
        new("Vladimir Rusev", "Sales", 47, 5100m, true),
        new("Yana Belcheva", "Engineering", 29, 4900m, true),
        new("Todor Slavov", "Design", 43, 5300m, false)
    ];

    public static readonly List<Folder> Folders =
    [
        new("Documents",
        [
            new("Invoices", [new("2025", []), new("2026", [])]),
            new("Contracts", [])
        ]),
        new("Pictures",
        [
            new("Holidays", []),
            new("Family", [])
        ]),
        new("Notes", [])
    ];
}
