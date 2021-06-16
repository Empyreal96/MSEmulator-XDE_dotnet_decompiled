using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Management.Automation.Internal
{
	// Token: 0x0200085F RID: 2143
	internal static class WinSQMWrapper
	{
		// Token: 0x0600526E RID: 21102 RVA: 0x001B7D78 File Offset: 0x001B5F78
		public static bool IsWinSqmOptedIn()
		{
			try
			{
				return WinSQMWrapper.WinSqmIsOptedIn();
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
			return false;
		}

		// Token: 0x0600526F RID: 21103 RVA: 0x001B7DAC File Offset: 0x001B5FAC
		public static void WinSqmIncrement(uint dataPointID, uint incAmount)
		{
			WinSQMWrapper.EventDescriptor eventDescriptor = new WinSQMWrapper.EventDescriptor(6, 0, 0, 4, 2, 0, 2251799813685248UL);
			WinSQMWrapper.FireWinSQMEvent(eventDescriptor, dataPointID, incAmount);
		}

		// Token: 0x06005270 RID: 21104 RVA: 0x001B7DD8 File Offset: 0x001B5FD8
		public static void WinSqmIncrement(Dictionary<uint, uint> dataToWrite)
		{
			WinSQMWrapper.EventDescriptor eventDescriptor = new WinSQMWrapper.EventDescriptor(6, 0, 0, 4, 2, 0, 2251799813685248UL);
			WinSQMWrapper.FireWinSQMEvent(eventDescriptor, dataToWrite);
		}

		// Token: 0x06005271 RID: 21105 RVA: 0x001B7E04 File Offset: 0x001B6004
		public static void WinSqmSet(uint dataPointID, uint dataPointValue)
		{
			WinSQMWrapper.EventDescriptor eventDescriptor = new WinSQMWrapper.EventDescriptor(5, 0, 0, 4, 0, 0, 2251799813685248UL);
			WinSQMWrapper.FireWinSQMEvent(eventDescriptor, dataPointID, dataPointValue);
		}

		// Token: 0x06005272 RID: 21106 RVA: 0x001B7E30 File Offset: 0x001B6030
		public static void WinSqmAddToStream(uint dataPointID, string stringData)
		{
			WinSQMWrapper.SqmStreamEntry sqmStreamEntry = WinSQMWrapper.SqmStreamEntry.CreateStringSqmStreamEntry(stringData);
			WinSQMWrapper.SqmStreamEntry[] array = new WinSQMWrapper.SqmStreamEntry[]
			{
				sqmStreamEntry
			};
			WinSQMWrapper.WinSqmAddToStream(WinSQMWrapper.HGLOBALSESSION, dataPointID, array.Length, array);
		}

		// Token: 0x06005273 RID: 21107 RVA: 0x001B7E6C File Offset: 0x001B606C
		public static void WinSqmAddToStream(uint dataPointID, string stringData, uint numericalData)
		{
			WinSQMWrapper.SqmStreamEntry sqmStreamEntry = WinSQMWrapper.SqmStreamEntry.CreateStringSqmStreamEntry(stringData);
			WinSQMWrapper.SqmStreamEntry sqmStreamEntry2 = WinSQMWrapper.SqmStreamEntry.CreateStringSqmStreamEntry(numericalData.ToString(CultureInfo.InvariantCulture));
			WinSQMWrapper.SqmStreamEntry[] array = new WinSQMWrapper.SqmStreamEntry[]
			{
				sqmStreamEntry,
				sqmStreamEntry2
			};
			WinSQMWrapper.WinSqmAddToStream(WinSQMWrapper.HGLOBALSESSION, dataPointID, array.Length, array);
		}

		// Token: 0x06005274 RID: 21108 RVA: 0x001B7EC4 File Offset: 0x001B60C4
		private static void FireWinSQMEvent(WinSQMWrapper.EventDescriptor eventDescriptor, Dictionary<uint, uint> dataToWrite)
		{
			Guid empty = Guid.Empty;
			if (!WinSQMWrapper.WinSqmEventEnabled(ref eventDescriptor, ref empty))
			{
				return;
			}
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(WinSQMWrapper.WINDOWS_SQM_GLOBALSESSION));
			IntPtr intPtr2 = Marshal.AllocHGlobal(4);
			IntPtr intPtr3 = Marshal.AllocHGlobal(4);
			try
			{
				Marshal.StructureToPtr(WinSQMWrapper.WINDOWS_SQM_GLOBALSESSION, intPtr, true);
				foreach (uint num in dataToWrite.Keys)
				{
					uint num2 = dataToWrite[num];
					Marshal.StructureToPtr(num, intPtr2, true);
					Marshal.StructureToPtr(num2, intPtr3, true);
					WinSQMWrapper.FireWinSQMEvent(eventDescriptor, intPtr, intPtr2, intPtr3);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
				Marshal.FreeHGlobal(intPtr2);
				Marshal.FreeHGlobal(intPtr3);
			}
		}

		// Token: 0x06005275 RID: 21109 RVA: 0x001B7FA8 File Offset: 0x001B61A8
		private static void FireWinSQMEvent(WinSQMWrapper.EventDescriptor eventDescriptor, uint dataPointID, uint dataPointValue)
		{
			Guid empty = Guid.Empty;
			if (!WinSQMWrapper.WinSqmEventEnabled(ref eventDescriptor, ref empty))
			{
				return;
			}
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(WinSQMWrapper.WINDOWS_SQM_GLOBALSESSION));
			IntPtr intPtr2 = Marshal.AllocHGlobal(4);
			IntPtr intPtr3 = Marshal.AllocHGlobal(4);
			try
			{
				Marshal.StructureToPtr(WinSQMWrapper.WINDOWS_SQM_GLOBALSESSION, intPtr, true);
				Marshal.StructureToPtr(dataPointID, intPtr2, true);
				Marshal.StructureToPtr(dataPointValue, intPtr3, true);
				WinSQMWrapper.FireWinSQMEvent(eventDescriptor, intPtr, intPtr2, intPtr3);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
				Marshal.FreeHGlobal(intPtr2);
				Marshal.FreeHGlobal(intPtr3);
			}
		}

		// Token: 0x06005276 RID: 21110 RVA: 0x001B8044 File Offset: 0x001B6244
		private static void FireWinSQMEvent(WinSQMWrapper.EventDescriptor eventDescriptor, IntPtr sessionHandle, IntPtr dataPointIDHandle, IntPtr dataValueHandle)
		{
			WinSQMWrapper.EventDataDescriptor eventDataDescriptor = new WinSQMWrapper.EventDataDescriptor(sessionHandle, Marshal.SizeOf(WinSQMWrapper.WINDOWS_SQM_GLOBALSESSION));
			WinSQMWrapper.EventDataDescriptor eventDataDescriptor2 = new WinSQMWrapper.EventDataDescriptor(dataPointIDHandle, 4);
			WinSQMWrapper.EventDataDescriptor eventDataDescriptor3 = new WinSQMWrapper.EventDataDescriptor(dataValueHandle, 4);
			WinSQMWrapper.EventDataDescriptor[] array = new WinSQMWrapper.EventDataDescriptor[]
			{
				eventDataDescriptor,
				eventDataDescriptor2,
				eventDataDescriptor3
			};
			WinSQMWrapper.WinSqmEventWrite(ref eventDescriptor, array.Length, array);
		}

		// Token: 0x06005277 RID: 21111
		[DllImport("ntdll.dll")]
		private static extern bool WinSqmIsOptedIn();

		// Token: 0x06005278 RID: 21112
		[DllImport("ntdll.dll")]
		private static extern bool WinSqmEventEnabled(ref WinSQMWrapper.EventDescriptor eventDescriptor, ref Guid guid);

		// Token: 0x06005279 RID: 21113
		[DllImport("ntdll.dll")]
		private static extern uint WinSqmEventWrite(ref WinSQMWrapper.EventDescriptor eventDescriptor, int userDataCount, WinSQMWrapper.EventDataDescriptor[] userData);

		// Token: 0x0600527A RID: 21114
		[DllImport("ntdll.dll")]
		private static extern void WinSqmAddToStream(IntPtr sessionGuid, uint dataPointID, int sqmStreamEntries, WinSQMWrapper.SqmStreamEntry[] streamEntries);

		// Token: 0x04002A45 RID: 10821
		private static Guid WINDOWS_SQM_GLOBALSESSION = new Guid("{ 0x95baba28, 0xed26, 0x49c9, { 0xb7, 0x4f, 0x93, 0xb1, 0x70, 0xe1, 0xb8, 0x49 }}");

		// Token: 0x04002A46 RID: 10822
		private static readonly IntPtr HGLOBALSESSION = IntPtr.Zero;

		// Token: 0x02000860 RID: 2144
		[StructLayout(LayoutKind.Explicit, Size = 16)]
		private struct EventDescriptor
		{
			// Token: 0x0600527C RID: 21116 RVA: 0x001B80D6 File Offset: 0x001B62D6
			internal EventDescriptor(ushort eventId, byte eventVersion, byte eventChannel, byte eventLevel, byte eventOpcode, ushort eventTask, ulong eventKeywords)
			{
				this.id = eventId;
				this.version = eventVersion;
				this.channel = eventChannel;
				this.level = eventLevel;
				this.opcode = eventOpcode;
				this.task = eventTask;
				this.keywords = eventKeywords;
			}

			// Token: 0x04002A47 RID: 10823
			[FieldOffset(0)]
			private ushort id;

			// Token: 0x04002A48 RID: 10824
			[FieldOffset(2)]
			private byte version;

			// Token: 0x04002A49 RID: 10825
			[FieldOffset(3)]
			private byte channel;

			// Token: 0x04002A4A RID: 10826
			[FieldOffset(4)]
			private byte level;

			// Token: 0x04002A4B RID: 10827
			[FieldOffset(5)]
			private byte opcode;

			// Token: 0x04002A4C RID: 10828
			[FieldOffset(6)]
			private ushort task;

			// Token: 0x04002A4D RID: 10829
			[FieldOffset(8)]
			private ulong keywords;
		}

		// Token: 0x02000861 RID: 2145
		[StructLayout(LayoutKind.Explicit, Size = 16)]
		private struct EventDataDescriptor
		{
			// Token: 0x0600527D RID: 21117 RVA: 0x001B810D File Offset: 0x001B630D
			internal EventDataDescriptor(IntPtr dp, int sz)
			{
				this.reserved = 0;
				this.size = sz;
				this.dataPointer = dp;
			}

			// Token: 0x04002A4E RID: 10830
			[FieldOffset(0)]
			private IntPtr dataPointer;

			// Token: 0x04002A4F RID: 10831
			[FieldOffset(8)]
			private int size;

			// Token: 0x04002A50 RID: 10832
			[FieldOffset(12)]
			private int reserved;
		}

		// Token: 0x02000862 RID: 2146
		private struct SqmStreamEntry
		{
			// Token: 0x0600527E RID: 21118 RVA: 0x001B8124 File Offset: 0x001B6324
			internal static WinSQMWrapper.SqmStreamEntry CreateStringSqmStreamEntry(string value)
			{
				return new WinSQMWrapper.SqmStreamEntry
				{
					type = 2U,
					stringValue = value
				};
			}

			// Token: 0x04002A51 RID: 10833
			private uint type;

			// Token: 0x04002A52 RID: 10834
			[MarshalAs(UnmanagedType.LPWStr)]
			private string stringValue;
		}
	}
}
