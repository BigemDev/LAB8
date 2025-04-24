using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
// using Microsoft.AspNetCore.SignalR.Client;

namespace CardGames
{
    public partial class GameWindowBigem : Window
    {
        private const int SmallBlind = 10;
        private const int BigBlind = 20;
        private const int StartingChips = 1000;

        private GameState _gameState;
        private int _dealerIndex;
        private int _currentPlayerIndex;
        private int _highestBetThisRound;
        private int _pot;
        private HashSet<int> _actedPlayers = new HashSet<int>();
        private const int LocalPlayerIndex = 0;

        public GameWindowBigem()
        {
            InitializeComponent();
            ResetGame();
        }

        private void ResetGame()
        {
            _gameState = new GameState();
            _dealerIndex = 0;
            _pot = 0;
            _highestBetThisRound = 0;
            _currentPlayerIndex = 0;
            _actedPlayers.Clear();

            CommunityCardsPlaceholder.Text = "No cards yet";
            PotText.Text = "0";
            CurrentTurnText.Text = "";
            YourHandText.Text = "";
            GameLog.Text = "";
            UpdateBetsDisplay();
        }

        private void StartGame_Click(object? sender, RoutedEventArgs e)
        {
            ResetGame();


            _gameState.Players = Enumerable.Range(1, 2)
                .Select(i => new Player { Name = $"Player {i}", Chips = StartingChips })
                .ToList();

            _gameState.Deck = new Deck();
            _gameState.Deck.Shuffle();
            foreach (var p in _gameState.Players)
                p.Hand = _gameState.Deck.Deal(2);

            PostBlinds();

            _currentPlayerIndex = _dealerIndex;
            _gameState.Round = Round.PreFlop;

            GameLog.Text += "-- Pre-Flop --\n";
            RefreshUI();
        }

        private void PostBlinds()
        {
            _actedPlayers.Clear();

            int sbIdx = _dealerIndex;                     
            int bbIdx = (_dealerIndex + 1) % 2;

            var sb = _gameState.Players[sbIdx];
            var bb = _gameState.Players[bbIdx];

            sb.Chips       -= SmallBlind;
            sb.CurrentBet   = SmallBlind;
            _pot           += SmallBlind;
            _actedPlayers.Add(sbIdx);

            bb.Chips       -= BigBlind;
            bb.CurrentBet   = BigBlind;
            _pot           += BigBlind;
            _actedPlayers.Add(bbIdx);

            _highestBetThisRound = BigBlind;
            PotText.Text         = _pot.ToString();

            GameLog.Text += $"{sb.Name} posts small blind ({SmallBlind}).\n";
            GameLog.Text += $"{bb.Name} posts big blind ({BigBlind}).\n";

            UpdateBetsDisplay();
        }

        private void DealCommunity(int count)
        {
            int toDeal = Math.Min(count, 5 - _gameState.CommunityCards.Count);
            _gameState.CommunityCards.AddRange(_gameState.Deck.Deal(toDeal));
        }

        private void AdvanceRound()
        {

            foreach (var p in _gameState.Players)
                p.CurrentBet = 0;
            _highestBetThisRound = 0;
            _actedPlayers.Clear();

            _dealerIndex = (_dealerIndex + 1) % 2;

            _currentPlayerIndex = (_gameState.Round == Round.PreFlop)
                ? _dealerIndex
                : (_dealerIndex + 1) % 2;

            switch (_gameState.Round)
            {
                case Round.PreFlop:
                    DealCommunity(3);
                    _gameState.Round = Round.Flop;
                    GameLog.Text += "\n-- Flop --\n";
                    break;
                case Round.Flop:
                    DealCommunity(1);
                    _gameState.Round = Round.Turn;
                    GameLog.Text += "\n-- Turn --\n";
                    break;
                case Round.Turn:
                    DealCommunity(1);
                    _gameState.Round = Round.River;
                    GameLog.Text += "\n-- River --\n";
                    break;
                case Round.River:
                    _gameState.Round = Round.Showdown;
                    GameLog.Text += "\n-- Showdown --\n";
                    Showdown();
                    return;
            }

            RefreshUI();
        }

        private void Showdown()
        {
            var results = _gameState.Players
                .Where(p => !p.HasFolded)
                .Select(p => new
                {
                    Player = p,
                    Score  = HandEvaluator.Evaluate(p.Hand
                             .Concat(_gameState.CommunityCards).ToList())
                })
                .ToList();

            results.Sort((a, b) => CompareHandScores(b.Score, a.Score));

            var winner = results.First();
            var desc   = HandEvaluator.Describe(winner.Score);

            GameLog.Text +=
                $"\n{winner.Player.Name} wins the pot of {_pot} with {desc}!\n";
            winner.Player.Chips += _pot;

           if (_gameState.Players.Count(p => p.Chips >= BigBlind) >= 2)
                NextHand();
            else
                GameLog.Text += "\nGame over.\n";
        }

