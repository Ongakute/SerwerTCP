using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace SerwerTCPAsynch
{
    class Logowanie
    {

        BazaUzytkownikow baza;
        int ziarno;
        public bool statusLogowania;
        bool czyLogin;
        bool czyHaslo;
        String login = null;
        String haslo = null;


        /// <summary>
        /// Konstruktor klasy logowanie
        /// </summary>
        public Logowanie()
        {

        }

        public Logowanie(BazaUzytkownikow baza, int ziarno)
        {
            this.baza = baza;
            this.ziarno = ziarno;
            statusLogowania = false;
            czyLogin = false;
            czyHaslo = false;
        }

        public String utworzOdpowiedz(String wiadomosc)
        {
            String odpowiedz = null;
            ;


            if (!czyLogin)
            {
                odpowiedz = "Podaj login: ";

                czyLogin = true;
            }
            else if (!czyHaslo)
            {
                login = wiadomosc;
                odpowiedz = "Podaj haslo: ";

                czyHaslo = true;
            }
            else
            {
                haslo = wiadomosc;
                zaloguj(login, haslo);
                if (statusLogowania)
                {
                    odpowiedz = "Poprawnie zalogowano. \r\n";
                }
                else
                {
                    odpowiedz = "Niepoprawny login lub haslo, sprobuj jeszcze raz.";
                    czyLogin = false;
                    czyHaslo = false;
                }

            }

            return odpowiedz;
        }

        /// <summary>
        /// Wywoływana przez rozpocznijTransmisję, po poprawnym zalogowaniu, powraca do funkcji wywołującej.
        /// </summary>
        /// <param name="stream"></param>
        protected bool zaloguj(string login, string haslo)
        {
            if (baza.czyIstnieje(login))
            {
                string hasloTajne = baza.pobieraczHasla(login);
                char[] tymczasowy = hasloTajne.ToCharArray();
                for (int i = 0; i < hasloTajne.Length; i++)
                {
                    tymczasowy[i] = Convert.ToChar(Char.ConvertToUtf32(hasloTajne, i) * ziarno);
                }
                hasloTajne = tymczasowy.ToString();
                if (hasloTajne == haslo)
                {
                    statusLogowania = true;
                    return true;

                }

            }

            return false;
        }
    }
}
