using System;
using System.Threading;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	// Token: 0x02000041 RID: 65
	internal static class EventManager
	{
		// Token: 0x06000236 RID: 566 RVA: 0x00005F68 File Offset: 0x00004168
		internal static bool IsMessageCaught(Guid eventGuid)
		{
			bool result = false;
			if (eventGuid == EventManager.BatteryCapacityChange)
			{
				if (!EventManager.batteryLifeCaught)
				{
					EventManager.batteryLifeCaught = true;
					result = true;
				}
			}
			else if (eventGuid == EventManager.MonitorPowerStatus)
			{
				if (!EventManager.monitorOnCaught)
				{
					EventManager.monitorOnCaught = true;
					result = true;
				}
			}
			else if (eventGuid == EventManager.PowerPersonalityChange)
			{
				if (!EventManager.personalityCaught)
				{
					EventManager.personalityCaught = true;
					result = true;
				}
			}
			else if (eventGuid == EventManager.PowerSourceChange && !EventManager.powerSrcCaught)
			{
				EventManager.powerSrcCaught = true;
				result = true;
			}
			return result;
		}

		// Token: 0x040001B6 RID: 438
		internal static AutoResetEvent monitorOnReset = new AutoResetEvent(false);

		// Token: 0x040001B7 RID: 439
		internal static readonly Guid PowerPersonalityChange = new Guid(610108737, 14659, 17442, 176, 37, 19, 167, 132, 246, 121, 183);

		// Token: 0x040001B8 RID: 440
		internal static readonly Guid PowerSourceChange = new Guid(1564383833U, 59861, 19200, 166, 189, byte.MaxValue, 52, byte.MaxValue, 81, 101, 72);

		// Token: 0x040001B9 RID: 441
		internal static readonly Guid BatteryCapacityChange = new Guid(2813165633U, 46170, 19630, 135, 163, 238, 203, 180, 104, 169, 225);

		// Token: 0x040001BA RID: 442
		internal static readonly Guid BackgroundTaskNotification = new Guid(1364996568U, 63284, 5693, 160, 253, 17, 160, 140, 145, 232, 241);

		// Token: 0x040001BB RID: 443
		internal static readonly Guid MonitorPowerStatus = new Guid(41095189, 17680, 17702, 153, 230, 229, 161, 126, 189, 26, 234);

		// Token: 0x040001BC RID: 444
		private static bool personalityCaught;

		// Token: 0x040001BD RID: 445
		private static bool powerSrcCaught;

		// Token: 0x040001BE RID: 446
		private static bool batteryLifeCaught;

		// Token: 0x040001BF RID: 447
		private static bool monitorOnCaught;
	}
}
