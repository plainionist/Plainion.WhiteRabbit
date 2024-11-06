using System;
using System.Windows.Forms;

namespace Plainion.WhiteRabbit.View
{
    public enum ReportScope
    {
        Day,
        Week,
        Month
    }

    public class ReportForm : Form
    {
        private Button myOkBtn;
        private WebBrowser myBrowser;

        internal ReportForm(IController controller, ReportScope scope)
        {
            InitializeComponent();

            if (scope == ReportScope.Day)
            {
                myBrowser.Navigate(controller.GenerateDayReport(controller.CurrentDay));
            }
            else if (scope == ReportScope.Week)
            {
                myBrowser.Navigate(controller.GenerateWeekReport(controller.CurrentDay));
            }
            else if (scope == ReportScope.Month)
            {
                myBrowser.Navigate(controller.GenerateMonthReport(controller.CurrentDay));
            }
        }

        private void OnOkClicked(object sender, EventArgs e)
        {
            Close();
        }

        private void InitializeComponent()
        {
            this.myOkBtn = new Button();
            this.myBrowser = new WebBrowser();
            this.SuspendLayout();
            // 
            // myOkBtn
            // 
            this.myOkBtn.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.myOkBtn.Location = new System.Drawing.Point(656, 501);
            this.myOkBtn.Margin = new Padding(4, 3, 4, 3);
            this.myOkBtn.Name = "myOkBtn";
            this.myOkBtn.Size = new System.Drawing.Size(88, 27);
            this.myOkBtn.TabIndex = 0;
            this.myOkBtn.Text = "&Ok";
            this.myOkBtn.UseVisualStyleBackColor = true;
            this.myOkBtn.Click += new EventHandler(this.OnOkClicked);
            // 
            // myBrowser
            // 
            this.myBrowser.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
            | AnchorStyles.Left)
            | AnchorStyles.Right)));
            this.myBrowser.Location = new System.Drawing.Point(4, 8);
            this.myBrowser.Margin = new Padding(4, 3, 4, 3);
            this.myBrowser.MinimumSize = new System.Drawing.Size(23, 23);
            this.myBrowser.Name = "myBrowser";
            this.myBrowser.Size = new System.Drawing.Size(740, 487);
            this.myBrowser.TabIndex = 1;
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 530);
            this.Controls.Add(this.myBrowser);
            this.Controls.Add(this.myOkBtn);
            this.Margin = new Padding(4, 3, 4, 3);
            this.Name = "ReportForm";
            this.SizeGripStyle = SizeGripStyle.Hide;
            this.Text = "Report";
            this.ResumeLayout(false);

        }


    }
}
