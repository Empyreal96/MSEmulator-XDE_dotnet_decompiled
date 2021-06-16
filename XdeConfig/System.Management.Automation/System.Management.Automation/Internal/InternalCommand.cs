using System;
using System.Diagnostics;
using System.Management.Automation.Host;
using System.Management.Automation.Language;

namespace System.Management.Automation.Internal
{
	// Token: 0x02000009 RID: 9
	[DebuggerDisplay("Command = {commandInfo}")]
	public abstract class InternalCommand
	{
		// Token: 0x06000046 RID: 70 RVA: 0x00002CF6 File Offset: 0x00000EF6
		internal InternalCommand()
		{
			this.CommandInfo = null;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002D17 File Offset: 0x00000F17
		// (set) Token: 0x06000048 RID: 72 RVA: 0x00002D1F File Offset: 0x00000F1F
		internal IScriptExtent InvocationExtent { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002D28 File Offset: 0x00000F28
		internal InvocationInfo MyInvocation
		{
			get
			{
				if (this.myInvocation == null)
				{
					this.myInvocation = new InvocationInfo(this);
				}
				return this.myInvocation;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002D44 File Offset: 0x00000F44
		// (set) Token: 0x0600004B RID: 75 RVA: 0x00002D4C File Offset: 0x00000F4C
		internal PSObject CurrentPipelineObject
		{
			get
			{
				return this.currentObjectInPipeline;
			}
			set
			{
				this.currentObjectInPipeline = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002D55 File Offset: 0x00000F55
		internal PSHost PSHostInternal
		{
			get
			{
				return this.CBhost;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002D5D File Offset: 0x00000F5D
		internal SessionState InternalState
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00002D68 File Offset: 0x00000F68
		internal bool IsStopping
		{
			get
			{
				MshCommandRuntime mshCommandRuntime = this.commandRuntime as MshCommandRuntime;
				return mshCommandRuntime != null && mshCommandRuntime.IsStopping;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002D8C File Offset: 0x00000F8C
		// (set) Token: 0x06000050 RID: 80 RVA: 0x00002D94 File Offset: 0x00000F94
		internal CommandInfo CommandInfo
		{
			get
			{
				return this.commandInfo;
			}
			set
			{
				this.commandInfo = value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002D9D File Offset: 0x00000F9D
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00002DA5 File Offset: 0x00000FA5
		internal ExecutionContext Context
		{
			get
			{
				return this.context;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("Context");
				}
				this.context = value;
				this.CBhost = this.context.EngineHostInterface;
				this.state = new SessionState(this.context.EngineSessionState);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002DE3 File Offset: 0x00000FE3
		public CommandOrigin CommandOrigin
		{
			get
			{
				return this.CommandOriginInternal;
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002DEB File Offset: 0x00000FEB
		internal virtual void DoBeginProcessing()
		{
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002DED File Offset: 0x00000FED
		internal virtual void DoProcessRecord()
		{
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002DEF File Offset: 0x00000FEF
		internal virtual void DoEndProcessing()
		{
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002DF1 File Offset: 0x00000FF1
		internal virtual void DoStopProcessing()
		{
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002DF3 File Offset: 0x00000FF3
		internal void ThrowIfStopping()
		{
			if (this.IsStopping)
			{
				throw new PipelineStoppedException();
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002E03 File Offset: 0x00001003
		internal void InternalDispose(bool isDisposing)
		{
			this.myInvocation = null;
			this.state = null;
			this.commandInfo = null;
			this.context = null;
		}

		// Token: 0x04000018 RID: 24
		internal ICommandRuntime commandRuntime;

		// Token: 0x04000019 RID: 25
		private InvocationInfo myInvocation;

		// Token: 0x0400001A RID: 26
		internal PSObject currentObjectInPipeline = AutomationNull.Value;

		// Token: 0x0400001B RID: 27
		private PSHost CBhost;

		// Token: 0x0400001C RID: 28
		private SessionState state;

		// Token: 0x0400001D RID: 29
		private CommandInfo commandInfo;

		// Token: 0x0400001E RID: 30
		private ExecutionContext context;

		// Token: 0x0400001F RID: 31
		internal CommandOrigin CommandOriginInternal = CommandOrigin.Internal;
	}
}
