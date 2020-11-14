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

        /* public void Ustawiacz(string klucz, string wartosc)
         {
             if (uzytkownicy.ContainsKey(klucz))
             {
                 uzytkownicy[klucz] = wartosc;
             }
             else
             {
                 uzytkownicy.Add(klucz, wartosc);
             }

         }*/

        public string pobieraczHasla(string klucz)
        {
            string wynik = null;

            if (uzytkownicy.ContainsKey(klucz))
            {
                wynik = uzytkownicy[klucz];
            }

            return wynik;
        }

        public void dodajUzytkownika(string login, string haslo)
        {
            uzytkownicy.Add(login, haslo);
        }

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
