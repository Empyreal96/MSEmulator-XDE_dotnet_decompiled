using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Management.Automation.Language
{
	// Token: 0x02000545 RID: 1349
	public class AttributeAst : AttributeBaseAst
	{
		// Token: 0x06003827 RID: 14375 RVA: 0x0012C7A8 File Offset: 0x0012A9A8
		public AttributeAst(IScriptExtent extent, ITypeName typeName, IEnumerable<ExpressionAst> positionalArguments, IEnumerable<NamedAttributeArgumentAst> namedArguments) : base(extent, typeName)
		{
			if (positionalArguments != null)
			{
				this.PositionalArguments = new ReadOnlyCollection<ExpressionAst>(positionalArguments.ToArray<ExpressionAst>());
				base.SetParents<ExpressionAst>(this.PositionalArguments);
			}
			else
			{
				this.PositionalArguments = AttributeAst.EmptyPositionalArguments;
			}
			if (namedArguments != null)
			{
				this.NamedArguments = new ReadOnlyCollection<NamedAttributeArgumentAst>(namedArguments.ToArray<NamedAttributeArgumentAst>());
				base.SetParents<NamedAttributeArgumentAst>(this.NamedArguments);
				return;
			}
			this.NamedArguments = AttributeAst.EmptyNamedAttributeArguments;
		}

		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x06003828 RID: 14376 RVA: 0x0012C818 File Offset: 0x0012AA18
		// (set) Token: 0x06003829 RID: 14377 RVA: 0x0012C820 File Offset: 0x0012AA20
		public ReadOnlyCollection<ExpressionAst> PositionalArguments { get; private set; }

		// Token: 0x17000C66 RID: 3174
		// (get) Token: 0x0600382A RID: 14378 RVA: 0x0012C829 File Offset: 0x0012AA29
		// (set) Token: 0x0600382B RID: 14379 RVA: 0x0012C831 File Offset: 0x0012AA31
		public ReadOnlyCollection<NamedAttributeArgumentAst> NamedArguments { get; private set; }

		// Token: 0x0600382C RID: 14380 RVA: 0x0012C83C File Offset: 0x0012AA3C
		public override Ast Copy()
		{
			ExpressionAst[] positionalArguments = Ast.CopyElements<ExpressionAst>(this.PositionalArguments);
			NamedAttributeArgumentAst[] namedArguments = Ast.CopyElements<NamedAttributeArgumentAst>(this.NamedArguments);
			return new AttributeAst(base.Extent, base.TypeName, positionalArguments, namedArguments);
		}

		// Token: 0x0600382D RID: 14381 RVA: 0x0012C874 File Offset: 0x0012AA74
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitAttribute(this);
		}

		// Token: 0x0600382E RID: 14382 RVA: 0x0012C880 File Offset: 0x0012AA80
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitAttribute(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				for (int i = 0; i < this.PositionalArguments.Count; i++)
				{
					ExpressionAst expressionAst = this.PositionalArguments[i];
					astVisitAction = expressionAst.InternalVisit(visitor);
					if (astVisitAction != AstVisitAction.Continue)
					{
						break;
					}
				}
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				for (int j = 0; j < this.NamedArguments.Count; j++)
				{
					NamedAttributeArgumentAst namedAttributeArgumentAst = this.NamedArguments[j];
					astVisitAction = namedAttributeArgumentAst.InternalVisit(visitor);
					if (astVisitAction != AstVisitAction.Continue)
					{
						break;
					}
				}
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}

		// Token: 0x0600382F RID: 14383 RVA: 0x0012C90E File Offset: 0x0012AB0E
		internal override Attribute GetAttribute()
		{
			return Compiler.GetAttribute(this);
		}

		// Token: 0x04001C97 RID: 7319
		private static readonly ReadOnlyCollection<ExpressionAst> EmptyPositionalArguments = new ReadOnlyCollection<ExpressionAst>(new ExpressionAst[0]);

		// Token: 0x04001C98 RID: 7320
		private static readonly ReadOnlyCollection<NamedAttributeArgumentAst> EmptyNamedAttributeArguments = new ReadOnlyCollection<NamedAttributeArgumentAst>(new NamedAttributeArgumentAst[0]);
	}
}
