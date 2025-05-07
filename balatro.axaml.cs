using System;
using System.Collections.Immutable;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CardGames;

public partial class balatro : Window, INotifyPropertyChanged
{
    private poczekalnia baza;
    private int nowa;
    private int ile_dodac = 0;
    private int liczba_odrzucen = 4;
    private int liczba_zagran = 4;
    private double wynik = 0;
    public ObservableCollection<poczekalnia.karta> do_uzycia { get; set; } = new ObservableCollection<poczekalnia.karta>();
    public ObservableCollection<poczekalnia.karta> wylosowane { get; set; } = new ObservableCollection<poczekalnia.karta>();
    public ObservableCollection<poczekalnia.karta> zagrane { get; set; } = new ObservableCollection<poczekalnia.karta>();
    public int[] wartosci;

    public balatro(poczekalnia p) {
        InitializeComponent();
        baza = p;
        DataContext = this;
        wartosci = new[] {0, 0, 0, 0, 0};
    }

    public void odrzucenie(object sender, RoutedEventArgs e) {
        if (liczba_odrzucen != 0) {
            Random rnd = new Random();
            for (int i = wylosowane.Count - 1; i >= 0; i--) {
                if (wylosowane[i].czy_wybrana) {
                    wylosowane.RemoveAt(i);
                    ile_dodac += 1;
                }
            }
            for (int i = 0; i < ile_dodac; i++) {
                nowa = rnd.Next(0, do_uzycia.Count); 
                wylosowane.Add(do_uzycia[nowa]); 
                do_uzycia.RemoveAt(nowa); 
            }
            ile_dodac = 0;
            liczba_odrzucen -= 1;
        }
        else {
            od.Text = "Wykorzystano wszytskie odrzucenia";
        }
    }

    public void zagranie(object sender, RoutedEventArgs e) {
        if (zagrane.Count() <= 5) {
            if (liczba_zagran != 100)
            {
                for (int i = 0; i < wylosowane.Count(); i++) {
                    wartosci[i] = Convert.ToInt32(wylosowane[i]);
                }
                Random rnd = new Random();
                for (int i = wylosowane.Count - 1; i >= 0; i--)
                {
                    if (wylosowane[i].czy_wybrana)
                    {
                        zagrane.Add(wylosowane[i]);
                        wynik += wylosowane[i].wartosc;
                        wylosowane.RemoveAt(i);
                        ile_dodac += 1;
                    }
                }

                for (int i = 0; i < ile_dodac; i++)
                {
                    nowa = rnd.Next(0, do_uzycia.Count);
                    wylosowane.Add(do_uzycia[nowa]);
                    do_uzycia.RemoveAt(nowa);
                }

                // var sort = wartosci.OrderBy(k => k).ToArray();

                // if (//royal flush
                    // sort[0] == 10
                    // && sort[1] == 11
                    // && sort[2] == 12
                    // && sort[3] == 13
                    // && sort[4] == 14
                    // && zagrane[0].suit == zagrane[1].suit && zagrane[0].suit == zagrane[2].suit && zagrane[0].suit == zagrane[3].suit && zagrane[0].suit == zagrane[4].suit)
                // {
                    // wynik += 700;
                // }
                // else if (//straight flush
                         // sort[0] == sort[1] - 1
                         // && sort[1] == sort[2] - 1
                         // && sort[2] == sort[3] - 1
                         // && sort[3] == sort[4] - 1 
                         // && zagrane[0].suit == zagrane[1].suit && zagrane[0].suit == zagrane[2].suit && zagrane[0].suit == zagrane[3].suit && zagrane[0].suit == zagrane[4].suit)
                // {
                    // wynik += 600;
                // }
                // else if (//four of a kind
                         // (sort[0] == sort[1] || sort[3] == sort[4])
                         // && sort[1] == sort[2]
                         // && sort[2] == sort[3])
                // {
                    // wynik += 500;
                // }
                // else if (//full house
                         // sort[0] == sort[1]
                         // && sort[3] == sort[4]
                         // && (sort[1] == sort[2]) || (sort[2] == sort[3]))
                // {
                    // wynik += 400;
                // }
                // else if (//flush
                         // zagrane[0].suit == zagrane[1].suit && zagrane[0].suit == zagrane[2].suit && zagrane[0].suit == zagrane[3].suit && zagrane[0].suit == zagrane[4].suit)
                // {
                    // wynik += 300;
                // }
                // else if (//straight
                         // sort[0] == sort[1] - 1
                         // && sort[1] == sort[2] - 1
                         // && sort[2] == sort[3] - 1
                         // && sort[3] == sort[4] - 1)
                // {
                    // wynik += 200;
                // }
                // else if (//tree of a kind
                         // ((sort[0] == sort[1]) && (sort[1] == sort[2]) 
                          // || (sort[1] == sort[2]) && (sort[2] == sort[3]) 
                          // || (sort[2] == sort[3]) && (sort[3] == sort[4])))
                // {
                    // wynik += 100;
                // }
                // else if (//two pair
                         // (sort[0] == sort[1]) && ((sort[2] == sort[3]) || (sort[3] == sort[4])) || (sort[1] == sort[2] && sort[3] == sort[4]))
                // {
                    // wynik += 70;
                // }
                // else if (//pair
                         // ((sort[0] == sort[1]) 
                          // || (sort[1] == sort[2]) 
                          // || (sort[2] == sort[3]) 
                          // || (sort[3] == sort[4]) ))
                // {
                    // wynik += 30;
                // }

                /*---------------*/
                ile_dodac = 0;
                liczba_zagran -= 1;
                wyswietlanie_wyniku.Text = Convert.ToString(wynik);
            }
            else
            {
                zag.Text = "Wykorzystano wszystkie zagrania!";
            }
        }
        else {
            zag.Text = "Za dużo zagranych kart!";
        }
    }
/*
    public static class ustawienia
    {
        private static readonly string[] Rangi =
            { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

        public static (int Category, int[] Ranks) Evaluate(ObservableCollection<poczekalnia.karta> cards) {
            var bySuit = cards.GroupBy(c => c.suit)
                .ToDictionary(g => g.Key, g => g.ToList());
            var byRank = cards.GroupBy(c => c.rank)
                .ToDictionary(g => g.Key, g => g.Count());

            bool IsFlush(out ObservableCollection<poczekalnia.karta> flushCards)
            {
                foreach (var kv in bySuit)
                    if (kv.Value.Count >= 5)
                    {
                        flushCards = kv.Value
                            .OrderByDescending(c => Array.IndexOf(Rangi, c.Rank))
                            .ToList();
                        return true;
                    }

                flushCards = null;
                return false;
            }

            bool IsStraight(IEnumerable<Card> src, out List<string> seq)
            {
                var distinct = src.Select(c => Array.IndexOf(GetAutoSafeAreaPadding(Rangi, c.Rank))
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
    }
    */
}