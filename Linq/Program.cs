Console.WriteLine("Select:");
Console.WriteLine();

var ducks = new List<Duck>
{
    new Duck("Jacuś"),
    new Duck("Piotruś"),
    new Duck("Azathoth")
};

foreach (var name in ducks.Select(d => d.Name))
{
    Console.WriteLine(name);
}

Console.WriteLine("Where:");
Console.WriteLine();

var values = new[] { 7, 42, 21, 1, 8 };

foreach (var val in values.Where(v => v >= 8))
{
    Console.WriteLine(val);
}

Console.WriteLine("SelectMany:");
Console.WriteLine();

ducks = new List<Duck>
{
    new Duck("Jacuś"),
    new Duck("Piotruś")
};

foreach (var letter in ducks.SelectMany(d => d.Name.ToCharArray()))
{
    Console.WriteLine(letter);
}

Console.WriteLine("GroupBy:");
Console.WriteLine();

ducks = new List<Duck>
{
    new Duck("Jacuś", Color.Yellow),
    new Duck("Piotruś", Color.Red),
    new Duck("Jacuś", Color.Green)
};

foreach (var grouping in ducks.GroupBy(d => d.Name))
{
    Console.WriteLine($"Ducks with name {grouping.Key}:");
    foreach (var duck in grouping)
    {
        Console.WriteLine($"{duck.Name}, {duck.Color} color");
    }
}

Console.WriteLine("OrderBy, ThenBy:");
Console.WriteLine();

ducks = new List<Duck>
{
    new Duck("Jacuś"),
    new Duck("Piotruś"),
    new Duck("Azathoth"),
    new Duck("Psuchawrl")
};

foreach (var duck in ducks.OrderBy(d => d.Name[0]).ThenBy(d => d.Name.Length))
{
    Console.WriteLine(duck.Name);
}

Console.WriteLine("Join:");
Console.WriteLine();

ducks = new List<Duck>
{
    new Duck("Jacuś", Color.Yellow),
    new Duck("Jacuś", Color.Green),
    new Duck("Piotruś", Color.Yellow)
};
var joined = from d1 in ducks
             join d2 in ducks on d1.Name equals d2.Name
             select (d1.Name, d1.Color, d2.Color);


foreach (var (name, colorA, colorB) in joined)
{
    Console.WriteLine($"{name}: {colorA} - {colorB}");
}

Console.WriteLine("Aggregate:");
Console.WriteLine();

ducks = new List<Duck>
{
    new Duck("Jacuś"),
    new Duck("Piotruś"),
    new Duck("Azathoth"),
    new Duck("Psuchawrl")
};

Console.WriteLine(ducks.Aggregate(0, (a, d) => (a + d.Name.Length) % 2));

Console.WriteLine("First:");
Console.WriteLine();

ducks = new List<Duck>
{
    new Duck("Jacuś"),
    new Duck("Piotruś"),
    new Duck("Azathoth"),
    new Duck("Psuchawrl")
};

var firstLong = ducks.First(d => d.Name.Length > 7);
Console.WriteLine(firstLong.Name);

// DEFFERED EXECUTION

// Linq queries do not execute unless explicitly materialised,
// for example with ToList().
Console.WriteLine("Deffered execution.");
Console.WriteLine();

ducks = new List<Duck>
{
    new Duck("Jacuś"),
    new Duck("Piotruś"),
};

var selectWithSideEffects = ducks.Select(d =>
{
    Console.WriteLine($"Selecting {d.Name}");
    return d.Name;
});
Console.WriteLine("Enumerating #1:");
Console.WriteLine();

foreach (var name in selectWithSideEffects)
{
    Console.WriteLine($"Printing {name}");
}

Console.WriteLine("Enumerating #2:");
Console.WriteLine();
foreach (var name in selectWithSideEffects)
{
    Console.WriteLine($"Printing {name}");
}

Console.WriteLine("ToList:");
Console.WriteLine();
var selectList = selectWithSideEffects.ToList();

Console.WriteLine("Enumerating list:");
Console.WriteLine();
foreach (var name in selectList)
{
    Console.WriteLine($"Printing {name}");
}

public class Duck
{
    public Color Color { get; init; } = Color.Yellow;

    public string Name { get; init; }

    public Duck(string name) => Name = name;

    public Duck(string name, Color color) => (Name, Color) = (name, color);
}

public enum Color
{
    Red = 1,
    Yellow = 2,
    Green = 4,
    Blue = 8,
    Black = 16
}