        private static int CompareHandScores((int Category, int[] Ranks) x,
                                             (int Category, int[] Ranks) y)
        {
            int catDiff = x.Category - y.Category;
            if (catDiff != 0)
                return catDiff;

           for (int i = 0; i < Math.Min(x.Ranks.Length, y.Ranks.Length); i++)
            {
                int diff = x.Ranks[i] - y.Ranks[i];
                if (diff != 0)
                    return diff;
            }

            return x.Ranks.Length - y.Ranks.Length;
        }

        private void NextHand()
        {
            _gameState.CommunityCards.Clear();
            foreach (var p in _gameState.Players)
            {
                p.CurrentBet = 0;
                p.HasFolded  = false;
            }
            _pot = 0;
            _highestBetThisRound = 0;
            _actedPlayers.Clear();

            _gameState.Deck.Shuffle();
            foreach (var p in _gameState.Players)
                p.Hand = _gameState.Deck.Deal(2);

            PostBlinds();
            _gameState.Round = Round.PreFlop;
            _currentPlayerIndex = _dealerIndex;

            GameLog.Text += "\n-- New Hand --\n-- Pre-Flop --\n";
            RefreshUI();
        }

        private void TryAdvanceRound()
        {
            var active = _gameState.Players
                .Select((p, i) => new { p.HasFolded, i })
                .Where(x => !x.HasFolded)
                .ToList();

            if (_highestBetThisRound > 0)
            {
                if (active.All(x => _gameState.Players[x.i].CurrentBet == _highestBetThisRound))
                    AdvanceRound();
                else
                    MoveToNextPlayer();
            }
            else
            {
                if (_actedPlayers.Count == active.Count)
                    AdvanceRound();
                else
                    MoveToNextPlayer();
            }
        }

        private void MoveToNextPlayer()
        {
            do
            {
                _currentPlayerIndex = (_currentPlayerIndex + 1) % 2;
            }
            while (_gameState.Players[_currentPlayerIndex].HasFolded);
            RefreshUI();
        }

        private void Bet_Click(object? sender, RoutedEventArgs e)
{
    int idx    = _currentPlayerIndex;
    var player = _gameState.Players[idx];

    if (player.HasFolded)
        return;

    if (!int.TryParse(BetAmountTextBox.Text, out int targetBet))
    {
        GameLog.Text += "\nInvalid bet amount.\n";
        return;
    }

    int current = player.CurrentBet;
    int toCall  = Math.Max(0, _highestBetThisRound - current);

    if (targetBet < current + toCall)
    {
        GameLog.Text += $"\nMust at least call ({toCall}).\n";
        return;
    }

    int raiseAmt = targetBet - _highestBetThisRound;
    if (raiseAmt > 0 && raiseAmt < BigBlind)
    {
        GameLog.Text += $"\nRaise must be ≥ big blind ({BigBlind}).\n";
        return;
    }

    int delta = targetBet - current;
    if (delta > player.Chips)
    {
        GameLog.Text += "\nNot enough chips.\n";
        return;
    }

    player.Chips        -= delta;
    player.CurrentBet    = targetBet;
    _pot               += delta;
    _highestBetThisRound = Math.Max(_highestBetThisRound, targetBet);

    if (delta == toCall)
        GameLog.Text += $"\n{player.Name} calls {toCall}.\n";
    else if (toCall == 0)
        GameLog.Text += $"\n{player.Name} bets {delta}.\n";
    else
        GameLog.Text += $"\n{player.Name} calls {toCall} and raises {raiseAmt}.\n";

    PotText.Text = _pot.ToString();
    _actedPlayers.Add(idx);
    TryAdvanceRound();
}

private void Call_Click(object? sender, RoutedEventArgs e)
{
    int idx = _currentPlayerIndex;
    var p   = _gameState.Players[idx];
    int toCall = _highestBetThisRound - p.CurrentBet;

    if (toCall <= 0)
    {
        GameLog.Text += "\nNothing to call.\n";
        return;
    }
    if (toCall > p.Chips)
    {
        GameLog.Text += "\nNot enough chips.\n";
        return;
    }

    p.Chips       -= toCall;
    p.CurrentBet  += toCall;
    _pot          += toCall;

    GameLog.Text += $"\n{p.Name} calls {toCall}.\n";
    PotText.Text = _pot.ToString();

    _actedPlayers.Add(idx);
    TryAdvanceRound();
}

private void Check_Click(object? sender, RoutedEventArgs e)
{
    int idx = _currentPlayerIndex;
    var p   = _gameState.Players[idx];

    if (_highestBetThisRound > p.CurrentBet)
    {
        GameLog.Text += "\nCannot check: outstanding bet.\n";
        return;
    }

    GameLog.Text += $"\n{p.Name} checks.\n";
    _actedPlayers.Add(idx);
    TryAdvanceRound();
}

private void Fold_Click(object? sender, RoutedEventArgs e)
{
    int idx = _currentPlayerIndex;
    var p   = _gameState.Players[idx];
    p.HasFolded = true;

    GameLog.Text += $"\n{p.Name} folds.\n";
    UpdateBetsDisplay();

    _actedPlayers.Add(idx);
    TryAdvanceRound();
}

private void LeaveGame_Click(object? sender, RoutedEventArgs e)
{
    Close();
}


