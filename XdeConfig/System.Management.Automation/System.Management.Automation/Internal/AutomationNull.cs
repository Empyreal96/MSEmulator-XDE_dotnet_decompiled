using System;

namespace System.Management.Automation.Internal
{
	// Token: 0x0200075C RID: 1884
	public static class AutomationNull
	{
		// Token: 0x17000FBB RID: 4027
		// (get) Token: 0x06004B1A RID: 19226 RVA: 0x00189462 File Offset: 0x00187662
		public static PSObject Value
		{
			get
			{
				return AutomationNull.value1;
			}
		}

		// Token: 0x04002447 RID: 9287
		private static readonly PSObject value1 = new PSObject();
	}
}
