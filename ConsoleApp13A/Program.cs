/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-12-13
 * Last Modified: 2023-12-13
 * Description: https://adventofcode.com/2023/day/13 - Part One
 * Keywords: LINQ Aggregate
 */

var fileLines = File.ReadAllLines("input.txt");

var segments = new List<Segment>();
var coordinates = new List<Coordinate>();
var row = 0;
foreach (var line in fileLines)
{
    if (line.Length == 0)
    {
        row = 0;
        segments.Add(new Segment(coordinates));
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
segments.Add(new Segment(coordinates));

var value = 0;

foreach (var segment in segments)
{
    // Scan horizontally
    var yValues = segment.Coordinates.Select(c => c.Y).Distinct().ToArray();
    foreach (var y in yValues)
    {
        if (y > 0)
        {
            var earlierY = y - 1;
            var laterY = y;
            var firstLine = segment.Coordinates
                .Where(c => c.Y == earlierY)
                .OrderBy(c => c.X)
                .Aggregate("", (current, c) => current + c.Sign);

            var secondLine = segment.Coordinates
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

                    if (earlierY < 0 || laterY > segment.Coordinates.Max(c => c.Y))
                    {
                        break;
                    }

                    var earlier = segment.Coordinates
                        .Where(c => c.Y == earlierY)
                        .OrderBy(c => c.X)
                        .Aggregate("", (current, c) => current + c.Sign);

                    var later = segment.Coordinates
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
                    value += y * 100;
                    break;
                }
            }
        }
    }

    // Scan vertically
    var xValues = segment.Coordinates.Select(c => c.X).Distinct().ToArray();
    foreach (var x in xValues)
    {
        if (x > 0)
        {
            var earlierX = x - 1;
            var laterX = x;
            var firstLine = segment.Coordinates
                .Where(c => c.X == earlierX)
                .OrderBy(c => c.Y)
                .Aggregate("", (current, c) => current + c.Sign);

            var secondLine = segment.Coordinates
                .Where(c => c.X == laterX)
                .OrderBy(c => c.Y)
                .Aggregate("", (current, c) => current + c.Sign);

            if (firstLine == secondLine)
            {
                var match = true;
                while (match)
                {
                    earlierX--;
                    laterX++;

                    if (earlierX < 0 || laterX > segment.Coordinates.Max(c => c.X))
                    {
                        break;
                    }

                    var earlier = segment.Coordinates
                        .Where(c => c.X == earlierX)
                        .OrderBy(c => c.Y)
                        .Aggregate("", (current, c) => current + c.Sign);

                    var later = segment.Coordinates
                        .Where(c => c.X == laterX)
                        .OrderBy(c => c.Y)
                        .Aggregate("", (current, c) => current + c.Sign);

                    if (earlier != later)
                    {
                        match = false;
                    }
                }

                if (match)
                {
                    value += x;
                    break;
                }
            }
        }
    }
}

Console.WriteLine($"Answer: {value}");

internal class Segment
{
    public readonly List<Coordinate> Coordinates;

    public Segment(List<Coordinate> coordinates)
    {
        Coordinates = coordinates;
    }
}

internal class Coordinate
{
    public readonly int X;
    public readonly int Y;
    public readonly char Sign;

    public Coordinate(int x, int y, char sign)
    {
        X = x;
        Y = y;
        Sign = sign;
    }
}