using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ClientTime
{
    public partial class Form1 : Form
    {
        private readonly UdpClient udpClient;
        private readonly IPEndPoint serverEndPoint;

        public Form1()
        {
            InitializeComponent();
            udpClient = new UdpClient();
            serverEndPoint = new IPEndPoint(IPAddress.Parse("192.168.56.1"), 1040);
            Process.Start("ServerTime.exe");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Tick += timer1_Tick;
            timer1.Start();
        }      
        private void timer1_Tick(object sender, EventArgs e)
        {
            string time = $"{textBox1.Text}:{textBox2.Text}:{textBox3.Text}";
            byte[]sendBuff = Encoding.Default.GetBytes(time);
            udpClient.Send(sendBuff, sendBuff.Length, serverEndPoint);
        }
    }
}