/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-12-10
 * Last Modified: 2023-12-11
 * Description: https://adventofcode.com/2023/day/10 - Part One
 * Keywords: N/A
 */

const string filePath1 = "input.txt";
var lines = File.ReadAllLines(filePath1);
var sPosX = -1; 
var sPosY = -1; 
var matrix = new char[lines[0].Length, lines.Length];
char[] directions = { '|', '-', 'L', 'J', '7', 'F' };

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
        matrix[j, i] = splitLines[j];
    }
}

var answer = 0;

foreach (var direction in directions)
{
    Console.WriteLine($"Direction: {direction}");
    var currentPosX = sPosX;
    var currentPosY = sPosY;
    var finished = false;
    var steps = 0;
    var currentChar = direction;

    var initalMovement = direction switch
    {
        '|' => Movement.South,
        '-' => Movement.East,
        'L' => Movement.South,
        'J' => Movement.South,
        '7' => Movement.East,
        'F' => Movement.West,
        _ => Movement.None
    };

    var movement = initalMovement;

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
                    case Movement.None:
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
                    case Movement.None:
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
                    case Movement.None:
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
                    case Movement.None:
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
                    case Movement.None:
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
                    case Movement.None:
                    case Movement.East:
                    case Movement.South:
                        Console.WriteLine("Unexpected movement");
                        finished = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(movement.ToString());
                }
                break;
            default:
                Console.WriteLine("Hitted a dot");
                finished = true;
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
            if (currentChar == 'S')
            {
                finished = true;
                if (movement == initalMovement)
                {
                    Console.WriteLine($"When initial sign is {direction} hit S at {steps} steps.");
                    answer = steps / 2;
                }
                else
                {
                    Console.WriteLine("Invalid initial direction");
                }
            }
        }
    }
    Console.WriteLine();
}

Console.WriteLine($"Answer: {answer}");

internal enum Movement
{
    None = default,
    North,
    East,
    South,
    West
}