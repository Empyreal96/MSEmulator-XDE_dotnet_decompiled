using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x02000543 RID: 1347
	public class NamedAttributeArgumentAst : Ast
	{
		// Token: 0x06003817 RID: 14359 RVA: 0x0012C66C File Offset: 0x0012A86C
		public NamedAttributeArgumentAst(IScriptExtent extent, string argumentName, ExpressionAst argument, bool expressionOmitted) : base(extent)
		{
			if (string.IsNullOrEmpty(argumentName))
			{
				throw PSTraceSource.NewArgumentNullException("argumentName");
			}
			if (argument == null)
			{
				throw PSTraceSource.NewArgumentNullException("argument");
			}
			this.Argument = argument;
			base.SetParent(argument);
			this.ArgumentName = argumentName;
			this.ExpressionOmitted = expressionOmitted;
		}

		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x06003818 RID: 14360 RVA: 0x0012C6BE File Offset: 0x0012A8BE
		// (set) Token: 0x06003819 RID: 14361 RVA: 0x0012C6C6 File Offset: 0x0012A8C6
		public string ArgumentName { get; private set; }

		// Token: 0x17000C62 RID: 3170
		// (get) Token: 0x0600381A RID: 14362 RVA: 0x0012C6CF File Offset: 0x0012A8CF
		// (set) Token: 0x0600381B RID: 14363 RVA: 0x0012C6D7 File Offset: 0x0012A8D7
		public ExpressionAst Argument { get; private set; }

		// Token: 0x17000C63 RID: 3171
		// (get) Token: 0x0600381C RID: 14364 RVA: 0x0012C6E0 File Offset: 0x0012A8E0
		// (set) Token: 0x0600381D RID: 14365 RVA: 0x0012C6E8 File Offset: 0x0012A8E8
		public bool ExpressionOmitted { get; private set; }

		// Token: 0x0600381E RID: 14366 RVA: 0x0012C6F4 File Offset: 0x0012A8F4
		public override Ast Copy()
		{
			ExpressionAst argument = Ast.CopyElement<ExpressionAst>(this.Argument);
			return new NamedAttributeArgumentAst(base.Extent, this.ArgumentName, argument, this.ExpressionOmitted);
		}

		// Token: 0x0600381F RID: 14367 RVA: 0x0012C725 File Offset: 0x0012A925
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return Ast.EmptyPSTypeNameArray;
		}

		// Token: 0x06003820 RID: 14368 RVA: 0x0012C72C File Offset: 0x0012A92C
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitNamedAttributeArgument(this);
		}

		// Token: 0x06003821 RID: 14369 RVA: 0x0012C738 File Offset: 0x0012A938
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitNamedAttributeArgument(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.Argument.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}
	}
}
