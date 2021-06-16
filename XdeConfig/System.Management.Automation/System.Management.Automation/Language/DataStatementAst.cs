using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Management.Automation.Language
{
	// Token: 0x02000556 RID: 1366
	public class DataStatementAst : StatementAst
	{
		// Token: 0x060038E6 RID: 14566 RVA: 0x0012E708 File Offset: 0x0012C908
		public DataStatementAst(IScriptExtent extent, string variableName, IEnumerable<ExpressionAst> commandsAllowed, StatementBlockAst body) : base(extent)
		{
			if (body == null)
			{
				throw PSTraceSource.NewArgumentNullException("body");
			}
			if (string.IsNullOrWhiteSpace(variableName))
			{
				variableName = null;
			}
			this.Variable = variableName;
			if (commandsAllowed != null && commandsAllowed.Any<ExpressionAst>())
			{
				this.CommandsAllowed = new ReadOnlyCollection<ExpressionAst>(commandsAllowed.ToArray<ExpressionAst>());
				base.SetParents<ExpressionAst>(this.CommandsAllowed);
				this.HasNonConstantAllowedCommand = this.CommandsAllowed.Any((ExpressionAst ast) => !(ast is StringConstantExpressionAst));
			}
			else
			{
				this.CommandsAllowed = new ReadOnlyCollection<ExpressionAst>(DataStatementAst.EmptyCommandsAllowed);
			}
			this.Body = body;
			base.SetParent(body);
		}

		// Token: 0x17000C9F RID: 3231
		// (get) Token: 0x060038E7 RID: 14567 RVA: 0x0012E7BC File Offset: 0x0012C9BC
		// (set) Token: 0x060038E8 RID: 14568 RVA: 0x0012E7C4 File Offset: 0x0012C9C4
		public string Variable { get; private set; }

		// Token: 0x17000CA0 RID: 3232
		// (get) Token: 0x060038E9 RID: 14569 RVA: 0x0012E7CD File Offset: 0x0012C9CD
		// (set) Token: 0x060038EA RID: 14570 RVA: 0x0012E7D5 File Offset: 0x0012C9D5
		public ReadOnlyCollection<ExpressionAst> CommandsAllowed { get; private set; }

		// Token: 0x17000CA1 RID: 3233
		// (get) Token: 0x060038EB RID: 14571 RVA: 0x0012E7DE File Offset: 0x0012C9DE
		// (set) Token: 0x060038EC RID: 14572 RVA: 0x0012E7E6 File Offset: 0x0012C9E6
		public StatementBlockAst Body { get; private set; }

		// Token: 0x060038ED RID: 14573 RVA: 0x0012E7F0 File Offset: 0x0012C9F0
		public override Ast Copy()
		{
			ExpressionAst[] commandsAllowed = Ast.CopyElements<ExpressionAst>(this.CommandsAllowed);
			StatementBlockAst body = Ast.CopyElement<StatementBlockAst>(this.Body);
			return new DataStatementAst(base.Extent, this.Variable, commandsAllowed, body);
		}

		// Token: 0x060038EE RID: 14574 RVA: 0x0012E828 File Offset: 0x0012CA28
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return this.Body.GetInferredType(context);
		}

		// Token: 0x17000CA2 RID: 3234
		// (get) Token: 0x060038EF RID: 14575 RVA: 0x0012E836 File Offset: 0x0012CA36
		// (set) Token: 0x060038F0 RID: 14576 RVA: 0x0012E83E File Offset: 0x0012CA3E
		internal bool HasNonConstantAllowedCommand { get; private set; }

		// Token: 0x060038F1 RID: 14577 RVA: 0x0012E847 File Offset: 0x0012CA47
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitDataStatement(this);
		}

		// Token: 0x060038F2 RID: 14578 RVA: 0x0012E850 File Offset: 0x0012CA50
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitDataStatement(this);
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

		// Token: 0x17000CA3 RID: 3235
		// (get) Token: 0x060038F3 RID: 14579 RVA: 0x0012E88A File Offset: 0x0012CA8A
		// (set) Token: 0x060038F4 RID: 14580 RVA: 0x0012E892 File Offset: 0x0012CA92
		internal int TupleIndex
		{
			get
			{
				return this._tupleIndex;
			}
			set
			{
				this._tupleIndex = value;
			}
		}

		// Token: 0x04001CE3 RID: 7395
		private static readonly ExpressionAst[] EmptyCommandsAllowed = new ExpressionAst[0];

		// Token: 0x04001CE4 RID: 7396
		private int _tupleIndex = -1;
	}
}
