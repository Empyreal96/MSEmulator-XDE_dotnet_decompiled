using System;

namespace System.Management.Automation
{
	// Token: 0x0200041B RID: 1051
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class PSTypeNameAttribute : Attribute
	{
		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x06002ED2 RID: 11986 RVA: 0x0010053B File Offset: 0x000FE73B
		// (set) Token: 0x06002ED3 RID: 11987 RVA: 0x00100543 File Offset: 0x000FE743
		public string PSTypeName { get; private set; }

		// Token: 0x06002ED4 RID: 11988 RVA: 0x0010054C File Offset: 0x000FE74C
		public PSTypeNameAttribute(string psTypeName)
		{
			if (string.IsNullOrEmpty(psTypeName))
			{
				throw PSTraceSource.NewArgumentException("psTypeName");
			}
			this.PSTypeName = psTypeName;
		}
	}
}
