using System;

namespace System.Management.Automation.Language
{
	// Token: 0x02000546 RID: 1350
	public class TypeConstraintAst : AttributeBaseAst
	{
		// Token: 0x06003831 RID: 14385 RVA: 0x0012C938 File Offset: 0x0012AB38
		public TypeConstraintAst(IScriptExtent extent, ITypeName typeName) : base(extent, typeName)
		{
		}

		// Token: 0x06003832 RID: 14386 RVA: 0x0012C942 File Offset: 0x0012AB42
		public TypeConstraintAst(IScriptExtent extent, Type type) : base(extent, new ReflectionTypeName(type))
		{
		}

		// Token: 0x06003833 RID: 14387 RVA: 0x0012C951 File Offset: 0x0012AB51
		public override Ast Copy()
		{
			return new TypeConstraintAst(base.Extent, base.TypeName);
		}

		// Token: 0x06003834 RID: 14388 RVA: 0x0012C964 File Offset: 0x0012AB64
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitTypeConstraint(this);
		}

		// Token: 0x06003835 RID: 14389 RVA: 0x0012C970 File Offset: 0x0012AB70
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitTypeConstraint(this);
			return visitor.CheckForPostAction(this, (astVisitAction == AstVisitAction.SkipChildren) ? AstVisitAction.Continue : astVisitAction);
		}

		// Token: 0x06003836 RID: 14390 RVA: 0x0012C994 File Offset: 0x0012AB94
		internal override Attribute GetAttribute()
		{
			return Compiler.GetAttribute(this);
		}
	}
}
