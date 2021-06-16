using System;

namespace System.Management.Automation.Internal
{
	// Token: 0x02000040 RID: 64
	public sealed class ShouldProcessParameters
	{
		// Token: 0x0600031A RID: 794 RVA: 0x0000BBAF File Offset: 0x00009DAF
		internal ShouldProcessParameters(MshCommandRuntime commandRuntime)
		{
			if (commandRuntime == null)
			{
				throw PSTraceSource.NewArgumentNullException("commandRuntime");
			}
			this.commandRuntime = commandRuntime;
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600031B RID: 795 RVA: 0x0000BBCC File Offset: 0x00009DCC
		// (set) Token: 0x0600031C RID: 796 RVA: 0x0000BBD9 File Offset: 0x00009DD9
		[Alias(new string[]
		{
			"wi"
		})]
		[Parameter]
		public SwitchParameter WhatIf
		{
			get
			{
				return this.commandRuntime.WhatIf;
			}
			set
			{
				this.commandRuntime.WhatIf = value;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600031D RID: 797 RVA: 0x0000BBE7 File Offset: 0x00009DE7
		// (set) Token: 0x0600031E RID: 798 RVA: 0x0000BBF4 File Offset: 0x00009DF4
		[Parameter]
		[Alias(new string[]
		{
			"cf"
		})]
		public SwitchParameter Confirm
		{
			get
			{
				return this.commandRuntime.Confirm;
			}
			set
			{
				this.commandRuntime.Confirm = value;
			}
		}

		// Token: 0x04000109 RID: 265
		private MshCommandRuntime commandRuntime;
	}
}
