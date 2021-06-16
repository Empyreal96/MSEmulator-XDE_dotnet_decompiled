using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Compute.Containers.BridgeMessage
{
	// Token: 0x020001BF RID: 447
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ContainerGetPropertiesResponse
	{
		// Token: 0x0600073F RID: 1855 RVA: 0x00016D80 File Offset: 0x00014F80
		public static bool IsJsonDefault(ContainerGetPropertiesResponse val)
		{
			return ContainerGetPropertiesResponse._default.JsonEquals(val);
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x00016D90 File Offset: 0x00014F90
		public bool JsonEquals(object obj)
		{
			ContainerGetPropertiesResponse graph = obj as ContainerGetPropertiesResponse;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ContainerGetPropertiesResponse), new DataContractJsonSerializerSettings
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

		// Token: 0x040009E5 RID: 2533
		private static readonly ContainerGetPropertiesResponse _default = new ContainerGetPropertiesResponse();

		// Token: 0x040009E6 RID: 2534
		[DataMember]
		public long Result;

		// Token: 0x040009E7 RID: 2535
		[DataMember]
		public Guid ActivityId;

		// Token: 0x040009E8 RID: 2536
		[DataMember(EmitDefaultValue = false)]
		public ErrorRecord[] ErrorRecords;

		// Token: 0x040009E9 RID: 2537
		[DataMember]
		public string Properties;
	}
}
