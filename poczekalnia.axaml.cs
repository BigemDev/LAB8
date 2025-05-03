using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace CardGames;

public partial class poczekalnia : Window
{
    private int wylosowana;
    public class Player {
        public string name { get; set; }
        public List<karta> hand { get; set; } = new List<karta>();
    }

    public class karta {
        public string rank { get; set; }
        public string suit { get; set; }
        public string status { get; set; } 
        public bool czy_wybrana { get; set; }
        public override string ToString() => $"{rank}{suit}";
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

    public karta wylosuj(Random rnd) {
        wylosowana = rnd.Next(0, deck._cards.Count);
        while (deck._cards[wylosowana].status == "uzywana" || deck._cards[wylosowana].status == "odrzucona") {
            wylosowana = rnd.Next(0, deck._cards.Count);
        }
        deck._cards[wylosowana].status = "uzywana";
        return deck._cards[wylosowana];
    }

    public void start(object sender, RoutedEventArgs e) {
        balatro gra = new balatro(this);
        Random rnd = new Random();
        for (int i = 0; i < 7; i++) {
            gra.wylosowane.Add(wylosuj(rnd));
        }
        gra.Show();
    }
}