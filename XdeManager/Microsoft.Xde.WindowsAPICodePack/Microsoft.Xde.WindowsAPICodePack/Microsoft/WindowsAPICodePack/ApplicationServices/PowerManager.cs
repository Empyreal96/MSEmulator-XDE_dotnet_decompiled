using System;
using System.ComponentModel;
using System.Security.Permissions;
using Microsoft.WindowsAPICodePack.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	// Token: 0x02000046 RID: 70
	public static class PowerManager
	{
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000241 RID: 577 RVA: 0x0000634C File Offset: 0x0000454C
		// (remove) Token: 0x06000242 RID: 578 RVA: 0x00006359 File Offset: 0x00004559
		public static event EventHandler PowerPersonalityChanged
		{
			add
			{
				MessageManager.RegisterPowerEvent(EventManager.PowerPersonalityChange, value);
			}
			remove
			{
				CoreHelpers.ThrowIfNotVista();
				MessageManager.UnregisterPowerEvent(EventManager.PowerPersonalityChange, value);
			}
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000243 RID: 579 RVA: 0x0000636B File Offset: 0x0000456B
		// (remove) Token: 0x06000244 RID: 580 RVA: 0x0000637D File Offset: 0x0000457D
		public static event EventHandler PowerSourceChanged
		{
			add
			{
				CoreHelpers.ThrowIfNotVista();
				MessageManager.RegisterPowerEvent(EventManager.PowerSourceChange, value);
			}
			remove
			{
				CoreHelpers.ThrowIfNotVista();
				MessageManager.UnregisterPowerEvent(EventManager.PowerSourceChange, value);
			}
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000245 RID: 581 RVA: 0x0000638F File Offset: 0x0000458F
		// (remove) Token: 0x06000246 RID: 582 RVA: 0x000063A1 File Offset: 0x000045A1
		public static event EventHandler BatteryLifePercentChanged
		{
			add
			{
				CoreHelpers.ThrowIfNotVista();
				MessageManager.RegisterPowerEvent(EventManager.BatteryCapacityChange, value);
			}
			remove
			{
				CoreHelpers.ThrowIfNotVista();
				MessageManager.UnregisterPowerEvent(EventManager.BatteryCapacityChange, value);
			}
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000247 RID: 583 RVA: 0x000063B3 File Offset: 0x000045B3
		// (remove) Token: 0x06000248 RID: 584 RVA: 0x000063C5 File Offset: 0x000045C5
		public static event EventHandler IsMonitorOnChanged
		{
			add
			{
				CoreHelpers.ThrowIfNotVista();
				MessageManager.RegisterPowerEvent(EventManager.MonitorPowerStatus, value);
			}
			remove
			{
				CoreHelpers.ThrowIfNotVista();
				MessageManager.UnregisterPowerEvent(EventManager.MonitorPowerStatus, value);
			}
		}

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000249 RID: 585 RVA: 0x000063D7 File Offset: 0x000045D7
		// (remove) Token: 0x0600024A RID: 586 RVA: 0x000063E9 File Offset: 0x000045E9
		public static event EventHandler SystemBusyChanged
		{
			add
			{
				CoreHelpers.ThrowIfNotVista();
				MessageManager.RegisterPowerEvent(EventManager.BackgroundTaskNotification, value);
			}
			remove
			{
				CoreHelpers.ThrowIfNotVista();
				MessageManager.UnregisterPowerEvent(EventManager.BackgroundTaskNotification, value);
			}
		}

		// Token: 0x0600024B RID: 587 RVA: 0x000063FB File Offset: 0x000045FB
		public static BatteryState GetCurrentBatteryState()
		{
			CoreHelpers.ThrowIfNotXP();
			return new BatteryState();
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600024C RID: 588 RVA: 0x00006407 File Offset: 0x00004607
		// (set) Token: 0x0600024D RID: 589 RVA: 0x00006413 File Offset: 0x00004613
		public static bool MonitorRequired
		{
			get
			{
				CoreHelpers.ThrowIfNotXP();
				return PowerManager.monitorRequired;
			}
			[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
			set
			{
				CoreHelpers.ThrowIfNotXP();
				if (value)
				{
					PowerManager.SetThreadExecutionState(ExecutionStates.DisplayRequired | ExecutionStates.Continuous);
				}
				else
				{
					PowerManager.SetThreadExecutionState(ExecutionStates.Continuous);
				}
				PowerManager.monitorRequired = value;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600024E RID: 590 RVA: 0x00006439 File Offset: 0x00004639
		// (set) Token: 0x0600024F RID: 591 RVA: 0x00006445 File Offset: 0x00004645
		public static bool RequestBlockSleep
		{
			get
			{
				CoreHelpers.ThrowIfNotXP();
				return PowerManager.requestBlockSleep;
			}
			[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
			set
			{
				CoreHelpers.ThrowIfNotXP();
				if (value)
				{
					PowerManager.SetThreadExecutionState(ExecutionStates.SystemRequired | ExecutionStates.Continuous);
				}
				else
				{
					PowerManager.SetThreadExecutionState(ExecutionStates.Continuous);
				}
				PowerManager.requestBlockSleep = value;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000250 RID: 592 RVA: 0x0000646B File Offset: 0x0000466B
		public static bool IsBatteryPresent
		{
			get
			{
				CoreHelpers.ThrowIfNotXP();
				return Power.GetSystemBatteryState().BatteryPresent;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0000647C File Offset: 0x0000467C
		public static bool IsBatteryShortTerm
		{
			get
			{
				CoreHelpers.ThrowIfNotXP();
				return Power.GetSystemPowerCapabilities().BatteriesAreShortTerm;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000252 RID: 594 RVA: 0x00006490 File Offset: 0x00004690
		public static bool IsUpsPresent
		{
			get
			{
				CoreHelpers.ThrowIfNotXP();
				PowerManagementNativeMethods.SystemPowerCapabilities systemPowerCapabilities = Power.GetSystemPowerCapabilities();
				return systemPowerCapabilities.BatteriesAreShortTerm && systemPowerCapabilities.SystemBatteriesPresent;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000253 RID: 595 RVA: 0x000064B8 File Offset: 0x000046B8
		public static PowerPersonality PowerPersonality
		{
			get
			{
				Guid guid;
				PowerManagementNativeMethods.PowerGetActiveScheme(IntPtr.Zero, out guid);
				PowerPersonality result;
				try
				{
					result = PowerPersonalityGuids.GuidToEnum(guid);
				}
				finally
				{
					CoreNativeMethods.LocalFree(ref guid);
				}
				return result;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000254 RID: 596 RVA: 0x000064F4 File Offset: 0x000046F4
		public static int BatteryLifePercent
		{
			get
			{
				CoreHelpers.ThrowIfNotVista();
				if (!Power.GetSystemBatteryState().BatteryPresent)
				{
					throw new InvalidOperationException(LocalizedMessages.PowerManagerBatteryNotPresent);
				}
				PowerManagementNativeMethods.SystemBatteryState systemBatteryState = Power.GetSystemBatteryState();
				return (int)Math.Round(systemBatteryState.RemainingCapacity / systemBatteryState.MaxCapacity * 100.0, 0);
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000255 RID: 597 RVA: 0x00006548 File Offset: 0x00004748
		// (set) Token: 0x06000256 RID: 598 RVA: 0x000065CC File Offset: 0x000047CC
		public static bool IsMonitorOn
		{
			get
			{
				CoreHelpers.ThrowIfNotVista();
				object obj = PowerManager.monitoronlock;
				lock (obj)
				{
					if (PowerManager.isMonitorOn == null)
					{
						PowerManager.IsMonitorOnChanged += delegate(object sender, EventArgs args)
						{
						};
						EventManager.monitorOnReset.WaitOne();
					}
				}
				return PowerManager.isMonitorOn.Value;
			}
			internal set
			{
				PowerManager.isMonitorOn = new bool?(value);
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000257 RID: 599 RVA: 0x000065D9 File Offset: 0x000047D9
		public static PowerSource PowerSource
		{
			get
			{
				CoreHelpers.ThrowIfNotVista();
				if (PowerManager.IsUpsPresent)
				{
					return PowerSource.Ups;
				}
				if (!PowerManager.IsBatteryPresent || PowerManager.GetCurrentBatteryState().ACOnline)
				{
					return PowerSource.AC;
				}
				return PowerSource.Battery;
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x000065FF File Offset: 0x000047FF
		public static void SetThreadExecutionState(ExecutionStates executionStateOptions)
		{
			if (PowerManagementNativeMethods.SetThreadExecutionState(executionStateOptions) == ExecutionStates.None)
			{
				throw new Win32Exception(LocalizedMessages.PowerExecutionStateFailed);
			}
		}

		// Token: 0x040001CC RID: 460
		private static bool? isMonitorOn;

		// Token: 0x040001CD RID: 461
		private static bool monitorRequired;

		// Token: 0x040001CE RID: 462
		private static bool requestBlockSleep;

		// Token: 0x040001CF RID: 463
		private static readonly object monitoronlock = new object();
	}
}
