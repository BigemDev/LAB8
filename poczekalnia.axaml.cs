using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace CardGames;

public partial class poczekalnia : Window
{
    public poczekalnia() {
        InitializeComponent();
    }

    public void start(object sender, RoutedEventArgs e) {
        balatro gra = new balatro();
        gra.Show();
    }
}