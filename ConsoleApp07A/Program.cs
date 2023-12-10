/*
 * FileName: Program.cs
 * Author: Benjamin Cederholm
 * Date Created: 2023-10-07
 * Last Modified: 2023-12-10
 * Description: https://adventofcode.com/2023/day/7 - Part One
 * Keywords: Poker
 */

const string filePath1 = "input.txt";
var lines1 = File.ReadAllLines(filePath1);

var cardRanks = new Dictionary<string, Rank>
{
    {"A", Rank.Ace},
    {"K", Rank.King},
    {"Q", Rank.Queen},
    {"J", Rank.Jack},
    {"T", Rank.Ten},
    {"9", Rank.Nine},
    {"8", Rank.Eight},
    {"7", Rank.Seven},
    {"6", Rank.Six},
    {"5", Rank.Five},
    {"4", Rank.Four},
    {"3", Rank.Three},
    {"2", Rank.Two}
};

var hands = (
    from line in lines1 select line.Split(" ", StringSplitOptions.RemoveEmptyEntries)
    into lineSegments let lineCards = new List<Card>(lineSegments[0].ToCharArray().Select(s => new Card(cardRanks[s.ToString()])).ToArray())
    select new Hand { Cards = lineCards, Bid = Convert.ToInt32(lineSegments[1]), Value = PokerHandEvaluator.EvaluateHand(lineCards) }).ToList();

var sortedHands = hands.OrderBy(h => h.Value)
    .ThenBy(h => h.Cards![0].Rank)
    .ThenBy(h => h.Cards![1].Rank)
    .ThenBy(h => h.Cards![2].Rank)
    .ThenBy(h => h.Cards![3].Rank)
    .ThenBy(h => h.Cards![4].Rank)
    .ToList();

var rankNumber = 0;
foreach (var hand in sortedHands)
{
    rankNumber++;
    hand.Winning = hand.Bid * rankNumber;
}

var totalWinnings = sortedHands.Sum(h => h.Winning);

Console.WriteLine($"Answer: {totalWinnings}");

internal enum Rank
{
    Two = 2,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
    Ace
}

internal class Hand
{
    public List<Card>? Cards { get; init; }
    public int Bid { get; init; }
    public PokerHandEvaluator.HandRank Value { get; init; }
    public int Winning { get; set; }
}

internal class Card
{
    public Rank Rank { get; }
    public Card(Rank rank)
    {
        Rank = rank;
    }
}

// Credit: https://blog.stackademic.com/building-a-simple-poker-hand-evaluator-in-c-1bb81676c25c
internal static class PokerHandEvaluator
{
    public enum HandRank
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind
    }

    public static HandRank EvaluateHand(List<Card> hand)
    {
        if (IsFiveOfAKind(hand)) return HandRank.FiveOfAKind;
        if (IsFourOfAKind(hand)) return HandRank.FourOfAKind;
        if (IsFullHouse(hand)) return HandRank.FullHouse;
        if (IsThreeOfAKind(hand)) return HandRank.ThreeOfAKind;
        if (IsTwoPair(hand)) return HandRank.TwoPair;
        return IsOnePair(hand) ? HandRank.OnePair : HandRank.HighCard;
    }

    private static bool IsFiveOfAKind(IEnumerable<Card> hand)
    {
        var rankGroups = hand.GroupBy(card => card.Rank);
        return rankGroups.Any(group => group.Count() == 5);
    }
    
    private static bool IsFourOfAKind(IEnumerable<Card> hand)
    {
        var rankGroups = hand.GroupBy(card => card.Rank);
        return rankGroups.Any(group => group.Count() == 4);
    }

    private static bool IsFullHouse(IEnumerable<Card> hand)
    {
        var rankGroups = hand.GroupBy(card => card.Rank);
        var enumerable = rankGroups as IGrouping<Rank, Card>[] ?? rankGroups.ToArray();
        return enumerable.Any(group => group.Count() == 3) && enumerable.Any(group => group.Count() == 2);
    }

    private static bool IsThreeOfAKind(IEnumerable<Card> hand)
    {
        var rankGroups = hand.GroupBy(card => card.Rank);
        return rankGroups.Any(group => group.Count() == 3);
    }

    private static bool IsTwoPair(IEnumerable<Card> hand)
    {
        var rankGroups = hand.GroupBy(card => card.Rank);
        return rankGroups.Count(group => group.Count() == 2) == 2;
    }

    private static bool IsOnePair(IEnumerable<Card> hand)
    {
        var rankGroups = hand.GroupBy(card => card.Rank);
        return rankGroups.Any(group => group.Count() == 2);
    }
}