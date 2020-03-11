namespace EnvironmentalInspectionManagement.Utilities.UtilityForms
{
    #region Usings
    using System.Collections.Generic;
    using System.Windows.Forms;
    using System.Drawing;
    using System;
    #endregion

    public partial class MessageGridForm : Form
    {
        #region Properties
        public List<object> GridDataSource;
        public string FormCaption;
        private DataGridViewCell _activeCell;
        #endregion

        #region Constructor
        public MessageGridForm()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.None;
            CenterToScreen();
            MaximizeBox = false;
        }
        #endregion

        #region Event Methods
        protected void FormLoad(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(FormCaption))
                captionLbl.Text = FormCaption;

            if (GridDataSource != null)
                FormDataGrid.DataSource = GridDataSource;
        }

        protected void ButtonClicked(object sender, EventArgs e)
        {
            Button currentSender = (sender as Button);

            if (currentSender != null)
            {
                switch (currentSender.Name)
                {
                    case "okBtn":
                        OkOperation();
                        break;
                    default:
                        $"Invalid Event Handle {currentSender.Name}".ErrMsg();
                        break;
                }
            }
        }

        protected void DataGridViewMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu contextMenu = new ContextMenu();
                contextMenu.MenuItems.Add(new MenuItem("Copy", CopyOnClick));


                DataGridView.HitTestInfo hitInfo = FormDataGrid.HitTest(e.X, e.Y);

                if (hitInfo != null && hitInfo.Type == DataGridViewHitTestType.Cell)
                {
                    _activeCell = FormDataGrid[hitInfo.ColumnIndex, hitInfo.RowIndex];
                    _activeCell.Selected = true;
                    contextMenu.Show(FormDataGrid, new Point(e.X, e.Y));
                }
            }
        }

        protected void CopyOnClick(object sender, EventArgs e)
        {
            if (!_activeCell.IsNull() && !_activeCell.Value.IsNull())
                Clipboard.SetText(_activeCell.Value.ToString());
        }
        #endregion

        #region Custom Methods
        private void OkOperation()
        {
            DestroyHandle();
            Dispose();
        }
        #endregion
    }
}
