using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x0200013A RID: 314
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VPMEMDeviceState
	{
		// Token: 0x060004F7 RID: 1271 RVA: 0x000103AF File Offset: 0x0000E5AF
		public static bool IsJsonDefault(VPMEMDeviceState val)
		{
			return VPMEMDeviceState._default.JsonEquals(val);
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x000103BC File Offset: 0x0000E5BC
		public bool JsonEquals(object obj)
		{
			VPMEMDeviceState graph = obj as VPMEMDeviceState;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VPMEMDeviceState), new DataContractJsonSerializerSettings
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

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x00010464 File Offset: 0x0000E664
		// (set) Token: 0x060004FA RID: 1274 RVA: 0x0001046C File Offset: 0x0000E66C
		[DataMember(Name = "InjectedErrorsState")]
		private VPMEMInjectedErrorState _InjectedErrorsState
		{
			get
			{
				return this.InjectedErrorsState;
			}
			set
			{
				if (value != null)
				{
					this.InjectedErrorsState = value;
				}
			}
		}

		// Token: 0x04000663 RID: 1635
		private static readonly VPMEMDeviceState _default = new VPMEMDeviceState();

		// Token: 0x04000664 RID: 1636
		[DataMember]
		public ulong BaseAddress;

		// Token: 0x04000665 RID: 1637
		[DataMember]
		public byte VirtualNumaNode;

		// Token: 0x04000666 RID: 1638
		[DataMember]
		public byte[] PersistenceToken;

		// Token: 0x04000667 RID: 1639
		public VPMEMInjectedErrorState InjectedErrorsState = new VPMEMInjectedErrorState();

		// Token: 0x04000668 RID: 1640
		[DataMember]
		public List<VPMEMArsErrorRange> ErrorRanges;

		// Token: 0x04000669 RID: 1641
		[DataMember]
		public VPMEMBackingStoreMetadata VpmemMetadata;

		// Token: 0x0400066A RID: 1642
		[DataMember]
		public Dictionary<ulong, byte[]> PersistenceTokenMap;
	}
}
