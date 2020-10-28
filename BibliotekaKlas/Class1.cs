using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace BibliotekaKlas
{
    /// <summary>
    /// Klasa Serwer obsługuje działanie serwera TCP obliczającego sinus podanego kąta alpha
    /// </summary>
    public class Serwer
    
    {
        IPAddress IPadd;
        int Port;
        byte[] buffor; 
        TcpListener serwer;
        TcpClient klient;
        /// <summary>
        /// Konstruktor domyślny
        /// </summary>
        public Serwer()
        {
            IPadd = IPAddress.Parse("127.0.0.1");
            Port = 7777;
            buffor = new byte[1024];
            serwer = new TcpListener(IPadd, Port);
        }

        /// <summary>
        /// Konstruktor klasy Serwer posiadający jako argumenty adres IP i numer portu
        /// </summary>
        /// <param IPadd="adresIP"></param>
        /// <param Port="port"></param>
        public Serwer(IPAddress adresIP, int port)
        {
            IPadd = adresIP;
            Port = port;
            buffor = new byte[1024];
            serwer = new TcpListener(IPadd, Port);
        }

        /// <summary>
        /// Metoda Start() uruchamia serwer TCP, łączy się z klientem (podany adres IP i port) i wysyła polecenie
        /// </summary>
        public void Start()
        {
            serwer.Start();
            Console.WriteLine("Utworzono serwer TCP, trwa oczekiwanie na klienta. ");
            klient = serwer.AcceptTcpClient();
            Console.WriteLine("Połączono z klientem. ");
            string pole = "Podaj wartosc kata alpha: ";
            byte[] bytes = Encoding.ASCII.GetBytes(pole);
            klient.GetStream().Write(bytes, 0, 26);
            while (true)
            {
                if (klient.Connected)
                {
                   

                    //Zczytanie informacji od klienta
                    klient.GetStream().Read(buffor, 0, buffor.Length);
                    string koniec = ASCIIEncoding.ASCII.GetString(buffor, 0, buffor.Length);

                    //Zabezpieczenie przed powtarzaniem
                   
                    if (buffor[0] != 13 && buffor[0] != 0)
                    {
                        //Zmiana zczytanej wartości na stringa, a następnie na doubla
                        string kat = ASCIIEncoding.ASCII.GetString(buffor, 0, buffor.Length);

                        //Obliczanie sinusa
                        string wynik = Sinus(kat);

                        //Odesłanie informacji do klienta
                        buffor = Encoding.ASCII.GetBytes("Wartosc sinus" + kat + "* = " + wynik+"\n");
                        klient.GetStream().Write(buffor, 0, buffor.Length);
                      
                        klient.GetStream().Write(bytes, 0, 26);
                        Array.Clear(buffor, 0, buffor.Length);

                       
                    }

                }
                
            }
        }
        /// <summary>
        /// Obliczanie sinusa podanego kąta
        /// </summary>
        /// <param name="wartosc"></param>
        /// <returns></returns>
        public string Sinus(string wartosc)
        {

            double kat = Convert.ToDouble(wartosc);
            double angle = Math.PI * kat / 180.0;
            double sinAngle = Math.Sin(angle);
            string wynik = sinAngle.ToString("0.000000");
            return wynik;
        }
    }
}
