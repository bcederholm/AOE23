/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-10-02
 * Last Modified: 2023-12-10
 * Description: https://adventofcode.com/2023/day/2 - Part Two
 * Keywords: N/A
 */

const string filePath = "input.txt";
var lines = File.ReadAllLines(filePath);

var sum = 0;

foreach (var line in lines)
{
    Console.WriteLine(line);

    var semiColonSplit = line.Split(':');
    var gamesSplit = semiColonSplit[1].Split(';');

    var red = 0;
    var green = 0;
    var blue = 0;

    foreach (var game in gamesSplit)
    {
        var cubesSplit = game.Split(',');
        
        foreach (var cube in cubesSplit)
        {
            var cubeSplit = cube.Trim().Split(' ');
            
            switch (cubeSplit[1])
            {
                case "red":
                    if (int.Parse(cubeSplit[0]) > red)
                    {
                        red = int.Parse(cubeSplit[0]);
                    }
                    break;
                case "green":
                    if (int.Parse(cubeSplit[0]) > green)
                    {
                        green = int.Parse(cubeSplit[0]);
                    }
                    break;
                case "blue":
                    if (int.Parse(cubeSplit[0]) > blue)
                    {
                        blue = int.Parse(cubeSplit[0]);
                    }
                    break;
            }
        }
    }

    sum += red * green * blue;
}

Console.WriteLine($"Answer: {sum}");