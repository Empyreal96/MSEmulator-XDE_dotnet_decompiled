using System;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200077B RID: 1915
	public class AlternateStreamData
	{
		// Token: 0x17000FE9 RID: 4073
		// (get) Token: 0x06004C7C RID: 19580 RVA: 0x001947C7 File Offset: 0x001929C7
		// (set) Token: 0x06004C7D RID: 19581 RVA: 0x001947CF File Offset: 0x001929CF
		public string FileName { get; set; }

		// Token: 0x17000FEA RID: 4074
		// (get) Token: 0x06004C7E RID: 19582 RVA: 0x001947D8 File Offset: 0x001929D8
		// (set) Token: 0x06004C7F RID: 19583 RVA: 0x001947E0 File Offset: 0x001929E0
		public string Stream { get; set; }

		// Token: 0x17000FEB RID: 4075
		// (get) Token: 0x06004C80 RID: 19584 RVA: 0x001947E9 File Offset: 0x001929E9
		// (set) Token: 0x06004C81 RID: 19585 RVA: 0x001947F1 File Offset: 0x001929F1
		public long Length { get; set; }
	}
}
