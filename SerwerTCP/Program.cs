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
            BibliotekaKlas.Serwer poloczenie = new Serwer();
            poloczenie.Start();
           
            
        }
    }
}
