using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Reflection;

namespace System.Management.Automation.Language
{
	// Token: 0x020005E4 RID: 1508
	internal static class TypeResolver
	{
		// Token: 0x060040AD RID: 16557 RVA: 0x0015706C File Offset: 0x0015526C
		private static Type LookForTypeInSingleAssembly(Assembly assembly, string typename)
		{
			Type type = assembly.GetType(typename, false, true);
			if (type != null && TypeResolver.IsPublic(type))
			{
				return type;
			}
			return null;
		}

		// Token: 0x060040AE RID: 16558 RVA: 0x00157098 File Offset: 0x00155298
		private static Type LookForTypeInAssemblies(TypeName typeName, IEnumerable<Assembly> assemblies, TypeResolutionState typeResolutionState)
		{
			string alternateTypeName = typeResolutionState.GetAlternateTypeName(typeName.Name);
			foreach (Assembly assembly in assemblies)
			{
				try
				{
					Type type = null;
					if (alternateTypeName != null)
					{
						type = TypeResolver.LookForTypeInSingleAssembly(assembly, alternateTypeName);
					}
					if (type == null)
					{
						type = TypeResolver.LookForTypeInSingleAssembly(assembly, typeName.Name);
					}
					if (type != null)
					{
						return type;
					}
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
				}
			}
			return null;
		}

		// Token: 0x060040AF RID: 16559 RVA: 0x00157138 File Offset: 0x00155338
		internal static bool IsPublic(Type type)
		{
			TypeInfo typeInfo = type.GetTypeInfo();
			return TypeResolver.IsPublic(typeInfo);
		}

