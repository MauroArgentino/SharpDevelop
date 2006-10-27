// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Matthew Ward" email="mrward@users.sourceforge.net"/>
//     <version>$Revision$</version>
// </file>

using System;
using System.Diagnostics;
using System.Windows.Forms;

using ICSharpCode.Core;
using ICSharpCode.SharpDevelop.Gui.XmlForms;

namespace ICSharpCode.CodeCoverage
{
	public class CodeCoverageRunnerNotFoundForm : XmlForm
	{		
		public CodeCoverageRunnerNotFoundForm()
		{
			SetupFromXmlStream(this.GetType().Assembly.GetManifestResourceStream("ICSharpCode.CodeCoverage.Resources.CodeCoverageRunnerNotFoundForm.xfrm"));
		
			((Label)ControlDictionary["messageLabel"]).Text = StringParser.Parse("${res:ICSharpCode.CodeCoverage.NCoverNotFound}");
			((Button)ControlDictionary["okButton"]).Text = StringParser.Parse("${res:Global.OKButtonText}");
			((PictureBox)ControlDictionary["iconPictureBox"]).Image = ResourceService.GetBitmap("Icons.32x32.Information");
			((LinkLabel)ControlDictionary["linkLabel"]).Click += LinkLabelClicked;
		}
		
		void LinkLabelClicked(object sender, EventArgs e)
		{
			Process.Start("http://ncover.org");
		}
	}
}
