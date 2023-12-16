/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-12-16
 * Last Modified: 2023-12-16
 * Description: https://adventofcode.com/2023/day/16 - Part Two
 * Keywords: N/A
 */

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
    ProcessScenario(xLoop, 0, 'S');
    Console.WriteLine($"xLoop: {xLoop} N");
    ProcessScenario(xLoop, coordinates.Max(c => c.y), 'N');
}

for (var yLoop = 0; yLoop <= coordinates.Max(c => c.y); yLoop++)
{
    Console.WriteLine($"yLoop: {yLoop} E");
    ProcessScenario(0, yLoop, 'E');
    Console.WriteLine($"yLoop: {yLoop} W");
    ProcessScenario(coordinates.Max(c => c.x), yLoop, 'W');
}

List<(int x, int y, char direction)> values;

Console.WriteLine($"Answer: {maxScenarioValue}");
return;

void ProcessScenario(int xScenario, int yScenario, char directionScenario)
{
    values = new List<(int x, int y, char direction)>();
    ProcessMovement(xScenario, yScenario, directionScenario);
    var distinctValues = values.GroupBy(v => new { v.x, v.y}).Select(vv => new {vv.Key.x, vv.Key.y});
    var totalValue = distinctValues.Count();
    if (totalValue > maxScenarioValue)
    {
        maxScenarioValue = totalValue;
    }
}

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

        var x1 = x;
        var y1 = y;
        var currentCoordinate = coordinates.First(c => c.x == x1 && c.y == y1);


        var x2 = x;
        var y2 = y;
        var direction1 = direction;
        if (values.Any(v => v.x == x2 && v.y == y2 && v.direction == direction1))
        {
            return;
        }

        values.Add((x, y, direction));

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