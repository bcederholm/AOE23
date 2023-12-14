/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-12-14
 * Last Modified: 2023-12-14
 * Description: https://adventofcode.com/2023/day/14 - Part One
 * Keywords: N/A
 */

var fileLines = File.ReadAllLines("input.txt");

var coordinates = new List<Coordinate>();
var row = 0;
foreach (var line in fileLines)
{
        var col = 0;
        foreach (var sign in line)
        {
            coordinates.Add(new Coordinate(col, row, sign));
            col++;
        }

        row++;
}

var xValues = coordinates.Select(c => c.X).Distinct().ToArray();
var yValues = coordinates.Select(c => c.Y).Distinct().ToArray();
foreach (var x in xValues)
{
    foreach (var y in yValues)
    {
        Console.WriteLine($"x: {x} y: {y}");
        var coordinate = coordinates.First(c => c.X == x && c.Y == y);
        if (coordinate.Sign == 'O')
        {
            var continueUp = true;
            var stepY = y;
            while (continueUp)
            {
                if (stepY == 0)
                {
                    break;
                }
                var currentCoordinate = coordinates.First(c => c.X == x && c.Y == stepY);
                var prevCoordinate = coordinates.First(c => c.X == x && c.Y == stepY - 1);
                switch (prevCoordinate.Sign)
                {
                    case '#':
                    case 'O':
                        continueUp = false;
                        break;
                    case '.':
                        prevCoordinate.Sign = 'O';
                        currentCoordinate.Sign = '.';
                        stepY--;
                        break;
                }
            }
        }
    }
}

const string output = "output.txt";
using var writer = new StreamWriter(output);
for (var y = 0; y < yValues.Length; y++) {
    for (var x = 0; x < xValues.Length; x++)
    {
        writer.Write(coordinates.Where(c => c.Y == y && c.X == x).Select(c => c.Sign).First());
    }
    writer.WriteLine();
}


var maxY = yValues.Length;
var sum = 0;
foreach (var y in yValues)
{
    var lineSum = coordinates.Where(c => c.Y == y && c.Sign == 'O').Select(c => c.X).Count() * (maxY - y);
    Console.WriteLine($"y: {y} lineSum: {lineSum}");
    sum+= lineSum;
}

Console.WriteLine($"Answer: {sum}");

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