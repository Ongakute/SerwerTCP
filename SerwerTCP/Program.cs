using BibliotekaKlas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SerwerTCP
{
    class Program
    {
        static void Main(string[] args)
        {
            Serwer a = new Serwer(IPAddress.Parse("127.0.0.1"), 7777);
            a.Start();
           
            
        }
    }
}
