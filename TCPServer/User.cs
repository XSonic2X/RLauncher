using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace TCPServer
{
    public class User : Client
    {
        public User(Socket socket, string HandTxtAcc, string HandTxtRec, RemovalClient add, RemovalClient rem)
        {
            this.HandTxtRec = HandTxtRec;
            this.HandTxtAcc = HandTxtAcc;
            this.socket = socket;
            iP = socket.RemoteEndPoint.ToString().Split(':')[0];
            //this.add = add;
            Add(add);
            Rem(rem);
            Connected = true;
            Handshake();//Рукопожатие, принятие сокета
        }
        public void Handshake()
        {
            if (Encoding.Unicode.GetString(ABSReceive() ?? SystemSC.bytes) == HandTxtAcc)
            {
                if (ABSSend(Encoding.Unicode.GetBytes(HandTxtRec), 2))
                {
                    Add();
                    return;
                }
            }
            Disconect();
        }
        public void Loading(FileStream file)//Передача файлов
        {
            string[] txt = file.Name.Split('\\');
            ABSSend(txt[txt.Length - 1], 40);
            long a = BitConverter.ToInt64(ABSReceive(), 0);//Получаем информацию о кол-ве байтов у клиента
            try
            {
                file.Position = a;//На каком байте остановились
                ABSSend(BitConverter.GetBytes(file.Length), 5);//Отправляем кол-во байтов от файла
                byte[] bytes = new byte[1048576];//Размер буфера
                while (true)
                {
                    ABSReceive();
                    if (code == 5) { break; }
                    if ((file.Length - file.Position) > 1048576)
                    {
                        file.Read(bytes, 0, bytes.Length);
                        ABSSend(bytes, 1);
                    }
                    else //Корректируем размер буфера, окончание загрузки
                    {
                        bytes = new byte[(file.Length - file.Position)];
                        file.Read(bytes, 0, bytes.Length);
                        ABSSend(bytes, 1);
                        file.Close();
                        break;
                    }
                }
            }
            catch { Disconect(); }
        }
    }
}
