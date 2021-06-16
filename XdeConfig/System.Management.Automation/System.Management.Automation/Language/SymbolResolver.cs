using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Runtime.CompilerServices;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation.Language
{
	// Token: 0x020005C9 RID: 1481
	internal class SymbolResolver : AstVisitor2, IAstPostVisitHandler
	{
		// Token: 0x17000DB8 RID: 3512
		// (get) Token: 0x06003FAE RID: 16302 RVA: 0x00150484 File Offset: 0x0014E684
		private static PowerShell UsingStatementResolvePowerShell
		{
			get
			{
				if (SymbolResolver._usingStatementResolvePowerShell == null)
				{
					if (Runspace.DefaultRunspace != null)
					{
						SymbolResolver._usingStatementResolvePowerShell = PowerShell.Create(RunspaceMode.CurrentRunspace);
					}
					else
					{
						InitialSessionState initialSessionState = InitialSessionState.Create();
						initialSessionState.Commands.Add(new SessionStateCmdletEntry("Get-Module", typeof(GetModuleCommand), null));
						SessionStateProviderEntry sessionStateProviderEntry = new SessionStateProviderEntry("FileSystem", typeof(FileSystemProvider), null);
						PSSnapInInfo pssnapIn = PSSnapInReader.ReadEnginePSSnapIns().FirstOrDefault((PSSnapInInfo snapIn) => snapIn.Name.Equals("Microsoft.PowerShell.Core", StringComparison.OrdinalIgnoreCase));
						sessionStateProviderEntry.SetPSSnapIn(pssnapIn);
						initialSessionState.Providers.Add(sessionStateProviderEntry);
						SymbolResolver._usingStatementResolvePowerShell = PowerShell.Create(initialSessionState);
					}
				}
				else if (Runspace.DefaultRunspace != null && SymbolResolver._usingStatementResolvePowerShell.Runspace != Runspace.DefaultRunspace)
				{
					SymbolResolver._usingStatementResolvePowerShell = PowerShell.Create(RunspaceMode.CurrentRunspace);
				}
				return SymbolResolver._usingStatementResolvePowerShell;
			}
		}

		// Token: 0x06003FAF RID: 16303 RVA: 0x00150560 File Offset: 0x0014E760
		private SymbolResolver(Parser parser, TypeResolutionState typeResolutionState)
		{
			this._symbolTable = new SymbolTable(parser);
			this._parser = parser;
			this._typeResolutionState = typeResolutionState;
			this._symbolResolvePostActionVisitor = new SymbolResolvePostActionVisitor
			{
				_symbolResolver = this
			};
		}

		// Token: 0x06003FB0 RID: 16304 RVA: 0x001505A4 File Offset: 0x0014E7A4
		internal static void ResolveSymbols(Parser parser, ScriptBlockAst scriptBlockAst)
		{
			TypeResolutionState typeResolutionState = (scriptBlockAst.UsingStatements.Count > 0) ? new TypeResolutionState(TypeOps.GetNamespacesForTypeResolutionState(scriptBlockAst.UsingStatements), TypeResolutionState.emptyAssemblies) : TypeResolutionState.GetDefaultUsingState(null);
			SymbolResolver symbolResolver = new SymbolResolver(parser, typeResolutionState);
			symbolResolver._symbolTable.EnterScope(scriptBlockAst, ScopeType.ScriptBlock);
			scriptBlockAst.Visit(symbolResolver);
			symbolResolver._symbolTable.LeaveScope();
		}

		// Token: 0x06003FB1 RID: 16305 RVA: 0x00150604 File Offset: 0x0014E804
		public override AstVisitAction VisitTypeDefinition(TypeDefinitionAst typeDefinitionAst)
		{
			this._symbolTable.EnterScope(typeDefinitionAst);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003FB2 RID: 16306 RVA: 0x00150613 File Offset: 0x0014E813
		public override AstVisitAction VisitScriptBlockExpression(ScriptBlockExpressionAst scriptBlockExpressionAst)
		{
			this._symbolTable.EnterScope(scriptBlockExpressionAst.ScriptBlock, ScopeType.ScriptBlock);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003FB3 RID: 16307 RVA: 0x00150628 File Offset: 0x0014E828
		public override AstVisitAction VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst)
		{
			if (!(functionDefinitionAst.Parent is FunctionMemberAst))
			{
				this._symbolTable.EnterScope(functionDefinitionAst.Body, ScopeType.Function);
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06003FB4 RID: 16308 RVA: 0x0015064A File Offset: 0x0014E84A
		public override AstVisitAction VisitPropertyMember(PropertyMemberAst propertyMemberAst)
		{
			return AstVisitAction.Continue;
		}

		// Token: 0x06003FB5 RID: 16309 RVA: 0x0015064D File Offset: 0x0014E84D
		public override AstVisitAction VisitFunctionMember(FunctionMemberAst functionMemberAst)
		{
			this._symbolTable.EnterScope(functionMemberAst.Body, ScopeType.Method);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003FB6 RID: 16310 RVA: 0x00150664 File Offset: 0x0014E864
		public override AstVisitAction VisitAssignmentStatement(AssignmentStatementAst assignmentStatementAst)
		{
			if (this._symbolTable.IsInMethodScope())
			{
				ExpressionAst[] array = assignmentStatementAst.GetAssignmentTargets().ToArray<ExpressionAst>();
				foreach (ExpressionAst expressionAst in array)
				{
					ExpressionAst expressionAst2 = expressionAst;
					VariableExpressionAst variableExpressionAst = expressionAst2 as VariableExpressionAst;
					while (variableExpressionAst == null && expressionAst2 != null)
					{
						ConvertExpressionAst convertExpressionAst = expressionAst2 as ConvertExpressionAst;
						if (convertExpressionAst == null)
						{
							break;
						}
						expressionAst2 = convertExpressionAst.Child;
						variableExpressionAst = (convertExpressionAst.Child as VariableExpressionAst);
					}
					if (variableExpressionAst != null && variableExpressionAst.VariablePath.IsVariable)
					{
						Ast ast = this._symbolTable.LookupVariable(variableExpressionAst.VariablePath);
						PropertyMemberAst propertyMemberAst = ast as PropertyMemberAst;
						if (propertyMemberAst != null)
						{
							if (propertyMemberAst.IsStatic)
							{
								TypeDefinitionAst currentTypeDefinitionAst = this._symbolTable.GetCurrentTypeDefinitionAst();
								this._parser.ReportError(variableExpressionAst.Extent, () => ParserStrings.MissingTypeInStaticPropertyAssignment, string.Format(CultureInfo.InvariantCulture, "[{0}]::", new object[]
								{
									currentTypeDefinitionAst.Name
								}), propertyMemberAst.Name);
							}
							else
							{
								this._parser.ReportError(variableExpressionAst.Extent, () => ParserStrings.MissingThis, "$this.", propertyMemberAst.Name);
							}
						}
					}
				}
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06003FB7 RID: 16311 RVA: 0x001507CC File Offset: 0x0014E9CC
		public override AstVisitAction VisitTypeExpression(TypeExpressionAst typeExpressionAst)
		{
			this.DispatchTypeName(typeExpressionAst.TypeName, 0, false);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003FB8 RID: 16312 RVA: 0x001507DE File Offset: 0x0014E9DE
		public override AstVisitAction VisitTypeConstraint(TypeConstraintAst typeConstraintAst)
		{
			this.DispatchTypeName(typeConstraintAst.TypeName, 0, false);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003FB9 RID: 16313 RVA: 0x001507F0 File Offset: 0x0014E9F0
		public override AstVisitAction VisitUsingStatement(UsingStatementAst usingStatementAst)
		{
			if (usingStatementAst.UsingStatementKind == UsingStatementKind.Module)
			{
				Collection<PSModuleInfo> collection = null;
				string text = usingStatementAst.Name.Value;
				try
				{
					if (!text.Contains("*"))
					{
						bool flag = text.Contains("\\");
						if (flag && !LocationGlobber.IsAbsolutePath(text))
						{
							string directoryName = Path.GetDirectoryName(this._parser._fileName);
							if (directoryName != null)
							{
								text = Path.Combine(directoryName, text);
							}
						}
						CmdletInfo commandInfo = new CmdletInfo("Get-Module", typeof(GetModuleCommand));
						SymbolResolver.UsingStatementResolvePowerShell.Commands.Clear();
						collection = SymbolResolver.UsingStatementResolvePowerShell.AddCommand(commandInfo).AddParameter("FullyQualifiedName", text).AddParameter("ListAvailable", true).Invoke<PSModuleInfo>();
					}
				}
				catch
				{
				}
				if (collection != null && collection.Count == 1)
				{
					ReadOnlyDictionary<string, TypeDefinitionAst> readOnlyDictionary = usingStatementAst.DefineImportedModule(collection[0]);
					using (IEnumerator<KeyValuePair<string, TypeDefinitionAst>> enumerator = readOnlyDictionary.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<string, TypeDefinitionAst> keyValuePair = enumerator.Current;
							this._symbolTable.AddTypeFromUsingModule(keyValuePair.Value, collection[0]);
						}
						return AstVisitAction.Continue;
					}
				}
				if (collection == null || collection.Count == 0)
				{
					this._parser.ReportError(usingStatementAst.Extent, () => ParserStrings.ModuleNotFoundDuringParse, usingStatementAst.Name.Value);
				}
				else
				{
					this._parser.ReportError(usingStatementAst.Extent, () => ParserStrings.MultipleModuleEntriesFoundDuringParse, usingStatementAst.Name.Value);
				}
			}
			return AstVisitAction.Continue;
		}

		// Token: 0x06003FBA RID: 16314 RVA: 0x001509B8 File Offset: 0x0014EBB8
		public override AstVisitAction VisitAttribute(AttributeAst attributeAst)
		{
			this.DispatchTypeName(attributeAst.TypeName, 0, true);
			return AstVisitAction.Continue;
		}

		// Token: 0x06003FBB RID: 16315 RVA: 0x001509CC File Offset: 0x0014EBCC
		private bool DispatchTypeName(ITypeName type, int genericArgumentCount, bool isAttribute)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			TypeName typeName = type as TypeName;
			if (typeName != null)
			{
				return this.VisitTypeName(typeName, genericArgumentCount, isAttribute);
			}
			ArrayTypeName arrayTypeName = type as ArrayTypeName;
			if (arrayTypeName != null)
			{
				return this.VisitArrayTypeName(arrayTypeName);
			}
			GenericTypeName genericTypeName = type as GenericTypeName;
			return genericTypeName != null && this.VisitGenericTypeName(genericTypeName);
		}

		// Token: 0x06003FBC RID: 16316 RVA: 0x00150A18 File Offset: 0x0014EC18
		private bool VisitArrayTypeName(ArrayTypeName arrayTypeName)
		{
			bool flag = this.DispatchTypeName(arrayTypeName.ElementType, 0, false);
			if (flag)
			{
				Type reflectionType = arrayTypeName.GetReflectionType();
				TypeCache.Add(arrayTypeName, this._typeResolutionState, reflectionType);
			}
			return flag;
		}

		// Token: 0x06003FBD RID: 16317 RVA: 0x00150A4C File Offset: 0x0014EC4C
		private bool VisitTypeName(TypeName typeName, int genericArgumentCount, bool isAttribute)
		{
			TypeLookupResult typeLookupResult = this._symbolTable.LookupType(typeName);
			if (typeLookupResult != null && typeLookupResult.IsAmbiguous())
			{
				this._parser.ReportError(typeName.Extent, () => ParserStrings.AmbiguousTypeReference, new object[]
				{
					typeName.Name,
					SymbolResolver.GetModuleQualifiedName(typeLookupResult.ExternalNamespaces[0], typeName.Name),
					SymbolResolver.GetModuleQualifiedName(typeLookupResult.ExternalNamespaces[1], typeName.Name)
				});
			}
			else if (typeLookupResult != null && genericArgumentCount == 0)
			{
				typeName.SetTypeDefinition(typeLookupResult.Type);
			}
			else
			{
				TypeResolutionState typeResolutionState = (genericArgumentCount > 0 || isAttribute) ? new TypeResolutionState(this._typeResolutionState, genericArgumentCount, isAttribute) : this._typeResolutionState;
				Exception ex;
				Type type = TypeResolver.ResolveTypeNameWithContext(typeName, out ex, null, typeResolutionState);
				if (!(type == null))
				{
					((ISupportsTypeCaching)typeName).CachedType = type;
					return true;
				}
				if (this._symbolTable.GetCurrentTypeDefinitionAst() != null)
				{
					this._parser.ReportError(typeName.Extent, isAttribute ? (() => ParserStrings.CustomAttributeTypeNotFound) : (() => ParserStrings.TypeNotFound), typeName.Name);
				}
			}
			return false;
		}

		// Token: 0x06003FBE RID: 16318 RVA: 0x00150BB4 File Offset: 0x0014EDB4
		private bool VisitGenericTypeName(GenericTypeName genericTypeName)
		{
			Type type = TypeCache.Lookup(genericTypeName, this._typeResolutionState);
			if (type != null)
			{
				((ISupportsTypeCaching)genericTypeName).CachedType = type;
				return true;
			}
			bool flag = true;
			flag &= this.DispatchTypeName(genericTypeName.TypeName, genericTypeName.GenericArguments.Count, false);
			foreach (ITypeName type2 in genericTypeName.GenericArguments)
			{
				flag &= this.DispatchTypeName(type2, 0, false);
			}
			if (flag)
			{
				Type reflectionType = genericTypeName.GetReflectionType();
				TypeCache.Add(genericTypeName, this._typeResolutionState, reflectionType);
			}
			return flag;
		}

		// Token: 0x06003FBF RID: 16319 RVA: 0x00150C60 File Offset: 0x0014EE60
		public void PostVisit(Ast ast)
		{
			ast.Accept(this._symbolResolvePostActionVisitor);
		}

		// Token: 0x06003FC0 RID: 16320 RVA: 0x00150C6F File Offset: 0x0014EE6F
		internal static string GetModuleQualifiedName(string namespaceName, string typeName)
		{
			return namespaceName + '.' + typeName;
		}

		// Token: 0x04001F66 RID: 8038
		private readonly SymbolResolvePostActionVisitor _symbolResolvePostActionVisitor;

		// Token: 0x04001F67 RID: 8039
		internal readonly SymbolTable _symbolTable;

		// Token: 0x04001F68 RID: 8040
		internal readonly Parser _parser;

		// Token: 0x04001F69 RID: 8041
		internal readonly TypeResolutionState _typeResolutionState;

		// Token: 0x04001F6A RID: 8042
		[ThreadStatic]
		private static PowerShell _usingStatementResolvePowerShell;
	}
}
