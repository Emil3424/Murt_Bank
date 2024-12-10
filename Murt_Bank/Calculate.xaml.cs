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
    /// Логика взаимодействия для Calculate.xaml
    /// </summary>
    public partial class Calculate : Window
    {
        public Calculate()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            //Получить начальное значение слайдера
            double slidersumma = Summasl.Value * 0.076;
            stabntb.Text = slidersumma.ToString() + " Руб.";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Dohod.stabdohod = stabntb.Text;
            Dohod.optimdohod = Optimtb.Text;
            Dohod.standartdohod = Standarttb.Text;
            Dohod.stabsumma = tbstab.Text;
            Vklad vklad = new Vklad();
            vklad.Show();
            this.Close();
        }
        public static decimal CalculateInterest(decimal principal, int days, decimal monthlyDeposit, string planType)
        {
            decimal annualInterestRate = 0;
            bool capitalize = false;
            bool monthdep = true;

            decimal totalInterest = 0;
            decimal currentBalance = principal;

            int months = (days + 30) / 30; // приблизительное количество месяцев
            switch (planType)
            {
                case "Stable":
                    annualInterestRate = 0.068m; // 8%
                    monthdep = false;
                    if(months < 3) months = 3;
                    break;
                case "Optimal":
                    annualInterestRate = 0.0562m; // 5%
                    capitalize = true;
                    if (months < 6) months = 6;
                    break;
                case "Standard":
                    annualInterestRate = 0.0605m; // 6%
                    if (months < 3) months = 3;
                    break;
            }

            decimal monthlyRate = annualInterestRate / 12;

            for (int i = 0; i < months; i++)
            {
                decimal interestEarned = currentBalance * monthlyRate;
                totalInterest += interestEarned;

                currentBalance += interestEarned; // капитализация, если включена
                if (i < months - 1 && monthdep == true) currentBalance += monthlyDeposit; //Пополнение в конце месяца

            }
            if (capitalize == true)
            {
                totalInterest = currentBalance - principal - (monthlyDeposit * (months - 1));
            }


            return totalInterest;
        }

        private void Summasl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //if (Summasl != null)
            //{
            //    double slidersumma = Summasl.Value * 0.076;
            //    stabntb.Text = slidersumma.ToString() + " Руб.";
            //}
            if (stabntb != null)
            {
                tbstab.Text = e.NewValue.ToString();
                string tip = "Stable";
                stabntb.Text = Math.Round(CalculateInterest(Convert.ToDecimal(e.NewValue), Convert.ToInt32(tbdays.Text),Convert.ToDecimal(tbpopol.Text), tip)) + " Руб.";
                tip = "Optimal";
                Optimtb.Text = Math.Round(CalculateInterest(Convert.ToDecimal(e.NewValue), Convert.ToInt32(tbdays.Text), Convert.ToDecimal(tbpopol.Text), tip)) + " Руб.";
                tip = "Standard";
                Standarttb.Text = Math.Round(CalculateInterest(Convert.ToDecimal(e.NewValue), Convert.ToInt32(tbdays.Text), Convert.ToDecimal(tbpopol.Text), tip)) + " Руб.";
            }
            else System.Diagnostics.Debug.WriteLine("myTextBlock is null!");

        }

        private void days_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (stabntb != null)
            {
                tbdays.Text = e.NewValue.ToString();
                string tip = "Stable";
                stabntb.Text = Math.Round(CalculateInterest(Convert.ToDecimal(tbstab.Text), Convert.ToInt32(e.NewValue), Convert.ToDecimal(tbpopol.Text), tip)) + " Руб.";
                tip = "Optimal";
                Optimtb.Text = Math.Round(CalculateInterest(Convert.ToDecimal(tbstab.Text), Convert.ToInt32(e.NewValue), Convert.ToDecimal(tbpopol.Text), tip)) + " Руб.";
                tip = "Standard";
                Standarttb.Text = Math.Round(CalculateInterest(Convert.ToDecimal(tbstab.Text), Convert.ToInt32(e.NewValue), Convert.ToDecimal(tbpopol.Text), tip)) + " Руб.";
            }
            else System.Diagnostics.Debug.WriteLine("myTextBlock is null!");

            
        }

        private void slpopol_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (stabntb != null)
            {
                tbpopol.Text = e.NewValue.ToString();
                string tip = "Optimal";
                Optimtb.Text = Math.Round(CalculateInterest(Convert.ToDecimal(tbstab.Text), Convert.ToInt32(tbdays.Text), Convert.ToDecimal(e.NewValue), tip)) + " Руб.";
                tip = "Standard";
                Standarttb.Text = Math.Round(CalculateInterest(Convert.ToDecimal(tbstab.Text), Convert.ToInt32(tbdays.Text), Convert.ToDecimal(e.NewValue), tip)) + " Руб.";
            }
            else System.Diagnostics.Debug.WriteLine("myTextBlock is null!");

            
        }
    }
}
