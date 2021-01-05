using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;


namespace SerwerTCPAsynch
{
    public class Logowanie
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
                //char[] tymczasowy = hasloTajne.ToCharArray();
                List<int> haslo_zahaszowane = new List<int>();
                int wartosc_znaku;
                for (int i = 0; i < hasloTajne.Length; i++)
                {
                    wartosc_znaku = Char.ConvertToUtf32(hasloTajne, i) * ziarno;
                    haslo_zahaszowane.Add(wartosc_znaku);
                    //tymczasowy[i] = Convert.ToChar(Char.ConvertToUtf32(hasloTajne, i) * ziarno);
                }
                //hasloTajne = tymczasowy.ToString();
                hasloTajne = string.Join("", haslo_zahaszowane);
                if (hasloTajne == haslo)
                {
                    statusLogowania = true;
                    return true;
                }
                else return false;
            }
            else return false;

        }

    }
}
