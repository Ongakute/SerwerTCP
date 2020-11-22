﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SerwerTCPAsynch
{
    public class SerwerAsynchroniczny : Serwer
    {
        #region Zmienne

        Logowanie logowanie;
        /// <summary>
        /// Delegatura watku dla każdego klienta
        /// </summary>
        /// <param name="strumien"></param>
        public delegate void watekTransmisji(NetworkStream strumien);
        #endregion

        #region Konstruktor
        /// <summary>
        /// Konstruktor klasy SerwerTCPAsynch dziedziczącej po klasie Serwer
        /// </summary>
        /// <param IP="IP"></param>
        /// <param port="port"></param>
        public SerwerAsynchroniczny(IPAddress IP, int port) : base(IP, port)
        {
            logowanie = new Logowanie();
        }

        #endregion

        #region Funkcje
        /// <summary>
        /// Funkcja akceptująca połączenie z klientem oraz tworząca wątek dla każdego klienta.
        /// </summary>
        protected override void akceptujPolaczenie()
        {

            while (true)

            {
                TcpClient tcpKlient = TcpListener.AcceptTcpClient();

                Strumien = tcpKlient.GetStream();

                watekTransmisji watek = new watekTransmisji(rozpocznijTransmisje);
                watek.BeginInvoke(Strumien, wywolanieZwrotne, tcpKlient);
            }
        }

        /// <summary>
        /// wywolanieZwrotne wywoływany po zakończeniu wątku
        /// </summary>
        protected void wywolanieZwrotne(IAsyncResult ar)
        {
            Console.WriteLine("Koniec polaczenia");
        }


        /// <summary>
        /// Funkcja wykonywana w każdym wątku, dla każdego klienta. 
        /// </summary>
        /// <param name="stream"></param>
        protected override void rozpocznijTransmisje(NetworkStream strumien)
        {
            string login;
            string haslo;
            string wiadomosc;

            wysylanieWiadomosci("Witaj uzytkowniku! \n\r");
            do
            {
                wysylanieWiadomosci("Podaj login: ");
                login = odbieranieWiadomosci();
                wysylanieWiadomosci("Podaj haslo: ");
                haslo = odbieranieWiadomosci();
                haslo = odbieranieWiadomosci();

            } while (!logowanie.zaloguj(login, haslo));

            Console.Write("Uzytkownik zalogowany \n\r");
            wysylanieWiadomosci("Poprawnie zalogowano!");

            Szyfrowanie szyfr = new Szyfrowanie();
            String klucz, wiadomosc_szyfr;

            while (true)
            {
                wysylanieWiadomosci("Wybierz opcje:  a) Zaszyfruj wiadomosc  b) Odszyfruj Wiadomosc ");
                wiadomosc = odbieranieWiadomosci();

                if (wiadomosc.Contains("a"))
                {
                    wysylanieWiadomosci("Wybrano Szyfrowanie: \r\n");
                    wysylanieWiadomosci("Wpisz wiadomosc: \r\n");
                    wiadomosc_szyfr = odbieranieWiadomosci();
                    wiadomosc_szyfr = odbieranieWiadomosci();
                    Console.Write(wiadomosc_szyfr + "\n\r");
                    wysylanieWiadomosci("Wpisz klucz do szyfrowania \r\n");
                    klucz = odbieranieWiadomosci();
                    klucz = odbieranieWiadomosci();
                    Console.Write(klucz + "\n\r");
                    wiadomosc = szyfr.tworzenieSzyfru(wiadomosc_szyfr, klucz);
                    wysylanieWiadomosci("Wiadomosc zaszyfrowana: ");
                    wysylanieWiadomosci(wiadomosc + "\r\n");

                } else if(wiadomosc.Contains("b"))
                {
                    wysylanieWiadomosci("Wybrano Deszyfrowanie: \r\n");
                    wysylanieWiadomosci("Wpisz wiadomosc: \r\n");
                    wiadomosc_szyfr = odbieranieWiadomosci();
                    wiadomosc_szyfr = odbieranieWiadomosci();
                    Console.Write(wiadomosc_szyfr + "\n\r");
                    wysylanieWiadomosci("Wpisz klucz do deszyfrowania \r\n");
                    klucz = odbieranieWiadomosci();
                    klucz = odbieranieWiadomosci();
                    Console.Write(klucz + "\n\r");
                    wiadomosc = szyfr.deszyfracja(wiadomosc_szyfr, klucz);
                    wysylanieWiadomosci("Wiadomosc odszyfrowana: \r\n");
                    wysylanieWiadomosci(wiadomosc + "\r\n");
                }
                //wysylanieWiadomosci(wiadomosc);
            }
        }

        /// <summary>
        /// Funkcja wysyłająca wiadomość z serwera do klienta
        /// </summary>
        /// <param name="wysylanieWiadomosci"></param>
        private void wysylanieWiadomosci(string wiadomosc)
        {
            try
            {


                Bufor = Encoding.ASCII.GetBytes(wiadomosc);
                Strumien.Write(Bufor, 0, Bufor.Length);
                Array.Clear(Bufor, 0, Bufor.Length);

            }
            catch (IOException e)
            {
                Console.Write("BLAD!");
            }
        }


        /// <summary>
        /// Funkcja która odbiera wiadomość
        /// </summary>
        /// <returns></returns>
        private string odbieranieWiadomosci()
        {
            string wiadomosc = "";
            try
            {
                Bufor = new byte[RozmiarBufora];
                int rozmiarWiadomosci = Strumien.Read(Bufor, 0, RozmiarBufora);

                wiadomosc = dekodowanieWiadomosci(rozmiarWiadomosci);
                Array.Clear(Bufor, 0, RozmiarBufora);
            }
            catch (IOException e)
            {
                Console.Write("BLAD!");
            }
            return wiadomosc;
        }

        /// <summary>
        /// Pomocnicza funkcja zamieniająca tablicę bytów na String bez białych znaków i zer 
        /// </summary>
        /// <param name="rozmiarWiadomosci"></param>
        /// <returns></returns>
        private String dekodowanieWiadomosci(int rozmiarWiadomosci)
        {
            char[] wiadomosc = new char[rozmiarWiadomosci];
            for (int i = 0; i < rozmiarWiadomosci; i++)
            {
                wiadomosc[i] = (char)Bufor[i];
            }
            String wynik = new String(wiadomosc);
            return wynik;
        }



        /// <summary>
        /// Odpowiada za uruchomienie instancji serwera.
        /// </summary>
        public override void start()
        {
            zacznijNasluchiwanie();
            akceptujPolaczenie();
        }




    }
    #endregion
}
