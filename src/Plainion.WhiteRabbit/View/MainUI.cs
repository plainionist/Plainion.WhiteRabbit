using System;
using System.Drawing;
using System.Windows.Forms;
using InstantUpdate.Controls;
using Plainion.WhiteRabbit.Model;
using Plainion.WhiteRabbit.Presentation;
using Plainion.WhiteRabbit.Properties;
using Plainion.WhiteRabbit.View;

namespace Plainion.WhiteRabbit
{
    public class MainUI : Form, IView
    {
        private DataGridView myTableView;
        private DateTimePicker myDateTime;
        private ContextMenuStrip myPreferencesMenu;
        private ToolStripMenuItem selectDatabaseToolStripMenuItem;
        private ContextMenuStrip myTableContextMenu;
        private ToolStripMenuItem myTableContextMenu_DeleteSelectedRow;
        private Button myRecordInitBtn;
        private ToolStripSeparator toolStripSeparator2;
        private SplitButton mySplitbutton;
        private ToolTip myToolTip;
        private ToolStripMenuItem dayReportToolStripMenuItem;
        private ToolStripMenuItem weekReportToolStripMenuItem;
        private ToolStripMenuItem monthReportToolStripMenuItem;
        private DataGridViewTextBoxColumn myBeginCol;
        private DataGridViewTextBoxColumn myEndCol;
        private DataGridViewTextBoxColumn myDurationCol;
        private DataGridViewTextBoxColumn myCommentCol;
        private System.ComponentModel.IContainer myComponents;

        private const string BEGIN_COLUMN_NAME = "myBeginCol";
        private const string END_COLUMN_NAME = "myEndCol";
        private const string DURATION_COLUMN_NAME = "myDurationCol";
        private const string COMMENT_COLUMN_NAME = "myCommentCol";

        private readonly BindingSource myBindingSource;
        private readonly Controller myController;

        public MainUI(Controller controller)
        {
            InitializeComponent();

            Text = "Plainion WhiteRabbit";

            myController = controller;

            Channel = new Channel();

            myBindingSource = new BindingSource();
            myTableView.DataSource = myBindingSource;

            // select the DataTable related to the current data
            myController.ChangeDay(myDateTime.Value);

            myTableView.AutoGenerateColumns = false;
            myTableView.Columns[BEGIN_COLUMN_NAME].DataPropertyName = ColumnNames.BEGIN;
            myTableView.Columns[END_COLUMN_NAME].DataPropertyName = ColumnNames.END;
            myTableView.Columns[DURATION_COLUMN_NAME].DataPropertyName = ColumnNames.DURATION;
            myTableView.Columns[COMMENT_COLUMN_NAME].DataPropertyName = ColumnNames.COMMENT;

            myBindingSource.DataSource = myController.CurrentDayData;

            mySplitbutton.ShowSplit = true;
        }

        public Channel Channel { get; }

        private void OnSelectDatabase(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog
            {
                SelectedPath = Settings.Default.DBStore,
                ShowNewFolderButton = true,
                Description = "Select database store"
            };

            if (dlg.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            if (dlg.SelectedPath != Settings.Default.DBStore)
            {
                Settings.Default.DBStore = dlg.SelectedPath;
                Settings.Default.Save();
            }
        }

        private void OnCellEdit(object sender, DataGridViewCellEventArgs e)
        {
            myBindingSource.EndEdit();

            myController.Database.StoreTable(myController.CurrentDayData);
        }

        private void OnDateTimeChanged(object sender, EventArgs e)
        {
            myController.ChangeDay(myDateTime.Value);
            myBindingSource.DataSource = myController.CurrentDayData;
        }

        private void OnCellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.ColumnIndex < 3)
            {
                DataGridViewCell cell = myTableView[e.ColumnIndex, e.RowIndex];

                string value = ((string)cell.EditedFormattedValue).Trim();
                if (value.EndsWith(":"))
                {
                    value += "00";
                    cell.Value = value;
                    myTableView.RefreshEdit();
                }

                DateTime result;
                if (!DateTime.TryParse(value, out result))
                {
                    cell.Style.ForeColor = Color.Red;
                }
                else
                {
                    cell.Style.ForeColor = Color.Black;
                }
            }
        }

