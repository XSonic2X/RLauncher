using BD;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using TCPServer;

namespace RServer
{
    public delegate void InfoServer(string info);
    internal class Program
    {
        static List<ProgramsFile> programsFiles = new List<ProgramsFile>();
        static string SQLCon = "Data Source=HOME-PC;Initial Catalog=RShopProgram;Integrated Security=True";
        static void Main(string[] args)
        {
            Server server = new Server("192.168.0.105", 104, 5, "test1", "test2"); //Максимум 5 челиксов
            server.getUser += AcceptanceByUsers;
            server.add += Add;
            server.remova += Rem;
            infoServer += Console.WriteLine;
            if (Scanning()) 
            {
                server.AsyncStart();
                Console.WriteLine("Start");
                do 
                {
                    switch (Console.ReadLine().ToLower())
                    {
                        case "stop":
                            server.Stop();
                            break;
                    }
                }
                while (server.start);
            }
            else 
            {
                Console.WriteLine("Files not found");
                Console.ReadLine();
            }
        }
        static event InfoServer infoServer;
        static bool Scanning() // Получаем все файлы в папке Programs
        {
            try
            {
                SQL sQL = new SQL(SQLCon);
                sQL.Open();
                Product[] products = sQL.AssemblingTable<Product>("ViewProducts");
                sQL.Close();
                string[] dir1 = Directory.GetFiles($"{Environment.CurrentDirectory}\\Programs", "*.*", SearchOption.AllDirectories);
                if (dir1.Length == 0) { return false; }
                foreach (string dir in dir1)
                {
                    programsFiles.Add(new ProgramsFile(dir));
                }
                int i;
                string[] sqkPs = new string[]
                {
                    "@Name",
                    "@Price"
                };
                if (products.Length > 0)
                {
                    bool faills = false;
                    foreach (ProgramsFile file in programsFiles)
                    {
                        faills = true;
                        for (i = 0; i < products.Length; i++)
                        {
                            if (products[i].IfName(file.name))
                            {
                                file.Price = products[i].Price;
                                faills = false;
                                break;
                            }
                        }
                        if (faills)
                        { Set(file, sQL, sQL.SqlParameters(sqkPs)); }
                    }
                }
                else
                {
                    foreach (ProgramsFile file in programsFiles)
                    {
                        Set(file, sQL, sQL.SqlParameters(sqkPs));
                    }
                }
                Console.Clear();
                return true;
            } catch
            {
                try { new DirectoryInfo(Environment.CurrentDirectory).CreateSubdirectory("Programs"); }
                catch { }
                return false;
            }
        }
        static void Set(ProgramsFile file, SQL sQL, SqlParameter[] sqlParameters)
        {
            decimal Price;
            string txt;
            do
            {
                Console.Clear();
                Console.WriteLine($"Specify the {file} price");
                Price = Convert.ToDecimal(Console.ReadLine());
                Console.WriteLine("Are you sure? Y/N");
                txt = Console.ReadLine().ToLower();
                //try
                //{
                //    Price = Convert.ToDecimal(Console.ReadLine());
                //    Console.WriteLine("Are you sure? Y/N");
                //    txt = Console.ReadLine().ToLower();
                //}
                //catch
                //{
                //    txt = "";
                //    Price = 0;
                //}
            } while (txt != "y" && txt != "yes");
            sqlParameters[0].Value = file.name;
            sqlParameters[1].Value = Price;
            file.Price = Price;
            sQL.Open();
            sQL.ProcedureInvoke("addProduct", sqlParameters);
            sQL.Close();
        }
        static void AcceptanceByUsers(User user)
        {
            SQL sQL = new SQL(SQLCon);
            sQL.Open();
            ProcessingUser processing = new ProcessingUser(sQL,user);
            processing.Processing(programsFiles, infoServer);
            sQL.Close();
        }
        static void Add(Client client)
        {
            Console.WriteLine("Add:"+client.ToString());
        }
        static void Rem(Client client)
        {
            Console.WriteLine("Rem:"+client.ToString());
        }
    }
}
