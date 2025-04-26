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
    private void Startwojna_Click(object? sender, RoutedEventArgs e)
    {
        var newWindow = new grawojna(); 
        newWindow.Show();
        this.Close();
    }
    
    
    private void Exit_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}