using System;

namespace System.Management.Automation.Language
{
	// Token: 0x02000588 RID: 1416
	public class StringConstantExpressionAst : ConstantExpressionAst
	{
		// Token: 0x06003ABD RID: 15037 RVA: 0x00135D58 File Offset: 0x00133F58
		public StringConstantExpressionAst(IScriptExtent extent, string value, StringConstantType stringConstantType) : base(extent, value)
		{
			if (value == null)
			{
				throw PSTraceSource.NewArgumentNullException("value");
			}
			this.StringConstantType = stringConstantType;
		}

		// Token: 0x06003ABE RID: 15038 RVA: 0x00135D77 File Offset: 0x00133F77
		internal StringConstantExpressionAst(StringToken token) : base(token.Extent, token.Value)
		{
			this.StringConstantType = StringConstantExpressionAst.MapTokenKindToStringContantKind(token);
		}

		// Token: 0x17000D22 RID: 3362
		// (get) Token: 0x06003ABF RID: 15039 RVA: 0x00135D97 File Offset: 0x00133F97
		// (set) Token: 0x06003AC0 RID: 15040 RVA: 0x00135D9F File Offset: 0x00133F9F
		public StringConstantType StringConstantType { get; private set; }

		// Token: 0x17000D23 RID: 3363
		// (get) Token: 0x06003AC1 RID: 15041 RVA: 0x00135DA8 File Offset: 0x00133FA8
		public new string Value
		{
			get
			{
				return (string)base.Value;
			}
		}

		// Token: 0x06003AC2 RID: 15042 RVA: 0x00135DB5 File Offset: 0x00133FB5
		public override Ast Copy()
		{
			return new StringConstantExpressionAst(base.Extent, this.Value, this.StringConstantType);
		}

		// Token: 0x17000D24 RID: 3364
		// (get) Token: 0x06003AC3 RID: 15043 RVA: 0x00135DCE File Offset: 0x00133FCE
		public override Type StaticType
		{
			get
			{
				return typeof(string);
			}
		}

		// Token: 0x06003AC4 RID: 15044 RVA: 0x00135DDC File Offset: 0x00133FDC
		internal static StringConstantType MapTokenKindToStringContantKind(Token token)
		{
			switch (token.Kind)
			{
			case TokenKind.Generic:
				return StringConstantType.BareWord;
			case TokenKind.StringLiteral:
				return StringConstantType.SingleQuoted;
			case TokenKind.StringExpandable:
				return StringConstantType.DoubleQuoted;
			case TokenKind.HereStringLiteral:
				return StringConstantType.SingleQuotedHereString;
			case TokenKind.HereStringExpandable:
				return StringConstantType.DoubleQuotedHereString;
			}
			throw PSTraceSource.NewInvalidOperationException();
		}

		// Token: 0x06003AC5 RID: 15045 RVA: 0x00135E2D File Offset: 0x0013402D
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitStringConstantExpression(this);
		}

		// Token: 0x06003AC6 RID: 15046 RVA: 0x00135E38 File Offset: 0x00134038
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitStringConstantExpression(this);
			return visitor.CheckForPostAction(this, (astVisitAction == AstVisitAction.SkipChildren) ? AstVisitAction.Continue : astVisitAction);
		}
	}
}
