// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

string filePath = "C:\\repos\\offside\\ConsoleApp01\\ConsoleApp04A\\Input4A.txt";
string[] lines = File.ReadAllLines(filePath);

char[] splitters = new char[] { '|', ':' };

var totalSum = 0;

foreach (var line in lines)
{
    var line2 = line.Replace("  ", " ");
    line2 = line2.Replace("  ", " ");
    line2 = line2.Replace("Card ", "");
    
    var sections = line2.Split(splitters, StringSplitOptions.RemoveEmptyEntries);
    var winningNumbers = sections[1].Trim().Split(' ');
    var myNumbers = sections[2].Trim().Split(' ');
    var matches = myNumbers.Intersect(winningNumbers).ToArray();
    
    if (matches.Length > 0)
    {
        var points = Math.Pow(2, matches.Length -1);
        totalSum += (int)(points);
    }
}

Console.WriteLine(totalSum);