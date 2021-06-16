using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x020005C7 RID: 1479
	internal class Scope
	{
		// Token: 0x06003F9C RID: 16284 RVA: 0x0014FFA1 File Offset: 0x0014E1A1
		internal Scope(IParameterMetadataProvider ast, ScopeType scopeType)
		{
			this._ast = (Ast)ast;
			this._scopeType = scopeType;
			this._typeTable = new Dictionary<string, TypeLookupResult>(StringComparer.OrdinalIgnoreCase);
			this._variableTable = new Dictionary<string, Ast>(StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x06003F9D RID: 16285 RVA: 0x0014FFDC File Offset: 0x0014E1DC
		internal Scope(TypeDefinitionAst typeDefinition)
		{
			this._ast = typeDefinition;
			this._scopeType = ScopeType.Type;
			this._typeTable = new Dictionary<string, TypeLookupResult>(StringComparer.OrdinalIgnoreCase);
			this._variableTable = new Dictionary<string, Ast>(StringComparer.OrdinalIgnoreCase);
			foreach (MemberAst memberAst in typeDefinition.Members)
			{
				PropertyMemberAst propertyMemberAst = memberAst as PropertyMemberAst;
				if (propertyMemberAst != null && !this._variableTable.ContainsKey(propertyMemberAst.Name))
				{
					this._variableTable.Add(propertyMemberAst.Name, propertyMemberAst);
				}
			}
		}

		// Token: 0x06003F9E RID: 16286 RVA: 0x00150088 File Offset: 0x0014E288
		internal void AddType(Parser parser, TypeDefinitionAst typeDefinitionAst)
		{
			TypeLookupResult typeLookupResult;
			if (!this._typeTable.TryGetValue(typeDefinitionAst.Name, out typeLookupResult))
			{
				this._typeTable.Add(typeDefinitionAst.Name, new TypeLookupResult(typeDefinitionAst));
				return;
			}
			if (typeLookupResult.ExternalNamespaces != null)
			{
				typeLookupResult.ExternalNamespaces = null;
				typeLookupResult.Type = typeDefinitionAst;
				return;
			}
			parser.ReportError(typeDefinitionAst.Extent, () => ParserStrings.MemberAlreadyDefined, typeDefinitionAst.Name);
		}

		// Token: 0x06003F9F RID: 16287 RVA: 0x0015010C File Offset: 0x0014E30C
		internal void AddTypeFromUsingModule(Parser parser, TypeDefinitionAst typeDefinitionAst, PSModuleInfo moduleInfo)
		{
			TypeLookupResult typeLookupResult;
			if (this._typeTable.TryGetValue(typeDefinitionAst.Name, out typeLookupResult))
			{
				if (typeLookupResult.ExternalNamespaces != null)
				{
					typeLookupResult.ExternalNamespaces.Add(moduleInfo.Name);
				}
			}
			else
			{
				TypeLookupResult typeLookupResult2 = new TypeLookupResult(typeDefinitionAst)
				{
					ExternalNamespaces = new List<string>()
				};
				typeLookupResult2.ExternalNamespaces.Add(moduleInfo.Name);
				this._typeTable.Add(typeDefinitionAst.Name, typeLookupResult2);
			}
			string moduleQualifiedName = SymbolResolver.GetModuleQualifiedName(moduleInfo.Name, typeDefinitionAst.Name);
			if (this._typeTable.TryGetValue(moduleQualifiedName, out typeLookupResult))
			{
				parser.ReportError(typeDefinitionAst.Extent, () => ParserStrings.MemberAlreadyDefined, moduleQualifiedName);
				return;
			}
			this._typeTable.Add(moduleQualifiedName, new TypeLookupResult(typeDefinitionAst));
		}

		// Token: 0x06003FA0 RID: 16288 RVA: 0x001501E4 File Offset: 0x0014E3E4
		internal TypeLookupResult LookupType(TypeName typeName)
		{
			if (typeName.AssemblyName != null)
			{
				return null;
			}
			TypeLookupResult result;
			this._typeTable.TryGetValue(typeName.Name, out result);
			return result;
		}

		// Token: 0x06003FA1 RID: 16289 RVA: 0x00150210 File Offset: 0x0014E410
		public Ast LookupVariable(VariablePath variablePath)
		{
			Ast result;
			this._variableTable.TryGetValue(variablePath.UserPath, out result);
			return result;
		}

		// Token: 0x04001F5F RID: 8031
		internal Ast _ast;

		// Token: 0x04001F60 RID: 8032
		internal ScopeType _scopeType;

		// Token: 0x04001F61 RID: 8033
		private readonly Dictionary<string, TypeLookupResult> _typeTable;

		// Token: 0x04001F62 RID: 8034
		private readonly Dictionary<string, Ast> _variableTable;
	}
}
