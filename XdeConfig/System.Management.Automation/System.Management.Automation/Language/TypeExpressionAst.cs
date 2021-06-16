using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x02000584 RID: 1412
	public class TypeExpressionAst : ExpressionAst
	{
		// Token: 0x06003A93 RID: 14995 RVA: 0x001348AA File Offset: 0x00132AAA
		public TypeExpressionAst(IScriptExtent extent, ITypeName typeName) : base(extent)
		{
			if (typeName == null)
			{
				throw PSTraceSource.NewArgumentNullException("typeName");
			}
			this.TypeName = typeName;
		}

		// Token: 0x17000D19 RID: 3353
		// (get) Token: 0x06003A94 RID: 14996 RVA: 0x001348C8 File Offset: 0x00132AC8
		// (set) Token: 0x06003A95 RID: 14997 RVA: 0x001348D0 File Offset: 0x00132AD0
		public ITypeName TypeName { get; private set; }

		// Token: 0x06003A96 RID: 14998 RVA: 0x001348D9 File Offset: 0x00132AD9
		public override Ast Copy()
		{
			return new TypeExpressionAst(base.Extent, this.TypeName);
		}

		// Token: 0x17000D1A RID: 3354
		// (get) Token: 0x06003A97 RID: 14999 RVA: 0x001348EC File Offset: 0x00132AEC
		public override Type StaticType
		{
			get
			{
				return typeof(Type);
			}
		}

		// Token: 0x06003A98 RID: 15000 RVA: 0x001349CC File Offset: 0x00132BCC
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			yield return new PSTypeName(this.StaticType);
			yield break;
		}

		// Token: 0x06003A99 RID: 15001 RVA: 0x001349E9 File Offset: 0x00132BE9
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitTypeExpression(this);
		}

		// Token: 0x06003A9A RID: 15002 RVA: 0x001349F4 File Offset: 0x00132BF4
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitTypeExpression(this);
			return visitor.CheckForPostAction(this, (astVisitAction == AstVisitAction.SkipChildren) ? AstVisitAction.Continue : astVisitAction);
		}
	}
}
