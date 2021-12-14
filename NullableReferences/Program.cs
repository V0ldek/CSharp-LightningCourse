// This is a warning.
Duck duck = null;

// This is not a warning.
Duck? duck2 = null;

// This is not a warning, since we explicilty silence it.
Duck duck3 = null!;

Print(duck);
Print(duck2);
Print(duck3);

void Print(Duck? duck)
{
    // duck?.Name gives null if duck is null.
    // x ?? "Duck is null" gives x if it is not null, and the rhs message otherwise.
    var message = duck?.Name ?? "Duck is null";

    Console.WriteLine(message);
}

public class Duck
{
    public string Name { get; init; }

    public Duck(string name) => Name = name;
}