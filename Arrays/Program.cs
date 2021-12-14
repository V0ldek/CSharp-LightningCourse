int[] tab = new int[16];

for (int i = 0; i < tab.Length; ++i)
{
    tab[i] = i;
}

for (int i = 0; i < tab.Length; ++i)
{
    Console.Write(i.ToString() + " ");
}
Console.WriteLine();

foreach (var i in tab)
{
    Console.Write(i.ToString() + " ");
}

Console.WriteLine();

int[][] jagged = new int[4][];
for (int i = 0; i < jagged.Length; ++i)
{
    jagged[i] = new int[i + 1];
}

jagged[1][1] = 1;

Console.WriteLine(jagged[1][1]);
