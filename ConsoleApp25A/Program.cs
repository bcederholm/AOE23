/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-12-24
 * Last Modified: 2023-12-24
 * Description: https://adventofcode.com/2023/day/24 - Part One
 * Keywords: LINQ All
 */

var fileLines = File.ReadAllLines("input.txt");

// Create all components
var globalComponents = new List<Component>();
foreach (var line in fileLines)
{
	var split = line.Split(':');
	if (globalComponents.All(c => c.Name != split[0]))
	{
		globalComponents.Add(new Component(split[0]));
	}

	var split1 = split[1].Trim().Split(' ');
	foreach (var con in split1)
	{
		if (globalComponents.All(c => c.Name != con))
		{
			globalComponents.Add(new Component(con));
		}
	}
}

// Create all connections
var globalConnections = new List<Connection>();
foreach (var line in fileLines)
{
	var split = line.Split(':');
	var split1 = split[1].Trim().Split(' ');
	foreach (var con in split1)
	{
		var component1 = globalComponents.First(c => c.Name == split[0]);
		var component2 = globalComponents.First(c => c.Name == con);
		globalConnections.Add(new Connection(component1, component2));
	}
}

var componentsWithConnections = globalComponents.ToDictionary(parentComponent => parentComponent,
		childComponent => GetConnectionComponents(globalComponents.First(c => c.Name == childComponent.Name)))
	.OrderByDescending(c => c.Value.Count)
	.ThenBy(c => c.Key.Name).ToList();


Console.WriteLine($"Components: {globalComponents.Count}");
Console.WriteLine($"Connections: {globalConnections.Count}");

var groupA = new List<Component>();

var firstComponent = componentsWithConnections.First().Key;
firstComponent.Grouped = true;
groupA.Add(firstComponent);

var groupACount = 0;
while (groupA.Count > groupACount)
{
	groupACount = groupA.Count;
	foreach (var component in componentsWithConnections.Where(c => !c.Key.Grouped))
	{
		var include = true;

		// First order. All my connected components are already in the group
		var except = component.Value.Except(groupA);
		if (except.Any())
		{
			include = false;
		}

		if (include)
		{
			component.Key.Grouped = true;
			groupA.Add(component.Key);
			break;
		}

		include = true;

		// Second order. Have to compare my connections with all group components connections
		for (var i = 0; i < groupA.Count; i++)
		{
			var groupComponentConnections = GetConnectionComponents(groupA[i]);

			var intersectionsWithGroupAConnections = component.Value.Intersect(groupComponentConnections).ToList();
			if (intersectionsWithGroupAConnections.Count < 2)
			{
				include = false;
				break;
			}

			if (groupComponentConnections.Intersect(component.Value).Count() < 2)
			{
				include = false;
			}
		}

		if (include)
		{
			component.Key.Grouped = true;
			groupA.Add(component.Key);
		}
	}
	Console.WriteLine($"Count: {groupACount}");
}

// var matchesWithGroupA = groupA.Select(GetConnectionComponents).ToList();
// var matchesInGroupARange = matchesWithGroupA.SelectMany(c => c).ToList();
// var intersectionsWithGroupAConnections = GetConnectionComponents(component)..Intersect(matchesInGroupARange).ToList();
//
// var intersectionsWithGroupAComponents = intersectionsWithGroupAConnections.Intersect(groupA).ToList();

var total = groupA.Count * (globalComponents.Count - groupA.Count);

Console.WriteLine($"Answer: {total}");

return;


List<Component> GetConnectionComponents(Component component)
{
	return (globalConnections ?? throw new InvalidOperationException()).Where(c => c.Component1 == component || c.Component2 == component)
		.Select(c => c.Component1 == component ? c.Component2 : c.Component1).ToList();
}

internal class Component(string name)
{
	public string Name { get; } = name;
	public bool Grouped { get; set; }
}

internal class Connection(Component component1, Component component2)
{
	public Component Component1 { get; } = component1;
	public Component Component2 { get; } = component2;

	public Connection Clone()
	{
		return (Connection)MemberwiseClone();
	}
}