using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x02000578 RID: 1400
	public class BlockStatementAst : StatementAst
	{
		// Token: 0x06003A0A RID: 14858 RVA: 0x0013294C File Offset: 0x00130B4C
		public BlockStatementAst(IScriptExtent extent, Token kind, StatementBlockAst body) : base(extent)
		{
			if (kind == null || body == null)
			{
				throw PSTraceSource.NewArgumentNullException((kind == null) ? "kind" : "body");
			}
			if (kind.Kind != TokenKind.Sequence && kind.Kind != TokenKind.Parallel)
			{
				throw PSTraceSource.NewArgumentException("kind");
			}
			this.Kind = kind;
			this.Body = body;
			base.SetParent(body);
		}

		// Token: 0x17000CE8 RID: 3304
		// (get) Token: 0x06003A0B RID: 14859 RVA: 0x001329B5 File Offset: 0x00130BB5
		// (set) Token: 0x06003A0C RID: 14860 RVA: 0x001329BD File Offset: 0x00130BBD
		public StatementBlockAst Body { get; private set; }

		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x06003A0D RID: 14861 RVA: 0x001329C6 File Offset: 0x00130BC6
		// (set) Token: 0x06003A0E RID: 14862 RVA: 0x001329CE File Offset: 0x00130BCE
		public Token Kind { get; private set; }

		// Token: 0x06003A0F RID: 14863 RVA: 0x001329D8 File Offset: 0x00130BD8
		public override Ast Copy()
		{
			StatementBlockAst body = Ast.CopyElement<StatementBlockAst>(this.Body);
			return new BlockStatementAst(base.Extent, this.Kind, body);
		}

		// Token: 0x06003A10 RID: 14864 RVA: 0x00132A03 File Offset: 0x00130C03
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return this.Body.GetInferredType(context);
		}

		// Token: 0x06003A11 RID: 14865 RVA: 0x00132A11 File Offset: 0x00130C11
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitBlockStatement(this);
		}

		// Token: 0x06003A12 RID: 14866 RVA: 0x00132A1C File Offset: 0x00130C1C
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitBlockStatement(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.Body.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}
	}
}
