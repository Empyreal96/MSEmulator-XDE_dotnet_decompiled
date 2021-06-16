using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Rdp
{
	// Token: 0x02000142 RID: 322
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SyntheticRdp
	{
		// Token: 0x06000517 RID: 1303 RVA: 0x000109A0 File Offset: 0x0000EBA0
		public static bool IsJsonDefault(SyntheticRdp val)
		{
			return SyntheticRdp._default.JsonEquals(val);
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x000109B0 File Offset: 0x0000EBB0
		public bool JsonEquals(object obj)
		{
			SyntheticRdp graph = obj as SyntheticRdp;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SyntheticRdp), new DataContractJsonSerializerSettings
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

		// Token: 0x0400068D RID: 1677
		private static readonly SyntheticRdp _default = new SyntheticRdp();

		// Token: 0x0400068E RID: 1678
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x0400068F RID: 1679
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;

		// Token: 0x04000690 RID: 1680
		[DataMember(EmitDefaultValue = false)]
		public bool EnableVailMode;

		// Token: 0x04000691 RID: 1681
		[DataMember(EmitDefaultValue = false)]
		public string PipeServerName;

		// Token: 0x04000692 RID: 1682
		[DataMember(EmitDefaultValue = false)]
		public string[] RdpAccessSids;

		// Token: 0x04000693 RID: 1683
		[DataMember(EmitDefaultValue = false, Name = "TransportType")]
		public TransportType TransportType;
	}
}
