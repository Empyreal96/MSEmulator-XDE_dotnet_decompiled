using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Vpci
{
	// Token: 0x02000111 RID: 273
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class FlexIoDeviceSaveState
	{
		// Token: 0x06000459 RID: 1113 RVA: 0x0000E6EC File Offset: 0x0000C8EC
		public static bool IsJsonDefault(FlexIoDeviceSaveState val)
		{
			return FlexIoDeviceSaveState._default.JsonEquals(val);
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0000E6FC File Offset: 0x0000C8FC
		public bool JsonEquals(object obj)
		{
			FlexIoDeviceSaveState graph = obj as FlexIoDeviceSaveState;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(FlexIoDeviceSaveState), new DataContractJsonSerializerSettings
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

		// Token: 0x04000566 RID: 1382
		private static readonly FlexIoDeviceSaveState _default = new FlexIoDeviceSaveState();

		// Token: 0x04000567 RID: 1383
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x04000568 RID: 1384
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;

		// Token: 0x04000569 RID: 1385
		[DataMember(EmitDefaultValue = false)]
		public byte[] EmulatorState;

		// Token: 0x0400056A RID: 1386
		[DataMember(IsRequired = true)]
		public ulong EmulatorStateSize;

		// Token: 0x0400056B RID: 1387
		[DataMember(IsRequired = true)]
		public ulong EmulatorStateVersion;

		// Token: 0x0400056C RID: 1388
		[DataMember(EmitDefaultValue = false)]
		public AllocatedDeviceInfo[] AllocatedDevices;
	}
}
