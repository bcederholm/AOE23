/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-10-11
 * Last Modified: 2023-12-11
 * Description: https://adventofcode.com/2023/day/11 - Part Two (1h 0m)
 * Keywords: N/A
 */

const string filePath1 = "input.txt";
var lines = File.ReadAllLines(filePath1);
var rowsWithoutGalaxy = new List<int>();
var columnsWithoutGalaxy = new List<int>();

var galaxies = new List<Tuple<int, int>>();
for (var y = 0; y < lines.Length; y++)
{
    for (var x = 0; x < lines[y].Length; x++)
    {
        if (lines[y][x] == '#')
        {
            galaxies.Add(new Tuple<int, int>(x, y));
        }
    }
}

for (var y = 0; y < lines.Length; y++)
{
    var result = galaxies.FirstOrDefault(g => g.Item2 == y);
    if (result == null)
    {
        rowsWithoutGalaxy.Add(y);
    }
}

for (var x = 0; x < lines[0].Length; x++)
{
    var result = galaxies.FirstOrDefault(g => g.Item1 == x);
    if (result == null)
    {
        columnsWithoutGalaxy.Add(x);
    }
}

var processedGalaxies = new List<Tuple<int, int>>();
var totalDistance = 0.0;
var pairs = 0;
const int multiplier = 1000000;
foreach (var galaxySource in galaxies)
{
    foreach (var galaxyTarget in galaxies.Where(galaxyTarget => !Equals(galaxyTarget, galaxySource) && !processedGalaxies.Contains(galaxyTarget)))
    {
        int distanceX;
        if (galaxyTarget.Item1 > galaxySource.Item1)
        {
            distanceX = galaxyTarget.Item1 - galaxySource.Item1;
            var emptyColumns = columnsWithoutGalaxy.Where(c => c > galaxySource.Item1 && c < galaxyTarget.Item1).ToList().Count;
            distanceX += emptyColumns * (multiplier - 1);
        }
        else
        {
            distanceX = galaxySource.Item1 - galaxyTarget.Item1;
            var emptyColumns = columnsWithoutGalaxy.Where(c => c > galaxyTarget.Item1 && c < galaxySource.Item1).ToList().Count;
            distanceX += emptyColumns * (multiplier - 1);
        }
        int distanceY;
        if (galaxyTarget.Item2 > galaxySource.Item2)
        {
            distanceY = galaxyTarget.Item2 - galaxySource.Item2;
            var emptyRows = rowsWithoutGalaxy.Where(r => r > galaxySource.Item2 && r < galaxyTarget.Item2).ToList().Count;
            distanceY += emptyRows * (multiplier - 1);
        }
        else
        {
            distanceY = galaxySource.Item2 - galaxyTarget.Item2;
            var emptyRows = rowsWithoutGalaxy.Where(r => r > galaxyTarget.Item2 && r < galaxySource.Item2).ToList().Count;
            distanceY += emptyRows * (multiplier - 1);
        }        
        
        totalDistance += distanceX + distanceY;
        pairs++;
    }

    processedGalaxies.Add(galaxySource);
}
Console.WriteLine($"Pairs: {pairs}");
Console.WriteLine($"Answer: {totalDistance}");