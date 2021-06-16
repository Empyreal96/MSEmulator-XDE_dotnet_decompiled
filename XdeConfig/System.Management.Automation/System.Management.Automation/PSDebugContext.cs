using System;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x020000F8 RID: 248
	public class PSDebugContext
	{
		// Token: 0x06000DCD RID: 3533 RVA: 0x0004B735 File Offset: 0x00049935
		public PSDebugContext(InvocationInfo invocationInfo, List<Breakpoint> breakpoints)
		{
			if (breakpoints == null)
			{
				throw new PSArgumentNullException("breakpoints");
			}
			this.InvocationInfo = invocationInfo;
			this.Breakpoints = breakpoints.ToArray();
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000DCE RID: 3534 RVA: 0x0004B75E File Offset: 0x0004995E
		// (set) Token: 0x06000DCF RID: 3535 RVA: 0x0004B766 File Offset: 0x00049966
		public InvocationInfo InvocationInfo { get; private set; }

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000DD0 RID: 3536 RVA: 0x0004B76F File Offset: 0x0004996F
		// (set) Token: 0x06000DD1 RID: 3537 RVA: 0x0004B777 File Offset: 0x00049977
		public Breakpoint[] Breakpoints { get; private set; }
	}
}
