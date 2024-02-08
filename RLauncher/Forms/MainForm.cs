using BD;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCPServer;

namespace RLauncher.DF
{
    public delegate void GetName(string name);
    public enum MainFTab
    {
        My,
        All_Program
    }
    public partial class MainForm : Form
    {
        public MainForm(SocketClient socketClient)
        {
            InitializeComponent();
            Form(socketClient);
            socketClient.ABSSend(SystemSC.bytes, 10);
            byte[] data = socketClient.ABSReceive();
            productsAll = BD.SystemCustom.ReadXml<ProgramsFile[]>(data);
            UpdatePanel(false);
            button2.Enabled = false;
            button5.Enabled = false;
        }
        MainFTab mainFTab = MainFTab.All_Program;
        public MainForm(SocketClient socketClient, BDUser bDUser)
        {
            InitializeComponent();
            Form(socketClient);
            //DownLoadPn.Hide();
            socketClient.ABSSend(SystemSC.bytes, 10);
            byte[] data = socketClient.ABSReceive();
            productsAll = BD.SystemCustom.ReadXml<ProgramsFile[]>(data);
            UpdatePanel(true);
            this.bDUser = bDUser;
        }
        BDUser bDUser;
        public void Form(SocketClient socketClient)
        {
            SystemCustom.FormRound(this, 0, 0, 15, 15);
            cap = new Cap(button1, this);
            this.socketClient = socketClient;
            point1 = DownLoadPn.Location;
            this.Size = new Size(1192, 644);
            getName += Lod;
        }
        Point point1;
        Cap cap;
        ProgramsFile[] productsAll;
        ProgramsFile[] productsMy;
        event GetName getName;
        SocketClient socketClient;
        public void UpdatePanel(bool E)
        {
            progPanel.Controls.Clear();
            int y = 0;
            int x = 0;
            switch (mainFTab)
            {
                case MainFTab.My:
                    socketClient.ABSSend(bDUser.Login, 9);
                    byte[] data = socketClient.ABSReceive();
                    productsMy = BD.SystemCustom.ReadXml<ProgramsFile[]>(data);
                    foreach (ProgramsFile file in productsMy)
                    {
                        UserControl1 userControl = new UserControl1(file, new Point(x, y));
                        userControl.setGN(getName);
                        userControl.Enabled = false;
                        progPanel.Controls.Add(userControl);
                        y += 90;
                    }
                    break;
                case MainFTab.All_Program:
                    foreach (ProgramsFile file in productsAll)
                    {
                        UserControl1 userControl = new UserControl1(file, new Point(x, y));
                        userControl.setGN(getName);
                        userControl.Enabled = E;
                        progPanel.Controls.Add(userControl);
                        y += 90;
                    }
                    break;
            }
        }
        public void Lod(string txt)
        {
            if (socketClient.loading) { return; }
            socketClient.ABSSend(bDUser.Login, 12);
            socketClient.ABSSend(txt, 11);
            LoadingMovement(true);
            try { new DirectoryInfo(Environment.CurrentDirectory).CreateSubdirectory("D" + txt.Split('.')[0]); }
            catch { }
            socketClient.ABSSend(txt, 11);
            socketClient.Loading(Environment.CurrentDirectory + $"\\D{txt.Split('.')[0]}");
            bar();
        }
        public async void bar() => await Task.Run(() =>
        {
            while (socketClient.loading)
            {
                Thread.Sleep(10);
                if (socketClient.max == 0) { continue; }
                if (100 <= SetBar((double)socketClient.current / (double)socketClient.max))
                {
                    break;
                }
            }
            LoadingMovement(false);
        });
        public double SetBar(double a)
        {
            a *= 100;
            Action action = () =>
            {
                label1.Text = $"Загрузка: {Math.Round(a, 3)}";
                try
                {
                    progressBar1.Value = (int)Math.Floor(a);
                }
                catch { }
            };
            if (InvokeRequired)
            { Invoke(action); }
            else { action(); }
            return progressBar1.Value;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SettingsBt_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            this.Hide();
            settingsForm.ShowDialog();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
        }
        bool costaliTime = false;
        private async void LoadingMovement(bool up)
        {
            await Task.Run(() => 
            {
                while (costaliTime)
                {
                    Thread.Sleep(10);
                }
                costaliTime = true;
                Point point = DownLoadPn.Location;
                Action action = () => { DownLoadPn.Location = point; };
                if (up)
                {
                    for (int i = 0; i < 63; i++)
                    {
                        point.Y -= 1;
                        Thread.Sleep(5);
                        if (InvokeRequired)
                        { Invoke(action); }
                        else { action(); }
                    }
                }
                else
                {
                    for (int i = 0; i < 63; i++)
                    {
                        point.Y += 1;
                        Thread.Sleep(5);
                        if (InvokeRequired)
                        { Invoke(action); }
                        else { action(); }
                    }
                }
                costaliTime = false;
            });
        }

        private void button8_Click(object sender, EventArgs e)
        {
            socketClient.Stop();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (socketClient.loading) { return; }
            mainFTab = MainFTab.My;
            UpdatePanel(true);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (socketClient.loading) { return; }
            mainFTab = MainFTab.All_Program;
            UpdatePanel(true);
        }
    }
}
