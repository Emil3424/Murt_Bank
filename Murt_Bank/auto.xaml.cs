using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Murt_Bank
{
    /// <summary>
    /// Логика взаимодействия для auto.xaml
    /// </summary>
    public partial class auto : Window
    {
        public auto()
        {
            InitializeComponent();
            tbLogin.Foreground = Brushes.Gray;
            tbLogin.Text = "Введите логин";
            tbPassword.Foreground = Brushes.Gray;
            tbPassword.Text = "Введите пароль";
            logbool = false;
            pasbool = false;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string enteredUsername = tbLogin.Text;
            string enteredPassword = tbPassword.Text;

            // Строка подключения к базе данных SQL Server
            string connectionString = "Server=45-14;Database=Olimp_Zad2_MurtBank;User Id=sa;Password=1234;";

            // SQL запрос для проверки пользователя
            string query = "SELECT COUNT(1) FROM [dbo].[User] WHERE Login = @Username AND Password = @Password";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Открытие соединения с базой данных
                    connection.Open();

                    // Создание команды с параметрами
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", enteredUsername);
                        command.Parameters.AddWithValue("@Password", enteredPassword); // В реальном приложении пароль должен быть хеширован

                        // Выполнение запроса и получение результата
                        int userCount = Convert.ToInt32(command.ExecuteScalar());

                        // Если найден пользователь с таким логином и паролем
                        if (userCount == 1)
                        {
                            MessageBox.Show("Login successful!");
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        public bool logbool;
        public bool pasbool;
        private void TbLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            if(logbool == false)
            {
                logbool = !logbool;
                tbLogin.Foreground = Brushes.Black;
                tbLogin.Text = "";
            }
        }

        //private void TbLogin_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    if(tbLogin.Text == null)
        //    {
        //        tbLogin.Foreground = Brushes.Gray;
        //        tbLogin.Text = "Введите логин";
        //    }
        //}

        //private void TbPassword_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    if(tbPassword != null)
        //    {
        //        tbpass.Foreground = Brushes.Gray;
        //        tbpass.Text = "Введите пароль";
        //    }
        //}

        private void TbPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if(pasbool == false)
            {
                pasbool = !pasbool;
                tbPassword.Foreground = Brushes.Black;
                tbPassword.Text = "";
            }
        }
    }
}
