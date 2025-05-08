using System;
using System.Media;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Media;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using MySqlConnector;

namespace CardGames;

public partial class grawojna : Window
{
    private List<int> wylosowaneKartygracz1 = new List<int>();
    private List<int> wylosowaneKartygracz2 = new List<int>();
    private List<int> stockgracz1 = new List<int>();
    private List<int> stockgracz2 = new List<int>();
    private List<string> stockgraczkolor1 = new List<string>();
    private List<string> stockgraczkolor2 = new List<string>();
    private List<int> tymstock = new List<int>();
    private List<string> tymstockkolor = new List<string>();
    private bool czekgracz1 = false;
    private bool czekgracz2 = false;
    private int liczbazaznaczona1;
    private int liczbazaznaczona2;
    private string kolorzaznaczony1;
    private string kolorzaznaczony2;
    private int licznik = 1;
    private const string AssetsBaseUri = "avares://CardGames/Assets/";

    public grawojna()
    {
        InitializeComponent();
    }

    private string kolorwybor(int number3)
    {
        return number3 switch
        {
            1 => "d",
            2 => "c",
            3 => "s",
            4 => "h",
            _ => "ni modo"
        };
    }

    private void generowanie()
    {
        textavatar.IsVisible = false;
        gracz1.IsVisible = true;
        bu1.IsVisible = true;

        var random = new Random();
        int number = random.Next(2, 15);
        int number2 = random.Next(2, 15);
        int number3 = random.Next(1, 5);
        int number4 = random.Next(1, 5);
        string color1 = kolorwybor(number3);
        string color2 = kolorwybor(number4);

        wylosowaneKartygracz1.Add(number);
        wylosowaneKartygracz2.Add(number2);

        var nowyButton = new Button
        {
            Background = Brushes.White,
            Name = color1,
            Tag = number,
            Content = $"{color1}{number}",
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
        };
        nowyButton.Click += (s, args) =>
        {
            kolorzaznaczony1 = nowyButton.Name;
            liczbazaznaczona1 = (int)nowyButton.Tag;
            czeker1.Content = "zaznaczono";
        };
        ListaKartgracz1.Children.Add(nowyButton);

        var nowyButton2 = new Button
        {
            Background = Brushes.White,
            Name = color2,
            Tag = number2,
            Content = $"{color2}{number2}",
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            IsVisible = false
        };
        nowyButton2.Click += (s, args) =>
        {
            kolorzaznaczony2 = nowyButton2.Name;
            liczbazaznaczona2 = (int)nowyButton2.Tag;
            czeker2.Content = "zaznaczono";
        };
        ListaKartgracz2.Children.Add(nowyButton2);
    }

    private void generowanieporundzieg1()
    {
        for (int i = 0; i < stockgracz1.Count; i++)
        {
            var nowyButton = new Button
            {
                Background = Brushes.White,
                Name = stockgraczkolor1[i],
                Tag = stockgracz1[i],
                Content = $"{stockgraczkolor1[i]}{stockgracz1[i]}",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };
            nowyButton.Click += (s, args) =>
            {
                liczbazaznaczona1 = Convert.ToInt32(nowyButton.Tag);
                kolorzaznaczony1 = nowyButton.Name;
                czeker1.Content = "zaznaczono";
            };
            ListaKartgracz1.Children.Add(nowyButton);
        }
        stockgracz1.Clear();
        stockgraczkolor1.Clear();
    }

    private void generowanieporundzieg2()
    {
        for (int i = 0; i < stockgracz2.Count; i++)
        {
            var nowyButton = new Button
            {
                Background = Brushes.White,
                Name = stockgraczkolor2[i],
                Tag = stockgracz2[i],
                Content = $"{stockgraczkolor2[i]}{stockgracz2[i]}",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };
            nowyButton.Click += (s, args) =>
            {
                liczbazaznaczona2 = Convert.ToInt32(nowyButton.Tag);
                kolorzaznaczony2 = nowyButton.Name;
                czeker2.Content = "zaznaczono";
            };
            ListaKartgracz2.Children.Add(nowyButton);
        }
        stockgracz2.Clear();
        stockgraczkolor2.Clear();
    }

    private void confirmg1_click(object sender, RoutedEventArgs e)
    {
        if (liczbazaznaczona1 > 0)
        {
            czekgracz1 = true;
            if (czekgracz1 && czekgracz2)
            {
                ProcessTurn();
            }

            if (ListaKartgracz1.Children.Count == 0)
            {
                generowanieporundzieg1();
            }

            if (ListaKartgracz2.Children.Count == 0)
            {
                generowanieporundzieg2();
            }

            UpdateUIForPlayer2();
            endofgamu();
            liczbazaznaczona2 = 0;
        }
    }

    private void confirmg2_click(object sender, RoutedEventArgs e)
    {
        if (liczbazaznaczona2 > 0)
        {
            czekgracz2 = true;
            if (czekgracz1 && czekgracz2)
            {
                ProcessTurn();
            }

            if (ListaKartgracz1.Children.Count == 0)
            {
                generowanieporundzieg1();
            }

            if (ListaKartgracz2.Children.Count == 0)
            {
                generowanieporundzieg2();
            }

            UpdateUIForPlayer1();
            endofgamu();
            liczbazaznaczona1 = 0;
        }
    }

