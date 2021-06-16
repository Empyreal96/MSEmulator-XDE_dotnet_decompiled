using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Management.Automation.Language
{
	// Token: 0x0200054C RID: 1356
	public class UsingStatementAst : StatementAst
	{
		// Token: 0x06003866 RID: 14438 RVA: 0x0012D3B4 File Offset: 0x0012B5B4
		public UsingStatementAst(IScriptExtent extent, UsingStatementKind kind, StringConstantExpressionAst name) : base(extent)
		{
			if (name == null)
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			if (kind == UsingStatementKind.Command || kind == UsingStatementKind.Type)
			{
				throw PSTraceSource.NewArgumentException("kind");
			}
			this.UsingStatementKind = kind;
			this.Name = name;
			base.SetParent(this.Name);
		}

		// Token: 0x06003867 RID: 14439 RVA: 0x0012D404 File Offset: 0x0012B604
		public UsingStatementAst(IScriptExtent extent, UsingStatementKind kind, StringConstantExpressionAst aliasName, StringConstantExpressionAst resolvedAliasAst) : base(extent)
		{
			if (aliasName == null)
			{
				throw PSTraceSource.NewArgumentNullException("aliasName");
			}
			if (resolvedAliasAst == null)
			{
				throw PSTraceSource.NewArgumentNullException("resolvedAliasAst");
			}
			if (kind == UsingStatementKind.Assembly)
			{
				throw PSTraceSource.NewArgumentException("kind");
			}
			this.UsingStatementKind = kind;
			this.Name = aliasName;
			this.Alias = resolvedAliasAst;
			base.SetParent(this.Name);
			base.SetParent(this.Alias);
		}

		// Token: 0x17000C76 RID: 3190
		// (get) Token: 0x06003868 RID: 14440 RVA: 0x0012D471 File Offset: 0x0012B671
		// (set) Token: 0x06003869 RID: 14441 RVA: 0x0012D479 File Offset: 0x0012B679
		public UsingStatementKind UsingStatementKind { get; private set; }

		// Token: 0x17000C77 RID: 3191
		// (get) Token: 0x0600386A RID: 14442 RVA: 0x0012D482 File Offset: 0x0012B682
		// (set) Token: 0x0600386B RID: 14443 RVA: 0x0012D48A File Offset: 0x0012B68A
		public StringConstantExpressionAst Name { get; private set; }

		// Token: 0x17000C78 RID: 3192
		// (get) Token: 0x0600386C RID: 14444 RVA: 0x0012D493 File Offset: 0x0012B693
		// (set) Token: 0x0600386D RID: 14445 RVA: 0x0012D49B File Offset: 0x0012B69B
		public StringConstantExpressionAst Alias { get; private set; }

		// Token: 0x17000C79 RID: 3193
		// (get) Token: 0x0600386E RID: 14446 RVA: 0x0012D4A4 File Offset: 0x0012B6A4
		// (set) Token: 0x0600386F RID: 14447 RVA: 0x0012D4AC File Offset: 0x0012B6AC
		internal PSModuleInfo ModuleInfo { get; private set; }

		// Token: 0x06003870 RID: 14448 RVA: 0x0012D4B8 File Offset: 0x0012B6B8
		public override Ast Copy()
		{
			UsingStatementAst usingStatementAst = (this.Alias != null) ? new UsingStatementAst(base.Extent, this.UsingStatementKind, this.Name, this.Alias) : new UsingStatementAst(base.Extent, this.UsingStatementKind, this.Name);
			usingStatementAst.ModuleInfo = this.ModuleInfo;
			return usingStatementAst;
		}

		// Token: 0x06003871 RID: 14449 RVA: 0x0012D514 File Offset: 0x0012B714
		internal override object Accept(ICustomAstVisitor visitor)
		{
			ICustomAstVisitor2 customAstVisitor = visitor as ICustomAstVisitor2;
			if (customAstVisitor == null)
			{
				return null;
			}
			return customAstVisitor.VisitUsingStatement(this);
		}

		// Token: 0x06003872 RID: 14450 RVA: 0x0012D534 File Offset: 0x0012B734
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitor2 astVisitor = visitor as AstVisitor2;
			AstVisitAction astVisitAction;
			if (astVisitor != null)
			{
				astVisitAction = astVisitor.VisitUsingStatement(this);
				if (astVisitAction != AstVisitAction.Continue)
				{
					return visitor.CheckForPostAction(this, astVisitAction);
				}
			}
			astVisitAction = this.Name.InternalVisit(visitor);
			if (astVisitAction != AstVisitAction.Continue)
			{
				return visitor.CheckForPostAction(this, astVisitAction);
			}
			if (this.Alias != null)
			{
				astVisitAction = this.Alias.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}

		// Token: 0x06003873 RID: 14451 RVA: 0x0012D597 File Offset: 0x0012B797
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06003874 RID: 14452 RVA: 0x0012D5A0 File Offset: 0x0012B7A0
		internal ReadOnlyDictionary<string, TypeDefinitionAst> DefineImportedModule(PSModuleInfo moduleInfo)
		{
			ReadOnlyDictionary<string, TypeDefinitionAst> exportedTypeDefinitions = moduleInfo.GetExportedTypeDefinitions();
			this.ModuleInfo = moduleInfo;
			return exportedTypeDefinitions;
		}

		// Token: 0x06003875 RID: 14453 RVA: 0x0012D5BC File Offset: 0x0012B7BC
		internal bool IsUsingModuleOrAssembly()
		{
			return this.UsingStatementKind == UsingStatementKind.Assembly || this.UsingStatementKind == UsingStatementKind.Module;
		}
	}
}
