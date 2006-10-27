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
using ICSharpCode.Core;
using SharpReportCore;

	/// <summary>
	/// This class is used to AutoGenerate a (PullData) Report
	/// (Reports, that grap the Data by themselve)
	/// </summary>
	/// <remarks>
	/// 	created by - Forstmeier Peter
	/// 	created on - 07.09.2005 13:23:14
	/// </remarks>


namespace ReportGenerator {	
	public class GeneratePullDataReport : AbstractReportGenerator {
		
		
		public GeneratePullDataReport(Properties customizer,
		                              ReportModel reportModel):base(customizer,reportModel){

			if (customizer == null) {
				throw new ArgumentException("customizer");
			}
			if (reportModel == null) {
				throw new ArgumentException("reportModel");
			}
			
			if (base.ReportModel.ReportSettings.DataModel != GlobalEnums.PushPullModel.PullData) {
				throw new ArgumentException ("Wrong DataModel in GeneratePullDataReport");
			}
			base.ReportItemCollection.Clear();
			base.ReportItemCollection.AddRange((ReportItemCollection)base.Customizer.Get ("ReportItemCollection"));
		}
		
		#region ReportGenerator.IReportGenerator interface implementation
		public override void GenerateReport() {
			try {
				base.ReportModel.ReportSettings.ReportType = GlobalEnums.ReportType.DataReport;
				base.ReportModel.ReportSettings.DataModel = GlobalEnums.PushPullModel.PullData;
				
				this.ReportModel.ReportSettings.AvailableFieldsCollection = 
					(ColumnCollection)base.Customizer.Get ("ColumnCollection");;
				base.GenerateReport();

				base.HeaderColumnsFromReportItems (base.ReportModel.PageHeader);
				base.BuildDataSection (base.ReportModel.DetailSection);

				using (TableLayout layout = new TableLayout(base.ReportModel)){
					layout.BuildLayout();
				}
				
				base.AdjustAllNames();
			} catch (Exception) {
				throw;
			}
		
		}
		
		
		#endregion
	}
}
