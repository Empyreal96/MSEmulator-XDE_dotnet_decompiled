using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x0200058D RID: 1421
	public class ArrayExpressionAst : ExpressionAst
	{
		// Token: 0x06003AF3 RID: 15091 RVA: 0x0013674A File Offset: 0x0013494A
		public ArrayExpressionAst(IScriptExtent extent, StatementBlockAst statementBlock) : base(extent)
		{
			if (statementBlock == null)
			{
				throw PSTraceSource.NewArgumentNullException("statementBlock");
			}
			this.SubExpression = statementBlock;
			base.SetParent(statementBlock);
		}

		// Token: 0x17000D31 RID: 3377
		// (get) Token: 0x06003AF4 RID: 15092 RVA: 0x0013676F File Offset: 0x0013496F
		// (set) Token: 0x06003AF5 RID: 15093 RVA: 0x00136777 File Offset: 0x00134977
		public StatementBlockAst SubExpression { get; private set; }

		// Token: 0x06003AF6 RID: 15094 RVA: 0x00136780 File Offset: 0x00134980
		public override Ast Copy()
		{
			StatementBlockAst statementBlock = Ast.CopyElement<StatementBlockAst>(this.SubExpression);
			return new ArrayExpressionAst(base.Extent, statementBlock);
		}

		// Token: 0x17000D32 RID: 3378
		// (get) Token: 0x06003AF7 RID: 15095 RVA: 0x001367A5 File Offset: 0x001349A5
		public override Type StaticType
		{
			get
			{
				return typeof(object[]);
			}
		}

		// Token: 0x06003AF8 RID: 15096 RVA: 0x00136884 File Offset: 0x00134A84
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			yield return new PSTypeName(typeof(object[]));
			yield break;
		}

		// Token: 0x06003AF9 RID: 15097 RVA: 0x001368A1 File Offset: 0x00134AA1
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitArrayExpression(this);
		}

		// Token: 0x06003AFA RID: 15098 RVA: 0x001368AC File Offset: 0x00134AAC
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitArrayExpression(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.SubExpression.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}
	}
}
