using System;
using System.Net.Sockets;
using System.Text;
using ServerMultiThread;
using System.Collections.Generic;
using System.Net;

namespace ServerMultiThread
{
    class Server
    {
        static void Main()
        {
            Console.Title = "Server";

            TcpListener ServerSocket = new TcpListener(new IPEndPoint(IPAddress.Any, 88));
            List<TcpClient> ClientSocketList = new List<TcpClient>();
 
            // Total amount of client
            int clientCount = 0;

            // Start server
            ServerSocket.Start();
            Console.WriteLine("Server started...");

            // Looping for accepting several client
            while (true)
            {
                clientCount++;
                TcpClient tempSocket = ServerSocket.AcceptTcpClient();
                ClientSocketList.Add(tempSocket);
                string confrimMassage = "Client No: " + Convert.ToString(clientCount) + " connected!";
                Console.WriteLine(confrimMassage);

                //Class to handle each client
                ServerMultiThread.ProcessClient client = new ProcessClient();
                client.SaveMassage(confrimMassage);
                client.StartClient(tempSocket, Convert.ToString(clientCount), ClientSocketList);
            }

            // Just in case exiting from loop
            foreach(TcpClient socket in ClientSocketList)
            {
                socket.Close();
            }
            ServerSocket.Stop();
            Console.WriteLine("Exit Loop Program");
            Console.ReadLine();
        }
    }
}
