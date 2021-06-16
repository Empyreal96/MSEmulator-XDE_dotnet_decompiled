using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Responses.System
{
	// Token: 0x0200005D RID: 93
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class GuestConnectionInfo
	{
		// Token: 0x0600016F RID: 367 RVA: 0x0000610C File Offset: 0x0000430C
		public static bool IsJsonDefault(GuestConnectionInfo val)
		{
			return GuestConnectionInfo._default.JsonEquals(val);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000611C File Offset: 0x0000431C
		public bool JsonEquals(object obj)
		{
			GuestConnectionInfo graph = obj as GuestConnectionInfo;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(GuestConnectionInfo), new DataContractJsonSerializerSettings
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

		// Token: 0x040001E3 RID: 483
		private static readonly GuestConnectionInfo _default = new GuestConnectionInfo();

		// Token: 0x040001E4 RID: 484
		[DataMember(EmitDefaultValue = false)]
		public Version[] SupportedSchemaVersions;

		// Token: 0x040001E5 RID: 485
		[DataMember(EmitDefaultValue = false)]
		public uint ProtocolVersion;

		// Token: 0x040001E6 RID: 486
		[DataMember(EmitDefaultValue = false)]
		public object GuestDefinedCapabilities;
	}
}
