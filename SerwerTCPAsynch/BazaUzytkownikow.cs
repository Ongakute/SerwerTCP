using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace SerwerTCPAsynch
{
    public class BazaUzytkownikow
    {

        SQLiteConnection sqlite_conn;

        /// <summary>
        /// Konstruktor klasy BazaUzytkownikow
        /// </summary>
        public BazaUzytkownikow()
        {
            sqlite_conn = CreateConnection();
        }

        private SQLiteConnection CreateConnection()
        {
            //SQLiteConnection sqlite_conn;
            // Create a new database connection:
            SQLiteConnection connection = new SQLiteConnection(@"Data Source=database.db");
            // Open the connection:
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {

            }

            //SQLiteCommand sqlite_cmd;
            //sqlite_cmd = connection.CreateCommand();
            //sqlite_cmd.CommandText = "INSERT INTO users (login, password) VALUES('root','root'); ";
            //sqlite_cmd.ExecuteNonQuery();
            return connection;
        }


        /// <summary>
        /// Funkcja, która umożliwia pobranie hasła na podstawie loginu
        /// </summary>
        /// <param name="klucz"></param>
        /// <returns></returns>
        public string pobieraczHasla(string login)
        {
            string wynik = null;

            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT haslo FROM users WHERE login LIKE $login ";
            sqlite_cmd.Parameters.AddWithValue("$login", login);
            //sqlite_cmd.CommandText = "SELECT * FROM users";

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetString(0);
                if (!myreader.Equals(null))
                    wynik = myreader;
            }
            //sqlite_conn.Close();
            return wynik;
        }

        /// <summary>
        /// Funkcja dodająca użytkowników do bazy
        /// </summary>
        /// <param name="login"></param>
        /// <param name="haslo"></param>
        public void dodajUzytkownika(string login, string haslo)
        {
            //uzytkownicy.Add(login, haslo);
        }

        /// <summary>
        /// Funkcja sprawdzająca czy istnieje uzytkownik o danym loginie
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public bool czyIstnieje(string login)
        {

            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT login FROM users WHERE login LIKE $login ";
            sqlite_cmd.Parameters.AddWithValue("$login", login);
            //sqlite_cmd.CommandText = "SELECT * FROM users";

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetString(0);
                if (!myreader.Equals(null))
                    return true;
                else return false;
            }
            //sqlite_conn.Close();

            /* if (uzytkownicy.ContainsKey(login))
             {
                 return true;
             }
             */
            return false;
        }

    }
}
