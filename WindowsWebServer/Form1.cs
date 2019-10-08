using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsWebServer
{
    public partial class Form1 : Form
    {

        public Form1()
        {

            InitializeComponent();
            button2.Enabled = false;

        }

        //Set button2 click properties
        private void Button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;

            ThreadPool.QueueUserWorkItem(SetBackGroundData, this);

        }

        //Set button click properties
        private void Button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
        }

        //Set Background Data
        public void SetBackGroundData(Object value)
        {
            Thread.Sleep(5000);
        }

        private void StartTcpListener(object arg)
        {
            TcpListener listener;
            int port = 5326;
            bool IsRunning = (bool)arg;

            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            while (IsRunning)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();

                    ThreadPool.QueueUserWorkItem(GetRequestedItem, client);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error received: " + ex.Message);
                }
            }
        }

        private void GetWebData(object input)
        {
            try
            {
                string strInput = input.ToString();
                // Response Data initialization
                Byte[] output = new Byte[1024];
                string response = String.Empty;
                // Create a new TCP Client
                var tcpClient = new TcpClient(strInput, 80);
                NetworkStream ns = tcpClient.GetStream();
                Console.WriteLine("Connected");

                // Write a request to the web server
                Byte[] cmd = System.Text.Encoding.ASCII.GetBytes("GET / HTTP/1.0 \nHost: " + strInput + "\n\n");
                ns.Write(cmd, 0, cmd.Length);
                // Get the output
                Int32 bytes = ns.Read(output, 0, output.Length);
                while (bytes > 0)
                {
                    response += System.Text.Encoding.ASCII.GetString(output);
                    bytes = ns.Read(output, 0, output.Length);
                }
                Thread.Sleep(5000);
                SetForegroundData(response);
            }
            catch (Exception Ex)
            {
                SetForegroundData(Ex.Message);
            }
        }
    }
}