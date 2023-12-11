/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-10-08
 * Last Modified: 2023-12-11
 * Description: https://adventofcode.com/2023/day/8 - Part Two
 * Keywords: LCM
 */

const string filePath1 = "input-lr.txt";
const string filePath2 = "input-nodes.txt";
var lines1 = File.ReadAllLines(filePath1);
var lines2 = File.ReadAllLines(filePath2);

var directions = lines1[0].ToCharArray();
var nodes = lines2.Select(line => line.Replace("= (", "")
    .Replace(",", "")
    .Replace(")", ""))
    .Select(cleanedLine => cleanedLine.Split(" ", StringSplitOptions.RemoveEmptyEntries))
    .Select(lineSegments => new Node { Current = lineSegments[0], Left = lineSegments[1], Right = lineSegments[2] })
    .ToList();

var nodesEndingWithA = nodes.Where(n => n.Current.Substring(2,1) == "A").ToList();
var nodeDictionary = nodes.ToDictionary(node => node.Current);

foreach (var nodeEndingWithA in nodesEndingWithA)
{
    var currentNode = nodeDictionary[nodeEndingWithA.Current];
    var found = false;
    
    while (!found)
    {
        foreach (var direction in directions)
        {
            nodeEndingWithA.Iterations++;
            var nextNode = nodeDictionary[direction == 'L' ? currentNode.Left : currentNode.Right];
            currentNode = nodeDictionary[nextNode.Current];
            if (currentNode.Current[2] == 'Z')
            {
                found = true;
                break;
            }
        }
    }
}

var lcmResult = LcmAggregated(nodesEndingWithA.Select(n => n.Iterations).ToArray());

Console.WriteLine($"Answer: {lcmResult}");

return;

// Credit: lcm -> https://www.reddit.com/r/adventofcode/comments/18df7px/2023_day_8_solutions/

static long LcmAggregated(IEnumerable<long> numbers)
{
    return numbers.Aggregate(Lcm);
}
static long Lcm(long a, long b)
{
    return Math.Abs(a * b) / Gcd(a, b);
}

static long Gcd(long a, long b)
{
    while (true)
    {
        if (b == 0) return a;
        var a1 = a;
        a = b;
        b = a1 % b;
    }
}

internal class Node
{
    public string Current { get; init; } = string.Empty;
    public string Left { get; init; } = string.Empty;
    public string Right { get; init; } = string.Empty;
    public long Iterations { get; set; }
}