		// Token: 0x060040B0 RID: 16560 RVA: 0x00157154 File Offset: 0x00155354
		internal static bool IsPublic(TypeInfo typeInfo)
		{
			if (typeInfo.IsPublic)
			{
				return true;
			}
			if (!typeInfo.IsNestedPublic)
			{
				return false;
			}
			Type declaringType;
			while ((declaringType = typeInfo.DeclaringType) != null)
			{
				typeInfo = declaringType.GetTypeInfo();
				if (!typeInfo.IsPublic && !typeInfo.IsNestedPublic)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060040B1 RID: 16561 RVA: 0x001571A0 File Offset: 0x001553A0
		private static Type ResolveTypeNameWorker(TypeName typeName, SessionStateScope currentScope, Assembly[] loadedAssemblies, TypeResolutionState typeResolutionState)
		{
			Type type;
			while (currentScope != null)
			{
				type = currentScope.LookupType(typeName.Name);
				if (type != null)
				{
					return type;
				}
				currentScope = currentScope.Parent;
			}
			if (TypeAccelerators.builtinTypeAccelerators.TryGetValue(typeName.Name, out type))
			{
				return type;
			}
			type = TypeResolver.LookForTypeInAssemblies(typeName, loadedAssemblies, typeResolutionState);
			if (type == null)
			{
				lock (TypeAccelerators.userTypeAccelerators)
				{
					TypeAccelerators.userTypeAccelerators.TryGetValue(typeName.Name, out type);
				}
			}
			return type;
		}

		// Token: 0x060040B2 RID: 16562 RVA: 0x0015723C File Offset: 0x0015543C
		internal static Type ResolveAssemblyQualifiedTypeName(TypeName typeName, out Exception exception)
		{
			exception = null;
			try
			{
				Type type = Type.GetType(typeName.FullName, false, true) ?? Type.GetType("System." + typeName.FullName, false, true);
				if (type != null && TypeResolver.IsPublic(type))
				{
					return type;
				}
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				exception = ex;
			}
			return null;
		}

		// Token: 0x060040B3 RID: 16563 RVA: 0x001572AC File Offset: 0x001554AC
		internal static Type ResolveTypeNameWithContext(TypeName typeName, out Exception exception, Assembly[] assemblies, TypeResolutionState typeResolutionState)
		{
			ExecutionContext executionContext = null;
			exception = null;
			if (typeResolutionState == null)
			{
				executionContext = LocalPipeline.GetExecutionContextFromTLS();
				typeResolutionState = TypeResolutionState.GetDefaultUsingState(executionContext);
			}
			Type type = typeResolutionState.typesDefined.Contains(typeName.Name) ? null : TypeCache.Lookup(typeName, typeResolutionState);
			if (type != null)
			{
				return type;
			}
			if (typeName.AssemblyName != null)
			{
				type = TypeResolver.ResolveAssemblyQualifiedTypeName(typeName, out exception);
				TypeCache.Add(typeName, typeResolutionState, type);
				return type;
			}
			IEnumerable<Assembly> second = (assemblies != null) ? ((IEnumerable<Assembly>)assemblies) : ClrFacade.GetAssemblies(typeResolutionState, typeName);
			Assembly[] loadedAssemblies = typeResolutionState.assemblies.Concat(second).ToArray<Assembly>();
			if (executionContext == null)
			{
				executionContext = LocalPipeline.GetExecutionContextFromTLS();
			}
			SessionStateScope currentScope = null;
			if (executionContext != null)
			{
				currentScope = executionContext.EngineSessionState.CurrentScope;
			}
			if (typeName._typeDefinitionAst != null)
			{
				return typeName._typeDefinitionAst.Type;
			}
			type = TypeResolver.ResolveTypeNameWorker(typeName, currentScope, loadedAssemblies, typeResolutionState);
			if (type != null)
			{
				TypeCache.Add(typeName, typeResolutionState, type);
				return type;
			}
			foreach (string str in typeResolutionState.namespaces)
			{
				string text = str + "." + typeName.Name;
				text = (typeResolutionState.GetAlternateTypeName(text) ?? text);
				TypeName typeName2 = new TypeName(typeName.Extent, text);
				Type type2 = TypeResolver.ResolveTypeNameWorker(typeName2, currentScope, loadedAssemblies, typeResolutionState);
				if (type2 != null)
				{
					if (!(type == null))
					{
						exception = InterpreterError.NewInterpreterException(typeName.Name, typeof(RuntimeException), typeName.Extent, "AmbiguousTypeReference", ParserStrings.AmbiguousTypeReference, new object[]
						{
							typeName.Name,
							type.FullName,
							type2.FullName
						});
						return null;
					}
					type = type2;
				}
			}
			if (type != null)
			{
				TypeCache.Add(typeName, typeResolutionState, type);
			}
			return type;
		}

		// Token: 0x060040B4 RID: 16564 RVA: 0x00157470 File Offset: 0x00155670
		internal static Type ResolveTypeName(TypeName typeName, out Exception exception)
		{
			return TypeResolver.ResolveTypeNameWithContext(typeName, out exception, null, null);
		}

		// Token: 0x060040B5 RID: 16565 RVA: 0x0015747C File Offset: 0x0015567C
		internal static bool TryResolveType(string typeName, out Type type)
		{
			Exception ex;
			type = TypeResolver.ResolveType(typeName, out ex);
			return type != null;
		}

		// Token: 0x060040B6 RID: 16566 RVA: 0x0015749C File Offset: 0x0015569C
		internal static Type ResolveITypeName(ITypeName iTypeName, out Exception exception)
		{
			exception = null;
			TypeName typeName = iTypeName as TypeName;
			if (typeName == null)
			{
				try
				{
					return iTypeName.GetReflectionType();
				}
				catch (Exception ex)
				{
					CommandProcessorBase.CheckForSevereException(ex);
					exception = ex;
					return null;
				}
			}
			return TypeResolver.ResolveTypeName(typeName, out exception);
		}

		// Token: 0x060040B7 RID: 16567 RVA: 0x001574E8 File Offset: 0x001556E8
		internal static Type ResolveType(string strTypeName, out Exception exception)
		{
			exception = null;
			if (string.IsNullOrWhiteSpace(strTypeName))
			{
				return null;
			}
			ITypeName typeName = Parser.ScanType(strTypeName, false);
			if (typeName == null)
			{
				return null;
			}
			return TypeResolver.ResolveITypeName(typeName, out exception);
		}
	}
}
