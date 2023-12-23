/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-12-23
 * Last Modified: 2023-12-23
 * Description: https://adventofcode.com/2023/day/23 - Part One
 * Keywords: is pattern, recursive, MemberwiseClone
 */

var fileLines = File.ReadAllLines("input.txt");

var matrixBase = new List<Coordinate>();
for (var y = 0; y < fileLines.Length; y++)
{
	for (var x = 0; x < fileLines[y].Length; x++)
	{
		switch (fileLines[y][x])
		{
			case '.':
				matrixBase.Add( new Coordinate(x, y, Sign.Empty));
				break;
			case '#':
				matrixBase.Add(new Coordinate(x, y, Sign.Wall));
				break;
			case '^':
				matrixBase.Add(new Coordinate(x, y, Sign.NorthSlope));
				break;
			case '>':
				matrixBase.Add(new Coordinate(x, y, Sign.EastSlope));
				break;
			case 'v':
				matrixBase.Add(new Coordinate(x, y, Sign.SouthSlope));
				break;
			case '<':
				matrixBase.Add(new Coordinate(x, y, Sign.WestSlope));
				break;
		}
	}
}
matrixBase.First(c => c.Y == 0 & c.Sign == Sign.Empty).Sign = Sign.Start;
matrixBase.First(c => c.Y == fileLines.Length - 1 & c.Sign == Sign.Empty).Sign = Sign.End;

var startPosition = matrixBase.First(c => c.Sign == Sign.Start);
startPosition.Visited = true;

var longestRoute = Walk(startPosition.X, startPosition.Y, Directions.South, matrixBase);
Console.WriteLine($"Answer: {longestRoute.Item1}");
return;

