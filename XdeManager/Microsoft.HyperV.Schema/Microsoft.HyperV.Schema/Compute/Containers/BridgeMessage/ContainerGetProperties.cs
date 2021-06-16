using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Compute.Containers.BridgeMessage
{
	// Token: 0x020001B6 RID: 438
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ContainerGetProperties
	{
		// Token: 0x06000717 RID: 1815 RVA: 0x000165E4 File Offset: 0x000147E4
		public static bool IsJsonDefault(ContainerGetProperties val)
		{
			return ContainerGetProperties._default.JsonEquals(val);
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x000165F4 File Offset: 0x000147F4
		public bool JsonEquals(object obj)
		{
			ContainerGetProperties graph = obj as ContainerGetProperties;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ContainerGetProperties), new DataContractJsonSerializerSettings
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

		// Token: 0x040009B5 RID: 2485
		private static readonly ContainerGetProperties _default = new ContainerGetProperties();

		// Token: 0x040009B6 RID: 2486
		[DataMember]
		public string ContainerId;

		// Token: 0x040009B7 RID: 2487
		[DataMember]
		public Guid ActivityId;

		// Token: 0x040009B8 RID: 2488
		[DataMember]
		public string Query;
	}
}
