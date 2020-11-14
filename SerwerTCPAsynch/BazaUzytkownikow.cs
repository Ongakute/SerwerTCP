using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerwerTCPAsynch
{
    class BazaUzytkownikow
    {
        Dictionary<String, String> uzytkownicy;

        /// <summary>
        /// Konstruktor klasy BazaUzytkownikow
        /// </summary>
        public BazaUzytkownikow()
        {
            uzytkownicy = new Dictionary<String, String>();
            uzytkownicy.Add("admin", "admin");
            uzytkownicy.Add("root", "admin");
            uzytkownicy.Add("user", "user");
        }

       
        /// <summary>
        /// Funkcja, która umożliwia pobranie hasła na podstawie loginu
        /// </summary>
        /// <param name="klucz"></param>
        /// <returns></returns>
        public string pobieraczHasla(string klucz)
        {
            string wynik = null;

            if (uzytkownicy.ContainsKey(klucz))
            {
                wynik = uzytkownicy[klucz];
            }

            return wynik;
        }

        /// <summary>
        /// Funkcja dodająca użytkowników do bazy
        /// </summary>
        /// <param name="login"></param>
        /// <param name="haslo"></param>
        public void dodajUzytkownika(string login, string haslo)
        {
            uzytkownicy.Add(login, haslo);
        }

        /// <summary>
        /// Funkcja sprawdzająca czy istnieje uzytkownik o danym loginie
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public bool czyIstnieje(string login)
        {
            if (uzytkownicy.ContainsKey(login))
            {
                return true;
            }

            return false;
        }

    }
}
