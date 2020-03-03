using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ScreenCapture
{
    public class Program : Form
    {
        internal readonly IContainer components;
        internal readonly PictureBox pictureBox;
        internal readonly ColorDialog colorDialog;
        internal readonly ToolStripMenuItem toolStripMenuItemExplain;
        internal readonly ToolStripMenuItem toolStripMenuItemExit;
        internal readonly ContextMenuStrip contextMenuStrip;
        internal readonly NotifyIcon notifyIcon;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private Program()
        {
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Program));

            pictureBox = new PictureBox()
            {
                Location = new Point(0, 0)
            };

            colorDialog = new ColorDialog();

            toolStripMenuItemExplain = new ToolStripMenuItem("说明");
            toolStripMenuItemExit = new ToolStripMenuItem("退出");

            contextMenuStrip = new ContextMenuStrip(components);
            contextMenuStrip.Items.AddRange(new ToolStripItem[] {
                toolStripMenuItemExplain,
                toolStripMenuItemExit
            });

            notifyIcon = new NotifyIcon(components)
            {
                ContextMenuStrip = contextMenuStrip,
                Icon = (Icon)resources.GetObject("myIcon.ico"),
                Visible = true
            };

            WindowState = FormWindowState.Minimized;
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;

            int width = Screen.PrimaryScreen.Bounds.Width;
            int height = Screen.PrimaryScreen.Bounds.Height;
            Location = new Point(0, 0);
            ClientSize = new Size(width, height);
            pictureBox.Size = new Size(width, height);
            Controls.Add(pictureBox);

            Functions.GetFunctions(this);
        }

        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Program());
        }
    }
}
