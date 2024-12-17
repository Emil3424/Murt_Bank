using System.Windows;

namespace Murt_Bank
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Calculate calculate = new Calculate();
            calculate.Show();
            this.Close();
        }
    }
}
