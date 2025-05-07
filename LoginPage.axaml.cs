using Avalonia.Controls;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;
using System;
using System.Security.Cryptography;
using System.Text;

namespace CardGames
{
    public partial class LoginPage : Window
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        private bool ValidateCredentials(string username, string password)
        {
            string connStr = "server=127.0.0.1;port=3306;user=appuser;password=apppassword;database=login_demo;";
            using var conn = new MySqlConnection(connStr);
            conn.Open();

            string hashedPassword = password;
            Console.Write(hashedPassword);
            string sql = "SELECT COUNT(*) FROM users WHERE username = @username AND password_hash = @password_hash";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password_hash", hashedPassword);

            int userCount = Convert.ToInt32(cmd.ExecuteScalar());
            return userCount > 0;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Text; 

            if (ValidateCredentials(username, password))
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                Console.Write("Invalid username or password");
            }
        }
    }
}
