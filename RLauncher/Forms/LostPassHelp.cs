using System;
using System.Windows.Forms;
using TCPServer;

namespace RLauncher.DF
{
    public partial class LostPassHelp : Form
    {
        Cap cap;
        public LostPassHelp(SocketClient socketClient)
        {
            InitializeComponent();
            newPassLb.Hide();
            cap = new Cap(button2, this);
            this.socketClient = socketClient;
        }
        SocketClient socketClient;
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpBt_Click(object sender, EventArgs e)
        {
            if (LoginTb.Text == "" | LoginTb.Text == "Введите логин")
            {
                SystemCustom.ShowMessage("Пожалуйста, введите свой логин", MessageBoxButtons.OK);
                return;
            }
            socketClient.ABSSend(LoginTb.Text.ToLower(), 13);
            byte[] b = socketClient.ABSReceive();
            if (socketClient.code == 5)
            {
                newPassLb.Text = socketClient.ByteToString(b);
                newPassLb.Show();
            }
            else 
            {
                SystemCustom.ShowMessage("ненашол", MessageBoxButtons.OK);
            }
        }

        private void LostPassHelp_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
        }
    }
}
