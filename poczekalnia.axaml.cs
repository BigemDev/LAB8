using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MySqlConnector;

namespace CardGames;

public partial class poczekalnia : Window {
    private int wylosowana;
    public int liczba_graczy = 1;
    public balatro gra1;
    public balatro gra2;
    public bool win_player_one; //WYGRANA GRACZA NUMER JEDEN
    
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
        public List<karta> _cards;
        public Deck() {
            var ranks = new[] { "2","3","4","5","6","7","8","9","10","J","Q","K","A" };
            var suits = new[] { "\u2665\ufe0f","\u2666\ufe0f","\u2660\ufe0f","\u2663\ufe0f" };
            _cards = ranks.SelectMany(r => suits.Select(s => new karta{rank = r,suit = s, status = "gotowa", czy_wybrana = false})).ToList();
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
                gra1.od.Text = "4";
                gra2.od.Text = "4";
                gra1.zag.Text = "4";
                gra2.zag.Text = "4";
                gra1.ID = 1;
                gra2.ID = 2;
                gra1.nastepna = gra2;
                gra2.nastepna = gra1;
        }
    }
    
//opierdol od Bigema za polski znak w nazwie funkcji
    public void zakończenie_gry() 
    {
        if (gra1.liczba_zagran == 0 && gra2.liczba_zagran == 0) 
        {
            gra1.Close();
            gra2.Close();
        
            bool? gameResult = null;
        
            if (gra1.wynik > gra2.wynik) 
            {
                gameResult = true;
                gracz_1.Text = "WYGRANA";
                gracz_2.Text = "PRZEGRANA";
            }
            else if (gra1.wynik < gra2.wynik) 
            {
                gameResult = null;
                gracz_1.Text = "PRZEGRANA";
                gracz_2.Text = "WYGRANA";
            }
            else 
            {
                gameResult = false;
                gracz_1.Text = "REMIS";
                gracz_2.Text = "REMIS";
            }
            
            if (gameResult != null)
            {
                RecordGameResult(gameResult.Value, MainWindow.Globals.username);
            }
        }
    }

     public void RecordGameResult(bool playerWon, string username)
        {
            string connStr = "server=127.0.0.1;port=3306;user=appuser;password=apppassword;database=login_demo;";
            using var conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                int userId = 0;
                string getUserSql = "SELECT id FROM users WHERE username = @username";
                using (var userCmd = new MySqlCommand(getUserSql, conn))
                {
                    userCmd.Parameters.AddWithValue("@username", username);
                    var result = userCmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        userId = Convert.ToInt32(result);
                    }
                    else
                    {
                        Console.WriteLine($"User '{username}' not found in database");
                        return;
                    }
                }
                string insertSql = @"
                    INSERT INTO game_history 
                    (user_id, game, won, played_at) 
                    VALUES 
                    (@user_id, @game, @won, @played_at)";
                using var historyCmd = new MySqlCommand(insertSql, conn);
                {
                    historyCmd.Parameters.AddWithValue("@user_id", userId);
                    historyCmd.Parameters.AddWithValue("@game", "balatro");
                    historyCmd.Parameters.AddWithValue("@won", playerWon ? 1 : 0);
                    historyCmd.Parameters.AddWithValue("@played_at", DateTime.Now);
                    int rowsAffected = historyCmd.ExecuteNonQuery();
                    if (rowsAffected == 1)
                    {
                        Console.WriteLine("Successfully recorded game result");
                    }
                    else
                    {
                        Console.WriteLine("Failed to record game result");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
        }
    
    public void wyjscie(object sender, RoutedEventArgs e) {
        this.Close();
    }
}