using SerwerTCPAsynch;
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
            SerwerTCPAsynch.SerwerAsynchroniczny<ProtokolSzyfrowania> serwer = new SerwerTCPAsynch.SerwerAsynchroniczny<ProtokolSzyfrowania>(IPAddress.Parse("127.0.0.1"), 2048);
            serwer.start();

        }
    }
}
