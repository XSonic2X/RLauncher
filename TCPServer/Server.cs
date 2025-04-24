using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace TCPServer
{
    public delegate void GetUser(User user);
    public class Server
    {
        public Server(string IP, int Port, int Listem, string HandTxtAcc, string HandTxtRec)
        {
            this.HandTxtRec = HandTxtRec;
            this.HandTxtAcc = HandTxtAcc;
            ipPoint = new IPEndPoint(IPAddress.Parse(IP), Port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipPoint);
            socket.Listen(Listem);//Кол-во людей в очереди  подключений
            add += Add;
            remova += Rem;
        }
        
        private string HandTxtAcc { get; set; }
        private string HandTxtRec { get; set; }
        public event RemovalClient add;
        public event RemovalClient remova;
        public event GetUser getUser;
        public Socket socket { get; set; }
        public List<Client> users = new List<Client>();
        public bool start { get; private set; } = false;
        private IPEndPoint ipPoint { get; set; }
        //public void Start()
        //{
        //    if (start) { return; }
        //    start = true;
        //    IfConnect();
        //    while (start) { AcceptUser(socket.Accept()); }
        //}
        public async Task AsyncStart() => await Task.Run(() =>
        {
            if (start) { return; }
            start = true;
            IfConnect();
            while (start) { AcceptUser(socket.Accept()); }
        });
        public async Task IfConnect() => await Task.Run(() =>
        {
            while (start)
            {
                Thread.Sleep(5000);//Не нагружаем проц
                for (int i = 0; i < users.Count; i++)
                {
                    users[i].IfConnected();
                }
            }
        });
        public void Stop()
        {
            if (start) { start = false; }
        }
        public async Task AcceptUser(Socket socket) => await Task.Run(() => 
        { 
            User user = new User(socket, HandTxtAcc, HandTxtRec, add, remova);//Пока в классе User что-то выполняется, мы сюда не вернёмся
            getUser?.Invoke(user);
            user.Disconect();
        });
        public void Add(Client client) => users.Add(client);
        public void Rem(Client client) => users.Remove(client);
    }
}
