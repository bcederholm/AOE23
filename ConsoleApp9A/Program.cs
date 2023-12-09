string filePath1 = "C:\\repos\\offside\\ConsoleApp1\\ConsoleApp9A\\Input9.txt";
string[] lines = File.ReadAllLines(filePath1);


var grandTotalExtrapolateNumbers = 0;
foreach (var line in lines)
{
    var splittedNumbers = line.Split(' ');
    
    var rowsOfNumbers = new List<List<int>>();
    var numbers = new List<int>();
    var finished = false;

    var currentNumbers = splittedNumbers.Select(c => int.Parse(c)).ToList();;
    rowsOfNumbers.Add(currentNumbers);
    
    while (!finished)
    {
        numbers = new List<int>();
        for (var i = 0; i < currentNumbers.Count - 1; i++)
        {
            numbers.Add(currentNumbers[i + 1] - currentNumbers[i]);
        }
        currentNumbers = numbers;
        rowsOfNumbers.Add(currentNumbers);

        if (!numbers.Any(n => n != 0))
        {
            finished = true;
        }
    }

    var sum = 0;
    var currentRowExtrapolateNumber = 0;
    for (var i = rowsOfNumbers.Count - 1; i > 0; i--)
    {
        var currentRow = rowsOfNumbers[i];
        var aboveRow = rowsOfNumbers[i-1];
        
        var aboveRowLastNumber = aboveRow.Last();
        
        currentRowExtrapolateNumber = aboveRowLastNumber + currentRowExtrapolateNumber;
        
        Console.WriteLine($"difference: {currentRowExtrapolateNumber} ");
    }
    grandTotalExtrapolateNumbers += currentRowExtrapolateNumber;
}

Console.WriteLine($"Grand total: {grandTotalExtrapolateNumbers}");