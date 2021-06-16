using System;

namespace System.Management.Automation.Language
{
	// Token: 0x02000570 RID: 1392
	public class MergingRedirectionAst : RedirectionAst
	{
		// Token: 0x0600399F RID: 14751 RVA: 0x00130F27 File Offset: 0x0012F127
		public MergingRedirectionAst(IScriptExtent extent, RedirectionStream from, RedirectionStream to) : base(extent, from)
		{
			this.ToStream = to;
		}

		// Token: 0x17000CC7 RID: 3271
		// (get) Token: 0x060039A0 RID: 14752 RVA: 0x00130F38 File Offset: 0x0012F138
		// (set) Token: 0x060039A1 RID: 14753 RVA: 0x00130F40 File Offset: 0x0012F140
		public RedirectionStream ToStream { get; private set; }

		// Token: 0x060039A2 RID: 14754 RVA: 0x00130F49 File Offset: 0x0012F149
		public override Ast Copy()
		{
			return new MergingRedirectionAst(base.Extent, base.FromStream, this.ToStream);
		}

		// Token: 0x060039A3 RID: 14755 RVA: 0x00130F62 File Offset: 0x0012F162
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitMergingRedirection(this);
		}

		// Token: 0x060039A4 RID: 14756 RVA: 0x00130F6C File Offset: 0x0012F16C
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitMergingRedirection(this);
			return visitor.CheckForPostAction(this, (astVisitAction == AstVisitAction.SkipChildren) ? AstVisitAction.Continue : astVisitAction);
		}
	}
}
