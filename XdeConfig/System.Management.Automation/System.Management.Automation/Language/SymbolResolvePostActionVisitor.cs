using System;

namespace System.Management.Automation.Language
{
	// Token: 0x020005CA RID: 1482
	internal class SymbolResolvePostActionVisitor : DefaultCustomAstVisitor2
	{
		// Token: 0x06003FC2 RID: 16322 RVA: 0x00150C7F File Offset: 0x0014EE7F
		public override object VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst)
		{
			if (!(functionDefinitionAst.Parent is FunctionMemberAst))
			{
				this._symbolResolver._symbolTable.LeaveScope();
			}
			return null;
		}

		// Token: 0x06003FC3 RID: 16323 RVA: 0x00150C9F File Offset: 0x0014EE9F
		public override object VisitScriptBlockExpression(ScriptBlockExpressionAst scriptBlockExpressionAst)
		{
			this._symbolResolver._symbolTable.LeaveScope();
			return null;
		}

		// Token: 0x06003FC4 RID: 16324 RVA: 0x00150CB2 File Offset: 0x0014EEB2
		public override object VisitTypeDefinition(TypeDefinitionAst typeDefinitionAst)
		{
			this._symbolResolver._symbolTable.LeaveScope();
			return null;
		}

		// Token: 0x06003FC5 RID: 16325 RVA: 0x00150CC5 File Offset: 0x0014EEC5
		public override object VisitFunctionMember(FunctionMemberAst functionMemberAst)
		{
			this._symbolResolver._symbolTable.LeaveScope();
			return null;
		}

		// Token: 0x04001F6C RID: 8044
		internal SymbolResolver _symbolResolver;
	}
}
