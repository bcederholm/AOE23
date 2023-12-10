/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-10-06
 * Last Modified: 2023-12-10
 * Description: https://adventofcode.com/2023/day/6 - Part Two
 * Keywords: N/A
 */

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

var timeDistances = new List<TimeDistance>
{
    new() { Time = 40829166, Distance = 277133813491063 }
};

foreach (var timeDistance in timeDistances)
{
    var winningWays = 0;
    for (var i = 0; i <= timeDistance.Time; i++)
    {
        var distanceInMillimeters = GetDistanceInMillimeters(timeDistance.Time, i);
        if (distanceInMillimeters > timeDistance.Distance)
        {
            winningWays++;
        }
    }
    Console.WriteLine($"Answer: {winningWays}");
}

return;

long GetDistanceInMillimeters(long raceTimeInMilliseconds, long pushTimeInMilliseconds)
{
    var remainingTimeInMilliseconds = raceTimeInMilliseconds - pushTimeInMilliseconds;
    var distanceInMillimeters = pushTimeInMilliseconds * remainingTimeInMilliseconds;
    return distanceInMillimeters;
}

internal class TimeDistance
{
    public long Time { get; init; }
    public long Distance { get; init; }
}