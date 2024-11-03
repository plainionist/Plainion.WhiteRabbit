using System;
using System.Drawing;
using System.Windows.Forms;

namespace Plainion.WhiteRabbit.View
{
    partial class SlimForm
    {
        private Panel myPanel;
        private Label myTimeElapsed;
        private Button myStopRecordBtn;
        private TextBox myCommentTxt;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SlimForm));

            this.myPanel = new System.Windows.Forms.Panel();
            this.myTimeElapsed = new System.Windows.Forms.Label();
            this.myStopRecordBtn = new System.Windows.Forms.Button();
            this.myCommentTxt = new System.Windows.Forms.TextBox();
            this.myPanel.SuspendLayout();
            this.SuspendLayout();

            this.myPanel.Controls.Add(this.myTimeElapsed);
            this.myPanel.Controls.Add(this.myStopRecordBtn);
            this.myPanel.Controls.Add(this.myCommentTxt);
            this.myPanel.Location = new System.Drawing.Point(0, 0);
            this.myPanel.Name = "panel1";
            this.myPanel.Size = new System.Drawing.Size(223, 29);
            this.myPanel.TabIndex = 17;
            this.myPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);

            this.myTimeElapsed.AutoSize = true;
            this.myTimeElapsed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.myTimeElapsed.Location = new System.Drawing.Point(166, 8);
            this.myTimeElapsed.Name = "myTimeElapsed";
            this.myTimeElapsed.Size = new System.Drawing.Size(51, 15);
            this.myTimeElapsed.TabIndex = 20;
            this.myTimeElapsed.Text = "00:00:00";

            this.myStopRecordBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.myStopRecordBtn.Image = GetImage("stop");
            this.myStopRecordBtn.Location = new System.Drawing.Point(3, 3);
            this.myStopRecordBtn.Name = "myStopRecordBtn";
            this.myStopRecordBtn.Size = new System.Drawing.Size(23, 23);
            this.myStopRecordBtn.TabIndex = 18;
            this.myStopRecordBtn.UseVisualStyleBackColor = true;
            this.myStopRecordBtn.Click += new System.EventHandler(this.OnStopButton);

            this.myCommentTxt.Location = new System.Drawing.Point(32, 5);
            this.myCommentTxt.Name = "myCommentTxt";
            this.myCommentTxt.Size = new System.Drawing.Size(128, 20);
            this.myCommentTxt.TabIndex = 21;

            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(223, 29);
            this.Controls.Add(this.myPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SlimForm";
            this.ControlBox = false;
            this.Text = String.Empty;
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
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
                return new Bitmap(image, new Size(17, 17));
            }
        }
    }
}