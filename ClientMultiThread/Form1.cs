using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace ClientMultiThread
{
    public partial class Form1 : Form
    {
        TcpClient mySocket = new TcpClient();
        NetworkStream ServerStream;

        int DataSize = 2028;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        { 
            // Alwasy try to connecting if failed to connect
            while (!mySocket.Connected)
            {
                mySocket.Connect(IPAddress.Loopback, 88);
                ServerStream = mySocket.GetStream();
            }

            Printf("Client Begin!");
            // Thread for receiving massage
            Thread receiveThread = new Thread(RecieveMassage);
            receiveThread.Start();
        }

        private void RecieveMassage()
        {
            while (mySocket.Connected)
            {
                byte[] ReceiveData = new byte[DataSize];
                string ReceiveMassage = null;
                try
                {
                    // Try to receive stream from server
                    ServerStream.Read(ReceiveData, 0, DataSize);
                }
                catch (Exception e)
                {
                    Printf("Error Stream : " + e);
                }
                // Get the massage
                ReceiveMassage = Encoding.ASCII.GetString(ReceiveData);
                // Print it
                Printf(ReceiveMassage);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Send Massage
            byte[] SendData = Encoding.ASCII.GetBytes(textBox2.Text); //Printf("Debug : " + textBox2.Text); //<-Save
            ServerStream.Write(SendData, 0, SendData.Length);
            ServerStream.Flush();

            // Reset textbox
            textBox2.Clear();
        }

        // Print anything in textbox 1
        // Little complicated because of Thread
        delegate void SetTextCallBack(string massage);
        private void Printf(string massage)
        {
            if (this.InvokeRequired)
            {
                SetTextCallBack temp = new SetTextCallBack(Printf);
                this.Invoke(temp, new object[] { massage });
            }
            else if (massage != "")
            {
                textBox1.Text = textBox1.Text + Environment.NewLine + "-> " + massage;
            }
        }
    }
}
