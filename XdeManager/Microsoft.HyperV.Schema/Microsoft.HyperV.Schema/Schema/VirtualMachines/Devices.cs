using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Schema.VirtualMachines.Resources;
using HCS.Schema.VirtualMachines.Resources.Network;
using HCS.Schema.VirtualMachines.Resources.Storage;
using HCS.Schema.VirtualMachines.Resources.Vpci;

namespace HCS.Schema.VirtualMachines
{
	// Token: 0x0200000D RID: 13
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Devices
	{
		// Token: 0x0600003D RID: 61 RVA: 0x00002913 File Offset: 0x00000B13
		public static bool IsJsonDefault(Devices val)
		{
			return Devices._default.JsonEquals(val);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002920 File Offset: 0x00000B20
		public bool JsonEquals(object obj)
		{
			Devices graph = obj as Devices;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Devices), new DataContractJsonSerializerSettings
			{
				UseSimpleDictionaryFormat = true
			});
			bool result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (MemoryStream memoryStream2 = new MemoryStream())
				{
					dataContractJsonSerializer.WriteObject(memoryStream, this);
					dataContractJsonSerializer.WriteObject(memoryStream2, graph);
					result = (Encoding.ASCII.GetString(memoryStream.ToArray()) == Encoding.ASCII.GetString(memoryStream2.ToArray()));
				}
			}
			return result;
		}

		// Token: 0x04000040 RID: 64
		private static readonly Devices _default = new Devices();

		// Token: 0x04000041 RID: 65
		[DataMember(EmitDefaultValue = false)]
		public Dictionary<uint, ComPort> ComPorts;

		// Token: 0x04000042 RID: 66
		[DataMember(EmitDefaultValue = false)]
		public VirtioSerial VirtioSerial;

		// Token: 0x04000043 RID: 67
		[DataMember(EmitDefaultValue = false)]
		public Dictionary<string, Scsi> Scsi;

		// Token: 0x04000044 RID: 68
		[DataMember(EmitDefaultValue = false)]
		public VirtualPMemController VirtualPMem;

		// Token: 0x04000045 RID: 69
		[DataMember(EmitDefaultValue = false)]
		public Dictionary<string, NetworkAdapter> NetworkAdapters;

		// Token: 0x04000046 RID: 70
		[DataMember(EmitDefaultValue = false)]
		public VideoMonitor VideoMonitor;

		// Token: 0x04000047 RID: 71
		[DataMember(EmitDefaultValue = false)]
		public Keyboard Keyboard;

		// Token: 0x04000048 RID: 72
		[DataMember(EmitDefaultValue = false)]
		public Mouse Mouse;

		// Token: 0x04000049 RID: 73
		[DataMember(EmitDefaultValue = false)]
		public HvSocket HvSocket;

		// Token: 0x0400004A RID: 74
		[DataMember(EmitDefaultValue = false)]
		public EnhancedModeVideo EnhancedModeVideo;

		// Token: 0x0400004B RID: 75
		[DataMember(EmitDefaultValue = false)]
		public GuestCrashReporting GuestCrashReporting;

		// Token: 0x0400004C RID: 76
		[DataMember(EmitDefaultValue = false)]
		public VirtualSmb VirtualSmb;

		// Token: 0x0400004D RID: 77
		[DataMember(EmitDefaultValue = false)]
		public Plan9 Plan9;

		// Token: 0x0400004E RID: 78
		[DataMember(EmitDefaultValue = false)]
		public Licensing Licensing;

		// Token: 0x0400004F RID: 79
		[DataMember(EmitDefaultValue = false)]
		public Battery Battery;

		// Token: 0x04000050 RID: 80
		[DataMember(EmitDefaultValue = false)]
		public Dictionary<string, FlexibleIoDevice> FlexibleIov;

		// Token: 0x04000051 RID: 81
		[DataMember(EmitDefaultValue = false)]
		public SharedMemoryConfiguration SharedMemory;

		// Token: 0x04000052 RID: 82
		[DataMember(EmitDefaultValue = false)]
		public KernelIntegration KernelIntegration;
	}
}
