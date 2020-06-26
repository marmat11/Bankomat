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
using System.Data.SqlClient;
using System.Data;

namespace Bankomat
{
    /// <summary>
    /// Logika interakcji dla klasy Okno.xaml
    /// </summary>
    public partial class Okno : Window
    {
        public Okno()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Metoda przycisku Wyciąg kartę. Metoda ustawia wartość zmiennej karta na false oraz blokuje przyciski Wypłać,Wpłać,Stan Konta oraz wypisuje napis w TBPokaż
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BKarta_Click(object sender, RoutedEventArgs e)
        {
            MainWindow k = new MainWindow();
            k.karta = false;
            LabKarta.Content = "Brak Karty";

            TBPokaż.Text = "Brak karty!\nProszę wyjść i włożyć kartę jeszcze raz.";

            BWpłać.IsEnabled = false;
            BWypłać.IsEnabled = false;
            BStankonta.IsEnabled = false;
        }

        private void B1_Click(object sender, RoutedEventArgs e)
        {
            Button obutton = (Button)sender;
            TbLogin.Text += obutton.Content;
        }

        /// <summary>
        /// Metoda przycisku Wyjdz. Zamyka okno i otwiera pierwsze okno główne.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BWyjdz_Click(object sender, RoutedEventArgs e)
        {
            MainWindow pokaż = new MainWindow();
            pokaż.Show();

            this.Close();
        }

        private void BZamknij_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Metoda przycisku Stan Konta.Metoda wyciąga wartość StanKonta z bazy danych dla zalogowanego użytkownika.
        /// Wyświetla w TbPokaż informację o stanie konta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BStankonta_Click(object sender, RoutedEventArgs e)
        {
            MainWindow p = new MainWindow();

            TBPokaż.Text = "Stan twojego konta wynosi = ";

            string connectionString = @"Data source=.\SQLExpress;database=BazaBankomat;Trusted_Connection=True";
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;

           //string przekaz1 = "1234567890123456";

            string commandText = "Select StanKonta from [Dane Logowania] where [Numer Karty] = '" + p.Przekaż() + "'";

            command.CommandText = commandText;

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    TBPokaż.Text += String.Format("{0}",
                        reader[0]);
                }
            }
            
            connection.Close();       
        }

        /// <summary>
        /// Metoda przycisku Wypłać. Metoda dokonuje zmiany[odejmuje] w wartości StanKonta w bazie danych o wartość podaną w TbLogin dla danego użytkownika.
        /// Wyświetla w TbPokaż informację o ilości wypłaconej gotówki oraz o zaktualizowanym stanie konta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BWypłać_Click(object sender, RoutedEventArgs e)
        {
            int wypłać = Convert.ToInt32(TbLogin.Text);

            MainWindow p = new MainWindow();

            TBPokaż.Text = "Wypłaciłeś :";
            TBPokaż.Text += wypłać.ToString();

            string connectionString = @"Data source=.\SQLExpress;database=BazaBankomat;Trusted_Connection=True";
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;

            string commandText = "Update [Dane Logowania] Set StanKonta = StanKonta - '" + wypłać + "' where [Numer Karty] = '" + p.Przekaż() + "'";

            command.CommandText = commandText;

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    TBPokaż.Text += String.Format("{0}",
                        reader[0]);
                }
            }

            TBPokaż.Text += "\nStan konta = ";

            string commandText1 = "Select StanKonta from [Dane Logowania] where [Numer Karty] = '" + p.Przekaż() + "'";

            command.CommandText = commandText1;

            using (SqlDataReader reader1 = command.ExecuteReader())
            {
                while (reader1.Read())
                {
                    TBPokaż.Text += String.Format("{0}", reader1[0]);         
                }
            }

            connection.Close();

        }

        /// <summary>
        /// Metoda przycisku Wpłać. Metoda dokonuje zmiany[dodaje] w wartości StanKonta w bazie danych o wartość podaną w TbLogin dla danego użytkownika.
        /// Wyświetla w TbPokaż informację o ilości wypłaconej gotówki oraz o zaktualizowanym stanie konta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BWpłać_Click(object sender, RoutedEventArgs e)
        {
            int wpłać = Convert.ToInt32(TbLogin.Text);

            MainWindow p = new MainWindow();

            TBPokaż.Text = "Wpłaciłeś : ";
            TBPokaż.Text += wpłać.ToString();

            string connectionString = @"Data source=.\SQLExpress;database=BazaBankomat;Trusted_Connection=True";
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;

            string commandText = "Update [Dane Logowania] Set StanKonta = StanKonta + '" + wpłać + "' where [Numer Karty] = '" + p.Przekaż() + "'";

            command.CommandText = commandText;

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    TBPokaż.Text += String.Format("{0}",
                        reader[0]);
                }
            }

            TBPokaż.Text += "\nStan konta = ";

            string commandText1 = "Select StanKonta from [Dane Logowania] where [Numer Karty] = '" + p.Przekaż() + "'";

            command.CommandText = commandText1;

            using (SqlDataReader reader1 = command.ExecuteReader())
            {
                while (reader1.Read())
                {
                    TBPokaż.Text += String.Format("{0}", reader1[0]);
                }
            }

            connection.Close();

        }
    }
}
