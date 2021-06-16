using System;
using System.Runtime.InteropServices;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020003DD RID: 989
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal class WSManPluginOperationShutdownContext
	{
		// Token: 0x06002CF7 RID: 11511 RVA: 0x000F9B43 File Offset: 0x000F7D43
		internal WSManPluginOperationShutdownContext(IntPtr plgContext, IntPtr shContext, IntPtr cmdContext, bool isRcvOp)
		{
			this.pluginContext = plgContext;
			this.shellContext = shContext;
			this.commandContext = cmdContext;
			this.isReceiveOperation = isRcvOp;
			this.isShuttingDown = false;
		}

		// Token: 0x040017B3 RID: 6067
		internal IntPtr pluginContext;

		// Token: 0x040017B4 RID: 6068
		internal IntPtr shellContext;

		// Token: 0x040017B5 RID: 6069
		internal IntPtr commandContext;

		// Token: 0x040017B6 RID: 6070
		internal bool isReceiveOperation;

		// Token: 0x040017B7 RID: 6071
		internal bool isShuttingDown;
	}
}
