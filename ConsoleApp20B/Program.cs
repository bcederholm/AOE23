﻿/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-12-22
 * Last Modified: 2023-12-22
 * Description: https://adventofcode.com/2023/day/20 - Part Two
 * Keywords: Sender modules iterations to get wanted value. LCM
 */

var fileLines = File.ReadAllLines("input.txt");

var fileMatrix = new List<(string type, string name, List<string> destinations)>();
for (var index = 0; index < fileLines.Length; index++)
{
	var line = fileLines[index];
	line = line.Replace(", ", ",");
	line = line.Replace(" -> ", " ");
	var split = line.Split(' ');
	var dest = split[1].Split(',').ToList();
	fileMatrix.Add(split[0].Contains("broadcaster")
		? new ValueTuple<string, string, List<string>>(split[0], split[0], dest.ToList())
		: new ValueTuple<string, string, List<string>>(split[0][0].ToString(), split[0].Substring(1, split[0].Length - 1), dest.ToList()));
}

var modules = new List<Module>();
foreach (var line in fileMatrix)
{
	var moduleType = line.type switch
	{
		"broadcaster" => ModuleType.Broadcaster,
		"%" => ModuleType.FlipFlop,
		"&" => ModuleType.Conjunction,
		_ => throw new Exception("Unknown module type")
	};

	var module = new Module(moduleType, line.name, line.destinations);
	modules.Add(module);
}

var xn = 0;
var xnFound = -1;
var qn = 0;
var qnFound = -1;
var xf = 0;
var xfFound = -1;
var zl = 0;
var zlFound = -1;


var lowPulseCounter = 0;
var highPulseCounter = 0;

var processQueue = new List<(int level, Module module, string source, PulseType incomingPulse)>();

var testingModule = new Module(ModuleType.Tester, "rx", new List<string>());
modules.Add(testingModule);

foreach (var module in modules)
{
	foreach (var destinationModule in module.MyDestinations.Select(destination => modules.First(m => m.MyName == destination)))
	{
		if (destinationModule.MyModuleType == ModuleType.Conjunction)
		{
			destinationModule.ConjunctionSources[module.MyName] = PulseType.Low;
		}
	}
}

var buttonModule = new Module(ModuleType.Button, "button", new List<string>{ "broadcaster" });
for (var i = 1; i <= 10000000; i++)
{
	xn++;
	qn++;
	xf++;
	zl++;
	ProcessModule(-2, buttonModule);
}

return;

void UpdateModule(Module module, string source, PulseType incomingPulse = PulseType.None)
{
	switch (incomingPulse)
	{
		case PulseType.Low:
			lowPulseCounter++;
			break;
		case PulseType.High:
			highPulseCounter++;
			break;
		case PulseType.None:
			break;
		default:
			throw new ArgumentOutOfRangeException(nameof(incomingPulse), incomingPulse, null);
	}

	switch (module.MyModuleType)
	{
		case ModuleType.Button:
		{
			break;
		}
		case ModuleType.Broadcaster:
		{
			break;
		}
		case ModuleType.FlipFlop:
		{
			if (incomingPulse == PulseType.Low)
			{
				module.FlipFlopOnOffState = module.FlipFlopOnOffState == FlipFlopOnOffState.On ? FlipFlopOnOffState.Off : FlipFlopOnOffState.On;
			}
			break;
		}
		case ModuleType.Conjunction:
		{
			if (module.MyName == "th" && incomingPulse == PulseType.High)
			{
				switch (source)
				{
					case "xn":
					{
						if (xnFound == -1)
						{
							Console.WriteLine($"xn iterations: {xn}");
							xnFound = xn;
						}
						break;
					}
					case "qn":
					{
						if (qnFound == -1)
						{	
							Console.WriteLine($"qn iterations: {qn}");
							qnFound = qn;
						}
						break;
					}
					case "xf":
					{
						if (xfFound == -1)
						{
							Console.WriteLine($"xf iterations: {xf}");
							xfFound = xf;
						}
						break;
					}
					case "zl":
					{
						if (zlFound == -1)
						{
							Console.WriteLine($"zl iterations: {zl}");
							zlFound = zl;
						}
						break;
						
					}
				}
				if (xnFound > -1 && qnFound > -1 && xfFound > -1 && zlFound > -1)
				{
					IEnumerable<long> numbers = new List<long> { xnFound, qnFound, xfFound, zlFound };
					Console.WriteLine($"Answer: {LcmAggregated(numbers)}");
					Environment.Exit(0);
				}
			}
			module.ConjunctionSources[source] = incomingPulse;
			break;
		}
		case ModuleType.Tester:
		{
			if (incomingPulse == PulseType.Low)
			{
				Console.WriteLine($"Answer: {lowPulseCounter * highPulseCounter}");
				Environment.Exit(0);
			}
			break;
		}
		default:
			throw new ArgumentOutOfRangeException(module.MyModuleType.ToString());
	}
}


