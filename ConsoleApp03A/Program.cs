/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-10-03
 * Last Modified: 2023-12-10
 * Description: https://adventofcode.com/2023/day/3 - Part One
 * Keywords: N/A
 */

const string filePath = "input.txt";
var lines = File.ReadAllLines(filePath);

var firstPos = -1;
var lastPos = -1;
var sum = 0;

for (var i = 0; i < lines.Length; i++)
{
    var previousLine = i > 0 ? lines[i - 1] : "";
    var currentLine = lines[i];
    var nextLine = i < lines.Length - 1 ? lines[i + 1] : "";
    
    for (var p = 0; p <= currentLine.Length - 1; p++)
    {
        if (i == 23 && p == 136)
        {
            Console.WriteLine("ERROR");
        }
        
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
                for (var pl = firstPos == 0 ? firstPos : firstPos - 1; pl <= (lastPos == currentLine.Length - 1 ? lastPos : lastPos + 1); pl++)
                {
                    if (previousLine[pl] != '.')
                    {
                        found = true;
                    }
                }
            }

            // Check character before
            if (firstPos > 0)
            {
                var characterBefore = currentLine[firstPos - 1];
                if (characterBefore != '.')
                {
                    found = true;
                }
            }

            // Check character after
            if (lastPos < currentLine.Length - 1)
            {
                var characterAfter = currentLine[lastPos + 1];
                if (characterAfter != '.')
                {
                    found = true;
                }
            }

            // Check next line
            if (nextLine != "")
            {
                for (var np = firstPos == 0 ? firstPos : firstPos - 1; np <= (lastPos == currentLine.Length - 1 ? lastPos : lastPos + 1); np++)
                {
                    if (nextLine[np] != '.')
                    {
                        found = true;
                    }
                }
            }

            if (found)
            {
                sum += int.Parse(number);
                Console.WriteLine($"Number {number} is a machine part");
            }
            else
            {
                Console.WriteLine($"Number {number} is not a machine part");
            }

            firstPos = -1;
            lastPos = -1;
        }
    }
}
Console.WriteLine($"Answer: {sum}");