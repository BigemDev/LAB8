using System;
using System.Collections.Generic;
using Avalonia;
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
        for (int i = 0; i < 5; i++)
        {
            generowanie();
        }
    }
    private void generowanie()
    {
        var random = new Random();
        int number = random.Next(2, 15);
        int number2 = random.Next(2, 15);
       // Karty.Content = $"{number}";
       wylosowaneKartygracz1.Add(number);
       wylosowaneKartygracz2.Add(number2);
       var nowyButton = new Button
       {
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
           Content = $"{number2}",
           HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
       };
       nowyButton2.Click += (s, args) =>
       {
           liczbazaznaczona2 = int.Parse((string)nowyButton2.Content);
           czeker2.Content = $"zaznaczono";
       };
       ListaKartgracz2.Children.Add(nowyButton2);
       
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
                
            }
            if (liczbazaznaczona1<liczbazaznaczona2)
            {
                for (int i = 0; i < tymstock.Count; i++)
                {
                    stockgracz2.Add(tymstock[i]);
                }
                
            }
            string tym1=liczbazaznaczona1.ToString();
            string tym2=liczbazaznaczona2.ToString();
            UsunButtong1(tym1);
            UsunButtong2(tym2);
            
        }
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
}