using System;

namespace System.Management.Automation.Internal
{
	// Token: 0x02000041 RID: 65
	public sealed class TransactionParameters
	{
		// Token: 0x0600031F RID: 799 RVA: 0x0000BC02 File Offset: 0x00009E02
		internal TransactionParameters(MshCommandRuntime commandRuntime)
		{
			this.commandRuntime = commandRuntime;
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000320 RID: 800 RVA: 0x0000BC11 File Offset: 0x00009E11
		// (set) Token: 0x06000321 RID: 801 RVA: 0x0000BC1E File Offset: 0x00009E1E
		[Parameter]
		[Alias(new string[]
		{
			"usetx"
		})]
		public SwitchParameter UseTransaction
		{
			get
			{
				return this.commandRuntime.UseTransaction;
			}
			set
			{
				this.commandRuntime.UseTransaction = value;
			}
		}

		// Token: 0x0400010A RID: 266
		private MshCommandRuntime commandRuntime;
	}
}
