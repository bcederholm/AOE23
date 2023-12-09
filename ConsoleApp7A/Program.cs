using System.Text.RegularExpressions;

const string filePath1 = "C:\\repos\\offside\\ConsoleApp1\\ConsoleApp7A\\Input7.txt";

var lines1 = File.ReadAllLines(filePath1);

var cardRanks = new Dictionary<string, Rank>()
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

var evaluator = new PokerHandEvaluator();

var hands = new List<Hand>();

foreach (var line in lines1)
{
    var lineSegments = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

    var lineCards = new List<Card>(lineSegments[0].ToCharArray().Select(s => new Card(cardRanks[s.ToString()])).ToArray());
    
    hands.Add(new Hand()
    {
        CardsString = lineSegments[0],
        Cards = lineCards,
        Bid = Convert.ToInt32(lineSegments[1]),
        Value = evaluator.EvaluateHand(lineCards),
    });
}

var sortedHands = hands.OrderBy(h => h.Value).ThenBy(h => h.Cards[0].Rank).ThenBy(h => h.Cards[1].Rank).ThenBy(h => h.Cards[2].Rank).ThenBy(h => h.Cards[3].Rank).ThenBy(h => h.Cards[4].Rank).ToList();

var rankNumber = 0;
foreach (var hand in sortedHands)
{
    rankNumber++;
    hand.Winning = hand.Bid * rankNumber;
}

var totalWinnings = sortedHands.Sum(h => h.Winning);

Console.WriteLine($"totalWinnings: {totalWinnings}");

public enum Rank
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

public class Hand
{
    public string CardsString { get; set; }
    public List<Card> Cards { get; set; }
    public int Bid { get; set; }
    public PokerHandEvaluator.HandRank Value { get; set; }
    public int Winning { get; set; }
}


public class Card
{
    public Rank Rank { get; }
    public Card(Rank rank)
    {
        Rank = rank;
    }
}


public class PokerHandEvaluator
{
    public enum HandRank
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind,
    }

    public HandRank EvaluateHand(List<Card> hand)
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
        return rankGroups.Any(group => group.Count() == 3) && rankGroups.Any(group => group.Count() == 2);
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