// See https://aka.ms/new-console-template for more information

using System.Reflection.Emit;

string filePath = "C:\\repos\\offside\\ConsoleApp1\\ConsoleApp2\\Games.txt";
string[] lines = File.ReadAllLines(filePath);

var total = 0;



var sumId = 0;
// Loop through each line and display it


foreach (string line in lines)
{
    Console.WriteLine(line);

    var semiColonSplit = line.Split(':');
    var prefixSplit = semiColonSplit[0].Split(' ');
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

    sumId += red * green * blue;
    
}

Console.WriteLine(sumId);