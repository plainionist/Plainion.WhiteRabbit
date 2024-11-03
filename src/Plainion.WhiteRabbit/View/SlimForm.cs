﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Plainion.WhiteRabbit.Presentation;
using System.Security;
using Plainion.WhiteRabbit.Model;

namespace Plainion.WhiteRabbit.View
{
    public partial class SlimForm : Form, IView
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute( "user32.dll" )]
        public static extern int SendMessage( IntPtr hWnd, int Msg, int wParam, int lParam );
        [DllImportAttribute( "user32.dll" )]
        public static extern bool ReleaseCapture();

        private Controller myController = null;
        private Channel myChannel = null;

        public SlimForm( Controller controller)
        {
            InitializeComponent();

            myController = controller;

            myChannel = new Channel();
            myChannel.OnTimeElapsedChanged = ( span ) => myTimeElapsed.Text = span.ToString();

            Width = panel1.Width;
            StartPosition = FormStartPosition.Manual;
            Location = new Point( Screen.PrimaryScreen.WorkingArea.Width / 2 - Width / 2, 0 );
            ShowInTaskbar = false;
            TopMost = true;
            //AllowTransparency = true;
            //TransparencyKey = SystemColors.Control;
        }

        public void Start( DayEntry entry )
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
        private void SlimForm_MouseDown( object sender, MouseEventArgs e )
        {
            if (e.Button == MouseButtons.Left && Control.ModifierKeys == Keys.Alt)
            {
                ReleaseCapture();
                SendMessage( Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0 );
            }
        }

        [SecuritySafeCritical]
        private void panel1_MouseDown( object sender, MouseEventArgs e )
        {
            if( e.Button == MouseButtons.Left && Control.ModifierKeys == Keys.Alt )
            {
                ReleaseCapture();
                SendMessage( Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0 );
            }
        }

        private void myStopRecordBtn_Click( object sender, EventArgs e )
        {
            myStopRecordBtn.Enabled = false;

            myController.StopTimeMeasurement( myCommentTxt.Text );
        }
    }
}
