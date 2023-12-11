/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-12-04
 * Last Modified: 2023-12-10
 * Description: https://adventofcode.com/2023/day/4 - Part One
 * Keywords: Linq
 */

const string filePath = "input.txt";
var lines = File.ReadAllLines(filePath);
var splitters = new[] { '|', ':' };
var totalSum = (
    from line in lines select line.Replace("  ", " ") 
    into line2 select line2.Replace("  ", " ")
    into line2 select line2.Replace("Card ", "")
    into line2 select line2.Split(splitters, StringSplitOptions.RemoveEmptyEntries)
    into sections let winningNumbers = sections[1].Trim().Split(' ')
    let myNumbers = sections[2].Trim().Split(' ')
    select myNumbers.Intersect(winningNumbers).ToArray()
    into matches where matches.Length > 0 select Math.Pow(2, matches.Length - 1)
    into points select (int)points).Sum();

Console.WriteLine($"Answer {totalSum}");