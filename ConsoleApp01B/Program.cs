/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-10-01
 * Last Modified: 2023-12-10
 * Description: https://adventofcode.com/2023/day/1 - Part Two
 * Keywords: N/A
 */

var keyValuePairs = new Dictionary<string, int>
{
    { "one", 1 },
    { "two", 2 },
    { "three", 3 },
    { "four", 4 },
    { "five", 5 },
    { "six", 6 },
    { "seven", 7 },
    { "eight", 8 },
    { "nine", 9 },
    { "zero", 0 }
};

const string filePath = "input.txt";
var lines = File.ReadAllLines(filePath);

var total = 0;

foreach (var line in lines)
{
    Console.WriteLine(line);
    var firstNumber = FindFirstNumber(line);
    var firstDigit = FindFirstDigit(line);
    var lastNumber = FindLastNumber(line);
    var lastDigit = FindLastDigit(line);
    var first = firstNumber.Item1 < firstDigit.Item1 ? firstNumber.Item2 : firstDigit.Item2;
    var last = lastNumber.Item1 > lastDigit.Item1 ? lastNumber.Item2 : lastDigit.Item2;
    total += int.Parse(first + last.ToString());
}

Console.Write($"Answer: {total}");
return;

(int, int) FindLastNumber(string input)
{
    var mostRightPosition = int.MinValue;
    var mostRightNumber = -1;

    foreach (var numberPair in keyValuePairs)
    {
        var foundPosition = input.LastIndexOf(numberPair.Key, StringComparison.Ordinal);
        if (foundPosition <= -1 || foundPosition <= mostRightPosition) continue;
        mostRightPosition = foundPosition;
        mostRightNumber = numberPair.Value;
    }

    return (mostRightPosition, mostRightNumber);
}

(int, int) FindLastDigit(string input)
{
    for (var i = input.Length - 1; i >= 0; i--)
    {
        if (char.IsDigit(input[i]))
        {
            return (i, int.Parse(input[i].ToString()));
        }
    }

    return (int.MinValue, int.MinValue);
}

(int, int) FindFirstDigit(string input)
{
    foreach (var c in input.Where(char.IsDigit))
    {
        return (input.IndexOf(c), int.Parse(c.ToString()));
    }

    return (int.MaxValue, int.MaxValue);
}

(int, int) FindFirstNumber(string input)
{
    var mostLeftPosition = int.MaxValue;
    var mostLeftNumber = -1;

    foreach (var numberPair in keyValuePairs)
    {
        var foundPosition = input.IndexOf(numberPair.Key, StringComparison.Ordinal);
        if (foundPosition <= -1 || foundPosition >= mostLeftPosition) continue;
        mostLeftPosition = foundPosition;
        mostLeftNumber = numberPair.Value;
    }

    return (mostLeftPosition, mostLeftNumber);
}