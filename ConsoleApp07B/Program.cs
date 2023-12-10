/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-10-07
 * Last Modified: 2023-12-10
 * Description: https://adventofcode.com/2023/day/7 - Part Two
 * Keywords: Jokers, Recursive
 */

const string filePath1 = "input.txt";
var lines1 = File.ReadAllLines(filePath1);
var hands = new List<Hand>();

foreach (var line in lines1)
{
    var lineSegments = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
    
    lineSegments[0] = lineSegments[0].Replace('J', '0');
    lineSegments[0] = lineSegments[0].Replace('2', 'a');
    lineSegments[0] = lineSegments[0].Replace('3', 'b');
    lineSegments[0] = lineSegments[0].Replace('4', 'c');
    lineSegments[0] = lineSegments[0].Replace('5', 'd');
    lineSegments[0] = lineSegments[0].Replace('6', 'e');
    lineSegments[0] = lineSegments[0].Replace('7', 'f');
    lineSegments[0] = lineSegments[0].Replace('8', 'g');
    lineSegments[0] = lineSegments[0].Replace('9', 'h');
    lineSegments[0] = lineSegments[0].Replace('T', 'i');
    lineSegments[0] = lineSegments[0].Replace('Q', 'j');
    lineSegments[0] = lineSegments[0].Replace('K', 'k');
    lineSegments[0] = lineSegments[0].Replace('A', 'l');

    hands.Add(new Hand
    {
        ValueString = lineSegments[0],
        Bid = Convert.ToInt32(lineSegments[1]),
        Value = PokerHandEvaluator.EvaluateHandWrapper(lineSegments[0].ToCharArray())
    });
}

var sortedHands = hands.OrderBy(h => h.Value).ThenBy(h => h.ValueString);

var rankNumber = 0;
var totalWinnings = 0;
foreach (var hand in sortedHands)
{
    rankNumber++;
    hand.Winning = hand.Bid * rankNumber;
    totalWinnings += hand.Winning;
}

Console.WriteLine($"Answer: {totalWinnings}");

internal class Hand
{
    public string? ValueString { get; init; }
    public int Bid { get; init; }
    public PokerHandEvaluator.HandRank Value { get; init; }
    public int Winning { get; set; }
}

internal static class PokerHandEvaluator
{
    public enum HandRank
    { 
        None = default,
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind
    }
    
    private static IEnumerable<string> MatrixRecursive(string wip, int deep)
    {
        Console.WriteLine($"{wip} at deep {deep}");
        
        var returnList = new List<string>();
        if (deep == 0)
        {
            returnList.Add(wip);
            return returnList;
        }
        deep--;
        for (var ch = 'a'; ch <= 'l'; ch++)
        {
            returnList.AddRange(MatrixRecursive(wip + ch, deep));
        }
        return returnList;
    }
    
    public static HandRank EvaluateHandWrapper(char[] hand)
    {
        // Identify how many jokers are in the hand
        var jokerCount = hand.Count(c => c == '0');
        
        var realCards = jokerCount == hand.Length ? "" : string.Concat(hand.Where(c => c != '0'));
        
        if (jokerCount == 0)
        {
            return EvaluateHand(hand);
        }
        
        var matrixRecursive = MatrixRecursive(realCards, jokerCount);

        var maxRank = HandRank.None;
        foreach (var currentRank in matrixRecursive
                     .Select(combination => EvaluateHand(combination.ToCharArray()))
                     .Where(currentRank => currentRank > maxRank))
        {
            maxRank = currentRank;
        }
        
        return maxRank;
    }

    private static HandRank EvaluateHand(char[] hand)
    {
        if (IsFiveOfAKind(hand)) return HandRank.FiveOfAKind;
        if (IsFourOfAKind(hand)) return HandRank.FourOfAKind;
        if (IsFullHouse(hand)) return HandRank.FullHouse;
        if (IsThreeOfAKind(hand)) return HandRank.ThreeOfAKind;
        if (IsTwoPair(hand)) return HandRank.TwoPair;
        return IsOnePair(hand) ? HandRank.OnePair : HandRank.HighCard;
    }

    private static bool IsFiveOfAKind(IEnumerable<char> hand)
    {
        var rankGroups = hand.GroupBy(card => card);
        return rankGroups.Any(group => group.Count() == 5);
    }
    
    private static bool IsFourOfAKind(IEnumerable<char> hand)
    {
        var rankGroups = hand.GroupBy(card => card);
        return rankGroups.Any(group => group.Count() == 4);
    }

    private static bool IsFullHouse(IEnumerable<char> hand)
    {
        var rankGroups = hand.GroupBy(card => card);
        var enumerable = rankGroups as IGrouping<char, char>[] ?? rankGroups.ToArray();
        return enumerable.Any(group => group.Count() == 3) && enumerable.Any(group => group.Count() == 2);
    }

    private static bool IsThreeOfAKind(IEnumerable<char> hand)
    {
        var rankGroups = hand.GroupBy(card => card);
        return rankGroups.Any(group => group.Count() == 3);
    }

    private static bool IsTwoPair(IEnumerable<char> hand)
    {
        var rankGroups = hand.GroupBy(card => card);
        return rankGroups.Count(group => group.Count() == 2) == 2;
    }

    private static bool IsOnePair(IEnumerable<char> hand)
    {
        var rankGroups = hand.GroupBy(card => card);
        return rankGroups.Any(group => group.Count() == 2);
    }
}