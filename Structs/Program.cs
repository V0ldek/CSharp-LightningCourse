Console.WriteLine("Structs are copied.");
var data = new SomeStruct();
data.SomeInt = 42;
Console.WriteLine($"Data before: {data.SomeInt}");
Foo(data);
Console.WriteLine($"Data after: {data.SomeInt}");

SomeStruct? nullable = null;
PrintValue(nullable);
nullable = new SomeStruct();
PrintValue(nullable);

void Foo(SomeStruct someStruct)
{
    Console.WriteLine($"Data at start of Foo: {someStruct.SomeInt}");
    someStruct.SomeInt = 37;
    Console.WriteLine($"Data at end of Foo: {someStruct.SomeInt}");
}

void PrintValue(SomeStruct? data)
{
    if (data.HasValue)
    {
        Console.WriteLine(data.Value.SomeInt);
    }
    else
    {
        Console.WriteLine("null");
    }
}

public struct SomeStruct
{
    public int SomeInt { get; set; }
}