        private void RefreshUI()
        {
            var local = _gameState.Players[LocalPlayerIndex];
            YourHandText.Text   = $"Your hand: {string.Join(", ", local.Hand)}";

            CurrentTurnText.Text = $"Current turn: {_gameState.Players[_currentPlayerIndex].Name}";

            CommunityCardsPlaceholder.Text = _gameState.CommunityCards.Any()
                ? string.Join(" | ", _gameState.CommunityCards)
                : "No cards yet";
            
            PotText.Text = _pot.ToString();
            UpdateBetsDisplay();
        }

        private void UpdateBetsDisplay()
        {
            BetsDisplay.Children.Clear();
            foreach (var p in _gameState.Players)
            {
                var tb = new TextBlock
                {
                    Text       = $"{p.Name}: Bet {p.CurrentBet}, Chips {p.Chips}",
                    Foreground = Brushes.White,
                    Margin     = new Thickness(0, 2)
                };
                BetsDisplay.Children.Add(tb);
            }
        }
    }
    #region Models & Evaluator

    public enum Round { PreFlop, Flop, Turn, River, Showdown }

    public class GameState
    {
        public Deck Deck { get; set; } = new Deck();
        public List<Player> Players { get; set; } = new List<Player>();
        public List<Card> CommunityCards { get; set; } = new List<Card>();
        public Round Round { get; set; }
    }

    public class Player
    {
        public string Name { get; set; }
        public List<Card> Hand { get; set; } = new List<Card>();
        public int Chips { get; set; }
        public int CurrentBet { get; set; }
        public bool HasFolded { get; set; }
    }

    public class Card
    {
        public string Rank { get; set; }
        public string Suit { get; set; }
        public override string ToString() => $"{Rank}{Suit[0]}";
    }

    public class Deck
    {
        private readonly Random _rand = new Random();
        private List<Card> _cards;
        public Deck()
        {
            var ranks = new[] { "2","3","4","5","6","7","8","9","10","J","Q","K","A" };
            var suits = new[] { "Hearts","Diamonds","Clubs","Spades" };
            _cards = ranks.SelectMany(r => suits.Select(s => new Card{Rank=r,Suit=s})).ToList();
        }
        public void Shuffle() => _cards = _cards.OrderBy(_ => _rand.Next()).ToList();
        public List<Card> Deal(int n)
        {
            var hand = _cards.Take(n).ToList();
            _cards.RemoveRange(0, n);
            return hand;
        }
    }

    public static class HandEvaluator
    {
        private static readonly string[] Ranks =
            { "2","3","4","5","6","7","8","9","10","J","Q","K","A" };

