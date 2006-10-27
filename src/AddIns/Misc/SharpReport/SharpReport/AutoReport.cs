//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.2032
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------


using System;
using System.Data;
using System.Drawing;

using ICSharpCode.Core;
using SharpReport.ReportItems;
using SharpReportCore;

/// <summary>
/// This Class creates a Report based on the DataSource selected in the Wizard
/// </summary>
/// <remarks>
/// 	created by - Forstmeier Peter
/// 	created on - 07.12.2004 13:56:07
/// </remarks>
	
namespace SharpReport {
	public class AutoReport : object,IDisposable {
			
		SharpReport.Designer.IDesignableFactory iDesignableFactory;

		public AutoReport() {
			iDesignableFactory = new SharpReport.Designer.IDesignableFactory();
		}
			
			
		#region Build ReportItemsCollection
			
		/// <summary>
		/// Build BaseDataItems from a schemaDataTable
		/// </summary>
		/// <param name="model">a valid ReportModel</param>
		/// <param name="schemaTable">SchemaDefinition Datatable</param>
		/// <returns>Collection of BaseDataItems</returns>
			
		public ReportItemCollection DataItemsFromTable (ReportModel model,DataTable schemaTable) {
			ReportItemCollection itemCol = new ReportItemCollection();
				
			for (int i = 0;i < schemaTable.Rows.Count;i++){
				DataRow r = schemaTable.Rows[i];
				ReportDataItem rItem = new ReportDataItem();
				rItem.ColumnName = r["ColumnName"].ToString();
				rItem.BaseTableName = r["BaseTableName"].ToString();
				rItem.Text = rItem.ColumnName;
				rItem.DataType = r["DataType"].ToString();
				rItem.Location = new Point (i * 30,5);
				itemCol.Add (rItem);
			}
			return itemCol;
		}
		
		
		/// <summary>
		/// Build BaseDataItems from a *.xsd (Schema) File
		/// </summary>
		/// <param name="model">a valid ReportModel</param>
		/// <param name="dataSet">DataSet from *.xsd File</param>
		/// <returns> Collection of BaseDataItems</returns>
			
		public ReportItemCollection DataItemsFromSchema (ReportModel model,DataSet dataSet) {
			if (dataSet.Tables.Count > 1) {
				MessageService.ShowError ("AutoBuildFromDataSet : at this time no more than one table is allowed " + dataSet.Tables.Count.ToString());
				throw new ArgumentException ("Too much Tables in DataSet");
			}
				
			ReportItemCollection itemCol = new ReportItemCollection();
			foreach (DataTable tbl in dataSet.Tables) {
				DataColumn col;
				for (int i = 0;i < tbl.Columns.Count ;i++ ) {
					col = tbl.Columns[i];
					ReportDataItem rItem = new ReportDataItem();
					rItem.ColumnName = col.ColumnName;	
					rItem.DbValue = col.ColumnName;
					rItem.BaseTableName = tbl.TableName;
					rItem.DataType = col.DataType.ToString();
					rItem.Location = new Point (i * 30,5);
					itemCol.Add (rItem);
				}
			}
			return itemCol;	
		}
		
		#endregion
		
		#region Build report's
		
		///<summary>
		/// Build a <see cref="ColumnCollection"></see> this collection holds all the fields
		/// comming from the DataSource
		///</summary>
	
		public ColumnCollection AbstractColumnsFromDataSet(DataSet dataSet) {
			if (dataSet.Tables.Count > 1) {
				MessageService.ShowError ("AutoBuildFromDataSet : at this time no more than one table is allowed " + dataSet.Tables.Count.ToString());
				throw new ArgumentException ("Too much Tables in DataSet");
			}
			
			ColumnCollection collection = new ColumnCollection();
			foreach (DataTable tbl in dataSet.Tables) {
				DataColumn col;
				for (int i = 0;i < tbl.Columns.Count ;i++ ) {
					col = tbl.Columns[i];
					AbstractColumn abstrColumn = new AbstractColumn();
					abstrColumn.ColumnName = col.ColumnName;	
					abstrColumn.DataType = col.DataType;
					collection.Add (abstrColumn);
				}
			}
			return collection;
		}
		
		
		
	
		public ReportItemCollection AutoDataColumns(ReportItemCollection col) {
			if (col != null) {
				ReportItemCollection itemCol = new ReportItemCollection();
				ReportDataItem sourceItem = null;
				for (int i = 0;i < col.Count ;i++ ){
					ReportDataItem destItem = new ReportDataItem();
					sourceItem = (ReportDataItem)col[i];
					
					destItem.VisualControl.Text = sourceItem.ColumnName;
					destItem.ColumnName = sourceItem.ColumnName;
					destItem.DbValue = sourceItem.DbValue;
					destItem.BaseTableName = sourceItem.BaseTableName;
					destItem.DataType = sourceItem.DataType;
					destItem.Location = new Point (i * 30,5);
					itemCol.Add(destItem);
				}
				return itemCol;
			} else {
				throw new ArgumentNullException ("AutoReport:ReportItemCollection");
			}
		}
	
