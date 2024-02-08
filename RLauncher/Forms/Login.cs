using BD;
using RLauncher.DF;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using TCPServer;

namespace RLauncher
{
    public partial class Login : Form
    {
        bool FirstClickTb1 = true, FirstClickTb2 = true;
        //Graphics graphics;
        public Login()
        {
            InitializeComponent();
            SystemCustom.FormRound(this, 0, 0, 15, 15);
            cap = new Cap(button2, this);
            if (socketClient.Connect())
            {
                try
                {
                    dUser = new BDUser(File.ReadAllBytes("SaveB.bd"));
                    textBox1.Text = dUser.Login;
                    checkBox1.Checked = true;
                }
                catch
                {
                    dUser = new BDUser();
                }
            }
            else
            {
                SystemCustom.ShowMessage("Не удалось подключиться к серверу", MessageBoxButtons.OK);
                Process.GetCurrentProcess().Kill();
            }
        }
        BDUser dUser;
        Cap cap;
        Client client;
        SocketClient socketClient = new SocketClient("127.0.0.1", 104, "test1", "test2");
        private void button1_Click(object sender, EventArgs e)
        {
            byte[] bytes;
            if (checkBox1.Checked && dUser.Key.Length > 5 && dUser.Login != "")
            {
                socketClient.ABSSend(dUser.Key, 11); //Отправили ключ, если есть
                bytes = socketClient.ABSReceive();
                if (socketClient.code == 1)
                {
                    MessageBox.Show("Сохранение");
                    return;
                }
                if (dUser.Login == socketClient.ByteToString(bytes))
                {
                    socketClient.ABSSend(SystemSC.bytes,11);
                    dUser.Name = socketClient.ByteToString(socketClient.ABSReceive());
                }
                else 
                {
                    socketClient.ABSSend(SystemSC.bytes, 1);
                    dUser.Key = "";
                    MessageBox.Show("Зайти без пароля невозможно.\n\rВедите пороль.");
                    return;
                }
            }
            else
            {
                if (textBox2.Text == "" || textBox1.Text == "")
                {
                    return;
                }
                dUser.Login = textBox1.Text.ToLower();
                socketClient.ABSSend(dUser.Login, 10);
                socketClient.ABSSend(textBox2.Text, 10);
                bytes = socketClient.ABSReceive();
                if (socketClient.code == 1)
                {
                    MessageBox.Show("Неверный Логин или Пароль");
                    return; 
                }
                dUser.SetKey(socketClient.ByteToString(bytes));
                dUser.Name = socketClient.ByteToString(socketClient.ABSReceive());
                if (checkBox1.Checked)
                {
                    using (MemoryStream MS = new MemoryStream())
                    {
                        new XmlSerializer(typeof(BDUser)).Serialize(MS, dUser);
                        File.WriteAllBytes("SaveB.bd", MS.ToArray());
                    }
                }
            }
            socketClient.ABSSend(SystemSC.bytes, 20);
            MainForm MF = new MainForm(socketClient, dUser);
            this.Hide();
            MF.ShowDialog();
            this.Close();
        }
        public void Logging()
        {
            
        }

        public async void Statys() => await Task.Run(() =>
        {
            while (true)
            {
                Thread.Sleep(10);
                if (socketClient.max == 0) { continue; }
                //SetBar((double)socketClient.current / (double)socketClient.max);
            }
        });

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void Login_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            if (FirstClickTb2)
            {
                textBox2.Text = "";
                FirstClickTb2 = false;
            }
        }

        private void RegLabel_Click(object sender, EventArgs e)
        {
            using (Register reg = new Register(socketClient))
            {
                this.Hide();
                reg.ShowDialog();
                this.ShowDialog();
            }
        }

        private void LostPassLb_Click(object sender, EventArgs e)
        {
            using (LostPassHelp lostPassHelp = new LostPassHelp(socketClient))
            {
                this.Hide();
                lostPassHelp.ShowDialog();
                this.ShowDialog();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            socketClient.ABSSend(SystemSC.bytes, 21);
            socketClient.ABSReceive();
            if (socketClient.code == 21)
            {
                socketClient.ABSSend(SystemSC.bytes, 20);
                MainForm MF = new MainForm(socketClient);
                this.Hide();
                MF.ShowDialog();
                this.Close();
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            if(FirstClickTb1)
            {
                textBox1.Text = "";
                FirstClickTb1 = false;
            }
        }
    }
}
