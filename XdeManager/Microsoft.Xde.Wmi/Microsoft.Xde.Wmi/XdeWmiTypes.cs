using System;

namespace Microsoft.Xde.Wmi
{
	// Token: 0x02000015 RID: 21
	public static class XdeWmiTypes
	{
		// Token: 0x06000184 RID: 388 RVA: 0x00008630 File Offset: 0x00006830
		public static string Name(this XdeWmiTypes.ResourceSubType subType)
		{
			switch (subType)
			{
			case XdeWmiTypes.ResourceSubType.DisketteDrive:
				return "Microsoft:Hyper-V:Synthetic Diskette Drive";
			case XdeWmiTypes.ResourceSubType.ParallelSCSIHBA:
				return "Microsoft:Hyper-V:Synthetic SCSI Controller";
			case XdeWmiTypes.ResourceSubType.IDEController:
				return "Microsoft:Hyper-V:Emulated IDE Controller";
			case XdeWmiTypes.ResourceSubType.DiskSynthetic:
				return "Microsoft:Hyper-V:Synthetic Disk Drive";
			case XdeWmiTypes.ResourceSubType.DiskPhysical:
				return "Microsoft:Hyper-V:Physical Disk Drive";
			case XdeWmiTypes.ResourceSubType.DVDPhysical:
				return "Microsoft:Hyper-V:Physical DVD Drive";
			case XdeWmiTypes.ResourceSubType.DVDSynthetic:
				return "Microsoft:Hyper-V:Synthetic DVD Drive";
			case XdeWmiTypes.ResourceSubType.CDROMPhysical:
				return "Microsoft:Hyper-V:Physical CD Drive";
			case XdeWmiTypes.ResourceSubType.CDROMSynthetic:
				return "Microsoft:Hyper-V:Synthetic CD Drive";
			case XdeWmiTypes.ResourceSubType.EthernetSynthetic:
				return "Microsoft:Hyper-V:Synthetic Ethernet Port";
			case XdeWmiTypes.ResourceSubType.EthernetEmulated:
				return "Microsoft:Hyper-V:Emulated Ethernet Port";
			case XdeWmiTypes.ResourceSubType.DVDLogical:
				return "Microsoft:Hyper-V:Virtual CD/DVD Disk";
			case XdeWmiTypes.ResourceSubType.ISOImage:
				return "Microsoft:Hyper-V:ISO Image";
			case XdeWmiTypes.ResourceSubType.VHD:
				return "Microsoft:Hyper-V:Virtual Hard Disk";
			case XdeWmiTypes.ResourceSubType.DVD:
				return "Microsoft:Hyper-V:Virtual DVD Disk";
			case XdeWmiTypes.ResourceSubType.VFD:
				return "Microsoft:Hyper-V:Virtual Floppy Disk";
			case XdeWmiTypes.ResourceSubType.VideoSynthetic:
				return "Microsoft:Hyper-V:Synthetic Display Controller";
			case XdeWmiTypes.ResourceSubType.EthernetConnection:
				return "Microsoft:Hyper-V:Ethernet Connection";
			case XdeWmiTypes.ResourceSubType.RemoteFx:
				return "Microsoft:Hyper-V:Synthetic 3D Display Controller";
			case XdeWmiTypes.ResourceSubType.S3Controller:
				return "Microsoft:Hyper-V:S3 Display Controller";
			}
			return null;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00008712 File Offset: 0x00006912
		public static string Name(this XdeWmiTypes.SerialPort serialPort)
		{
			switch (serialPort)
			{
			case XdeWmiTypes.SerialPort.COM1:
				return "COM 1";
			case XdeWmiTypes.SerialPort.COM2:
				return "COM 2";
			}
			return null;
		}

		// Token: 0x02000026 RID: 38
		public enum ResourceType
		{
			// Token: 0x04000098 RID: 152
			Other = 1,
			// Token: 0x04000099 RID: 153
			ComputerSystem,
			// Token: 0x0400009A RID: 154
			Processor,
			// Token: 0x0400009B RID: 155
			Memory,
			// Token: 0x0400009C RID: 156
			IDEController,
			// Token: 0x0400009D RID: 157
			ParallelSCSIHBA,
			// Token: 0x0400009E RID: 158
			FCHBA,
			// Token: 0x0400009F RID: 159
			iSCSIHBA,
			// Token: 0x040000A0 RID: 160
			IBHCA,
			// Token: 0x040000A1 RID: 161
			EthernetAdapter,
			// Token: 0x040000A2 RID: 162
			OtherNetworkAdapter,
			// Token: 0x040000A3 RID: 163
			IOSlot,
			// Token: 0x040000A4 RID: 164
			IODevice,
			// Token: 0x040000A5 RID: 165
			FloppyDrive,
			// Token: 0x040000A6 RID: 166
			CDDrive,
			// Token: 0x040000A7 RID: 167
			DVDdrive,
			// Token: 0x040000A8 RID: 168
			Serialport = 21,
			// Token: 0x040000A9 RID: 169
			Parallelport = 18,
			// Token: 0x040000AA RID: 170
			USBController,
			// Token: 0x040000AB RID: 171
			GraphicsController,
			// Token: 0x040000AC RID: 172
			StorageExtent = 17,
			// Token: 0x040000AD RID: 173
			Disk = 22,
			// Token: 0x040000AE RID: 174
			Tape,
			// Token: 0x040000AF RID: 175
			DisplayController,
			// Token: 0x040000B0 RID: 176
			FirewireController,
			// Token: 0x040000B1 RID: 177
			PartitionableUnit,
			// Token: 0x040000B2 RID: 178
			BasePartitionableUnit,
			// Token: 0x040000B3 RID: 179
			PowerSupply,
			// Token: 0x040000B4 RID: 180
			CoolingDevice,
			// Token: 0x040000B5 RID: 181
			VirtualHardDisk = 31,
			// Token: 0x040000B6 RID: 182
			EthernetPort = 33
		}

		// Token: 0x02000027 RID: 39
		public enum ResourceSubType
		{
			// Token: 0x040000B8 RID: 184
			DisketteController,
			// Token: 0x040000B9 RID: 185
			DisketteDrive,
			// Token: 0x040000BA RID: 186
			ParallelSCSIHBA,
			// Token: 0x040000BB RID: 187
			IDEController,
			// Token: 0x040000BC RID: 188
			DiskSynthetic,
			// Token: 0x040000BD RID: 189
			DiskPhysical,
			// Token: 0x040000BE RID: 190
			DVDPhysical,
			// Token: 0x040000BF RID: 191
			DVDSynthetic,
			// Token: 0x040000C0 RID: 192
			CDROMPhysical,
			// Token: 0x040000C1 RID: 193
			CDROMSynthetic,
			// Token: 0x040000C2 RID: 194
			EthernetSynthetic,
			// Token: 0x040000C3 RID: 195
			EthernetEmulated,
			// Token: 0x040000C4 RID: 196
			DVDLogical,
			// Token: 0x040000C5 RID: 197
			ISOImage,
			// Token: 0x040000C6 RID: 198
			VHD,
			// Token: 0x040000C7 RID: 199
			DVD,
			// Token: 0x040000C8 RID: 200
			VFD,
			// Token: 0x040000C9 RID: 201
			VideoSynthetic,
			// Token: 0x040000CA RID: 202
			EthernetConnection,
			// Token: 0x040000CB RID: 203
			RemoteFx,
			// Token: 0x040000CC RID: 204
			S3Controller
		}

		// Token: 0x02000028 RID: 40
		public enum SerialPort
		{
			// Token: 0x040000CE RID: 206
			None,
			// Token: 0x040000CF RID: 207
			COM1,
			// Token: 0x040000D0 RID: 208
			COM2
		}

		// Token: 0x02000029 RID: 41
		public enum AutomaticStartupAction
		{
			// Token: 0x040000D2 RID: 210
			None = 2,
			// Token: 0x040000D3 RID: 211
			RestartIfPreviouslyRunning,
			// Token: 0x040000D4 RID: 212
			AlwaysStartup
		}

		// Token: 0x0200002A RID: 42
		public enum AutomaticShutdownAction
		{
			// Token: 0x040000D6 RID: 214
			TurnOff = 2,
			// Token: 0x040000D7 RID: 215
			SaveState,
			// Token: 0x040000D8 RID: 216
			ShutDown
		}
	}
}
