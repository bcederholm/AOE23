// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

// List<TimeDistance> timeDistances = new List<TimeDistance>
// {
//     new TimeDistance { Time = 7, Distance = 9 },
//     new TimeDistance { Time = 15, Distance = 40 },
//     new TimeDistance { Time = 30, Distance = 200 }
// };

// List<TimeDistance> timeDistances = new List<TimeDistance>
// {
//     new TimeDistance { Time = 40, Distance = 277 },
//     new TimeDistance { Time = 82, Distance = 1338 },
//     new TimeDistance { Time = 91, Distance = 1349 },
//     new TimeDistance { Time = 66, Distance = 1063 },
// };

List<TimeDistance> timeDistances = new List<TimeDistance>
{
    new TimeDistance { Time = 40829166, Distance = 277133813491063 },
};

foreach (var timeDistance in timeDistances)
{
    var winningWays = 0;
    for (var i = 0; i <= timeDistance.Time; i++)
    {
        var distanceinMillimeters = GetDistanceInMillimeters(timeDistance.Time, i);
        if (distanceinMillimeters > timeDistance.Distance)
        {
            winningWays++;
        }
    }
    Console.WriteLine($"winningWays: {winningWays}");
}

long GetDistanceInMillimeters(long raceTimeInMilliseconds, long pushTimeInMilliseconds)
{
    var remainingTimeInMilliseconds = raceTimeInMilliseconds - pushTimeInMilliseconds;
    var distanceInMillimeters = pushTimeInMilliseconds * remainingTimeInMilliseconds;
    return distanceInMillimeters;
}


public class TimeDistance
{
    public long Time { get; set; }
    public long Distance { get; set; }
}