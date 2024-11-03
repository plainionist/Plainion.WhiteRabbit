using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;
using Plainion.WhiteRabbit.Model;
using Plainion.WhiteRabbit.Presentation;

namespace Plainion.WhiteRabbit.View
{
    public partial class SlimForm : Form, IView
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private Controller myController;
        private Channel myChannel;

        public SlimForm(Controller controller)
        {
            InitializeComponent();

            myController = controller;

            myChannel = new Channel();
            myChannel.OnTimeElapsedChanged = (span) => myTimeElapsed.Text = span.ToString();

            Width = myPanel.Width;
            StartPosition = FormStartPosition.Manual;
            Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - Width / 2, 0);
            ShowInTaskbar = false;
            TopMost = true;
        }

        public void Start(DayEntry entry)
        {
            myStopRecordBtn.Enabled = true;

            myCommentTxt.Text = entry.Comment;
        }

        public Channel Channel
        {
            get
            {
                return myChannel;
            }
        }

        [SecuritySafeCritical]
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Control.ModifierKeys == Keys.Alt)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void OnStopButton(object sender, EventArgs e)
        {
            myStopRecordBtn.Enabled = false;

            myController.StopTimeMeasurement(myCommentTxt.Text);
        }
    }
}
