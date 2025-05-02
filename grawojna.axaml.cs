using System;
using System.Media;
using System.Diagnostics;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;

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
    bool czekgracz1 = false;
    bool czekgracz2 = false;
    int liczbazaznaczona1;
    int liczbazaznaczona2;
    string kolorzaznaczony1;
    string kolorzaznaczony2;
    public grawojna()
    {
        InitializeComponent();
       
        

        for (int i = 0; i < 3; i++)
        {
            generowanie();
        }
    }

    private string kolorwybor(int number3)
    {
        switch (number3)
        {
            case 1:
                return "d";
            case 2:
                return "c";
            case 3:
                return "s";
            case 4:
                return "h";
            default:
                return "ni modo";
        }
    }
    private void generowanie()
    {
        ZagrajDzwiek("select.mp3");
        var random = new Random();
        int number = random.Next(2, 15);
        int number2 = random.Next(2, 15);
        int number3 = random.Next(1, 5);
        int number4 = random.Next(1, 5);
        string color1;
        string color2;
        color1 = kolorwybor(number3);
        color2 = kolorwybor(number4);
       // Karty.Content = $"{number}";
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
           czeker1.Content = $"zaznaczono";
       };
       ListaKartgracz1.Children.Add(nowyButton);
       var nowyButton2 = new Button
       {
           Background = Brushes.White,
           Name = color2,
           Tag = number2,
           Content = $"{color2}{number2}",
           HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
       };
       nowyButton2.IsVisible = false;
       nowyButton2.Click += (s, args) =>
       {
           kolorzaznaczony2 = nowyButton2.Name;
           liczbazaznaczona2 = (int)nowyButton2.Tag;
           czeker2.Content = $"zaznaczono";
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
                czeker1.Content = $"zaznaczono";
            };
            ListaKartgracz1.Children.Add(nowyButton);
        }
        stockgracz1.Clear();
        stockgraczkolor1.Clear();
        if ((ListaKartgracz2.Children.Count!=0||stockgracz2.Count!=0)&&(ListaKartgracz1.Children.Count!=0||stockgracz1.Count!=0))
        {
            ZagrajDzwiek("skibidi.wav");
        }
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
                czeker2.Content = $"zaznaczono";
            };
            ListaKartgracz2.Children.Add(nowyButton);
        }
        stockgracz2.Clear();
        stockgraczkolor2.Clear();
        if ((ListaKartgracz2.Children.Count!=0||stockgracz2.Count!=0)&&(ListaKartgracz1.Children.Count!=0||stockgracz1.Count!=0))
        {
            ZagrajDzwiek("skibidi.wav");
        }
        
       
    }

    private void confirmg1_click(object sender, RoutedEventArgs e)
    {
        
        
        if (liczbazaznaczona1 > 0)
        {



            czekgracz1 = true;
            if (czekgracz1 == true && czekgracz2 == true)
            {
                czekgracz1 = false;
                czekgracz2 = false;
                czeker1.Content = $"";
                czeker2.Content = $"";
                tymstock.Add(liczbazaznaczona1);
                tymstock.Add(liczbazaznaczona2);
                tymstockkolor.Add(kolorzaznaczony1);
                tymstockkolor.Add(kolorzaznaczony2);
                if (liczbazaznaczona1 > liczbazaznaczona2)
                {
                    for (int i = 0; i < tymstock.Count; i++)
                    {
                        stockgracz1.Add(tymstock[i]);
                        stockgraczkolor1.Add(tymstockkolor[i]);
                    }

                    tymstockkolor.Clear();
                    tymstock.Clear();
                    remis.IsVisible = false;

                }

                if (liczbazaznaczona1 < liczbazaznaczona2)
                {
                    for (int i = 0; i < tymstock.Count; i++)
                    {
                        stockgracz2.Add(tymstock[i]);
                        stockgraczkolor2.Add(tymstockkolor[i]);
                    }

                    tymstockkolor.Clear();
                    tymstock.Clear();
                    remis.IsVisible = false;

                }

                if (liczbazaznaczona1 == liczbazaznaczona2)
                {
                    remis.IsVisible = true;
                }

                string tym1 = liczbazaznaczona1.ToString();
                string tym2 = liczbazaznaczona2.ToString();
                UsunButtong1(tym1);
                UsunButtong2(tym2);

            }

            if (ListaKartgracz1.Children.Count == 0)
            {
                generowanieporundzieg1();
            }

            if (ListaKartgracz2.Children.Count == 0)
            {
                generowanieporundzieg2();
            }

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
            endofgamu();
            liczbazaznaczona2 = 0;
            if ((ListaKartgracz2.Children.Count != 0 || stockgracz2.Count != 0) &&
                (ListaKartgracz1.Children.Count != 0 || stockgracz1.Count != 0))
            {
                ZagrajDzwiek("button.mp3");
            }
        }
    }

    private void confirmg2_click(object sender, RoutedEventArgs e)
    {
        
        if (liczbazaznaczona2 > 0)
        {


            czekgracz2 = true;
            if (czekgracz1 == true && czekgracz2 == true)
            {
                czekgracz1 = false;
                czekgracz2 = false;
                czeker1.Content = $"";
                czeker2.Content = $"";
                tymstock.Add(liczbazaznaczona1);
                tymstock.Add(liczbazaznaczona2);
                tymstockkolor.Add(kolorzaznaczony1);
                tymstockkolor.Add(kolorzaznaczony2);
                if (liczbazaznaczona1 > liczbazaznaczona2)
                {
                    for (int i = 0; i < tymstock.Count; i++)
                    {
                        stockgracz1.Add(tymstock[i]);
                        stockgraczkolor1.Add(tymstockkolor[i]);
                    }

                    tymstockkolor.Clear();
                    tymstock.Clear();
                    remis.IsVisible = false;

                }

                if (liczbazaznaczona1 < liczbazaznaczona2)
                {
                    for (int i = 0; i < tymstock.Count; i++)
                    {
                        stockgracz2.Add(tymstock[i]);
                        stockgraczkolor2.Add(tymstockkolor[i]);
                    }

                    tymstockkolor.Clear();
                    tymstock.Clear();
                    remis.IsVisible = false;

                }

                if (liczbazaznaczona1 == liczbazaznaczona2)
                {
                    remis.IsVisible = true;
                }

                string tym1 = kolorzaznaczony1 + liczbazaznaczona1;
                string tym2 = kolorzaznaczony2 + liczbazaznaczona2;
                UsunButtong1(tym1);
                UsunButtong2(tym2);

            }

            if (ListaKartgracz1.Children.Count == 0)
            {
                generowanieporundzieg1();
            }

            if (ListaKartgracz2.Children.Count == 0)
            {
                generowanieporundzieg2();
            }

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
            endofgamu();
            liczbazaznaczona1 = 0;
            if ((ListaKartgracz2.Children.Count != 0 || stockgracz2.Count != 0) &&
                (ListaKartgracz1.Children.Count != 0 || stockgracz1.Count != 0))
            {
                ZagrajDzwiek("button.mp3");
            }
        }
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

    private void tru()
    {
        var random = new Random();
        int number = random.Next(1, 6);
        switch (number)
        {
            case 1:
                ZagrajDzwiek("tru1.mp3");
                break;
            case 2:
                ZagrajDzwiek("tru2.mp3");
                break;
            case 3:
                ZagrajDzwiek("tru3.mp3");
                break;
            case 4:
                ZagrajDzwiek("tru4.mp3");
                break;
            case 5:
                ZagrajDzwiek("tru5.mp3");
                break;
            default:
                break;
        }
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

    private void endofgamu()
    {
        
        if (ListaKartgracz1.Children.Count==0&&stockgracz1.Count==0)
        {
            remis.IsVisible = false;
            ZagrajDzwiek("wygrana.mp3");
            wygrany.Content = "wygral gracz 2";
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
        if (ListaKartgracz2.Children.Count==0&&stockgracz2.Count==0)
        {
            ZagrajDzwiek("wygrana.mp3");
            wygrany.Content = "wygral gracz 1";
            gracz1.IsVisible = true;
            gracz2.IsVisible = false;
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
        
       
    }
    
    

}