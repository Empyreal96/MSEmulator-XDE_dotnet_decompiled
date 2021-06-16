using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x0200058A RID: 1418
	public class ScriptBlockExpressionAst : ExpressionAst
	{
		// Token: 0x06003AD7 RID: 15063 RVA: 0x0013615B File Offset: 0x0013435B
		public ScriptBlockExpressionAst(IScriptExtent extent, ScriptBlockAst scriptBlock) : base(extent)
		{
			if (scriptBlock == null)
			{
				throw PSTraceSource.NewArgumentNullException("scriptBlock");
			}
			this.ScriptBlock = scriptBlock;
			base.SetParent(scriptBlock);
		}

		// Token: 0x17000D2A RID: 3370
		// (get) Token: 0x06003AD8 RID: 15064 RVA: 0x00136180 File Offset: 0x00134380
		// (set) Token: 0x06003AD9 RID: 15065 RVA: 0x00136188 File Offset: 0x00134388
		public ScriptBlockAst ScriptBlock { get; private set; }

		// Token: 0x06003ADA RID: 15066 RVA: 0x00136194 File Offset: 0x00134394
		public override Ast Copy()
		{
			ScriptBlockAst scriptBlock = Ast.CopyElement<ScriptBlockAst>(this.ScriptBlock);
			return new ScriptBlockExpressionAst(base.Extent, scriptBlock);
		}

		// Token: 0x17000D2B RID: 3371
		// (get) Token: 0x06003ADB RID: 15067 RVA: 0x001361B9 File Offset: 0x001343B9
		public override Type StaticType
		{
			get
			{
				return typeof(ScriptBlock);
			}
		}

		// Token: 0x06003ADC RID: 15068 RVA: 0x00136298 File Offset: 0x00134498
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			yield return new PSTypeName(typeof(ScriptBlock));
			yield break;
		}

		// Token: 0x06003ADD RID: 15069 RVA: 0x001362B5 File Offset: 0x001344B5
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitScriptBlockExpression(this);
		}

		// Token: 0x06003ADE RID: 15070 RVA: 0x001362C0 File Offset: 0x001344C0
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitScriptBlockExpression(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.ScriptBlock.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}
	}
}