(int, List<Coordinate>) Walk(int x, int y, Directions direction, List<Coordinate> matrix)
{
	var currentPosition = matrix.First(c => c.X == x && c.Y == y);
	var steps = 0;
	while (true)
	{
		if (currentPosition.Sign == Sign.End)
		{
			Console.WriteLine($"Reached the end in {steps} steps!");
			return (steps, matrix);
		}
		steps ++;
		currentPosition.Visited = true;
		
		Coordinate evaluatedPosition;
		
		var alternatives = new List<(Coordinate coordinate, Directions direction)>();

		switch (direction)
		{
			case Directions.North:
			{
				// Is it possible to turn left? (West)
				var pos1 = currentPosition;
				evaluatedPosition = matrix.First(c => c.X == pos1.X - 1 && c.Y == pos1.Y);
				if (evaluatedPosition is { Visited: false, Sign: Sign.Empty or Sign.WestSlope or Sign.End })
				{
					alternatives.Add((evaluatedPosition, Directions.West));
				}

				// Is it possible to continue forward? (North)
				var pos2 = currentPosition;
				evaluatedPosition = matrix.First(c => c.X == pos2.X && c.Y == pos2.Y - 1);
				if (evaluatedPosition is { Visited: false, Sign: Sign.Empty or Sign.NorthSlope or Sign.End })
				{
					alternatives.Add((evaluatedPosition, Directions.North));
				}

				// Is it possible to turn right? (East)
				var pos3 = currentPosition;
				evaluatedPosition = matrix.First(c => c.X == pos3.X + 1 && c.Y == pos3.Y);
				if (evaluatedPosition is { Visited: false, Sign: Sign.Empty or Sign.EastSlope or Sign.End })
				{
					alternatives.Add((evaluatedPosition, Directions.East));
				}

				break;
			}
			case Directions.East:
			{
				if (currentPosition is { X: 20, Y: 11 })
				{
					Console.WriteLine("DEBUG");
				}
				// Is it possible to turn left? (North)
				var pos1 = currentPosition;
				evaluatedPosition = matrix.First(c => c.X == pos1.X && c.Y == pos1.Y - 1);
				if (evaluatedPosition is { Visited: false, Sign: Sign.Empty or Sign.NorthSlope or Sign.End })
				{
					alternatives.Add((evaluatedPosition, Directions.North));
				}

				// Is it possible to continue forward? (East)
				var pos2 = currentPosition;
				evaluatedPosition = matrix.First(c => c.X == pos2.X + 1 && c.Y == pos2.Y);
				if (evaluatedPosition is { Visited: false, Sign: Sign.Empty or Sign.EastSlope or Sign.End })
				{
					alternatives.Add((evaluatedPosition, Directions.East));
				}

				// Is it possible to turn right? (South)
				var pos3 = currentPosition;
				evaluatedPosition = matrix.First(c => c.X == pos3.X && c.Y == pos3.Y + 1);
				if (evaluatedPosition is { Visited: false, Sign: Sign.Empty or Sign.SouthSlope or Sign.End})
				{
					alternatives.Add((evaluatedPosition, Directions.South));
				}
				break;
			}
			case Directions.South:
			{
				// Is it possible to turn left? (East)
				var pos1 = currentPosition;
				evaluatedPosition = matrix.First(c => c.X == pos1.X + 1 && c.Y == pos1.Y);
				if (evaluatedPosition is { Visited: false, Sign: Sign.Empty or Sign.EastSlope or Sign.End })
				{
					alternatives.Add((evaluatedPosition, Directions.East));
				}
				
				// Is it possible to continue forward? (South)
				var pos2 = currentPosition;
				evaluatedPosition = matrix.First(c => c.X == pos2.X && c.Y == pos2.Y + 1);
				if (evaluatedPosition is { Visited: false, Sign: Sign.Empty or Sign.SouthSlope or Sign.End })
				{
					alternatives.Add((evaluatedPosition, Directions.South));
				}
				// Is it possible to turn right? (West)
				var pos3 = currentPosition;
				evaluatedPosition = matrix.First(c => c.X == pos3.X - 1 && c.Y == pos3.Y);
				if (evaluatedPosition is { Visited: false, Sign: Sign.Empty or Sign.WestSlope or Sign.End })
				{
					alternatives.Add((evaluatedPosition, Directions.West));
				}
				break;
			}
			case Directions.West:
			{
				// Is it possible to turn left? (South)
				var pos1 = currentPosition;
				evaluatedPosition = matrix.First(c => c.X == pos1.X && c.Y == pos1.Y + 1);
				if (evaluatedPosition is { Visited: false, Sign: Sign.Empty or Sign.SouthSlope or Sign.End })
				{
					alternatives.Add((evaluatedPosition, Directions.South));
				}

				// Is it possible to continue forward? (West)
				var pos2 = currentPosition;
				evaluatedPosition = matrix.First(c => c.X == pos2.X - 1 && c.Y == pos2.Y);
				if (evaluatedPosition is { Visited: false, Sign: Sign.Empty or Sign.WestSlope or Sign.End })
				{
					alternatives.Add((evaluatedPosition, Directions.West));
				}

				// Is it possible to turn right? (North)
				var pos3 = currentPosition;
				evaluatedPosition = matrix.First(c => c.X == pos3.X && c.Y == pos3.Y - 1);
				if (evaluatedPosition is { Visited: false, Sign: Sign.Empty or Sign.NorthSlope or Sign.End })
				{
					alternatives.Add((evaluatedPosition, Directions.North));
				}
				break;
			}
			case Directions.None:
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
		}
		
		switch (alternatives.Count)
		{
			case 0:
				Console.WriteLine("No alternatives found, going back");
				return (-1, new List<Coordinate>());
			case 1:
				currentPosition = alternatives.First().coordinate;
				direction = alternatives.First().direction;
				break;
			case > 1:
			{
				var maxSteps = 0;
				var maxMatrix = new List<Coordinate>();
				foreach (var alternative in alternatives)
				{
					Console.WriteLine($"Alternative: {alternative.coordinate.X}, {alternative.coordinate.Y}, {alternative.direction}");
					
					var matrixClone = matrix.Select(c => c.Clone()).ToList();
					
					var (tempSteps, tempMatrix) = Walk(alternative.coordinate.X, alternative.coordinate.Y, alternative.direction, matrixClone);
					if (tempSteps <= maxSteps) continue;
					maxSteps = tempSteps;
					maxMatrix = tempMatrix;
				}
				if (maxSteps <= 0) return (steps, matrix);
				matrix = maxMatrix;
				steps += maxSteps;
				return (steps, matrix);
			}
		}
	}
}

internal enum Sign
{
	Start,
	End,
	Empty,
	Wall,
	NorthSlope,
	EastSlope,
	SouthSlope,
	WestSlope
}

internal enum Directions
{
	None = 0,
	North = 1,
	East = 2,
	South = 4,
	West = 8
}

internal class Coordinate
{
	public Coordinate(int x, int y, Sign sign)
	{
		X = x;
		Y = y;
		Sign = sign;
	}
	
	public Coordinate Clone()
	{
		return (Coordinate)MemberwiseClone();
	}

	public int X { get; }
	public int Y { get; }
	public Sign Sign { get; set; }
	public bool Visited { get; set; }
}
