using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SerwerTCPAsynch
{
    public class SerwerAsynchroniczny<T> : Serwer<T> where T : ProtokolSzyfrowania, new()
    {
        #region Zmienne


        /// <summary>
        /// Delegatura watku dla każdego klienta
        /// </summary>
        /// <param name="strumien"></param>
        public delegate void watekTransmisji(NetworkStream strumien);
        int ziarno;
        #endregion

        #region Konstruktor
        /// <summary>
        /// Konstruktor klasy SerwerTCPAsynch dziedziczącej po klasie Serwer
        /// </summary>
        /// <param IP="IP"></param>
        /// <param port="port"></param>
        public SerwerAsynchroniczny(IPAddress IP, int port) : base(IP, port)
        {

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
            ProtokolSzyfrowania protokol = new T();
            var rand = new Random();
            ziarno = rand.Next(1, 1000);
            Logowanie logowanie = new Logowanie(Uzytkownicy, ziarno);



            String wiadomosc = null;

            wiadomosc = odbieranieWiadomosci();
            wysylanieWiadomosci(ziarno.ToString());

            while (!logowanie.statusLogowania)
            {
                wiadomosc = odbieranieWiadomosci();
                wysylanieWiadomosci(logowanie.utworzOdpowiedz(wiadomosc));
                //wiadomosc = "";
            }


            Console.Write("Uzytkownik zalogowany \n\r");
            wiadomosc = odbieranieWiadomosci();
            wysylanieWiadomosci(protokol.inicjalizujPrace());
            while (true)
            {
                wiadomosc = odbieranieWiadomosci();
                wysylanieWiadomosci(protokol.utworzOdpowiedz(wiadomosc));
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
                //Console.Write("BLAD!");
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
                //Console.Write("BLAD!");
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
