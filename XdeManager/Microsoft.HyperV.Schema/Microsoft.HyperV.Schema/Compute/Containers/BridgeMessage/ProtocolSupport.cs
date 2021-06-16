using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Compute.Containers.BridgeMessage
{
	// Token: 0x020001AB RID: 427
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ProtocolSupport
	{
		// Token: 0x060006E1 RID: 1761 RVA: 0x00015BF8 File Offset: 0x00013DF8
		public static bool IsJsonDefault(ProtocolSupport val)
		{
			return ProtocolSupport._default.JsonEquals(val);
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00015C08 File Offset: 0x00013E08
		public bool JsonEquals(object obj)
		{
			ProtocolSupport graph = obj as ProtocolSupport;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ProtocolSupport), new DataContractJsonSerializerSettings
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

		// Token: 0x0400097D RID: 2429
		private static readonly ProtocolSupport _default = new ProtocolSupport();

		// Token: 0x0400097E RID: 2430
		[DataMember(EmitDefaultValue = false)]
		public string MinimumVersion;

		// Token: 0x0400097F RID: 2431
		[DataMember(EmitDefaultValue = false)]
		public string MaximumVersion;

		// Token: 0x04000980 RID: 2432
		[DataMember]
		public uint MinimumProtocolVersion;

		// Token: 0x04000981 RID: 2433
		[DataMember]
		public uint MaximumProtocolVersion;
	}
}
