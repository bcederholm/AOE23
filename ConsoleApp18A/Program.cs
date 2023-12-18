/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-12-16
 * Date Modified: 2023-12-18
 * Last Modified: 2023-12-18
 * Description: https://adventofcode.com/2023/day/18 - Part One
 * Keywords: Shoelace formula
 */

var fileLines = File.ReadAllLines("input.txt");

var coordinates = new List<Point>();
var currentCoordinate = (x: 0, y: 0);
coordinates.Add(new Point(currentCoordinate.x, currentCoordinate.y ));
var totalDistance = 0;
foreach (var line in fileLines)
{
    var splitLine = line.Split(' ');

    var direction = splitLine[0][0];
    var distance = int.Parse(splitLine[1]);
    switch (direction)
    {
        case 'R':
            currentCoordinate.x += distance;
            break;
        case 'L':
            currentCoordinate.x -= distance;
            break;
        case 'D':
            currentCoordinate.y += distance;
            break;
        case 'U':
            currentCoordinate.y -= distance;
            break;

    }
    coordinates.Add(new Point(currentCoordinate.x, currentCoordinate.y ));
    totalDistance += distance;
}

var polygon = new Polygon(coordinates);
var area = polygon.CalculateArea();
Console.WriteLine($"Answer: {area + 1 + totalDistance / 2}");

internal class Point
{
    public int X { get; }
    public int Y { get; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}

internal class Polygon
{
    private List<Point> Vertices { get; }

    public Polygon(List<Point> vertices)
    {
        Vertices = vertices;
    }

    public double CalculateArea()
    {
        var n = Vertices.Count;
        double area = 0;

        for (var i = 0; i < n; i++)
        {
            var j = (i + 1) % n;
            area += Vertices[i].X * Vertices[j].Y;
            area -= Vertices[j].X * Vertices[i].Y;
        }

        area = Math.Abs(area) / 2.0;
        return area;
    }
}
