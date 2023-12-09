const string filePath1 = "C:\\repos\\offside\\ConsoleApp01\\ConsoleApp07A\\Input7.txt";
var lines1 = File.ReadAllLines(filePath1);
var evaluator = new PokerHandEvaluator();
var hands = new List<Hand>();

foreach (var line in lines1)
{
    var lineSegments = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

    var originalString = lineSegments[0];

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

    hands.Add(new Hand()
    {
        OriginalString = originalString,
        ValueString = lineSegments[0],
        Cards = lineSegments[0].ToCharArray(),
        Bid = Convert.ToInt32(lineSegments[1]),
        Value = evaluator.EvaluateHandWrapper(lineSegments[0].ToCharArray()),
    });
}

var sortedHands = hands.OrderBy(h => h.Value).ThenBy(h => h.ValueString);

var rankNumber = 0;
foreach (var hand in sortedHands)
{
    rankNumber++;
    hand.Winning = hand.Bid * rankNumber;
}

var totalWinnings = sortedHands.Sum(h => h.Winning);

Console.WriteLine($"totalWinnings: {totalWinnings}");

public class Hand
{
    public string OriginalString { get; set; }
    public string ValueString { get; set; }
    public char[] Cards { get; set; }
    public int Bid { get; set; }
    public PokerHandEvaluator.HandRank Value { get; set; }
    public int Winning { get; set; }
}

public class PokerHandEvaluator
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
        FiveOfAKind,
    }

    
    
    public List<string> MatrixRecursive(string wip, int deep)
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
    
    public HandRank EvaluateHandWrapper(char[] hand)
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
        foreach (var combination in matrixRecursive)
        {
            var currentRank = EvaluateHand(combination.ToCharArray());
            if (currentRank > maxRank)
            {
                maxRank = currentRank;
            }
        }
        
        return maxRank;
    }

    public HandRank EvaluateHand(char[] hand)
    {
        if (IsFiveOfAKind(hand)) return HandRank.FiveOfAKind;
        if (IsFourOfAKind(hand)) return HandRank.FourOfAKind;
        if (IsFullHouse(hand)) return HandRank.FullHouse;
        if (IsThreeOfAKind(hand)) return HandRank.ThreeOfAKind;
        if (IsTwoPair(hand)) return HandRank.TwoPair;
        return IsOnePair(hand) ? HandRank.OnePair : HandRank.HighCard;
    }

    private static bool IsFiveOfAKind(char[] hand)
    {
        var rankGroups = hand.GroupBy(card => card);
        return rankGroups.Any(group => group.Count() == 5);
    }
    
    private static bool IsFourOfAKind(char[] hand)
    {
        var rankGroups = hand.GroupBy(card => card);
        return rankGroups.Any(group => group.Count() == 4);
    }

    private static bool IsFullHouse(char[] hand)
    {
        var rankGroups = hand.GroupBy(card => card);
        return rankGroups.Any(group => group.Count() == 3) && rankGroups.Any(group => group.Count() == 2);
    }

    private static bool IsThreeOfAKind(char[] hand)
    {
        var rankGroups = hand.GroupBy(card => card);
        return rankGroups.Any(group => group.Count() == 3);
    }

    private static bool IsTwoPair(char[] hand)
    {
        var rankGroups = hand.GroupBy(card => card);
        return rankGroups.Count(group => group.Count() == 2) == 2;
    }

    private static bool IsOnePair(char[] hand)
    {
        var rankGroups = hand.GroupBy(card => card);
        return rankGroups.Any(group => group.Count() == 2);
    }
}