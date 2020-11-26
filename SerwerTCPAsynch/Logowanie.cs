using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace SerwerTCPAsynch
{
    class Logowanie
    {
       
        BazaUzytkownikow uzytkownicy;
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
            uzytkownicy = new BazaUzytkownikow();
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
            else if(!czyHaslo)
            {
                login = wiadomosc;
                odpowiedz = "Podaj haslo: ";
                
                czyHaslo = true;
            }
            else
            {
                haslo = wiadomosc;
                zaloguj(login, haslo);
                if(statusLogowania)
                {
                    odpowiedz = "Poprawnie zalogowano. \r\n";
                }
                else
                {
                    odpowiedz = "Niepoprawny login lub haslo, sprobuj jeszcze raz.";
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
            if (uzytkownicy.czyIstnieje(login))
            {
                if (uzytkownicy.pobieraczHasla(login) == haslo)
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
