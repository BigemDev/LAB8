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
    private bool czy;
    public ObservableCollection<poczekalnia.karta> wylosowane { get; set; } = new ObservableCollection<poczekalnia.karta>();
    public balatro(poczekalnia p) {
        InitializeComponent();
        baza = p;
        DataContext = this;
    }

    public void odrzucenie(object sender, RoutedEventArgs e) {
        Random rnd = new Random();
        for (int i = 0; i < wylosowane.Count; i++) {
            if (wylosowane[i].czy_wybrana) {
                wylosowane.RemoveAt(i);
                wylosowane.Add(baza.wylosuj(rnd));
            }
        }
    }

    public void zagranie(object sender, RoutedEventArgs e)
    {
        
    }
}