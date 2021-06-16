using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Video
{
	// Token: 0x02000117 RID: 279
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class RdpEncoder
	{
		// Token: 0x06000471 RID: 1137 RVA: 0x0000EB2C File Offset: 0x0000CD2C
		public static bool IsJsonDefault(RdpEncoder val)
		{
			return RdpEncoder._default.JsonEquals(val);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0000EB3C File Offset: 0x0000CD3C
		public bool JsonEquals(object obj)
		{
			RdpEncoder graph = obj as RdpEncoder;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(RdpEncoder), new DataContractJsonSerializerSettings
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

		// Token: 0x04000589 RID: 1417
		private static readonly RdpEncoder _default = new RdpEncoder();

		// Token: 0x0400058A RID: 1418
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x0400058B RID: 1419
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;

		// Token: 0x0400058C RID: 1420
		[DataMember(EmitDefaultValue = false)]
		public string PipeServerName;

		// Token: 0x0400058D RID: 1421
		[DataMember(EmitDefaultValue = false)]
		public string[] RdpAccessSids;
	}
}
