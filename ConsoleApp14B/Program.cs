/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-12-14
 * Last Modified: 2023-12-14
 * Description: https://adventofcode.com/2023/day/14 - Part Two
 * Keywords: N/A                  *** NOT SOLVED ***
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

for (var i = 0; i < 100; i++)
{
    coordinates = Process(coordinates); // North
    coordinates = Rotate(coordinates);
    coordinates = Rotate(coordinates);
    coordinates = Rotate(coordinates);
    coordinates = Process(coordinates); // West
    coordinates = Rotate(coordinates);
    coordinates = Rotate(coordinates);
    coordinates = Rotate(coordinates);
    coordinates = Process(coordinates); // South
    coordinates = Rotate(coordinates);
    coordinates = Rotate(coordinates);
    coordinates = Rotate(coordinates);
    coordinates = Process(coordinates); // East
    coordinates = Rotate(coordinates);
    coordinates = Rotate(coordinates);
    coordinates = Rotate(coordinates);
}

var xValuesForFile = coordinates.Select(c => c.X).Distinct().ToArray();
var yValuesForFile = coordinates.Select(c => c.Y).Distinct().ToArray();
const string output = "output.txt";
using var writer = new StreamWriter(output);
for (var y = 0; y < yValuesForFile.Length; y++)
{
    for (var x = 0; x < xValuesForFile.Length; x++)
    {
        writer.Write(coordinates.Where(c => c.Y == y && c.X == x).Select(c => c.Sign).First());
    }
    writer.WriteLine();
}

List<Coordinate> Process(List<Coordinate> coordinatesToProcess)
{
    var xValues = coordinatesToProcess.Select(c => c.X).Distinct().ToArray();
    var yValues = coordinatesToProcess.Select(c => c.Y).Distinct().ToArray();
    foreach (var x in xValues)
    {
        foreach (var y in yValues)
        {
            var coordinate = coordinatesToProcess.First(c => c.X == x && c.Y == y);
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

                    var currentCoordinate = coordinatesToProcess.First(c => c.X == x && c.Y == stepY);
                    var prevCoordinate = coordinatesToProcess.First(c => c.X == x && c.Y == stepY - 1);
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

    return coordinatesToProcess;
}

var maxY = coordinates.Select(c => c.Y).Distinct().Max();
var sum = 0;
for (var y = 0; y <= maxY; y++)
{
    var lineSum = coordinates.Where(c => c.Y == y && c.Sign == 'O').Select(c => c.X).Count() * (maxY + 1 - y);
    Console.WriteLine($"y: {y} lineSum: {lineSum}");
    sum += lineSum;
}

Console.WriteLine($"Answer: {sum}");
return;

// Credit: ChatGPT 3.5
List<Coordinate> Rotate(List<Coordinate> coordinatesToRotate)
{
    // Create a 2D array from coordinates
    var matrix = new char[coordinatesToRotate.Max(c => c.X+1), coordinatesToRotate.Max(c => c.Y+1)];

    foreach (var c in coordinatesToRotate)
    {
        matrix[c.X, c.Y] = c.Sign;
    }

    // Transpose the matrix
    for (var i = 0; i < matrix.GetLength(0); i++)
    {
        for (var j = i + 1; j < matrix.GetLength(1); j++)
        {
            (matrix[i, j], matrix[j, i]) = (matrix[j, i], matrix[i, j]);
        }
    }

    // Reverse each row
    for (var i = 0; i < matrix.GetLength(0); i++)
    {
        var left = 0;
        var right = matrix.GetLength(1) - 1;
        while (left < right)
        {
            (matrix[i, left], matrix[i, right]) = (matrix[i, right], matrix[i, left]);
            left++;
            right--;
        }
    }

    var rotatedCoordinates = new List<Coordinate>();

    for (var i = 0; i < matrix.GetLength(0); i++)
    {
        for (var j = 0; j < matrix.GetLength(1); j++)
        {
            rotatedCoordinates.Add(new Coordinate(i, j, matrix[i, j]));
        }
    }

    return rotatedCoordinates;
}

internal class Coordinate
{
    public int X;
    public int Y;
    public char Sign;

    public Coordinate(int x, int y, char sign)
    {
        X = x;
        Y = y;
        Sign = sign;
    }
}