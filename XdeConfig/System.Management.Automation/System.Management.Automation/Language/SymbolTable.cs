using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x020005C8 RID: 1480
	internal class SymbolTable
	{
		// Token: 0x06003FA2 RID: 16290 RVA: 0x00150232 File Offset: 0x0014E432
		internal SymbolTable(Parser parser)
		{
			this._scopes = new List<Scope>();
			this._parser = parser;
		}

		// Token: 0x06003FA3 RID: 16291 RVA: 0x00150258 File Offset: 0x0014E458
		internal void AddTypesInScope(Ast ast)
		{
			IEnumerable<Ast> enumerable = ast.FindAll((Ast x) => x is TypeDefinitionAst, false);
			foreach (Ast ast2 in enumerable)
			{
				this.AddType((TypeDefinitionAst)ast2);
			}
		}

		// Token: 0x06003FA4 RID: 16292 RVA: 0x001502CC File Offset: 0x0014E4CC
		internal void EnterScope(IParameterMetadataProvider ast, ScopeType scopeType)
		{
			Scope item = new Scope(ast, scopeType);
			this._scopes.Add(item);
			this.AddTypesInScope((Ast)ast);
		}

		// Token: 0x06003FA5 RID: 16293 RVA: 0x001502FC File Offset: 0x0014E4FC
		internal void EnterScope(TypeDefinitionAst typeDefinition)
		{
			Scope item = new Scope(typeDefinition);
			this._scopes.Add(item);
			this.AddTypesInScope(typeDefinition);
		}

		// Token: 0x06003FA6 RID: 16294 RVA: 0x00150323 File Offset: 0x0014E523
		internal void LeaveScope()
		{
			this._scopes.RemoveAt(this._scopes.Count - 1);
		}

		// Token: 0x06003FA7 RID: 16295 RVA: 0x0015033D File Offset: 0x0014E53D
		public void AddType(TypeDefinitionAst typeDefinitionAst)
		{
			this._scopes[this._scopes.Count - 1].AddType(this._parser, typeDefinitionAst);
		}

		// Token: 0x06003FA8 RID: 16296 RVA: 0x00150363 File Offset: 0x0014E563
		public void AddTypeFromUsingModule(TypeDefinitionAst typeDefinitionAst, PSModuleInfo moduleInfo)
		{
			this._scopes[this._scopes.Count - 1].AddTypeFromUsingModule(this._parser, typeDefinitionAst, moduleInfo);
		}

		// Token: 0x06003FA9 RID: 16297 RVA: 0x0015038C File Offset: 0x0014E58C
		public TypeLookupResult LookupType(TypeName typeName)
		{
			TypeLookupResult typeLookupResult = null;
			for (int i = this._scopes.Count - 1; i >= 0; i--)
			{
				typeLookupResult = this._scopes[i].LookupType(typeName);
				if (typeLookupResult != null)
				{
					break;
				}
			}
			return typeLookupResult;
		}

		// Token: 0x06003FAA RID: 16298 RVA: 0x001503CC File Offset: 0x0014E5CC
		public Ast LookupVariable(VariablePath variablePath)
		{
			Ast ast = null;
			for (int i = this._scopes.Count - 1; i >= 0; i--)
			{
				ast = this._scopes[i].LookupVariable(variablePath);
				if (ast != null)
				{
					break;
				}
			}
			return ast;
		}

		// Token: 0x06003FAB RID: 16299 RVA: 0x0015040C File Offset: 0x0014E60C
		public TypeDefinitionAst GetCurrentTypeDefinitionAst()
		{
			for (int i = this._scopes.Count - 1; i >= 0; i--)
			{
				TypeDefinitionAst typeDefinitionAst = this._scopes[i]._ast as TypeDefinitionAst;
				if (typeDefinitionAst != null)
				{
					return typeDefinitionAst;
				}
			}
			return null;
		}

		// Token: 0x06003FAC RID: 16300 RVA: 0x0015044E File Offset: 0x0014E64E
		public bool IsInMethodScope()
		{
			return this._scopes[this._scopes.Count - 1]._scopeType == ScopeType.Method;
		}

		// Token: 0x04001F63 RID: 8035
		internal readonly List<Scope> _scopes;

		// Token: 0x04001F64 RID: 8036
		internal readonly Parser _parser;
	}
}
