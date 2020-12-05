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
            Klient klient = new Klient();
            klient.Polacz();
        }


    }
}
