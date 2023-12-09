using System.Diagnostics;

string filePath1 = "C:\\repos\\offside\\ConsoleApp1\\ConsoleApp8A\\Input8A-lr.txt";
string filePath2 = "C:\\repos\\offside\\ConsoleApp1\\ConsoleApp8A\\Input8A-nodes.txt";
string[] lines1 = File.ReadAllLines(filePath1);
string[] lines2 = File.ReadAllLines(filePath2);


var directions = lines1[0].ToCharArray();
var nodes = new List<Node>();

foreach (var line in lines2)
{
    var cleanedLine = line.Replace("= (", "").Replace(",", "").Replace(")", "");
    var lineSegments = cleanedLine.Split(" ", StringSplitOptions.RemoveEmptyEntries);
    nodes.Add(new Node()
    {
        Current = lineSegments[0],
        Left = lineSegments[1],
        Right = lineSegments[2],
    });
}

var found = false;
var current = "AAA";
var iterations = 0;
while (!found)
{
    foreach (var direction in directions)
    {
        var currentNode = nodes.First(x => x.Current == current);
        var nextNode = nodes.First(x => x.Current == (direction.ToString() == "L" ? currentNode.Left : currentNode.Right));
        current = nextNode.Current;
        iterations++;
        if (current == "ZZZ")
        {
            found = true;
            break;
        }
    }
}

Console.WriteLine($"Iterations: {iterations}");

public class Node
{
    public string Current { get; set; }   
    public string Left { get; set; }
    public string Right { get; set; }

}