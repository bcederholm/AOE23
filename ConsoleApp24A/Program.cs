/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-12-24
 * Last Modified: 2023-12-24
 * Description: https://adventofcode.com/2023/day/24 - Part One
 * Keywords: N/A
 */


var fileLines = File.ReadAllLines("input.txt");

var rays = new List<Ray>();

foreach (var line in fileLines)
{
	var split = line.Split('@');
	var split0 = split[0].Split(',');
	var split1 = split[1].Split(',');
	rays.Add(new Ray(int.Parse(split0[0]), int.Parse(split0[1]), int.Parse(split1[0]), int.Parse(split1[1])));
}

foreach (var ray1 in rays)
{
	foreach (var ray2 in rays)
	{
		if (ray1 == ray2)
		{
			continue;
		}

		if (ray1.Intersects(ray2, out var intersectionX, out var intersectionY))
		{
			Console.WriteLine($"Intersection at ({intersectionX}, {intersectionY})");
		}
	}
}

internal class Ray
{
	public int CurrentX { get; }
	public int CurrentY { get; }
	public int DirectionX { get; }
	public int DirectionY { get; }

	public Ray(int currentX, int currentY, int directionX, int directionY)
	{
		CurrentX = currentX;
		CurrentY = currentY;
		DirectionX = directionX;
		DirectionY = directionY;
	}

	public bool Intersects(Ray otherRay, out decimal intersectionX, out decimal intersectionY)
	{
		// Cross-product to determine if the rays are parallel
		var crossProduct = DirectionX * otherRay.DirectionY - DirectionY * otherRay.DirectionX;

		if (crossProduct == 0)
		{
			// Rays are parallel or collinear, no intersection
			intersectionX = 0;
			intersectionY = 0;
			return false;
		}

		// Solve for t and u using vector equations
		var t = (otherRay.DirectionX * (CurrentY - otherRay.CurrentY) - otherRay.DirectionY * (CurrentX - otherRay.CurrentX)) / crossProduct;
		var u = (DirectionX * (CurrentY - otherRay.CurrentY) - DirectionY * (CurrentX - otherRay.CurrentX)) / crossProduct;

		if (t >= 0 && u >= 0)
		{
			// Calculate intersection point
			intersectionX = CurrentX + t * DirectionX;
			intersectionY = CurrentY + t * DirectionY;
			return true;
		}

		// Rays do not intersect
		intersectionX = 0;
		intersectionY = 0;
		return false;
	}
}
