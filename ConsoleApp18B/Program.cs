/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-12-18
 * Last Modified: 2023-12-18
 * Description: https://adventofcode.com/2023/day/18 - Part Two
 * Keywords: Double
 */

var fileLines = File.ReadAllLines("input.txt");

var coordinates = new List<Point>();
(double x, double y) currentCoordinate = (x: 0, y: 0);
coordinates.Add(new Point(currentCoordinate.x, currentCoordinate.y ));
double totalDistance = 0;
foreach (var line in fileLines)
{
    var splitLine = line.Split(' ');

    var direction = splitLine[2][7];
    
    var distance = int.Parse(splitLine[2].Substring(2,5),System.Globalization.NumberStyles.HexNumber);
    switch (direction)
    {
        case '0':
            currentCoordinate.x += distance;
            break;
        case '2':
            currentCoordinate.x -= distance;
            break;
        case '1':
            currentCoordinate.y += distance;
            break;
        case '3':
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
    public double X { get; }
    public double Y { get; }

    public Point(double x, double y)
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