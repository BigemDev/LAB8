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
    public int ID = 0;
    public balatro nastepna;
    private int nowa;
    private int ile_dodac = 0;
    private int liczba_odrzucen = 4;
    private int liczba_zagran = 4;
    private double wynik = 0;
    private int liczba_zagranych = 0;
    public ObservableCollection<poczekalnia.karta> do_uzycia { get; set; } = new ObservableCollection<poczekalnia.karta>();
    public ObservableCollection<poczekalnia.karta> wylosowane { get; set; } = new ObservableCollection<poczekalnia.karta>();
    public ObservableCollection<poczekalnia.karta> zagrane { get; set; } = new ObservableCollection<poczekalnia.karta>();
    public int[] wartosci;

    public balatro(poczekalnia p) {
        InitializeComponent();
        baza = p;
        DataContext = this;
        wartosci = new int[] {21, 28, 35, 42, 49};
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
            od.Text = "Wykorzystano wszystkie odrzucenia";
        }
    }

    public void zagranie(object sender, RoutedEventArgs e) {
        for (int i = zagrane.Count() - 1; i >= 0; i--) {  
            zagrane.RemoveAt(i);
        }
        for (int i = wylosowane.Count() - 1; i >= 0; i--) {
            if (wylosowane[i].czy_wybrana) {
                liczba_zagranych += 1;
            }
        }
        if (liczba_zagranych <= 5) {
            if (liczba_zagran != 100) {
                Random rnd = new Random();
                Console.WriteLine(wylosowane.Count());
                for (int i = wylosowane.Count() - 1; i >= 0; i--) {
                    Console.WriteLine($"Czy wybrana: {wylosowane[i].czy_wybrana}");
                    if (wylosowane[i].czy_wybrana == true) {
                        zagrane.Add(wylosowane[i]);
                        wynik += wylosowane[i].wartosc;
                        Console.WriteLine($"poszło {wylosowane[i].rank}");
                        wylosowane.RemoveAt(i);
                        ile_dodac += 1;
                        Console.WriteLine($"zagrane {zagrane.Count}");
                    }
                }
                Console.WriteLine($"zagrane {zagrane.Count}");

                for (int i = zagrane.Count() - 1; i >= 0; i--) {
                    wartosci[i] = Convert.ToInt32(zagrane[i].kolejnosc);
                    Console.WriteLine(wartosci[i]);
                }

                for (int i = 0; i < ile_dodac; i++) {
                    Console.WriteLine("losuję");
                    nowa = rnd.Next(0, do_uzycia.Count);
                    wylosowane.Add(do_uzycia[nowa]);
                    do_uzycia.RemoveAt(nowa);
                }

                var sort = wartosci.OrderBy(k => k).ToArray();

                if (//royal flush
                    sort[0] == 10
                    && sort[1] == 11
                    && sort[2] == 12
                    && sort[3] == 13
                    && sort[4] == 14
                    &&  zagrane.All(k => k.suit == zagrane[0].suit))
                {
                    wynik += 700;
                }
                else if (//straight flush
                         sort[0] == sort[1] - 1
                         && sort[1] == sort[2] - 1
                         && sort[2] == sort[3] - 1
                         && sort[3] == sort[4] - 1 
                         // && zagrane[0].suit == zagrane[1].suit && zagrane[0].suit == zagrane[2].suit && zagrane[0].suit == zagrane[3].suit && zagrane[0].suit == zagrane[4].suit)
                ){
                    wynik += 600;
                }
                else if (//four of a kind
                         (sort[0] == sort[1] || sort[3] == sort[4])
                         && sort[1] == sort[2]
                         && sort[2] == sort[3])
                {
                    wynik += 500;
                }
                else if (//full house
                         sort[0] == sort[1]
                         && sort[3] == sort[4]
                         && (sort[1] == sort[2]) || (sort[2] == sort[3])) {
                    wynik += 400;
                }
                else if (//flush
                         zagrane.All(k => k.suit == zagrane[0].suit))
                {
                    wynik += 300;
                }
                else if (//straight
                         sort[0] == sort[1] - 1
                         && sort[1] == sort[2] - 1
                         && sort[2] == sort[3] - 1
                         && sort[3] == sort[4] - 1) {
                    wynik += 200;
                }
                else if (//tree of a kind
                         ((sort[0] == sort[1]) && (sort[1] == sort[2]) 
                          || (sort[1] == sort[2]) && (sort[2] == sort[3]) 
                          || (sort[2] == sort[3]) && (sort[3] == sort[4])))
                {
                    wynik += 100;
                }
                else if (//two pair
                         (sort[0] == sort[1]) && ((sort[2] == sort[3]) || (sort[3] == sort[4])) || (sort[1] == sort[2] && sort[3] == sort[4])) {
                    wynik += 70;
                }
                else if (//pair
                         ((sort[0] == sort[1]) 
                          || (sort[1] == sort[2]) 
                          || (sort[2] == sort[3]) 
                          || (sort[3] == sort[4]) )) {
                    wynik += 30;
                }

                /*---------------*/
                ile_dodac = 0;
                liczba_zagran -= 1;
                liczba_zagranych = 0;
                // wyswietlanie_wyniku.Text = Convert.ToString(wynik);
                for (int i = 0; i < 5; i++) {
                    wartosci[i] = ( i + 3 ) * 7;
                }
            }
            else {
                zag.Text = "Wykorzystano wszystkie zagrania!";
            }
        }
        else {
            zag.Text = "Za dużo zagranych kart!";
        }
    }

    public void next(object sender, RoutedEventArgs e) {
        this.Hide();
        nastepna.Show();
    }
}