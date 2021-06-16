using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Management.Automation.Language
{
	// Token: 0x02000589 RID: 1417
	public class ExpandableStringExpressionAst : ExpressionAst
	{
		// Token: 0x06003AC7 RID: 15047 RVA: 0x00135E5C File Offset: 0x0013405C
		public ExpandableStringExpressionAst(IScriptExtent extent, string value, StringConstantType type) : base(extent)
		{
			if (value == null)
			{
				throw PSTraceSource.NewArgumentNullException("value");
			}
			if (type != StringConstantType.DoubleQuoted && type != StringConstantType.DoubleQuotedHereString && type != StringConstantType.BareWord)
			{
				throw PSTraceSource.NewArgumentException("type");
			}
			ExpressionAst expressionAst = Parser.ScanString(value);
			ExpandableStringExpressionAst expandableStringExpressionAst = expressionAst as ExpandableStringExpressionAst;
			if (expandableStringExpressionAst != null)
			{
				this.FormatExpression = expandableStringExpressionAst.FormatExpression;
				this.NestedExpressions = expandableStringExpressionAst.NestedExpressions;
			}
			else
			{
				this.FormatExpression = "{0}";
				this.NestedExpressions = new ReadOnlyCollection<ExpressionAst>(new ExpressionAst[]
				{
					expressionAst
				});
			}
			for (int i = 0; i < this.NestedExpressions.Count; i++)
			{
				this.NestedExpressions[i].ClearParent();
			}
			base.SetParents<ExpressionAst>(this.NestedExpressions);
			this.Value = value;
			this.StringConstantType = type;
		}

		// Token: 0x06003AC8 RID: 15048 RVA: 0x00135F23 File Offset: 0x00134123
		internal ExpandableStringExpressionAst(Token token, string value, string formatString, IEnumerable<ExpressionAst> nestedExpressions) : this(token.Extent, value, formatString, StringConstantExpressionAst.MapTokenKindToStringContantKind(token), nestedExpressions)
		{
		}

		// Token: 0x06003AC9 RID: 15049 RVA: 0x00135F3B File Offset: 0x0013413B
		private ExpandableStringExpressionAst(IScriptExtent extent, string value, string formatString, StringConstantType type, IEnumerable<ExpressionAst> nestedExpressions) : base(extent)
		{
			this.FormatExpression = formatString;
			this.Value = value;
			this.StringConstantType = type;
			this.NestedExpressions = new ReadOnlyCollection<ExpressionAst>(nestedExpressions.ToArray<ExpressionAst>());
			base.SetParents<ExpressionAst>(this.NestedExpressions);
		}

		// Token: 0x17000D25 RID: 3365
		// (get) Token: 0x06003ACA RID: 15050 RVA: 0x00135F78 File Offset: 0x00134178
		// (set) Token: 0x06003ACB RID: 15051 RVA: 0x00135F80 File Offset: 0x00134180
		public string Value { get; private set; }

		// Token: 0x17000D26 RID: 3366
		// (get) Token: 0x06003ACC RID: 15052 RVA: 0x00135F89 File Offset: 0x00134189
		// (set) Token: 0x06003ACD RID: 15053 RVA: 0x00135F91 File Offset: 0x00134191
		public StringConstantType StringConstantType { get; private set; }

		// Token: 0x17000D27 RID: 3367
		// (get) Token: 0x06003ACE RID: 15054 RVA: 0x00135F9A File Offset: 0x0013419A
		// (set) Token: 0x06003ACF RID: 15055 RVA: 0x00135FA2 File Offset: 0x001341A2
		public ReadOnlyCollection<ExpressionAst> NestedExpressions { get; private set; }

		// Token: 0x06003AD0 RID: 15056 RVA: 0x00135FAC File Offset: 0x001341AC
		public override Ast Copy()
		{
			ExpressionAst[] nestedExpressions = Ast.CopyElements<ExpressionAst>(this.NestedExpressions);
			return new ExpandableStringExpressionAst(base.Extent, this.Value, this.FormatExpression, this.StringConstantType, nestedExpressions);
		}

		// Token: 0x17000D28 RID: 3368
		// (get) Token: 0x06003AD1 RID: 15057 RVA: 0x00135FE3 File Offset: 0x001341E3
		public override Type StaticType
		{
			get
			{
				return typeof(string);
			}
		}

		// Token: 0x17000D29 RID: 3369
		// (get) Token: 0x06003AD2 RID: 15058 RVA: 0x00135FEF File Offset: 0x001341EF
		// (set) Token: 0x06003AD3 RID: 15059 RVA: 0x00135FF7 File Offset: 0x001341F7
		internal string FormatExpression { get; private set; }

		// Token: 0x06003AD4 RID: 15060 RVA: 0x001360D0 File Offset: 0x001342D0
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			yield return new PSTypeName(typeof(string));
			yield break;
		}

		// Token: 0x06003AD5 RID: 15061 RVA: 0x001360ED File Offset: 0x001342ED
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitExpandableStringExpression(this);
		}

		// Token: 0x06003AD6 RID: 15062 RVA: 0x001360F8 File Offset: 0x001342F8
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitExpandableStringExpression(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue && this.NestedExpressions != null)
			{
				for (int i = 0; i < this.NestedExpressions.Count; i++)
				{
					ExpressionAst expressionAst = this.NestedExpressions[i];
					astVisitAction = expressionAst.InternalVisit(visitor);
					if (astVisitAction != AstVisitAction.Continue)
					{
						break;
					}
				}
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}
	}
}
