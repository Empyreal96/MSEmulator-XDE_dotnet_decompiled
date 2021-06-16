using System;
using System.Globalization;
using Microsoft.WindowsAPICodePack.Resources;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	// Token: 0x0200003A RID: 58
	[CLSCompliant(false)]
	public class RecoverySettings
	{
		// Token: 0x0600020C RID: 524 RVA: 0x00005C8B File Offset: 0x00003E8B
		public RecoverySettings(RecoveryData data, uint interval)
		{
			this.recoveryData = data;
			this.pingInterval = interval;
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00005CA1 File Offset: 0x00003EA1
		public RecoveryData RecoveryData
		{
			get
			{
				return this.recoveryData;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600020E RID: 526 RVA: 0x00005CA9 File Offset: 0x00003EA9
		public uint PingInterval
		{
			get
			{
				return this.pingInterval;
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00005CB4 File Offset: 0x00003EB4
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, LocalizedMessages.RecoverySettingsFormatString, this.recoveryData.Callback.Method.ToString(), this.recoveryData.State.ToString(), this.PingInterval);
		}

		// Token: 0x0400019F RID: 415
		private RecoveryData recoveryData;

		// Token: 0x040001A0 RID: 416
		private uint pingInterval;
	}
}
