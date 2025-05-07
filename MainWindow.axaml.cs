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
    
    
    private void Exit_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}