using System;
using System.Globalization;
using Microsoft.WindowsAPICodePack.Resources;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	// Token: 0x02000040 RID: 64
	public class BatteryState
	{
		// Token: 0x06000226 RID: 550 RVA: 0x00005DC8 File Offset: 0x00003FC8
		internal BatteryState()
		{
			PowerManagementNativeMethods.SystemBatteryState systemBatteryState = Power.GetSystemBatteryState();
			if (!systemBatteryState.BatteryPresent)
			{
				throw new InvalidOperationException(LocalizedMessages.PowerManagerBatteryNotPresent);
			}
			this.ACOnline = systemBatteryState.AcOnLine;
			this.MaxCharge = (int)systemBatteryState.MaxCapacity;
			this.CurrentCharge = (int)systemBatteryState.RemainingCapacity;
			this.ChargeRate = (int)systemBatteryState.Rate;
			uint estimatedTime = systemBatteryState.EstimatedTime;
			if (estimatedTime != 4294967295U)
			{
				this.EstimatedTimeRemaining = new TimeSpan(0, 0, (int)estimatedTime);
			}
			else
			{
				this.EstimatedTimeRemaining = TimeSpan.MaxValue;
			}
			this.SuggestedCriticalBatteryCharge = (int)systemBatteryState.DefaultAlert1;
			this.SuggestedBatteryWarningCharge = (int)systemBatteryState.DefaultAlert2;
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000227 RID: 551 RVA: 0x00005E62 File Offset: 0x00004062
		// (set) Token: 0x06000228 RID: 552 RVA: 0x00005E6A File Offset: 0x0000406A
		public bool ACOnline { get; private set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000229 RID: 553 RVA: 0x00005E73 File Offset: 0x00004073
		// (set) Token: 0x0600022A RID: 554 RVA: 0x00005E7B File Offset: 0x0000407B
		public int MaxCharge { get; private set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600022B RID: 555 RVA: 0x00005E84 File Offset: 0x00004084
		// (set) Token: 0x0600022C RID: 556 RVA: 0x00005E8C File Offset: 0x0000408C
		public int CurrentCharge { get; private set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600022D RID: 557 RVA: 0x00005E95 File Offset: 0x00004095
		// (set) Token: 0x0600022E RID: 558 RVA: 0x00005E9D File Offset: 0x0000409D
		public int ChargeRate { get; private set; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600022F RID: 559 RVA: 0x00005EA6 File Offset: 0x000040A6
		// (set) Token: 0x06000230 RID: 560 RVA: 0x00005EAE File Offset: 0x000040AE
		public TimeSpan EstimatedTimeRemaining { get; private set; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000231 RID: 561 RVA: 0x00005EB7 File Offset: 0x000040B7
		// (set) Token: 0x06000232 RID: 562 RVA: 0x00005EBF File Offset: 0x000040BF
		public int SuggestedCriticalBatteryCharge { get; private set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000233 RID: 563 RVA: 0x00005EC8 File Offset: 0x000040C8
		// (set) Token: 0x06000234 RID: 564 RVA: 0x00005ED0 File Offset: 0x000040D0
		public int SuggestedBatteryWarningCharge { get; private set; }

		// Token: 0x06000235 RID: 565 RVA: 0x00005EDC File Offset: 0x000040DC
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, LocalizedMessages.BatteryStateStringRepresentation, new object[]
			{
				Environment.NewLine,
				this.ACOnline,
				this.MaxCharge,
				this.CurrentCharge,
				this.ChargeRate,
				this.EstimatedTimeRemaining,
				this.SuggestedCriticalBatteryCharge,
				this.SuggestedBatteryWarningCharge
			});
		}
	}
}
