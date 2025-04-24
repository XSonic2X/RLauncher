using System;
using System.Windows.Forms;

namespace RLauncher
{
    public class Cap
    {
        public Cap(Button button, Form form) 
        { 
            this.form = form;
            button.MouseDown += new MouseEventHandler(buttonMouseDown);
            button.MouseMove += new MouseEventHandler(buttonMouseMove);
            button.MouseUp += new MouseEventHandler(buttonMouseUp);
            end = () =>
            {
                button.MouseDown -= new MouseEventHandler(buttonMouseDown);
                button.MouseMove -= new MouseEventHandler(buttonMouseMove);
                button.MouseUp -= new MouseEventHandler(buttonMouseUp);
            };
        }
        ~Cap()
        {
            end();
        }
        private Action end;
        private Form form;
        private bool moveForm = false;
        private void buttonMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                moveForm = false;
            }
        }

        private void buttonMouseMove(object sender, MouseEventArgs e)
        {
            if (moveForm)
            {
                SystemCustom.MoveForm(form);
            }
        }

        private void buttonMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SystemCustom.UpdateMouse(form);
                moveForm = true;
            }
        }
    }
}
