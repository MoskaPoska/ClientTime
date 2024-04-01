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
            // Создаем сокет
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            // Устанавливаем IP-адрес и порт для прослушивания
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 1040);
            socket.Bind(endPoint);

            // Запускаем прослушивание входящих пакетов
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        // Получаем данные от клиента
                        EndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                        byte[] buff = new byte[1024];
                        int bytesRead = socket.ReceiveFrom(buff, ref clientEndPoint);

                        // Преобразуем полученные данные в строку
                        string time = Encoding.Default.GetString(buff, 0, bytesRead);

                        // Обновляем метку с временем на форме
                        UpdateTimeLabel(time);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при получении данных: {ex.Message}");
                    }
                }
            });

            // Устанавливаем заголовок окна
            Text = "Server is working";
        }

        private void UpdateTimeLabel(string time)
        {
            // Обновляем метку с временем на форме
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