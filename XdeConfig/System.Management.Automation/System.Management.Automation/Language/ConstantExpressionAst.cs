using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x02000586 RID: 1414
	public class ConstantExpressionAst : ExpressionAst
	{
		// Token: 0x06003AB4 RID: 15028 RVA: 0x00135BB7 File Offset: 0x00133DB7
		public ConstantExpressionAst(IScriptExtent extent, object value) : base(extent)
		{
			this.Value = value;
		}

		// Token: 0x06003AB5 RID: 15029 RVA: 0x00135BC7 File Offset: 0x00133DC7
		internal ConstantExpressionAst(NumberToken token) : base(token.Extent)
		{
			this.Value = token.Value;
		}

		// Token: 0x17000D20 RID: 3360
		// (get) Token: 0x06003AB6 RID: 15030 RVA: 0x00135BE1 File Offset: 0x00133DE1
		// (set) Token: 0x06003AB7 RID: 15031 RVA: 0x00135BE9 File Offset: 0x00133DE9
		public object Value { get; private set; }

		// Token: 0x06003AB8 RID: 15032 RVA: 0x00135BF2 File Offset: 0x00133DF2
		public override Ast Copy()
		{
			return new ConstantExpressionAst(base.Extent, this.Value);
		}

		// Token: 0x17000D21 RID: 3361
		// (get) Token: 0x06003AB9 RID: 15033 RVA: 0x00135C05 File Offset: 0x00133E05
		public override Type StaticType
		{
			get
			{
				if (this.Value == null)
				{
					return typeof(object);
				}
				return this.Value.GetType();
			}
		}

		// Token: 0x06003ABA RID: 15034 RVA: 0x00135D0C File Offset: 0x00133F0C
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			if (this.Value != null)
			{
				yield return new PSTypeName(this.Value.GetType());
			}
			yield break;
		}

		// Token: 0x06003ABB RID: 15035 RVA: 0x00135D29 File Offset: 0x00133F29
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitConstantExpression(this);
		}

		// Token: 0x06003ABC RID: 15036 RVA: 0x00135D34 File Offset: 0x00133F34
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitConstantExpression(this);
			return visitor.CheckForPostAction(this, (astVisitAction == AstVisitAction.SkipChildren) ? AstVisitAction.Continue : astVisitAction);
		}
	}
}
