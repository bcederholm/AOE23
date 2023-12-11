/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-10-08
 * Last Modified: 2023-12-11
 * Description: https://adventofcode.com/2023/day/8 - Part One
 * Keywords: N/A
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

Console.WriteLine($"Answer: {iterations}");

internal class Node
{
    public string? Current { get; init; }   
    public string? Left { get; init; }
    public string? Right { get; init; }
}