/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-12-31
 * Last Modified: 2024-01-01
 * Description: https://adventofcode.com/2023/day/19 - Part Two
 * Keywords: Recursive
 */

var fileLines = File.ReadAllLines("input.txt");

var workflows = new List<Workflow>();
foreach (var line in fileLines)
{
	if (line == "")
	{
		break;
	}

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

var initialWorkflow = workflows.First(w => w.Name == "in");
var approvedRulesStacks = new List<List<Rule>>();
ProcessWorkFlow(initialWorkflow, []);

var totalCombinations = approvedRulesStacks.Select(UpdateLimits).Select(newCombination => newCombination.Value).Aggregate<Int128, Int128>(0, (current, newCombinationValue) => current + newCombinationValue);

Console.WriteLine($"Answer: {totalCombinations}");
return;

Combination UpdateLimits(List<Rule> rulesStack)
{
	var combination = new Combination();
	foreach (var rule in rulesStack)
	{
		switch (rule.Category)
		{
			case 'x':
				if (rule.Comparison == '>')
				{
					combination.MinX = Math.Max(combination.MinX, rule.Limit + 1);
				}
				else
				{
					combination.MaxX = Math.Min(combination.MaxX, rule.Limit - 1);
				}
				break;
			case 'm':
				if (rule.Comparison == '>')
				{
					combination.MinM = Math.Max(combination.MinM, rule.Limit + 1);
				}
				else
				{
					combination.MaxM = Math.Min(combination.MaxM, rule.Limit - 1);
				}
				break;
			case 'a':
				if (rule.Comparison == '>')
				{
					combination.MinA = Math.Max(combination.MinA, rule.Limit + 1);
				}
				else
				{
					combination.MaxA = Math.Min(combination.MaxA, rule.Limit - 1);
				}
				break;
			case 's':
				if (rule.Comparison == '>')
				{
					combination.MinS = Math.Max(combination.MinS, rule.Limit + 1);
				}
				else
				{
					combination.MaxS = Math.Min(combination.MaxS, rule.Limit - 1);
				}
				break;
		}
	}
	return combination;
}

Rule AddReverseRule(Rule addRule)
{
	addRule = addRule.Comparison switch
	{
		'<' => new Rule(addRule.Category, '>', addRule.Limit - 1, addRule.DestinationWorkFlow),
		'>' => new Rule(addRule.Category, '<', addRule.Limit + 1, addRule.DestinationWorkFlow),
		_ => addRule
	};
	
	return addRule;
}

void ProcessWorkFlow(Workflow workflow, List<Rule> rulesStack)
{
	foreach (var rule in workflow.Rules)
	{
		List<Rule>? rulesStackClone;
		switch (rule.DestinationWorkFlow)
		{
			case "A":
				rulesStackClone = new List<Rule>(rulesStack) { rule };
				approvedRulesStacks.Add(rulesStackClone);	
				break;
			case "R":
				break;
			default:
				rulesStackClone = new List<Rule>(rulesStack) { rule };
				ProcessWorkFlow(workflows.First(w => w.Name == rule.DestinationWorkFlow), rulesStackClone);
				break;
		}
		rulesStack.Add(AddReverseRule(rule));
	}
	switch (workflow.NoRuleWorkFlow)
	{
		case "A":
		{
			approvedRulesStacks.Add(rulesStack);
			return;
		}
		case "R":
			return;
		default:
			ProcessWorkFlow(workflows.First(w => w.Name == workflow.NoRuleWorkFlow), rulesStack);
			return;
	}
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

internal class Combination
{
	public long MaxX = 4000;
	public long MinX = 1;
	public long MaxM = 4000;
	public long MinM = 1;
	public long MaxA = 4000;
	public long MinA = 1;
	public long MaxS = 4000;
	public long MinS = 1;
	public Int128 Value => (MaxX - MinX + 1) * (MaxM - MinM + 1) * (MaxA - MinA + 1) * (MaxS - MinS + 1);
}