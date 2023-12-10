string filePath1 = "C:\\repos\\AOE23\\ConsoleApp10A\\Input10.txt";
string[] lines = File.ReadAllLines(filePath1);

var sPosX = -1; 
var sPosY = -1; 

char[,] matrix = new char[lines[0].Length, lines.Length];

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
        var copy = lineSplitted[j];
        matrix[j, i] = copy;
    }
}

var currentPosX = sPosX;
var currentPosY = sPosY;
var finished = false;
var steps = 0;
var initialChar = '|';
var currentChar = initialChar;

var initialMovement = Movement.North;
var movement = initialMovement;

var pipe = new List<(int, int)>();
pipe.Add((sPosX, sPosY));

while (!finished)
{
    steps++;
    Console.Write(currentChar);
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
        pipe.Add((currentPosX, currentPosY));
        if (currentChar == 'S')
        {
            finished = true;
            if (movement == initialMovement)
            {
                Console.WriteLine($"Hit S at {steps} steps.");
            }
            else
            {
                Console.WriteLine("Invalid initial direction");
            }
        }
    }
}

// Replace all junk with .
for (int yClean = 0; yClean < matrix.GetLength(1); yClean++) {
    for (int xClean = 0; xClean < matrix.GetLength(0); xClean++)
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

for (int yDot = 0; yDot < matrix.GetLength(1); yDot++) {
    for (int xDot = 0; xDot < matrix.GetLength(0); xDot++)
    {
        if (matrix[xDot, yDot] == '.')
        {
            var even = true;

            var countUpDown = 0;
            var countUpRight = 0;
            var countUpLeft = 0;
            var countDownLeft = 0;
            var countDownRight = 0;

            for (int xSearch = 0; xSearch < xDot; xSearch++)
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
            countDownLeft -= countUpRight; // Same line
            countDownRight -= countUpLeft; // Same line
            
            var crossings = countUpDown + countUpRight + countDownLeft + countUpLeft + countDownRight; 
            if (crossings % 2 == 0) {
                even = true;
            } 
            else {
                even = false;
            }          
            
            if (!even)
            {
                Console.WriteLine($"Inside: xDot:{xDot} yDot:{yDot}");
                totalInside++;
            }
        }
    }
}

Console.WriteLine($"Total inside: {totalInside}");











string output = "C:\\repos\\AOE23\\ConsoleApp10B\\Output10B.txt";

using (StreamWriter writer = new StreamWriter(output)) {
    for (int i = 0; i < matrix.GetLength(1); i++) {
        for (int j = 0; j < matrix.GetLength(0); j++) {
            writer.Write(matrix[j, i]);
        }
        writer.WriteLine();
    }
}


Console.WriteLine($"Done");

enum Movement
{
    None = default,
    North,
    East,
    South,
    West
}