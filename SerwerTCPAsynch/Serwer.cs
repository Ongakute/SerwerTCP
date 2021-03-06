﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SerwerTCPAsynch
{
    /// <summary>
    /// Abstrakcyjna klasa Serwer
    /// </summary>
    public abstract class Serwer<T> where T : ProtokolKomunikacyjny, new()
    {
        #region Pola

        IPAddress mojeIP;
        BazaUzytkownikow uzytkownicy;
        int mojPort;
        int rozmiarBufora;
        byte[] bufor;
        TcpListener serwer;
        TcpClient klient;
        NetworkStream strumien;
        bool polaczony;
        #endregion

        #region Właściwości Pól
        protected TcpListener TcpListener { get => serwer; set => serwer = value; }

        protected TcpClient TcpClient { get => klient; set => klient = value; }

        protected NetworkStream Strumien { get => strumien; set => strumien = value; }

        public BazaUzytkownikow Uzytkownicy { get => uzytkownicy; set => uzytkownicy = value; }

        protected byte[] Bufor { get => bufor; set => bufor = value; }

        protected int RozmiarBufora { get => rozmiarBufora; set => rozmiarBufora = value; }

        protected bool Polaczony { get => polaczony; set => polaczony = value; }
        #endregion

        #region Konstruktory

        /// <summary>
        /// Konstruktor domyślny
        /// </summary>
        public Serwer()
        {
            mojeIP = IPAddress.Parse("127.0.0.1");
            mojPort = 7777;
            uzytkownicy = new BazaUzytkownikow();
            bufor = new byte[1024];
            rozmiarBufora = 1024;
            polaczony = false;
        }

        /// <summary>
        /// Konstruktor klasy SerwerTCP z dwoma argumentami: adres IP oraz numer portu.
        /// </summary>
        /// <param mojeIP="adresIP"></param>
        /// <param mojPort="port"></param>
        public Serwer(IPAddress adresIP, int port)
        {
            if (adresIP.ToString() != "127.0.0.1")
            {
                throw new Exception("Podano błędny adres IP");
            }

            if (port != 2048)
            {
                throw new Exception("Podano błędny port");
            }


            mojeIP = adresIP;
            mojPort = port;
            uzytkownicy = new BazaUzytkownikow();
            bufor = new byte[1024];
            rozmiarBufora = 1024;
            polaczony = false;
        }

        #endregion

        #region Funkcje
        /// <summary>
        /// Rozpoczyna nasłuchiwanie przez serwer
        /// </summary>
        protected void zacznijNasluchiwanie()
        {
            serwer = new TcpListener(mojeIP, mojPort);
            serwer.Start();
            Console.Out.WriteLine("Rozpoczecie nasluchiwania.");
        }

        /// <summary>
        /// Funkcja akceptująca połączenie z klientem
        /// </summary>
        protected abstract void akceptujPolaczenie();

        /// <summary>
        /// Rozpoczęcie transmisji pomiędzy serwerem i klientem
        /// </summary>
        protected abstract void rozpocznijTransmisje(NetworkStream strumien);

        /// <summary>
        /// Wystartowanie działania
        /// </summary>
        public abstract void start();

        #endregion


    }
}