    private void ProcessTurn()
    {
        czekgracz1 = false;
        czekgracz2 = false;
        czeker1.Content = "";
        czeker2.Content = "";
        
        tymstock.Add(liczbazaznaczona1);
        tymstock.Add(liczbazaznaczona2);
        tymstockkolor.Add(kolorzaznaczony1);
        tymstockkolor.Add(kolorzaznaczony2);

        if (liczbazaznaczona1 > liczbazaznaczona2)
        {
            AddToPlayer1Stock();
            remis.IsVisible = false;
        }
        else if (liczbazaznaczona1 < liczbazaznaczona2)
        {
            AddToPlayer2Stock();
            remis.IsVisible = false;
        }
        else
        {
            remis.IsVisible = true;
        }

        string tym1 = kolorzaznaczony1 + liczbazaznaczona1;
        string tym2 = kolorzaznaczony2 + liczbazaznaczona2;
        UsunButtong1(tym1);
        UsunButtong2(tym2);
    }

    private void AddToPlayer1Stock()
    {
        for (int i = 0; i < tymstock.Count; i++)
        {
            stockgracz1.Add(tymstock[i]);
            stockgraczkolor1.Add(tymstockkolor[i]);
        }
        tymstockkolor.Clear();
        tymstock.Clear();
    }

    private void AddToPlayer2Stock()
    {
        for (int i = 0; i < tymstock.Count; i++)
        {
            stockgracz2.Add(tymstock[i]);
            stockgraczkolor2.Add(tymstockkolor[i]);
        }
        tymstockkolor.Clear();
        tymstock.Clear();
    }

    private void UpdateUIForPlayer2()
    {
        foreach (var btn in ListaKartgracz2.Children)
        {
            btn.IsVisible = true;
        }

        foreach (var btn in ListaKartgracz1.Children)
        {
            btn.IsVisible = false;
        }

        gracz1.IsVisible = false;
        gracz2.IsVisible = true;
        bu2.IsVisible = true;
        bu1.IsVisible = false;
    }

    private void UpdateUIForPlayer1()
    {
        foreach (var btn in ListaKartgracz1.Children)
        {
            btn.IsVisible = true;
        }

        foreach (var btn in ListaKartgracz2.Children)
        {
            btn.IsVisible = false;
        }

        gracz1.IsVisible = true;
        gracz2.IsVisible = false;
        bu1.IsVisible = true;
        bu2.IsVisible = false;
    }

    private void UsunButtong1(string content)
    {
        foreach (var child in ListaKartgracz1.Children)
        {
            if (child is Button button && button.Content.ToString() == content)
            {
                ListaKartgracz1.Children.Remove(button);
                break;
            }
        }
    }

    private void UsunButtong2(string content)
    {
        foreach (var child in ListaKartgracz2.Children)
        {
            if (child is Button button && button.Content.ToString() == content)
            {
                ListaKartgracz2.Children.Remove(button);
                break;
            }
        }
    }

    private void endofgamu()
{
    //japierdole nawet to musialem poprawiac - Bigem
    bool player1Won = false;
    bool gameEnded = false;

    if (ListaKartgracz1.Children.Count == 0 && stockgracz1.Count == 0)
    {
        remis.IsVisible = false;
        wygrany.Content = "wygral gracz 2";
        player1Won = false;
        gameEnded = true;
    }
    else if (ListaKartgracz2.Children.Count == 0 && stockgracz2.Count == 0)
    {
        wygrany.Content = "wygral gracz 1";
        player1Won = true;
        gameEnded = true;
    }

    if (gameEnded)
    {
        ShowEndGameState();
        RecordGameResult(player1Won, MainWindow.Globals.username);
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
                    historyCmd.Parameters.AddWithValue("@game", "wojna");
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

    private void ShowEndGameState()
    {
        gracz2.IsVisible = true;
        gracz1.IsVisible = false;
        foreach (var btn in ListaKartgracz1.Children)
        {
            btn.IsVisible = false;
        }
        foreach (var btn in ListaKartgracz2.Children)
        {
            btn.IsVisible = false;
        }
        bu1.IsVisible = false;
        bu2.IsVisible = false;
    }

    private void avatar_wybor(object sender, RoutedEventArgs e)
    {
        int number = int.Parse((sender as Button).Tag.ToString());

        switch (number)
        {
            case 1:
                SetAvatarImage("foto.png", number);
                break;
            case 2:
                SetAvatarImage("photo.png", number);
                break;
            case 3:
                SetAvatarImage("av3.jpeg", number);
                break;
            case 4:
                SetAvatarImage("av4.jpg", number);
                break;
            case 5:
                SetAvatarImage("av7.png", number);
                break;
            case 6:
                SetAvatarImage("av6.png", number);
                break;
            case 7:
                licznik--;
                break;
        }
        licznik++;
    }

    private void SetAvatarImage(string imageName, int buttonNumber)
    {
        var imagePath = new Uri($"{AssetsBaseUri}{imageName}");
        var bitmap = new Bitmap(AssetLoader.Open(imagePath));

        if (licznik == 1)
        {
            gracz1.Source = bitmap;
            GetAvatarButton(buttonNumber).IsVisible = false;
        }
        else if (licznik == 2)
        {
            gracz2.Source = bitmap;
            avatary.IsVisible = false;
            for (int i = 0; i < 3; i++)
            {
                generowanie();
            }
            licznik = 3;
        }
    }

    private Button GetAvatarButton(int number)
    {
        return number switch
        {
            1 => av1,
            2 => av2,
            3 => av3,
            4 => av4,
            5 => av7,
            6 => av6,
            _ => null
        };
    }

    private void ZagrajDzwiek(string sciezka)
    {
        try
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "ffplay",
                    Arguments = $"-nodisp -autoexit -volume 30 \"{sciezka}\"",
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    UseShellExecute = true,
                    CreateNoWindow = true
                }
            };
            process.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd odtwarzania dźwięku: {ex.Message}");
        }
    }
}