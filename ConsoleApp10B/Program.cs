/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-12-10
 * Last Modified: 2023-12-11
 * Description: https://adventofcode.com/2023/day/10 - Part Two
 * Keywords: Extended line crossing pattern
 */

const string filePath1 = "input.txt";
var lines = File.ReadAllLines(filePath1);
var sPosX = -1; 
var sPosY = -1; 
var matrix = new char[lines[0].Length, lines.Length];

for (var i = 0; i < lines.Length; i++)
{
    var x = lines[i].IndexOf('S');
    if (x > -1)
    {
        sPosX = x;
        sPosY = i;
        Console.WriteLine($"S is at {sPosX}, {sPosY}");
    }
    var splitLines = lines[i].ToCharArray();
    
    for (var j = 0; j < splitLines.Length; j++)
    {
        var copy = splitLines[j];
        matrix[j, i] = copy;
    }
}

var currentPosX = sPosX;
var currentPosY = sPosY;
var finished = false;
var steps = 0;
const char initialChar = '|';
var currentChar = initialChar;
const Movement initialMovement = Movement.North;
var movement = initialMovement;
var pipe = new List<(int, int)> { (sPosX, sPosY) };

while (!finished)
{
    steps++;
    switch (currentChar)
    {
        case '|':
            switch (movement)
            {
                case Movement.South:
                    currentPosY++;
                    break;
                case Movement.North:
                    currentPosY--;
                    break;
                case Movement.East:
                case Movement.West:
                    Console.WriteLine("Unexpected movement");
                    finished = true;
                    break; 
                default:
                    throw new ArgumentOutOfRangeException(movement.ToString());
            }
            break;
        case '-':
            switch (movement)
            {
                case Movement.East:
                    currentPosX++;
                    break;
                case Movement.West:
                    currentPosX--;
                    break;
                case Movement.North:
                case Movement.South:
                    Console.WriteLine("Unexpected movement");
                    finished = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(movement.ToString());
            }
            break;
        case 'L':
            switch (movement)
            {
                case Movement.South:
                    currentPosX++;
                    movement = Movement.East;
                    break;
                case Movement.West:
                    currentPosY--;
                    movement = Movement.North;
                    break;
                case Movement.North:
                case Movement.East:
                    Console.WriteLine("Unexpected movement");
                    finished = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(movement.ToString());
            }
            break;
        case 'J':
            switch (movement)
            {
                case Movement.South:
                    currentPosX--;
                    movement = Movement.West;
                    break;
                case Movement.East:
                    currentPosY--;
                    movement = Movement.North;
                    break;
                case Movement.North:
                case Movement.West:
                    Console.WriteLine("Unexpected movement");
                    finished = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(movement.ToString());
            }
            break;
        case '7':
            switch (movement)
            {
                case Movement.East:
                    currentPosY++;
                    movement = Movement.South;
                    break;
                case Movement.North:
                    currentPosX--;
                    movement = Movement.West;
                    break;
                case Movement.South:
                case Movement.West:
                    Console.WriteLine("Unexpected movement");
                    finished = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(movement.ToString());
            }
            break;
        case 'F':
            switch (movement)
            {
                case Movement.West:
                    currentPosY++;
                    movement = Movement.South;
                    break;
                case Movement.North:
                    currentPosX++;
                    movement = Movement.East;
                    break;
                case Movement.East:
                case Movement.South:
                    Console.WriteLine("Unexpected movement");
                    finished = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(movement.ToString());
            }
            break;
    }
    if (currentPosX < 0 || currentPosX >= matrix.GetLength(0) || currentPosY < 0 || currentPosY >= matrix.GetLength(1))
    {
        finished = true;
        Console.WriteLine("Out of bounds");
    }
    else
    {
        currentChar = matrix[currentPosX, currentPosY];
        pipe.Add((currentPosX, currentPosY));
        if (currentChar == 'S')
        {
            finished = true;
            Console.WriteLine(movement == initialMovement ? $"Hit S at {steps} steps." : "Invalid initial direction");
        }
    }
}

// Replace all junk with .
for (var yClean = 0; yClean < matrix.GetLength(1); yClean++) {
    for (var xClean = 0; xClean < matrix.GetLength(0); xClean++)
    {
        if (!pipe.Contains((xClean, yClean)))
        {
            matrix[xClean, yClean] = '.';
        }
        if (matrix[xClean, yClean] == 'S')
        {
            matrix[xClean, yClean] = initialChar;
        }
    }
}

var totalInside = 0;
for (var yDot = 0; yDot < matrix.GetLength(1); yDot++) {
    for (var xDot = 0; xDot < matrix.GetLength(0); xDot++)
    {
        if (matrix[xDot, yDot] == '.')
        {
            var countUpDown = 0;
            var countUpRight = 0;
            var countUpLeft = 0;
            var countDownLeft = 0;
            var countDownRight = 0;

            for (var xSearch = 0; xSearch < xDot; xSearch++)
            {
                switch (matrix[xSearch, yDot])
                {
                    case '|':
                        countUpDown++;
                        break;
                    case 'L':
                        countUpRight++;
                        break;
                    case 'J':
                        countUpLeft++;
                        break;
                    case '7':
                        countDownLeft++;
                        break;
                    case 'F':
                        countDownRight++;
                        break;
                }
            }
            
            // Credit: Crossing line even/odd pattern -> https://www.reddit.com/r/adventofcode/comments/18evyu9/2023_day_10_solutions/
            // L+7 vs F+J needs to be identified as same line
            
            countDownLeft -= countUpRight; // Same line
            countDownRight -= countUpLeft; // Same line
            var crossings = countUpDown + countUpRight + countDownLeft + countUpLeft + countDownRight; 
            var even = crossings % 2 == 0;          
            
            if (!even)
            {
                Console.WriteLine($"Inside: xDot:{xDot} yDot:{yDot}");
                totalInside++;
            }
        }
    }
}

Console.WriteLine($"Answer: {totalInside}");

const string output = "output.txt";
using var writer = new StreamWriter(output);
for (var i = 0; i < matrix.GetLength(1); i++) {
    for (var j = 0; j < matrix.GetLength(0); j++) {
        writer.Write(matrix[j, i]);
    }
    writer.WriteLine();
}

internal enum Movement
{
    North,
    East,
    South,
    West
}