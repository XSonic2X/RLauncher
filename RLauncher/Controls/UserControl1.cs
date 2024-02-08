using BD;
using RLauncher.Forms;
using System.Drawing;
using System.Windows.Forms;

namespace RLauncher.DF
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
            pictureBox1.Image = SystemCustom.bitmap;
        }
        public UserControl1(ProgramsFile programs, Point point)
        {
            InitializeComponent();
            this.programs = programs;
            progName = programs.name;
            cost = $"Цена:{programs.Price}";
            pictureBox1.Image = SystemCustom.bitmap;
            Location = point;
        }
        public bool Enabled {
            get { return panel1.Enabled; }
            set { panel1.Enabled = value; }
        }
        public ProgramsFile programs;
        public string progName { get { return ProgName.Text; }  set { ProgName.Text = value; } }
        public string cost { get { return ProgCost.Text;} set { ProgCost.Text = value; }}
        event GetName getName;
        public void setGN(GetName getName) => this.getName = getName;

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            using (ProgInfo PI = new ProgInfo(programs))
            {
                PI.ShowDialog();
                if (PI.test)
                {
                    getName?.Invoke(progName);
                }
            }
        }
    }
}
