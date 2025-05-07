using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CardGames;

public partial class poczekalnia : Window {
    private int wylosowana;
    public int liczba_graczy = 1;
    public balatro gra1;
    public balatro gra2;
    public class Player {
        public string name { get; set; }
        public List<karta> hand { get; set; } = new List<karta>();
    }
    
    public class karta {
        public string rank { get; set; }
        public string suit { get; set; }
        public double kolejnosc { get; set; }
        public string status { get; set; } 
        private bool _czy_wybrana { get; set; }
        public double wartosc { get; set; }
        public bool czy_wybrana {
            get => _czy_wybrana;
            set { _czy_wybrana = value; OnPropertyChanged(); }
        }
        public override string ToString() => $"{rank}{suit}";
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Deck {
        private readonly Random _rand = new Random();
        public List<karta> _cards;
        public Deck() {
            var ranks = new[] { "2","3","4","5","6","7","8","9","10","J","Q","K","A" };
            var suits = new[] { "\u2665\ufe0f","\u2666\ufe0f","\u2660\ufe0f","\u2663\ufe0f" };
            _cards = ranks.SelectMany(r => suits.Select(s => new karta{rank = r,suit = s, status = "gotowa", czy_wybrana = false})).ToList();
        }
        public void Shuffle() => _cards = _cards.OrderBy(_ => _rand.Next()).ToList();
        
        public List<karta> Deal(int n) {
            var hand = _cards.Take(n).ToList();
            _cards.RemoveRange(0, n);
            return hand;
        }
    }

    private Deck deck = new Deck();
    
    public poczekalnia() {
        InitializeComponent();
    }

    public void start(object sender, RoutedEventArgs e) {
        if (liczba_graczy == 1) {
            for (int i = 0; i < deck._cards.Count; i++) {
                if (double.TryParse(deck._cards[i].rank, out double val)) {
                    deck._cards[i].wartosc = val;
                    deck._cards[i].kolejnosc = val;
                }
                else if (deck._cards[i].rank is "J") {
                    deck._cards[i].wartosc = 10;
                    deck._cards[i].kolejnosc = 11;
                }
                else if (deck._cards[i].rank is "Q") {
                    deck._cards[i].wartosc = 10;
                    deck._cards[i].kolejnosc = 12;
                }
                else if (deck._cards[i].rank is "K") {
                    deck._cards[i].wartosc = 10;
                    deck._cards[i].kolejnosc = 13;
                }
                else if (deck._cards[i].rank == "A") {
                    deck._cards[i].wartosc = 11;
                    deck._cards[i].kolejnosc = 14;
                }

                // Console.WriteLine(deck._cards[i].suit);
                // Console.WriteLine(deck._cards[i].rank);
                // Console.WriteLine(deck._cards[i].wartosc);
                // Console.WriteLine(deck._cards[i].kolejnosc);
            }

            Random rnd = new Random();
                gra1 = new balatro(this);
                for (int i = 0; i < 7; i++) {
                    wylosowana = rnd.Next(0, deck._cards.Count);
                    while (deck._cards[wylosowana].status == "uzywana" || deck._cards[wylosowana].status == "odrzucona") {
                        wylosowana = rnd.Next(0, deck._cards.Count);
                    }

                    deck._cards[wylosowana].status = "uzywana";
                    gra1.wylosowane.Add(deck._cards[wylosowana]);
                }

                for (int i = 0; i < deck._cards.Count; i++) {
                    if (deck._cards[i].status == "gotowa") {
                        gra1.do_uzycia.Add(deck._cards[i]);
                    }
                }
                gra1.Show();
                liczba_graczy += 1;
                /*----------------------------*/
                gra2 = new balatro(this);
                for (int i = 0; i < 7; i++) {
                    wylosowana = rnd.Next(0, deck._cards.Count);
                    while (deck._cards[wylosowana].status == "uzywana" || deck._cards[wylosowana].status == "odrzucona") {
                        wylosowana = rnd.Next(0, deck._cards.Count);
                    }

                    deck._cards[wylosowana].status = "uzywana";
                    gra2.wylosowane.Add(deck._cards[wylosowana]);
                }

                for (int i = 0; i < deck._cards.Count; i++) {
                    if (deck._cards[i].status == "gotowa") {
                        gra2.do_uzycia.Add(deck._cards[i]);
                    }
                }
                gra1.numer_gracza.Text = "GRACZ NR. 1";
                gra2.numer_gracza.Text = "GRACZ NR. 2";
                gra1.ID = 1;
                gra2.ID = 2;
                gra1.nastepna = gra2;
                gra2.nastepna = gra1;

            // gra.wylosowane = new ObservableCollection<poczekalnia.karta>(gra.wylosowane.OrderBy(k => k.kolejnosc).ToList());
            {
                // Console.WriteLine(x.suit);
                // Console.WriteLine(x.kolejnosc);

            }
        }
    }
}