using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace socket
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectToServer();
        }
        public static void ConnectToServer(String val = "JOIN#")
        {
            System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();



            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 6000);

            try
            {
                if (!clientSocket.Connected)
                    //clientSocket.Connect(serverIP, port);

                    clientSocket.Connect(localEndPoint);

                NetworkStream serverStream = clientSocket.GetStream();
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(val);
                serverStream.Write(outStream, 0, outStream.Length);



                //byte[] inStream = new byte[clientSocket.ReceiveBufferSize];
                //serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
                //string returndata = System.Text.Encoding.ASCII.GetString(inStream);

                //MessageBox.Show(returndata);


                serverStream.Flush();
                serverStream.Close();

                //var count = serverStream.Read(inStream, 0, inStream.Length);
                //string returndata = Encoding.ASCII.GetString(inStream, 0, count);

                ListenToServer();

            }
            catch (Exception m)
            {

                Console.WriteLine(m.Message);
            }

            Console.ReadKey();



        }



        private static void ListenToServer()
        {
            const int PORT_NO = 7000;
            const string SERVER_IP = "127.0.0.1";


            //---listen at the specified IP and port no.---
            IPAddress localAdd = IPAddress.Parse(SERVER_IP);
            TcpListener listener = new TcpListener(localAdd, PORT_NO);

            listener.Start();


            //---incoming client connected---
            TcpClient client = listener.AcceptTcpClient();

            //---get the incoming data through a network stream---
            NetworkStream nwStream = client.GetStream();
            byte[] buffer = new byte[client.ReceiveBufferSize];

            //---read incoming stream---
            int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

            //---convert the data received into a string---
            string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Received : " + dataReceived);



            //Read user commands
            ConsoleKeyInfo key = Console.ReadKey();

            if (key.Key == ConsoleKey.UpArrow)
            {

                Console.WriteLine("Sending back : " + "UP#");
                ConnectToServer("UP#");
            }

            if (key.Key == ConsoleKey.DownArrow)
            {

                Console.WriteLine("Sending back : " + "DOWN#");
                ConnectToServer("UP#");
            }


            if (key.Key == ConsoleKey.LeftArrow)
            {

                Console.WriteLine("Sending back : " + "LEFT#");
                ConnectToServer("UP#");
            }


            if (key.Key == ConsoleKey.RightArrow)
            {

                Console.WriteLine("Sending back : " + "RIGHT#");
                ConnectToServer("UP#");
            }

            if (key.Key == ConsoleKey.Enter)
            {

                Console.WriteLine("Sending back : " + "SHOOT#");
                ConnectToServer("UP#");
            }




        }
    }
}
