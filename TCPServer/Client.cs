using System;
using System.Net.Sockets;
using System.Text;

namespace TCPServer
{
    public delegate void RemovalClient(Client client);
    public abstract class Client : IDisposable
    {
        protected Socket socket { get; set; }
        public event RemovalClient add;
        public event RemovalClient remova;
        protected string iP;
        protected int port = 0;
        public int BufferSize = 16;
        public bool Connected;
        public byte code;
        protected string HandTxtAcc { get; set; } 
        protected string HandTxtRec { get; set; }
        public bool Send(byte[] bytes, byte code)//Отправка байтов
        {
            if (Connected)
            {
                try
                {
                    bytes = Collecting(bytes, code);
                    socket.Send(bytes);
                    return true;
                }
                catch { Disconect(); }
            }
            return false;
        }
        private byte[] Collecting(byte[] bytes, byte code) // То что нужно собрать перед отправкой байтов
        {
            byte[] b = new byte[(bytes.Length+1)];
            b[0] = code;
            for (int i = 1; i < b.Length; i++) { b[i] = bytes[i - 1]; }
            return b;
        }
        public byte[] WaitingReceive() => WaitingReceive(BufferSize);
        public byte[] WaitingReceive(int BufferSize)//Ждём ответа от клиента или клиент от сервера
        {
            if (Connected)
            {
                byte[] data = new byte[BufferSize];
                try
                {
                    do 
                    {
                        socket.Receive(data);
                    }
                    while (socket.Available > 0);
                    //|| data.Length < 2 || data?[0] == 0
                    (data, code) = Disassemble(data);
                    return data;
                }
                catch { Disconect(); }
            }
            return null;
        }
        public (byte[],byte) Disassemble(byte[] bytes)//Разборка байтов и code
        {
            byte code = bytes[0];
            byte[] b = new byte[(bytes.Length - 1)];
            for (int i = 0;i < b.Length;i++ ) { b[i] = bytes[i + 1]; }
            return (b, code);
        }
        public byte[] ABSReceive()//Синхронизированное получение ответа от клиента о успешном получении массива байтов + генерация буфера
        {
            int Size;
            try
            {
                Size = BitConverter.ToInt32(WaitingReceive(), 0);
            } catch { Size = 0; }
            Send(SystemSC.bytes, 1);
            byte[] bytes = WaitingReceive(Size); 
            Send(SystemSC.bytes, 1);
            return bytes;
        }
        public string ByteToString(byte[] bytes) => Encoding.Unicode.GetString(bytes);
        public bool ABSSend(byte[] bytes, byte code)//Синх отправка + буфер
        {
            Send(BitConverter.GetBytes(bytes.Length+1), 1);
            WaitingReceive();
            Send(bytes, code);
            WaitingReceive();
            return Connected;
        }
        public bool ABSSend(string txt, byte code) => ABSSend(Encoding.Unicode.GetBytes(txt), code);//
        protected void Add() => add?.Invoke(this);
        protected void Add(RemovalClient add) => this.add = add;
        protected void Rem() => remova?.Invoke(this);
        protected void Rem(RemovalClient rem) => remova = rem;
        public void Disconect()
        {
            if (Connected) 
            {
                Connected = false;
                code = 0;
                try
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch { }
                remova?.Invoke(this);
                socket.Dispose();
            }
        }
        public override string ToString()
        {
            return iP;
        }
        public void IfConnected(){ if (socket.Poll(1000, SelectMode.SelectRead) && (socket.Available == 0)) { Disconect(); }}

        public void Dispose()
        {
            socket.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
