using BD;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using TCPServer;

namespace RServer
{
    public class ProcessingUser: IDisposable
    {
        public ProcessingUser(SQL sQL, User user)
        {
            this.user = user;
            this.sQL = sQL;
        }
        SQL sQL;
        User user;
        event InfoServer info;
        public void Processing(List<ProgramsFile> files, InfoServer infoServer)//обработка пользователя
        {
            info = infoServer;
            byte[] b;
            string txt = "";
            SqlParameter[] sqlParameters;
            SqlDataReader SDR;
            //Для процедур
            while (user.Connected)
            {
                b = user.ABSReceive();
                BDUser bDUser;
                switch (user.code)
                {
                    case 10://Логин
                        string login = user.ByteToString(b), pass = user.ByteToString(user.ABSReceive());
                        sqlParameters = sQL.SqlParameters(new string[] { "@Login", "@Password" });
                        sqlParameters[0].Value = login;
                        sqlParameters[1].Value = pass;
                        bDUser = new BDUser(sQL.ProcedureInvokeSDR("Log_In", sqlParameters));
                        if (bDUser.Key == "")
                        {
                            //неверный логин
                            user.ABSSend(SystemSC.bytes, 1);
                            continue;
                        }
                        //принятие
                        sqlParameters = sQL.SqlParameters(new string[] { "@Login", "@NewKey" });
                        sqlParameters[0].Value = login;
                        sqlParameters[1].Value = bDUser.Key;
                        SDR = sQL.ProcedureInvokeSDR("ChangeKey", sqlParameters);
                        SDR.Close();
                        SDR.Dispose();
                        user.ABSSend(bDUser.Key, 10);
                        user.ABSSend(bDUser.Name, 10);
                        break;
                    case 11:// Логин через ключ
                        string Key = user.ByteToString(b);
                        if (Key == "")
                        {
                            user.Disconect();
                            continue;
                        }
                        sqlParameters = sQL.SqlParameters(new string[] { "@Key" });
                        sqlParameters[0].Value = Key;
                        bDUser = new BDUser(sQL.ProcedureInvokeSDR("Log_In_key", sqlParameters));
                        user.ABSSend(bDUser.Login, 11);
                        user.ABSReceive();
                        if (user.code == 11)
                        {
                            user.ABSSend(bDUser.Name, 11);
                        }
                        else
                        {
                            continue;
                        }
                        break;
                        case 12://Регистрация
                        BDUser bDUser1 = SystemCustom.ReadXml<BDUser>(b);
                        sqlParameters = sQL.SqlParameters(new string[] { "@Name", "@Pass", "@Login" });
                        sqlParameters[0].Value = bDUser1.Name;
                        sqlParameters[1].Value = bDUser1.Password;
                        sqlParameters[2].Value = bDUser1.Login;
                        SDR = sQL.ProcedureInvokeSDR("CreateUser", sqlParameters);
                        if (SDR.HasRows)
                        { user.ABSSend(SystemSC.bytes, 5); }
                        else { user.ABSSend(SystemSC.bytes, 4); }
                        SDR.Close();
                        SDR.Dispose();
                        continue;
                        case 13://Вспомнить пароль
                        txt = user.ByteToString(b);
                        sqlParameters = sQL.SqlParameters(new string[] { "@login"});
                        sqlParameters[0].Value = txt;
                        SDR = sQL.ProcedureInvokeSDR("PassHelp", sqlParameters);
                        if (SDR.HasRows)
                        { user.ABSSend((string)SDR["Pass"], 5); }
                        else { user.ABSSend(SystemSC.bytes, 4); }
                        SDR.Close();
                        SDR.Dispose();
                        continue;
                    case 21:
                        user.ABSSend(SystemSC.bytes, 21);
                        break;
                }
                user.ABSReceive();
                if (user.code == 20)
                {
                    break;
                }
                else
                {
                    info?.Invoke($"Непредвиденый баг c user ip:{user}");
                }

            }
            txt = "";
            while (user.Connected)
            {
                b = user.ABSReceive();
                switch (user.code)
                {
                    case 9:
                        txt = user.ByteToString(b);
                        sqlParameters = sQL.SqlParameters(new string[] { "@loginUser" });
                        sqlParameters[0].Value = txt;
                        user.ABSSend(SystemCustom.WriteXml(sQL.AssemblingTable<ProgramsFile>("ViewListProducts", sqlParameters)), 9);
                        break;
                    case 10://Клиент спрашивает какие файлы есть
                        user.ABSSend(SystemCustom.WriteXml(files.ToArray()), 10);
                        break;
                        case 11://Клиент выбирает файл и передаёт нам информацию о файле чтобы мы передали файл на клиент
                        string name = user.ByteToString(b);
                        foreach (ProgramsFile file in files)
                        {
                            if (file.name == name)
                            {
                                user.Loading(File.OpenRead(file.path));
                                break;
                            }
                        }
                        break;
                    case 12:
                        txt = user.ByteToString(b);
                        sqlParameters = sQL.SqlParameters(new string[] { "@LoginUser", "@ProdName" });
                        sqlParameters[0].Value = txt;
                        txt = user.ByteToString(user.ABSReceive());
                        sqlParameters[1].Value = txt;
                        sQL.ProcedureInvoke("addToListProduct", sqlParameters);
                        break;
                    default:
                        //MessageBox.Show("Непредвиденый баг");
                        return;
                }
            }    
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
