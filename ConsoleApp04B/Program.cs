/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-12-04
 * Last Modified: 2023-12-10
 * Description: https://adventofcode.com/2023/day/4 - Part Two
 * Keywords: Linq
 */

const string filePath = "input.txt";
var lines = File.ReadAllLines(filePath);
var splitters = new[] { '|', ':' };
var linesExtended = (
    from line in lines select line.Replace("  ", " ")
    into line2 select line2.Replace("  ", " ")
    into line2 select line2.Replace("Card ", "")
    into line2 select line2.Split(splitters, StringSplitOptions.RemoveEmptyEntries)
    into sections let winningNumbers = sections[1].Trim().Split(' ')
    let myNumbers = sections[2].Trim().Split(' ')
    select new LineExtended { CardNumber = int.Parse(sections[0]), Matches = winningNumbers.Intersect(myNumbers).Select(int.Parse).ToArray().Length, Quantity = 1 }).ToList();

foreach (var line in linesExtended.Where(line => line.Matches > 0))
{
    for (var i = line.CardNumber + 1; i <= line.CardNumber + line.Matches; i++)
    {
        if (i <= linesExtended.Count)
        {
            linesExtended[i - 1].Quantity += line.Quantity;
        }
    }
}

Console.WriteLine($"Answer: {linesExtended.Select(l => l.Quantity).Sum()}");

internal class LineExtended
{
    public int CardNumber { get; init; }
    public int Matches { get; init; }
    public int Quantity { get; set; }
}