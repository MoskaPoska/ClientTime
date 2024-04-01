using System.Net.Sockets;
using System.Net;
using System.Text;

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

            Task.Run(async () =>
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);
                IPAddress address = Dns.GetHostAddresses(Dns.GetHostName())[2];
                IPEndPoint endPoint = new IPEndPoint(address, 1040);
                socket.Bind(endPoint);
                EndPoint point = new IPEndPoint(IPAddress.Any, 1040);

                byte[] buff = new byte[1024];
                do
                {
                    await socket.ReceiveFromAsync(buff, SocketFlags.None, point).ContinueWith(t =>
                    {
                        SocketReceiveFromResult res = t.Result;
                        timer1.Interval += 1000;                       
                        string timer = Encoding.Default.GetString(buff);
                        label1.BeginInvoke(new Action<string>(s => { label1.Text = s; }), timer);
                    });
                } while (true);
            });
            Text = "Server is working";
        }
    }
}
