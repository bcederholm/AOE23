/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-12-31
 * Last Modified: 2023-12-31
 * Description: https://adventofcode.com/2023/day/19 - Part One
 * Keywords: Recursive
 */

var fileLines = File.ReadAllLines("input.txt");

var workflows = new List<Workflow>();
var parts = new List<Part>();
var workflowsImport = true;

foreach (var line in fileLines)
{
	if (line == "")
	{
		workflowsImport = false;
		continue;
	}

	if (workflowsImport)
	{
		var firstSplit = line.Split('{');
		var secondSplit = firstSplit[1].Split(',');

		var rules = new List<Rule>();

		foreach (var ruleString in secondSplit)
		{
			Rule rule;
			if (ruleString.Contains('<'))
			{
				var ruleStringParts = ruleString.Split('<', ':');
				rule = new Rule(ruleStringParts[0][0], '<', int.Parse(ruleStringParts[1]), ruleStringParts[2]);
				rules.Add(rule);
			}
			else if (ruleString.Contains('>'))
			{
				var ruleStringParts = ruleString.Split('>', ':');
				rule = new Rule(ruleStringParts[0][0], '>', int.Parse(ruleStringParts[1]), ruleStringParts[2]);
				rules.Add(rule);
			}
			else
			{
				workflows.Add(new Workflow(firstSplit[0], rules, ruleString.TrimEnd('}')));
			}
		}
	}
	else
	{
		var split = line.TrimStart('{').TrimEnd('}').Split(',');
		var part = new Part(int.Parse(split[0].Split('=')[1]), int.Parse(split[1].Split('=')[1]), int.Parse(split[2].Split('=')[1]),
			int.Parse(split[3].Split('=')[1]));
		parts.Add(part);
	}
}

var initialWorkflow = workflows.First(w => w.Name == "in");
var partValueSum = (from part in parts let result = ProcessWorkFlow(part, initialWorkflow) where result == 'A' select part.X + part.M + part.A + part.S).Sum();

Console.WriteLine($"Answer: {partValueSum}");
return;

char ProcessWorkFlow(Part part, Workflow workflow)
{
	foreach (var rule in workflow.Rules)
	{
		var value = rule.Category switch
		{
			'x' => part.X,
			'm' => part.M,
			'a' => part.A,
			's' => part.S,
			_ => 0
		};

		switch (rule.Comparison)
		{
			case '<':
				if (value < rule.Limit)
				{
					return rule.DestinationWorkFlow switch
					{
						"A" => 'A',
						"R" => 'R',
						_ => ProcessWorkFlow(part, workflows.First(w => w.Name == rule.DestinationWorkFlow))
					};
				}

				break;
			case '>':
				if (value > rule.Limit)
				{
					return rule.DestinationWorkFlow switch
					{
						"A" => 'A',
						"R" => 'R',
						_ => ProcessWorkFlow(part, workflows.First(w => w.Name == rule.DestinationWorkFlow))
					};
				}

				break;
		}
	}
	return workflow.NoRuleWorkFlow switch
	{
		"A" => 'A',
		"R" => 'R',
		_ => ProcessWorkFlow(part, workflows.First(w => w.Name == workflow.NoRuleWorkFlow))
	};
}

internal class Workflow(string name, List<Rule> rules, string noRuleWorkFlow)
{
	public string Name { get; } = name;
	public List<Rule> Rules { get; } = rules;
	public string NoRuleWorkFlow { get; } = noRuleWorkFlow;
}

internal class Rule(char category, char comparison, int limit, string destinationWorkFlow)
{
	public char Category { get; } = category;
	public char Comparison { get; } = comparison;
	public int Limit { get; } = limit;
	public string DestinationWorkFlow { get; } = destinationWorkFlow;
}

internal class Part(int x, int m, int a, int s)
{
	public int X { get; } = x;
	public int M { get; } = m;
	public int A { get; } = a;
	public int S { get; } = s;
}