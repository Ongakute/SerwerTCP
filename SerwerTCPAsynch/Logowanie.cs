using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace SerwerTCPAsynch
{
    class Logowanie
    {
        String podajLogin;
        String podajHaslo;
        BazaUzytkownikow uzytkownicy;

        public Logowanie()
        {
            podajLogin = "Podaj login: ";
            podajHaslo = "Podaj haslo: ";
            uzytkownicy = new BazaUzytkownikow();
        }
        //klasa logowanie bool-? czy oistnieje login i hasło arg - key, valuew
        //klasa serwerasyhnochroniczy dodać funkcj wysyłanie i funkcd odbior wiadonocści
        //echo kalsa - dostanie w arg stream

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
            /*
            byte[] Bufor = Encoding.ASCII.GetBytes(witaj);
            stream.Write(Bufor, 0, Bufor.Length);
            Array.Clear(Bufor, 0, Bufor.Length);

            while (true)
            {
                try
                {

                    Bufor = Encoding.ASCII.GetBytes(podajLogin);
                    stream.Write(Bufor, 0, Bufor.Length);
                    Array.Clear(Bufor, 0, Bufor.Length);

                    Bufor = new byte[RozmiarBufora];

                    int message_size = stream.Read(Bufor, 0, RozmiarBufora);


                    if (Bufor[0] == 13)
                    {
                        message_size = stream.Read(Bufor, 0, RozmiarBufora);
                    }

                    String login = dekodowanieWiadomosci(message_size);

                    if (uzytkownicy.czyIstnieje(login))
                    {
                        Bufor = Encoding.ASCII.GetBytes(podajHaslo);
                        stream.Write(Bufor, 0, Bufor.Length);
                        Array.Clear(Bufor, 0, Bufor.Length);

                        Bufor = new byte[RozmiarBufora];
                        message_size = stream.Read(Bufor, 0, RozmiarBufora);
                        if (Bufor[0] == 13)
                        {
                            message_size = stream.Read(Bufor, 0, RozmiarBufora);
                            String haslo = dekodowanieWiadomosci(message_size);
                            String temp;
                            uzytkownicy.TryGetValue(login, out temp);
                            if (temp == haslo)
                            {
                                return;
                            }
                            else
                            {
                                Bufor = Encoding.ASCII.GetBytes("Bledne haslo. Sprobuj jeszcze raz.\r\n");
                                stream.Write(Bufor, 0, Bufor.Length);
                                Array.Clear(Bufor, 0, Bufor.Length);
                            }
                        }
                    }
                    else
                    {
                        Bufor = Encoding.ASCII.GetBytes("Bledny login. \r\n");
                        stream.Write(Bufor, 0, Bufor.Length);
                        Array.Clear(Bufor, 0, Bufor.Length);
                    }


                }
                catch (IOException e)
                {
                    return;
                }*/
            return false;

        }

    }
}
