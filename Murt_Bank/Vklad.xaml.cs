using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для Vklad.xaml
    /// </summary>
    public partial class Vklad : Window
    {
        public Vklad()
        {
            InitializeComponent();
            stabdohod.Content  = Dohod.stabdohod;
            optimdohod.Content = Dohod.optimdohod;
            standartdohod.Content= Dohod.standartdohod;

            string stabdohodi = Dohod.stabdohod.Remove(Dohod.stabdohod.IndexOf(" "), 5);
            stabkonec.Content = Convert.ToInt32(stabdohodi) + Convert.ToInt32(Dohod.stabsumma) + " Руб.";
            stabdohodi = Dohod.optimdohod.Remove(Dohod.optimdohod.IndexOf(" "), 5);
            optimkonec.Content = Convert.ToInt32(stabdohodi) + Convert.ToInt32(Dohod.stabsumma) + " Руб.";
            stabdohodi = Dohod.standartdohod.Remove(Dohod.standartdohod.IndexOf(" "), 5);
            standartkonec.Content = Convert.ToInt32(stabdohodi) + Convert.ToInt32(Dohod.stabsumma) + " Руб.";
        }

        private void vklad1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void vklad2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void vklad_Click(object sender, RoutedEventArgs e)
        {
            auto auto = new auto();
            auto.Show();
            this.Close();
        }
    }
}
