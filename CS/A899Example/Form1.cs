using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace A899Example
{
    public partial class Form1 : Form
    {
        DataSet dataSet;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Fill data set with two tables and set up Master - Detail relationship between them
            dataSet = new DataSet();
            dataSet.Tables.Add(FillMasterTable());
            dataSet.Tables.Add(FillDetailTable());

            DataColumn keyColumn = dataSet.Tables["MasterTable"].Columns["Vendor"];
            DataColumn foreignKeyColumn = dataSet.Tables["DetailTable"].Columns["Vendor"];
            dataSet.Relations.Add("Models", keyColumn, foreignKeyColumn);

            // Bind master table to the grid control
            gridControl1.DataSource = dataSet.Tables["MasterTable"];
            gridView1.OptionsDetail.AllowExpandEmptyDetails = true;

            // Create and configure detail grid view
            GridView detailGridView = new GridView(gridControl1);
            gridControl1.LevelTree.Nodes.Add("Models", detailGridView);
            detailGridView.PopulateColumns(dataSet.Tables["DetailTable"]);
            detailGridView.Columns["Vendor"].VisibleIndex = -1;
        }

        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;
            if(e.Column.VisibleIndex == 0 && view.IsMasterRowEmpty(e.RowHandle))
                (e.Cell as GridCellInfo).CellButtonRect = Rectangle.Empty;
        }

        public static DataTable FillMasterTable()
        {
            DataTable _masterTable = new DataTable();
            _masterTable.TableName = "MasterTable";
            _masterTable.Columns.Add(new DataColumn("Vendor", typeof(string)));
            _masterTable.Columns.Add(new DataColumn("Availability", typeof(bool)));
            _masterTable.Rows.Add(new object[] { "HP", true });
            _masterTable.Rows.Add(new object[] { "Dell", false });
            return _masterTable;
        }

        public static DataTable FillDetailTable()
        {
            DataTable _detailTable = new DataTable();
            _detailTable.TableName = "DetailTable";
            _detailTable.Columns.Add(new DataColumn("Vendor", typeof(string)));
            _detailTable.Columns.Add(new DataColumn("Model", typeof(string)));
            _detailTable.Columns.Add(new DataColumn("Availability", typeof(bool)));
            _detailTable.Rows.Add(new object[] { "HP", "Pavillion", true });
            _detailTable.Rows.Add(new object[] { "HP", "Mini", true });
            return _detailTable;
        }
    }
}
