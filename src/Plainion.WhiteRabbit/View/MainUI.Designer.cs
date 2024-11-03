using System;
using System.Drawing;

namespace Plainion.WhiteRabbit
{
    partial class MainUI
    {
        private System.Windows.Forms.DataGridView myTableView;
        private System.Windows.Forms.DateTimePicker myDateTime;
        private System.Windows.Forms.ContextMenuStrip myPreferencesMenu;
        private System.Windows.Forms.ToolStripMenuItem selectDatabaseToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip myTableContextMenu;
        private System.Windows.Forms.ToolStripMenuItem myTableContextMenu_DeleteSelectedRow;
        private System.Windows.Forms.Button myRecordInitBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.NotifyIcon myNotifyIcon;
        private InstantUpdate.Controls.SplitButton mySplitbutton;
        private System.Windows.Forms.ToolTip myToolTip;
        private System.Windows.Forms.ToolStripMenuItem dayReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem weekReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem monthReportToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn myBeginCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn myEndCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn myDurationCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn myCommentCol;
        private System.ComponentModel.IContainer myComponents;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.myComponents = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainUI));
            this.myTableView = new System.Windows.Forms.DataGridView();
            this.myBeginCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.myEndCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.myDurationCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.myCommentCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.myTableContextMenu = new System.Windows.Forms.ContextMenuStrip(this.myComponents);
            this.myTableContextMenu_DeleteSelectedRow = new System.Windows.Forms.ToolStripMenuItem();
            this.myDateTime = new System.Windows.Forms.DateTimePicker();
            this.myPreferencesMenu = new System.Windows.Forms.ContextMenuStrip(this.myComponents);
            this.selectDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.dayReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.weekReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.monthReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.myRecordInitBtn = new System.Windows.Forms.Button();
            this.myNotifyIcon = new System.Windows.Forms.NotifyIcon(this.myComponents);
            this.myToolTip = new System.Windows.Forms.ToolTip(this.myComponents);
            this.mySplitbutton = new InstantUpdate.Controls.SplitButton();
            ((System.ComponentModel.ISupportInitialize)(this.myTableView)).BeginInit();
            this.myTableContextMenu.SuspendLayout();
            this.myPreferencesMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // myTableView
            // 
            this.myTableView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.myTableView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.myTableView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.myBeginCol,
            this.myEndCol,
            this.myDurationCol,
            this.myCommentCol});
            this.myTableView.Location = new System.Drawing.Point(3, 32);
            this.myTableView.Name = "myTableView";
            this.myTableView.RowHeadersVisible = false;
            this.myTableView.Size = new System.Drawing.Size(511, 320);
            this.myTableView.TabIndex = 3;
            this.myTableView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.myTableView_CellEndEdit);
            this.myTableView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.myTableView_CellValidating);
            this.myTableView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.myTableView_KeyUp);
            this.myTableView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.myTableView_MouseDown);
            this.myTableView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.myTableView_MouseUp);
            // 
            // myBeginCol
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "t";
            dataGridViewCellStyle1.NullValue = null;
            this.myBeginCol.DefaultCellStyle = dataGridViewCellStyle1;
            this.myBeginCol.HeaderText = "Begin";
            this.myBeginCol.Name = "myBeginCol";
            this.myBeginCol.Width = 40;
            // 
            // myEndCol
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "t";
            dataGridViewCellStyle2.NullValue = null;
            this.myEndCol.DefaultCellStyle = dataGridViewCellStyle2;
            this.myEndCol.HeaderText = "End";
            this.myEndCol.Name = "myEndCol";
            this.myEndCol.Width = 40;
            // 
            // myDurationCol
            // 
            this.myDurationCol.HeaderText = "Duration";
            this.myDurationCol.MinimumWidth = 30;
            this.myDurationCol.Name = "myDurationCol";
            this.myDurationCol.Width = 65;
            // 
            // myCommentCol
            // 
            this.myCommentCol.HeaderText = "Comment";
            this.myCommentCol.Name = "myCommentCol";
            this.myCommentCol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.myCommentCol.Width = 300;
            // 
            // myTableContextMenu
            // 
            this.myTableContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.myTableContextMenu_DeleteSelectedRow});
            this.myTableContextMenu.Name = "myTableContextMenu";
            this.myTableContextMenu.Size = new System.Drawing.Size(177, 54);
            // 
            // myTableContextMenu_DeleteSelectedRow
            // 
            this.myTableContextMenu_DeleteSelectedRow.Name = "myTableContextMenu_DeleteSelectedRow";
            this.myTableContextMenu_DeleteSelectedRow.Size = new System.Drawing.Size(176, 22);
            this.myTableContextMenu_DeleteSelectedRow.Text = "Delete selected row";
            this.myTableContextMenu_DeleteSelectedRow.Click += new System.EventHandler(this.deleteSelectedRowMenuItem_Click);
            // 
            // myDateTime
            // 
            this.myDateTime.Location = new System.Drawing.Point(3, 4);
            this.myDateTime.Name = "myDateTime";
            this.myDateTime.Size = new System.Drawing.Size(225, 20);
            this.myDateTime.TabIndex = 4;
            this.myDateTime.ValueChanged += new System.EventHandler(this.myDateTime_ValueChanged);
            // 
            // myPreferencesMenu
            // 
            this.myPreferencesMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectDatabaseToolStripMenuItem,
            this.toolStripSeparator2,
            this.dayReportToolStripMenuItem,
            this.weekReportToolStripMenuItem,
            this.monthReportToolStripMenuItem});
            this.myPreferencesMenu.Name = "myPreferencesMenu";
            this.myPreferencesMenu.Size = new System.Drawing.Size(177, 104);
            // 
            // selectDatabaseToolStripMenuItem
            // 
            this.selectDatabaseToolStripMenuItem.Name = "selectDatabaseToolStripMenuItem";
            this.selectDatabaseToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.selectDatabaseToolStripMenuItem.Text = "Select database ...";
            this.selectDatabaseToolStripMenuItem.Click += new System.EventHandler(this.selectDatabaseToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(173, 6);
            // 
            // dayReportToolStripMenuItem
            // 
            this.dayReportToolStripMenuItem.Name = "dayReportToolStripMenuItem";
            this.dayReportToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.dayReportToolStripMenuItem.Text = "Report day ...";
            this.dayReportToolStripMenuItem.Click += new System.EventHandler(this.OnReportToday);
            // 
            // weekReportToolStripMenuItem
            // 
            this.weekReportToolStripMenuItem.Name = "weekReportToolStripMenuItem";
            this.weekReportToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.weekReportToolStripMenuItem.Text = "Report week ...";
            this.weekReportToolStripMenuItem.Click += new System.EventHandler(this.OnReportWeek);
            // 
            // monthReportToolStripMenuItem
            // 
            this.monthReportToolStripMenuItem.Name = "monthReportToolStripMenuItem";
            this.monthReportToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.monthReportToolStripMenuItem.Text = "Report month  ...";
            this.monthReportToolStripMenuItem.Click += new System.EventHandler(this.OnReportMonth);
            // 
            // myRecordInitBtn
            // 
            this.myRecordInitBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.myRecordInitBtn.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.myRecordInitBtn.FlatAppearance.BorderSize = 0;
            this.myRecordInitBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.myRecordInitBtn.Image = GetImage("play");
            this.myRecordInitBtn.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.myRecordInitBtn.Location = new System.Drawing.Point(419, 3);
            this.myRecordInitBtn.Name = "myRecordInitBtn";
            this.myRecordInitBtn.Size = new System.Drawing.Size(23, 23);
            this.myRecordInitBtn.TabIndex = 14;
            this.myToolTip.SetToolTip(this.myRecordInitBtn, "Start");
            this.myRecordInitBtn.UseVisualStyleBackColor = true;
            this.myRecordInitBtn.Click += new System.EventHandler(this.myRecordInitBtn_Click);
            // 
            // myNotifyIcon
            // 
            this.myNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("myNotifyIcon.Icon")));
            this.myNotifyIcon.Text = "White Rabbit";
            this.myNotifyIcon.DoubleClick += new System.EventHandler(this.myNotifyIcon_DoubleClick);
            // 
            // mySplitbutton
            // 
            this.mySplitbutton.AutoSize = true;
            this.mySplitbutton.ContextMenuStrip = this.myPreferencesMenu;
            this.mySplitbutton.FlatAppearance.BorderSize = 0;
            this.mySplitbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mySplitbutton.Image = GetImage("properties");
            this.mySplitbutton.Location = new System.Drawing.Point(465, 3);
            this.mySplitbutton.Name = "mySplitbutton";
            this.mySplitbutton.Size = new System.Drawing.Size(46, 23);
            this.mySplitbutton.SplitMenuStrip = this.myPreferencesMenu;
            this.mySplitbutton.TabIndex = 16;
            this.myToolTip.SetToolTip(this.mySplitbutton, "Properties");
            this.mySplitbutton.UseVisualStyleBackColor = true;
            // 
            // MainUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 357);
            this.Controls.Add(this.myDateTime);
            this.Controls.Add(this.myTableView);
            this.Controls.Add(this.mySplitbutton);
            this.Controls.Add(this.myRecordInitBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainUI";
            ((System.ComponentModel.ISupportInitialize)(this.myTableView)).EndInit();
            this.myTableContextMenu.ResumeLayout(false);
            this.myPreferencesMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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

        protected override void Dispose(bool disposing)
        {
            if (disposing && (myComponents != null))
            {
                myComponents.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

