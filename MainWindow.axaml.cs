using Avalonia.Controls;
using Avalonia.Interactivity;

namespace CardGames;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    private void StartGameBigem_Click(object? sender, RoutedEventArgs e)
    {
        
    }
    
    
    private void Exit_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}