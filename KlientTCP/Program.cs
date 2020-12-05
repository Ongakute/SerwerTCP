using System;
using System.Net.Sockets;

namespace KlientTCP
{
    class Program
    {

        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        static void Main(string[] args)
        {
            String ip, port;

            Console.Write("Podaj adres IP serwera: ");
            ip = Console.ReadLine();
            Console.Write("Podaj port serwera: ");
            port = Console.ReadLine();

            Klient klient = new Klient(ip, Int32.Parse(port));
            Console.Clear();
            klient.Polacz();
        }


    }
}
