using System;
using System.Drawing;
using System.Windows.Forms;
using Plainion.WhiteRabbit.Model;
using Plainion.WhiteRabbit.Presentation;
using Plainion.WhiteRabbit.Properties;
using Plainion.WhiteRabbit.View;

namespace Plainion.WhiteRabbit
{
    public partial class MainUI : Form, IView
    {
        private const string BEGIN_COLUMN_NAME = "myBeginCol";
        private const string END_COLUMN_NAME = "myEndCol";
        private const string DURATION_COLUMN_NAME = "myDurationCol";
        private const string COMMENT_COLUMN_NAME = "myCommentCol";

        private readonly BindingSource myBindingSource;
        private readonly Controller myController;
        private readonly Channel myChannel;

        public MainUI(Controller controller)
        {
            InitializeComponent();

            Text = "Plainion WhiteRabbit";

            myController = controller;

            myChannel = new Channel();

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

        public Channel Channel
        {
            get { return myChannel; }
        }

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
                    //myTableView.InvalidateRow( e.RowIndex );
                    myTableView.RefreshEdit();
                }

                DateTime result;
                if (!DateTime.TryParse(value, out result))
                {
                    cell.Style.ForeColor = Color.Red;
                    //DataGridViewAdvancedBorderStyle style = new DataGridViewAdvancedBorderStyle();
                    //style.All = DataGridViewAdvancedCellBorderStyle.OutsetDouble;
                    //cell.AdjustCellBorderStyle( style, style, false, false, false, false );
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
    }
}