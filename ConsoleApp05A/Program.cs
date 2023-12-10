/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-10-05
 * Last Modified: 2023-12-10
 * Description: https://adventofcode.com/2023/day/5 - Part One
 * Keywords: N/A
 */

const string filePath1 = "input-seed.txt";
const string filePath2 = "input-map.txt";
var lines1 = File.ReadAllLines(filePath1);
var lines2 = File.ReadAllLines(filePath2);
List<Seed> seeds = (
    from line in lines1
    select line.Replace("seeds: ", "")
    into line2 from seedNumber in line2.Split(" ", StringSplitOptions.RemoveEmptyEntries)
    select new Seed { SeedNumber = long.Parse(seedNumber) }).ToList();

var mapType = "";
var maps = new List<Maps>();
var fromTo = new List<FromTo>();

foreach (var line in lines2)
{
    if (line == "")
    {
        continue;
    }
    
    if (line.Contains(':'))
    {
        if (fromTo.Count > 0)
        {
            maps.Add(new Maps
            {
                MapType = mapType,
                FromToMappings = fromTo
            });
        }
        mapType = line.Replace(" map:", "");
        fromTo = new List<FromTo>();
        continue;
    }
    
    var lineSplit = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
    fromTo.Add(new FromTo
    {
        FromSourceNumber = long.Parse(lineSplit[1]),
        ToSourceNumber = long.Parse(lineSplit[1]) + long.Parse(lineSplit[2]),
        FromTargetNumber = long.Parse(lineSplit[0])
    });
}

//Last run
if (fromTo.Count > 0)
{
    maps.Add(new Maps
    {
        MapType = mapType,
        FromToMappings = fromTo
    });
}

foreach (var seed in seeds)
{
    var workNumber = LookupMapping("seed-to-soil", seed.SeedNumber);
    workNumber = LookupMapping("soil-to-fertilizer", workNumber);
    workNumber = LookupMapping("fertilizer-to-water", workNumber);
    workNumber = LookupMapping("water-to-light", workNumber);
    workNumber = LookupMapping("light-to-temperature", workNumber);
    workNumber = LookupMapping("temperature-to-humidity", workNumber);
    workNumber = LookupMapping("humidity-to-location", workNumber);
    seed.LocationNumber = workNumber;
}

var totalSum = seeds.Select(s => s.LocationNumber).Min();

Console.WriteLine($"Answer: {totalSum}");
return;

long LookupMapping(string mt, long sourceNumber)
{
    var ft = (maps.First(m => m.MapType == mt).FromToMappings ?? throw new InvalidOperationException()).FirstOrDefault(m => m.FromSourceNumber <= sourceNumber && m.ToSourceNumber >= sourceNumber);
    var targetNumber = ft == null ? sourceNumber : ft.FromTargetNumber + (sourceNumber - ft.FromSourceNumber);
    return targetNumber;
}

internal class FromTo
{
    public long FromSourceNumber { get; init; }
    public long ToSourceNumber { get; init; }
    public long FromTargetNumber { get; init; }
}

internal class Maps
{
    public string? MapType { get; init; }
    public List<FromTo>? FromToMappings { get; init; }
}

internal class Seed
{
    public long SeedNumber { get; init; }
    public long LocationNumber { get; set; }
}