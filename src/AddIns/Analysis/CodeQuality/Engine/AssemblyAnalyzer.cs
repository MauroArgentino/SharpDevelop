﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ICSharpCode.CodeQuality.Engine.Dom;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem.Implementation;
using Mono.Cecil;

namespace ICSharpCode.CodeQuality.Engine
{
	/// <summary>
	/// Description of AssemblyAnalyzer.
	/// </summary>
	public class AssemblyAnalyzer
	{
		CecilLoader loader = new CecilLoader(true) { IncludeInternalMembers = true };
		ICompilation compilation;
		internal Dictionary<IAssembly, AssemblyNode> assemblyMappings;
		internal Dictionary<string, NamespaceNode> namespaceMappings;
		internal Dictionary<ITypeDefinition, TypeNode> typeMappings;
		internal Dictionary<IMethod, MethodNode> methodMappings;
		internal Dictionary<IField, FieldNode> fieldMappings;
		internal Dictionary<MemberReference, IEntity> cecilMappings;
		List<string> fileNames;
		
		public AssemblyAnalyzer()
		{
			fileNames = new List<string>();
		}
		
		public void AddAssemblyFiles(params string[] files)
		{
			fileNames.AddRange(files);
		}
		
		public ReadOnlyCollection<AssemblyNode> Analyze()
		{
			var loadedAssemblies = LoadAssemblies().ToArray();
			compilation = new SimpleCompilation(loadedAssemblies.First(), loadedAssemblies.Skip(1));
			
			assemblyMappings = new Dictionary<IAssembly, AssemblyNode>();
			namespaceMappings = new Dictionary<string, NamespaceNode>();
			typeMappings = new Dictionary<ITypeDefinition, TypeNode>();
			fieldMappings = new Dictionary<IField, FieldNode>();
			methodMappings = new Dictionary<IMethod, MethodNode>();
			cecilMappings = new Dictionary<MemberReference, IEntity>();
			
			// first we have to read all types so every method, field or property has a container
			foreach (var type in compilation.GetAllTypeDefinitions()) {
				var tn = ReadType(type);
				
				foreach (var field in type.Fields) {
					var node = new FieldNode(field);
					fieldMappings.Add(field, node);
					var cecilObj = loader.GetCecilObject((IUnresolvedField)field.UnresolvedMember);
					if (cecilObj != null)
						cecilMappings[cecilObj] = field;
					tn.AddChild(node);
				}
				
				foreach (var method in type.Methods) {
					var node = new MethodNode(method);
					methodMappings.Add(method, node);
					var cecilObj = loader.GetCecilObject((IUnresolvedMethod)method.UnresolvedMember);
					if (cecilObj != null)
						cecilMappings[cecilObj] = method;
					tn.AddChild(node);
				}
			}
			
			ILAnalyzer analyzer = new ILAnalyzer(loadedAssemblies.Select(asm => loader.GetCecilObject(asm)).ToArray(), this);
			int count = methodMappings.Count + fieldMappings.Count;
			int i  = 0;
			
			foreach (var element in methodMappings) {
				Console.WriteLine("{0} of {1}", ++i, count);
				var cecilObj = loader.GetCecilObject((IUnresolvedMethod)element.Key.UnresolvedMember);
				if (cecilObj != null)
					analyzer.Analyze(cecilObj.Body, element.Value);
				var node = element.Value;
				var method = element.Key;
				AddRelationshipsForType(node, method.ReturnType);
				AddRelationshipsForAttributes(method.Attributes, node);
				AddRelationshipsForAttributes(method.ReturnTypeAttributes, node);
				AddRelationshipsForTypeParameters(method.TypeParameters, node);
				foreach (var param in method.Parameters) {
					AddRelationshipsForType(node, param.Type);
					AddRelationshipsForAttributes(param.Attributes, node);
				}
			}
			
			foreach (var element in fieldMappings) {
				Console.WriteLine("{0} of {1}", ++i, count);
				var node = element.Value;
				var field = element.Key;
				AddRelationshipsForType(node, field.Type);
				AddRelationshipsForAttributes(field.Attributes, node);
			}
			
			return new ReadOnlyCollection<AssemblyNode>(assemblyMappings.Values.ToList());
		}
		
