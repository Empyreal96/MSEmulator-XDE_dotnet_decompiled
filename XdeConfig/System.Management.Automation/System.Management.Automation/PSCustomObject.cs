using System;

namespace System.Management.Automation
{
	// Token: 0x02000155 RID: 341
	public class PSCustomObject
	{
		// Token: 0x0600117A RID: 4474 RVA: 0x00060849 File Offset: 0x0005EA49
		private PSCustomObject()
		{
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x00060851 File Offset: 0x0005EA51
		public override string ToString()
		{
			return "";
		}

		// Token: 0x0400076C RID: 1900
		internal static PSCustomObject SelfInstance = new PSCustomObject();
	}
}
