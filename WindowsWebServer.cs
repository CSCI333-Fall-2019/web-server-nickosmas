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

        private void Button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;

            ThreadPool.QueueUserWorkItem(SetBackGroundData, this);

        }

        //SetForeGroundData

        public void SetBackGroundData(Object value)
        {
            Thread.Sleep(5000);
        }


        private void Button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
        }
    }
}
