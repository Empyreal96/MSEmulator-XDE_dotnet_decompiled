using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x02000576 RID: 1398
	public class BinaryExpressionAst : ExpressionAst
	{
		// Token: 0x060039F1 RID: 14833 RVA: 0x00132600 File Offset: 0x00130800
		public BinaryExpressionAst(IScriptExtent extent, ExpressionAst left, TokenKind @operator, ExpressionAst right, IScriptExtent errorPosition) : base(extent)
		{
			if ((@operator.GetTraits() & TokenFlags.BinaryOperator) == TokenFlags.None)
			{
				throw PSTraceSource.NewArgumentException("operator");
			}
			if (left == null || right == null || errorPosition == null)
			{
				throw PSTraceSource.NewArgumentNullException((left == null) ? "left" : ((right == null) ? "right" : "errorPosition"));
			}
			this.Left = left;
			base.SetParent(left);
			this.Operator = @operator;
			this.Right = right;
			base.SetParent(right);
			this.ErrorPosition = errorPosition;
		}

		// Token: 0x17000CE0 RID: 3296
		// (get) Token: 0x060039F2 RID: 14834 RVA: 0x00132685 File Offset: 0x00130885
		// (set) Token: 0x060039F3 RID: 14835 RVA: 0x0013268D File Offset: 0x0013088D
		public TokenKind Operator { get; private set; }

		// Token: 0x17000CE1 RID: 3297
		// (get) Token: 0x060039F4 RID: 14836 RVA: 0x00132696 File Offset: 0x00130896
		// (set) Token: 0x060039F5 RID: 14837 RVA: 0x0013269E File Offset: 0x0013089E
		public ExpressionAst Left { get; private set; }

		// Token: 0x17000CE2 RID: 3298
		// (get) Token: 0x060039F6 RID: 14838 RVA: 0x001326A7 File Offset: 0x001308A7
		// (set) Token: 0x060039F7 RID: 14839 RVA: 0x001326AF File Offset: 0x001308AF
		public ExpressionAst Right { get; private set; }

		// Token: 0x17000CE3 RID: 3299
		// (get) Token: 0x060039F8 RID: 14840 RVA: 0x001326B8 File Offset: 0x001308B8
		// (set) Token: 0x060039F9 RID: 14841 RVA: 0x001326C0 File Offset: 0x001308C0
		public IScriptExtent ErrorPosition { get; private set; }

		// Token: 0x060039FA RID: 14842 RVA: 0x001326CC File Offset: 0x001308CC
		public override Ast Copy()
		{
			ExpressionAst left = Ast.CopyElement<ExpressionAst>(this.Left);
			ExpressionAst right = Ast.CopyElement<ExpressionAst>(this.Right);
			return new BinaryExpressionAst(base.Extent, left, this.Operator, right, this.ErrorPosition);
		}

		// Token: 0x17000CE4 RID: 3300
		// (get) Token: 0x060039FB RID: 14843 RVA: 0x0013270C File Offset: 0x0013090C
		public override Type StaticType
		{
			get
			{
				TokenKind @operator = this.Operator;
				switch (@operator)
				{
				case TokenKind.And:
				case TokenKind.Or:
				case TokenKind.Xor:
					break;
				default:
					if (@operator != TokenKind.Is)
					{
						return typeof(object);
					}
					break;
				}
				return typeof(bool);
			}
		}

		// Token: 0x060039FC RID: 14844 RVA: 0x00132750 File Offset: 0x00130950
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			TokenKind @operator = this.Operator;
			switch (@operator)
			{
			case TokenKind.And:
			case TokenKind.Or:
			case TokenKind.Xor:
				break;
			default:
				if (@operator != TokenKind.Is)
				{
					return this.Left.GetInferredType(context);
				}
				break;
			}
			return BinaryExpressionAst.BoolTypeNameArray;
		}

		// Token: 0x060039FD RID: 14845 RVA: 0x00132790 File Offset: 0x00130990
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitBinaryExpression(this);
		}

		// Token: 0x060039FE RID: 14846 RVA: 0x0013279C File Offset: 0x0013099C
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitBinaryExpression(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.Left.InternalVisit(visitor);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.Right.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}

		// Token: 0x04001D49 RID: 7497
		internal static readonly PSTypeName[] BoolTypeNameArray = new PSTypeName[]
		{
			new PSTypeName(typeof(bool))
		};
	}
}
