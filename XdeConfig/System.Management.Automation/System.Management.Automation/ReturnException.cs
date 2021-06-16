using System;

namespace System.Management.Automation
{
	// Token: 0x02000481 RID: 1153
	internal class ReturnException : FlowControlException
	{
		// Token: 0x0600334B RID: 13131 RVA: 0x0011828A File Offset: 0x0011648A
		internal ReturnException(object argument)
		{
			this.Argument = argument;
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x0600334C RID: 13132 RVA: 0x00118299 File Offset: 0x00116499
		// (set) Token: 0x0600334D RID: 13133 RVA: 0x001182A1 File Offset: 0x001164A1
		internal object Argument { get; set; }
	}
}
