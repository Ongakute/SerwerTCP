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

        /// <summary>
        /// Konstruktor klasy logowanie
        /// </summary>
        public Logowanie()
        {
       
            uzytkownicy = new BazaUzytkownikow();
        }
        

        /// <summary>
        /// Wywoływana przez rozpocznijTransmisję, po poprawnym zalogowaniu, powraca do funkcji wywołującej.
        /// </summary>
        /// <param name="stream"></param>
        public bool zaloguj(string login, string haslo)
        {
            if (uzytkownicy.czyIstnieje(login))
            {
                if (uzytkownicy.pobieraczHasla(login) == haslo)
                {
                    return true;
                }
                else return false;
            }
            else return false;
            
           

        }

    }
}
