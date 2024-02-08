using BD;
using System;
using System.Windows.Forms;

namespace RLauncher.Forms
{
    public partial class ProgInfo : Form
    {
        public ProgInfo(ProgramsFile file)
        {
            InitializeComponent();
            NameLb.Text = file.name;
            InfoTb.Text = file.Description;
            cap = new Cap(button2, this);
            pictureBox1.Image = SystemCustom.bitmap;
        }
        Cap cap;

        /// <summary>
        /// Я исправлю
        /// </summary>
        public bool test = false;
        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void ProgInfo_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddToUser_Click(object sender, EventArgs e)
        {
            test = true;
            this.Close();
        }
    }
}
