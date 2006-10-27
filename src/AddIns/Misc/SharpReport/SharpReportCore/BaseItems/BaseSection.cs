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
using System.ComponentModel;

/// <summary>
/// This Class is the BaseClass for <see cref="ReportSection"></see>
/// </summary>


namespace SharpReportCore {
	public class BaseSection : SharpReportCore.BaseReportObject {
		
		private int sectionMargin;
//		private bool pageBreakBefore;
		private bool pageBreakAfter;
		
		private ReportItemCollection items;
		
		public event EventHandler<SectionEventArgs> SectionPrinting;
		public event EventHandler<SectionEventArgs> SectionPrinted;
		
		
		#region Constructors
		
		public BaseSection(): base() {
			base.Name = String.Empty;
		}
		
		public BaseSection (string sectionName) :base(){
			base.Name = sectionName;
		}
		
		#endregion
		
		#region Rendering
		
		public override void Render(ReportPageEventArgs rpea){
			this.NotifyPrinting();
			base.Render(rpea);
			this.NotifyPrinted();
		}
		
		private void NotifyPrinting () {
			if (this.SectionPrinting != null) {
				SectionEventArgs ea = new SectionEventArgs (this);
				SectionPrinting (this,ea);
			} 
		}
		
		private void NotifyPrinted () {
			if (this.SectionPrinted != null) {
				SectionEventArgs ea = new SectionEventArgs (this);
				SectionPrinted (this,ea);
			}
		}
		
		#endregion
		
		#region properties
		
		
		public  int SectionMargin {
			get {
				return this.sectionMargin;
			}
			set {
				this.sectionMargin = value;
			}
		}
	
		
	
		[Browsable(false)]
		public ReportItemCollection Items{
			get {
				if (this.items == null) {
					items = new ReportItemCollection();
				}
				return items;
			}
		}
		
		/*
		public virtual bool PageBreakBefore {
			get {
				return pageBreakBefore;
			}
			set {
				pageBreakBefore = value;
				NotifyPropertyChanged ("PageBreakBefore");
			}
		}
		*/
		public virtual bool PageBreakAfter {
			get {
				return pageBreakAfter;
			}
			set {
				pageBreakAfter = value;
				NotifyPropertyChanged ("PageBreakAfter");
			}
		}
		
		
		#endregion
		
		#region System.IDisposable interface implementation
		public override void Dispose () {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		
		
		protected override void Dispose(bool disposing) {
			try {
				if (disposing){
					if (this.items != null) {
						this.items.Clear();
						this.items = null;
					}
				}
			} finally {
				base.Dispose(disposing);
			}
		}
		
		#endregion
	}
}
