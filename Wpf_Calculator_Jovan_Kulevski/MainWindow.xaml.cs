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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf_Calculator_Jovan_Kulevski
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double num1 = 0;
        double num2 = 0;
        string Operator = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void input(string a)
        {
            if (txt_Main.Text == "0")
                txt_Main.Text = a;
            else
                txt_Main.Text += a;
        }

        private void btn_O_Click(object sender, RoutedEventArgs e)
        {
            Button btn_Operator = (Button)sender;
            Operator = btn_Operator.Content.ToString();
            num1 = double.Parse(txt_Main.Text);
            txt_Main.Text = "0";
        }

        private void btn_Number_Click(object sender, RoutedEventArgs e)
        {
            Button btn_Number = (Button)sender;
            input(btn_Number.Content.ToString());
        }

        private void btn_Dot_Click(object sender, RoutedEventArgs e)
        {
            if (txt_Main.Text != "")
            {
                if (!txt_Main.Text.Contains("."))
                    input(".");
            }
        }

        private void txt_Main_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btn_Remove_Click(object sender, RoutedEventArgs e)
        {
            if (txt_Main.Text != "0")
            {
                if (txt_Main.Text.Length == 1)
                {
                    txt_Main.Text = "0";
                }
                else if (txt_Main.Text.Length > 0)
                {
                    txt_Main.Text = txt_Main.Text.Substring(0, txt_Main.Text.Length - 1);
                }
            }
        }

        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            txt_Main.Text = "0";
            txt_ResultWord.Text = "";
            Operator = "";
            num1 = 0;
            num2 = 0;
        }

        private void btn_Equal_Click(object sender, RoutedEventArgs e)
        {
            double result = 0;
            num2 = double.Parse(txt_Main.Text);
            txt_ResultWord.Text = "";

            switch (Operator)
            {
                case "+":
                    result = num1 + num2;
                    txt_Main.Text = result.ToString();
                    break;
                case "-":
                    result = num1 - num2;
                    txt_Main.Text = result.ToString();
                    break;
                case "×":
                    result = num1 * num2;
                    txt_Main.Text = result.ToString();
                    break;
                case "/":
                    if (num2 != 0)
                    {
                        result = num1 / num2;
                        txt_Main.Text = result.ToString();
                    }
                    else
                        MessageBox.Show("You cannot divide by zero ");
                    break;
                default:
                    break;
            }
            try
            {
                txt_ResultWord.Text = Numeric.ToWords(result);
            }
            catch (Exception)
            {
                txt_ResultWord.Text = "Beskonacno";
            }
        }

        public static class Numeric
        {
            private enum Digit
            {
                nula = 0, jedna = 1, dve = 2, tri = 3, cetiri = 4,
                pet = 5, sest = 6, sedam = 7, osam = 8, devet = 9
            }

            private enum Teen
            {
                deset = 10, jedanaest = 11, dvanaest = 12, trinaest = 13, cetrnaest = 14,
                petnaest = 15, sesnaest = 16, sedamnaest = 17, osamnaest = 18, devetneast = 19
            }

            private enum Ten
            {
                dvadeset = 2, trideset = 3, cetrdeset = 4, pedeset = 5,
                sestedeset = 6, sedamdeset = 7, osamdeset = 8, devedeset = 9
            }

            private enum PowerOfTen
            {
                stotina = 0, hiljada = 1, miliona = 2, milijardi = 3,
                biliona = 4, bilijarda = 5, triliona = 6
            }

            private static int PowersOfTen = Enum.GetValues(typeof(PowerOfTen)).Length;

            public static string ToWords(double N)
            {
                string Prefix = N < 0 ? "minus " : "";
                string Significand = Digit.nula.ToString();
                string Mantissa = "";
                if ((N = Math.Abs(N)) > 0)
                {
                    
                    if (N != Math.Floor(N))
                    {
                        Mantissa = " tacka";
                        foreach (char C in N.ToString().Substring(N.ToString().IndexOf('.') + 1))
                            Mantissa += " " + ((Digit)(int.Parse(C.ToString())));
                    }

                    if ((Convert.ToInt64(N = Math.Floor(N)) % 100) < Int64.MaxValue)
                    {
                        Int64 n = Convert.ToInt64(N = Math.Floor(N)) % 100;
                        Significand = n == 0 ? ""
                          : n < 10 ? ((Digit)n).ToString()
                          : n < 20 ? ((Teen)n).ToString()
                          : (Ten)(n / 10) + "-" + (Digit)(n % 10);

                        if ((N = Math.Floor(N / 100D)) > 0)
                        {
                            string EW = "";
                            for (int i = 0; (N > 0) && (i < PowersOfTen); i++)
                            {
                                double p = Math.Pow(10, (i << 1) + 1);
                                n = Convert.ToInt64(N % p);
                                if (n > 0)
                                    EW = ToWords(n) + " " + (PowerOfTen)i + (EW.Length == 0 ? "" : ", " + EW);
                                N = Math.Floor(N / p);
                            }
                            if (EW.Length > 0)
                                Significand = EW + (Significand.Length == 0 ? "" : " i " + Significand);
                        }
                    }
                    else
                    {
                        return "Beskonacno";
                    }
                }
                return Prefix + (Significand + Mantissa).Trim();
            }
        }
    }
}
