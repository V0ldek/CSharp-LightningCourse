var mix = Color.Red | Color.Yellow | Color.Black;
Console.WriteLine((int)mix);

var duck = new Duck("Jacuś");
BetterDuck betterDuck = new BetterDuck("Lepszy Jacuś")
{
    Color = Color.Green
};
Duck betterDuckDisguisedAsANormalDuck = new BetterDuck("Lepszy Jacuś w przebraniu");

Console.WriteLine(duck.TimesSqueaked); // Calls the get method.
Console.WriteLine(duck.Name);

Console.WriteLine("Static squeaking:");
Squeaking.SqueakADuck(duck);
Squeaking.SqueakADuck(betterDuck);
Squeaking.SqueakADuck(betterDuckDisguisedAsANormalDuck);

Console.WriteLine("Virtual squeaking:");
Squeaking.SqueakADuck(duck);
Squeaking.SqueakADuck(betterDuck);
Squeaking.SqueakADuck(betterDuckDisguisedAsANormalDuck);

public static class Squeaking
{
    public static void SqueakADuck(Duck duck) =>
        Console.WriteLine("Static squeak!");

    public static void SqueakADuck(BetterDuck duck) =>
        Console.WriteLine("Better static squeak!");
}

public struct DuckData
{
    public string Name { get; }
    public Color Color { get; }

    public DuckData(string name, Color color)
    {
        Name = name;
        Color = color;
    }
}

public interface IDuck
{
    int TimesSqueaked { get; }
    void Squeak();
}

public abstract class DuckBase : IDuck
{
    public int TimesSqueaked { get; private set; }

    public void Squeak()
    {
        TimesSqueaked += 1;
        ProcessSqueaked();
    }

    protected abstract void ProcessSqueaked();
}

public class Duck : DuckBase
{
    public DuckData Data => new(Name, Color);

    public Color Color { get; init; } = Color.Yellow;

    public string Name { get; init;  }

    public Duck(string name) => Name = name;

    protected override void ProcessSqueaked() => Console.WriteLine("Squeak!");
}

public class BetterDuck : Duck
{
    public BetterDuck(string name) : base(name)
    {
    }

    protected override void ProcessSqueaked() => Console.WriteLine("Better squeak!");
}

public enum Color
{
    Red = 1,
    Yellow = 2,
    Green = 4,
    Blue = 8,
    Black = 16
}
