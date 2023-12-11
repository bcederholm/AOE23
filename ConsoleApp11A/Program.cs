/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-12-11
 * Last Modified: 2023-12-11
 * Description: https://adventofcode.com/2023/day/11 - Part One (1h 0m)
 * Keywords: Tuple
 */

string filePath1 = "input.txt";
var lines = File.ReadAllLines(filePath1);
var linesExpandedY = new List<string>();
var columnsWithGalaxy = new List<int>();

foreach (var line in lines)
{
    linesExpandedY.Add(line);
    if (!line.Contains('#'))
    {
        linesExpandedY.Add(line);
    }

    for (var i = 0; i < line.Length; i++)
    {
        if (line[i] == '#' && !columnsWithGalaxy.Contains(i))
        {
            columnsWithGalaxy.Add(i);
        }
    }
}

var linesExpanded = new List<string>();
foreach (var line in linesExpandedY)
{
    var lineExpandedX = "";
    for (var i = 0; i < line.Length; i++)
    {
        lineExpandedX += line[i];
        if (!columnsWithGalaxy.Contains(i))
        {
            lineExpandedX += '.';
        }
    }
    linesExpanded.Add(lineExpandedX);
}

var galaxies = new List<Tuple<int, int>>();
for (var y = 0; y < linesExpanded.Count; y++)
{
    for (var x = 0; x < linesExpanded[y].Length; x++)
    {
        if (linesExpanded[y][x] == '#')
        {
            galaxies.Add(new Tuple<int, int>(x, y));
        }
    }
}

var processedGalaxies = new List<Tuple<int, int>>();
var totalDistance = 0.0;
var pairs = 0;
foreach (var galaxySource in galaxies)
{
    foreach (var galaxyTarget in galaxies.Where(galaxyTarget => !Equals(galaxyTarget, galaxySource) && !processedGalaxies.Contains(galaxyTarget)))
    {
        int distanceX;
        if (galaxyTarget.Item1 > galaxySource.Item1)
        {
            distanceX = galaxyTarget.Item1 - galaxySource.Item1;
        }
        else
        {
            distanceX = galaxySource.Item1 - galaxyTarget.Item1;
        }
        int distanceY;
        if (galaxyTarget.Item2 > galaxySource.Item2)
        {
            distanceY = galaxyTarget.Item2 - galaxySource.Item2;
        }
        else
        {
            distanceY = galaxySource.Item2 - galaxyTarget.Item2;
        }        
        
        totalDistance += distanceX + distanceY;
        pairs++;
    }

    processedGalaxies.Add(galaxySource);
}
Console.WriteLine($"Pairs: {pairs}");
Console.WriteLine($"Answer: {totalDistance}");

const string output = @"C:\repos\AOE23\ConsoleApp11A\output.txt";

using var writer = new StreamWriter(output);
foreach (var line in linesExpanded)
{
    writer.WriteLine(line);
}