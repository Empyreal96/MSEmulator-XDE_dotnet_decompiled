using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers
{
	// Token: 0x02000182 RID: 386
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class NetworkEndpoint
	{
		// Token: 0x06000629 RID: 1577 RVA: 0x00013B1B File Offset: 0x00011D1B
		public static bool IsJsonDefault(NetworkEndpoint val)
		{
			return NetworkEndpoint._default.JsonEquals(val);
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x00013B28 File Offset: 0x00011D28
		public bool JsonEquals(object obj)
		{
			NetworkEndpoint graph = obj as NetworkEndpoint;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(NetworkEndpoint), new DataContractJsonSerializerSettings
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

		// Token: 0x04000832 RID: 2098
		private static readonly NetworkEndpoint _default = new NetworkEndpoint();

		// Token: 0x04000833 RID: 2099
		[DataMember(IsRequired = true)]
		public Guid Id;

		// Token: 0x04000834 RID: 2100
		[DataMember(EmitDefaultValue = false)]
		public string EndpointName;

		// Token: 0x04000835 RID: 2101
		[DataMember(EmitDefaultValue = false)]
		public string StaticMacAddress;

		// Token: 0x04000836 RID: 2102
		[DataMember(EmitDefaultValue = false)]
		public string StaticIPAddress;

		// Token: 0x04000837 RID: 2103
		[DataMember(IsRequired = true)]
		public Guid NetworkId;

		// Token: 0x04000838 RID: 2104
		[DataMember(EmitDefaultValue = false)]
		public string[] Policies;
	}
}
