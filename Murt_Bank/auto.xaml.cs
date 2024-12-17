using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Application = Microsoft.Office.Interop.Word.Application;
using Document = Microsoft.Office.Interop.Word.Document;

namespace Murt_Bank
{
    /// <summary>
    /// Логика взаимодействия для auto.xaml
    /// </summary>
    public partial class auto : System.Windows.Window
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

                        query = "SELECT IDUser FROM [dbo].[User] WHERE Login = @Username AND Password = @Password";
                        command.CommandText = query;  // Изменяем текст запроса на новый

                        object result = command.ExecuteScalar(); // Выполняем новый запрос

                        if (result != null)
                        {
                            Dohod.IDUser = Convert.ToInt32(result);
                            MessageBox.Show($"Login successful! {Dohod.IDUser}");
                        }
                        else
                        {
                            MessageBox.Show("Failed to retrieve user ID.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

            try
            {
                List<Dictionary<string, object>> data = GetDataFromDatabase(connectionString, Dohod.IDUser);

                if (data.Count > 0)
                {
                    string templatePath = @"C:\Users\Эмиль Муртазин\Desktop\Шаблон договора.docx";
                    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string outputPath = $"C:\\Users\\Эмиль Муртазин\\Desktop\\Договор_{data[0]["Account"]}.docx";

                    ProcessWordContract(data[0], templatePath, outputPath);
                }
                else
                {
                    MessageBox.Show("Пользователь не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private List<Dictionary<string, object>> GetDataFromDatabase(string connectionString, int userID)
        {
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string sqlQuery = @"SELECT u.Name, u.Surname, u.Patronymic, c.Amount, c.Period, c.ExpirationDate, c.Percet, c.Account,                                         u.Adress, u.[E-mail], u.Series, u.Number, u.DateOfIssue, u.Issued, u.DateOfBirth, u.PlaceOfBirth 
                                    FROM [dbo].[User] u 
                                    INNER JOIN [dbo].[Contract] c ON c.UserID = u.IDUser 
                                    WHERE u.IDUser = @UserID"; //Параметризованный запрос

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userID);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, object> row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row.Add(reader.GetName(i), reader[i]);
                            }
                            data.Add(row);
                        }
                    }
                }
            }
            return data;
        }
        private void ProcessWordContract(Dictionary<string, object> contractData, string templatePath, string outputPath)
        {
            try
            {
                Application wordApp = new Application();
                Document doc = wordApp.Documents.Open(templatePath);

                DateTime expirationDate = Convert.ToDateTime(contractData["ExpirationDate"]);
                DateTime dateOfBirth = Convert.ToDateTime(contractData["DateOfBirth"]);
                DateTime dateOfIssue = Convert.ToDateTime(contractData["DateOfIssue"]);

                ReplacePlaceholder(doc, "{{Номер договора}}", contractData["Account"]?.ToString());
                ReplacePlaceholder(doc, "{{день}}", expirationDate.Day.ToString());
                ReplacePlaceholder(doc, "{{месяц}}", expirationDate.ToString("MMMM", new CultureInfo("ru-RU")));
                ReplacePlaceholder(doc, "{{2018}}", expirationDate.Year.ToString());
                ReplacePlaceholder(doc, "{{ФИО вкладчика}}", $"{contractData["Name"]} {contractData["Surname"]} {contractData["Patronymic"]}");
                ReplacePlaceholder(doc, "{{Сумма вкла}}", contractData["Amount"]?.ToString());
                ReplacePlaceholder(doc, "{{Срок вкла}}", contractData["Period"]?.ToString());
                ReplacePlaceholder(doc, "{{Дата окончания срока вкла}}", expirationDate.ToShortDateString());
                ReplacePlaceholder(doc, "{{Процентная ставка по вкла}}", $"{contractData["Percet"]}%");
                ReplacePlaceholder(doc, "{{Номер счета вк}}", contractData["Account"]?.ToString());
                ReplacePlaceholder(doc, "{{ФИО вклад}}", $"{contractData["Name"]} {contractData["Surname"]} {contractData["Patronymic"]}");
                ReplacePlaceholder(doc, "{{Адрес регистра}}", contractData["Adress"]?.ToString());
                ReplacePlaceholder(doc, "{{Адрес электронной по}}", contractData["E-mail"]?.ToString());
                ReplacePlaceholder(doc, "{{Серия}}", contractData["Series"]?.ToString());
                ReplacePlaceholder(doc, "{{Номер}}", contractData["Number"]?.ToString());
                ReplacePlaceholder(doc, "{{Кем и когда выдан}}", $"{contractData["Issued"]} {dateOfIssue.ToShortDateString()}");
                ReplacePlaceholder(doc, "{{Дата рождени}}", dateOfBirth.ToShortDateString());
                ReplacePlaceholder(doc, "{{Место рожден}}", contractData["PlaceOfBirth"]?.ToString());

                doc.SaveAs2(outputPath);
                wordApp.Visible = true;
                wordApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(wordApp);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при генерации договора: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ReplacePlaceholder(Document doc, string placeholder, string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            Find find = doc.Content.Find;
            find.Text = placeholder;
            find.Replacement.Text = value;
            find.Execute(Replace: WdReplace.wdReplaceAll);
        }

        public bool logbool;
        public bool pasbool;
        private void TbLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            if (logbool == false)
            {
                logbool = !logbool;
                tbLogin.Foreground = Brushes.Black;
                tbLogin.Text = "";
            }
        }

        private void TbPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (pasbool == false)
            {
                pasbool = !pasbool;
                tbPassword.Foreground = Brushes.Black;
                tbPassword.Text = "";
            }
        }
    }
}
