
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;


// Declare a Dictionary<string, int>
Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();

// Adding example pairs
keyValuePairs.Add("one", 1);
keyValuePairs.Add("two", 2);
keyValuePairs.Add("three", 3);
keyValuePairs.Add("four", 4);
keyValuePairs.Add("five", 5);
keyValuePairs.Add("six", 6);
keyValuePairs.Add("seven", 7);
keyValuePairs.Add("eight", 8);
keyValuePairs.Add("nine", 9);
keyValuePairs.Add("zero", 0);

string filePath = "C:\\repos\\offside\\ConsoleApp01\\ConsoleApp01\\NewFile1.txt"; // Replace with your file path

// Read all lines from the file
string[] lines = File.ReadAllLines(filePath);

var total = 0;
// Loop through each line and display it
foreach (string line in lines)
{
    Console.WriteLine(line);

    (int, int) firstNumber = FindFirstNumber(line);
    (int, int) firstDigit = FindFirstDigit(line);
    (int, int) lastNumber = FindLastNumber(line);
    (int, int) lastDigit = FindLastDigit(line);

    int first = firstNumber.Item1 < firstDigit.Item1 ? firstNumber.Item2 : firstDigit.Item2;
    int last = lastNumber.Item1 > lastDigit.Item1 ? lastNumber.Item2 : lastDigit.Item2;
    
    total += int.Parse(first.ToString() + last.ToString());

    // Perform operations on each line here
}

(int, int) FindFirstNumber(string input)
{
    int mostLeftPosition = 1000;
    int mostLeftNumber = -1;

    foreach (var numberPair in keyValuePairs)
    {
        var foundPosition = input.IndexOf(numberPair.Key);
        if (foundPosition > -1 && (mostLeftPosition == -1 || foundPosition < mostLeftPosition))
        {
            mostLeftPosition = foundPosition;
            mostLeftNumber = numberPair.Value;
        }
    }

    return (mostLeftPosition, mostLeftNumber);
}

(int, int) FindLastNumber(string input)
{
    int mostRightPosition = -1000;
    int mostRightNumber = -1;

    foreach (var numberPair in keyValuePairs)
    {
        var foundPosition = input.LastIndexOf(numberPair.Key);
        if (foundPosition > -1 && (mostRightPosition == -1 || foundPosition > mostRightPosition))
        {
            mostRightPosition = foundPosition;
            mostRightNumber = numberPair.Value;
        }
    }

    return (mostRightPosition, mostRightNumber);
}

(int, int) FindFirstDigit(string input)
{
    int position = 1000;
    foreach (char c in input)
    {
        if (Char.IsDigit(c))
        {
            return (input.IndexOf(c), int.Parse(c.ToString()));
        }
    }
    return (1000, 1000);
}

(int, int) FindLastDigit(string input)
{
    int position = -1000;
    for (int i = input.Length - 1; i >= 0; i--)
    {
        if (Char.IsDigit(input[i]))
        {
            return (i, int.Parse(input[i].ToString()));
        }
    }
    return (-1000, -1000);
}

Console.Write(total);

