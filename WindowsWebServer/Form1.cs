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
        // Threadsafe IsRunning flag to keep track of running state
        private bool _isRunning;
        static readonly object _isRunningLock = new object();  
        public bool IsRunning
        {
            get
            {
                lock (_isRunningLock)
                {
                    return this._isRunning;
                }
            }
            set
            {
                lock (_isRunningLock)
                {
                    this._isRunning = value;
                }
                button1.Enabled = !value;
                button2.Enabled = value;
            }
        }

        public Form1()
        {

            InitializeComponent();
            button2.Enabled = false;

        }

        //Set button2 click properties
        private void Button1_Click(object sender, EventArgs e)
        {
            //Deactivate button1 and activate button2
            button1.Enabled = false;
            button2.Enabled = true;

            IsRunning = true;
            ThreadPool.QueueUserWorkItem(SetBackGroundData, this.IsRunning);

            // Start the main object's listener
            main.listener = new TcpListener(IPAddress.Any, main.port);
            main.listener.Start();
            while (main.IsRunning) //Run until we're told to exit
            {
                try
                {
                    // Each new request to the listener get a TcpClient
                    TcpClient lClient = main.listener.AcceptTcpClient();
                    // Send that TcpClient to its own thread to process
                    ThreadPool.QueueUserWorkItem(GetRequestedItem, lClient);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error received: " + ex.Message);
                }
            }

        }

        //Set button click properties
        private void Button2_Click(object sender, EventArgs e)
        {
            //Deactivate button 2 and activate button1
            button1.Enabled = true;
            button2.Enabled = false;

            IsRunning = false;
        }

        //Set Background Data
        public void StartListene(Object info)
        {
            if (info == null)
                return;

            fMain main = (fMain)info;
            string response = String.Empty;
            int nBytes = 0;

            while (main.IsRunning)
            {
                try
                {
                    TcpClient client = main.listener.AcceptClient;
                    ThreadPool.QueueUserWorkItem(GetUserItem, client);
                }
                catch (Exception ex){
                    Console.WriteLine("Error received" + ex.Message);
                }
            }

           // Thread.Sleep(5000);
        }

        

       
    }
}