		void AddRelationshipsForTypeParameters(IList<ITypeParameter> typeParameters, NodeBase node)
		{
			foreach (var param in typeParameters) {
				AddRelationshipsForAttributes(param.Attributes, node);
				AddRelationshipsForType(node, param.EffectiveBaseClass);
			}
		}
		
		void AddRelationshipsForTypes(IEnumerable<IType> directBaseTypes, NodeBase node)
		{
			foreach (var baseType in directBaseTypes) {
				AddRelationshipsForType(node, baseType);
			}
		}
		
		void AddRelationshipsForAttributes(IList<IAttribute> attributes, NodeBase node)
		{
			foreach (var attr in attributes) {
				node.AddRelationship(methodMappings[attr.Constructor]);
			}
		}
		
		void AddRelationshipsForType(NodeBase node, IType type)
		{
			type.AcceptVisitor(new AnalysisTypeVisitor(this, node));
		}
		
		class AnalysisTypeVisitor : TypeVisitor
		{
			NodeBase node;
			AssemblyAnalyzer context;
			
			public AnalysisTypeVisitor(AssemblyAnalyzer context, NodeBase node)
			{
				this.context = context;
				this.node = node;
			}
			
			public override IType VisitTypeDefinition(ITypeDefinition type)
			{
				TypeNode  typeNode;
				if (context.typeMappings.TryGetValue(type, out typeNode))
					node.AddRelationship(typeNode);
				return base.VisitTypeDefinition(type);
			}
		}
		
		IEnumerable<IUnresolvedAssembly> LoadAssemblies()
		{
			var resolver = new AssemblyResolver();
			List<AssemblyDefinition> assemblies = new List<AssemblyDefinition>();
			foreach (var file in fileNames.Distinct(StringComparer.OrdinalIgnoreCase))
				assemblies.Add(resolver.LoadAssemblyFile(file));
			foreach (var asm in assemblies.ToArray())
				assemblies.AddRange(asm.Modules.SelectMany(m => m.AssemblyReferences).Select(r => resolver.Resolve(r)));
			return assemblies.Distinct().Select(asm => loader.LoadAssembly(asm));
		}
		
		NamespaceNode GetOrCreateNamespace(AssemblyNode assembly, string namespaceName)
		{
			NamespaceNode result;
			var asmDef = loader.GetCecilObject(assembly.AssemblyInfo.UnresolvedAssembly);
			if (!namespaceMappings.TryGetValue(namespaceName + "," + asmDef.FullName, out result)) {
				result = new NamespaceNode(namespaceName);
				assembly.AddChild(result);
				namespaceMappings.Add(namespaceName + "," + asmDef.FullName, result);
			}
			return result;
		}
		
		AssemblyNode GetOrCreateAssembly(IAssembly asm)
		{
			AssemblyNode result;
			if (!assemblyMappings.TryGetValue(asm, out result)) {
				result = new AssemblyNode(asm);
				assemblyMappings.Add(asm, result);
			}
			return result;
		}
		
		TypeNode ReadType(ITypeDefinition type)
		{
			var asm = GetOrCreateAssembly(type.ParentAssembly);
			var ns = GetOrCreateNamespace(asm, type.Namespace);
			TypeNode parent;
			var node = new TypeNode(type);
			if (type.DeclaringTypeDefinition != null) {
				if (typeMappings.TryGetValue(type.DeclaringTypeDefinition, out parent))
					parent.AddChild(node);
				else
					throw new Exception("TypeNode not found: " + type.DeclaringTypeDefinition.FullName);
			} else
				ns.AddChild(node);
			cecilMappings[loader.GetCecilObject(type.Parts.First())] = type;
			typeMappings.Add(type, node);
			return node;
		}
		
		class AssemblyResolver : DefaultAssemblyResolver
		{
			public AssemblyDefinition LoadAssemblyFile(string fileName)
			{
				var assembly = AssemblyDefinition.ReadAssembly(fileName, new ReaderParameters { AssemblyResolver = this });
				RegisterAssembly(assembly);
				return assembly;
			}
		}
	}
}