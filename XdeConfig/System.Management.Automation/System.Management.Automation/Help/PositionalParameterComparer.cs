using System;
using System.Collections;

namespace System.Management.Automation.Help
{
	// Token: 0x020001D4 RID: 468
	internal class PositionalParameterComparer : IComparer
	{
		// Token: 0x06001586 RID: 5510 RVA: 0x000871C4 File Offset: 0x000853C4
		public int Compare(object x, object y)
		{
			CommandParameterInfo commandParameterInfo = x as CommandParameterInfo;
			CommandParameterInfo commandParameterInfo2 = y as CommandParameterInfo;
			return commandParameterInfo.Position - commandParameterInfo2.Position;
		}
	}
}
