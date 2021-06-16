using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Compute.Containers.BridgeMessage
{
	// Token: 0x020001BC RID: 444
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ContainerCreateResponse
	{
		// Token: 0x06000733 RID: 1843 RVA: 0x00016B1E File Offset: 0x00014D1E
		public static bool IsJsonDefault(ContainerCreateResponse val)
		{
			return ContainerCreateResponse._default.JsonEquals(val);
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00016B2C File Offset: 0x00014D2C
		public bool JsonEquals(object obj)
		{
			ContainerCreateResponse graph = obj as ContainerCreateResponse;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ContainerCreateResponse), new DataContractJsonSerializerSettings
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

		// Token: 0x040009D5 RID: 2517
		private static readonly ContainerCreateResponse _default = new ContainerCreateResponse();

		// Token: 0x040009D6 RID: 2518
		[DataMember]
		public long Result;

		// Token: 0x040009D7 RID: 2519
		[DataMember]
		public Guid ActivityId;

		// Token: 0x040009D8 RID: 2520
		[DataMember(EmitDefaultValue = false)]
		public ErrorRecord[] ErrorRecords;

		// Token: 0x040009D9 RID: 2521
		[DataMember(EmitDefaultValue = false)]
		public string SelectedVersion;

		// Token: 0x040009DA RID: 2522
		[DataMember]
		public uint SelectedProtocolVersion;
	}
}
