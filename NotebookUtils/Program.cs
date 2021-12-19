// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

var fragments = ReadLines().Select(Fragment.FromLine);
var tableOfContents = new TableOfContents(fragments);

Console.WriteLine(tableOfContents.Display());

var modifiedFragments = tableOfContents.EnrichFragments();

foreach (var fragment in modifiedFragments) 
{
    Console.WriteLine(fragment.Text);
}

IEnumerable<string> ReadLines()
{
    string? line = Console.ReadLine();
    while (line is not null)
    {
        yield return line;
        line = Console.ReadLine();
    }
}

enum FragmentType
{
    Text,
    MarkdownHeader2,
    MarkdownHeader3,
    HtmlHeader2,
    HtmlHeader3
}

class TableOfContents
{
    private const string TocId = "toc";

    private readonly Dictionary<string, int> _usedIds = new Dictionary<string, int>
    {
        {TocId, 1}
    };

    private readonly IReadOnlyList<(Fragment fragment, string? id)> _fragments;

    public TableOfContents(IEnumerable<Fragment> fragments) =>
        _fragments = (from f in fragments
                      let trim = f.TrimMarkup()
                      let id = GenerateId(trim)
                      select (trim, id)).ToList();

    public string Display()
    {
        var result = new StringBuilder($"#!markdown\n<h2 id=\"{TocId}\">Table of Contents</h2>\n\n");
        var index = Index.Zero;

        foreach (var (fragment, id) in _fragments)
        {
            if (fragment.Type == FragmentType.MarkdownHeader2 || fragment.Type == FragmentType.HtmlHeader2)
            {
                Debug.Assert(id is not null);

                index = index.NextSection();
                result.AppendLine($"{index.Section}. <a href=\"#{id}\">{fragment.Text}</a>");
            }
            else if (fragment.Type == FragmentType.MarkdownHeader3 || fragment.Type == FragmentType.HtmlHeader3)
            {
                Debug.Assert(id is not null);

                index = index.NextSubsection();
                result.AppendLine("<br>");
                result.AppendLine($"    {index.Section}.{index.Subsection} <a href=\"#{id}\">{fragment.Text}</a>");
            }
        }

        return result.ToString();
    }

    public IEnumerable<Fragment> EnrichFragments()
    {
        var index = Index.Zero;

        foreach (var (fragment, id) in _fragments)
        {
            int? headerNumber = fragment.Type switch
            {
                FragmentType.MarkdownHeader2 => 2,
                FragmentType.HtmlHeader2 => 2,
                FragmentType.MarkdownHeader3 => 3,
                FragmentType.HtmlHeader3 => 3,
                _ => null
            };

            if (headerNumber is null)
            {
                yield return fragment;
            }
            else if (headerNumber == 2)
            {
                index = index.NextSection();
                var newText = $"<h2 id=\"{id}\">{index.Section}. {fragment.Text}</h2> <small><a href=\"#{TocId}\">Back to top</a></small>";
                yield return fragment with { Text = newText };
            }
            else if (headerNumber == 3)
            {
                index = index.NextSubsection();
                var newText = $"<h3 id=\"{id}\">{index.Section}.{index.Subsection} {fragment.Text}</h3> <small><a href=\"#{TocId}\">Back to top</a></small>";
                yield return fragment with { Text = newText };
            }
        }
    }

    private string? GenerateId(Fragment fragment)
    {
        if (fragment.Type == FragmentType.Text)
        {
            return null;
        }

        var baseId = new string(fragment.Text.ToLowerInvariant().Select(ConvertChar).ToArray());
        var result = baseId.Length > 0 ? baseId : "header-id";

        if (_usedIds.TryGetValue(baseId, out var count))
        {
            result += $"-{count}";
            _usedIds[baseId] = count + 1;
        }
        else
        {
            _usedIds[baseId] = 1;
        }

        return result;

        static char ConvertChar(char ch)
        {
            if (char.IsLetterOrDigit(ch) && char.IsAscii(ch))
            {
                return char.ToLowerInvariant(ch);
            }
            else
            {
                return '-';
            }
        }
    }

    private struct Index
    {
        public int Section { get; init; }

        public int Subsection { get; init; }

        // First section and subsection should be 0, so we start 1 below.
        public static readonly Index Zero = new Index { Section = -1, Subsection = -1 };

        public Index NextSubsection() => this with { Subsection = Subsection + 1 };

        public Index NextSection() => this with { Section = Section + 1, Subsection = Index.Zero.Subsection };
    }
}

readonly struct Fragment
{
    public FragmentType Type { get; init; }

    public string Text { get; init; }

    public Fragment(FragmentType type, string text) => (Type, Text) = (type, text);

    public static Fragment FromLine(string line)
    {
        if (line.StartsWith("## "))
        {
            return new Fragment(FragmentType.MarkdownHeader2, line);
        }
        if (line.StartsWith("### "))
        {
            return new Fragment(FragmentType.MarkdownHeader3, line);
        }
        if (line.EndsWith("</h2>"))
        {
            return new Fragment(FragmentType.HtmlHeader2, line);
        }
        if (line.EndsWith("</h3>"))
        {
            return new Fragment(FragmentType.HtmlHeader3, line);
        }

        return new Fragment(FragmentType.Text, line);
    }

    public Fragment TrimMarkup() => this with
    {
        Text = Type switch
        {
            // Remove leading 3 characters: "## "
            FragmentType.MarkdownHeader2 => Text[3..],
            // Remove leading 4 characters: "### "
            FragmentType.MarkdownHeader3 => Text[4..],
            // Remove all characters up to and including the end of the <h2> opening tag; and
            // Remove all 5 last characters: "</h2>"
            FragmentType.HtmlHeader2 => Text[(Text.IndexOf('>') + 1)..^5],
            // Remove all characters up to and including the end of the <h2> opening tag; and
            // Remove all 5 last characters: "</h2>"
            FragmentType.HtmlHeader3 => Text[(Text.IndexOf('>') + 1)..^5],
            // Remove nothing.
            _ => Text
        }
    };
}