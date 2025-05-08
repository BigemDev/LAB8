using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Layout;
using MySqlConnector;

namespace CardGames
{
    public partial class GameHistoryWindow : Window
    {
        public GameHistoryWindow()
        {
            InitializeComponent();
            LoadGameHistory();
        }

        private void LoadGameHistory()
        {
            string connStr = "server=127.0.0.1;port=3306;user=appuser;password=apppassword;database=login_demo;";
            
            try
            {
                using var conn = new MySqlConnection(connStr);
                conn.Open();
                
                string sql = @"
                    SELECT gh.game, gh.won, gh.played_at 
                    FROM game_history gh
                    JOIN users u ON gh.user_id = u.id
                    WHERE u.username = @username
                    ORDER BY gh.played_at DESC";
                
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@username", MainWindow.Globals.username);
                
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    bool won = reader.GetBoolean("won");
                    string game = reader.GetString("game");
                    string playedAt = reader.GetDateTime("played_at").ToString("yyyy-MM-dd HH:mm");
                    
                    var recordPanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Spacing = 10,
                        Margin = new Thickness(0, 5)
                    };
                    
                    recordPanel.Children.Add(new TextBlock 
                    { 
                        Text = game,
                        Width = 100,
                        FontWeight = FontWeight.Bold
                    });
                    
                    recordPanel.Children.Add(new TextBlock 
                    { 
                        Text = won ? "Won" : "Lost",
                        Foreground = won ? Brushes.Green : Brushes.Red,
                        Width = 60
                    });
                    
                    recordPanel.Children.Add(new TextBlock 
                    { 
                        Text = playedAt,
                        Width = 150
                    });
                    
                    HistoryContainer.Children.Add(recordPanel);
                }
            }
            catch (Exception ex)
            {
                HistoryContainer.Children.Add(new TextBlock 
                { 
                    Text = "Failed to load game history",
                    Foreground = Brushes.Red
                });
                Console.WriteLine($"Error loading game history: {ex.Message}");
            }
            
            if (HistoryContainer.Children.Count == 1) // Only the title exists
            {
                HistoryContainer.Children.Add(new TextBlock 
                { 
                    Text = "No game history found",
                    FontStyle = FontStyle.Italic
                });
            }
        }
    }
}