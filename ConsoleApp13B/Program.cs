/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-12-13
 * Last Modified: 2023-12-13
 * Description: https://adventofcode.com/2023/day/13 - Part Two
 * Keywords: N/A
 */

// Found hint here about focus on lines with one char difference
// https://www.reddit.com/r/adventofcode/comments/18h940b/2023_day_13_solutions/

var fileLines = File.ReadAllLines("input.txt");

var segments = new List<Segment>();
var coordinates = new List<Coordinate>();
var row = 0;
var segmentId = 0;
foreach (var line in fileLines)
{
    if (line.Length == 0)
    {
        row = 0;
        segments.Add(new Segment(segmentId, coordinates));
        segmentId++;
        coordinates = new List<Coordinate>();
    }
    else
    {
        var col = 0;
        foreach (var sign in line)
        {
            coordinates.Add(new Coordinate(col, row, sign));
            col++;
        }

        row++;
    }
}

// Final segment
segments.Add(new Segment(segmentId, coordinates));

var totalValue = 0;
foreach (var segment in segments)
{
    // Identify lines with one char difference
    var compared = new List<(int y1, int y2, int pos)>();
    var yValues = segment.Coordinates.Select(c => c.Y).Distinct().ToArray();
    foreach (var y1 in yValues)
    {
        foreach (var y2 in yValues)
        {
            if (y1 == y2 || compared.Any(c => c.y1 == y2 && c.y2 == y1))
            {
                continue;
            }

            var firstLine = segment.Coordinates
                .Where(c => c.Y == y1)
                .OrderBy(c => c.X)
                .Aggregate("", (current, c) => current + c.Sign);

            var secondLine = segment.Coordinates
                .Where(c => c.Y == y2)
                .OrderBy(c => c.X)
                .Aggregate("", (current, c) => current + c.Sign);

            var pos = GetDifferingCharPosition(firstLine, secondLine);
            if (pos == -1)
            {
                continue;
            }

            Console.WriteLine(
                $"s: {segment.Id}, y1: {y1}, y2: {y2} Found match: {firstLine} and {secondLine} on position {pos}");
            compared.Add((y1, y2, pos));
            break;
        }
    }

    int[] swipes = { 1, 2 };

    // Determine of change of character creates line of reflection with segment scan, keep the highest value
    var comparisonValue = 0;
    foreach (var c in compared)
    {
        foreach (var swipe in swipes)
        {
            var segmentCopy = new Segment(segment.Id, segment.Coordinates);

            switch (swipe)
            {
                case 1:
                {
                    var currentSign = segmentCopy.Coordinates.First(co => co.Y == c.y1 && co.X == c.pos).Sign;
                    segmentCopy.Coordinates.First(co => co.Y == c.y1 && co.X == c.pos).Sign = currentSign == '#' ? '.' : '#';
                    break;
                }
                case 2:
                {
                    var currentSign = segmentCopy.Coordinates.First(co => co.Y == c.y2 && co.X == c.pos).Sign;
                    segmentCopy.Coordinates.First(co => co.Y == c.y2 && co.X == c.pos).Sign = currentSign == '#' ? '.' : '#';
                    break;
                }
            }

            int y;
            
            var linesBetween = c.y2 - c.y1 - 1;
            if (linesBetween == 1)
            {
                continue;
            }
            var mod = linesBetween % 2;
            if (mod > 0)
            {
                continue;
            }
            y = c.y2 - linesBetween / 2;

            if (y > 0)
            {
                var earlierY = y - 1;
                var laterY = y;
                var firstLine = segmentCopy.Coordinates
                    .Where(c => c.Y == earlierY)
                    .OrderBy(c => c.X)
                    .Aggregate("", (current, c) => current + c.Sign);

                var secondLine = segmentCopy.Coordinates
                    .Where(c => c.Y == laterY)
                    .OrderBy(c => c.X)
                    .Aggregate("", (current, c) => current + c.Sign);

                if (firstLine == secondLine)
                {
                    var match = true;
                    while (match)
                    {
                        earlierY--;
                        laterY++;

                        if (earlierY < 0 || laterY > segmentCopy.Coordinates.Max(c => c.Y))
                        {
                            break;
                        }

                        var earlier = segmentCopy.Coordinates
                            .Where(c => c.Y == earlierY)
                            .OrderBy(c => c.X)
                            .Aggregate("", (current, c) => current + c.Sign);

                        var later = segmentCopy.Coordinates
                            .Where(c => c.Y == laterY)
                            .OrderBy(c => c.X)
                            .Aggregate("", (current, c) => current + c.Sign);

                        if (earlier != later)
                        {
                            match = false;
                        }
                    }

                    if (match)
                    {
                        if (y * 100 > comparisonValue)
                        {
                            comparisonValue = y * 100;
                            Console.WriteLine($"comparisonValue: {comparisonValue}");
                        }

                        break;
                    }
                }
            }
        }
    }
    if (comparisonValue == 0)
    {
        Console.WriteLine($"NOT FOUND: {segment.Id}");
    }

    totalValue += comparisonValue;
}

Console.WriteLine($"Answer: {totalValue}");
return;

// Credit: ChatGPT 3.5
static int GetDifferingCharPosition(string str1, string str2)
{
    if (str1.Length != str2.Length || str1 == str2)
        return -1; // Strings have different lengths or are identical
    var differingPosition = -1;
    for (var i = 0; i < str1.Length; i++)
    {
        if (str1[i] == str2[i]) continue;
        if (differingPosition != -1)
            return -1; // More than one difference found

        differingPosition = i;
    }

    return differingPosition;
}

internal class Segment
{
    public readonly int Id;
    public readonly List<Coordinate> Coordinates;

    public Segment(int id, List<Coordinate> coordinates)
    {
        Id = id;
        Coordinates = coordinates;
    }
}

internal class Coordinate
{
    public readonly int X;
    public readonly int Y;
    public char Sign;

    public Coordinate(int x, int y, char sign)
    {
        X = x;
        Y = y;
        Sign = sign;
    }
}