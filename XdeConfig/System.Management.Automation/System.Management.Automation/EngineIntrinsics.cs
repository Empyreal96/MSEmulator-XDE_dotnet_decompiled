using System;
using System.Management.Automation.Host;

namespace System.Management.Automation
{
	// Token: 0x020000C6 RID: 198
	public class EngineIntrinsics
	{
		// Token: 0x06000AC3 RID: 2755 RVA: 0x000403B0 File Offset: 0x0003E5B0
		private EngineIntrinsics()
		{
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x000403B8 File Offset: 0x0003E5B8
		internal EngineIntrinsics(ExecutionContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			this._context = context;
			this._host = context.EngineHostInterface;
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x000403E1 File Offset: 0x0003E5E1
		public PSHost Host
		{
			get
			{
				return this._host;
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x000403E9 File Offset: 0x0003E5E9
		public PSEventManager Events
		{
			get
			{
				return this._context.Events;
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x000403F6 File Offset: 0x0003E5F6
		public ProviderIntrinsics InvokeProvider
		{
			get
			{
				return this._context.EngineSessionState.InvokeProvider;
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x00040408 File Offset: 0x0003E608
		public SessionState SessionState
		{
			get
			{
				return this._context.EngineSessionState.PublicSessionState;
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x0004041A File Offset: 0x0003E61A
		public CommandInvocationIntrinsics InvokeCommand
		{
			get
			{
				if (this._invokeCommand == null)
				{
					this._invokeCommand = new CommandInvocationIntrinsics(this._context);
				}
				return this._invokeCommand;
			}
		}

		// Token: 0x040004C3 RID: 1219
		private ExecutionContext _context;

		// Token: 0x040004C4 RID: 1220
		private PSHost _host;

		// Token: 0x040004C5 RID: 1221
		private CommandInvocationIntrinsics _invokeCommand;
	}
}
