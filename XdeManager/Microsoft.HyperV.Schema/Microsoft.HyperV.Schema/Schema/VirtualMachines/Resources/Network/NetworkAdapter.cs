using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources.Network
{
	// Token: 0x02000030 RID: 48
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class NetworkAdapter
	{
		// Token: 0x060000CB RID: 203 RVA: 0x000043EF File Offset: 0x000025EF
		public static bool IsJsonDefault(NetworkAdapter val)
		{
			return NetworkAdapter._default.JsonEquals(val);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000043FC File Offset: 0x000025FC
		public bool JsonEquals(object obj)
		{
			NetworkAdapter graph = obj as NetworkAdapter;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(NetworkAdapter), new DataContractJsonSerializerSettings
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

		// Token: 0x040000D4 RID: 212
		private static readonly NetworkAdapter _default = new NetworkAdapter();

		// Token: 0x040000D5 RID: 213
		[DataMember]
		public Guid EndpointId;

		// Token: 0x040000D6 RID: 214
		[DataMember(EmitDefaultValue = false)]
		public string MacAddress;

		// Token: 0x040000D7 RID: 215
		[DataMember(EmitDefaultValue = false)]
		public uint MediaType;
	}
}
