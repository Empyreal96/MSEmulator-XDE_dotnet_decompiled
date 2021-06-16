using System;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	// Token: 0x0200003F RID: 63
	internal static class PowerManagementNativeMethods
	{
		// Token: 0x06000221 RID: 545
		[DllImport("powrprof.dll")]
		internal static extern uint CallNtPowerInformation(PowerManagementNativeMethods.PowerInformationLevel informationLevel, IntPtr inputBuffer, uint inputBufferSize, out PowerManagementNativeMethods.SystemPowerCapabilities outputBuffer, uint outputBufferSize);

		// Token: 0x06000222 RID: 546
		[DllImport("powrprof.dll")]
		internal static extern uint CallNtPowerInformation(PowerManagementNativeMethods.PowerInformationLevel informationLevel, IntPtr inputBuffer, uint inputBufferSize, out PowerManagementNativeMethods.SystemBatteryState outputBuffer, uint outputBufferSize);

		// Token: 0x06000223 RID: 547
		[DllImport("powrprof.dll")]
		internal static extern void PowerGetActiveScheme(IntPtr rootPowerKey, [MarshalAs(UnmanagedType.LPStruct)] out Guid activePolicy);

		// Token: 0x06000224 RID: 548
		[DllImport("User32", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		internal static extern int RegisterPowerSettingNotification(IntPtr hRecipient, ref Guid PowerSettingGuid, int Flags);

		// Token: 0x06000225 RID: 549
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern ExecutionStates SetThreadExecutionState(ExecutionStates esFlags);

		// Token: 0x040001AA RID: 426
		internal const uint PowerBroadcastMessage = 536U;

		// Token: 0x040001AB RID: 427
		internal const uint PowerSettingChangeMessage = 32787U;

		// Token: 0x040001AC RID: 428
		internal const uint ScreenSaverSetActive = 17U;

		// Token: 0x040001AD RID: 429
		internal const uint UpdateInFile = 1U;

		// Token: 0x040001AE RID: 430
		internal const uint SendChange = 2U;

		// Token: 0x02000061 RID: 97
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		public struct PowerBroadcastSetting
		{
			// Token: 0x04000267 RID: 615
			public Guid PowerSetting;

			// Token: 0x04000268 RID: 616
			public int DataLength;
		}

		// Token: 0x02000062 RID: 98
		public struct SystemPowerCapabilities
		{
			// Token: 0x04000269 RID: 617
			[MarshalAs(UnmanagedType.I1)]
			public bool PowerButtonPresent;

			// Token: 0x0400026A RID: 618
			[MarshalAs(UnmanagedType.I1)]
			public bool SleepButtonPresent;

			// Token: 0x0400026B RID: 619
			[MarshalAs(UnmanagedType.I1)]
			public bool LidPresent;

			// Token: 0x0400026C RID: 620
			[MarshalAs(UnmanagedType.I1)]
			public bool SystemS1;

			// Token: 0x0400026D RID: 621
			[MarshalAs(UnmanagedType.I1)]
			public bool SystemS2;

			// Token: 0x0400026E RID: 622
			[MarshalAs(UnmanagedType.I1)]
			public bool SystemS3;

			// Token: 0x0400026F RID: 623
			[MarshalAs(UnmanagedType.I1)]
			public bool SystemS4;

			// Token: 0x04000270 RID: 624
			[MarshalAs(UnmanagedType.I1)]
			public bool SystemS5;

			// Token: 0x04000271 RID: 625
			[MarshalAs(UnmanagedType.I1)]
			public bool HiberFilePresent;

			// Token: 0x04000272 RID: 626
			[MarshalAs(UnmanagedType.I1)]
			public bool FullWake;

			// Token: 0x04000273 RID: 627
			[MarshalAs(UnmanagedType.I1)]
			public bool VideoDimPresent;

			// Token: 0x04000274 RID: 628
			[MarshalAs(UnmanagedType.I1)]
			public bool ApmPresent;

			// Token: 0x04000275 RID: 629
			[MarshalAs(UnmanagedType.I1)]
			public bool UpsPresent;

			// Token: 0x04000276 RID: 630
			[MarshalAs(UnmanagedType.I1)]
			public bool ThermalControl;

			// Token: 0x04000277 RID: 631
			[MarshalAs(UnmanagedType.I1)]
			public bool ProcessorThrottle;

			// Token: 0x04000278 RID: 632
			public byte ProcessorMinimumThrottle;

			// Token: 0x04000279 RID: 633
			public byte ProcessorMaximumThrottle;

			// Token: 0x0400027A RID: 634
			[MarshalAs(UnmanagedType.I1)]
			public bool FastSystemS4;

			// Token: 0x0400027B RID: 635
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] spare2;

			// Token: 0x0400027C RID: 636
			[MarshalAs(UnmanagedType.I1)]
			public bool DiskSpinDown;

			// Token: 0x0400027D RID: 637
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			public byte[] spare3;

			// Token: 0x0400027E RID: 638
			[MarshalAs(UnmanagedType.I1)]
			public bool SystemBatteriesPresent;

			// Token: 0x0400027F RID: 639
			[MarshalAs(UnmanagedType.I1)]
			public bool BatteriesAreShortTerm;

			// Token: 0x04000280 RID: 640
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public PowerManagementNativeMethods.BatteryReportingScale[] BatteryScale;

			// Token: 0x04000281 RID: 641
			public PowerManagementNativeMethods.SystemPowerState AcOnlineWake;

			// Token: 0x04000282 RID: 642
			public PowerManagementNativeMethods.SystemPowerState SoftLidWake;

			// Token: 0x04000283 RID: 643
			public PowerManagementNativeMethods.SystemPowerState RtcWake;

			// Token: 0x04000284 RID: 644
			public PowerManagementNativeMethods.SystemPowerState MinimumDeviceWakeState;

			// Token: 0x04000285 RID: 645
			public PowerManagementNativeMethods.SystemPowerState DefaultLowLatencyWake;
		}

		// Token: 0x02000063 RID: 99
		public enum PowerInformationLevel
		{
			// Token: 0x04000287 RID: 647
			SystemPowerPolicyAc,
			// Token: 0x04000288 RID: 648
			SystemPowerPolicyDc,
			// Token: 0x04000289 RID: 649
			VerifySystemPolicyAc,
			// Token: 0x0400028A RID: 650
			VerifySystemPolicyDc,
			// Token: 0x0400028B RID: 651
			SystemPowerCapabilities,
			// Token: 0x0400028C RID: 652
			SystemBatteryState,
			// Token: 0x0400028D RID: 653
			SystemPowerStateHandler,
			// Token: 0x0400028E RID: 654
			ProcessorStateHandler,
			// Token: 0x0400028F RID: 655
			SystemPowerPolicyCurrent,
			// Token: 0x04000290 RID: 656
			AdministratorPowerPolicy,
			// Token: 0x04000291 RID: 657
			SystemReserveHiberFile,
			// Token: 0x04000292 RID: 658
			ProcessorInformation,
			// Token: 0x04000293 RID: 659
			SystemPowerInformation,
			// Token: 0x04000294 RID: 660
			ProcessorStateHandler2,
			// Token: 0x04000295 RID: 661
			LastWakeTime,
			// Token: 0x04000296 RID: 662
			LastSleepTime,
			// Token: 0x04000297 RID: 663
			SystemExecutionState,
			// Token: 0x04000298 RID: 664
			SystemPowerStateNotifyHandler,
			// Token: 0x04000299 RID: 665
			ProcessorPowerPolicyAc,
			// Token: 0x0400029A RID: 666
			ProcessorPowerPolicyDc,
			// Token: 0x0400029B RID: 667
			VerifyProcessorPowerPolicyAc,
			// Token: 0x0400029C RID: 668
			VerifyProcessorPowerPolicyDc,
			// Token: 0x0400029D RID: 669
			ProcessorPowerPolicyCurrent,
			// Token: 0x0400029E RID: 670
			SystemPowerStateLogging,
			// Token: 0x0400029F RID: 671
			SystemPowerLoggingEntry,
			// Token: 0x040002A0 RID: 672
			SetPowerSettingValue,
			// Token: 0x040002A1 RID: 673
			NotifyUserPowerSetting,
			// Token: 0x040002A2 RID: 674
			PowerInformationLevelUnused0,
			// Token: 0x040002A3 RID: 675
			PowerInformationLevelUnused1,
			// Token: 0x040002A4 RID: 676
			SystemVideoState,
			// Token: 0x040002A5 RID: 677
			TraceApplicationPowerMessage,
			// Token: 0x040002A6 RID: 678
			TraceApplicationPowerMessageEnd,
			// Token: 0x040002A7 RID: 679
			ProcessorPerfStates,
			// Token: 0x040002A8 RID: 680
			ProcessorIdleStates,
			// Token: 0x040002A9 RID: 681
			ProcessorCap,
			// Token: 0x040002AA RID: 682
			SystemWakeSource,
			// Token: 0x040002AB RID: 683
			SystemHiberFileInformation,
			// Token: 0x040002AC RID: 684
			TraceServicePowerMessage,
			// Token: 0x040002AD RID: 685
			ProcessorLoad,
			// Token: 0x040002AE RID: 686
			PowerShutdownNotification,
			// Token: 0x040002AF RID: 687
			MonitorCapabilities,
			// Token: 0x040002B0 RID: 688
			SessionPowerInit,
			// Token: 0x040002B1 RID: 689
			SessionDisplayState,
			// Token: 0x040002B2 RID: 690
			PowerRequestCreate,
			// Token: 0x040002B3 RID: 691
			PowerRequestAction,
			// Token: 0x040002B4 RID: 692
			GetPowerRequestList,
			// Token: 0x040002B5 RID: 693
			ProcessorInformationEx,
			// Token: 0x040002B6 RID: 694
			NotifyUserModeLegacyPowerEvent,
			// Token: 0x040002B7 RID: 695
			GroupPark,
			// Token: 0x040002B8 RID: 696
			ProcessorIdleDomains,
			// Token: 0x040002B9 RID: 697
			WakeTimerList,
			// Token: 0x040002BA RID: 698
			SystemHiberFileSize,
			// Token: 0x040002BB RID: 699
			PowerInformationLevelMaximum
		}

		// Token: 0x02000064 RID: 100
		public struct BatteryReportingScale
		{
			// Token: 0x040002BC RID: 700
			public uint Granularity;

			// Token: 0x040002BD RID: 701
			public uint Capacity;
		}

		// Token: 0x02000065 RID: 101
		public enum SystemPowerState
		{
			// Token: 0x040002BF RID: 703
			Unspecified,
			// Token: 0x040002C0 RID: 704
			Working,
			// Token: 0x040002C1 RID: 705
			Sleeping1,
			// Token: 0x040002C2 RID: 706
			Sleeping2,
			// Token: 0x040002C3 RID: 707
			Sleeping3,
			// Token: 0x040002C4 RID: 708
			Hibernate,
			// Token: 0x040002C5 RID: 709
			Shutdown,
			// Token: 0x040002C6 RID: 710
			Maximum
		}

		// Token: 0x02000066 RID: 102
		public struct SystemBatteryState
		{
			// Token: 0x040002C7 RID: 711
			[MarshalAs(UnmanagedType.I1)]
			public bool AcOnLine;

			// Token: 0x040002C8 RID: 712
			[MarshalAs(UnmanagedType.I1)]
			public bool BatteryPresent;

			// Token: 0x040002C9 RID: 713
			[MarshalAs(UnmanagedType.I1)]
			public bool Charging;

			// Token: 0x040002CA RID: 714
			[MarshalAs(UnmanagedType.I1)]
			public bool Discharging;

			// Token: 0x040002CB RID: 715
			public byte Spare1;

			// Token: 0x040002CC RID: 716
			public byte Spare2;

			// Token: 0x040002CD RID: 717
			public byte Spare3;

			// Token: 0x040002CE RID: 718
			public byte Spare4;

			// Token: 0x040002CF RID: 719
			public uint MaxCapacity;

			// Token: 0x040002D0 RID: 720
			public uint RemainingCapacity;

			// Token: 0x040002D1 RID: 721
			public uint Rate;

			// Token: 0x040002D2 RID: 722
			public uint EstimatedTime;

			// Token: 0x040002D3 RID: 723
			public uint DefaultAlert1;

			// Token: 0x040002D4 RID: 724
			public uint DefaultAlert2;
		}
	}
}
