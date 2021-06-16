using System;
using System.Collections.Generic;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x02000419 RID: 1049
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public sealed class AliasAttribute : ParsingBaseAttribute
	{
		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x06002EBB RID: 11963 RVA: 0x001003FD File Offset: 0x000FE5FD
		public IList<string> AliasNames
		{
			get
			{
				return this.aliasNames;
			}
		}

		// Token: 0x06002EBC RID: 11964 RVA: 0x00100405 File Offset: 0x000FE605
		public AliasAttribute(params string[] aliasNames)
		{
			if (aliasNames == null)
			{
				throw PSTraceSource.NewArgumentNullException("aliasNames");
			}
			this.aliasNames = aliasNames;
		}

		// Token: 0x04001894 RID: 6292
		internal string[] aliasNames;
	}
}
