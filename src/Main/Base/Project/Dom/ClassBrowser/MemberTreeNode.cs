﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.TreeView;

namespace ICSharpCode.SharpDevelop.Dom.ClassBrowser
{
	public class MemberTreeNode : SharpTreeNode
	{
		IMemberModel model;
		
		public MemberTreeNode(IMemberModel model)
		{
			if (model == null)
				throw new ArgumentNullException("model");
			this.model = model;
			// disable lazy loading to avoid showing a useless + sign in the tree.
			// remove this line if you add child nodes
			LazyLoading = false;
		}
		
		protected override object GetModel()
		{
			return model;
		}
		
		public override object Icon {
			// TODO why do I have to resolve this?
			get {
				return ClassBrowserIconService.GetIcon(model.Resolve()).ImageSource;
			}
		}
		
		object cachedText;
		
		public override object Text {
			get {
				if (cachedText == null)
					cachedText = GetText();
				return cachedText;
			}
		}
		
		object GetText()
		{
			var member = model.Resolve();
			if (member == null)
				return model.Name;
			IAmbience ambience = AmbienceService.GetCurrentAmbience();
			ambience.ConversionFlags = ConversionFlags.ShowTypeParameterList | ConversionFlags.ShowParameterList | ConversionFlags.ShowParameterNames;
			return ambience.ConvertEntity(member);
		}
		
		public override void ActivateItem(System.Windows.RoutedEventArgs e)
		{
			var target = model.Resolve();
			if (target != null)
				NavigationService.NavigateTo(target);
		}
	}
}

