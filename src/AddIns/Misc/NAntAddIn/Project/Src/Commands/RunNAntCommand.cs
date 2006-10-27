// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Matthew Ward" email="mrward@users.sourceforge.net"/>
//     <version>$Revision$</version>
// </file>

using System;
using System.IO;
using ICSharpCode.Core;

namespace ICSharpCode.NAntAddIn.Commands
{
	/// <summary>
    /// Represents the command that runs NAnt on the project's build file.
    /// </summary>
	public class RunNAntCommand : AbstractRunNAntCommand
	{		
        /// <summary>
        /// Runs the <see cref="RunNAntCommand"/>.
        /// </summary>
        public override void Run()
        {   
        	try {
        		string buildFileName = GetProjectBuildFileName();
        			
        		RunPreBuildSteps();
        		
        		RunBuild(Path.GetFileName(buildFileName),
        		         Path.GetDirectoryName(buildFileName),
        		         IsActiveConfigurationDebug);
        	
        	} catch (NAntAddInException ex) {
        		MessageService.ShowMessage(ex.Message);
        	}
        }
	}
}
