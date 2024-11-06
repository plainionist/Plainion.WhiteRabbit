using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;
using Plainion.WhiteRabbit.Model;
using Plainion.WhiteRabbit.Presentation;

namespace Plainion.WhiteRabbit.View
{
    public  class SlimForm : Form, IView
    {
        private Panel myPanel;
        private Label myTimeElapsed;
        private Button myStopRecordBtn;
        private TextBox myCommentTxt;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private IController myController;

        internal SlimForm(IController controller)
        {
            InitializeComponent();

            myController = controller;

            Channel = new Channel();
            Channel.OnTimeElapsedChanged = (span) => myTimeElapsed.Text = span.ToString();

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

        public Channel Channel { get; }

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

        private void InitializeComponent()
        {
            this.myPanel = new Panel();
            this.myTimeElapsed = new Label();
            this.myStopRecordBtn = new Button();
            this.myCommentTxt = new TextBox();
            this.myPanel.SuspendLayout();
            this.SuspendLayout();

            this.myPanel.Controls.Add(this.myTimeElapsed);
            this.myPanel.Controls.Add(this.myStopRecordBtn);
            this.myPanel.Controls.Add(this.myCommentTxt);
            this.myPanel.Location = new Point(0, 0);
            this.myPanel.Name = "panel1";
            this.myPanel.Size = new Size(223, 29);
            this.myPanel.TabIndex = 17;
            this.myPanel.MouseDown += new MouseEventHandler(this.OnMouseDown);

            this.myTimeElapsed.AutoSize = true;
            this.myTimeElapsed.BorderStyle = BorderStyle.FixedSingle;
            this.myTimeElapsed.Location = new Point(166, 8);
            this.myTimeElapsed.Name = "myTimeElapsed";
            this.myTimeElapsed.Size = new Size(51, 15);
            this.myTimeElapsed.TabIndex = 20;
            this.myTimeElapsed.Text = "00:00:00";

            this.myStopRecordBtn.FlatStyle = FlatStyle.Popup;
            this.myStopRecordBtn.FlatAppearance.BorderColor = Color.White;
            this.myStopRecordBtn.FlatAppearance.BorderSize = 0;
            this.myStopRecordBtn.Image = GetImage("stop");
            this.myStopRecordBtn.ImageAlign = ContentAlignment.TopCenter;
            this.myStopRecordBtn.Location = new Point(3, 5);
            this.myStopRecordBtn.Name = "myStopRecordBtn";
            this.myStopRecordBtn.Size = new Size(23, 23);
            this.myStopRecordBtn.TabIndex = 18;
            this.myStopRecordBtn.UseVisualStyleBackColor = true;
            this.myStopRecordBtn.Click += new System.EventHandler(this.OnStopButton);

            this.myCommentTxt.Location = new Point(32, 5);
            this.myCommentTxt.Name = "myCommentTxt";
            this.myCommentTxt.Size = new Size(128, 20);
            this.myCommentTxt.TabIndex = 21;

            this.AutoScroll = true;
            this.ClientSize = new Size(223, 29);
            this.Controls.Add(this.myPanel);
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SlimForm";
            this.ControlBox = false;
            this.Text = String.Empty;
            this.MouseDown += new MouseEventHandler(this.OnMouseDown);
            this.myPanel.ResumeLayout(false);
            this.myPanel.PerformLayout();
            this.ResumeLayout(false);
        }

        private Image GetImage(string name)
        {
            using (var stream = GetType().Assembly.GetManifestResourceStream($"Plainion.WhiteRabbit.Resources.{name}.png"))
            {
                if (stream == null)
                {
                    throw new Exception($"Resource '{name}' not found");
                }

                var image = Image.FromStream(stream);
                return new Bitmap(image, new Size(15, 15));
            }
        }

    }
}
