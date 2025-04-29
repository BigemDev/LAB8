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
    private List<int> tymstock = new List<int>();
    bool czekgracz1 = false;
    bool czekgracz2 = false;
    int liczbazaznaczona1;
    int liczbazaznaczona2;
    public grawojna()
    {
        InitializeComponent();
       
        

        for (int i = 0; i < 3; i++)
        {
            generowanie();
        }
    }
    private void generowanie()
    {
        ZagrajDzwiek("select.mp3");
        var random = new Random();
        int number = random.Next(2, 15);
        int number2 = random.Next(2, 15);
       // Karty.Content = $"{number}";
       wylosowaneKartygracz1.Add(number);
       wylosowaneKartygracz2.Add(number2);
       var nowyButton = new Button
       {
           Background = Brushes.White,
           Content = $"{number}",
           HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
       };
       nowyButton.Click += (s, args) =>
       {
           liczbazaznaczona1 = int.Parse((string)nowyButton.Content);
           czeker1.Content = $"zaznaczono";
       };
       ListaKartgracz1.Children.Add(nowyButton);
       var nowyButton2 = new Button
       {
           Background = Brushes.White,
           Content = $"{number2}",
           HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
       };
       nowyButton2.IsVisible = false;
       nowyButton2.Click += (s, args) =>
       {
           liczbazaznaczona2 = int.Parse((string)nowyButton2.Content);
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
                Content = $"{stockgracz1[i]}",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };
            nowyButton.Click += (s, args) =>
            {
                liczbazaznaczona1 = int.Parse((string)nowyButton.Content);
                czeker1.Content = $"zaznaczono";
            };
            ListaKartgracz1.Children.Add(nowyButton);
        }
        stockgracz1.Clear();
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
                Content = $"{stockgracz2[i]}",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };
            nowyButton.Click += (s, args) =>
            {
                liczbazaznaczona2 = int.Parse((string)nowyButton.Content);
                czeker2.Content = $"zaznaczono";
            };
            ListaKartgracz2.Children.Add(nowyButton);
        }
        stockgracz2.Clear();
        if ((ListaKartgracz2.Children.Count!=0||stockgracz2.Count!=0)&&(ListaKartgracz1.Children.Count!=0||stockgracz1.Count!=0))
        {
            ZagrajDzwiek("skibidi.wav");
        }
        
       
    }
    private void confirmg1_click(object sender, RoutedEventArgs e)
    {
        czekgracz1 = true;
        if (czekgracz1==true&&czekgracz2==true)
        {
            czekgracz1 = false;
            czekgracz2 = false;
            czeker1.Content = $"";
            czeker2.Content = $"";
            tymstock.Add(liczbazaznaczona1);
            tymstock.Add(liczbazaznaczona2);
            if (liczbazaznaczona1>liczbazaznaczona2)
            {
                for (int i = 0; i < tymstock.Count; i++)
                {
                    stockgracz1.Add(tymstock[i]);
                }
                tymstock.Clear();
                
            }
            if (liczbazaznaczona1<liczbazaznaczona2)
            {
                for (int i = 0; i < tymstock.Count; i++)
                {
                    stockgracz2.Add(tymstock[i]);
                }
                tymstock.Clear();
                
            }
            string tym1=liczbazaznaczona1.ToString();
            string tym2=liczbazaznaczona2.ToString();
            UsunButtong1(tym1);
            UsunButtong2(tym2);
            
        }

        if (ListaKartgracz1.Children.Count==0)
        {
            generowanieporundzieg1();
        }
        if (ListaKartgracz2.Children.Count==0)
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
        endofgamu();
    }
    private void confirmg2_click(object sender, RoutedEventArgs e)
    {
        czekgracz2 = true;
        if (czekgracz1==true&&czekgracz2==true)
        {
            czekgracz1 = false;
            czekgracz2 = false;
            czeker1.Content = $"";
            czeker2.Content = $"";
            tymstock.Add(liczbazaznaczona1);
            tymstock.Add(liczbazaznaczona2);
            if (liczbazaznaczona1>liczbazaznaczona2)
            {
                for (int i = 0; i < tymstock.Count; i++)
                {
                    stockgracz1.Add(tymstock[i]);
                }
                tymstock.Clear();
                
            }
            if (liczbazaznaczona1<liczbazaznaczona2)
            {
                for (int i = 0; i < tymstock.Count; i++)
                {
                    stockgracz2.Add(tymstock[i]);
                }
                tymstock.Clear();
                
            }
            string tym1=liczbazaznaczona1.ToString();
            string tym2=liczbazaznaczona2.ToString();
            UsunButtong1(tym1);
            UsunButtong2(tym2);
            
        }

        if (ListaKartgracz1.Children.Count==0)
        {
            generowanieporundzieg1();
        }
        if (ListaKartgracz2.Children.Count==0)
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
        endofgamu();
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

    private void ZagrajDzwiek(string sciezka)
    {
        try
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "ffplay",
                    Arguments = $"-nodisp -autoexit \"{sciezka}\"",
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