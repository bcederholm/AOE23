/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-10-03
 * Last Modified: 2023-12-10
 * Description: https://adventofcode.com/2023/day/3 - Part Two
 * Lessons learned: Linq
 */

var connections = new List<Connection>();

const string filePath = "input.txt";
var lines = File.ReadAllLines(filePath);

var firstPos = -1;
var lastPos = -1;

for (var i = 0; i < lines.Length; i++)
{
    var previousLine = i > 0 ? lines[i - 1] : "";
    var currentLine = lines[i];
    var nextLine = i < lines.Length - 1 ? lines[i + 1] : "";
    
    for (var p = 0; p <= currentLine.Length - 1; p++)
    {
        if (firstPos == -1 && char.IsNumber(currentLine[p]))
        {
            firstPos = p;
        }
        
        if (firstPos != -1)
        {
            if (!char.IsNumber(currentLine[p]))
            {
                lastPos = p - 1;
            }
            
            if (lastPos == -1 && p == currentLine.Length - 1)
            {
                lastPos = p;
            }
        }
        
        if (firstPos != -1 && lastPos != -1)
        {
            var number = currentLine.Substring(firstPos, lastPos - firstPos + 1);

            // Check previous line
            if (previousLine != "")
            {
                for (var pl = firstPos == 0 ? firstPos : firstPos - 1; pl <= (lastPos == currentLine.Length - 1 ? lastPos : lastPos + 1); pl++)
                {
                    if (previousLine[pl] == '*')
                    {
                        connections.Add(new Connection { Line = i - 1, Position = pl, Number = int.Parse(number) });
                    }
                }
            }

            // Check character before
            if (firstPos > 0)
            {
                var characterBefore = currentLine[firstPos - 1];
                if (characterBefore == '*')
                {
                    connections.Add(new Connection { Line = i, Position = firstPos - 1, Number = int.Parse(number) });
                }
            }

            // Check character after
            if (lastPos < currentLine.Length - 1)
            {
                var characterAfter = currentLine[lastPos + 1];
                if (characterAfter == '*')
                {
                    connections.Add(new Connection { Line = i, Position = lastPos + 1, Number = int.Parse(number) });
                }
            }

            // Check next line
            if (nextLine != "")
            {
                for (var np = firstPos == 0 ? firstPos : firstPos - 1; np <= (lastPos == currentLine.Length - 1 ? lastPos : lastPos + 1); np++)
                {
                    if (nextLine[np] == '*')
                    {
                        connections.Add(new Connection { Line = i + 1, Position = np, Number = int.Parse(number) });
                    }
                }
            }

            firstPos = -1;
            lastPos = -1;
        }
    }
}

var duplicateConnections = connections
    .GroupBy(c => new { c.Line, c.Position })
    .Where(g => g.Count() == 2)
    .SelectMany(g => g);

var result = duplicateConnections
    .GroupBy(c => new { c.Line, c.Position })
    .Select(g => new
    {
        g.Key.Line,
        g.Key.Position,
        MultiplicationResult = g.Select(c => c.Number).Aggregate((a, b) => a * b)
    });

var totalSum = result.Sum(item => item.MultiplicationResult);

Console.WriteLine($"Answer: {totalSum}");

internal class Connection
{ 
    public int Line;
    public int Position;
    public int Number;
}