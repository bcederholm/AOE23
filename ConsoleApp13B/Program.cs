/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-12-13
 * Last Modified: 2023-12-13
 * Description: https://adventofcode.com/2023/day/13 - Part Two
 * Keywords: N/A
 */

// Find hint here.. https://www.reddit.com/r/adventofcode/comments/18h940b/2023_day_13_solutions/

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

var value = 0;
foreach (var segmentForLoop in segments)
{

    foreach (var coordinateForLoop in segmentForLoop.Coordinates)
    {
        Console.WriteLine($"segmentForLoop {segmentForLoop.Id} {coordinateForLoop.X} {coordinateForLoop.Y}");
        var segment = new Segment(segmentForLoop.Id, segmentForLoop.Coordinates.Select(c => new Coordinate(c.X, c.Y, c.Sign)).ToList());
        var coordinate = segment.Coordinates.First(c => c.X == coordinateForLoop.X && c.Y == coordinateForLoop.Y);
        coordinate.Sign = coordinate.Sign == '#' ? '.' : '#';

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
                        Console.WriteLine("Match!");
                        break;
                    }
                }
            }
        }
    }
}

Console.WriteLine($"Answer: {value}");

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