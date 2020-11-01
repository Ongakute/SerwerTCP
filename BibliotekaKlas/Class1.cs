using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace BibliotekaKlas
{
    public class Serwer : SerwerAbstract
    {
        Dictionary<String, String> uzytkownicy;

        public delegate void TransmisionDataDelegate(NetworkStream nwStream);
        public Serwer(IPAddress IP, int Port) : base(IP, Port)
        {
            uzytkownicy = new Dictionary<String, String>();
            uzytkownicy.Add("admin", "admin");
            uzytkownicy.Add("anna", "cheetos");
            uzytkownicy.Add("nowy", "nowehaslo");

        }

        protected override void AcceptClient()
        {
            while (true)
            {
                TcpClient client = TcpListener.AcceptTcpClient();
                Stream = client.GetStream();
                Console.WriteLine("Połączono z klientem. ");
                int bufor_size = client.ReceiveBufferSize;
                TransmisionDataDelegate transmisionDelegate = new TransmisionDataDelegate(BeginDataTransmission);
                transmisionDelegate.BeginInvoke(Stream, TransmissionCallback, client);
            }
        }

        private void TransmissionCallback(IAsyncResult a)
        {
            Console.WriteLine("Koniec polaczenia");
        }



        /*

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
                }*/

        public string Sinus(string wartosc)
        {

            double kat = Convert.ToDouble(wartosc);
            double angle = Math.PI * kat / 180.0;
            double sinAngle = Math.Sin(angle);
            string wynik = sinAngle.ToString("0.000000");
            return wynik;
        }



        protected override void BeginDataTransmission(NetworkStream stream)
        {

            Buffer = Encoding.ASCII.GetBytes("Witaj!");
            stream.Write(Buffer, 0, Buffer.Length);
            Array.Clear(Buffer, 0, Buffer.Length);

            while (true)
            {
                zaloguj(stream);
                Buffer = Encoding.ASCII.GetBytes("Jestes zalogowany");
                stream.Write(Buffer, 0, Buffer.Length);
                Array.Clear(Buffer, 0, Buffer.Length);

                while (true)
                {
                    try
                    {
                        Buffer = new byte[Buffer_size];
                        int message_size = stream.Read(Buffer, 0, Buffer_size);


                        String wiad = ASCIIEncoding.ASCII.GetString(Buffer, 0, Buffer.Length);
                        if (wiad.ToString().Equals("koniec"))
                        {
                            Buffer = Encoding.ASCII.GetBytes("Koniec polaczenia.");
                            stream.Write(Buffer, 0, Buffer.Length);
                            Array.Clear(Buffer, 0, Buffer.Length);

                            break;
                        }
                        string pole = "\nPodaj wartosc kata alpha: ";
                        byte[] bytes = Encoding.ASCII.GetBytes(pole);
                        stream.Write(bytes, 0, 26);
                        Array.Clear(Buffer, 0, Buffer.Length);
                        while (true)
                        {
                            stream.Read(Buffer, 0, Buffer.Length);

                             wiad = ASCIIEncoding.ASCII.GetString(Buffer, 0, Buffer.Length);
                            if (wiad.ToString().Equals("koniec"))
                            {
                                Buffer = Encoding.ASCII.GetBytes("Koniec polaczenia.");
                                stream.Write(Buffer, 0, Buffer.Length);
                                Array.Clear(Buffer, 0, Buffer.Length);

                                break;
                            }
                            //Zabezpieczenie przed powtarzaniem

                            if (Buffer[0] != 13 && Buffer[0] != 0)
                            {
                                //Zmiana zczytanej wartości na stringa, a następnie na doubla
                                string kat = ASCIIEncoding.ASCII.GetString(Buffer, 0, Buffer.Length);

                                //Obliczanie sinusa
                                string wynik = Sinus(kat);

                                //Odesłanie informacji do klienta
                                Buffer = Encoding.ASCII.GetBytes("Wartosc sinus" + kat + "* = " + wynik + "\n");
                                stream.Write(Buffer, 0, Buffer.Length);
                                Array.Clear(Buffer, 0, Buffer.Length);
                                stream.Write(bytes, 0, 26);

                                Array.Clear(Buffer, 0, Buffer.Length);

                            }
                        }
                    }
                    catch (IOException e)
                    {
                        break;
                    }

                }
            }
        }

        protected void zaloguj(NetworkStream stream)
        {
          

            while (true)
            {
                try
                {

                    Buffer = Encoding.ASCII.GetBytes("\nPodaj login uzytkownika: ");
                    stream.Write(Buffer, 0, Buffer.Length);
                    Array.Clear(Buffer, 0, Buffer.Length);

                    Buffer = new byte[Buffer_size];

                    int message_size = stream.Read(Buffer, 0, Buffer_size);


                    if (Buffer[0] == 13)
                    {
                        message_size = stream.Read(Buffer, 0, Buffer_size);
                    }

                    String login = ASCIIEncoding.ASCII.GetString(Buffer, 0, Buffer.Length);
                  // Console.WriteLine(login); 
                    char[] wiadomosc = new char[message_size];
                    for (int i = 0; i < message_size; i++)
                    {
                        wiadomosc[i] = (char)Buffer[i];
                    }
                    String wynik = new String(wiadomosc);
                   
                    if (uzytkownicy.ContainsKey(wynik))
                    {
                        Buffer = Encoding.ASCII.GetBytes("\nPodaj haslo uzytkownika: ");
                        stream.Write(Buffer, 0, Buffer.Length);
                        Array.Clear(Buffer, 0, Buffer.Length);

                        Buffer = new byte[Buffer_size];
                      int   haslo_size = stream.Read(Buffer, 0, Buffer_size);
                        if (Buffer[0] == 13)
                        {
                            haslo_size = stream.Read(Buffer, 0, Buffer_size);
                        }
                           // String haslo = ASCIIEncoding.ASCII.GetString(Buffer, 0, Buffer.Length);
                            char[] haslo = new char[haslo_size];
                            for (int i = 0; i < haslo_size; i++)
                            {
                                haslo[i] = (char)Buffer[i];
                            }
                            String haslo2 = new String(haslo);
                            //Console.WriteLine(haslo2); 
                            String temp = "";

                            uzytkownicy.TryGetValue(wynik, out temp);
                       
                        // Console.WriteLine(uzytkownicy[login]);
                        if (temp == haslo2)
                            {
                                return;
                            }
                            else
                            {
                                Buffer = Encoding.ASCII.GetBytes("Zle haslo");
                                stream.Write(Buffer, 0, Buffer.Length);
                                Array.Clear(Buffer, 0, Buffer.Length);
                            }
                        }
                    
                    else
                    {
                        Buffer = Encoding.ASCII.GetBytes("Zly login");
                        stream.Write(Buffer, 0, Buffer.Length);
                        Array.Clear(Buffer, 0, Buffer.Length);
                    }


                }
                catch (IOException e)
                {
                    return;
                }
            }

        }

        public override void Start()
        {
            Console.WriteLine("Utworzono serwer TCP, trwa oczekiwanie na klienta. ");
            StartListening();
            AcceptClient();
        }
    }
}
