using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    public class SocketClient : Client
    {
        public SocketClient(string IP, int port, string HandTxtAcc, string HandTxtRec)
        {
            this.HandTxtRec = HandTxtRec;
            this.HandTxtAcc = HandTxtAcc;
            this.iP = IP;
            this.port = port;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public long max = 0;
        public long current;
        public string nameFile = "";
        public bool Connect()//
        {
            if (port > 0)
            {
                try
                {
                    socket.Connect(new IPEndPoint(IPAddress.Parse(iP), port));
                    Connected = true;
                    return Handshake();
                }
                catch { }
            }
            Disconect();
            return false;
        }
        public bool Handshake()//
        {
            if (ABSSend(Encoding.Unicode.GetBytes(HandTxtAcc), 1))
            {
                if (Encoding.Unicode.GetString(ABSReceive()) == HandTxtRec)
                {
                    return true;
                }
            }
            return false;
        }
        public bool loading { get; private set; } = false;
        public void Stop()
        {
            loading = false;
        }
        public async void Loading(string Name)//Загрузка на стороне клиента
        {
            max = 0;
            current = 1;
            loading = true;
            await Task.Run(() =>
            {
                byte[] b = ABSReceive();
                nameFile = this.ByteToString(b);
                FileStream file = new FileStream($"{Name}\\{nameFile}", FileMode.OpenOrCreate);
                ABSSend(BitConverter.GetBytes(file.Length), 5);//циферки в байты
                file.Position = file.Length;//
                long Max = BitConverter.ToInt64(ABSReceive(), 0);
                byte[] bytes;
                while (true)
                {
                    if (loading) { ABSSend(SystemSC.bytes, 10); }
                    else { ABSSend(SystemSC.bytes, 5); break; }
                    if ((Max - file.Position) > 102400000)
                    {
                        bytes = ABSReceive();
                        file.Write(bytes, 0, bytes.Length);
                        max = Max;
                        current = file.Position;
                    }
                    else
                    {
                        bytes = ABSReceive();
                        file.Write(bytes, 0, bytes.Length);
                        max = Max;
                        current = file.Position;
                        break;
                    }
                }
                file.Close();
                Stop();
                //max = current;
            });
        }
    }
}
