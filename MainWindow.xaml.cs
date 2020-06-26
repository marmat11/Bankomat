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
using System.Data.SqlClient;
using System.Data;

namespace Bankomat
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //zmienna definująca czy karta jest włożona
        public bool karta = false;
        // zmienna zliczająca nieudane próby logowania
        private int liczLogowania = 0;

        
 
        public MainWindow()
        {
            InitializeComponent();
        }
        
       
        /// <summary>
        /// Metoda dla przycisków numerycznych. Wysyła nazwę buttona do TbLogin oraz ustawia maksymalną długość TbLogin na 16 znaków a PbHasło na 4 znaki.
        /// </summary>
        /// <param name="sender"></param>
        public void Button(object sender)
        {
            TbLogin.MaxLength = 16;
            PbHasło.MaxLength = 4;
            int ilośćznakówtb = TbLogin.GetLineLength(0);

            Button obutton = (Button)sender;

            if (ilośćznakówtb <= 16)
            {
                TbLogin.Text += obutton.Content;
            }
            else
            {
                PbHasło.Password += obutton.Content;
            }

        }

        /// <summary>
        /// Metoda zwracająca numer karty osoby zalogowanej
        /// </summary>
        /// <returns>Wraca numer karty osoby zalogowanej</returns>
        public string Przekaż()
        {
            string przekażlogin;
            string error = "error";
            string Login = TbLogin.Text;
            string Hasło = PbHasło.Password;

            SqlDataReader sprawdz = null;


            string connectionString = @"Data source=.\SQLExpress;database=BazaBankomat;Trusted_Connection=True";
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;



            string commandText = "Select * From[Dane Logowania] Where[Numer Karty]='" + Login + "' And Pin ='" + Hasło + "'";


            command.CommandText = commandText;
            sprawdz = command.ExecuteReader();

            if (sprawdz.HasRows == true )
            {
              return  przekażlogin = TbLogin.Text;

            }
            else
            {
                return error;
            }

            
        }

       /// <summary>
       /// Metoda przycisku Włóż Kartę. Zmienia wartość zmiennej karta na true oraz zmienia Napis w LabKarta na Karta włożona
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void BKarta_Click(object sender, RoutedEventArgs e)
        {
            karta = true;
            LabKarta.Content = "Karta włożona";
        }

        /// <summary>
        /// Metoda przycisków numerycznych.Wywołuje motedę Button()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BNumer_Click(object sender, RoutedEventArgs e)
        {
            Button(sender); 
        }
        
        /// <summary>
        /// Metoda przycisku Zaloguj.Sprawdza podane wartości TbLogin i PbHasło z danymi zawartymi w bazie danaych [Numer Karty == Login i Pin == Hasło] 
        /// oraz wartość zmiennej karta , jeśli podane dane są prawdziwe i wartość karta = true to pokazuje komunikat logowanie udane i wywołuje nowe okno
        /// jeśli któryś z warunków jest niepoprawny pokazuje się komunikat informujący o błędnym logowaniu jeśli niepoprawne logowanie powtarza się 3 razy 
        /// to zarówno TbLogin jak i PbHasło zostają zablokowane.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BZaloguj_Click(object sender, RoutedEventArgs e)
        {
            string Login = TbLogin.Text;
            string Hasło = PbHasło.Password;

            SqlDataReader sprawdz = null;


            string connectionString = @"Data source=.\SQLExpress;database=BazaBankomat;Trusted_Connection=True";
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;



            string commandText = "Select * From[Dane Logowania] Where[Numer Karty]='" + Login + "' And Pin ='" + Hasło + "'";
          

            command.CommandText = commandText;
            sprawdz = command.ExecuteReader();

            if (sprawdz.HasRows == true && karta == true)
            {
                MessageBox.Show("Logowanie udane");

                Okno pokaż = new Okno();
                pokaż.Show();
                this.Close();
            }
            else
            {
                liczLogowania++;
                int próba = 3;
                próba = próba - liczLogowania;
                MessageBox.Show("Logowanie nieudane.  Zostało prób : " + próba + "\nPo 3 nieudanych próbach możliwość logowania zostaje zablokowana.");
            }

            connection.Close();

            if (liczLogowania >= 3)
            {
                TbLogin.Clear();
                TbLogin.IsReadOnly = true;
                TbLogin.IsEnabled = false;
                PbHasło.Clear();
                PbHasło.IsEnabled = false;
            }

        }

        /// <summary>
        /// Metoda przycisku Cofnij. Metoda usuwa jeden znak z TbLogin.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BCofnij_Click(object sender, RoutedEventArgs e)
        {
            int ile = TbLogin.GetLineLength(0);
            int ile1 = ile - 1;
            string tekst = TbLogin.Text;
            string tekst1 = tekst.Remove(ile1, 1);
            TbLogin.Text = tekst1;

        }

        /// <summary>
        /// Metoda przycisku Zamknij.Zamyka aplikację.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BZamknij_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
