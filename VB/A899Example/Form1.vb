Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo

Namespace A899Example
	Partial Public Class Form1
		Inherits Form
		Private dataSet As DataSet

		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			' Fill data set with two tables and set up Master - Detail relationship between them
			dataSet = New DataSet()
			dataSet.Tables.Add(FillMasterTable())
			dataSet.Tables.Add(FillDetailTable())

			Dim keyColumn As DataColumn = dataSet.Tables("MasterTable").Columns("Vendor")
			Dim foreignKeyColumn As DataColumn = dataSet.Tables("DetailTable").Columns("Vendor")
			dataSet.Relations.Add("Models", keyColumn, foreignKeyColumn)

			' Bind master table to the grid control
			gridControl1.DataSource = dataSet.Tables("MasterTable")
			gridView1.OptionsDetail.AllowExpandEmptyDetails = True

			' Create and configure detail grid view
			Dim detailGridView As New GridView(gridControl1)
			gridControl1.LevelTree.Nodes.Add("Models", detailGridView)
			detailGridView.PopulateColumns(dataSet.Tables("DetailTable"))
			detailGridView.Columns("Vendor").VisibleIndex = -1
		End Sub

		Private Sub gridView1_CustomDrawCell(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs) Handles gridView1.CustomDrawCell
			Dim view As GridView = TryCast(sender, GridView)
			If e.Column.VisibleIndex = 0 AndAlso view.IsMasterRowEmpty(e.RowHandle) Then
				TryCast(e.Cell, GridCellInfo).CellButtonRect = Rectangle.Empty
			End If
		End Sub

		Public Shared Function FillMasterTable() As DataTable
			Dim _masterTable As New DataTable()
			_masterTable.TableName = "MasterTable"
			_masterTable.Columns.Add(New DataColumn("Vendor", GetType(String)))
			_masterTable.Columns.Add(New DataColumn("Availability", GetType(Boolean)))
			_masterTable.Rows.Add(New Object() { "HP", True })
			_masterTable.Rows.Add(New Object() { "Dell", False })
			Return _masterTable
		End Function

		Public Shared Function FillDetailTable() As DataTable
			Dim _detailTable As New DataTable()
			_detailTable.TableName = "DetailTable"
			_detailTable.Columns.Add(New DataColumn("Vendor", GetType(String)))
			_detailTable.Columns.Add(New DataColumn("Model", GetType(String)))
			_detailTable.Columns.Add(New DataColumn("Availability", GetType(Boolean)))
			_detailTable.Rows.Add(New Object() { "HP", "Pavillion", True })
			_detailTable.Rows.Add(New Object() { "HP", "Mini", True })
			Return _detailTable
		End Function
	End Class
End Namespace
