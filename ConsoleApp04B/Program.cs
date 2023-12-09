Console.WriteLine("Hello, World!");

string filePath = "C:\\repos\\offside\\ConsoleApp01\\ConsoleApp04B\\Input4B.txt";
string[] lines = File.ReadAllLines(filePath);
char[] splitters = new char[] { '|', ':' };
List<LineExtended> lineExtendeds = new List<LineExtended>();

foreach (var line in lines)
{
    var line2 = line.Replace("  ", " ");
    line2 = line2.Replace("  ", " ");
    line2 = line2.Replace("Card ", "");
    
    var sections = line2.Split(splitters, StringSplitOptions.RemoveEmptyEntries);
    var winningNumbers = sections[1].Trim().Split(' ');
    var myNumbers = sections[2].Trim().Split(' ');
    lineExtendeds.Add(new LineExtended()
    {
        CardNumber = int.Parse(sections[0]),
        Matches = winningNumbers.Intersect(myNumbers).Select(n => int.Parse(n)).ToArray().Length,
        Quantity = 1,
    });
}

foreach (var line in lineExtendeds)
{
    if (line.Matches > 0)
    {
        for (var i = line.CardNumber + 1; i <= line.CardNumber + line.Matches; i++)
        {
            if (i <= lineExtendeds.Count)
            {
                lineExtendeds[i - 1].Quantity += line.Quantity;
            }
        }
    }
}

Console.WriteLine(lineExtendeds.Select(l => l.Quantity).Sum());
    

public class LineExtended
{
    public int CardNumber { get; set; }
    public int Matches { get; set; }
    public int Quantity { get; set; }
}