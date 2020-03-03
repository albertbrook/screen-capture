using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ScreenCapture
{
    internal class Functions
    {
        private static Functions functions;
        private readonly Program program;

        private bool isDraw = false;
        private int previousX;
        private int previousY;
        private readonly SolidBrush brush = new SolidBrush(Color.Red);
        private int size = 3;

        private Functions(Program program)
        {
            this.program = program;
            this.program.pictureBox.MouseDown += new MouseEventHandler(PictureBox_MouseDown);
            this.program.pictureBox.MouseUp += new MouseEventHandler(PictureBox_MouseUp);
            this.program.pictureBox.MouseMove += new MouseEventHandler(PictureBox_MouseMove);
            this.program.pictureBox.MouseWheel += new MouseEventHandler(PictureBox_MouseWheel);
            this.program.toolStripMenuItemExplain.Click += new EventHandler(ToolStripMenuItemExplain_Click);
            this.program.toolStripMenuItemExit.Click += new EventHandler(ToolStripMenuItemExit_Click);
            this.program.notifyIcon.MouseClick += new MouseEventHandler(NotifyIcon_MouseClick);
        }

        internal static Functions GetFunctions(Program program)
        {
            if (functions == null)
                functions = new Functions(program);
            return functions;
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDraw = true;
                previousX = e.Location.X;
                previousY = e.Location.Y;
            }
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            isDraw = false;
            switch (e.Button)
            {
                case MouseButtons.Middle:
                    program.colorDialog.ShowDialog();
                    brush.Color = program.colorDialog.Color;
                    break;
                case MouseButtons.Right:
                    program.Hide();
                    break;
            }
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraw)
            {
                Bitmap image = (Bitmap) program.pictureBox.Image;
                Graphics g = Graphics.FromImage(image);
                int moveX = e.Location.X - previousX;
                int moveY = e.Location.Y - previousY;
                int raiseX = moveX > 0 ? 1 : -1;
                int raiseY = moveY > 0 ? 1 : -1;
                double max, average;
                bool isMaxX;
                {
                    int absX = Math.Abs(moveX);
                    int absY = Math.Abs(moveY);
                    max = absX > absY ? absX : absY;
                    average = absX < absY ? absX : absY;
                    average /= max;
                    isMaxX = max == absX ? true : false;
                    if (!isMaxX)
                        ExchangeVariables(ref raiseX, ref raiseY);
                }
                double j = 0;
                for (int i = 0; i < max; i++)
                {
                    int relativeX, relativeY;
                    j += (raiseY * average);
                    relativeX = raiseX * i - (size >> 1);
                    relativeY = (int)j - (size >> 1);
                    if (!isMaxX)
                        ExchangeVariables(ref relativeX, ref relativeY);
                    g.FillEllipse(brush, previousX + relativeX, previousY + relativeY, size, size);
                }
                previousX = e.Location.X;
                previousY = e.Location.Y;
                program.pictureBox.Image = image;
            }
        }

        private void ExchangeVariables(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        private void PictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0 && size < 100)
                size++;
            else if (size > 2)
                size--;
        }

        private void ToolStripMenuItemExplain_Click(object sender, EventArgs e)
        {
            string url = "https://github.com/albertbrook";
            string message = "操作\n鼠标左键开始画图\n鼠标中键选择颜色\n鼠标滚轮调整大小\n鼠标右键退出画图\n\n是否打开？\n" + url;
            string title = "AlbertBrook";
            DialogResult resault = MessageBox.Show(message, title, MessageBoxButtons.OKCancel);
            if (resault == DialogResult.OK)
                Process.Start(url);
        }

        private void ToolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            program.Dispose();
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int width = Screen.PrimaryScreen.Bounds.Width;
                int height = Screen.PrimaryScreen.Bounds.Height;
                Bitmap image = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(image);
                g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
                g.Dispose();

                program.pictureBox.Image = image;
                program.WindowState = FormWindowState.Normal;
                program.Show();
            }
        }
    }
}
