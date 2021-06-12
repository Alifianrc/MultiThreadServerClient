using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ServerMultiThread
{
    class ProcessClient
    {
        List<TcpClient> ClientSocketList;
        TcpClient mySocket;
        string Id;

        string filePath = @"D:\Tugas_Kuliah\Sistem_Oprasi_Jaringan_Komputer\FP_MultiClient\ServerMultiThread\Massage_Data.txt";

        // Act like constructor
        public void StartClient(TcpClient inClientSocket, string clineNo, List<TcpClient> socketList)
        {
            this.mySocket = inClientSocket;
            this.Id = clineNo;
            ClientSocketList = socketList;
            // Start a chatting Thread
            Thread clientThread = new Thread(SendMessage);
            clientThread.Start();
        }

        private void SendMessage()
        {
            byte[] byteReceived = new byte[1024];
            string dataReceived =  null;

            byte[] byteSend;
            string dataSend;

            int massageCount = 0;

            while (mySocket.Connected)
            {
                try
                {
                    massageCount++;
                    // Received data stream
                    NetworkStream networkStream = mySocket.GetStream();
                    //networkStream.Read(byteReceived, 0, (int)mySocket.ReceiveBufferSize);
                    // This size is static, make a big gap between message
                    networkStream.Read(byteReceived, 0, 1024);

                    // Processing received data
                    dataReceived = Encoding.ASCII.GetString(byteReceived);
                    //dataReceived = dataReceived.Substring(0, dataReceived.IndexOf("$"));
                    string message = "From Client-" + Id + " : " + dataReceived;
                    // Write massage in console
                    Console.WriteLine(message);

                    // Save massage every single time
                    SaveMessage(message);

                    // Send data to all client
                    dataSend = "Client-" + Id + " : " + dataReceived + " END";
                    byteSend = Encoding.ASCII.GetBytes(dataSend);
                    NetworkStream OtherStream;
                    foreach (TcpClient socket in ClientSocketList)
                    {
                        OtherStream = socket.GetStream();
                        OtherStream.Write(byteSend, 0, byteSend.Length);
                        OtherStream.Flush();
                    }

                    // Reset data
                    dataReceived = "";
                    byteReceived = new byte[1024];
                }
                catch (Exception e)
                {
                    Console.WriteLine("Process Client Error : " + e.ToString());
                }
            }

            // If disconnect
            Console.WriteLine("Client No: " + Id + " Disconnected");
            ClientSocketList.Remove(mySocket);
        }

        public void SaveMessage(string newMessage)
        {
            // Load old massage from txt
            List<string> MessageList = new List<string>();
            try
            {
                MessageList = File.ReadAllLines(filePath).ToList();
            }
            catch(Exception e)
            {
                Console.WriteLine("Save Error : " + e);
            }

            // Add to list
            MessageList.Add(newMessage);

            // Save all
            File.WriteAllLines(filePath, MessageList);
        }
    }
}
