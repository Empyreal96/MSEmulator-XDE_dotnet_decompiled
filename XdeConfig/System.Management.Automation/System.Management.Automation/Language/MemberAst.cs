using System;

namespace System.Management.Automation.Language
{
	// Token: 0x0200054D RID: 1357
	public abstract class MemberAst : Ast
	{
		// Token: 0x06003876 RID: 14454 RVA: 0x0012D5D1 File Offset: 0x0012B7D1
		protected MemberAst(IScriptExtent extent) : base(extent)
		{
		}

		// Token: 0x17000C7A RID: 3194
		// (get) Token: 0x06003877 RID: 14455
		public abstract string Name { get; }

		// Token: 0x06003878 RID: 14456
		internal abstract string GetTooltip();
	}
}
