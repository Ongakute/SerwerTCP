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
        int ziarno;
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




        public void Polaczenie(NetworkStream strumien)
        {


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

                if (odpowiedz.Equals("koniec"))
                {
                    break;
                }

                dane = System.Text.Encoding.ASCII.GetBytes(odpowiedz);
                strumien.Write(dane, 0, dane.Length);
            }
        }
        /// <summary>
        /// Metoda odpowiedzialna za połączenie klienta z serwerem.
        /// </summary>
        public void Polacz()
        {


            klient.Connect(ip, port);
            NetworkStream strumien = klient.GetStream();

            //inicjalizacja połączenia przez klienta
            dane = System.Text.Encoding.ASCII.GetBytes("ok.");
            strumien.Write(dane, 0, dane.Length);
            dane = new Byte[1024];

            int odczyt = strumien.Read(dane, 0, dane.Length);
            wiadomosc = System.Text.Encoding.ASCII.GetString(dane, 0, odczyt);
            ziarno = Int32.Parse(wiadomosc);

            dane = System.Text.Encoding.ASCII.GetBytes("ok.");
            strumien.Write(dane, 0, dane.Length);
            dane = new Byte[1024];

            while (!czyZalogowany)
            {
            Odbior:
                dane = new Byte[1024];
                // odebranie danych od serwera.
                odczyt = strumien.Read(dane, 0, dane.Length);
                wiadomosc = System.Text.Encoding.ASCII.GetString(dane, 0, odczyt);
                Console.WriteLine(wiadomosc);

                //przypadek występuje po poprawnym zalogowaniu 
                if (wiadomosc.Equals("Poprawnie zalogowano. \r\n"))
                {
                    czyZalogowany = true;
                    dane = new Byte[1024];
                    dane = System.Text.Encoding.ASCII.GetBytes("ok");
                    strumien.Write(dane, 0, dane.Length);
                    Console.Clear();
                    break;
                }
                else if (wiadomosc.Contains("Niepoprawny login"))
                {
                    dane = new Byte[1024];
                    dane = System.Text.Encoding.ASCII.GetBytes("ok");
                    strumien.Write(dane, 0, dane.Length);
                    goto Odbior;
                }


                //czyszczenie bufora po odebraniu wiadomosci od serwera
                dane = new Byte[1024];

                odpowiedz = String.Empty;


                if (wiadomosc.Contains("haslo"))
                {

                    odpowiedz = Console.ReadLine();
                    //char[] tymczasowy = odpowiedz.ToCharArray();
                    List<int> haslo_zahaszowane = new List<int>();
                    int wartosc_znaku;
                    for (int i = 0; i < odpowiedz.Length; i++)
                    {
                        wartosc_znaku = Char.ConvertToUtf32(odpowiedz, i) * ziarno;
                        haslo_zahaszowane.Add(wartosc_znaku);
                        //tymczasowy[i] = Convert.ToChar(Char.ConvertToUtf32(odpowiedz, i) * ziarno);
                    }
                    //odpowiedz = tymczasowy.ToString();
                    odpowiedz = string.Join("", haslo_zahaszowane);
                    dane = System.Text.Encoding.ASCII.GetBytes(odpowiedz);
                    strumien.Write(dane, 0, dane.Length);
                    goto Odbior;
                }
                //wysylanie wiadomosci
                odpowiedz = Console.ReadLine();
                if (odpowiedz.Equals("koniec"))
                {
                    break;
                }

                dane = System.Text.Encoding.ASCII.GetBytes(odpowiedz);
                strumien.Write(dane, 0, dane.Length);
            }

            Polaczenie(strumien);

            // kończenie połączenia
            strumien.Close();
            klient.Close();

            Console.WriteLine("\n Nacisnij Enter aby zakonczyc...");
            Console.Read();
        }
    }
}