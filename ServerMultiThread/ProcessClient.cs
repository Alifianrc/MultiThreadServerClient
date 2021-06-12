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
            Thread clientThread = new Thread(SendMassage);
            clientThread.Start();
        }

        private void SendMassage()
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
                    // This size is static, make a big gap between massage
                    networkStream.Read(byteReceived, 0, 1024);

                    // Processing received data
                    dataReceived = Encoding.ASCII.GetString(byteReceived);
                    //dataReceived = dataReceived.Substring(0, dataReceived.IndexOf("$"));
                    string massage = "From Client-" + Id + " : " + dataReceived;
                    // Write massage in console
                    Console.WriteLine(massage);

                    // Save massage every single time
                    SaveMassage(massage);

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

        public void SaveMassage(string newMassage)
        {
            // Load old massage from txt
            List<string> MassageList = new List<string>();
            try
            {
                MassageList = File.ReadAllLines(filePath).ToList();
            }
            catch(Exception e)
            {
                Console.WriteLine("Save Error : " + e);
            }

            // Add to list
            MassageList.Add(newMassage);

            // Save all
            File.WriteAllLines(filePath, MassageList);
        }
    }
}