// Copied from 08B

static long LcmAggregated(IEnumerable<long> numbers)
{
	return numbers.Aggregate(Lcm);
}
static long Lcm(long a, long b)
{
	return Math.Abs(a * b) / Gcd(a, b);
}

static long Gcd(long a, long b)
{
	while (true)
	{
		if (b == 0) return a;
		var a1 = a;
		a = b;
		b = a1 % b;
	}
}


void AddToQueue(int level, Module module, string source, PulseType incomingPulse = PulseType.None)
{
	processQueue.Add((level, module, source, incomingPulse));
}

void ProcessQueue()
{
	while (processQueue.Count > 0)
	{
		var (level, moduleDequeue, sourceDequeue, incomingPulseDequeue) = processQueue.MinBy(q => q.level);
		processQueue.Remove((level, moduleDequeue, sourceDequeue, incomingPulseDequeue));
		ProcessModule(level, moduleDequeue, incomingPulseDequeue);
	}
}

void ProcessModule(int level, Module module, PulseType incomingPulse = PulseType.None)
{
	level++;
	switch (module.MyModuleType)
	{
		case ModuleType.Button:
			foreach (var destinationModule in module.MyDestinations.Select(destination => modules.First(m => m.MyName == destination)))
			{
				UpdateModule(destinationModule, module.MyName, PulseType.Low);
			}
			foreach (var destinationModule in module.MyDestinations.Select(destination => modules.First(m => m.MyName == destination)))
			{
				AddToQueue(level, destinationModule, module.MyName, PulseType.Low);
			}
			break;
		case ModuleType.Broadcaster:
		{
			foreach (var destinationModule in module.MyDestinations.Select(destination => modules.First(m => m.MyName == destination)))
			{
				UpdateModule(destinationModule, module.MyName, incomingPulse);
			}
			foreach (var destinationModule in module.MyDestinations.Select(destination => modules.First(m => m.MyName == destination)))
			{
				AddToQueue(level, destinationModule, module.MyName, PulseType.Low);
			}
			break;
		}
		case ModuleType.FlipFlop:
		{
			if (incomingPulse == PulseType.Low)
			{
				var sendPulse = module.FlipFlopOnOffState == FlipFlopOnOffState.On ? PulseType.High : PulseType.Low;
				foreach (var destinationModule in module.MyDestinations.Select(destination => modules.First(m => m.MyName == destination)))
				{
					UpdateModule(destinationModule, module.MyName, sendPulse);
				}
				foreach (var destinationModule in module.MyDestinations.Select(destination => modules.First(m => m.MyName == destination)))
				{
					AddToQueue(level, destinationModule, module.MyName, sendPulse);
				}
			}
			break;
		}
		case ModuleType.Conjunction:
		{
			var sendPulse = module.ConjunctionSources.Any(s => s.Value == PulseType.Low) ? PulseType.High : PulseType.Low;
			foreach (var destinationModule in module.MyDestinations.Select(destination => modules.First(m => m.MyName == destination)))
			{
				UpdateModule(destinationModule, module.MyName, sendPulse);
			}
			foreach (var destinationModule in module.MyDestinations.Select(destination => modules.First(m => m.MyName == destination)))
			{
				AddToQueue(level, destinationModule, module.MyName, sendPulse);
			}
			break;
		}
		case ModuleType.Tester:
			break;
		default:
			throw new ArgumentOutOfRangeException(module.MyModuleType.ToString());
	}
	ProcessQueue();
}

internal enum ModuleType
{
	Button,
	Broadcaster,
	FlipFlop,
	Conjunction,
	Tester
}

internal enum PulseType
{	
	None,
	High,
	Low
}

internal enum FlipFlopOnOffState
{
	None,
	On,
	Off
}

internal class Module
{
	public ModuleType MyModuleType { get; }
	public string MyName { get; }
	public List<string> MyDestinations { get;  }
	
	public FlipFlopOnOffState FlipFlopOnOffState { get; set; }

	public Dictionary<string, PulseType> ConjunctionSources { get; }
	
	public Module(ModuleType moduleType, string name, List<string> destinations)
	{
		MyModuleType = moduleType;
		MyName = name;
		MyDestinations = destinations;
		FlipFlopOnOffState = moduleType == ModuleType.FlipFlop ? FlipFlopOnOffState.Off : FlipFlopOnOffState.None;
		ConjunctionSources = new Dictionary<string, PulseType>();
	}
}