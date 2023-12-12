/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-12-12
 * Last Modified: 2023-12-12
 * Description: https://adventofcode.com/2023/day/12 - Part One (4h m)
 * Keywords: Brute force, 2^, Regex, https://regex101.com/
 */

using System.Text.RegularExpressions;

const string filePath1 = "input.txt";
var lines = File.ReadAllLines(filePath1);

var totalMatches = 0;
for (var l = 0; l < lines.Length; l++)
{
    var splitLine = lines[l].Split(' ', StringSplitOptions.RemoveEmptyEntries);
    var springs = splitLine[0];
    springs = springs.Trim('.');
    while (springs.Contains(".."))
    {
        springs = springs.Replace("..", ".");
    }
    var groupSizes= splitLine[1].Split(',');
    var unknownPositions = new List<int>();
    for (var p = 0; p < springs.Length; p++)
    {
        if (springs[p] == '?')
        {
            unknownPositions.Add(p);
        }
    }

    var unknownPositionsCount = unknownPositions.Count;
    var numCombinations = 1 << unknownPositionsCount; // 2^numItems
    var matches = 0;
    
    // Credit: Rider AI Assistant
    var pattern = groupSizes.Select(int.Parse).Aggregate(@"^[\\.]*", (current, numOfHashes) => current + $@"(#){{{numOfHashes}}}[\.]+"); // start with a dot (optional)
    pattern = RemoveLastOccurrence(pattern, @"[\.]+") + @"[\\.]*$"; // end with a dot (optional)
    
    for (var combination = 0; combination < numCombinations; combination++)
    {
        var boolCombinations = new bool[unknownPositionsCount];
        for (var item = 0; item < unknownPositionsCount; item++)
        {
            boolCombinations[item] = (combination & (1 << item)) != 0;
        }

        var springsCopy = springs;
        for (var unknown = 0 ; unknown < unknownPositionsCount; unknown++)
        {
            springsCopy = OverwriteCharAt(springsCopy, unknownPositions[unknown], boolCombinations[unknown] ? '#' : '.');
        }
        
        var match = Regex.IsMatch(springsCopy, pattern);
        matches += match ? 1 : 0;
    }
    totalMatches += matches;
    Console.WriteLine($"Line {l} has { matches }.");
}

Console.WriteLine($"Answer { totalMatches }");
return;

string OverwriteCharAt(string str, int position, char newChar)
{
    if (position < 0 || position >= str.Length)
    {
        throw new ArgumentException("Position is outside the string bounds.");
    }
    return str.Remove(position, 1).Insert(position, newChar.ToString());
}

static string RemoveLastOccurrence(string s, string sequenceToRemove)
{
    var idx = s.LastIndexOf(sequenceToRemove, StringComparison.Ordinal);
    return idx != -1 ? s.Remove(idx, sequenceToRemove.Length) : s;
}