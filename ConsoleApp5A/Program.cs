Console.WriteLine("Hello, World!");

string filePath1 = "C:\\repos\\offside\\ConsoleApp1\\ConsoleApp5A\\Input5A-seed.txt";
string filePath2 = "C:\\repos\\offside\\ConsoleApp1\\ConsoleApp5A\\Input5A-map.txt";
string[] lines1 = File.ReadAllLines(filePath1);
string[] lines2 = File.ReadAllLines(filePath2);

List<Seed> seeds = new List<Seed>();

foreach (var line in lines1)
{
    var line2 = line.Replace("seeds: ", "");
    var seedNumbers = line2.Split(" ", StringSplitOptions.RemoveEmptyEntries);
    foreach (var seedNumber in seedNumbers)
    {
        seeds.Add(new Seed()
        {
            SeedNumber = Int64.Parse(seedNumber)
        });
    }
}

var mapType = "";

var maps = new List<Maps>();

List<FromTo> fromTo = new List<FromTo>();

foreach (var line in lines2)
{
    if (line == "")
    {
        continue;
    }
    
    if (line.Contains(":"))
    {
        if (fromTo.Count > 0)
        {
            maps.Add(new Maps()
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
    fromTo.Add(new FromTo()
    {
        FromSourceNumber = Int64.Parse(lineSplit[1]),
        ToSourceNumber = Int64.Parse(lineSplit[1]) + Int64.Parse(lineSplit[2]),
        FromTargetNumber = Int64.Parse(lineSplit[0]),
        ToTargetumber = Int64.Parse(lineSplit[0]) + Int64.Parse(lineSplit[2]),
    });
}

//Last run
if (fromTo.Count > 0)
{
    maps.Add(new Maps()
    {
        MapType = mapType,
        FromToMappings = fromTo,
    });
}

long LookupMapping(string mapType, long sourceNumber)
{
    var fromTo = maps.First(m => m.MapType == mapType).FromToMappings.FirstOrDefault(m => m.FromSourceNumber <= sourceNumber && m.ToSourceNumber >= sourceNumber);
    var targetNumber = fromTo == null ? sourceNumber : fromTo.FromTargetNumber + (sourceNumber - fromTo.FromSourceNumber);
    return targetNumber;
}

foreach (var seed in seeds)
{
    var workNumber = seed.SeedNumber;
    workNumber = LookupMapping("seed-to-soil", seed.SeedNumber);
    workNumber = LookupMapping("soil-to-fertilizer", workNumber);
    workNumber = LookupMapping("fertilizer-to-water", workNumber);
    workNumber = LookupMapping("water-to-light", workNumber);
    workNumber = LookupMapping("light-to-temperature", workNumber);
    workNumber = LookupMapping("temperature-to-humidity", workNumber);
    workNumber = LookupMapping("humidity-to-location", workNumber);
    seed.LocationNumber = workNumber;
}

var totalSum = seeds.Select(s => s.LocationNumber).Min();

Console.WriteLine($"Finished: {totalSum}");

public class FromTo
{
    public Int64 FromSourceNumber { get; set; }
    public Int64 ToSourceNumber { get; set; }
    public Int64 FromTargetNumber { get; set; }
    public Int64 ToTargetumber { get; set; }
}

public class Maps
{
    public string MapType { get; set; }
    public List<FromTo> FromToMappings { get; set; }
}

public class Seed
{
    public Int64 SeedNumber { get; set; }
    public Int64 LocationNumber { get; set; }
}