using System;

namespace System.Management.Automation
{
	// Token: 0x02000076 RID: 118
	internal sealed class UnboundParameter
	{
		// Token: 0x06000643 RID: 1603 RVA: 0x0001F014 File Offset: 0x0001D214
		private UnboundParameter()
		{
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x0001F01C File Offset: 0x0001D21C
		internal static object Value
		{
			get
			{
				return UnboundParameter._singletonValue;
			}
		}

		// Token: 0x0400028F RID: 655
		private static readonly object _singletonValue = new object();
	}
}
