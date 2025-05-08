using Avalonia.Controls;
using Avalonia.Interactivity;

namespace CardGames;

public partial class MainWindow : Window
{
    public static class Globals
    {
        public static string username;
        public static string password;
    }
    public MainWindow()
    {
        InitializeComponent();
    }
    private void StartGameBigem_Click(object? sender, RoutedEventArgs e)
    {
        GameWindowBigem game = new GameWindowBigem();
        game.Show();
    }
    private void StartGameHoda_Click(object? sender, RoutedEventArgs e) {
        poczekalnia p = new poczekalnia();
        p.Show();
    }
    private void Startwojna_Click(object? sender, RoutedEventArgs e)
    {
        var newWindow = new grawojna(); 
        newWindow.Show();
        this.Close();
    }

    private void ShowGameHistory_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(Globals.username))
        {
            return;
        }

        var historyWindow = new GameHistoryWindow();
        historyWindow.Show();
    }
    
    
    private void Exit_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}