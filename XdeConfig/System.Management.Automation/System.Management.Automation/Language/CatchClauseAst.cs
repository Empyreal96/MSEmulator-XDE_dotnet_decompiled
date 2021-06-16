using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Management.Automation.Language
{
	// Token: 0x02000561 RID: 1377
	public class CatchClauseAst : Ast
	{
		// Token: 0x0600392B RID: 14635 RVA: 0x0012F300 File Offset: 0x0012D500
		public CatchClauseAst(IScriptExtent extent, IEnumerable<TypeConstraintAst> catchTypes, StatementBlockAst body) : base(extent)
		{
			if (body == null)
			{
				throw PSTraceSource.NewArgumentNullException("body");
			}
			if (catchTypes != null)
			{
				this.CatchTypes = new ReadOnlyCollection<TypeConstraintAst>(catchTypes.ToArray<TypeConstraintAst>());
				base.SetParents<TypeConstraintAst>(this.CatchTypes);
			}
			else
			{
				this.CatchTypes = CatchClauseAst.EmptyCatchTypes;
			}
			this.Body = body;
			base.SetParent(body);
		}

		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x0600392C RID: 14636 RVA: 0x0012F35D File Offset: 0x0012D55D
		// (set) Token: 0x0600392D RID: 14637 RVA: 0x0012F365 File Offset: 0x0012D565
		public ReadOnlyCollection<TypeConstraintAst> CatchTypes { get; private set; }

		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x0600392E RID: 14638 RVA: 0x0012F36E File Offset: 0x0012D56E
		public bool IsCatchAll
		{
			get
			{
				return this.CatchTypes.Count == 0;
			}
		}

		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x0600392F RID: 14639 RVA: 0x0012F37E File Offset: 0x0012D57E
		// (set) Token: 0x06003930 RID: 14640 RVA: 0x0012F386 File Offset: 0x0012D586
		public StatementBlockAst Body { get; private set; }

		// Token: 0x06003931 RID: 14641 RVA: 0x0012F390 File Offset: 0x0012D590
		public override Ast Copy()
		{
			TypeConstraintAst[] catchTypes = Ast.CopyElements<TypeConstraintAst>(this.CatchTypes);
			StatementBlockAst body = Ast.CopyElement<StatementBlockAst>(this.Body);
			return new CatchClauseAst(base.Extent, catchTypes, body);
		}

		// Token: 0x06003932 RID: 14642 RVA: 0x0012F3C2 File Offset: 0x0012D5C2
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return this.Body.GetInferredType(context);
		}

		// Token: 0x06003933 RID: 14643 RVA: 0x0012F3D0 File Offset: 0x0012D5D0
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitCatchClause(this);
		}

		// Token: 0x06003934 RID: 14644 RVA: 0x0012F3DC File Offset: 0x0012D5DC
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitCatchClause(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			for (int i = 0; i < this.CatchTypes.Count; i++)
			{
				TypeConstraintAst typeConstraintAst = this.CatchTypes[i];
				if (astVisitAction != AstVisitAction.Continue)
				{
					break;
				}
				astVisitAction = typeConstraintAst.InternalVisit(visitor);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.Body.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}

		// Token: 0x04001D01 RID: 7425
		private static readonly ReadOnlyCollection<TypeConstraintAst> EmptyCatchTypes = new ReadOnlyCollection<TypeConstraintAst>(new TypeConstraintAst[0]);
	}
}
