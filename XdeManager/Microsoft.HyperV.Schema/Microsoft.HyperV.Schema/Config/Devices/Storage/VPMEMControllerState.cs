using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x0200013B RID: 315
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VPMEMControllerState
	{
		// Token: 0x060004FD RID: 1277 RVA: 0x00010497 File Offset: 0x0000E697
		public static bool IsJsonDefault(VPMEMControllerState val)
		{
			return VPMEMControllerState._default.JsonEquals(val);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x000104A4 File Offset: 0x0000E6A4
		public bool JsonEquals(object obj)
		{
			VPMEMControllerState graph = obj as VPMEMControllerState;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VPMEMControllerState), new DataContractJsonSerializerSettings
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

		// Token: 0x0400066B RID: 1643
		private static readonly VPMEMControllerState _default = new VPMEMControllerState();

		// Token: 0x0400066C RID: 1644
		[DataMember]
		public ulong ACPIBufferGPA;

		// Token: 0x0400066D RID: 1645
		[DataMember]
		public uint[] DeviceEventRegisters;

		// Token: 0x0400066E RID: 1646
		[DataMember]
		public Dictionary<uint, VPMEMDeviceState> Devices;

		// Token: 0x0400066F RID: 1647
		[DataMember]
		public bool ARSScrubInitiated;

		// Token: 0x04000670 RID: 1648
		[DataMember]
		public ulong ARSStartAddress;

		// Token: 0x04000671 RID: 1649
		[DataMember]
		public ulong ARSLength;
	}
}