        public static (int Category, int[] Ranks) Evaluate(List<Card> cards)
        {
            if (cards.Count < 5)
                throw new ArgumentException("Need at least 5 cards", nameof(cards));

            var bySuit = cards.GroupBy(c => c.Suit)
                              .ToDictionary(g => g.Key, g => g.ToList());
            var byRank = cards.GroupBy(c => c.Rank)
                              .ToDictionary(g => g.Key, g => g.Count());

            bool IsFlush(out List<Card> flushCards)
            {
                foreach (var kv in bySuit)
                    if (kv.Value.Count >= 5)
                    {
                        flushCards = kv.Value
                            .OrderByDescending(c => Array.IndexOf(Ranks, c.Rank))
                            .ToList();
                        return true;
                    }
                flushCards = null;
                return false;
            }

            bool IsStraight(IEnumerable<Card> src, out List<string> seq)
            {
                var distinct = src.Select(c => Array.IndexOf(Ranks, c.Rank))
                                  .Distinct().OrderBy(i => i).ToList();
                if (distinct.Contains(12)) distinct.Add(-1);
                int consec = 1, bestStart = -1;
                for (int i = 1; i < distinct.Count; i++)
                {
                    consec = (distinct[i] == distinct[i - 1] + 1) ? consec + 1 : 1;
                    if (consec >= 5) bestStart = distinct[i] - 4;
                }
                if (bestStart >= 0)
                {
                    seq = Enumerable.Range(bestStart, 5)
                                    .Select(i => Ranks[(i + 13) % 13])
                                    .ToList();
                    return true;
                }
                seq = null;
                return false;
            }

            // 1) Straight Flush / Royal Flush
            if (IsFlush(out var flushCards) && IsStraight(flushCards, out var sfSeq))
            {
                if (sfSeq[0] == "10" && sfSeq[4] == "A")
                    return (9, sfSeq.Select(r => Array.IndexOf(Ranks, r)).ToArray());
                return (8, sfSeq.Select(r => Array.IndexOf(Ranks, r)).ToArray());
            }

            // 2) Four of a Kind
            var four = byRank.Where(kv => kv.Value == 4)
                             .Select(kv => Array.IndexOf(Ranks, kv.Key))
                             .ToList();
            if (four.Any())
            {
                var kicker = cards.Where(c => Array.IndexOf(Ranks, c.Rank) != four[0])
                                  .Max(c => Array.IndexOf(Ranks, c.Rank));
                return (7, new[] { four[0], kicker });
            }

            // 3) Full House
            var threes = byRank.Where(kv => kv.Value == 3)
                              .Select(kv => Array.IndexOf(Ranks, kv.Key))
                              .OrderByDescending(i => i).ToList();
            var pairs = byRank.Where(kv => kv.Value >= 2)
                              .Select(kv => Array.IndexOf(Ranks, kv.Key))
                              .OrderByDescending(i => i).ToList();
            if (threes.Any() && pairs.Count >= 2)
                return (6, new[] { threes[0], pairs.First(p => p != threes[0]) });

            // 4) Flush
            if (flushCards != null)
                return (5, flushCards.Take(5).Select(c => Array.IndexOf(Ranks, c.Rank)).ToArray());

            // 5) Straight
            if (IsStraight(cards, out var stSeq))
                return (4, stSeq.Select(r => Array.IndexOf(Ranks, r)).ToArray());

            // 6) Three of a Kind
            if (threes.Any())
            {
                var kickers = cards.Where(c => Array.IndexOf(Ranks, c.Rank) != threes[0])
                                   .Select(c => Array.IndexOf(Ranks, c.Rank))
                                   .OrderByDescending(i => i)
                                   .Take(2).ToArray();
                return (3, new[] { threes[0] }.Concat(kickers).ToArray());
            }

            // 7) Two Pair
            if (pairs.Count >= 2)
            {
                var top2 = pairs.Take(2).ToArray();
                var kicker = cards.Where(c => !top2.Contains(Array.IndexOf(Ranks, c.Rank)))
                                  .Max(c => Array.IndexOf(Ranks, c.Rank));
                return (2, new[] { top2[0], top2[1], kicker });
            }

            // 8) One Pair
            if (pairs.Any())
            {
                var pr = pairs[0];
                var kickers = cards.Where(c => Array.IndexOf(Ranks, c.Rank) != pr)
                                   .Select(c => Array.IndexOf(Ranks, c.Rank))
                                   .OrderByDescending(i => i)
                                   .Take(3).ToArray();
                return (1, new[] { pr }.Concat(kickers).ToArray());
            }

            // 9) High Card
            var top5 = cards.Select(c => Array.IndexOf(Ranks, c.Rank))
                            .OrderByDescending(i => i)
                            .Take(5).ToArray();
            return (0, top5);
        }

        public static string Describe((int Category, int[] Ranks) hand)
        {
            int c = hand.Category;
            string r(int i) => Ranks[hand.Ranks[i]];
            return c switch
            {
                9 => "Royal Flush",
                8 => $"Straight Flush to {r(4)}",
                7 => $"Four of a Kind, {r(0)}s w/ kicker {r(1)}",
                6 => $"Full House, {r(0)}s full of {r(1)}s",
                5 => $"Flush, top {r(0)}",
                4 => $"Straight to {r(4)}",
                3 => $"Three of a Kind, {r(0)}s",
                2 => $"Two Pair, {r(0)}s & {r(1)}s",
                1 => $"Pair of {r(0)}s",
                _ => $"High Card {r(0)}"
            };
        }
    }

    #endregion
}
