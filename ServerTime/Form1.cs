using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ServerTime
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // ������� �����
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            // ������������� IP-����� � ���� ��� �������������
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 1040);
            socket.Bind(endPoint);

            // ��������� ������������� �������� �������
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        // �������� ������ �� �������
                        EndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                        byte[] buff = new byte[1024];
                        int bytesRead = socket.ReceiveFrom(buff, ref clientEndPoint);

                        // ����������� ���������� ������ � ������
                        string time = Encoding.Default.GetString(buff, 0, bytesRead);

                        // ��������� ����� � �������� �� �����
                        UpdateTimeLabel(time);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"������ ��� ��������� ������: {ex.Message}");
                    }
                }
            });

            // ������������� ��������� ����
            Text = "Server is working";
        }

        private void UpdateTimeLabel(string time)
        {
            // ��������� ����� � �������� �� �����
            if (label1.InvokeRequired)
            {
                label1.Invoke(new Action<string>(UpdateTimeLabel), time);
            }
            else
            {
                label1.Text = time;
            }
        }
    }
}