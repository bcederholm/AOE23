/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-12-16
 * Last Modified: 2023-12-16
 * Description: https://adventofcode.com/2023/day/16 - Part One
 * Keywords: N/A
 */

using System.Globalization;

var fileLines = File.ReadAllLines("input.txt");

var coordinates = new List<(int x, int y, char sign)>();

for (var y = 0; y < fileLines.Length; y++)
{
    for (var x = 0; x < fileLines[y].Length; x++)
    {
        coordinates.Add((x, y, fileLines[y][x]));
    }
}

var maxScenarioValue = 0;

for (var xLoop = 0; xLoop <= coordinates.Max(c => c.x); xLoop++)
{
    Console.WriteLine($"xLoop: {xLoop} S");
    ProcessScenerio(xLoop, 0, 'S');
    Console.WriteLine($"xLoop: {xLoop} N");
    ProcessScenerio(xLoop, coordinates.Max(c => c.y), 'N');
}

for (var yLoop = 0; yLoop <= coordinates.Max(c => c.y); yLoop++)
{
    Console.WriteLine($"yLoop: {yLoop} E");
    ProcessScenerio(yLoop, 0, 'E');
    Console.WriteLine($"yLoop: {yLoop} W");
    ProcessScenerio(yLoop, coordinates.Max(c => c.x), 'W');
}

List<(int x, int y, char direction)> values;

void ProcessScenerio(int xScenario, int yScenario, char directionScenario)
{
    values = new List<(int x, int y, char direction)>();

    ProcessMovement(xScenario, yScenario, directionScenario);

    var distinctValues = values.GroupBy(v => new { v.x, v.y}).Select(vv => new {vv.Key.x, vv.Key.y});
    var totalValue = distinctValues.Count();
    
    Console.WriteLine($"totalValue: {totalValue}");
    
    if (totalValue > maxScenarioValue)
    {
        maxScenarioValue = totalValue;
    }
}

Console.WriteLine($"Answer: {maxScenarioValue}");
return;

void ProcessMovement(int xOriginal, int yOriginal, char directionOriginal)
{
    var maxX = coordinates.Max(c => c.x);
    var maxY = coordinates.Max(c => c.y);

    var x = xOriginal;
    var y = yOriginal;
    var direction = directionOriginal;

    while (true)
    {
        if (x < 0 || x > maxX || y < 0 || y > maxY)
        {
            return;
        }

        var currentCoordinate = coordinates.First(c => c.x == x && c.y == y);


        if (values.Any(v => v.x == x && v.y == y && v.direction == direction))
        {
            return;
        }
        else
        {
            values.Add((x, y, direction));
        }

        switch (currentCoordinate.sign, direction)
        {
            case ('.', 'N'):
                y--;
                break;
            case ('.', 'E'):
                x++;
                break;
            case ('.', 'S'):
                y++;
                break;
            case ('.', 'W'):
                x--;
                break;

            case ('\\', 'N'):
                x--;
                direction = 'W';
                break;
            case ('\\', 'E'):
                y++;
                direction = 'S';
                break;
            case ('\\', 'S'):
                x++;
                direction = 'E';
                break;
            case ('\\', 'W'):
                y--;
                direction = 'N';
                break;

            case ('/', 'N'):
                x++;
                direction = 'E';
                break;
            case ('/', 'E'):
                y--;
                direction = 'N';
                break;
            case ('/', 'S'):
                x--;
                direction = 'W';
                break;
            case ('/', 'W'):
                y++;
                direction = 'S';
                break;

            case ('|', 'N'):
                y--;
                direction = 'N';
                break;
            case ('|', 'E'):
            case ('|', 'W'):
                ProcessMovement(x, y - 1, 'N');
                ProcessMovement(x, y + 1, 'S');
                return;
            case ('|', 'S'):
                y++;
                direction = 'S';
                break;

            case ('-', 'N'):
            case ('-', 'S'):
                ProcessMovement(x - 1, y, 'W');
                ProcessMovement(x + 1, y, 'E');
                return;
            case ('-', 'E'):
                x++;
                direction = 'E';
                break;
            case ('-', 'W'):
                x--;
                direction = 'W';
                break;
        }
    }
}