using System;
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
    public balatro(poczekalnia p) {
        InitializeComponent();
        baza = p;
        DataContext = this;
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
        if (liczba_zagran != 0) {
            Random rnd = new Random();
            for (int i = wylosowane.Count - 1; i >= 0; i--) {
                if (wylosowane[i].czy_wybrana) {
                    zagrane.Add(wylosowane[i]);
                    wynik += wylosowane[i].wartosc;
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
            wyswietlanie_wyniku.Text = Convert.ToString(wynik);
        }
    }
}