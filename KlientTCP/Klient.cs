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
        bool czyZmianaHasla = false;
        bool czyZalogowany = true;
        bool czyKoniec = false;
        bool czyMenu = false;
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
                    odpowiedz = "ok";
                    dane = System.Text.Encoding.ASCII.GetBytes(odpowiedz);
                    strumien.Write(dane, 0, dane.Length);
                    Console.WriteLine(wiadomosc);
                    goto Odbior;
                }
                Console.WriteLine(wiadomosc);

                //wysylanie

                odpowiedz = Console.ReadLine();

                if (odpowiedz.Equals("koniec"))
                {
                    strumien.Close();
                    czyKoniec = true;
                    break;
                }
                else if (odpowiedz == "")
                {
                    odpowiedz = "ok";
                }

                dane = System.Text.Encoding.ASCII.GetBytes(odpowiedz);
                strumien.Write(dane, 0, dane.Length);
            }
        }

        void PanelLogowania(NetworkStream strumien)
        {
            int odczyt;
            while (!czyZalogowany)
            {
                Odbior:
                dane = new Byte[1024];
                // odebranie danych od serwera.
                odczyt = strumien.Read(dane, 0, dane.Length);
                wiadomosc = System.Text.Encoding.ASCII.GetString(dane, 0, odczyt);
                Console.WriteLine(wiadomosc);

                //przypadek występuje po poprawnym zalogowaniu lub poprawnym dodaniu użytkownika
                if (wiadomosc.Contains("Poprawnie"))
                {
                    if (wiadomosc.Contains("nowego uzytkownika"))
                    {
                        czyZalogowany = true;
                        czyMenu = false;
                        dane = new Byte[1024];
                        dane = System.Text.Encoding.ASCII.GetBytes("ok");
                        strumien.Write(dane, 0, dane.Length);
                        Console.Clear();
                        break;
                    }
                    czyZalogowany = true;
                    dane = new Byte[1024];
                    dane = System.Text.Encoding.ASCII.GetBytes("ok");
                    strumien.Write(dane, 0, dane.Length);
                    Console.Clear();
                    break;

                }
                else if (wiadomosc.Contains("sprobuj ponownie"))
                {
                    dane = new Byte[1024];
                    dane = System.Text.Encoding.ASCII.GetBytes("ok");
                    strumien.Write(dane, 0, dane.Length);
                    goto Odbior;
                }


                //czyszczenie bufora po odebraniu wiadomosci od serwera
                dane = new Byte[1024];
                odpowiedz = String.Empty;

                // przekazanie hasła z ziarnem dla 
                if (wiadomosc.Contains("haslo"))
                {

                    odpowiedz = Console.ReadLine();
                    if (odpowiedz == "")
                    {
                        odpowiedz = "ok";
                    }
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
                    odpowiedz = string.Join("|", haslo_zahaszowane);
                    dane = System.Text.Encoding.ASCII.GetBytes(odpowiedz);
                    strumien.Write(dane, 0, dane.Length);
                    goto Odbior;
                }


                //wysylanie wiadomosci
                odpowiedz = Console.ReadLine();
                if (odpowiedz.Equals("koniec"))
                {
                    strumien.Close();
                    czyKoniec = true;
                    return;
                    
                }
                else if (odpowiedz == "")
                {
                    odpowiedz = "ok";
                }

                dane = System.Text.Encoding.ASCII.GetBytes(odpowiedz);
                strumien.Write(dane, 0, dane.Length);
                
            }

            while(!czyZmianaHasla)
            {
                Odbior:
                dane = new Byte[1024];
                // odebranie danych od serwera.
                odczyt = strumien.Read(dane, 0, dane.Length);
                wiadomosc = System.Text.Encoding.ASCII.GetString(dane, 0, odczyt);
                Console.WriteLine(wiadomosc);

                if(wiadomosc.Contains("Wybrano 1 - Przejscie do aplikacji"))
                {
                    czyZmianaHasla = true;
                    dane = new Byte[1024];
                    dane = System.Text.Encoding.ASCII.GetBytes("ok");
                    strumien.Write(dane, 0, dane.Length);
                    break;
                }
                else if(wiadomosc.Contains("Poprawnie zmieniono haslo"))
                {
                    czyZmianaHasla = true;
                    dane = new Byte[1024];
                    dane = System.Text.Encoding.ASCII.GetBytes("ok");
                    strumien.Write(dane, 0, dane.Length);
                    break;
                }
                // przekazanie hasła z ziarnem dla 
                else if (wiadomosc.Contains("haslo"))
                {

                    odpowiedz = Console.ReadLine();
                    if (odpowiedz == "")
                    {
                        odpowiedz = "ok";
                    }
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
                    odpowiedz = string.Join("|", haslo_zahaszowane);
                    dane = System.Text.Encoding.ASCII.GetBytes(odpowiedz);
                    strumien.Write(dane, 0, dane.Length);
                    goto Odbior;
                }
                odpowiedz = Console.ReadLine();
                dane = System.Text.Encoding.ASCII.GetBytes(odpowiedz);
                strumien.Write(dane, 0, dane.Length);

            }

   
        }

        void Menu(NetworkStream strumien)
        {
            int odczyt;
            while (!czyMenu)
            {
                dane = new Byte[1024];
                // odebranie danych od serwera.
                odczyt = strumien.Read(dane, 0, dane.Length);
                wiadomosc = System.Text.Encoding.ASCII.GetString(dane, 0, odczyt);
                Console.WriteLine(wiadomosc);

                if (wiadomosc.Contains("Wybrano"))
                {
                    dane = System.Text.Encoding.ASCII.GetBytes("ok");
                    strumien.Write(dane, 0, dane.Length);
                    czyMenu = true;
                    czyZalogowany = false;

                    Console.Clear();
                    break;
                }

                odpowiedz = Console.ReadLine();
                if (odpowiedz.Equals("koniec"))
                {
                    dane = System.Text.Encoding.ASCII.GetBytes(odpowiedz);
                    strumien.Write(dane, 0, dane.Length);
                    strumien.Close();
                    czyKoniec = true;
                    break;
                }
                else if(odpowiedz == "")
                {
                    odpowiedz = "ok";
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

            while(!czyKoniec)
            {
                Menu(strumien);

                PanelLogowania(strumien);

                Polaczenie(strumien);
            }

            // kończenie połączenia
            klient.Close();

            Console.WriteLine("Nacisnij Enter aby zakonczyc...");
            Console.Read();
        }
    }
}