using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x02000563 RID: 1379
	public class TrapStatementAst : StatementAst
	{
		// Token: 0x06003942 RID: 14658 RVA: 0x0012F9A9 File Offset: 0x0012DBA9
		public TrapStatementAst(IScriptExtent extent, TypeConstraintAst trapType, StatementBlockAst body) : base(extent)
		{
			if (body == null)
			{
				throw PSTraceSource.NewArgumentNullException("body");
			}
			if (trapType != null)
			{
				this.TrapType = trapType;
				base.SetParent(trapType);
			}
			this.Body = body;
			base.SetParent(body);
		}

		// Token: 0x17000CB5 RID: 3253
		// (get) Token: 0x06003943 RID: 14659 RVA: 0x0012F9DF File Offset: 0x0012DBDF
		// (set) Token: 0x06003944 RID: 14660 RVA: 0x0012F9E7 File Offset: 0x0012DBE7
		public TypeConstraintAst TrapType { get; private set; }

		// Token: 0x17000CB6 RID: 3254
		// (get) Token: 0x06003945 RID: 14661 RVA: 0x0012F9F0 File Offset: 0x0012DBF0
		// (set) Token: 0x06003946 RID: 14662 RVA: 0x0012F9F8 File Offset: 0x0012DBF8
		public StatementBlockAst Body { get; private set; }

		// Token: 0x06003947 RID: 14663 RVA: 0x0012FA04 File Offset: 0x0012DC04
		public override Ast Copy()
		{
			TypeConstraintAst trapType = Ast.CopyElement<TypeConstraintAst>(this.TrapType);
			StatementBlockAst body = Ast.CopyElement<StatementBlockAst>(this.Body);
			return new TrapStatementAst(base.Extent, trapType, body);
		}

		// Token: 0x06003948 RID: 14664 RVA: 0x0012FA36 File Offset: 0x0012DC36
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return this.Body.GetInferredType(context);
		}

		// Token: 0x06003949 RID: 14665 RVA: 0x0012FA44 File Offset: 0x0012DC44
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitTrap(this);
		}

		// Token: 0x0600394A RID: 14666 RVA: 0x0012FA50 File Offset: 0x0012DC50
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitTrap(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue && this.TrapType != null)
			{
				astVisitAction = this.TrapType.InternalVisit(visitor);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.Body.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}
	}
}
