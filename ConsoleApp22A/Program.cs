/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-12-24
 * Last Modified: 2023-12-24
 * Description: https://adventofcode.com/2023/day/2A - Part One
 * Keywords: LINQ, 3D
 */

var fileLines = File.ReadAllLines("input.txt");
var bricks = new List<Brick>();

var characterCounter = 0;

foreach (var line in fileLines)
{
	var split = line.Split('~');
	var split0 = split[0].Split(',');
	var split1 = split[1].Split(',');

	var character = (char)('A' + characterCounter);
	characterCounter++;
	var brick = new Brick(int.Parse(split0[0]), int.Parse(split0[1]), int.Parse(split0[2]), int.Parse(split1[0]), int.Parse(split1[1]), int.Parse(split1[2]), character);
	bricks.Add(brick);
}

Console.WriteLine($"Amount of bricks: {bricks.Count}");

while (bricks.Any(b => !b.Settled))
{
	var settled = false;
	var brick = bricks.Where(b => !b.Settled).OrderBy(b => b.GetLowestZ()).First(); // Always work with the unsettled brick with the lowest Z

	foreach (var coordinateBelow in brick.GetCoordinates().Select(coordinate => new Coordinate(coordinate.X, coordinate.Y, coordinate.Z - 1)))
	{
		// Check if coordinate below is on the ground
		if (coordinateBelow.Z == 0)
		{
			settled = true;
			break;
		}

		// Check if coordinate below is occupied (Contain doesn't work because it is NOT same object)
		if (bricks.Where(b => b != brick).Any(b => b.GetCoordinates().Any(c => c.X == coordinateBelow.X && c.Y == coordinateBelow.Y && c.Z == coordinateBelow.Z)))
		{
			settled = true;
			break;
		}
	}

	if (settled)
	{
		// Console.WriteLine($"Brick {brick.Character} is settled");
		brick.Settled = true;
	}
	else
	{
		// Console.WriteLine($"Brick {brick.Character} is moved down");
		brick.Z1--;
		brick.Z2--;
	}
}

Console.WriteLine("All bricks are settled");

var bricksCanBeDisintegrated = 0;

foreach (var brick in bricks)
{
	var free = true;
	
	// Identify all bricks that are above current brick
	foreach (var coordinateAbove in brick.GetCoordinates().Select(coordinate => new Coordinate(coordinate.X, coordinate.Y, coordinate.Z + 1)))
	{
		// Check if coordinate below is occupied (Contain doesn't work because it is NOT same object)
		IEnumerable<Brick> bricksAbove = bricks.Where(b => b != brick && b.GetCoordinates().Any(c => c.X == coordinateAbove.X && c.Y == coordinateAbove.Y && c.Z == coordinateAbove.Z)).ToList();
		if (bricksAbove.Any())
		{
			var hasStillSupport = false;
			foreach (var brickAbove in bricksAbove)
			{
				hasStillSupport = brickAbove.GetCoordinates().Select(coordinate => new Coordinate(coordinate.X, coordinate.Y, coordinate.Z - 1)).Any(coordinateBelow => bricks.Where(b => b != brick && b != brickAbove).Any(b => b.GetCoordinates().Any(c => c.X == coordinateBelow.X && c.Y == coordinateBelow.Y && c.Z == coordinateBelow.Z)));
			}
			if (!hasStillSupport)
			{
				free = false;
				break;
			}
		}
	}
	
	if (free)
	{
		bricksCanBeDisintegrated++;
	}
}

Console.WriteLine($"Answer: {bricksCanBeDisintegrated}");

internal class Brick(int x1, int y1, int z1, int x2, int y2, int z2, char character)
{
	public char Character { get; } = character;
	private int X1 { get; } = x1;
	private int Y1 { get; } = y1;
	public int Z1 { get; set; } = z1;

	private int X2 { get; } = x2;
	private int Y2 { get; } = y2;
	public int Z2 { get; set; } = z2;

	public bool Settled { get; set; }

	public int GetLowestZ()
	{
		return Z1 <= Z2 ? Z1 : Z2;
	}

	public IEnumerable<Coordinate> GetCoordinates()
	{
		var coordinates = new List<Coordinate>();
		for (var x = X1; x <= X2; x++)
		{
			for (var y = Y1; y <= Y2; y++)
			{
				for (var z = Z1; z <= Z2; z++)
				{
					coordinates.Add(new Coordinate(x, y, z));
				}
			}
		}

		return coordinates;
	}
}

internal class Coordinate(int x, int y, int z)
{
	public int X { get; } = x;
	public int Y { get; } = y;
	public int Z { get; } = z;
}