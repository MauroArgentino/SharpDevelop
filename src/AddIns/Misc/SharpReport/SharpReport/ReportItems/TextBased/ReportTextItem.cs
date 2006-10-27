/*
 * Created by SharpDevelop.
 * User: Forstmeier Helmut
 * Date: 13.11.2004
 * Time: 22:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.ComponentModel;
using System.Drawing;

using SharpReport.Designer;
using SharpReportCore;

namespace SharpReport.ReportItems {
	/// <summary>
	/// Description of ReportTextControl.
	/// </summary>
	///<remarks>The <see cref="IDesignable"></see> Interface implementaion is already 
	/// included in the BaseClasses</remarks>
	
	

	public class ReportTextItem : BaseTextItem,IDesignable {
		
		private ReportTextControl visualControl;
		
		#region Constructor
		public ReportTextItem() : base(){
			visualControl = new ReportTextControl();

			this.Text = visualControl.Name;
			
			visualControl.ContentAlignment = base.ContentAlignment;
			visualControl.StringTrimming = base.StringTrimming;
			
//			ItemsHelper.UpdateBaseFromTextControl (this.visualControl,this);

			this.visualControl.LocationChanged += new EventHandler (OnControlChanged);
			
			this.visualControl.Click += new EventHandler(OnControlSelect);
			this.visualControl.LocationChanged += new EventHandler (OnControlChanged);
			this.visualControl.VisualControlChanged += new EventHandler (OnControlChanged);
			this.visualControl.BackColorChanged += new EventHandler (OnControlChanged);
			this.visualControl.FontChanged += new EventHandler (OnControlChanged);
			this.visualControl.ForeColorChanged += new EventHandler (OnControlChanged);
			//Event from Tracker
			this.visualControl.PropertyChanged += new PropertyChangedEventHandler (ControlPropertyChange);
			
			base.PropertyChanged += new PropertyChangedEventHandler (BasePropertyChange);
			
		}
	
		
		#endregion
		
		#region events
		
		//Tracker
		private void ControlPropertyChange (object sender, PropertyChangedEventArgs e){
			ItemsHelper.UpdateBaseFromTextControl (this.visualControl,this);
			this.HandlePropertyChanged(e.PropertyName);
		}
		
		private void BasePropertyChange (object sender, PropertyChangedEventArgs e){
			ItemsHelper.UpdateControlFromTextBase(this.visualControl,this);
			this.visualControl.ContentAlignment = base.ContentAlignment;
			this.visualControl.StringTrimming = base.StringTrimming;
			this.visualControl.DrawBorder = base.DrawBorder;
			this.HandlePropertyChanged(e.PropertyName);
		}
		

		private void OnControlChanged (object sender, EventArgs e) {
			base.SuspendLayout();
			ItemsHelper.UpdateBaseFromTextControl (this.visualControl,this);
			base.ResumeLayout();
			this.HandlePropertyChanged("OnControlChanged");
		}
		
		
		private void OnControlSelect(object sender, EventArgs e){
			if (Selected != null){
				Selected(this,e);
			}
		}
		
		/// <summary>
		/// A Property in ReportItem has changed, inform the Designer
		/// to set the View's 'IsDirtyFlag' to true
		/// </summary>
		
		protected void HandlePropertyChanged(string info) {
			if ( !base.Suspend) {
				if (PropertyChanged != null) {
					PropertyChanged (this,new PropertyChangedEventArgs(info));
				}
			}
		}
		
		#endregion
	
		public override Point Location {
			get {
				return base.Location;
			}
			set {
				base.Location = value;
				if (this.visualControl != null) {
					this.visualControl.Location = value;
				}
				this.HandlePropertyChanged("Location");
			}
		}
	
		public override Font Font {
			get {
				return base.Font;
			}
			set {
				base.Font = value;
				if (this.visualControl != null) {
					this.visualControl.Font = value;
				}
				this.HandlePropertyChanged("Font");
			}
		}
		///<summary>
		/// Holds the text to be displayed
		/// </summary>
		
		public override string Text{
			get {
				return base.Text;
			}
			set {
				base.Text = value;
				if (this.visualControl.Text != value) {
					this.visualControl.Text = value;
					this.visualControl.Refresh();
				}
				this.HandlePropertyChanged("Text");
			}
		}
	
		#region IDesignable
	
		[System.Xml.Serialization.XmlIgnoreAttribute]
		[Browsable(false)]
		public ReportObjectControlBase VisualControl {
			get {
				return visualControl;
			}
		}
	
		public new event PropertyChangedEventHandler PropertyChanged;
		public event EventHandler <EventArgs> Selected;

		#endregion
		
		#region overrides
		
		public override string ToString(){
			return this.Name;
		}
		
		#endregion
		/*
		#region IDisposable
		public override void Dispose(){
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		
		~ReportTextItem()
		{
			Dispose(false);
		}
		
		protected override void Dispose(bool disposing){
			try {
				if (disposing) {
				// Free other state (managed objects).
				if (this.visualControl != null) {
					this.visualControl.Dispose();
				}
			}
			} finally {
				base.Dispose();
			}
		}
		#endregion
		*/
	}
}
