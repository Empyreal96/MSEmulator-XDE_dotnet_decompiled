using System;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	// Token: 0x02000039 RID: 57
	public class RecoveryData
	{
		// Token: 0x06000206 RID: 518 RVA: 0x00005C37 File Offset: 0x00003E37
		public RecoveryData(RecoveryCallback callback, object state)
		{
			this.Callback = callback;
			this.State = state;
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00005C4D File Offset: 0x00003E4D
		// (set) Token: 0x06000208 RID: 520 RVA: 0x00005C55 File Offset: 0x00003E55
		public RecoveryCallback Callback { get; set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000209 RID: 521 RVA: 0x00005C5E File Offset: 0x00003E5E
		// (set) Token: 0x0600020A RID: 522 RVA: 0x00005C66 File Offset: 0x00003E66
		public object State { get; set; }

		// Token: 0x0600020B RID: 523 RVA: 0x00005C6F File Offset: 0x00003E6F
		public void Invoke()
		{
			if (this.Callback != null)
			{
				this.Callback(this.State);
			}
		}
	}
}
