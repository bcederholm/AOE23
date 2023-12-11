/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-12-09
 * Last Modified: 2023-12-11
 * Description: https://adventofcode.com/2023/day/9 - Part Two
 * Keywords: N/A
 */

const string filePath1 = "input.txt";
var lines = File.ReadAllLines(filePath1);

var grandTotalExtrapolateNumbers = 0;
foreach (var line in lines)
{
    var splitNumbers = line.Split(' ');
    var rowsOfNumbers = new List<List<int>>();
    var finished = false;
    var currentNumbers = splitNumbers.Select(int.Parse).ToList();
    rowsOfNumbers.Add(currentNumbers);
    
    while (!finished)
    {
        var numbers = new List<int>();
        for (var i = 0; i < currentNumbers.Count - 1; i++)
        {
            numbers.Add(currentNumbers[i + 1] - currentNumbers[i]);
        }
        currentNumbers = numbers;
        rowsOfNumbers.Add(currentNumbers);

        if (numbers.All(n => n == 0))
        {
            finished = true;
        }
    }

    var currentRowExtrapolateNumber = 0;
    for (var i = rowsOfNumbers.Count - 1; i > 0; i--)
    {
        var aboveRow = rowsOfNumbers[i-1];
        
        var aboveRowFirstNumber = aboveRow.First();
        
        currentRowExtrapolateNumber = aboveRowFirstNumber - currentRowExtrapolateNumber;
    }
    grandTotalExtrapolateNumbers += currentRowExtrapolateNumber;
}

Console.WriteLine($"Answer: {grandTotalExtrapolateNumbers}");