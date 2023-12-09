// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");
List<Connection> connections = new List<Connection>();

string filePath = "C:\\repos\\offside\\ConsoleApp1\\ConsoleApp3B\\Input3.txt";
string[] lines = File.ReadAllLines(filePath);

string previousLine;
string currentLine;
string nextLine;

var firstPos = -1;
var lastPos = -1;

int linenumber = 1;

for (int i = 0; i < lines.Length; i++)
{
    previousLine = i > 0 ? lines[i - 1] : "";
    currentLine = lines[i];
    nextLine = i < lines.Length - 1 ? lines[i + 1] : "";
    
    for (int p = 0; p <= currentLine.Length - 1; p++)
    {
        
        if (firstPos == -1 && char.IsNumber(currentLine[p]))
        {
            firstPos = p;
        }
        
        if (firstPos != -1)
        {
            if (!char.IsNumber(currentLine[p]))
            {
                lastPos = p - 1;
            }
            
            if (lastPos == -1 && p == currentLine.Length - 1)
            {
                lastPos = p;
            }
        }
        
        if (firstPos != -1 && lastPos != -1)
        {
            
            var number = currentLine.Substring(firstPos, lastPos - firstPos + 1);
            var found = false;

            // Check previous line
            if (previousLine != "")
            {
                for (int pl = (firstPos == 0 ? firstPos : firstPos - 1); pl <= (lastPos == currentLine.Length - 1 ? lastPos : lastPos + 1); pl++)
                {
                    if (previousLine[pl] == '*')
                    {
                        connections.Add(new Connection() { Line = i - 1, Position = pl, Number = int.Parse(number) });
                    }
                }
            }

            // Check character before
            if (firstPos > 0)
            {
                var characterBefore = currentLine[firstPos - 1];
                if (characterBefore == '*')
                {
                    connections.Add(new Connection() { Line = i, Position = firstPos - 1, Number = int.Parse(number) });
                }
            }

            // Check character after
            if (lastPos < currentLine.Length - 1)
            {
                var characterAfter = currentLine[lastPos + 1];
                if (characterAfter == '*')
                {
                    connections.Add(new Connection() { Line = i, Position = lastPos + 1, Number = int.Parse(number) });
                }
            }

            // Check next line
            if (nextLine != "")
            {
                for (int np = (firstPos == 0 ? firstPos : firstPos - 1); np <= (lastPos == currentLine.Length - 1 ? lastPos : lastPos + 1); np++)
                {
                    if (nextLine[np] == '*')
                    {
                        connections.Add(new Connection() { Line = i + 1, Position = np, Number = int.Parse(number) });
                    }
                }
            }

            firstPos = -1;
            lastPos = -1;
        }
    }
    
    linenumber++;
}

var duplicateConnections = connections
    .GroupBy(c => new { c.Line, c.Position })
    .Where(g => g.Count() == 2)
    .SelectMany(g => g);

var result = duplicateConnections
    .GroupBy(c => new { c.Line, c.Position })
    .Select(g => new
    {
        Line = g.Key.Line,
        Position = g.Key.Position,
        MultiplicationResult = g.Select(c => c.Number).Aggregate((a, b) => a * b)
    });

int totalSum = result.Sum(item => item.MultiplicationResult);

Console.WriteLine($"Total sum: {totalSum}");

class Connection
{ 
    public int Line;
    public int Position;
    public int Number;
}