using System;
using System.Threading;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002AF RID: 687
	public class CmdletMethodInvoker<T>
	{
		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x0600213B RID: 8507 RVA: 0x000BFBF6 File Offset: 0x000BDDF6
		// (set) Token: 0x0600213C RID: 8508 RVA: 0x000BFBFE File Offset: 0x000BDDFE
		public Func<Cmdlet, T> Action { get; set; }

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x0600213D RID: 8509 RVA: 0x000BFC07 File Offset: 0x000BDE07
		// (set) Token: 0x0600213E RID: 8510 RVA: 0x000BFC0F File Offset: 0x000BDE0F
		public Exception ExceptionThrownOnCmdletThread { get; set; }

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x0600213F RID: 8511 RVA: 0x000BFC18 File Offset: 0x000BDE18
		// (set) Token: 0x06002140 RID: 8512 RVA: 0x000BFC20 File Offset: 0x000BDE20
		public ManualResetEventSlim Finished { get; set; }

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x06002141 RID: 8513 RVA: 0x000BFC29 File Offset: 0x000BDE29
		// (set) Token: 0x06002142 RID: 8514 RVA: 0x000BFC31 File Offset: 0x000BDE31
		public object SyncObject { get; set; }

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x06002143 RID: 8515 RVA: 0x000BFC3A File Offset: 0x000BDE3A
		// (set) Token: 0x06002144 RID: 8516 RVA: 0x000BFC42 File Offset: 0x000BDE42
		public T MethodResult { get; set; }
	}
}
