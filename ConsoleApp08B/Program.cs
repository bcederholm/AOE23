string filePath1 = "C:\\repos\\offside\\ConsoleApp01\\ConsoleApp08A\\Input8A-lr.txt";
string filePath2 = "C:\\repos\\offside\\ConsoleApp01\\ConsoleApp08A\\Input8A-nodes.txt";
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

var nodesEndingWithA = nodes.Where(n => n.Current.Substring(2,1) == "A").ToList();

var nodeDictionary = nodes.ToDictionary(node => node.Current);
var lowestRecord = 10;

Console.WriteLine($"Start: {DateTime.Now}");

foreach (var nodeEndingWithA in nodesEndingWithA)
{
    var currentNode = nodeDictionary[nodeEndingWithA.Current];
    var found = false;
    
    while (!found)
    {
        foreach (var direction in directions)
        {
            nodeEndingWithA.Iterations++;
            var nextNode = nodeDictionary[(direction == 'L') ? currentNode.Left : currentNode.Right];
            currentNode = nodeDictionary[nextNode.Current];
            if (currentNode.Current[2] == 'Z')
            {
                found = true;
                break;
            }
        }
    }
}

var lcmResult = LCM(nodesEndingWithA.Select(n => n.Iterations).ToArray());


Console.WriteLine($"LCM: {lcmResult}");

// CREDIT: https://stackoverflow.com/a/6251668/12347616
static long LCM(long[] numbers)
{
    return numbers.Aggregate(lcm);
}
static long lcm(long a, long b)
{
    return Math.Abs(a * b) / GCD(a, b);
}
static long GCD(long a, long b)
{
    return b == 0 ? a : GCD(b, a % b);
}

public class Node
{
    public string Current { get; set; }   
    public string Left { get; set; }
    public string Right { get; set; }
    public long Iterations { get; set; }
}