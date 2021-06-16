using System;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x02000485 RID: 1157
	internal class StopUpstreamCommandsException : FlowControlException
	{
		// Token: 0x06003355 RID: 13141 RVA: 0x001182EC File Offset: 0x001164EC
		public StopUpstreamCommandsException(InternalCommand requestingCommand)
		{
			this.RequestingCommandProcessor = requestingCommand.Context.CurrentCommandProcessor;
		}

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x06003356 RID: 13142 RVA: 0x00118305 File Offset: 0x00116505
		// (set) Token: 0x06003357 RID: 13143 RVA: 0x0011830D File Offset: 0x0011650D
		public CommandProcessorBase RequestingCommandProcessor { get; private set; }
	}
}
