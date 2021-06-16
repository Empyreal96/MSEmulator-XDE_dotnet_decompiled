using System;

namespace System.Management.Automation.Language
{
	// Token: 0x02000571 RID: 1393
	public class FileRedirectionAst : RedirectionAst
	{
		// Token: 0x060039A5 RID: 14757 RVA: 0x00130F90 File Offset: 0x0012F190
		public FileRedirectionAst(IScriptExtent extent, RedirectionStream stream, ExpressionAst file, bool append) : base(extent, stream)
		{
			if (file == null)
			{
				throw PSTraceSource.NewArgumentNullException("file");
			}
			this.Location = file;
			base.SetParent(file);
			this.Append = append;
		}

		// Token: 0x17000CC8 RID: 3272
		// (get) Token: 0x060039A6 RID: 14758 RVA: 0x00130FBE File Offset: 0x0012F1BE
		// (set) Token: 0x060039A7 RID: 14759 RVA: 0x00130FC6 File Offset: 0x0012F1C6
		public ExpressionAst Location { get; private set; }

		// Token: 0x17000CC9 RID: 3273
		// (get) Token: 0x060039A8 RID: 14760 RVA: 0x00130FCF File Offset: 0x0012F1CF
		// (set) Token: 0x060039A9 RID: 14761 RVA: 0x00130FD7 File Offset: 0x0012F1D7
		public bool Append { get; private set; }

		// Token: 0x060039AA RID: 14762 RVA: 0x00130FE0 File Offset: 0x0012F1E0
		public override Ast Copy()
		{
			ExpressionAst file = Ast.CopyElement<ExpressionAst>(this.Location);
			return new FileRedirectionAst(base.Extent, base.FromStream, file, this.Append);
		}

		// Token: 0x060039AB RID: 14763 RVA: 0x00131011 File Offset: 0x0012F211
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitFileRedirection(this);
		}

		// Token: 0x060039AC RID: 14764 RVA: 0x0013101C File Offset: 0x0012F21C
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitFileRedirection(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.Location.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}
	}
}