        private void OnRecordClicked(object sender, EventArgs e)
        {
            myController.StartTimeMeasurement();
        }

        private void OnTableKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                DataObject d = myTableView.GetClipboardContent();
                Clipboard.SetDataObject(d);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                myTableView.CurrentCell.Value = Clipboard.GetText();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                myTableView.CurrentCell.Value = string.Empty;
            }
        }

        private void OnDeleteCurrentRow(object sender, EventArgs e)
        {
            myController.DeleteDayEntry(myTableView.CurrentRow.Index);
        }

        private void OnTableMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo aHTI = myTableView.HitTest(e.X, e.Y);
                bool isCellValid = IsValidCell(aHTI.ColumnIndex, aHTI.RowIndex);
                if (isCellValid)
                {
                    myTableView.ClearSelection();
                    myTableView.CurrentCell = myTableView[aHTI.ColumnIndex, aHTI.RowIndex];
                }
            }
        }

        private void OnTableMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo aHTI = myTableView.HitTest(e.X, e.Y);
                bool isCellValid = IsValidCell(aHTI.ColumnIndex, aHTI.RowIndex);

                if (isCellValid)
                {
                    myTableContextMenu.Show((Control)sender, e.X, e.Y);
                }
            }
        }

        private bool IsValidCell(int col, int row)
        {
            return ((-1 != col && -1 != row) &&
                     !(myTableView[col, row].Value is DBNull) &&
                     null != myTableView[col, row].Value);
        }

        private void OnReportToday(object sender, EventArgs e)
        {
            new ReportForm(myController, ReportScope.Day) { }.ShowDialog(this);
        }

        private void OnReportWeek(object sender, EventArgs e)
        {
            new ReportForm(myController, ReportScope.Week).ShowDialog(this);
        }

        private void OnReportMonth(object sender, EventArgs e)
        {
            new ReportForm(myController, ReportScope.Month).ShowDialog(this);
        }

        private void InitializeComponent()
        {
            this.myComponents = new System.ComponentModel.Container();
            var dataGridViewCellStyle1 = new DataGridViewCellStyle();
            var dataGridViewCellStyle2 = new DataGridViewCellStyle();
            var dataGridViewCellStyle3 = new DataGridViewCellStyle();

            this.myTableView = new DataGridView();
            this.myBeginCol = new DataGridViewTextBoxColumn();
            this.myEndCol = new DataGridViewTextBoxColumn();
            this.myDurationCol = new DataGridViewTextBoxColumn();
            this.myCommentCol = new DataGridViewTextBoxColumn();
            this.myTableContextMenu = new ContextMenuStrip(this.myComponents);
            this.myTableContextMenu_DeleteSelectedRow = new ToolStripMenuItem();
            this.myDateTime = new DateTimePicker();
            this.myPreferencesMenu = new ContextMenuStrip(this.myComponents);
            this.selectDatabaseToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.dayReportToolStripMenuItem = new ToolStripMenuItem();
            this.weekReportToolStripMenuItem = new ToolStripMenuItem();
            this.monthReportToolStripMenuItem = new ToolStripMenuItem();
            this.myRecordInitBtn = new Button();
            this.myToolTip = new ToolTip(this.myComponents);
            this.mySplitbutton = new InstantUpdate.Controls.SplitButton();
            ((System.ComponentModel.ISupportInitialize)(this.myTableView)).BeginInit();
            this.myTableContextMenu.SuspendLayout();
            this.myPreferencesMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // myTableView
            // 
            this.myTableView.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.myTableView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.myTableView.Columns.AddRange(new DataGridViewColumn[] {
            this.myBeginCol,
            this.myEndCol,
            this.myDurationCol,
            this.myCommentCol});
            this.myTableView.Location = new System.Drawing.Point(3, 32);
            this.myTableView.Name = "myTableView";
            this.myTableView.RowHeadersVisible = false;
            this.myTableView.Size = new System.Drawing.Size(511, 320);
            this.myTableView.TabIndex = 3;
            this.myTableView.CellEndEdit += new DataGridViewCellEventHandler(this.OnCellEdit);
            this.myTableView.CellValidating += new DataGridViewCellValidatingEventHandler(this.OnCellValidating);
            this.myTableView.KeyUp += new KeyEventHandler(this.OnTableKeyUp);
            this.myTableView.MouseDown += new MouseEventHandler(this.OnTableMouseDown);
            this.myTableView.MouseUp += new MouseEventHandler(this.OnTableMouseUp);
            // 
            // myBeginCol
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "t";
            dataGridViewCellStyle1.NullValue = null;
            this.myBeginCol.DefaultCellStyle = dataGridViewCellStyle1;
            this.myBeginCol.HeaderText = "Begin";
            this.myBeginCol.Name = "myBeginCol";
            this.myBeginCol.Width = 40;
            // 
            // myEndCol
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
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
            this.myCommentCol.Resizable = DataGridViewTriState.True;
            this.myCommentCol.Width = 300;
            // 
            // myTableContextMenu
            // 
            this.myTableContextMenu.Items.AddRange(new ToolStripItem[] {
            this.myTableContextMenu_DeleteSelectedRow});
            this.myTableContextMenu.Name = "myTableContextMenu";
            this.myTableContextMenu.Size = new System.Drawing.Size(177, 54);
            // 
            // myTableContextMenu_DeleteSelectedRow
            // 
            this.myTableContextMenu_DeleteSelectedRow.Name = "myTableContextMenu_DeleteSelectedRow";
            this.myTableContextMenu_DeleteSelectedRow.Size = new System.Drawing.Size(176, 22);
            this.myTableContextMenu_DeleteSelectedRow.Text = "Delete selected row";
            this.myTableContextMenu_DeleteSelectedRow.Click += new System.EventHandler(this.OnDeleteCurrentRow);
            // 
            // myDateTime
            // 
            this.myDateTime.Location = new System.Drawing.Point(3, 4);
            this.myDateTime.Name = "myDateTime";
            this.myDateTime.Size = new System.Drawing.Size(225, 20);
            this.myDateTime.TabIndex = 4;
            this.myDateTime.ValueChanged += new System.EventHandler(this.OnDateTimeChanged);
            // 
            // myPreferencesMenu
            // 
            this.myPreferencesMenu.Items.AddRange(new ToolStripItem[] {
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
            this.selectDatabaseToolStripMenuItem.Click += new System.EventHandler(this.OnSelectDatabase);
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
            this.myRecordInitBtn.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.myRecordInitBtn.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.myRecordInitBtn.FlatAppearance.BorderSize = 0;
            this.myRecordInitBtn.FlatStyle = FlatStyle.Popup;
            this.myRecordInitBtn.Image = GetImage("play");
            this.myRecordInitBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.myRecordInitBtn.Location = new System.Drawing.Point(419, 3);
            this.myRecordInitBtn.Name = "myRecordInitBtn";
            this.myRecordInitBtn.Size = new System.Drawing.Size(23, 23);
            this.myRecordInitBtn.TabIndex = 14;
            this.myRecordInitBtn.UseVisualStyleBackColor = true;
            this.myRecordInitBtn.Click += new System.EventHandler(this.OnRecordClicked);
            // 
            // mySplitbutton
            // 
            this.mySplitbutton.AutoSize = true;
            this.mySplitbutton.ContextMenuStrip = this.myPreferencesMenu;
            this.mySplitbutton.FlatAppearance.BorderSize = 0;
            this.mySplitbutton.FlatStyle = FlatStyle.Flat;
            this.mySplitbutton.Image = GetImage("properties");
            this.mySplitbutton.Location = new System.Drawing.Point(465, 3);
            this.mySplitbutton.Name = "mySplitbutton";
            this.mySplitbutton.Size = new System.Drawing.Size(46, 23);
            this.mySplitbutton.SplitMenuStrip = this.myPreferencesMenu;
            this.mySplitbutton.TabIndex = 16;
            this.mySplitbutton.UseVisualStyleBackColor = true;
            // 
            // MainUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 357);
            this.Controls.Add(this.myDateTime);
            this.Controls.Add(this.myTableView);
            this.Controls.Add(this.mySplitbutton);
            this.Controls.Add(this.myRecordInitBtn);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Icon = GetLogo();
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

        private Icon GetLogo()
        {
            using (var stream = GetType().Assembly.GetManifestResourceStream($"Plainion.WhiteRabbit.Logo.ico"))
            {
                return new Icon(stream);
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