using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Management.Automation.Language
{
	// Token: 0x02000542 RID: 1346
	public class NamedBlockAst : Ast
	{
		// Token: 0x06003806 RID: 14342 RVA: 0x0012C3A8 File Offset: 0x0012A5A8
		public NamedBlockAst(IScriptExtent extent, TokenKind blockName, StatementBlockAst statementBlock, bool unnamed) : base(extent)
		{
			if (!blockName.HasTrait(TokenFlags.ScriptBlockBlockName) || (unnamed && (blockName == TokenKind.Begin || blockName == TokenKind.Dynamicparam)))
			{
				throw PSTraceSource.NewArgumentException("blockName");
			}
			if (statementBlock == null)
			{
				throw PSTraceSource.NewArgumentNullException("statementBlock");
			}
			this.Unnamed = unnamed;
			this.BlockKind = blockName;
			ReadOnlyCollection<StatementAst> statements = statementBlock.Statements;
			this.Statements = statements;
			for (int i = 0; i < statements.Count; i++)
			{
				StatementAst statementAst = statements[i];
				statementAst.ClearParent();
			}
			base.SetParents<StatementAst>(statements);
			ReadOnlyCollection<TrapStatementAst> traps = statementBlock.Traps;
			if (traps != null && traps.Any<TrapStatementAst>())
			{
				this.Traps = traps;
				for (int j = 0; j < traps.Count; j++)
				{
					TrapStatementAst trapStatementAst = traps[j];
					trapStatementAst.ClearParent();
				}
				base.SetParents<TrapStatementAst>(traps);
			}
			if (!unnamed)
			{
				InternalScriptExtent internalScriptExtent = statementBlock.Extent as InternalScriptExtent;
				if (internalScriptExtent != null)
				{
					this.OpenCurlyExtent = new InternalScriptExtent(internalScriptExtent.PositionHelper, internalScriptExtent.StartOffset, internalScriptExtent.StartOffset + 1);
					this.CloseCurlyExtent = new InternalScriptExtent(internalScriptExtent.PositionHelper, internalScriptExtent.EndOffset - 1, internalScriptExtent.EndOffset);
				}
			}
		}

		// Token: 0x17000C5B RID: 3163
		// (get) Token: 0x06003807 RID: 14343 RVA: 0x0012C4CF File Offset: 0x0012A6CF
		// (set) Token: 0x06003808 RID: 14344 RVA: 0x0012C4D7 File Offset: 0x0012A6D7
		public bool Unnamed { get; private set; }

		// Token: 0x17000C5C RID: 3164
		// (get) Token: 0x06003809 RID: 14345 RVA: 0x0012C4E0 File Offset: 0x0012A6E0
		// (set) Token: 0x0600380A RID: 14346 RVA: 0x0012C4E8 File Offset: 0x0012A6E8
		public TokenKind BlockKind { get; private set; }

		// Token: 0x17000C5D RID: 3165
		// (get) Token: 0x0600380B RID: 14347 RVA: 0x0012C4F1 File Offset: 0x0012A6F1
		// (set) Token: 0x0600380C RID: 14348 RVA: 0x0012C4F9 File Offset: 0x0012A6F9
		public ReadOnlyCollection<StatementAst> Statements { get; private set; }

		// Token: 0x17000C5E RID: 3166
		// (get) Token: 0x0600380D RID: 14349 RVA: 0x0012C502 File Offset: 0x0012A702
		// (set) Token: 0x0600380E RID: 14350 RVA: 0x0012C50A File Offset: 0x0012A70A
		public ReadOnlyCollection<TrapStatementAst> Traps { get; private set; }

		// Token: 0x0600380F RID: 14351 RVA: 0x0012C514 File Offset: 0x0012A714
		public override Ast Copy()
		{
			TrapStatementAst[] traps = Ast.CopyElements<TrapStatementAst>(this.Traps);
			StatementAst[] array = Ast.CopyElements<StatementAst>(this.Statements);
			IScriptExtent extent = base.Extent;
			if (this.OpenCurlyExtent != null && this.CloseCurlyExtent != null)
			{
				InternalScriptExtent internalScriptExtent = (InternalScriptExtent)this.OpenCurlyExtent;
				InternalScriptExtent internalScriptExtent2 = (InternalScriptExtent)this.CloseCurlyExtent;
				extent = new InternalScriptExtent(internalScriptExtent.PositionHelper, internalScriptExtent.StartOffset, internalScriptExtent2.EndOffset);
			}
			array = (array ?? new StatementAst[0]);
			StatementBlockAst statementBlock = new StatementBlockAst(extent, array, traps);
			return new NamedBlockAst(base.Extent, this.BlockKind, statementBlock, this.Unnamed);
		}

		// Token: 0x17000C5F RID: 3167
		// (get) Token: 0x06003810 RID: 14352 RVA: 0x0012C5B1 File Offset: 0x0012A7B1
		// (set) Token: 0x06003811 RID: 14353 RVA: 0x0012C5B9 File Offset: 0x0012A7B9
		internal IScriptExtent OpenCurlyExtent { get; private set; }

		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x06003812 RID: 14354 RVA: 0x0012C5C2 File Offset: 0x0012A7C2
		// (set) Token: 0x06003813 RID: 14355 RVA: 0x0012C5CA File Offset: 0x0012A7CA
		internal IScriptExtent CloseCurlyExtent { get; private set; }

		// Token: 0x06003814 RID: 14356 RVA: 0x0012C5EC File Offset: 0x0012A7EC
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return this.Statements.SelectMany((StatementAst ast) => ast.GetInferredType(context));
		}

		// Token: 0x06003815 RID: 14357 RVA: 0x0012C61D File Offset: 0x0012A81D
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitNamedBlock(this);
		}

		// Token: 0x06003816 RID: 14358 RVA: 0x0012C628 File Offset: 0x0012A828
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitNamedBlock(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = StatementBlockAst.InternalVisit(visitor, this.Traps, this.Statements, astVisitAction);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}
	}
}
