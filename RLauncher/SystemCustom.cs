using System;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RLauncher
{
    public static class SystemCustom
    {
        static Point Mouse = Control.MousePosition;
        static Point mouseForm;
        static int X = 0, Y = 0;
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn( int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
        public static void FormRound(Form form, int nLeftRect, int nTopRect, int nWidthEllipse, int nHeightEllipse)
        {
            form.FormBorderStyle = FormBorderStyle.None;
            form.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(nLeftRect, nTopRect, form.Width, form.Height, nWidthEllipse, nHeightEllipse));
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
    }
}
