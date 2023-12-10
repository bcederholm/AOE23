string filePath1 = "C:\\repos\\AOE23\\ConsoleApp10A\\Input10.txt";
string[] lines = File.ReadAllLines(filePath1);

var sPosX = -1; 
var sPosY = -1; 

char[,] matrix = new char[lines[0].Length, lines.Length];

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
    var lineSplitted = lines[i].ToCharArray();
    
    for (var j = 0; j < lineSplitted.Length; j++)
    {
        matrix[j, i] = lineSplitted[j];
    }
}


foreach (var direction in directions)
{
    Console.WriteLine();
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
                    default:
                        Console.WriteLine("Unexpected movement");
                        finished = true;
                        break;
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
                    default:
                        Console.WriteLine("Unexpected movement");
                        finished = true;
                        break;
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
                    default:
                        Console.WriteLine("Unexpected movement");
                        finished = true;
                        break;
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
                    default:
                        Console.WriteLine("Unexpected movement");
                        finished = true;
                        break;
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
                    default:
                        Console.WriteLine("Unexpected movement");
                        finished = true;
                        break;
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
                    default:
                        Console.WriteLine("Unexpected movement");
                        finished = true;
                        break;
                }
                break;
        }
        if (currentPosX < 0 || currentPosX >= matrix.GetLength(0) || currentPosY < 0 || currentPosY >= matrix.GetLength(1))
        {
            finished = true;
            Console.WriteLine($"Out of bounds");
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
                }
                else
                {
                    Console.WriteLine("Invalid initial direction");
                }
            }
        }
    }
}

Console.WriteLine($"Prepare finished");

enum Movement
{
    None = default,
    North,
    East,
    South,
    West
}