		#endregion
		
		#region HeaderColumns
		/// <summary>
		/// Build Headerline from a schemaDataTable
		/// </summary>
		/// <param name="model">the ReportModel</param>
		/// <param name="section">location of the Headerlines</param>
		/// <param name="schemaTable">the Schematable with ColumnInrofmations</param>
		/// <param name="setOnTop">Locate the Columns of Top or an Bottom of the Section</param>
		/// <returns>a Collection of BaseTextItems</returns>
			
		public  ReportItemCollection HeaderColumnsFromTable (BaseSection section,DataTable schemaTable,bool setOnTop) {
			ReportItemCollection itemCol = new ReportItemCollection();
			for (int i = 0;i < schemaTable.Rows.Count;i++){
				DataRow r = schemaTable.Rows[i];
				BaseTextItem rItem = (BaseTextItem)iDesignableFactory.Create("ReportTextItem");
				if (rItem != null) {
					rItem.Text = r["ColumnName"].ToString();
					if (setOnTop) {
						rItem.Location = new Point (i * 30,1);
					} else {
						int y = section.Size.Height - rItem.Size.Height - 5;
						rItem.Location = new Point (i * 30,y);
					}
					itemCol.Add (rItem);
					} else {
					throw new ArgumentException("AutoHeaderfromTable : Unable to create ReporttextItem");
				}
			}
			return itemCol;
		}
		
		
		public ReportItemCollection HeaderColumnsFromReportItems(ReportItemCollection reportItemCollection,BaseSection section,bool setOnTop) {
			if (reportItemCollection == null) {
				throw new ArgumentNullException ("reportItemCollection");
			}
			if (section == null) {
				throw new ArgumentNullException ("section");
			}
			
			ReportItemCollection itemCol = new ReportItemCollection();
			ReportDataItem sourceItem = null;
			for (int i = 0;i < reportItemCollection.Count ;i++ ){
				BaseTextItem rItem = (BaseTextItem)iDesignableFactory.Create("ReportTextItem");
				if (rItem != null) {
					sourceItem = (ReportDataItem)reportItemCollection[i];
					
					rItem.Text = sourceItem.ColumnName;
					rItem.Text = sourceItem.ColumnName;
					if (setOnTop) {
						rItem.Location = new Point (i * 30,1);
					} else {
						int y = section.Size.Height - rItem.Size.Height - 5;
						rItem.Location = new Point (i * 30,y);
					}
					itemCol.Add(rItem);
				}
			}
			return itemCol;
		}
		
		#endregion
		/*
		#region Standarts for all reports (Headlines etc)
		
		/// <summary>
		/// Insert a <see cref="ReportTextItem"></see> in the PageHeader with
		/// the <see cref="ReportModel.ReportSettings.ReportName"></see> as
		/// text
		/// </summary>
		/// <param name="model">ReportModel</param>
		public void CreatePageHeader (ReportModel model) {
			BaseSection section = model.PageHeader;
			section.SuspendLayout();
			SharpReport.Designer.IDesignableFactory gf = new SharpReport.Designer.IDesignableFactory();
			BaseTextItem item = (BaseTextItem)gf.Create ("ReportTextItem");
			item.SuspendLayout();
			item.Text = model.ReportSettings.ReportName;
			item.Font = CopyFont(model.ReportSettings.DefaultFont);
			item.Location = new Point (0,0);
			item.Size = new Size (item.Size.Width,item.Font.Height + SharpReportCore.GlobalValues.EnlargeControl);
			section.Items.Add (item);
			item.ResumeLayout();
			section.ResumeLayout();
		}
		
		///<summary>
		/// Insert Function 'PageNumber' in Section PageFooter
		/// </summary>
		/// <param name="model">ReportModel</param>
		
		public void CreatePageNumber (ReportModel model) {
			BaseSection section = model.PageFooter;
			section.SuspendLayout();
			FunctionFactory gf = new FunctionFactory();
			PageNumber pageNumber = (PageNumber)gf.Create ("PageNumber");
			pageNumber.SuspendLayout();
			
			pageNumber.Text = ResourceService.GetString("SharpReport.Toolbar.Functions.PageNumber");
			pageNumber.Location = new Point (0,0);
			section.Items.Add(pageNumber);
			pageNumber.ResumeLayout();
			section.ResumeLayout();
		}
			
		#endregion
		*/
		
		
		
		#region System.IDisposable interface implementation
		public void Dispose() {
		}
		#endregion
	}
}
	
