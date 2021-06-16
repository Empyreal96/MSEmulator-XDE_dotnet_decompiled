using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x02000577 RID: 1399
	public class UnaryExpressionAst : ExpressionAst
	{
		// Token: 0x06003A00 RID: 14848 RVA: 0x00132814 File Offset: 0x00130A14
		public UnaryExpressionAst(IScriptExtent extent, TokenKind tokenKind, ExpressionAst child) : base(extent)
		{
			if ((tokenKind.GetTraits() & TokenFlags.UnaryOperator) == TokenFlags.None)
			{
				throw PSTraceSource.NewArgumentException("tokenKind");
			}
			if (child == null)
			{
				throw PSTraceSource.NewArgumentNullException("child");
			}
			this.TokenKind = tokenKind;
			this.Child = child;
			base.SetParent(child);
		}

		// Token: 0x17000CE5 RID: 3301
		// (get) Token: 0x06003A01 RID: 14849 RVA: 0x00132864 File Offset: 0x00130A64
		// (set) Token: 0x06003A02 RID: 14850 RVA: 0x0013286C File Offset: 0x00130A6C
		public TokenKind TokenKind { get; private set; }

		// Token: 0x17000CE6 RID: 3302
		// (get) Token: 0x06003A03 RID: 14851 RVA: 0x00132875 File Offset: 0x00130A75
		// (set) Token: 0x06003A04 RID: 14852 RVA: 0x0013287D File Offset: 0x00130A7D
		public ExpressionAst Child { get; private set; }

		// Token: 0x06003A05 RID: 14853 RVA: 0x00132888 File Offset: 0x00130A88
		public override Ast Copy()
		{
			ExpressionAst child = Ast.CopyElement<ExpressionAst>(this.Child);
			return new UnaryExpressionAst(base.Extent, this.TokenKind, child);
		}

		// Token: 0x17000CE7 RID: 3303
		// (get) Token: 0x06003A06 RID: 14854 RVA: 0x001328B3 File Offset: 0x00130AB3
		public override Type StaticType
		{
			get
			{
				if (this.TokenKind != TokenKind.Not && this.TokenKind != TokenKind.Exclaim)
				{
					return typeof(object);
				}
				return typeof(bool);
			}
		}

		// Token: 0x06003A07 RID: 14855 RVA: 0x001328DE File Offset: 0x00130ADE
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			if (this.TokenKind != TokenKind.Not && this.TokenKind != TokenKind.Exclaim)
			{
				return this.Child.GetInferredType(context);
			}
			return BinaryExpressionAst.BoolTypeNameArray;
		}

		// Token: 0x06003A08 RID: 14856 RVA: 0x00132906 File Offset: 0x00130B06
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitUnaryExpression(this);
		}

		// Token: 0x06003A09 RID: 14857 RVA: 0x00132910 File Offset: 0x00130B10
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitUnaryExpression(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.Child.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}
	}
}
