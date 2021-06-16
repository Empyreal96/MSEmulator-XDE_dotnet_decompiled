using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x0200056E RID: 1390
	public abstract class RedirectionAst : Ast
	{
		// Token: 0x0600399B RID: 14747 RVA: 0x00130EFF File Offset: 0x0012F0FF
		protected RedirectionAst(IScriptExtent extent, RedirectionStream from) : base(extent)
		{
			this.FromStream = from;
		}

		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x0600399C RID: 14748 RVA: 0x00130F0F File Offset: 0x0012F10F
		// (set) Token: 0x0600399D RID: 14749 RVA: 0x00130F17 File Offset: 0x0012F117
		public RedirectionStream FromStream { get; private set; }

		// Token: 0x0600399E RID: 14750 RVA: 0x00130F20 File Offset: 0x0012F120
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return Ast.EmptyPSTypeNameArray;
		}
	}
}
