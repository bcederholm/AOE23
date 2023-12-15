/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-12-15
 * Last Modified: 2023-12-15
 * Description: https://adventofcode.com/2023/day/15 - Part One
 * Keywords: ASCII
 */

var fileLines = File.ReadAllLines("input.txt");

var segments = fileLines[0].Split(',');
var totalValue = 0;
foreach (var segment in segments)
{
    var currentValue = 0;
    foreach (var asciiValue in segment.Select(currentChar => (int)currentChar))
    {
        currentValue += asciiValue;
        currentValue *= 17;
        currentValue %= 256;
    }
    totalValue += currentValue;
}


Console.WriteLine("Answer: " + totalValue);