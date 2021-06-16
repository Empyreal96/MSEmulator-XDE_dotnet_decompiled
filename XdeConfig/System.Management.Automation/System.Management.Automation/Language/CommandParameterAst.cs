using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x0200056A RID: 1386
	public class CommandParameterAst : CommandElementAst
	{
		// Token: 0x06003978 RID: 14712 RVA: 0x0012FF9C File Offset: 0x0012E19C
		public CommandParameterAst(IScriptExtent extent, string parameterName, ExpressionAst argument, IScriptExtent errorPosition) : base(extent)
		{
			if (errorPosition == null || string.IsNullOrEmpty(parameterName))
			{
				throw PSTraceSource.NewArgumentNullException((errorPosition == null) ? "errorPosition" : "parameterName");
			}
			this.ParameterName = parameterName;
			if (argument != null)
			{
				this.Argument = argument;
				base.SetParent(argument);
			}
			this.ErrorPosition = errorPosition;
		}

		// Token: 0x17000CBE RID: 3262
		// (get) Token: 0x06003979 RID: 14713 RVA: 0x0012FFF2 File Offset: 0x0012E1F2
		// (set) Token: 0x0600397A RID: 14714 RVA: 0x0012FFFA File Offset: 0x0012E1FA
		public string ParameterName { get; private set; }

		// Token: 0x17000CBF RID: 3263
		// (get) Token: 0x0600397B RID: 14715 RVA: 0x00130003 File Offset: 0x0012E203
		// (set) Token: 0x0600397C RID: 14716 RVA: 0x0013000B File Offset: 0x0012E20B
		public ExpressionAst Argument { get; private set; }

		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x0600397D RID: 14717 RVA: 0x00130014 File Offset: 0x0012E214
		// (set) Token: 0x0600397E RID: 14718 RVA: 0x0013001C File Offset: 0x0012E21C
		public IScriptExtent ErrorPosition { get; private set; }

		// Token: 0x0600397F RID: 14719 RVA: 0x00130028 File Offset: 0x0012E228
		public override Ast Copy()
		{
			ExpressionAst argument = Ast.CopyElement<ExpressionAst>(this.Argument);
			return new CommandParameterAst(base.Extent, this.ParameterName, argument, this.ErrorPosition);
		}

		// Token: 0x06003980 RID: 14720 RVA: 0x00130059 File Offset: 0x0012E259
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return Ast.EmptyPSTypeNameArray;
		}

		// Token: 0x06003981 RID: 14721 RVA: 0x00130060 File Offset: 0x0012E260
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitCommandParameter(this);
		}

		// Token: 0x06003982 RID: 14722 RVA: 0x0013006C File Offset: 0x0012E26C
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitCommandParameter(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (this.Argument != null && astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.Argument.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}
	}
}
