// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="none" email=""/>
//     <version>$Revision$</version>
// </file>

using System;
using System.Windows.Forms;
using ICSharpCode.SharpDevelop.Gui;

namespace ICSharpCode.Svn.Gui
{
	/// <summary>
	/// The Output Window options panel.
	/// </summary>
	public class SubversionOptionsPanel : AbstractOptionPanel
	{
		public SubversionOptionsPanel()
		{
		}
		
		public override void LoadPanelContents()
		{
			SetupFromXmlStream(this.GetType().Assembly.GetManifestResourceStream("ICSharpCode.Svn.Resources.SubversionOptionsPanel.xfrm"));
			ControlDictionary["logMessageTextBox"].Text                        = AddInOptions.DefaultLogMessage;
			((CheckBox)ControlDictionary["autoAddFilesCheckBox"]).Checked      = AddInOptions.AutomaticallyAddFiles;
			((CheckBox)ControlDictionary["autoDeleteFilesCheckBox"]).Checked   = AddInOptions.AutomaticallyDeleteFiles;
			((CheckBox)ControlDictionary["autoReloadProjectCheckBox"]).Checked = AddInOptions.AutomaticallyReloadProject;
		}
		
		public override bool StorePanelContents()
		{
			AddInOptions.DefaultLogMessage          = ControlDictionary["logMessageTextBox"].Text;
			AddInOptions.AutomaticallyAddFiles      = ((CheckBox)ControlDictionary["autoAddFilesCheckBox"]).Checked;
			AddInOptions.AutomaticallyDeleteFiles   = ((CheckBox)ControlDictionary["autoDeleteFilesCheckBox"]).Checked;
			AddInOptions.AutomaticallyReloadProject = ((CheckBox)ControlDictionary["autoReloadProjectCheckBox"]).Checked;
			
			return true;
		}		
	}
}
