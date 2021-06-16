using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x02000538 RID: 1336
	internal class SequencePointAst : Ast
	{
		// Token: 0x06003796 RID: 14230 RVA: 0x0012A961 File Offset: 0x00128B61
		public SequencePointAst(IScriptExtent extent) : base(extent)
		{
		}

		// Token: 0x06003797 RID: 14231 RVA: 0x0012A96A File Offset: 0x00128B6A
		public override Ast Copy()
		{
			return null;
		}

		// Token: 0x06003798 RID: 14232 RVA: 0x0012A96D File Offset: 0x00128B6D
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return null;
		}

		// Token: 0x06003799 RID: 14233 RVA: 0x0012A970 File Offset: 0x00128B70
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			return visitor.CheckForPostAction(this, AstVisitAction.Continue);
		}

		// Token: 0x0600379A RID: 14234 RVA: 0x0012A97A File Offset: 0x00128B7A
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return Ast.EmptyPSTypeNameArray;
		}
	}
}
