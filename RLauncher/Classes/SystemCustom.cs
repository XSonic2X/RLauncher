using RLauncher.DF;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RLauncher
{
    public static class SystemCustom
    {
        public static Bitmap bitmap { get; private set; } = new Bitmap(Properties.Resources.p3069437);

        private static Point Mouse = Control.MousePosition;
        private static Point mouseForm;

        private static int X = 0, Y = 0;

        public static void FormRound(Form form, int nLeftRect, int nTopRect, int nWidthEllipse, int nHeightEllipse)
        {
            form.FormBorderStyle = FormBorderStyle.None;
            form.Region = Region.FromHrgn(CreateRoundRectRgn(nLeftRect, nTopRect, form.Width, form.Height, nWidthEllipse, nHeightEllipse));
        }
        public static void UpdateMouse(Form form)
        {
            Mouse = Control.MousePosition;
            X = form.Location.X - Mouse.X;
            Y = form.Location.Y - Mouse.Y;
        }
        public static void MoveForm(Form form)
        {
            mouseForm = Control.MousePosition;
            mouseForm.Offset(X, Y);
            form.Location = mouseForm;
        }
        public static DialogResult ShowMessage(string message, MessageBoxButtons button)
        {
            DialogResult dlgResult = DialogResult.None;
            switch (button)
            {
                case MessageBoxButtons.OK:
                    using (CustomMessageBox msgOK = new CustomMessageBox())
                    {
                        msgOK.Message = message;
                        dlgResult = msgOK.ShowDialog();
                    }
                    break;
                case MessageBoxButtons.YesNo:
                    using (CustomMessageBoxYesNo msgYesNo = new CustomMessageBoxYesNo())
                    {
                        msgYesNo.Message = message;
                        dlgResult = msgYesNo.ShowDialog();
                    }
                    break;
            }
            return dlgResult;
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

    }
}
