using System;
using System.Windows.Forms;

namespace RLauncher.DF
{
    public partial class CustomMessageBox : Form
    {
        public CustomMessageBox()
        {
            InitializeComponent();
            cap = new Cap(button2,this);
        }
        Cap cap;
        public string Message
        {
            get { return lbMessage.Text; }
            set { lbMessage.Text = value; }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CustomMessageBox_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
        }
    }
}
