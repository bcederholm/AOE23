/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-12-15
 * Last Modified: 2023-12-15
 * Description: https://adventofcode.com/2023/day/15 - Part Two
 * Keywords: N/A
 */

var fileLines = File.ReadAllLines("input.txt");

var segments = fileLines[0].Split(',');
var boxes = new List<Box>();

for (var i = 0; i < 256; i++)
{
    boxes.Add(new Box { BoxId = i });
}

char[] delimiters = { '=', '-' };
foreach (var segment in segments)
{
    var splitSegment = segment.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
    var boxId = 0;
    foreach (var chr in splitSegment[0].Select(currentChar => currentChar))
    {
        var asciiValue = (int)chr;
        boxId += asciiValue;
        boxId *= 17;
        boxId %= 256;
    }
    if (splitSegment.Length == 1) // Minus
    {
        boxes[boxId].Lenses?.RemoveAll(lens => lens.Label == splitSegment[0]);
    }
    else // Equals
    {
        if (boxes[boxId].Lenses!.FirstOrDefault(l => l.Label == splitSegment[0]) == null)
        {
            boxes[boxId].Lenses!.Add(new Lens(splitSegment[0], int.Parse(splitSegment[1])));
        }
        else
        {
            boxes[boxId].Lenses!.First(l => l.Label == splitSegment[0]).FocalLength = int.Parse(splitSegment[1]);
        }
    }
}

var totalValue = 0;
foreach (var b in boxes)
{
    for (var s = 0; s < b.Lenses?.Count; s++)
    {
        var lensValue = (b.BoxId + 1) * (s + 1) * b.Lenses![s].FocalLength;
        Console.WriteLine($"lensValue: {lensValue}");
        totalValue += lensValue;
    }
}

Console.WriteLine($"Answer: {totalValue}");

internal class Box
{
    public int BoxId { get; init; }
    public readonly List<Lens>? Lenses = new();
}

internal class Lens
{
    public Lens(string label, int focalLength)
    {
        Label = label;
        FocalLength = focalLength;
    }

    public string Label { get; }
    public int FocalLength { get; set; }
}