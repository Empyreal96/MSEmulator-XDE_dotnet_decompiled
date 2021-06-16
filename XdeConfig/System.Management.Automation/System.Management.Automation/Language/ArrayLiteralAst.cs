using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Management.Automation.Language
{
	// Token: 0x0200058B RID: 1419
	public class ArrayLiteralAst : ExpressionAst, ISupportsAssignment
	{
		// Token: 0x06003ADF RID: 15071 RVA: 0x001362FA File Offset: 0x001344FA
		public ArrayLiteralAst(IScriptExtent extent, IList<ExpressionAst> elements) : base(extent)
		{
			if (elements == null || !elements.Any<ExpressionAst>())
			{
				throw PSTraceSource.NewArgumentException("elements");
			}
			this.Elements = new ReadOnlyCollection<ExpressionAst>(elements);
			base.SetParents<ExpressionAst>(this.Elements);
		}

		// Token: 0x17000D2C RID: 3372
		// (get) Token: 0x06003AE0 RID: 15072 RVA: 0x00136331 File Offset: 0x00134531
		// (set) Token: 0x06003AE1 RID: 15073 RVA: 0x00136339 File Offset: 0x00134539
		public ReadOnlyCollection<ExpressionAst> Elements { get; private set; }

		// Token: 0x06003AE2 RID: 15074 RVA: 0x00136344 File Offset: 0x00134544
		public override Ast Copy()
		{
			ExpressionAst[] elements = Ast.CopyElements<ExpressionAst>(this.Elements);
			return new ArrayLiteralAst(base.Extent, elements);
		}

		// Token: 0x17000D2D RID: 3373
		// (get) Token: 0x06003AE3 RID: 15075 RVA: 0x00136369 File Offset: 0x00134569
		public override Type StaticType
		{
			get
			{
				return typeof(object[]);
			}
		}

		// Token: 0x06003AE4 RID: 15076 RVA: 0x00136448 File Offset: 0x00134648
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			yield return new PSTypeName(typeof(object[]));
			yield break;
		}

		// Token: 0x06003AE5 RID: 15077 RVA: 0x00136465 File Offset: 0x00134665
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitArrayLiteral(this);
		}

		// Token: 0x06003AE6 RID: 15078 RVA: 0x00136470 File Offset: 0x00134670
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitArrayLiteral(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				for (int i = 0; i < this.Elements.Count; i++)
				{
					ExpressionAst expressionAst = this.Elements[i];
					astVisitAction = expressionAst.InternalVisit(visitor);
					if (astVisitAction != AstVisitAction.Continue)
					{
						break;
					}
				}
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}

		// Token: 0x06003AE7 RID: 15079 RVA: 0x001364CC File Offset: 0x001346CC
		IAssignableValue ISupportsAssignment.GetAssignableValue()
		{
			return new ArrayAssignableValue
			{
				ArrayLiteral = this
			};
		}
	}
}
