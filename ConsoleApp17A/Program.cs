/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-12-17
 * Last Modified: 2023-12-17
 * Description: https://adventofcode.com/2023/day/17 - Part One
 * Keywords: N/A
 * WIP */

var fileLines = File.ReadAllLines("input.txt");

var matrix = fileLines.Select(line => line.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray()).ToArray();