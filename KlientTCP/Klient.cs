using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace KlientTCP
{

    public class Klient
    {

        String ip;
        int port;
        bool czyZalogowany = false;
        bool czyOpcja = false;
        Byte[] dane = new Byte[1024];
        String wiadomosc = String.Empty;
        String odpowiedz = String.Empty;
        TcpClient klient = new TcpClient();

        public Klient(String ip, int port)
        {
            this.ip = ip;
            this.port = port;
         }
        
        
        
        

        /// <summary>
        /// Metoda odpowiedzialna za połączenie klienta z serwerem.
        /// </summary>
        public void Polacz()
        {
            
            
            klient.Connect(ip,port);
            NetworkStream strumien = klient.GetStream();

            //inicjalizacja połączenia przez klienta
            dane = System.Text.Encoding.ASCII.GetBytes("ok.");
            strumien.Write(dane, 0, dane.Length);
            dane = new Byte[1024];


            while (!czyZalogowany)
            {

                dane = new Byte[1024];
                // odebranie danych od serwera.
                int odczyt = strumien.Read(dane, 0, dane.Length);
                wiadomosc = System.Text.Encoding.ASCII.GetString(dane, 0, odczyt);
                Console.WriteLine(wiadomosc);

                //przypadek występuje po poprawnym zalogowaniu 
                if (wiadomosc.Equals("Poprawnie zalogowano. \r\n"))
                {
                    czyZalogowany = true;
                    dane = new Byte[1024];
                    dane = System.Text.Encoding.ASCII.GetBytes("ok");
                    strumien.Write(dane, 0, dane.Length);
                    break;
                }
                

                //czyszczenie bufora po odebraniu wiadomosci od serwera
                dane = new Byte[1024];

                odpowiedz = String.Empty;

                //wysylanie wiadomosci
                odpowiedz = Console.ReadLine();
                dane = System.Text.Encoding.ASCII.GetBytes(odpowiedz);
                strumien.Write(dane, 0, dane.Length);
            }

            while (czyZalogowany)
            {
                Odbior:
                dane = new Byte[1024];
                odpowiedz = String.Empty;

                // odebranie wiadomosci od serwera
                int odczyt = strumien.Read(dane, 0, dane.Length);
                wiadomosc = System.Text.Encoding.ASCII.GetString(dane, 0, odczyt);

                //skończona iteracja działania aplikacji obsługiwanej przez serwer
                if (wiadomosc.Contains(":/"))
                {

                    Console.WriteLine(wiadomosc);

                    odpowiedz = "ok";
                    dane = System.Text.Encoding.ASCII.GetBytes(odpowiedz);
                    strumien.Write(dane, 0, dane.Length);
                    goto Odbior;
                }
                Console.WriteLine(wiadomosc);

                //wysylanie

                odpowiedz = Console.ReadLine();
                dane = System.Text.Encoding.ASCII.GetBytes(odpowiedz);
                strumien.Write(dane, 0, dane.Length);
            }
            // kończenie połączenia
            strumien.Close();
            klient.Close();

            Console.WriteLine("\n Nacisnij Enter aby zakonczyc...");
            Console.Read();
        }
    }
}

