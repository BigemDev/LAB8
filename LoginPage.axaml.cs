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

            string hashedPassword = HashPassword(password);
            string sql = "SELECT COUNT(*) FROM users WHERE username = @username AND password_hash = @password_hash";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password_hash", hashedPassword);

            int userCount = Convert.ToInt32(cmd.ExecuteScalar());
            return userCount > 0;
        }

        private bool RegisterUser(string username, string password)
        {
            string connStr = "server=127.0.0.1;port=3306;user=appuser;password=apppassword;database=login_demo;";
            using var conn = new MySqlConnection(connStr);
            conn.Open();
            
            string checkSql = "SELECT COUNT(*) FROM users WHERE username = @username";
            using var checkCmd = new MySqlCommand(checkSql, conn);
            checkCmd.Parameters.AddWithValue("@username", username);
            
            if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0)
            {
                Console.Write("Username already exists");
                return false;
            }

            string hashedPassword = HashPassword(password);
            string insertSql = "INSERT INTO users (username, password_hash) VALUES (@username, @password_hash)";
            using var insertCmd = new MySqlCommand(insertSql, conn);
            insertCmd.Parameters.AddWithValue("@username", username);
            insertCmd.Parameters.AddWithValue("@password_hash", hashedPassword);

            int rowsAffected = insertCmd.ExecuteNonQuery();
            return rowsAffected > 0;
        }

        private string HashPassword(string password)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Globals.username = UsernameTextBox.Text;
            MainWindow.Globals.password = PasswordBox.Text;

            if (ValidateCredentials(MainWindow.Globals.username, MainWindow.Globals.password))
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

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Console.Write("Username and password cannot be empty");
                return;
            }

            if (RegisterUser(username, password))
            {
                Console.Write("Registration successful! Please login.");
            }
            else
            {
                Console.Write("Registration failed. Username may already exist.");
            }
        }
    }
}