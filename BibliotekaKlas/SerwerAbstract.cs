using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;


namespace BibliotekaKlas
{
    public abstract class SerwerAbstract
    {
        #region pola
        IPAddress IPAddress;
        int port;
        int buffer_size;
        byte[] buffer;
        bool running;
        TcpListener tcpListener;
        TcpClient tcpClient;
        NetworkStream stream;
        #endregion

        #region Wlasciwosci


        protected TcpListener TcpListener { get => tcpListener; set => tcpListener = value; }
        protected TcpClient TcpClient { get => tcpClient; set => tcpClient = value; }
        protected NetworkStream Stream { get => stream; set => stream = value; }
        protected byte[] Buffer { get => buffer; set => buffer = value; }

        protected int Buffer_size { get => buffer_size; set => buffer_size = value; }

        protected bool Running { get => running; set => running = value; }

        #endregion

        public SerwerAbstract()
        {

            IPAddress = IPAddress.Parse("127.0.0.1");
            port = 7777;
            Buffer = new byte[1024];
            Buffer_size = 1024;
            Running = false;
        }

        public SerwerAbstract(IPAddress IP, int port)
        {
            this.IPAddress = IP;
            this.port = port;
            Buffer = new byte[1024];
            Buffer_size = 1024;
            Running = false;
        }

        protected bool checkPort()
        {
            if (port < 1024 || port > 49151) return false;
            return true;
        }

        protected void StartListening()
        {
            tcpListener = new TcpListener(IPAddress, port);
            tcpListener.Start();
            Console.Out.WriteLine("Rozpoczeto oczekiwanie na klienta\n");
        }


        protected abstract void AcceptClient();
        protected abstract void BeginDataTransmission(NetworkStream stream);
        public abstract void Start();



    }


}
