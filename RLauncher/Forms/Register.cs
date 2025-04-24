using BD;
using System;
using System.Windows.Forms;
using TCPServer;

namespace RLauncher.DF
{
    public partial class Register : Form
    {
        public Register(SocketClient socketClient)
        {
            InitializeComponent();
            cap = new Cap(button2, this);
            this.socketClient = socketClient;
        }
        #region DesignStuff
        Cap cap;
        SocketClient socketClient;
        bool FirstClickTb1 = true;
        bool FirstClickTb2 = true;
        bool FirstClickTb3 = true;
        bool FirstClickTb4 = true;
        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void PasswordTb_Click(object sender, EventArgs e)
        {
            if (FirstClickTb3)
            {
                PasswordTb.Text = "";
                FirstClickTb3 = false;
            }
            PasswordTb.UseSystemPasswordChar = true;
        }

        private void RepeatPassTb_Click(object sender, EventArgs e)
        {
            if (FirstClickTb4)
            {
                RepeatPassTb.Text = "";
                FirstClickTb4 = false;
            }
            RepeatPassTb.UseSystemPasswordChar = true;
        }

        private void LoginTb_Click(object sender, EventArgs e)
        {
            if (FirstClickTb2)
            {
                LoginTb.Text = "";
                FirstClickTb2 = false;
            }
        }

        private void NameTb_Click(object sender, EventArgs e)
        {
            if (FirstClickTb1)
            {
                NameTb.Text = "";
                FirstClickTb1 = false;
            }
        }
        private void NameTb_Leave(object sender, EventArgs e)
        {
            if (NameTb.Text == "")
            {
                NameTb.Text = "Ваше имя";
                FirstClickTb1 = true;
            }
        }

        private void LoginTb_Leave(object sender, EventArgs e)
        {
            if (LoginTb.Text == "")
            {
                LoginTb.Text = "Придумайте логин";
                FirstClickTb2 = true;
            }
        }

        private void PasswordTb_Leave(object sender, EventArgs e)
        {
            if (PasswordTb.Text == "")
            {
                PasswordTb.Text = "Придумайте пароль";
                FirstClickTb3 = true;
                PasswordTb.UseSystemPasswordChar = false;
            }
        }

        private void RepeatPassTb_Leave(object sender, EventArgs e)
        {
            if (RepeatPassTb.Text == "")
            {
                RepeatPassTb.Text = "Повторите пароль";
                FirstClickTb4 = true;
                RepeatPassTb.UseSystemPasswordChar = false;
            }
        }
        #endregion

        private void RegBt_Click(object sender, EventArgs e)
        {
            if(NameTb.Text == "" || NameTb.Text == "Ваше имя")
            {
                SystemCustom.ShowMessage("Пожалуйста, укажите своё имя",  MessageBoxButtons.OK);
                return;
            }
            if (LoginTb.Text == "" || LoginTb.Text == "Придумайте логин")
            {
                SystemCustom.ShowMessage("Пожалуйста, придумайте логин!", MessageBoxButtons.OK);
                return;
            }
            if (PasswordTb.Text == "" || PasswordTb.Text == "Придумайте пароль" && RepeatPassTb.Text == "Повторите пароль")
            {
                SystemCustom.ShowMessage("Пожалуйста, придумайте пароль!", MessageBoxButtons.OK);
                return;
            }    
            if(PasswordTb.Text != RepeatPassTb.Text || RepeatPassTb.Text == "Повторите пароль")
            {
                SystemCustom.ShowMessage("Пароли не совпадают!", MessageBoxButtons.OK);
                return;
            }
            BDUser bDUser = new BDUser();
            bDUser.Name = NameTb.Text;
            bDUser.Login = LoginTb.Text.ToLower();
            bDUser.Password = PasswordTb.Text;

            socketClient.ABSSend(BD.SystemCustom.WriteXml(bDUser), 12);
            socketClient.ABSReceive();
            if (socketClient.code == 4) 
            {
                SystemCustom.ShowMessage("Токой пользователь уже существует!", MessageBoxButtons.OK);
                return; 
            }
            this.Close();
        }

        private void Register_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
        }
    }
}
